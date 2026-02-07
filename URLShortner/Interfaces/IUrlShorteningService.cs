public interface iUrlShorteningService
{
    Task<string> GenerateUniqueCode();
    Task<string> ShortenUrlRequest(ShortenUrlRequest dto, HttpContext httpsContext);
    Task<string> URlReturn(string code );
}