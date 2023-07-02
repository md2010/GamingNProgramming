using GamingNProgramming.Common;
using GamingNProgramming.DAL.Context;
using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Player> Entities;

        public PlayerRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Player>();
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<Player> GetAsync(Guid id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<Player> GetByUsername(string username)
        {
            return Entities.FirstOrDefault(a => a.Username == username) ?? null;
        }

        public async Task<PagedList<Player>> FindAsync(
           Expression<Func<Player, bool>> filter = null,
           Func<IQueryable<Player>, IOrderedQueryable<Player>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<Player> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.ToPagedListAsync(1, 10);
        }

        private void ApplyFilter()
        {

        }

        public async Task AddAsync(Player entity)
        {
            Entities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Player entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

    }
}
