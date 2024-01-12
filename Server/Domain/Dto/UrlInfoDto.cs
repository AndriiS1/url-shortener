namespace Domain.Dto
{
    public class UrlInfoDto
    {
        public long? Id { get; set; }
        public string? OriginalUrl { get; set; }
        public string? ShortUrl { get; set; }
        public string? UserName { get; set; }
        public DateTime? Date { get; set; }
    }
}
