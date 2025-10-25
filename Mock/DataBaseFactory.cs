using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mock
{
    public class DataBaseFactory : IDesignTimeDbContextFactory<DataBase>
    {
        public DataBase CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBase>();

            // Try to get the PostgreSQL connection string from the environment variable
            // (used in Render or other production-like environments)
            var pgConnStr = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            if (!string.IsNullOrWhiteSpace(pgConnStr))
            {
                // PostgreSQL connection available → use it for migrations / production
                optionsBuilder.UseNpgsql(pgConnStr);
            }
            else
            {
                // No environment variable → fallback to local SQL Server for dev/testing
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=project_yedidim1;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
                );
            }

            return new DataBase(optionsBuilder.Options);
        }
    }
}
