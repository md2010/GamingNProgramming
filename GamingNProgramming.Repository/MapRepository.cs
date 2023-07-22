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
            return await Entities.FindAsync(id);
        }

        public async Task<Map> GetByProfessorIdAsync(Guid id)
        {
            return Entities.FirstOrDefault(a => a.ProfessorId == id) ?? null;
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

        public async Task RemoveAsync(Map entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
