using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Repository.Entites;
using Mock;
using System.Text.Json;
using System.Threading.Tasks;
using Service.interfaces;
using Common.Dto;

 

namespace Service.Algorithm
{
    //Step 1: Collect data
    public class DataFetcher:IDataFetcher
    {
        private readonly DataBase _db;
        private readonly IConfiguration _configuration;
        public DataFetcher(DataBase db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        //task 1.1 - Get all unassigned and incomplete help requests

        public List<Message> GetOpenMessages()
        {
            var openMessages = _db.Messages
                .Where(m => m.volunteer_id == null && m.isDone == false)
                .ToList();

            Console.WriteLine($"🔍 סך הכל הודעות פתוחות שנמצאו: {openMessages.Count}");

            foreach (var msg in openMessages)
            {
                Console.WriteLine($"🧾 הודעה: ID={msg.message_id}, תיאור='{msg.description}', helped_id={msg.helped_id}, isDone={msg.isDone}, volunteer_id={(msg.volunteer_id.HasValue ? msg.volunteer_id.Value.ToString() : "null")}");
            }

            return openMessages;
        }


        //task 1.2 - Get all volunteers that are marked as active (not deleted)
        public List<Volunteer> GetAvailableVolunteers()
        {
            return _db.Volunteers
                .Where(v => v.IsDeleted == false)
                .ToList();
        }
    }
}
