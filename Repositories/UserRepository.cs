using Dapper;
using ArithmeticChat.Domain;
using ArithmeticChat.Interfaces;
using ArithmeticChat.Helpers;
namespace ArithmeticChat.Repositories;
public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _factory;
    public UserRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT TOP(1) * FROM Users WHERE Username = @Username";
        return await conn.QueryFirstOrDefaultAsync<User>(new CommandDefinition(sql, new { Username = username }, cancellationToken: ct));
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await conn.QueryFirstOrDefaultAsync<User>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
    }

    public async Task<int> CreateAsync(User user, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
INSERT INTO Users (Username, PasswordHash, Email, Role)
VALUES (@Username, @PasswordHash, @Email, @Role);
SELECT CAST(SCOPE_IDENTITY() as int);";
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, user, cancellationToken: ct));
    }

    public async Task UpdatePasswordAsync(int userId, string newPasswordHash, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();
        var sql = "UPDATE Users SET PasswordHash = @Hash WHERE Id = @Id";
        await conn.ExecuteAsync(new CommandDefinition(sql, new { Hash = newPasswordHash, Id = userId }, cancellationToken: ct));
    }
}
