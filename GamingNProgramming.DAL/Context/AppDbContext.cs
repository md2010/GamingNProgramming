using GamingNProgramming.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;

namespace GamingNProgramming.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<CoreUser> CoreUsers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<PlayerTask> PlayersTasks { get; set; }
        public DbSet<PlayerTaskAnswer> PlayersTasksAnswers { get; set; }


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
