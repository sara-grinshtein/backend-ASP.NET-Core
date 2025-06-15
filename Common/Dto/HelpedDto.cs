using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
   public class HelpedDto
    {
        public int helped_id { get; set; }
        public string password { get; set; }
        public string helped_first_name { get; set; } //שם פרטי
        public string? helped_last_name { get; set; } //משפחה 
        public string tel { get; set; }//טל   
        public string email { get; set; }
        public string location { get; set; }
        public bool IsDeleted { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
