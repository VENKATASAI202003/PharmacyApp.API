using PharmaFlow.Models;

namespace PharmaFlow.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> Register(User user, string password);
        Task<User> GetUserByEmail(string email);
        Task<bool> CheckPassword(User user, string password);
    }
}