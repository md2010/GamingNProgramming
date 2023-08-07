﻿using GamingNProgramming.Common;
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

        public async Task<bool> InsertPlayerTask(PlayerTask playerTask, bool isDefaultMap)
        {
            playerTask.DateUpdated = DateTime.Now;
            playerTask.DateCreated = DateTime.Now;
            playerTask.Id = Guid.NewGuid();

            await this.PlayerRepository.InsertPlayerTask(playerTask);

            var player = await PlayerRepository.GetAsync(playerTask.PlayerId);
            if(isDefaultMap)
            {
                player.DefultPoints += playerTask.ScoredPoints;
            }
            else
            {
                player.Points += playerTask.ScoredPoints;
            }

            await PlayerRepository.UpdatePlayer(player);

            return true;
        }

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

                foreach (var task in level.Assignments)
                {
                    task.DateCreated = DateTime.Now;
                    task.DateUpdated = DateTime.Now;
                    task.Id = Guid.NewGuid();
                    task.LevelId = level.Id;
                    task.Number = j;
                    points += task.Points;

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
                i++;
            }
            map.Points = points;
        }
    }
}
