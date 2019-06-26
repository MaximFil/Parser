using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parser.DAL;
using Parser.DAL.Entities;
using Parser.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
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
                var siteRepository = new SiteRepository(context);
                var repository = new Repository(context);
                if (!siteRepository.Any())
                {
                    siteRepository.Add
                        (new Site
                        {
                            Name = "habr",
                            Domain = "habr.com"
                        });
                    siteRepository.Add
                        (new Site
                        {
                            Name = "tut.by",
                            Domain = "news.tut.by"
                        });
                    siteRepository.Add
                        (new Site
                        {
                            Name = "belta",
                            Domain = "www.belta.by"
                        });
                    repository.SaveChanges();                    
                }
            }
        }
    }
}