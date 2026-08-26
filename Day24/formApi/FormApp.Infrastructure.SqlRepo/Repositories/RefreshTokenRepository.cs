using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Domain.Entities;
using formApi.FormApp.Infrastructure.SqlRepo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace formApi.FormApp.Infrastructure.SqlRepo.Repositories
{
    public class RefreshTokenRepository:IRefreshTokenRepository
    {

        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }


        public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash) =>
            _context.RefreshTokens
               .Include(t => t.AdminUser)
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();


    }
}
