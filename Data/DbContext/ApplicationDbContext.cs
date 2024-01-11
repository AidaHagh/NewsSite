using Entities;
using Entities.news;
using Entities.Setting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUsers, ApplicationRoles, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<News> NewsTbl { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Advertising> Advertisings { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<AboutUs> Abouts { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<NewsPaper> NewsPapers { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUsers>(entity =>
            {
                entity.ToTable(name: "Users_Tbl");
                entity.Property(e => e.Id).HasColumnName("UserId");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationRoles>(entity =>
            {
                entity.ToTable(name: "Roles_Tbl");
            });
        }
    }
}
