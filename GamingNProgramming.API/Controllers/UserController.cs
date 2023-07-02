using GamingNProgramming.Common;
using GamingNProgramming.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected ICoreUserService Service { get; set; }
        protected IPlayerService PlayerService { get; set; }

        public UserController(ICoreUserService service, IPlayerService playerService)
        {
            this.Service = service;
            this.PlayerService = playerService;
        }

        [Authorize]
        [HttpGet]
        [Route("player/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var uuid = Helper.TransformGuid(id);
            var result = (await PlayerService.FindAsync(u => u.UserId == uuid, null, "Avatar")).FirstOrDefault();
            return Ok(result);
        }

    }
}
