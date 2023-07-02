using GamingNProgramming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Service
{
    public interface ICoreUserService
    {
        Task<IEnumerable<CoreUser>> GetAllAsync();

        Task<CoreUser> GetAsync(Guid id);

        Task<IEnumerable<CoreUser>> FindAsync(
           Expression<Func<CoreUser, bool>> filter = null,
           Func<IQueryable<CoreUser>, IOrderedQueryable<CoreUser>> orderBy = null,
           string includeProperties = "");

        Task<CoreUser> GetByEmailAsync(string name);

        Task<CoreUser> ValidateUserAsync(string email, string password);

        Task AddAsync(CoreUser entity);

        Task RemoveAsync(CoreUser entity);

        Task RegisterPlayerAsync(CoreUser user, Player player);

        Task RegisterProfessorAsync(CoreUser user, Professor professor);
    }
}
