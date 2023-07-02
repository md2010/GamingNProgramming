using GamingNProgramming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Service
{
    public interface IProfessorService
    {
        Task<IEnumerable<Professor>> GetAllAsync();

        Task<Professor> GetAsync(Guid id);

        Task AddAsync(Professor entity);

        Task RemoveAsync(Professor entity);

    }
}
