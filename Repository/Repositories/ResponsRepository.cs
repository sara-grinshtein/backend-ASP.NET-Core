using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
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
        public Response AddItem(Response item)
        {
            this.context.responses.Add(item);
            this.context.Save();
            return item;
        }

        public Response DeleteItem(int id)
        {
            this.context.responses.Remove(Getbyid(id));
            this.context.Save();
            return Getbyid(id);
        }

        public List<Response> GetAll()
        {
            return context.responses.ToList();
        }

        public Response Getbyid(int id)
        {
            return context.responses.FirstOrDefault(x => x.response_id == id);

        }

        public Response UpDateItem(int id, Response item)
        {
            var respon = Getbyid(id);
            respon.context = item.context;
           // respon.response_id = item.response_id;
            respon.rating = item.rating;
            respon.helped_id = item.helped_id;
            context.Save();

            return Getbyid(id);//?


        }
    }
}

