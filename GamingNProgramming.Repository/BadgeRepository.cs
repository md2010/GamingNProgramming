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
    public class BadgeRepository : IBadgeRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Badge> Entities;

        public BadgeRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Badge>();
        }

        public async Task<IEnumerable<Badge>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<Badge> GetAsync(Guid id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task AddAsync(Badge entity)
        {
            Entities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Badge entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
