using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Service
{
    public class PlayerService : IPlayerService
    {
        protected IPlayerRepository Repository { get; set; }
        public PlayerService(IPlayerRepository repository)
        {
            Repository = repository;
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public async Task<Player> GetAsync(Guid id)
        {
            return await Repository.GetAsync(id);
        }

        public async Task<Player> GetByUsername(string username)
        {
            return await Repository.GetByUsername(username);
        }

        public async Task<IEnumerable<Player>> FindAsync(
           Expression<Func<Player, bool>> filter = null,
           Func<IQueryable<Player>, IOrderedQueryable<Player>> orderBy = null,
           string includeProperties = "")
        {
            return await Repository.FindAsync(filter, orderBy, includeProperties);
        }

        public async Task AddAsync(Player entity)
        {
            Create(entity);
            await Repository.AddAsync(entity);
        }

        private void Create(Player entity)
        {            
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entity.XPs = 0;
            entity.Points = 0;
        }

        public async Task RemoveAsync(Player entity)
        {
            await Repository.RemoveAsync(entity);
        }
    }
}
