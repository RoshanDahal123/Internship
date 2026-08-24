using formApi.FormApp.Application.DTOs;
using formApi.FormApp.Application.Common;


namespace formApi.FormApp.Application.Interfaces;

// The use-case boundary the API layer talks to. Controllers should only
// ever call this — never the repository or file storage directly.
public interface IFormEntryService
{
    Task<PagedResult<FormEntryDto>> GetAllAsync(PaginationParams pagination, string baseUrl);
    Task<FormEntryDto?> GetByIdAsync(int id, string baseUrl);
    Task<FormEntryDto> CreateAsync(CreateFormEntryDto dto, string baseUrl);
    Task<FormEntryDto> UpdateAsync(int id, UpdateFormEntryDto dto, string baseUrl);
    Task<bool> DeleteAsync(int id);
    Task<int> DeleteAllAsync();
}

