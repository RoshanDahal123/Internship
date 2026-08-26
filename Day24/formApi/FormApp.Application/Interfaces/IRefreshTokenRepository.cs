using formApi.FormApp.Domain.Entities;
namespace formApi.FormApp.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task SaveChangesAsync();

    }
}
