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
    public class HelpedRepository : Irepository<Helped>
    {
        private readonly Icontext context;
        public HelpedRepository(Icontext context)
        {
            this.context = context;
        }

        public async Task<Helped> AddItem(Helped item)
        {
            await this.context.Helpeds.AddAsync(item);
            await this.context.Save();
            return item;
        }

        async Task<Helped> Irepository<Helped>.DeleteItem(int id)
        {
            var Helped = await ((Irepository<Helped>)this).Getbyid(id);
            Helped.IsDeleted = true;
            await context.Save();
            return Helped;
        }

        async Task<List<Helped>> Irepository<Helped>.GetAll()
        {
            return await context.Helpeds
                .Where(h => !h.IsDeleted)
                .ToListAsync();
        }


        public async Task<Helped> Getbyid(int id)
        {
            return await context.Helpeds.FirstOrDefaultAsync(x => x.helped_id == id);
        }

        public async Task< Helped> UpDateItem(int id, Helped item)
        {

            var helped =await Getbyid(id);
            helped.tel = item.tel;
            helped.email = item.email;
            helped.password = item.password;
            helped.helped_first_name = item.helped_first_name;
            helped.helped_last_name = item.helped_last_name;
            helped.location = item.location;
            await context.Save();
            return helped;
        }


    }
}
