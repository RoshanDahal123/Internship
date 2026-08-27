using formApi.FormApp.Application.Common;
using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Domain.Entities;
using formApi.FormApp.Infrastructure.SqlRepo.Common;
using formApi.FormApp.Infrastructure.SqlRepo.Persistence;
using Microsoft.EntityFrameworkCore;
namespace formApi.FormApp.Infrastructure.SqlRepo.Repositories
{
    public class FormEntryRepository:IFormEntryRepository
    {

        private readonly AppDbContext _context;

        public FormEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<PagedResult<UserEntry>> GetPagedAsync(int page, int pageSize,string? search)
        {
            var query = _context.UserEntries
            .Include(u => u.Education)
             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u =>
                    u.Name.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term));
            }
            return query
          .OrderBy(u => u.Id)
          .ToPagedResultAsync(page, pageSize);
        }

        public Task<UserEntry?> GetByIdAsync(int id)
        {
            return _context.UserEntries
                .Include(u => u.Education)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(UserEntry entity)
        {
            _context.UserEntries.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(UserEntry entity)
        {
            _context.UserEntries.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.UserEntries.FindAsync(id);
            if (entry is null)
                return false;

            _context.UserEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<int> DeleteAllAsync()
        {
            return _context.UserEntries.ExecuteDeleteAsync();
        }
    }
}
