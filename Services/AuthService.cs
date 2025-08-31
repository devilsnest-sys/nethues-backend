using ArithmeticChat.Interfaces;
using ArithmeticChat.DTOs;
using ArithmeticChat.Helpers;
using ArithmeticChat.Domain;
namespace ArithmeticChat.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly JwtHelper _jwt;
    public AuthService(IUserRepository users, JwtHelper jwt) { _users = users; _jwt = jwt; }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        var user = await _users.GetByUsernameAsync(dto.Username, ct)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = _jwt.CreateToken(user.Id, user.Username);
        return new LoginResponseDto { Token = token, Username = user.Username };
    }

    public async Task<int> RegisterAsync(RegisterDto dto, CancellationToken ct)
    {
        var exists = await _users.GetByUsernameAsync(dto.Username, ct);
        if (exists != null) throw new ArgumentException("Username already exists");

        var u = new User { Username = dto.Username, PasswordHash = PasswordHasher.Hash(dto.Password), Email = dto.Email };
        return await _users.CreateAsync(u, ct);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(userId, ct) ?? throw new Exception("User not found");
        if (!PasswordHasher.Verify(dto.OldPassword, user.PasswordHash)) throw new UnauthorizedAccessException("Old password invalid");
        var newHash = PasswordHasher.Hash(dto.NewPassword);
        await _users.UpdatePasswordAsync(userId, newHash, ct);
    }
}
