using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class UserLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        [Display(Name = "Role (Only required for registration)", Description = "Use 'Volunteer' or 'Helped' when registering a new user")]
        public string? Role { get; set; }

        public TimeSpan? Start_time { get; set; }
        public TimeSpan? End_time { get; set; }

        public string Tel { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Location { get; set; } // ← להוסיף את זה

    }
}
