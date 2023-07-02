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
    public interface IAvatarRepository
    {
        Task<IEnumerable<Avatar>> GetAllAsync();

        Task<Avatar> GetAsync(Guid id);

        Task AddAsync(Avatar entity);

        Task RemoveAsync(Avatar entity);
    }
}
