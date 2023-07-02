using GamingNProgramming.Common;
using GamingNProgramming.DAL.Context;
using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GamingNProgramming.Repository
{
    public class CoreUserRepository : ICoreUserRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<CoreUser> Entities;

        public CoreUserRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<CoreUser>();
        }

        public async Task<IEnumerable<CoreUser>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<CoreUser> GetAsync(Guid id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<CoreUser> GetByEmailAsync(string email)
        {
            return Entities.FirstOrDefault(a => a.Email == email) ?? null;
        }

        public async Task<CoreUser> ValidateUserAsync(string email, string password)
        {
            CoreUser user = Entities
                .Where(a => a.Email == email)
                .Where(c => c.Password == password)
                .Include(u => u.Role)
                .FirstOrDefault() ?? null;
            return user;
        }

        public async Task<PagedList<CoreUser>> FindAsync(
           Expression<Func<CoreUser, bool>> filter = null,
           Func<IQueryable<CoreUser>, IOrderedQueryable<CoreUser>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<CoreUser> query = Entities;

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

        public async Task AddAsync(CoreUser entity)
        {
            Entities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(CoreUser entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }    

    }
}