using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class Context: DbContext
    {
        public DbSet<User> DbUser { get; set; }
        public DbSet<Article> DbArticle { get; set; }
        public DbSet<Site> DbSite { get; set; }
        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ParserDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserArticle>().HasKey(t => new { t.UserId, t.ArticleId });

            modelBuilder.Entity<UserArticle>().HasOne(sc => sc.User)
                .WithMany(s => s.userArticle)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserArticle>().HasOne(sc => sc.Article)
                .WithMany(c => c.userArticle)
                .HasForeignKey(sc => sc.ArticleId);

            modelBuilder.Entity<UserSite>().HasKey(t => new { t.UserId, t.SiteId });

            modelBuilder.Entity<UserSite>().HasOne(sc => sc.User)
                .WithMany(c => c.userSite)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserSite>().HasOne(sc => sc.Site)
                .WithMany(c => c.userSite)
                .HasForeignKey(sc => sc.SiteId);
        }
    }
    public class UserArticle
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public bool Deleted { get; set; }
    }
    public class UserSite
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
 