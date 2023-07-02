using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected ICoreUserService Service { get; set; }
        protected IPlayerService PlayerService { get; set; }
        protected AuthenticationService AuthService { get; set; }

        protected PasswordGenerator PasswordGenerator { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(ICoreUserService service, IPlayerService playerService, IHttpContextAccessor httpContextAccessor) 
        {
            this.Service = service;
            this.PlayerService = playerService;
            this._httpContextAccessor = httpContextAccessor;
            AuthService = new AuthenticationService();
            PasswordGenerator = new PasswordGenerator();
        }

        // GET: api/<LoginController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = "";
            return Ok(result);
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await Service.GetAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(model == null)
            {
                return BadRequest();
            }

            var user = await Service.GetByEmailAsync(model.Username);
            if (user != null)
            {
                string passwordHashed = PasswordGenerator.GenerateHashedPassword(model.Password, user.PasswordSalt);
                var validatedUser = await Service.ValidateUserAsync(model.Username, passwordHashed);
                if(validatedUser != null)
                {
                    var accessToken = AuthService.CreateToken(validatedUser);
                    return Ok(new AuthResponse
                    {
                        UserId = validatedUser.Id,
                        Token = accessToken,
                        RoleId = validatedUser.Role.Id.ToString(),
                        RoleName = validatedUser.Role.Name
                    });
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(model == null)
            {
                return BadRequest();
            }

            if(await CheckIfEmailExists(model.Email))
            {
                return Conflict("Email already exists.");
            }

            if (await CheckIfUsernameExists(model.Username))
            {
                return Conflict("Username already exists.");
            }

            var user = MapCoreUser(model);

            if(model.RoleName == "Student")
            {
                var player = MapPlayer(model);
                await Service.RegisterPlayerAsync(user, player);
            }
            else
            {
                var professor = MapProfessor(model);
                await Service.RegisterProfessorAsync(user, professor);
            }
           
            return Ok();
        }

        private async Task<bool> CheckIfEmailExists(string email)
        {
            return await Service.GetByEmailAsync(email) == null ? false : true ;
        }

        private async Task<bool> CheckIfUsernameExists(string username)
        {
            return await PlayerService.GetByUsername(username) == null ? false : true;            
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            var result = "";
            return Ok(result);
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = "";
            return Ok(result);
        }

        #region Classes

        public class LoginModel 
        { 
            public string Username { get; set; }

            public string Password { get; set; }    
        }

        public class RegisterModel
        {
            public string Username { get; set; }

            public string Password { get; set; }

            public string Email { get; set; }

            public string AvatarId { get; set; }

            public string RoleId { get; set; }

            public string RoleName { get; set; }
        }

        public class AuthResponse
        {
            public Guid UserId { get; set; }

            public string Token { get; set; }

            public string RoleName { get; set; }

            public string RoleId { get; set; }
        }

        private CoreUser MapCoreUser(RegisterModel model)
        {
            CoreUser entity = new CoreUser();
            entity.Password = model.Password;
            entity.Email = model.Email;
            entity.RoleId = Helper.TransformGuid(model.RoleId);
            return entity;
        }

        private Player MapPlayer(RegisterModel model)
        {
            Player entity = new Player();
            entity.Username = model.Username;
            entity.AvatarId = Helper.TransformGuid(model.AvatarId);
            return entity;
        }

        private Professor MapProfessor(RegisterModel model)
        {
            Professor entity = new Professor();
            entity.Username = model.Username;
            return entity;
        }

        #endregion
    }
}
