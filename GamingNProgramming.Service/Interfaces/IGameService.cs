using GamingNProgramming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Service.Interfaces
{
    public interface IGameService
    {
        Task<Map> GetAsync(Guid id);
        Task<Map> GetTaskAsync(Guid id);
        Task<bool> AddMapAsync(Guid professorId, Map map);

        Task<Map> GetMapByProfessorIdForEditingAsync(Guid id);

        Task<List<Map>> GetMapByProfessorIdAsync(Guid id);

        Task<bool> UpdateMapAsync(Guid professorId, Map map);

        Task<bool> InsertPlayerTask(PlayerTask playerTask, bool isDefaultMap);
    }
}
