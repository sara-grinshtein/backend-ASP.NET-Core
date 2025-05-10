using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entites;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class HelpedRepository : Irepository<Helped>
    {
        private readonly Icontext context;
        public HelpedRepository(Icontext context)
        {
            this.context = context;
        }

        public Helped AddItem(Helped item)
        {
            this.context.Helpeds.Add(item);
            this.context.Save();
            return item;
        }

        public Helped DeleteItem(int id)
        {
            this.context.Helpeds.Remove(Getbyid(id));
            this.context.Save();
            return Getbyid(id);
        }

        public List<Helped> GetAll()
        {
            return context.Helpeds.ToList();
        }

        public Helped Getbyid(int id)
        {
            return context.Helpeds.FirstOrDefault(x => x.helped_id == id);
        }

        public Helped UpDateItem(int id, Helped item)
        {

            var helped = Getbyid(id);
            helped.tel = item.tel;
            helped.email = item.email;
            helped.password = item.password;
           //// helped.helped_id = item.helped_id;
            helped.helped_first_name = item.helped_first_name;
            helped.helped_last_name = item.helped_last_name;
            helped.location = item.location;
            context.Save();
            return Getbyid(id);//? האם לעשות זא

            ///להשלים את כל השדות
            
        }
    }
}
