using Microsoft.EntityFrameworkCore;

// this is a helper method for database migration when app starts
// like if database doesn't exist create it, update the database
// only 1 instance runs once per application start up
public static class MigrationExtensions
{
    // this WebApplication app that makes the apply migration is an extension method
    // so we can call it app.ApplyMigrations()
    public static void ApplyMigrations(this WebApplication app)
    {
        // a temp container so dbContext exists, when done release the memory
        using var scope = app.Services.CreateScope();

        // asks if this service exists, if not throw an error
        // dbContext is open only while writing to the database
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // create the database if it doesn't exist
        // update columns and tables if they aren't matching the code
        dbContext.Database.Migrate();
    }
}
