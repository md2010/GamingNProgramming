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

        public MapRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Map>();
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
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Map entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
