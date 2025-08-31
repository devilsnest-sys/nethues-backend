using Dapper;
using ArithmeticChat.Domain;
using ArithmeticChat.Interfaces;
using ArithmeticChat.Helpers;
namespace ArithmeticChat.Repositories;
public class ChatRepository : IChatRepository
{
    private readonly DbConnectionFactory _factory;
    public ChatRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<int> AddAsync(ChatMessage msg, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
INSERT INTO ChatMessages (UserId, Expression, NumericResult, WordsResult)
VALUES (@UserId, @Expression, @NumericResult, @WordsResult);
SELECT CAST(SCOPE_IDENTITY() as int);";
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { msg.UserId, msg.Expression, msg.NumericResult, msg.WordsResult }, cancellationToken: ct));
    }

    public async Task<IEnumerable<ChatMessage>> GetByUserAsync(int userId, int page, int pageSize, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
SELECT * FROM ChatMessages WHERE UserId = @UserId
ORDER BY CreatedAt DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
        var offset = (page - 1) * pageSize;
        return await conn.QueryAsync<ChatMessage>(new CommandDefinition(sql, new { UserId = userId, Offset = offset, PageSize = pageSize }, cancellationToken: ct));
    }
}
