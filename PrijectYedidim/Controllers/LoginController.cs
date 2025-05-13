using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery][FromHeader(Name = "role")] string role)
        {
            if (string.IsNullOrEmpty(role))
                return BadRequest("Role is req" +
                    "" +
                    "" +
                    "" +
                    "uired (Volunteer or Helped)");

            if (role.ToLower() == "volunteer")
            {
                var volunteer = await volunteerService.Getbyid(id);
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
        public async Task< IActionResult> Post([FromBody] UserLogin value)
        {
            // אם המשתמש כבר קיים — מחזירים שגיאה מתאימה בלי לדרוש Role
            if (await IsVolunteer(value))
                return BadRequest("Volunteer already exists.");

            if (await IsHelped(value))
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

                await volunteerService.AddItem(newVolunteer);
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
        public async Task<IActionResult> Login([FromBody] UserLogin value)
        {
            var volunteer =await AuthenticateVolunteer(value);
            if ( volunteer != null)
            {
                var token = GenerateVolunteerToken(volunteer);
                return Ok(new { token, role = "Volunteer" });
            }

            var helped = await AuthenticateHelped(value);
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
        private async Task<bool> IsVolunteer(UserLogin value)
        {
            var volunteers = await volunteerService.GetAll();
            var volunteer = volunteers.FirstOrDefault(x =>
                x.volunteer_first_name == value.FirstName &&
                x.volunteer_last_name == value.LastName &&
                x.password == value.Password &&
                x.email == value.Email);

            return volunteer != null;
        }

        private async  Task<bool> IsHelped(UserLogin value)
        {
            var helpeds = await helpedService.GetAll();
            var helped = helpeds.FirstOrDefault(x =>
                x.helped_first_name == value.FirstName &&
                x.helped_last_name == value.LastName &&
                x.password == value.Password &&
                x.email == value.Email);

            return helped != null;
        }

        private async Task<VolunteerDto?> AuthenticateVolunteer(UserLogin value)
        {
            var volunteers = await volunteerService.GetAll();
            return volunteers.FirstOrDefault(x =>
                x.volunteer_first_name == value.FirstName &&
                x.volunteer_last_name == value.LastName &&
                x.password == value.Password &&
                x.email == value.Email);
        }

        private async Task<HelpedDto?> AuthenticateHelped(UserLogin value)
        {
            var helpeds = await helpedService.GetAll();
            return helpeds.FirstOrDefault(x =>
                x.helped_first_name == value.FirstName &&
                x.helped_last_name == value.LastName &&
                x.password == value.Password &&
                x.email == value.Email);
        }

    }
}
