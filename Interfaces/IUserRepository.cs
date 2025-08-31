using ArithmeticChat.Domain;

namespace ArithmeticChat.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
        Task<User?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(User user, CancellationToken ct);
        Task UpdatePasswordAsync(int userId, string newPasswordHash, CancellationToken ct);
    }
}
