using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Mock;

public class DataBaseFactory : IDesignTimeDbContextFactory<DataBase>
{
    public DataBase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataBase>();

        // Get connection string from environment variable
        var pgConnStr = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        if (!string.IsNullOrWhiteSpace(pgConnStr))
        {
            // Use connection string from environment (Render, production, etc.)
            optionsBuilder.UseNpgsql(pgConnStr);
        }
        else
        {
            // Local development fallback — use environment variables locally too
            var localConnStr = Environment.GetEnvironmentVariable("LOCAL_PG_CONNECTION");

            if (string.IsNullOrWhiteSpace(localConnStr))
            {
                throw new InvalidOperationException("No PostgreSQL connection string found. Please set LOCAL_PG_CONNECTION.");
            }

            optionsBuilder.UseNpgsql(localConnStr);
        }

        return new DataBase(optionsBuilder.Options);
    }
}
