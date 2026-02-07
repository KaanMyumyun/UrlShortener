using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// we use this class for creating the random link
public class UrlShorteningService:iUrlShorteningService
{
    // we do 7 because it gives us billions of combinations
    public const int NumberOfCharsInShortLink = 7;

    // this is the characters used to make the link
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnoprstuvwxyz0123456789";

    // we inject the database to check if it's unique or not
    private readonly ApplicationDbContext _dbContext;

    // we do this so when we create this service you must give it an ApplicationDbContext
    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // random generator
    private readonly Random _random = new();

    // generating the random url
    // async because database calls are slow
    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharsInShortLink];

        while (true)
        {
            // generate random code
            for (var i = 0; i < NumberOfCharsInShortLink; i++)
            {
                int randomIndex = _random.Next(Alphabet.Length);
                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);

            // check if the code already exists
            if (!await _dbContext.ShortenUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }  
    }

    public async Task<string> URlReturn(string code)
    {
        var Url = await _dbContext.ShortenUrls.FirstOrDefaultAsync(x => x.Code== code);
         return Url.LongUrl;  
    }

  // Modified to return the ShortUrl string
public async Task<string> ShortenUrlRequest(ShortenUrlRequest dto, HttpContext httpsContext)
{
    if (!Uri.TryCreate(dto.Url, UriKind.Absolute, out var uri) ||
        (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
    {
        throw new ArgumentException("Invalid URL.");
    }
    
    for (int attempt = 0; attempt < 5; attempt++)
    {
        var code = await GenerateUniqueCode();
        var shortenedUrl = new ShortenUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = dto.Url,
            Code = code,
            ShortUrl = $"{httpsContext.Request.Scheme}://{httpsContext.Request.Host}/{code}",
            CreatedOnUtc = DateTime.UtcNow
        };
        
        _dbContext.ShortenUrls.Add(shortenedUrl);
        
        try
        {
            await _dbContext.SaveChangesAsync();
            return shortenedUrl.ShortUrl; // Just return the ShortUrl you already created!
        }
        catch (DbUpdateException)
        {
            _dbContext.Entry(shortenedUrl).State = EntityState.Detached;
        }
    }
    
    throw new Exception("Could not generate short link. Try again.");
}
}
