using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;

namespace Mock
{
    public class DataBaseForTest : DataBase, Icontext
    {
        private readonly string? _testDbName;

        public DataBaseForTest(DbContextOptions<DataBase> options) : base(options)
        {
        }

        public DataBaseForTest(string testDbName)
        {
            _testDbName = testDbName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_testDbName))
            {
                optionsBuilder.UseSqlServer(
                    $"Server=localhost;Database={_testDbName};Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;");
            }
        }

        // ממשק Icontext - חשיפת DbSet לפי צורך
        DbSet<My_areas_of_knowledge> Icontext.areas_Of_Knowledges
        {
            get => areas_Of_Knowledges;
            set => areas_Of_Knowledges = value;
        }

        DbSet<Response> Icontext.Responses
        {
            get =>Responses;
            set => Responses = value;
        }

        public async Task Save()
        {
            await SaveChangesAsync();
        }
    }
}
