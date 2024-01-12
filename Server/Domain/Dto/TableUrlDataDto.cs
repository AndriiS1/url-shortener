namespace Domain.Dto
{
    public class TableUrlDataDto
    {
        public long? Id { get; set; } 
        public string? OriginalUrl { get; set; }
        public string? ShortUrl { get; set; }
        public bool? CanDelete { get; set; } = false;

    }
}
