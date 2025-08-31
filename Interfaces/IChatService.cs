using ArithmeticChat.Domain;
using ArithmeticChat.DTOs;

namespace ArithmeticChat.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDto> EvaluateAsync(int userId, ChatRequestDto dto, CancellationToken ct);
        Task<IEnumerable<ChatHistoryDto>> GetHistoryAsync(int userId, int page, int pageSize, CancellationToken ct);
    }
}
