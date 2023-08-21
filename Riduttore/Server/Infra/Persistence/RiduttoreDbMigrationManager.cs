using Microsoft.EntityFrameworkCore; 
public static class RiduttoreDbMigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<RiduttoreDbContext>())
            {
                try
                {
                    appContext.Database.Migrate();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        return webApp;
    }
}