using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedList<Player>> GetProfessorsNotStudentsAsync(
           Guid id,
           List<Expression<Func<Player, bool>>> filter = null,
           string sortOrder = "",
           string includeProperties = ""
           )
        {
            return await Repository.GetProfessorsNotStudentsAsync(id, filter, sortOrder, includeProperties);
        }

        public async Task AddStudentAsync(Guid id, Guid playerId)
        {
            var entity = await GetAsync(playerId);
            await Repository.AddStudentAsync(id, entity);
        }

        public async Task RemoveStudentAsync(Guid playerId)
        {
            var entity = await GetAsync(playerId);
            await Repository.RemoveStudentAsync(entity);
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
           List<Expression<Func<Player, bool>>> filter = null,
           string sortOrder = "",
           string includeProperties = ""
        )
        {
            return await Repository.FindAsync(filter, sortOrder, includeProperties);
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

        #region Friends
        public async Task AddFriendAsync(Guid uid, Guid pid)
        {
            Friend entity = new Friend
            {
                Player1Id = uid,
                Player2Id = pid,
            };
            CreateFriend(entity);

            await Repository.AddFriendAsync(entity);
        }

        public async Task RemoveFriendAsync(Guid uid, Guid pid)
        {
            var friendToDelete = await Repository.GetFriendAsync(uid, pid);

            await Repository.RemoveFriendAsync(friendToDelete);
        }

        private void CreateFriend(Friend entity)
        {
            if (entity.Id == null || entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
        }
        public async Task<PagedList<Player>> GetPlayersFriendsAsync(
           Guid id,
           List<Expression<Func<Friend, bool>>> filter = null,
           string sortOrder = "",
           bool includeUser = false,
           string includeProperties = ""
           )
        {
            return await Repository.GetPlayersFriendsAsync(id, filter, sortOrder, includeUser, includeProperties);
        }

        public async Task<PagedList<Player>> GetPlayersNotFriendsAsync(
           Guid id,
           List<Expression<Func<Player, bool>>> filter = null,
           string includeProperties = ""
        )
        {
            return await Repository.GetPlayersNotFriendsAsync(id, filter, includeProperties);
        }

        public async Task<List<PlayerTask>> GetPlayerTask(Guid playerId, Guid mapId)
        {
            var list = await Repository.GetPlayerTask(playerId, mapId);
            return list;
        }
        #endregion
    }
}
