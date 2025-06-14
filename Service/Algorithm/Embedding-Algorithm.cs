using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock;
using Repository.Entites;

namespace Service.Algorithm
{
    public class Embedding_Algorithm
    {
        private readonly DataBase _db;
        public Embedding_Algorithm(DataBase db) 
        {
            _db = db; 
        }

        //task 1.1 - Get all unassigned and incomplete help requests
        public List<Message> GetOpenMessages()
        {
            return _db.Messages
                .Where(m => m.volunteer_id == null && m.isDone == false)
                .ToList();
        }

        //task 1.2 - Get all volunteers that are marked as active (not deleted)
        public List<Volunteer> GetAvailableVolunteers()
        {
            return _db.Volunteers
                .Where(v => v.IsDeleted == false)
                .ToList();
        }

        //task 2.1 - Get volunteers available at the current time (filtered from database)
        public List<Volunteer> GetVolunteersAvailableNow()
        {
            var now = DateTime.Now.TimeOfDay;

            return _db.Volunteers
                .Where(v => v.IsDeleted == false &&
                            v.start_time.HasValue &&
                            v.end_time.HasValue &&
                            v.start_time.Value <= now &&
                            now <= v.end_time.Value)
                .ToList();
        }


    }
}
