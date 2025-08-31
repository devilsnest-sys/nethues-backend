using ArithmeticChat.DTOs;

namespace ArithmeticChat.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken ct);
        Task<int> RegisterAsync(RegisterDto dto, CancellationToken ct);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken ct);
    }
}
