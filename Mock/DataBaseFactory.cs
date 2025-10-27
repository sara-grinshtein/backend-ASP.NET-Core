using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mock
{
    public class DataBaseFactory : IDesignTimeDbContextFactory<DataBase>
    {
        public DataBase CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBase>();

            var pgConnStr = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            if (!string.IsNullOrWhiteSpace(pgConnStr))
            {
                // Use PostgreSQL connection string from environment variables 
                // (used in production environments like Render)
                optionsBuilder.UseNpgsql(pgConnStr);
            }
            else
            {
                // Fallback for local development – use local PostgreSQL instead of SQL Server
                // This ensures EF will never attempt to connect to a SQL Server instance
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=yedidim_local;Username=postgres;Password=postgres;Ssl Mode=Disable;Trust Server Certificate=true;"
                );
            }

            return new DataBase(optionsBuilder.Options);
        }
    }


}
