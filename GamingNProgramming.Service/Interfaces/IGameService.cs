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
        Task<Assignment> GetTaskAsync(Guid id);
        Task<bool> AddMapAsync(Guid professorId, Map map);

        Task<Map> GetMapByProfessorIdForEditingAsync(Guid id);

        Task<List<Map>> GetMapByProfessorIdAsync(Guid id);

        Task<Map> GetDefaultMapAsync();

        Task<bool> UpdateMapAsync(Guid professorId, Map map);

        Task<bool> InsertPlayerTask(PlayerTask playerTask, bool isDefaultMap, Guid levelId);

        Task<bool> UpdateScoredPoints(Guid playerTaskId, int newPoints);

        Task<Battle> InsertBattle(Guid player1Id, Guid player2Id);

        Task<Battle> GetBattleAsync(Guid id);

        Task<List<Battle>> FindBattlesAsync(Guid playerId);

        Task UpdateBattleAsync(Battle battle);

    }
}
