using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DAL.Entities
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Site> Sites { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public Context() : base()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserArticle>()
                .HasKey(t => new { t.UserId, t.ArticleId });

            modelBuilder.Entity<UserArticle>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserArticle)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserArticle>()
                .HasOne(sc => sc.Article)
                .WithMany(c => c.UserArticle)
                .HasForeignKey(sc => sc.ArticleId);

            modelBuilder.Entity<UserSite>()
                .HasKey(t => new { t.UserId, t.SiteId });

            modelBuilder.Entity<UserSite>().HasOne(sc => sc.User)
                .WithMany(c => c.UserSite)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserSite>().HasOne(sc => sc.Site)
                .WithMany(c => c.UserSite)
                .HasForeignKey(sc => sc.SiteId);
        }
    }
}
