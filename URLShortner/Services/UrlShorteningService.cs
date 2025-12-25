// we use ths class of creating the random link
using Microsoft.EntityFrameworkCore;

public class UrlShorteningService
{
    //we do 7 because it gives us billions of combinations 
    public const int NumberOfCharsInShortLink = 7;

    // this is the charecters used to make the link 
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnoprstuvwxyz0123456789";

    //we inject the data base to check if its unique or not 
    private readonly ApplicationDbContext _dbContext;
    // we do this so when we create this service you must give it an applicationDbContext
    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //to generate a random index 
    private readonly Random _random = new();

    //generating the random url
    // async makes this method might need to wait for something slow (like a database)and the app doesnt freeze while waiting
    public async Task<string> GenerateUniqueCode()
    {
        //we make char array that will be the random  text for the shorturl
        var codeChars = new char[NumberOfCharsInShortLink];

        while (true)
        {
            // we loop 7 times 
            for (var i = 0; i < NumberOfCharsInShortLink; i++)
            {
                //we get random index from 
                int randomIndex = _random.Next(Alphabet.Length - 1);
                // we assing the from aphabet the random index we generated
                codeChars[i] = Alphabet[randomIndex];
            }
            var code = new string(codeChars);

            if (!await _dbContext.ShortenUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }



    }
}