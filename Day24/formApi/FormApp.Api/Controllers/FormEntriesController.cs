using formApi.FormApp.Application.Common;
using formApi.FormApp.Application.DTOs;
using formApi.FormApp.Application.Exceptions;
using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace formApi.FormApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class StudentsController : ControllerBase
{
    private readonly IFormEntryService _formEntryService;

    public StudentsController(IFormEntryService formEntryService)
    {
        _formEntryService = formEntryService;
    }

    private string BaseUrl => $"{Request.Scheme}://{Request.Host}";


    [HttpGet]
    public async Task<ActionResult<PagedResult<FormEntryDto>>> GetAll([FromQuery] PaginationParams pagination)
    {
        var result = await _formEntryService.GetAllAsync(pagination, BaseUrl);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FormEntryDto>> GetById(int id)
    {
        var entry = await _formEntryService.GetByIdAsync(id, BaseUrl);
        if (entry is null)
            return NotFound(new { message = $"Entry with id {id} not found" });

        return Ok(entry);
    }
    [Authorize(Roles ="Admin")]
    [HttpPost]
    [Consumes("multipart/form-data")] // tells swagger/clients this endpoint expects a file, not raw json
    public async Task<ActionResult<FormEntryDto>> Create([FromForm] CreateFormEntryFormData form)
    {
        var dto = new CreateFormEntryDto
        {
            Name = form.Name,
            Email = form.Email,
            Age = form.Age,
            Address = form.Address,
            Description = form.Description,
            DateOfBirth = form.DateOfBirth,
            EducationJson = form.EducationJson,
            CvFile = form.CvFile is not null
                ? new FileUploadDto
                {
                    FileName = form.CvFile.FileName,
                    Length = form.CvFile.Length,
                    OpenStream = form.CvFile.OpenReadStream()
                }
                : null
        };

        try
        {
            var created = await _formEntryService.CreateAsync(dto, BaseUrl);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (AppValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:int}")]
    [Consumes("multipart/form-data")] // tells swagger/clients this endpoint expects a file, not raw json
    public async Task<ActionResult<FormEntryDto>> Update([FromForm] UpdateFormEntryDto form, int id)
    {
        var dto = new UpdateFormEntryDto
        {
            Id= id,
            Name = form.Name,
            Email = form.Email,
            Age = form.Age,
            Address = form.Address,
            Description = form.Description,
            DateOfBirth = form.DateOfBirth,
            EducationJson = form.EducationJson,
            CvFile = form.CvFile
        };

        try
        {
            var updated = await _formEntryService.UpdateAsync(id, dto, BaseUrl);
            return Ok(new { message = $"Updated entries." });
        }
        catch (AppValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _formEntryService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Entry with id {id} not found" });

        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAll()
    {
        var deletedCount = await _formEntryService.DeleteAllAsync();
        if (deletedCount == 0)
            return NotFound(new { message = "No entries found to delete." });

        return Ok(new { message = $"Deleted {deletedCount} entries." });
    }
}
