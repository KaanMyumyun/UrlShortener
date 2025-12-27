using System.ComponentModel.DataAnnotations;

// this represents a row in the database
public class ShortenUrl
{
    // primary key
    public Guid Id { get; set; }

    // original long url
    [MaxLength(2048)]
    public string LongUrl { get; set; } = string.Empty;

    // unique short code
    public string Code { get; set; } = string.Empty;

    // full shortened url
    public string ShortUrl { get; set; } = string.Empty;

    // creation timestamp
    public DateTime CreatedOnUtc { get; set; }
}
