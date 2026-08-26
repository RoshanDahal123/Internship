using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Domain.Entities;
using formApi.FormApp.Infrastructure.SqlRepo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace formApi.FormApp.Infrastructure.SqlRepo.Repositories
{
    public class AdminUserRepository:IAdminUserRepository
    {
        private readonly AppDbContext _context;

        public AdminUserRepository(AppDbContext context)
        {
            _context = context;
        }


        public Task<AdminUser?> GetByEmailAsync(string email) =>
             _context.AdminUsers.FirstOrDefaultAsync(a => a.Email == email);

        public Task<bool> EmailExistsAsync(string email) =>
            _context.AdminUsers.AnyAsync(a => a.Email == email);

        public async Task AddAsync(AdminUser admin)
        {
            _context.AdminUsers.Add(admin);
            await _context.SaveChangesAsync();
        }

    }
}
