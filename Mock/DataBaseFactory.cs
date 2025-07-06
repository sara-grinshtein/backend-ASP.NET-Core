//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore;

//namespace Mock
//{
//    // DataBaseFactory.cs
//    public class DataBaseFactory : IDesignTimeDbContextFactory<DataBase>
//    {
//        public DataBase CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<DataBase>();
//            optionsBuilder.UseSqlServer("Server=localhost;Database=project_yedidim1;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;");
//            return new DataBase(optionsBuilder.Options);
//        }
//    }

//}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mock
{
    public class DataBaseFactory : IDesignTimeDbContextFactory<DataBase>
    {
        public DataBase CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBase>();
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=project_yedidim1;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
            );
            return new DataBase(optionsBuilder.Options);
        }
    }
}

