using PharmaFlow.DTOs.Auth;

namespace PharmaFlow.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);
    }
}