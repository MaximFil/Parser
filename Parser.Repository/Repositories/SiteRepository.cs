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

        public List<Site> GetSites()
        {
            return _context.Sites.ToList();
        }
    }
}
