
using  formApi.FormApp.Domain.Entities;
namespace formApi.FormApp.Application.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<AdminUser?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);

        Task AddAsync(AdminUser user);

    }
}
