using PharmaFlow.DTOs.Auth;
using PharmaFlow.Models;
using PharmaFlow.Repositories.Interfaces;
using PharmaFlow.Services.Interfaces;
using PharmaFlow.Helpers;

namespace PharmaFlow.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IAuthRepository repo, JwtHelper jwtHelper)
        {
            _repo = repo;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
                Role = "User"
            };

            var result = await _repo.Register(user, dto.Password);

            if (!result)
                throw new Exception("Registration failed");

            return "User registered successfully";
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _repo.GetUserByEmail(dto.Email);

            if (user == null)
                throw new Exception("User not found");

            var valid = await _repo.CheckPassword(user, dto.Password);

            if (!valid)
                throw new Exception("Invalid credentials");

            return _jwtHelper.GenerateToken(user);
        }
    }
}