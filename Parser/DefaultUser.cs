using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser
{
    public static class DefaultUser
    {
        public static void InitializeUser(IServiceProvider serviceProvider)
        {
            using (var context = new Context(serviceProvider.GetRequiredService<DbContextOptions<Context>>()))
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
