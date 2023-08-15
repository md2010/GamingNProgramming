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

        Task<Map> GetDefaultMapAsync();

        Task<Assignment> GetTaskAsync(Guid id);

        Task<Level> GetLevelAsync(Guid levelId);

        Task<List<Assignment>> GetAssignmentsForForBattleAsync(int levelNumber, Guid mapId);

        Task<Map> GetMapByProfessorIdForEditingAsync(Guid id);

        Task<List<Map>> GetMapByProfessorIdAsync(Guid id);

        Task AddAsync(Map entity);

        void Add(Map entity);

        void Remove(Map entity);
    }
}
