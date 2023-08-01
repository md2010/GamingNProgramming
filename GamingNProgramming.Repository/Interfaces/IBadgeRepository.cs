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
    public interface IBadgeRepository
    {
        Task<IEnumerable<Badge>> GetAllAsync();

        Task<Badge> GetAsync(Guid id);

        Task AddAsync(Badge entity);

        Task RemoveAsync(Badge entity);
    }
}
