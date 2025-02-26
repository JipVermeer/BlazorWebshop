using Webshop.Models.Entities;

namespace Webshop.Services
{
    public interface IUserService
    {
        Task AddUserAsync(User user);

        Task<bool> EmailExists(string email);
    }
}
