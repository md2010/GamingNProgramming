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
        Task<bool> AddMapAsync(Guid professorId, Map map);

        Task<Map> GetMapByProfessorIdForEditingAsync(Guid id);

        Task<List<Map>> GetMapByProfessorIdAsync(Guid id);

        Task<bool> UpdateMapAsync(Guid professorId, Map map);
    }
}
