using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;
using System;
using MySqlX.XDevAPI.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using GamingNProgramming.Service.Interfaces;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected ICoreUserService Service { get; set; }
        protected IPlayerService PlayerService { get; set; }
        protected IProfessorService ProfessorService { get; set; }
        protected IGameService GameService { get; set; }

        public UserController(
            ICoreUserService service, 
            IPlayerService playerService, 
            IProfessorService professorService,
            IGameService gameService)
        {
            this.Service = service;
            this.PlayerService = playerService;
            this.ProfessorService = professorService;
            this.GameService = gameService;
        }

        #region Professor

        [Authorize]
        [HttpGet]
        [Route("professor/{id}")]
        public async Task<IActionResult> GetProfessor(string id)
        {
            var uuid = Helper.TransformGuid(id);

            List<Expression<Func<Professor, bool>>> filters = new List<Expression<Func<Professor, bool>>>
            {
                u => u.UserId == uuid
            };

            var result = (await ProfessorService.FindAsync(filters)).FirstOrDefault();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("not-students")]
        public async Task<IActionResult> GetNotStudents([FromQuery] SearchPlayers model)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uuid = Helper.TransformGuid(id);

            List<Expression<Func<Player, bool>>> filters = new List<Expression<Func<Player, bool>>>();
            if (model != null)
            {
                if (!String.IsNullOrEmpty(model.Name))
                {
                    filters.Add(p => p.Username == model.Name || p.Username == model.Name);
                }
            }
            var result = await PlayerService.GetProfessorsNotStudentsAsync(uuid, filters, model.SortOrder, model.IncludeProperties);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        [Route("add-student/{id}")]
        public async Task<IActionResult> AddStudent(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uid = Helper.TransformGuid(userId);
            var pid = Helper.TransformGuid(id);
            await PlayerService.AddStudentAsync(uid, pid);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("delete-student/{id}")]
        public async Task<IActionResult> RemoveStudent(string id)
        {
            var uid = Helper.TransformGuid(id);

            await PlayerService.RemoveStudentAsync(uid);
            return Ok();
        }


        #endregion

        [Authorize]
        [HttpGet]
        [Route("player/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var uuid = Helper.TransformGuid(id);

            List<Expression<Func<Player, bool>>> filters = new List<Expression<Func<Player, bool>>>();
            filters.Add(u => u.UserId == uuid);

            var result = (await PlayerService.FindAsync(filters, "", "Avatar")).FirstOrDefault();
            if (result.ProfessorId != null)
            {
                var maps = await this.GameService.GetMapByProfessorIdAsync((Guid)result.ProfessorId);
                var sum = 0;
                foreach (var map in maps)
                {
                    sum += map.Points;
                }
                return Ok(new PlayerREST() { Player = result, Sum = sum} );
            }
        
            return Ok(new PlayerREST() { Player = result, Sum = 0 });
        }

        [Authorize]
        [HttpGet]
        [Route("players")]
        public async Task<IActionResult> FindPlayers([FromQuery] SearchPlayers model)
        {
            List<Expression<Func<Player, bool>>> filters = new List<Expression<Func<Player, bool>>>();
            if (model != null)
            {
                if(!string.IsNullOrEmpty(model.Name))
                {
                    filters.Add(u => u.Username != model.Name);
                }
                if (!string.IsNullOrEmpty(model.ProfessorId))
                {
                    var id = Helper.TransformGuid(model.ProfessorId);
                    filters.Add(u => u.ProfessorId == id);
                }
            }

            var result = await PlayerService.FindAsync(filters, model.SortOrder, model.IncludeProperties);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("friends")]
        public async Task<IActionResult> FindFriends([FromQuery]SearchFriends model)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uuid = Helper.TransformGuid(id);

            List<Expression<Func<Friend, bool>>> filters = new List<Expression<Func<Friend, bool>>>();
            if (model != null)
            {
                if(!String.IsNullOrEmpty(model.Name))
                {
                    filters.Add(p => p.Player1.Username == model.Name || p.Player2.Username == model.Name);
                }                
            }
            var result = await PlayerService.GetPlayersFriendsAsync(uuid, filters, model.SortOrder, Convert.ToBoolean(model.IncludeUser), model.IncludeProperties);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("add-friend/{id}")]
        public async Task<IActionResult> AddFriend(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uid = Helper.TransformGuid(userId);
            var pid = Helper.TransformGuid(id);
            await PlayerService.AddFriendAsync(uid, pid);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("delete-friend/{id}")]
        public async Task<IActionResult> RemoveFriend(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uid = Helper.TransformGuid(userId);
            var pid = Helper.TransformGuid(id);

            await PlayerService.RemoveFriendAsync(uid, pid);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("not-friends")]
        public async Task<IActionResult> GetNotFriends([FromQuery] SearchPlayers model)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uuid = Helper.TransformGuid(id);

            List<Expression<Func<Player, bool>>> filters = new List<Expression<Func<Player, bool>>>();
            if (model != null)
            {
                if (!String.IsNullOrEmpty(model.Name))
                {
                    filters.Add(p => p.Username == model.Name || p.Username == model.Name);
                }
            }
            var result = await PlayerService.GetPlayersNotFriendsAsync(uuid, filters, model.IncludeProperties);
            return Ok(result);
        }

        #region Classes
        public class SearchFriends
        {
            [FromQuery(Name = "username")]
            public string Name { get; set; } = "";

            [FromQuery(Name = "sortOrder")]
            public string SortOrder { get; set; } = "";

            [FromQuery(Name = "includeProperties")]
            public string IncludeProperties { get; set; } = "";

            [FromQuery(Name = "includeUser")]
            public string IncludeUser { get; set; } = "false";
        }

        public class SearchPlayers
        {
            [FromQuery(Name = "userId")]
            public string UserId { get; set; } = "";

            [FromQuery(Name = "username")]
            public string Name { get; set; } = "";

            [FromQuery(Name = "sortOrder")]
            public string SortOrder { get; set; } = "";

            [FromQuery(Name = "includeProperties")]
            public string IncludeProperties { get; set; } = "";
            [FromQuery(Name = "professorId")]
            public string ProfessorId { get; set; } = "";
        }

        public class PlayerREST
        {
            public Player Player { get; set; }
            public int Sum { get; set; } = 0;
        }


        #endregion

        #region Mapping

        #endregion

    }
}
