using formApi.Common;
using formApi.Data;
using formApi.DTOs;
using formApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace formApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentsController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<FormEntryDto>>> GetAll([FromQuery]PaginationParams pagination)
        {
            // Step 1 — paginate at the ENTITY level, before any Request-dependent mapping.
            var pagedEntities = await _context.UserEntries
                .Include(u => u.Education)
                .OrderBy(u=>u.Id)
                .ToPagedResultAsync(pagination.Page, pagination.PageSize);
            // Step 2 — now that data is materialized in memory (List<UserEntry>),

            var dtoItems= pagedEntities.Items.Select(u => MapToDto(u)).ToList();

            var result = new PagedResult<FormEntryDto>
            {
                Items = dtoItems,
                Page = pagedEntities.Page,
                PageSize = pagedEntities.PageSize,
                TotalCount = pagedEntities.TotalCount
            };
            return Ok(result);//without this ,Education would come back
        }

        [HttpPost]
        [Consumes("multipart/form-data")] //tells swagger/clients this endpount excepts a file not a raw json object

        public async Task<ActionResult<FormEntryDto>>Create([FromForm] CreateFormEntryFormData dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Name and Email are required." });

            List<EducationDto> educationList;

            try
            {
                educationList= string.IsNullOrEmpty(dto.EducationJson)
                    ? new List<EducationDto>()
                    : JsonSerializer.Deserialize<List<EducationDto>>(dto.EducationJson,
                      new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EducationDto>();
            }
            catch (JsonException)
            {
                return BadRequest(new {message= "Invalid education data format. Please provide valid JSON." });
            }
            string? savedCvPath = null;
            if(dto.CvFile is not null && dto.CvFile.Length > 0)
            {
                // Basic validation — worth having on any file upload endpoint
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx"};

                var extension = Path.GetExtension(dto.CvFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new { message = "Only PDF or Word documents are allowed for CVUpload" });

                if (dto.CvFile.Length > 5 * 1024 * 1024)//5MB limit
                    return BadRequest(new { message = "CV file must be under 5MB." });

                var uploadsFolder= Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                if(!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Generate a unique filename — never trust/reuse the original filename directly,
                // two people uploading "resume.pdf" would otherwise overwrite each other's file

                var uniqueFileName=$"{Guid.NewGuid()}{extension}";

                var filePath= Path.Combine(uploadsFolder, uniqueFileName);

                using(var stream = new FileStream(filePath,FileMode.Create))
                {
                    await dto.CvFile.CopyToAsync(stream);
                }
                savedCvPath = $"/uploads/{uniqueFileName}";
            }
            
            
            var entity = new UserEntry
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                Address = dto.Address,
                Description = dto.Description,
                DateOfBirth=dto.DateOfBirth,
                CvFilePath = savedCvPath,
                Education = educationList.Select(e => new Education
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

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.UserEntries.FindAsync(id);
            if (entry == null)
            {
                return NotFound(new { message = $"Entry with id {id} not found" });
            }
            _context.UserEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            var hasEntries = await _context.UserEntries.AnyAsync(); // properly awaited, returns bool

            if (!hasEntries)
            {
                return NotFound(new { message = "No entries found to delete." });
            }

            var deletedCount = await _context.UserEntries.ExecuteDeleteAsync();
            return Ok(new { message = $"Deleted {deletedCount} entries." });
        }


        private FormEntryDto MapToDto(UserEntry entity)
        {
            return new FormEntryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Age = entity.Age,
                Address = entity.Address,
                Description = entity.Description,
                DateOfBirth = entity.DateOfBirth,
                CvFileUrl=entity.CvFilePath is not null
                ? $"{Request.Scheme}://{Request.Host}{entity.CvFilePath}"
                :null,

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
