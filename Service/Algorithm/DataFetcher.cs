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
    }
}
