using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GamingNProgramming.Service
{
    public class CoreUserService : ICoreUserService
    {
        protected ICoreUserRepository Repository { get; set; }
        protected IPlayerService PlayerService { get; set; }

        protected IProfessorService ProfessorService { get; set; }

        protected PasswordGenerator PasswordGenerator { get; set; }

        public CoreUserService (ICoreUserRepository repository, IPlayerService playerService, IProfessorService professorService)
        {
            Repository = repository;
            PlayerService = playerService;
            ProfessorService = professorService;
            PasswordGenerator = new PasswordGenerator ();
        }

        public async Task<IEnumerable<CoreUser>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public async Task<CoreUser> GetAsync(Guid id)
        {
            return await Repository.GetAsync(id);
        }

        public async Task<CoreUser> GetByEmailAsync(string email)
        {
            return await Repository.GetByEmailAsync(email);
        }

        public async Task<CoreUser> ValidateUserAsync(string email, string password)
        {
            return await Repository.ValidateUserAsync(email, password);
        }

        public async Task<IEnumerable<CoreUser>> FindAsync(
           Expression<Func<CoreUser, bool>> filter = null,
           Func<IQueryable<CoreUser>, IOrderedQueryable<CoreUser>> orderBy = null,
           string includeProperties = "")
        {
            return await Repository.FindAsync(filter, orderBy, includeProperties);
        }

        public async Task AddAsync(CoreUser entity)
        {
            Create(entity);
            await Repository.AddAsync(entity);
        }

        private void Create(CoreUser entity)
        {
            if(entity.Id == null || entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entity.Password = PasswordGenerator.GenerateHashedPassword(entity.Password);
            entity.PasswordSalt = PasswordGenerator.Salt;
        }

        public async Task RemoveAsync(CoreUser entity)
        {
            await Repository.RemoveAsync(entity);
        }

        public async Task RegisterPlayerAsync(CoreUser user, Player player)
        {
            Create(user);
            await Repository.AddAsync(user);
            player.UserId = user.Id;
            await PlayerService.AddAsync(player);
        }

        public async Task RegisterProfessorAsync(CoreUser user, Professor professor)
        {
            Create(user);
            await Repository.AddAsync(user);
            professor.UserId = user.Id;
            await ProfessorService.AddAsync(professor);
        }

    }
}