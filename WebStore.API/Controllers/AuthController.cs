using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebStore.API.Data;
using WebStore.API.Models.User;
using WebStore.API.Static;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private UserManager<ApiUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ApiUser> _logger;
        private readonly IConfiguration _conf;
        public AuthController(UserManager<ApiUser> userManager, 
            RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger<ApiUser> logger,
            IConfiguration conf)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
            _conf = conf;
        }
        // GET: api/<UserController>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempts to {userDTO.Email}");
            try
            {
                _logger.LogInformation($"Register method is started");
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                if (string.IsNullOrEmpty(userDTO.Role))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something went wrong  {nameof(Register)}");
                return Problem($"{ex.Message} - {ex.InnerException}");
            }
            
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDTO userDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDTO.Email);
                var confirm = await _userManager.CheckPasswordAsync(user, userDTO.Password);

                if (user == null || confirm == false)
                {
                    return Unauthorized(userDTO);
                }
                var tokenString = await GenerateToken(user);

                var response = new AuthResponse()
                {
                    Email = userDTO.Email,
                    Token = tokenString,
                    UserId = user.Id
                };

                return Accepted(response);


            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Something went wrong{ex.Message} - {nameof(Login)}");
                return BadRequest(ex);
            }
            return NoContent();
        }
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        private async Task<string> GenerateToken(ApiUser user)
        {
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JwtSettings:Key"]));
            var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(p => new Claim(ClaimTypes.Role, p));
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid,user.Id),

            }.Union(userClaims).Union(roleClaims);
            var token = new JwtSecurityToken(
                issuer: _conf["JwtSettings: Issuer"],
                audience: _conf["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToInt32(_conf["JwtSettings:Duration"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
