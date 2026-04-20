using Microsoft.EntityFrameworkCore;
using URL.Services;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ShortenUrl> ShortenUrls { get; set; } = null!;

    // override the method so we can control how the database is mapped
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // this is the class that has the rules the method gonna use for the mapping of the database
        // like columns, indexes, constraints and relationships between the tables
        modelBuilder.Entity<ShortenUrl>(builder =>
        {
            builder.ToTable("ShortenUrls");
            // to get more performance we set max length so there isnt confusion
            builder
                .Property(s => s.Code)
                .HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink)
                .IsRequired();

            // this ensures the key we create is unique by the database
            builder
                .HasIndex(s => s.Code)
                .IsUnique();
        });
    }
}
