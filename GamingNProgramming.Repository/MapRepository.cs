using GamingNProgramming.Common;
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
