using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService<VolunteerDto> volunteerService;
        private readonly IService<HelpedDto> helpedService;
        private readonly IConfiguration config;

        public LoginController(IService<VolunteerDto> volunteerService, IService<HelpedDto> helpedService, IConfiguration config)
        {
            this.volunteerService = volunteerService;
            this.helpedService = helpedService;
            this.config = config;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromQuery][FromHeader(Name = "role")] string role)
        {
            if (string.IsNullOrEmpty(role))
                return BadRequest("Role is required (Volunteer or Helped)");

            if (role.ToLower() == "volunteer")
            {
                var volunteer = volunteerService.Getbyid(id);
                if (volunteer == null)
                    return NotFound();
                return Ok(volunteer);
            }

            if (role.ToLower() == "helped")
            {
                var helped = helpedService.Getbyid(id);
                if (helped == null)
                    return NotFound();
                return Ok(helped);
            }

            return BadRequest("Invalid role. Use 'Volunteer' or 'Helped'.");
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserLogin value)
        {
            // אם המשתמש כבר קיים — מחזירים שגיאה מתאימה בלי לדרוש Role
            if (IsVolunteer(value))
                return BadRequest("Volunteer already exists.");

            if (IsHelped(value))
                return BadRequest("Helped already exists.");

            // אם המשתמש לא קיים – נדרש שRole יהיה קיים
            if (string.IsNullOrEmpty(value.Role))
                return BadRequest("New user must specify a role.");

            if (value.Role == "Volunteer")
            {
                var newVolunteer = new VolunteerDto
                {
                    volunteer_first_name = value.FirstName,
                    volunteer_last_name = value.LastName,
                    password = value.Password,
                    email = value.Email
                };

                volunteerService.AddItem(newVolunteer);
                return Ok("Volunteer registered successfully.");
            }

            if (value.Role == "Helped")
            {
                var newHelped = new HelpedDto
                {
                    helped_first_name = value.FirstName,
                    helped_last_name = value.LastName,
                    password = value.Password,
                    email = value.Email
                };

                helpedService.AddItem(newHelped);
                return Ok("Helped registered successfully.");
            }

            return BadRequest("Invalid role specified.");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin value)
        {
            var volunteer = AuthenticateVolunteer(value);
            if (volunteer != null)
            {
                var token = GenerateVolunteerToken(volunteer);
                return Ok(new { token, role = "Volunteer" });
            }

            var helped = AuthenticateHelped(value);
            if (helped != null)
            {
                var token = GenerateHelpedToken(helped);
                return Ok(new { token, role = "Helped" });
            }

            return Unauthorized("Invalid login credentials.");
        }

        private string GenerateVolunteerToken(VolunteerDto volunteer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, volunteer.volunteer_first_name),
                new Claim(ClaimTypes.Email, volunteer.email),
                new Claim(ClaimTypes.Role, "Volunteer"),
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateHelpedToken(HelpedDto helped)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, helped.helped_first_name),
                new Claim(ClaimTypes.Email, helped.email),
                new Claim(ClaimTypes.Role, "Helped"),
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool IsVolunteer(UserLogin value)
        {
            var volunteer = volunteerService.GetAll()
                .FirstOrDefault(x =>
                    x.volunteer_first_name == value.FirstName &&
                    x.volunteer_last_name == value.LastName &&
                    x.password == value.Password &&
                    x.email == value.Email);  

            return volunteer != null;
        }

        private bool IsHelped(UserLogin value)
        {
            var helped = helpedService.GetAll()
                .FirstOrDefault(x =>
                    x.helped_first_name == value.FirstName &&
                    x.helped_last_name == value.LastName &&
                    x.password == value.Password &&
                    x.email == value.Email);  // ✅ נבדק גם מייל

            return helped != null;
        }

        private VolunteerDto? AuthenticateVolunteer(UserLogin value)
        {
            return volunteerService.GetAll()
                .FirstOrDefault(x =>
                    x.volunteer_first_name == value.FirstName &&
                    x.volunteer_last_name == value.LastName &&
                    x.password == value.Password &&
                    x.email == value.Email);  // מחפש מתנדב לפי כל השדות
        }

        private HelpedDto? AuthenticateHelped(UserLogin value)
        {
            return helpedService.GetAll()
                .FirstOrDefault(x =>
                    x.helped_first_name == value.FirstName &&
                    x.helped_last_name == value.LastName &&
                    x.password == value.Password &&
                    x.email == value.Email);  // מחפש נעזר לפי כל השדות
        }



        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
