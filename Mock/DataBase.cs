using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;

namespace Mock
{
    public class DataBase :DbContext, Icontext
    {

        public DbSet<Helped> Helpeds{ get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<My_areas_of_knowledge> areas_Of_Knowledges { get; set; }
        public DbSet<Response> responses { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        //שכבה שמטפלת בSQL
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=project_yedidim1;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
