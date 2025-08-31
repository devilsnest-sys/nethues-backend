namespace ArithmeticChat.DTOs
{
    public class LoginDto { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty;  }
    public class RegisterDto { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; public string? Email { get; set; } }
    public class LoginResponseDto { public string Token { get; set; } = string.Empty; public string Username { get; set; } = string.Empty; public int Id { get; set; } }
    public class ChangePasswordDto { public string OldPassword { get; set; } = string.Empty; public string NewPassword { get; set; } = string.Empty; }
    public class ChatRequestDto { public string Expression { get; set; } = string.Empty; }
    public class ChatResponseDto { public string Expression { get; set; } = string.Empty; public decimal Result { get; set; } public string ResultInWords { get; set; } = string.Empty; public DateTime CreatedAt { get; set; } }
    public class ChatHistoryDto { public int Id { get; set; } public string Expression { get; set; } = string.Empty; public decimal NumericResult { get; set; } public string WordsResult { get; set; } = string.Empty; public DateTime CreatedAt { get; set; } }
}
