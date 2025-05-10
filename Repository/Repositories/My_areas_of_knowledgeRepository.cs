using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        public My_areas_of_knowledge AddItem(My_areas_of_knowledge item)
        {
            this.context.areas_Of_Knowledges.Add(item);
            this.context.Save();
            return item;
        }

        public My_areas_of_knowledge DeleteItem(int id)
        {
            this.context.areas_Of_Knowledges.Remove(Getbyid(id));
            this.context.Save();
            return Getbyid(id);
        }
        //lalalalaallaalalallllllsssssss
        public List<My_areas_of_knowledge> GetAll()
        {
            return context.areas_Of_Knowledges.ToList();
        }

        public My_areas_of_knowledge Getbyid(int id)
        {
            return context.areas_Of_Knowledges.FirstOrDefault(x => x.ID_knowledge == id);
        }

        public My_areas_of_knowledge UpDateItem(int id, My_areas_of_knowledge item)
        {
            var knowledge = Getbyid(id);
            knowledge.describtion = item.describtion;
            context.Save();
            return Getbyid(id);
        }
    }
}
