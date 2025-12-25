// models are created to organize data so with out it will return lose json random stuff
public class ShortenUrlRequest
{
    // in this case we will return only the shorten url that will be created in entity folder shortenUrl.cs
    public string Url { get; set; }= string.Empty;
}