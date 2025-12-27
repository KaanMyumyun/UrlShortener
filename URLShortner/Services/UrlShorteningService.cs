using Microsoft.EntityFrameworkCore;

// we use this class for creating the random link
public class UrlShorteningService
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
}
