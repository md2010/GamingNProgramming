using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Repository;
using GamingNProgramming.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Service
{
    public class GameService : IGameService
    {
        private IMapRepository Repository { get; set; }
        private IPlayerRepository PlayerRepository { get; set; }
        public GameService(IMapRepository repo, IPlayerRepository playerRepository)
        {
            this.Repository = repo;
            this.PlayerRepository = playerRepository;
        }

        public async Task<bool> InsertPlayerTask(PlayerTask playerTask, bool isDefaultMap, Guid levelId)
        {
            playerTask.DateUpdated = DateTime.Now;
            playerTask.DateCreated = DateTime.Now;
            playerTask.Id = Guid.NewGuid();

            await this.PlayerRepository.InsertPlayerTask(playerTask);

            var player = await PlayerRepository.GetAsync(playerTask.PlayerId);
            if(isDefaultMap)
            {
                player.DefaultPoints += playerTask.ScoredPoints;
                player.DefaultTimeConsumed += playerTask.ExecutionTime;
            }
            else
            {
                player.Points += playerTask.ScoredPoints;
                player.TimeConsumed += playerTask.ExecutionTime;
            }
            
            await PlayerRepository.UpdatePlayer(player);

            return true;
        }

        public async Task<bool> UpdateScoredPoints(Guid playerTaskId, int newPoints)
        {
            var entity = await PlayerRepository.GetPlayerTask(playerTaskId);

            var player = await PlayerRepository.GetAsync(entity.PlayerId);
            
            if(entity.ScoredPoints < newPoints)
            {
                player.Points += (newPoints - entity.ScoredPoints);
            }
            else
            {
                player.Points -= (entity.ScoredPoints - newPoints);
            }

            entity.ScoredPoints = newPoints;

            await PlayerRepository.UpdatePlayerTask(entity);
            await PlayerRepository.UpdatePlayer(player);

            return true;
        }

        #region Battle

        public async Task<Battle> GetBattleAsync(Guid id)
        {
            return await PlayerRepository.GetBattleAsync(id);
        }
        public async Task<Battle> InsertBattle(Guid player1Id, Guid player2Id)
        {
            var result = await GetAssignmentsForBattleAsync(player1Id, player2Id);

            var battle = new Battle();
            battle.DateUpdated = DateTime.Now;
            battle.DateCreated = DateTime.Now;
            battle.Id = Guid.NewGuid();
            battle.LevelNumber = result.Item2;
            battle.Player1Points = 0;
            battle.Player2Points = 0;

            battle.AssignmentIds = string.Join(",", result.Item1.Select(x => x.Id));

            await PlayerRepository.AddBattleAsync(battle);

            return battle;
        }

        private async Task<int> GetLevelNumberForBattle(Guid player1Id, Guid player2Id, Guid defaultMapId)
        {
            var level1 = 1;
            var level2 = 1;

            var playerTask1 = await PlayerRepository.FindPlayerTaskForBattle(player1Id, defaultMapId!);
            if(playerTask1 != null)
            {
                level1 = (await Repository.GetLevelAsync(playerTask1.Assignment.LevelId)).Number;
            }

            var playerTask2 = await PlayerRepository.FindPlayerTaskForBattle(player2Id, defaultMapId);
            if(playerTask2 != null)
            {
                level2 = (await Repository.GetLevelAsync(playerTask2.Assignment.LevelId)).Number;
            }
            
            return level1 > level2 ? level2 : level1;
        }

        private async Task<Tuple<List<Assignment>, int>> GetAssignmentsForBattleAsync(Guid player1Id, Guid player2Id)
        {
            var defaultMapId = (await Repository.GetDefaultMapAsync()).Id;

            int levelNumber = await GetLevelNumberForBattle(player1Id, player2Id, defaultMapId);
            var assignments = await Repository.GetAssignmentsForForBattleAsync(levelNumber, defaultMapId);

            return new Tuple<List<Assignment>, int>(assignments, levelNumber);
        }


        #endregion
        public async Task<Map> GetAsync(Guid id)
        {
            return await this.Repository.GetAsync(id);
        }

        public async Task<Assignment> GetTaskAsync(Guid id)
        {
            return await this.Repository.GetTaskAsync(id);
        }

        public async Task<Map> GetMapByProfessorIdForEditingAsync(Guid id)
        {
            return await this.Repository.GetMapByProfessorIdForEditingAsync(id);
        }

        public async Task<List<Map>> GetMapByProfessorIdAsync(Guid id)
        {
            return await this.Repository.GetMapByProfessorIdAsync(id);
        }
        public async Task<bool> AddMapAsync(Guid professorId, Map map)
        {
            map.ProfessorId = professorId;
            CreateMapEntity(map);
            await this.Repository.AddAsync(map);

            return true;
        }

        public async Task<bool> UpdateMapAsync(Guid professorId, Map map)
        {
            var oldMap = await this.Repository.GetMapByProfessorIdForEditingAsync(professorId);
            this.Repository.Remove(oldMap);

            map.ProfessorId = professorId;            
            CreateMapEntity(map);
            this.Repository.Add(map);

            return true;
        }

        private void CreateMapEntity(Map map)
        {
            map.DateCreated = DateTime.Now;
            map.DateUpdated = DateTime.Now;
            map.Id = Guid.NewGuid();
            var points = 0;

            int i = 1; 
            foreach (var level in map.Levels)
            {
                int j = 1;
                level.DateCreated = DateTime.Now;
                level.DateUpdated = DateTime.Now;
                level.Id = Guid.NewGuid();
                level.MapId = map.Id;
                level.Number = i;
                var levelPoints = 0;

                foreach (var task in level.Assignments)
                {
                    task.DateCreated = DateTime.Now;
                    task.DateUpdated = DateTime.Now;
                    task.Id = Guid.NewGuid();
                    task.LevelId = level.Id;
                    task.Number = j;
                    points += task.Points;
                    levelPoints += task.Points;

                    foreach (var testCase in task.TestCases)
                    {
                        testCase.DateCreated = DateTime.Now;
                        testCase.DateUpdated = DateTime.Now;
                        testCase.Id = Guid.NewGuid();
                        testCase.AssignmentId = task.Id;
                    }

                    foreach (var answer in task.Answers)
                    {
                        answer.DateCreated = DateTime.Now;
                        answer.DateUpdated = DateTime.Now;
                        answer.Id = Guid.NewGuid();
                        answer.AssignmentId = task.Id;
                    }
                    j++;
                }
                level.Points = levelPoints;
                i++;
            }
            map.Points = points;
        }
    }
}
