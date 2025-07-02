using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;

namespace Mock
{
    public class DataBaseForTest : DbContext, Icontext
    {
        private readonly string _testDbName;

        public DataBaseForTest(string testDbName)
        {
            _testDbName = testDbName;
        }

        public DataBaseForTest(DbContextOptions<DataBaseForTest> options) : base(options)
        {
        }

        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Helped> Helpeds { get; set; }
        public DbSet<My_areas_of_knowledge> areas_Of_Knowledges { get; set; }
        public DbSet<Response> responses { get; set; }

        // מימוש מלא של הממשק Icontext
        DbSet<My_areas_of_knowledge> Icontext.areas_Of_Knowledges
        {
            get => areas_Of_Knowledges;
            set => areas_Of_Knowledges = value;
        }

        DbSet<Response> Icontext.responses
        {
            get => responses;
            set => responses = value;
        }

        public async Task Save()
        {
            await SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = !string.IsNullOrEmpty(_testDbName)
                    ? $"Server=localhost\\SQLEXPRESS01;Database={_testDbName};Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
                    : "Server=localhost\\SQLEXPRESS01;Database=TestDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.HasKey(v => v.volunteer_id);
                entity.Property(v => v.volunteer_id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.message_id);
                entity.Property(m => m.message_id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Helped>(entity =>
            {
                entity.HasKey(h => h.helped_id);
                entity.Property(h => h.helped_id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<My_areas_of_knowledge>(entity =>
            {
                entity.HasKey(a => a.ID_knowledge);
                entity.Property(a => a.ID_knowledge).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.HasKey(r => r.response_id);
                entity.Property(r => r.response_id).ValueGeneratedOnAdd();
            });
        }
    }
}
