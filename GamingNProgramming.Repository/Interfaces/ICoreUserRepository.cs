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
    public interface ICoreUserRepository
    {
        Task<IEnumerable<CoreUser>> GetAllAsync();

        Task<CoreUser> GetAsync(Guid id);

        Task<CoreUser> GetByEmailAsync(string name);

        Task<CoreUser> ValidateUserAsync(string email, string password);

        Task<PagedList<CoreUser>> FindAsync(
           Expression<Func<CoreUser, bool>> filter = null,
           Func<IQueryable<CoreUser>, IOrderedQueryable<CoreUser>> orderBy = null,
           string includeProperties = "");

        Task AddAsync(CoreUser entity);

        Task RemoveAsync(CoreUser entity);
        
    }
}
