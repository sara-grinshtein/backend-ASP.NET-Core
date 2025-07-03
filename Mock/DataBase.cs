﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;

namespace Mock
{
    public class DataBase : DbContext, Icontext
    {
        public DbSet<Helped> Helpeds { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<My_areas_of_knowledge> areas_Of_Knowledges { get; set; }
        public DbSet<Response> responses { get; set; }
 

        public async Task Save()
        {
            await SaveChangesAsync();
        }
        public DataBase(DbContextOptions<DataBase> options) : base(options)
        {
        }
        public DataBase() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=project_yedidim1;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;");
        }



    }
}
