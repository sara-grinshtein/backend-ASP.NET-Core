using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserLogin value)
        {
            if (await IsVolunteer(value))
                return BadRequest("Volunteer already exists.");

            if (await IsHelped(value))
                return BadRequest("Helped already exists.");

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

                await helpedService.AddItem(newHelped);
                return Ok("Helped registered successfully.");
            }

            return BadRequest("Invalid role specified.");
        }

        // ✅ התחברות עם מייל + סיסמה בלבד
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
                return BadRequest("Missing credentials.");

            var volunteer = await AuthenticateVolunteer(credentials);
            if (volunteer != null)
            {
                var token = GenerateVolunteerToken(volunteer);
                return Ok(new { token });
            }

            var helped = await AuthenticateHelped(credentials);
            if (helped != null)
            {
                var token = GenerateHelpedToken(helped);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials.");
        }

        private string GenerateVolunteerToken(VolunteerDto volunteer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", volunteer.volunteer_id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, volunteer.volunteer_first_name),
                new Claim(ClaimTypes.Email, volunteer.email),
                new Claim(ClaimTypes.Role, "Volunteer"),
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateHelpedToken(HelpedDto helped)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", helped.helped_id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, helped.helped_first_name),
                new Claim(ClaimTypes.Email, helped.email),
                new Claim(ClaimTypes.Role, "Helped"),
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // אימות קיים בהרשמה
        private async Task<bool> IsVolunteer(UserLogin value)
        {
            var list = await volunteerService.GetAll();
            return list.Any(x =>
                x.email.ToLower() == value.Email.ToLower() &&
                x.password == value.Password);
        }

        private async Task<bool> IsHelped(UserLogin value)
        {
            var list = await helpedService.GetAll();
            return list.Any(x =>
                x.email.ToLower() == value.Email.ToLower() &&
                x.password == value.Password);
        }

        // התחברות לפי email+password בלבד
        private async Task<VolunteerDto?> AuthenticateVolunteer(UserCredentials credentials)
        {
            var list = await volunteerService.GetAll();
            return list.FirstOrDefault(x =>
                x.email.ToLower() == credentials.Email.ToLower() &&
                x.password == credentials.Password);
        }

        private async Task<HelpedDto?> AuthenticateHelped(UserCredentials credentials)
        {
            var list = await helpedService.GetAll();
            return list.FirstOrDefault(x =>
                x.email.ToLower() == credentials.Email.ToLower() &&
                x.password == credentials.Password);
        }
    }
}
