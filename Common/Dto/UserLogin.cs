using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class UserLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        [Display(Name = "Role (Only required for registration)", Description = "Use 'Volunteer' or 'Helped' when registering a new user")]
        public string?  Role { get; set; }  

        

    }
}
