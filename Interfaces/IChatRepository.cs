using ArithmeticChat.Domain;

namespace ArithmeticChat.Interfaces
{
    public interface IChatRepository
    {
        Task<int> AddAsync(ChatMessage msg, CancellationToken ct);
        Task<IEnumerable<ChatMessage>> GetByUserAsync(int userId, int page, int pageSize, CancellationToken ct);
    }
}
