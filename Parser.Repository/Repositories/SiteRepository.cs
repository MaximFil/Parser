using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parser.DAL.Entities;
using Parser.DAL;

namespace Parser.Repository.Repositories
{
    public class SiteRepository
    {
        private readonly ApplicationDbContext _context;
        public SiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<Site> GetSites()
        {
            return _context.Sites;
        }
        public int GetSiteIdForDomain(string domain)
        {
            return _context.Sites.FirstOrDefault(t => t.Domain == domain).Id;
        }
        public void Add(Site site)
        {
            _context.Sites.Add(site);
        }
        public bool Any()
        {
            return _context.Sites.Any();
        }
        public int GetSiteidForNameSite(string nameSite)
        {
            return _context.Sites.FirstOrDefault(t => t.Name == nameSite).Id;
        }
    }
}
