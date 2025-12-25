using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

// this is a helper method for for database migration when app starts
//like database doesnt exit create it update the databased
// only 1 instance runs once per application start up
public static class MigrationExtensions
{
    //this WebApplication app that makes the apply migration is a extension method 
    // si we can call it app.ApllyMigration
    public static void ApplyMigrations(this WebApplication app)
    {
        // a temp container so dbContext to exist when done release the memory 
        using var scoped = app.Services.CreateScope();
        //asks if this servise exist if not throw a erro
        // dbContext is pen after writing in the database it gets thrown away
        var dbContext = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // create the database if exist if not and update collums and tables if they arent matching the code
        dbContext.Database.Migrate();
    }
}