using GamingNProgramming.Common;
using GamingNProgramming.Model;
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
        Task<IEnumerable<Player>> GetAllAsync();

        Task<Player> GetAsync(Guid id);

        Task<Player> GetByUsername(string username);

        Task<PagedList<Player>> FindAsync(
           Expression<Func<Player, bool>> filter = null,
           Func<IQueryable<Player>, IOrderedQueryable<Player>> orderBy = null,
           string includeProperties = "");

        Task AddAsync(Player entity);

        Task RemoveAsync(Player entity);
    }
}
