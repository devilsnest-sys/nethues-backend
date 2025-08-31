using ArithmeticChat.Interfaces;
using ArithmeticChat.DTOs;
using ArithmeticChat.Domain;
using ArithmeticChat.Helpers;
namespace ArithmeticChat.Services;
public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepo;
    private readonly ExpressionEvaluator _evaluator;
    private readonly INumberToWordsHelper _words;
    public ChatService(IChatRepository chatRepo, ExpressionEvaluator evaluator, INumberToWordsHelper words)
    {
        _chatRepo = chatRepo; _evaluator = evaluator; _words = words;
    }

    public async Task<ChatResponseDto> EvaluateAsync(int userId, ChatRequestDto dto, CancellationToken ct)
    {
        var numeric = _evaluator.Evaluate(dto.Expression);
        var words = _words.Convert(numeric);
        var now = DateTime.UtcNow;
        var msg = new ChatMessage { UserId = userId, Expression = dto.Expression, NumericResult = numeric, WordsResult = words, CreatedAt = now };
        var id = await _chatRepo.AddAsync(msg, ct);
        return new ChatResponseDto { Expression = dto.Expression, Result = numeric, ResultInWords = words, CreatedAt = now };
    }

    public async Task<IEnumerable<ChatHistoryDto>> GetHistoryAsync(int userId, int page, int pageSize, CancellationToken ct)
    {
        var rows = await _chatRepo.GetByUserAsync(userId, page, pageSize, ct);
        return rows.Select(r => new ChatHistoryDto { Id = r.Id, Expression = r.Expression, NumericResult = r.NumericResult, WordsResult = r.WordsResult, CreatedAt = r.CreatedAt });
    }
}
