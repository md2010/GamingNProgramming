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
    public class ProfessorService : IProfessorService
    {
        protected IProfessorRepository Repository { get; set; }
        public ProfessorService(IProfessorRepository repository)
        {
            Repository = repository;
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public async Task<Professor> GetAsync(Guid id)
        {
            return await Repository.GetAsync(id);
        }

        public async Task AddAsync(Professor entity)
        {
            Create(entity);
            await Repository.AddAsync(entity);
        }

        private void Create(Professor entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
        }

        public async Task RemoveAsync(Professor entity)
        {
            await Repository.RemoveAsync(entity);
        }
    }
}
