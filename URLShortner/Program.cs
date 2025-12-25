using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//this line tells the app how to connect and what provider to use for the sql
//server database
//it sets the rules not the database
//injects ApplicationDbContext
//tells entityframework to use sqlserver
//provides address of the Database
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()
    ));
builder.Services.AddScoped<UrlShorteningService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();    
}
// we get a request 
app.MapPost("api/shorten",async (
    ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    ApplicationDbContext dbContext,
    HttpContext httpContext)=>
{
    //we check if the url is valid if not we return badrequest
    if(!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified Url is invalid");
    }

    //generates a random short code check if its uniques uses await because database is slow
    var code = await urlShorteningService.GenerateUniqueCode();

//this creates a shortenurl entity whitch represents a row in the database
    var shortenedUrl = new ShortenUrl
    {
      // the primary key
      Id = Guid.NewGuid(),
      //the original url
      LongUrl = request.Url,
      //unique short code
      Code = code,
      // full shortened link
      ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
      // timestamp of creation
      CreatedOnUtc = DateTime.Now
    };
    // marks entity to be saved
    dbContext.ShortenUrls.Add(shortenedUrl);
    // actually write to db nothing is saved until savechangesasync is done
    await dbContext.SaveChangesAsync();
    //return if everything is ok
    return Results.Ok(shortenedUrl.ShortUrl);
});

app.UseHttpsRedirection();
app.Run();


