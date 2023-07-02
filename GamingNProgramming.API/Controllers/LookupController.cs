using GamingNProgramming.Model;
using GamingNProgramming.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/lookup")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        protected IRoleRepository Repository { get; set; }

        protected IAvatarRepository AvatarRepository { get; set; }
        public LookupController(IRoleRepository repository, IAvatarRepository repo) 
        {
            Repository = repository;   
            AvatarRepository = repo;
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await Repository.GetAllAsync();
            if(result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("avatars")]
        public async Task<IActionResult> GetAvatars()
        {
            var result = await AvatarRepository.GetAllAsync();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }
        
    }
}
