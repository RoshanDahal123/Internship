using formApi.FormApp.Application.Common;
using formApi.FormApp.Domain.Entities;

namespace formApi.FormApp.Application.Interfaces

{
    public interface IFormEntryRepository
    {
        Task<PagedResult<UserEntry>> GetPagedAsync(int page, int pageSize, string? search);
        Task<UserEntry?> GetByIdAsync(int id);
        Task AddAsync(UserEntry entity);
        Task UpdateAsync(UserEntry entity);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();

    }
}
