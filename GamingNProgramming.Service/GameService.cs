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
        public GameService(IMapRepository repo)
        {
            this.Repository = repo;
        }
        public async Task<bool> AddMapAsync(Guid professorId, Map map)
        {
            map.ProfessorId = professorId;
            CreateMapEntity(map);
            await this.Repository.AddAsync(map);

            return true;
        }

        private void CreateMapEntity(Map map)
        {
            map.DateCreated = DateTime.Now;
            map.DateUpdated = DateTime.Now;
            map.Id = Guid.NewGuid();

            int i = 1; int j = 1; 
            foreach (var level in map.Levels)
            {
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
        }
    }
}
