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
    public class MessageRepository : Irepository<Message>   
    {
        private readonly Icontext context;
        public MessageRepository(Icontext context)
        {
            this.context = context;
        }

        async Task<Message> Irepository<Message>.AddItem(Message item)
        {
           // Console.WriteLine($"Adding message: helped_id={item.helped_id}, volunteer_id={item.volunteer_id}");
            await this.context.Messages.AddAsync(item);
            await this.context.Save();
            return item;
        }
        async Task<Message> Irepository<Message>.Getbyid(int id)
        {
            return await context.Messages.FirstOrDefaultAsync(x => x.message_id == id);
        }
        async Task<Message> Irepository<Message>.DeleteItem(int id)
        {
            var item = await ((Irepository<Message>)this).Getbyid(id);
            this.context.Messages.Remove(item);
            await this.context.Save();
            return item;
        }
  
        async Task<Message> Irepository<Message>.UpDateItem(int id, Message item)
        {
            var message = await ((Irepository<Message>)this).Getbyid(id);

            if (message != null)
            {
                message.isDone = item.isDone;
                message.description = item.description;
                message.ConfirmArrival = item.ConfirmArrival; 
                await context.Save();
            }

            return message;
        }
         
        async Task<List<Message>> Irepository<Message>.GetAll()
        {
            return await context.Messages.ToListAsync();
        }

       
    }
}
