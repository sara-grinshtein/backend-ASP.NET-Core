using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Repository.Entites;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class VolunteerRpository : Irepository<Volunteer>
    {

        private readonly Icontext context;
        public VolunteerRpository(Icontext context)
        {
            this.context = context;
        }

        public Volunteer AddItem(Volunteer item)
        {
            this.context.Volunteers.Add(item);
            this.context.Save();
            return item;

        }


        public Volunteer DeleteItem(int id)
        {
            this.context.Volunteers.Remove(Getbyid(id));
            this.context.Save();
            return Getbyid(id);

        }

        public List<Volunteer> GetAll()
        {
            return context.Volunteers.ToList();
        }

        public Volunteer Getbyid(int id)
        {
            return context.Volunteers.FirstOrDefault(x => x.volunteer_id == id);

        }

        public Volunteer UpDateItem(int id, Volunteer item)
        {
            var Volunteer = Getbyid(id);
            Volunteer.tel = item.tel;
            Volunteer.email = item.email;
            Volunteer.start_time = item.start_time;
            Volunteer.end_time = item.end_time;
            Volunteer.password = item.password;
         //   Volunteer.volunteer_id = item.volunteer_id;
            Volunteer.volunteer_first_name = item.volunteer_first_name;
            Volunteer.volunteer_last_name = item.volunteer_last_name;
            Volunteer.location = item.location;
            Volunteer.areas_of_knowledge = item.areas_of_knowledge;
            context.Save();
            return Getbyid(id);//?

            ///להשלים את כל השדות
            


        }
    }
}
