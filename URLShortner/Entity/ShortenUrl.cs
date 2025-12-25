using System.Data;

// we use entity to make the data base structure
public class ShortenUrl
{
    public Guid Id{get; set;}
    public string LongUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedOnUtc{get;set;}
}