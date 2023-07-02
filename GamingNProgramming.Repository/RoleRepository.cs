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
    public class RoleRepository : IRoleRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Role> Entities;

        public RoleRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Role>();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<Role> GetAsync(Guid id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task AddAsync(Role entity)
        {
            Entities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Role entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
