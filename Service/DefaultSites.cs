using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parser.DAL;
using Parser.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParserService
{
    public class DefaultSites
    {
        public static void InitializeSites(DbContextOptions<ApplicationDbContext> options)
        {
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Sites.Any())
                {
                    context.Sites.Add
                        (new Site
                        {
                            Name = "habr",
                            Domain = "habr.com"
                        });
                    context.Sites.Add
                        (new Site
                        {
                            Name = "tut.by",
                            Domain = "news.tut.by"
                        });
                    context.Sites.Add
                        (new Site
                        {
                            Name = "belta",
                            Domain = "www.belta.by"
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}