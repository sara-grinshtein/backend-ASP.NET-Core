using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;

// Mock 
namespace Mock
{
    public class DataBase : DbContext, Icontext
    {
        public DbSet<Helped> Helpeds { get; set; } 
        public DbSet<Message> Messages { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<KnowledgeCategory> KnowledgeCategories { get; set; }
        public DbSet<My_areas_of_knowledge> areas_Of_Knowledges { get; set; }
        public DataBase(DbContextOptions<DataBase> options) : base(options) { }

        public DataBase() { }

     
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public Task Save()
        {
            return SaveChangesAsync();
        }
    }
}
