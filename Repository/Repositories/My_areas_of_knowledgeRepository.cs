using System.Collections.Generic;
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

        public async Task<My_areas_of_knowledge> AddItem(My_areas_of_knowledge item)
        {
            await context.areas_Of_Knowledges.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task<My_areas_of_knowledge> DeleteItem(int id)
        {
            var item = await Getbyid(id);
            if (item == null) return null;

            context.areas_Of_Knowledges.Remove(item);
            await context.Save();
            return item;
        }

        public async Task<List<My_areas_of_knowledge>> GetAll()
        {
            return await context.areas_Of_Knowledges
                .Include(x => x.KnowledgeCategory)  // encluding the description of knowledge area
                .Include(x => x.Volunteer)          // encluding information about the volunteer
                .ToListAsync();
        }

        public async Task<My_areas_of_knowledge> Getbyid(int id)
        {
            return await context.areas_Of_Knowledges
                .Include(x => x.KnowledgeCategory)
                .Include(x => x.Volunteer)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

     

        public async Task<My_areas_of_knowledge> UpDateItem(int id, My_areas_of_knowledge item)
        {
            var knowledge = await Getbyid(id);
            if (knowledge == null) return null;

            knowledge.volunteer_id = item.volunteer_id;
            knowledge.ID_knowledge = item.ID_knowledge;

            await context.Save();
            return knowledge;
        }

        public async Task Save()
        {
             await context.SaveChangesAsync();
        }

    }
}
