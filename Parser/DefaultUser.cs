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
    public static class DefaultUser
    {
        public static void InitializeUser(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (!context.Users.Any())
                {
                    context.Users.Add
                        (new User
                        {
                            FirstName = "Maxim",
                            LastName = "Filipovich",
                            DateSetting = DateTime.Now,
                            ViewSetting = 1
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}
