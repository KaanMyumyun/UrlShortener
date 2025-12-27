using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// this line tells the app how to connect and what provider to use for the database
// it sets the rules not the database itself
// injects ApplicationDbContext
// tells entity framework to use PostgreSQL
// provides address of the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// we register our service so it can be injected
builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // apply migrations automatically when app starts
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

// endpoint that creates the short url
app.MapPost("/api/shorten", async (
    ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    ApplicationDbContext dbContext,
    HttpContext httpsContext) =>
{
    // we check if the url is valid if not we return bad request
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified Url is invalid");
    }

    // generates a random short code and checks if it is unique
    var code = await urlShorteningService.GenerateUniqueCode();

    // this creates a ShortenUrl entity which represents a row in the database
    var shortenedUrl = new ShortenUrl
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpsContext.Request.Scheme}://{httpsContext.Request.Host}/api/{code}",
        CreatedOnUtc = DateTime.UtcNow
    };

    // mark entity to be saved
    dbContext.ShortenUrls.Add(shortenedUrl);

    // actually write to db
    await dbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});

app.Run();
