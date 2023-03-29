using HD_Backend.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HD_Backend.Data
{
    public class HelpDeskDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration Configuration;

        public HelpDeskDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("HelpDeskAPI"));
        }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Department> Departments { get; set;  }

        public DbSet<Faculty> Faculties { get; set; }


    }
}
