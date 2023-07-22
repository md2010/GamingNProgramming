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
    public class AvatarRepository : IAvatarRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Avatar> Entities;

        public AvatarRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Avatar>();
        }

        public async Task<IEnumerable<Avatar>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<Avatar> GetAsync(Guid id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task AddAsync(Avatar entity)
        {
            Entities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Avatar entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
