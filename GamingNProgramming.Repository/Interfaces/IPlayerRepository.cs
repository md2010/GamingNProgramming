using GamingNProgramming.Common;
using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Repository
{
    public interface IPlayerRepository
    {
        #region Professor
        Task<PagedList<Player>> GetProfessorsNotStudentsAsync(
         Guid id,
         List<Expression<Func<Player, bool>>> filter = null,
         string sortOrder = "",
         string includeProperties = "");

        Task AddStudentAsync(Guid id, Player entity);

        Task RemoveStudentAsync(Player entity);

        #endregion

        Task<bool> InsertPlayerTask(PlayerTask playerTask);

        Task<List<PlayerTask>> GetPlayerTask(Guid playerId, Guid mapId, Guid? taskId = null);

        Task<bool> UpdatePlayer(Player player);

        Task<IEnumerable<Player>> GetAllAsync();

        Task<Player> GetAsync(Guid id);

        Task<Player> GetByUsername(string username);

        Task<PagedList<Player>> FindAsync(
           List<Expression<Func<Player, bool>>> filter = null,
           string sortOrder = "",
           string includeProperties = "");

        Task AddAsync(Player entity);

        Task RemoveAsync(Player entity);

        #region Friends

        Task<Friend> GetFriendAsync(Guid id, Guid pid);
        Task AddFriendAsync(Friend entity);

        Task RemoveFriendAsync(Friend entity);
        Task<PagedList<Player>> GetPlayersFriendsAsync(
           Guid id,
           List<Expression<Func<Friend, bool>>> filter = null,
           string sortOrder = "",
           bool includeUser = false,
           string includeProperties = ""
        );

        Task<PagedList<Player>> GetPlayersNotFriendsAsync(
           Guid id,
           List<Expression<Func<Player, bool>>> filter = null,
           string includeProperties = ""
        );

        #endregion
    }
}
