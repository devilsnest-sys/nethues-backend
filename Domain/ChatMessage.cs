namespace ArithmeticChat.Domain
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Expression { get; set; } = string.Empty;
        public decimal NumericResult { get; set; }
        public string WordsResult { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
