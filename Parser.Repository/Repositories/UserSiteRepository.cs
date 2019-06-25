using Parser.DAL;
using Parser.DAL.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Repository.Repositories
{
    public class UserSiteRepository
    {
        private readonly ApplicationDbContext _context;
        public UserSiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddDefaultUserSites(UserSite userSite)
        {
            if (!CheckForEmpty())
            {
                _context.UserSites.Add(userSite);
            }
        }
        public bool CheckForEmpty()
        {
            return _context.UserSites.Any();
        }
        public void RemoveRange()
        {
            GetUserSites().RemoveRange(GetUserSites());
        }
        public DbSet<UserSite> GetUserSites()
        {
            return _context.UserSites;
        }

        public void Add(UserSite userSite)
        {
            _context.UserSites.Add(userSite);
        }
    }
}