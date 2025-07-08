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
    public class VolunteerRpository : Irepository<Volunteer>
    {
        private readonly Icontext context;

        public VolunteerRpository(Icontext context)
        {
            this.context = context;
        }

        public async Task<Volunteer> AddItem(Volunteer item)
        {
            try
            {
                Console.WriteLine("📥 Repository: Adding volunteer entity to context...");
                await context.Volunteers.AddAsync(item);

                Console.WriteLine("💾 Repository: Saving changes to database...");
                await context.Save();

                Console.WriteLine($"✅ Repository: Volunteer saved successfully with ID={item.volunteer_id}");

                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Repository: Failed to save volunteer! {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"🔍 Inner Exception: {ex.InnerException.Message}");
                throw;
            }
        }


        public async Task<Volunteer> Getbyid(int id)
        {
            return await context.Volunteers
                .Include(v => v.areas_of_knowledge)
                .FirstOrDefaultAsync(x => x.volunteer_id == id);
        }

        public async Task<Volunteer> UpDateItem(int id, Volunteer item)
        {
            var volunteer = await Getbyid(id);

            if (volunteer == null)
                return null;

            volunteer.tel = item.tel;
            volunteer.email = item.email;
            volunteer.start_time = item.start_time;
            volunteer.end_time = item.end_time;
            volunteer.password = item.password;
            volunteer.volunteer_first_name = item.volunteer_first_name;
            volunteer.volunteer_last_name = item.volunteer_last_name;
            volunteer.Latitude = item.Latitude;
            volunteer.Longitude = item.Longitude;
            volunteer.areas_of_knowledge = item.areas_of_knowledge;

            context.Volunteers.Update(volunteer);
            await context.Save();
            return volunteer;
        }

        public async Task<Volunteer> DeleteItem(int id)
        {
            var volunteer = await Getbyid(id);
            if (volunteer == null)
                return null;

            volunteer.IsDeleted = true;
            await context.Save();
            return volunteer;
        }

        public async Task<List<Volunteer>> GetAll()
        {
            return await context.Volunteers
                .Include(v => v.areas_of_knowledge)
                .Where(v => !v.IsDeleted)
                .ToListAsync();
        }


        public async Task Save()
        {
           context.SaveChangesAsync(); // ✅ חובה לשמור למסד
        }

    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Repository.Entites;
//using Repository.interfaces;

//namespace Repository.Repositories
//{
//    public class VolunteerRpository : Irepository<Volunteer>
//    {

//        private readonly Icontext context;
//        public VolunteerRpository(Icontext context)
//        {
//            this.context = context;
//        }

//        async Task<Volunteer> Irepository<Volunteer>.AddItem(Volunteer item)
//        {
//            await this.context.Volunteers.AddAsync(item);
//            await this.context.Save();
//            return item;
//        }

//        async Task<Volunteer> Irepository<Volunteer>.Getbyid(int id)
//        {
//            return await context.Volunteers.FirstOrDefaultAsync(x => x.volunteer_id == id);
//        }

//        async Task<Volunteer> Irepository<Volunteer>.UpDateItem(int id, Volunteer item)
//        {
//            var volunteer = await ((Irepository<Volunteer>)this).Getbyid(id);
//            volunteer.tel = item.tel;
//            volunteer.email = item.email;
//            volunteer.start_time = item.start_time;
//            volunteer.end_time = item.end_time;
//            volunteer.password = item.password;
//            volunteer.volunteer_first_name = item.volunteer_first_name;
//            volunteer.volunteer_last_name = item.volunteer_last_name;
//            volunteer.areas_of_knowledge = item.areas_of_knowledge;
//            await context.Save();
//            return volunteer;
//        }

//        //async Task<Volunteer> Irepository<Volunteer>.DeleteItem(int id)
//        //{
//        //    var item = await ((Irepository<Volunteer>)this).Getbyid(id);
//        //    this.context.Volunteers.Remove(item);
//        //    await this.context.Save();
//        //    return item;
//        //}
//        async Task<Volunteer> Irepository<Volunteer>.DeleteItem(int id)
//        {
//            var volunteer = await ((Irepository<Volunteer>)this).Getbyid(id);
//            volunteer.IsDeleted = true;
//            await context.Save();
//            return volunteer;
//        }


//        //async Task<List<Volunteer>> Irepository<Volunteer>.GetAll()
//        //{
//        //    return await context.Volunteers.ToListAsync();
//        //}
//        async Task<List<Volunteer>> Irepository<Volunteer>.GetAll()
//        {
//            return await context.Volunteers
//                .Where(v => !v.IsDeleted)
//                .ToListAsync();
//        }



//    }
//}
