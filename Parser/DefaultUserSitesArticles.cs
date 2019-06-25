using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser.DAL;
using Parser.DAL.Entities;
using Parser.Repository.Repositories;
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
                var siteRepository = new SiteRepository(context);////         
                var userReposotiry = new UserRepository(context);////
                var userSiteRepository = new UserSiteRepository(context);
                var repository = new Repository.Repositories.Repository(context);
                userReposotiry.AddDefaultUser(user);////
                var sites = siteRepository.GetSites();////
                foreach (var site in sites)
                {
                    userSiteRepository.AddDefaultUserSites(new UserSite { UserId = user.Id, SiteId = site.Id });
                }
                repository.SaveChanges();    
            }
        }
    }
}
