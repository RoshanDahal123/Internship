using formApi.Data;
using formApi.DTOs;
using formApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace formApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormEntriesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public FormEntriesController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<FormEntryDto>>> GetAll()
        {
            var entries = await _context.UserEntries
                .Include(u => u.Education)
                .Select(u => MapToDto(u))
                .ToListAsync();


                return Ok(entries);//without this ,Education would come back


        }



        public async Task<ActionResult<FormEntryDto>>Create(CreateFormEntryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Name and Email are required." });

            var entity = new UserEntry
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                Address = dto.Address,
                Description = dto.Description,
                Education = dto.Education.Select(e => new Education
                {
                    Degree = e.Degree,
                    InstitutionName = e.InstitutionName,
                    Year = e.Year
                }).ToList()
            };

            _context.UserEntries.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, MapToDto(entity));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FormEntryDto>> GetById(int id)
        {
            var entry= await _context.UserEntries
                .Include(u => u.Education)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(entry is null)
            {
                return NotFound(new { message = $"Entry with id{id}not found" });
            }
                return Ok(MapToDto(entry));
        }




        private static FormEntryDto MapToDto(UserEntry entity)
        {
            return new FormEntryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Age = entity.Age,
                Address = entity.Address,
                Description = entity.Description,
                Education = entity.Education.Select(e => new EducationDto
                {
                    Degree = e.Degree,
                    InstitutionName = e.InstitutionName,
                    Year = e.Year
                }).ToList()

            };
        }
    }
}
