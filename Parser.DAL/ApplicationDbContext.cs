using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.DAL.Entities;

namespace Parser.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<UserSite> UserSites { get; set; }
        public DbSet<UserArticle> UserArticles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserArticle>()
                .HasKey(t => new { t.UserId, t.ArticleId });

            modelBuilder.Entity<UserArticle>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserArticles)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserArticle>()
                .HasOne(sc => sc.Article)

                .WithMany(c => c.UserArticles)
                .HasForeignKey(sc => sc.ArticleId);

            modelBuilder.Entity<UserSite>()
                .HasKey(t => new { t.UserId, t.SiteId });

            modelBuilder.Entity<UserSite>().HasOne(sc => sc.User)
                .WithMany(c => c.UserSites)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserSite>().HasOne(sc => sc.Site)
                .WithMany(c => c.UserSites)
                .HasForeignKey(sc => sc.SiteId);
        }
    }
}