using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entites;
using Repository.interfaces;
//tytgbgjh

namespace Repository.Repositories
{
    public class MessageRpository : Irepository<Message>
    {

        private readonly Icontext context;
        public MessageRpository(Icontext context)
        {
            this.context = context;
        }

        public Message AddItem(Message item)
        {
            this.context.Messages.Add(item);
            this.context.Save();
            return item;
        }

        public Message DeleteItem(int id)
        {
            this.context.Messages.Remove(Getbyid(id));
            this.context.Save();
            return Getbyid(id);
        }

        public List<Message> GetAll()
        {
            return context.Messages.ToList();
        }

        public Message Getbyid(int id)
        {
            return context.Messages.FirstOrDefault(x => x.message_id == id);
        }

        public Message UpDateItem(int id, Message item)
        {
            var message = Getbyid(id);
            if (message == null)
                return null;

            message.description = item.description;
            message.isDone = item.isDone;
            message.volunteer_id = item.volunteer_id;
            message.helped_id = item.helped_id;

            this.context.Save();        

            return message;
        }


    }
}
