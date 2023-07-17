using GamingNProgramming.Common;
using GamingNProgramming.DAL.Context;
using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace GamingNProgramming.Repository
{
    public class ProfessorRepository : IProfessorRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Professor> Entities;

        public ProfessorRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Professor>();
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<Professor> GetAsync(Guid id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<PagedList<Professor>> FindAsync(
           List<Expression<Func<Professor, bool>>> filter = null,
           string includeProperties = "")
        {
            IQueryable<Professor> query = Entities; 

            if (filter != null)
            {
                foreach (var f in filter)
                {
                    query = query.Where(f);
                }
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

        public async Task AddAsync(Professor entity)
        {
            Entities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Professor entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

    }
}
