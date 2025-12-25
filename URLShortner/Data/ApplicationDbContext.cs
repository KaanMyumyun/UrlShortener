using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options):base(options)
    {
        
    }
    
    public DbSet<ShortenUrl> ShortenUrls{get;set;}

//overide the method so can port the data base how we want
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //this is the class that has the rules the method gonna use for the mapping of the database 
        // like columns indexes Constraints and Relationships between the data bases
        modelBuilder.Entity<ShortenUrl>(builder =>
        {
            //to get more performance we set max length so there isnt confusions 
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);

            // this insures the key we create is unique by the database
            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}

//we are gonna port our database with this

