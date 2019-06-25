using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Repository.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddDefaultUser(User user)
        {
            if (!CheckForEmpty())
            {
                _context.Users.Add(user);
            }
        }
        public bool CheckForEmpty()
        {
            return _context.Users.Any();
        }

        public IEnumerable<int> GetUserSitesIds()
        {
            return _context.Users.Include(u => u.UserSites).FirstOrDefault().UserSites.Select(s => s.SiteId);
        }

        public bool GetUserViewSetting()
        {
            return _context.Users.First().ViewSetting;
        }

        public int GetUserId()
        {
            return _context.Users.First().Id;
        }
        public IQueryable<User> GetUser()
        {
            return _context.Users;
        }
    }
}
