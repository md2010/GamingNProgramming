using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;

namespace GamingNProgramming.DAL.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext() {}
        public AppDbContext(IConfiguration configuration, DbContextOptions<AppDbContext> options) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-7LG8UG8;Initial Catalog=GamingNProgrammingDB;Integrated Security=True; Trust Server Certificate = True");
        }
       
    }
}
