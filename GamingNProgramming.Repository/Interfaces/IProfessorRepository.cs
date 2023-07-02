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
    public interface IProfessorRepository
    {
        Task<IEnumerable<Professor>> GetAllAsync();

        Task<Professor> GetAsync(Guid id);

        Task<PagedList<Professor>> FindAsync(
           Expression<Func<Professor, bool>> filter = null,
           Func<IQueryable<Professor>, IOrderedQueryable<Professor>> orderBy = null,
           string includeProperties = "");

        Task AddAsync(Professor entity);

        Task RemoveAsync(Professor entity);
    }
}
