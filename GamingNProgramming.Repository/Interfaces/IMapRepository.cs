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
    public interface IMapRepository
    {
        Task<IEnumerable<Map>> GetAllAsync();

        Task<Map> GetAsync(Guid id);

        Task AddAsync(Map entity);

        Task RemoveAsync(Map entity);
    }
}
