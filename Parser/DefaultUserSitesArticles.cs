using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser.DAL;
using Parser.DAL.Entities;
using System.IO;

namespace Parser
{
    public static class DefaultUserSitesArticles
    {
        public static void InitializeUserSites(IServiceProvider serviceProvider)
        {
            var user = new User
            {
                    FirstName = "Maxim",
                    LastName = "Filipovich",
                    DateSetting = DateTime.Now,
                    ViewSetting = true
                };
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {              
                if (!context.Users.Any())
                {
                    context.Users.Add(user);                    
                }
                var sites = context.Sites.ToList();
                foreach (var site in sites)
                {
                    if (!context.UserSites.Any())
                    {
                        context.UserSites.Add(new UserSite { UserId = user.Id, SiteId = site.Id });
                    }
                }
                //var articles = context.Articles.ToList();
                //var users = context.Users.FirstOrDefault();
                //foreach (var article in articles)
                //{
                //    var dbArticle = context.UserArticles.FirstOrDefault(t=>t.ArticleId==article.Id);
                //    if (dbArticle == null)
                //    {
                //        context.UserArticles.Add(new UserArticle { UserId = users.Id, ArticleId = article.Id, Deleted = false });
                //    }
                //}
                context.SaveChanges();    
            }
        }
    }
}
