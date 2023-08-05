using GamingNProgramming.Common;
using GamingNProgramming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Service
{
    public interface IPlayerService
    {
        #region Professor
        Task<PagedList<Player>> GetProfessorsNotStudentsAsync(
           Guid id,
           List<Expression<Func<Player, bool>>> filter = null,
           string sortOrder = "",
           string includeProperties = ""
        );

        Task AddStudentAsync(Guid id, Guid playerId);

        Task RemoveStudentAsync(Guid playerId);

        #endregion
        Task<IEnumerable<Player>> GetAllAsync();

        Task<Player> GetAsync(Guid id);

        Task<Player> GetByUsername(string username);

        Task<IEnumerable<Player>> FindAsync(
           List<Expression<Func<Player, bool>>> filter = null,
           string sortOrder = "",
           string includeProperties = ""
        );

        Task AddAsync(Player entity);

        Task RemoveAsync(Player entity);

        #region Friends

        Task AddFriendAsync(Guid uid, Guid pid);

        Task RemoveFriendAsync(Guid uid, Guid pid);
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

        Task<List<PlayerTask>> GetPlayerTask(Guid playerId);

        #endregion
    }
}
