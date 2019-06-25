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
    public class UserArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserArticle> GetUserArticles()
        {
            return _context.UserArticles;
        }

        public void AddUserArticle(UserArticle userArticle)
        {
            _context.UserArticles.Add(userArticle);
        }
    }
}
