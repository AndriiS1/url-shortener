namespace Domain.Models
{
    public class Url
    {
        public long Id { get; set; }
        public DateTime? Date { get; set; }
        public string? OriginalUrl { get; set; }
        public string? ShortUrl { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
    }
}
