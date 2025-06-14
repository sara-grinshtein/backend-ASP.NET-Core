using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.interfaces;
using Response = Repository.Entites.Response;
namespace Repository.Repositories
{
    public class ResponsRepository : Irepository<Response>
    {
        private readonly Icontext context;


        public ResponsRepository(Icontext context)
        {
            this.context = context;
        }


        async Task<Response> Irepository<Response>.AddItem(Response item)
        {
            await this.context.responses.AddAsync(item);
            await this.context.Save();
            return item;
        }

        async Task<Response> Irepository<Response>.DeleteItem(int id)
        {
            var item = await ((Irepository<Response>)this).Getbyid(id);
            this.context.responses.Remove(item);
            await this.context.Save();
            return item;
        }

        async Task<List<Response>> Irepository<Response>.GetAll()
        {
            return await context.responses.ToListAsync();
        }

        async Task<Response> Irepository<Response>.Getbyid(int id)
        {
            return await context.responses.FirstOrDefaultAsync(x => x.response_id == id);
        }

        async Task<Response> Irepository<Response>.UpDateItem(int id, Response item)
        {
            var response = await ((Irepository<Response>)this).Getbyid(id);
            response.context = item.context;
            response.rating = item.rating;
            response.helped_id = item.helped_id;
            await context.Save();
            return response;
        }

       
        public async Task<List<Response>> GetByHelpedId(int helpedId)
        {
            return await context.responses
                .Where(r => r.helped_id == helpedId)
                .ToListAsync();
        }

    }
}

