using GamingNProgramming.Common;
using GamingNProgramming.DAL.Context;
using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        protected AppDbContext DbContext;
        protected DbSet<Player> Entities;
        protected DbSet<Friend> FriendEntities;

        public PlayerRepository(AppDbContext context)
        {
            DbContext = context;
            Entities = DbContext.Set<Player>();
            FriendEntities = DbContext.Set<Friend>();
        }

        #region Professor
        public async Task<PagedList<Player>> GetProfessorsNotStudentsAsync(
          Guid id,
          List<Expression<Func<Player, bool>>> filter = null,
          string sortOrder = "",
          string includeProperties = "")
        {           
            IQueryable<Player> query = Entities;
            query = query.Where(p => p.ProfessorId != id && p.ProfessorId == null);

            if (filter != null)
            {
                query = ApplyPlayerFilter(query, filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            query = query.Include("Avatar");

            var result = await query.ToPagedListAsync(1, 20);

            if (!String.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "desc")
                    result.OrderByDescending(p => p.Points);
                else
                    result.OrderBy(p => p.Points);
            }

            return result;
        }

        public async Task AddStudentAsync(Guid id, Player entity)
        {
            entity.ProfessorId = id;
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveStudentAsync(Player entity)
        {
            entity.ProfessorId = null;
            await DbContext.SaveChangesAsync();
        }

        #endregion

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
           List<Expression<Func<Player, bool>>> filter = null,
           string sortOrder = "",
           string includeProperties = "")
        {
            IQueryable<Player> query = Entities;

            if (filter != null)
            {
                query = ApplyPlayerFilter(query, filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var result = await query.ToPagedListAsync(1, 20);

            if (!String.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "desc")
                    result.OrderByDescending(p => p.Points);
                else
                    result.OrderBy(p => p.Points);
            }

            return result;
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

        #region Friend

        public async Task<Friend> GetFriendAsync(Guid id, Guid pid)
        {
            IQueryable<Friend> query = FriendEntities;

            query = query.Where(f => f.Player1Id == id && f.Player2Id == pid);           
            var r = await query.ToListAsync();

            if(!r.Any())
            {
                query = query.Where(f => f.Player2Id == id && f.Player1Id == pid);
                r = await query.ToListAsync();
            }
            return r.ElementAt(0);
        }
        public async Task AddFriendAsync(Friend entity)
        {
            FriendEntities.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveFriendAsync(Friend entity)
        {
            FriendEntities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<PagedList<Player>> GetPlayersFriendsAsync(
           Guid id, 
           List<Expression<Func<Friend, bool>>> filter = null,
           string sortOrder = "",
           bool includeUser = false,
           string includeProperties = "")
        {
            IQueryable<Friend> query = FriendEntities;

            query = query.Where(f => f.Player1Id == id || f.Player2Id == id);  
            
            if(filter != null)
            {
                query = ApplyFriendFilter(query, filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            query = query.Include("Player1");
            query = query.Include("Player2");
            query = query.Include("Player1.Avatar");
            query = query.Include("Player2.Avatar");

            var str = query.ToQueryString();

            var result = await query.ToPagedListAsync(1, 20);
            var playersFriends = await FilterPlayersFriends(id, result, includeUser);
            
            if (!String.IsNullOrEmpty(sortOrder))
            {
                if(sortOrder == "desc")
                    playersFriends.OrderByDescending(p => p.Points);
                else
                    playersFriends.OrderBy(p => p.Points);
            }

            return playersFriends;
        }

        private async Task<PagedList<Player>> FilterPlayersFriends(Guid id, PagedList<Friend> friends, bool includeUser = false)
        {
            List<Player> players = new List<Player>();

            foreach(var f in friends)
            {
                if(f.Player1.UserId != id)
                {
                    players.Add(f.Player1);
                }
                else
                {
                    players.Add(f.Player2);
                }
            }

            if (includeUser)
            {
                var user = friends.Where(x => x.Player1Id == id).Select(x => x.Player1).FirstOrDefault();
                if (user == null)
                {
                    user = friends.Where(x => x.Player2Id == id).Select(x => x.Player2).FirstOrDefault();
                }

                players.Add(user);
            }

            return await players.ToPagedListAsync(1, 20);
        }
        
        private IQueryable<Friend> ApplyFriendFilter(IQueryable<Friend> query, List<Expression<Func<Friend, bool>>> filter = null)
        {
            if(filter != null)
            {
                foreach(var f in filter)
                {
                    query = query.Where(f);
                }
            }
            return query;
        }

        private IQueryable<Player> ApplyPlayerFilter(IQueryable<Player> query, List<Expression<Func<Player, bool>>> filter = null)
        {
            if (filter != null)
            {
                foreach (var f in filter)
                {
                    query = query.Where(f);
                }
            }
            return query;
        }

        public async Task<PagedList<Player>> GetPlayersNotFriendsAsync(
           Guid id,
           List<Expression<Func<Player, bool>>> filter = null,
           string includeProperties = "")
        {
            IQueryable<Friend> queryFriend = FriendEntities;

            queryFriend = queryFriend.Where(f => f.Player1Id == id || f.Player2Id == id);
            var result = await queryFriend.ToListAsync();

            List<Guid> friendIds = new List<Guid>();

            foreach(var f in result)
            {
                if(f.Player1Id != id)
                {
                    friendIds.Add(f.Player1Id);
                }
                if (f.Player2Id != id)
                {
                    friendIds.Add(f.Player2Id);
                }
            }
            friendIds.Add(id);

            IQueryable<Player> query = Entities;
            query = query.Where(p => !friendIds.Contains(p.UserId));

            if (filter != null)
            {
                query = ApplyPlayerFilter(query, filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            query = query.Include("Avatar");

            return await query.ToPagedListAsync(1, 20);
        }

        #endregion

    }
}
