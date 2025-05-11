using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class My_areas_of_knowledgeRepository : Irepository<My_areas_of_knowledge>
    {
        private readonly Icontext context;

        public My_areas_of_knowledgeRepository(Icontext context)
        {
            this.context = context;
        }
        
        public async Task< My_areas_of_knowledge> AddItem(My_areas_of_knowledge item)
        {
            await this.context.areas_Of_Knowledges.AddAsync(item);
            await this.context.Save();
            return item;
        }

        public async Task< My_areas_of_knowledge> DeleteItem(int id)
        {
            var item = await ((Irepository<My_areas_of_knowledge>)this).Getbyid(id);
            this.context.areas_Of_Knowledges.Remove(item);
            await this.context.Save();
            return item;
        }
        public async Task< List<My_areas_of_knowledge>> GetAll()
        {
            return await context.areas_Of_Knowledges.ToListAsync();
        }

        public async Task  <My_areas_of_knowledge> Getbyid(int id)
        {
            return await context.areas_Of_Knowledges.FirstOrDefaultAsync(x => x.ID_knowledge == id);
        }

        public async Task <My_areas_of_knowledge> UpDateItem(int id, My_areas_of_knowledge item)
        {
            var knowledge = await ((Irepository<My_areas_of_knowledge>)this).Getbyid(id);
            knowledge.describtion=item.describtion;
            await context.Save();
            return knowledge;
        }
    }
}
