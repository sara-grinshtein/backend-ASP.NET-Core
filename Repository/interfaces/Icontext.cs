
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Response = Repository.Entites.Response;

namespace Repository.interfaces
{
    //ממשק המתאר את הנתונים 
   public interface Icontext
    {
        int SaveChanges();
        public DbSet<Helped> Helpeds { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<My_areas_of_knowledge> areas_Of_Knowledges { get; set; }
        public DbSet<Response>  Responses{ get; set; }
        public DbSet<KnowledgeCategory> KnowledgeCategories { get; set; }

        public Task Save();
        Task<int> SaveChangesAsync();

    }


}
