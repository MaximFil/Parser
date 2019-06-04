using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser.DAL;
using Parser.DAL.Entities;

namespace Parser
{
    public static class DefaultUserSites
    {
        public static void InitializeUserSites(IServiceProvider serviceProvider)
        {
            var user = new User
                {
                    FirstName = "Maxim",
                    LastName = "Filipovich",
                    DateSetting = DateTime.Now,
                    ViewSetting = 1
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
                    user.UserSites.Add(new UserSite { UserId=user.Id,SiteId=site.Id});
                }
                context.SaveChanges();    
            }
        }
    }
}
