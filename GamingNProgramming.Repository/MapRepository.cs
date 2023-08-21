﻿using GamingNProgramming.Common;
using GamingNProgramming.DAL.Context;
using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Repository
{
    public class MapRepository : IMapRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Map> Entities;
        protected DbSet<Level> LevelEntities;
        protected DbSet<Assignment> AssignmentEntities;
        protected DbSet<TestCase> TestCaseEntities;
        protected DbSet<Answer> AnswerEntities;

        public MapRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Map>();
            LevelEntities = DbContext.Set<Level>();
            AssignmentEntities = DbContext.Set<Assignment>();
            TestCaseEntities = DbContext.Set<TestCase>();
            AnswerEntities = DbContext.Set<Answer>();
        }

        public async Task<IEnumerable<Map>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<Map> GetAsync(Guid id)
        {
            var map = Entities
                .Where(a => a.Id == id)
                .Where(a => a.IsVisible == true)
                .Include(a => a.Levels)
                .ThenInclude(b => b.Assignments)
                    .ThenInclude(c => c.Answers)
                .Include(a => a.Levels)
                    .ThenInclude(b => b.Assignments)
                        .ThenInclude(c => c.TestCases)
                .Include(a => a.Levels)
                    .ThenInclude(b => b.Assignments)
                        .ThenInclude(c => c.Badge)
                .FirstOrDefault();

            map.Levels = map?.Levels.OrderBy(a => a.Number).ToList();
            foreach(var level in map.Levels)
            {
                level.Assignments = level.Assignments.OrderBy(a => a.Number).ToList();
            }

            return map;
        }

        public async Task<Map> GetDefaultMapAsync()
        {
            var map = Entities.Where(a => a.ProfessorId == null)
                .Include(a => a.Levels)
                .ThenInclude(b => b.Assignments)
                .FirstOrDefault();           

            map.Levels = map?.Levels.OrderBy(a => a.Number).ToList();

            foreach (var level in map.Levels)
            {
                level.Assignments = level.Assignments.OrderBy(a => a.Number).ToList();
            }

            return map;
        }

        public async Task<Assignment> GetTaskAsync(Guid id)
        {
            var a = AssignmentEntities
                .Where(a => a.Id == id)
                .Include(a => a.Answers)               
                .Include(a => a.TestCases)  
                .Include(a => a.Badge)
                .FirstOrDefault();

            return a;
        }

        public async Task<Level> GetLevelAsync(Guid levelId)
        {
            var a = LevelEntities
                .Where(a => a.Id == levelId)
                .Include(a => a.Assignments)
                .FirstOrDefault();

            return a;
        }

        public async Task<List<Assignment>> GetAssignmentsForForBattleAsync(int levelNumber, Guid mapId)
        {
            var levels = await LevelEntities
                .Where(a => a.Number <= levelNumber)
                .Where(a => a.MapId == mapId)
                .Include(a => a.Assignments)
                .ToListAsync();

            var random = new Random();
            int index1 = random.Next(levels.Count);
            var codingTasks1 = levels[index1].Assignments.Where(a => a.IsCoding == true);
            var codingTask1 = codingTasks1.ElementAt(random.Next(codingTasks1.Count()));
            var theoryTasks1 = levels[index1].Assignments.Where(a => a.IsCoding == false);
            var theoryTask1 = theoryTasks1.ElementAt(random.Next(theoryTasks1.Count()));

            int index2 = random.Next(levels.Count);
            var codingTasks2 = levels[index2].Assignments.Where(a => a.IsCoding == true);
            var codingTask2 = codingTasks2.ElementAt(random.Next(codingTasks2.Count()));
            var theoryTasks2 = levels[index2].Assignments.Where(a => a.IsCoding == false);
            var theoryTask2 = theoryTasks2.ElementAt(random.Next(theoryTasks2.Count()));
            if(codingTask2.Id == codingTask1.Id)
            {
                do
                {
                    index2 = random.Next(levels.Count);
                    codingTasks2 = levels[index2].Assignments.Where(a => a.IsCoding == true);
                    codingTask2 = codingTasks2.ElementAt(random.Next(codingTasks2.Count()));

                } while (codingTask1.Id == codingTask2.Id);
            }
            if (theoryTask2.Id == theoryTask1.Id)
            {
                do
                {
                    index2 = random.Next(levels.Count);
                    theoryTasks2 = levels[index2].Assignments.Where(a => a.IsCoding == true);
                    theoryTask2 = codingTasks2.ElementAt(random.Next(codingTasks2.Count()));

                } while (theoryTask1.Id == theoryTask2.Id);
            }

            return new List<Assignment> { codingTask1, theoryTask1, codingTask2, theoryTask2 };
        }

        public async Task<Map> GetMapByProfessorIdForEditingAsync(Guid id)
        {
            var map = Entities
                .Where(a => a.ProfessorId == id)
                .Where(a => a.IsVisible == false)
                .Include(a => a.Levels)
                .ThenInclude(b => b.Assignments)
                    .ThenInclude(c => c.Answers)
                .Include(a => a.Levels)
                    .ThenInclude(b => b.Assignments)
                        .ThenInclude(c => c.TestCases)
                .ToList().FirstOrDefault();

            map.Levels = map?.Levels.OrderBy(a => a.Number).ToList();
            foreach (var level in map.Levels)
            {
                level.Assignments = level.Assignments.OrderBy(a => a.Number).ToList();
            }

            return map;
        }

        public async Task<List<Map>> GetMapByProfessorIdAsync(Guid id)
        {
            var maps = await Entities
                .Where(a => a.ProfessorId == id)
                .Where(a => a.IsVisible == true)
                .Include(a => a.Levels)
                .ThenInclude(b => b.Assignments)
                    .ThenInclude(c => c.Answers)
                .Include(a => a.Levels)
                    .ThenInclude(b => b.Assignments)
                        .ThenInclude(c => c.TestCases)
                .ToListAsync();

            foreach(var map in maps)
            {
                map.Levels = map?.Levels.OrderBy(a => a.Number).ToList();
                foreach (var level in map.Levels)
                {
                    level.Assignments = level.Assignments.OrderBy(a => a.Number).ToList();
                }
            }

            return maps;
        }

        public async Task AddAsync(Map entity)
        {
            Entities.Add(entity);
            foreach(var level in entity.Levels)
            {
                LevelEntities.Add(level);
                foreach(var task in level.Assignments)
                {
                    AssignmentEntities.Add(task);
                    if(task.TestCases != null && task.TestCases.Any())
                    {
                        foreach(var testaCase in task.TestCases)
                        {
                            TestCaseEntities.Add(testaCase);
                        }
                    }
                    if (task.Answers != null && task.Answers.Any())
                    {
                        foreach (var answer in task.Answers)
                        {
                            AnswerEntities.Add(answer);
                        }
                    }
                }
            }
            await DbContext.SaveChangesAsync();
        }

        public void Add(Map entity)
        {
            Entities.Add(entity);
            foreach (var level in entity.Levels)
            {
                LevelEntities.Add(level);
                foreach (var task in level.Assignments)
                {
                    AssignmentEntities.Add(task);
                    if (task.TestCases != null && task.TestCases.Any())
                    {
                        foreach (var testaCase in task.TestCases)
                        {
                            TestCaseEntities.Add(testaCase);
                        }
                    }
                    if (task.Answers != null && task.Answers.Any())
                    {
                        foreach (var answer in task.Answers)
                        {
                            AnswerEntities.Add(answer);
                        }
                    }
                }
            }
            DbContext.SaveChanges();
        }

        public void Remove(Map entity)
        {
            foreach (var level in entity.Levels.Where(l => l.Id != Guid.Empty))
            {
                foreach (var task in level.Assignments.Where(l => l.Id != Guid.Empty))
                {
                    if (task.TestCases != null && task.TestCases.Any())
                    {
                        foreach (var testaCase in task.TestCases.Where(l => l.Id != Guid.Empty))
                        {
                            TestCaseEntities.Remove(testaCase);
                        }
                    }
                    if (task.Answers != null && task.Answers.Any())
                    {
                        foreach (var answer in task.Answers.Where(l => l.Id != Guid.Empty))
                        {
                            AnswerEntities.Remove(answer);
                        }
                    }
                    AssignmentEntities.Remove(task);
                }
                LevelEntities.Remove(level);
            }
            Entities.Remove(entity);

            DbContext.SaveChanges();
        }
       
    }
}
