using formApi.FormApp.Application.DTOs;
using formApi.FormApp.Application.Exceptions;
using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Application.Common;
using formApi.FormApp.Domain.Entities;
using System.Text.Json;

namespace formApi.FormApp.Application.Services;

public class FormEntryService : IFormEntryService
{
    private static readonly string[] AllowedCvExtensions = { ".pdf", ".doc", ".docx" };
    private const long MaxCvFileSizeBytes = 5 * 1024 * 1024; // 5MB

    private readonly IFormEntryRepository _repository;
    private readonly IFileStorageService _fileStorageService;

    public FormEntryService(IFormEntryRepository repository, IFileStorageService fileStorageService)
    {
        _repository = repository;
        _fileStorageService = fileStorageService;
    }

    public async Task<PagedResult<FormEntryDto>> GetAllAsync(PaginationParams pagination, string baseUrl)
    {
        var pagedEntities = await _repository.GetPagedAsync(pagination.Page, pagination.PageSize);

        return new PagedResult<FormEntryDto>
        {
            Items = pagedEntities.Items.Select(u => MapToDto(u, baseUrl)).ToList(),
            Page = pagedEntities.Page,
            PageSize = pagedEntities.PageSize,
            TotalCount = pagedEntities.TotalCount
        };
    }

    public async Task<FormEntryDto?> GetByIdAsync(int id, string baseUrl)
    {
        var entry = await _repository.GetByIdAsync(id);
        return entry is null ? null : MapToDto(entry, baseUrl);
    }

    public async Task<FormEntryDto> CreateAsync(CreateFormEntryDto dto, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
            throw new AppValidationException("Name and Email are required.");

        List<EducationDto> educationList;
        try
        {
            educationList = string.IsNullOrEmpty(dto.EducationJson)
                ? new List<EducationDto>()
                : JsonSerializer.Deserialize<List<EducationDto>>(dto.EducationJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EducationDto>();
        }
        catch (JsonException)
        {
            throw new AppValidationException("Invalid education data format. Please provide valid JSON.");
        }

        string? savedCvPath = null;
        if (dto.CvFile is not null && dto.CvFile.Length > 0)
        {
            var extension = Path.GetExtension(dto.CvFile.FileName).ToLowerInvariant();

            if (!AllowedCvExtensions.Contains(extension))
                throw new AppValidationException("Only PDF or Word documents are allowed for CV upload.");

            if (dto.CvFile.Length > MaxCvFileSizeBytes)
                throw new AppValidationException("CV file must be under 5MB.");

            savedCvPath = await _fileStorageService.SaveAsync(dto.CvFile);
        }

        var entity = new UserEntry
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age,
            Address = dto.Address,
            Description = dto.Description,
            DateOfBirth = dto.DateOfBirth,
            CvFilePath = savedCvPath,
            Education = educationList.Select(e => new Education
            {
                Degree = e.Degree,
                InstitutionName = e.InstitutionName,
                Year = e.Year
            }).ToList()
        };

        await _repository.AddAsync(entity);

        return MapToDto(entity, baseUrl);
    }

    public async Task<FormEntryDto> UpdateAsync(int id, UpdateFormEntryDto dto, string baseUrl)
    {
        // 1. Fetch the existing entity from the database
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            throw new AppValidationException($"Form entry with ID {id} was not found.");

        // 2. Validate basic input fields
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
            throw new AppValidationException("Name and Email are required.");

        // 3. Process Education JSON
        List<EducationDto> educationList;
        try
        {
            educationList = string.IsNullOrEmpty(dto.EducationJson)
                ? new List<EducationDto>()
                : JsonSerializer.Deserialize<List<EducationDto>>(dto.EducationJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EducationDto>();
        }
        catch (JsonException)
        {
            throw new AppValidationException("Invalid education data format. Please provide valid JSON.");
        }

        // 4. Handle CV File Replacement (if a new file is uploaded)
        if (dto.CvFile is not null && dto.CvFile.Length > 0)
        {
            var extension = Path.GetExtension(dto.CvFile.FileName).ToLowerInvariant();

            if (!AllowedCvExtensions.Contains(extension))
                throw new AppValidationException("Only PDF or Word documents are allowed for CV upload.");

            if (dto.CvFile.Length > MaxCvFileSizeBytes)
                throw new AppValidationException("CV file must be under 5MB.");

            // Optionally delete the old CV file if one exists
            if (!string.IsNullOrEmpty(entity.CvFilePath))
            {
                await _fileStorageService.DeleteAsync(entity.CvFilePath);
            }

            // Save the new file
            entity.CvFilePath = await _fileStorageService.SaveAsync(dto.CvFile);
        }

        // 5. Update Entity Properties
        entity.Name = dto.Name;
        entity.Email = dto.Email;
        entity.Age = dto.Age;
        entity.Address = dto.Address;
        entity.Description = dto.Description;
        entity.DateOfBirth = dto.DateOfBirth
     ?? throw new AppValidationException("Date of birth is required.");

        // 6. Update Child Education Collection
        entity.Education.Clear();
        foreach (var edu in educationList)
        {
            entity.Education.Add(new Education
            {
                Degree = edu.Degree,
                InstitutionName = edu.InstitutionName,
                Year = edu.Year
            });
        }

        // 7. Persist and return DTO
        await _repository.UpdateAsync(entity);

        return MapToDto(entity, baseUrl);
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

    public Task<int> DeleteAllAsync() => _repository.DeleteAllAsync();

    private static FormEntryDto MapToDto(UserEntry entity, string baseUrl)
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
            CvFileUrl = entity.CvFilePath is not null
                ? $"{baseUrl.TrimEnd('/')}{entity.CvFilePath}"
                : null,
            Education = entity.Education.Select(e => new EducationDto
            {
                Degree = e.Degree,
                InstitutionName = e.InstitutionName,
                Year = e.Year
            }).ToList()
        };
    }
}
