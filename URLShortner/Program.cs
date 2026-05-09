using Microsoft.EntityFrameworkCore;
using URL.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
            "https://localhost:5173",
            "http://localhost:3000",
            "https://localhost:3000",
            "https://urlshortener-az5.pages.dev/"
           
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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
//gr
// we register our service so it can be injected
builder.Services.AddScoped<iUrlShorteningService,UrlShorteningService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Helps Swagger find your routes
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("ReactPolicy");
app.MapControllers();
// endpoint that creates the short url
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
app.Run();
