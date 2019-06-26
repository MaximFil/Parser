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

        public DbSet<UserArticle> GetUserArticles()
        {
            return _context.UserArticles;
        }

        public UserArticle GetUserArticle(int userId, int articleId)
        {
            return GetUserArticles()
                .FirstOrDefault(u => u.ArticleId == articleId && u.UserId == userId);
        }

        public void AddUserArticle(UserArticle userArticle)
        {
            GetUserArticles().Add(userArticle);
        }
        public UserArticle GetUserArticle(int articleId)
        {
            return GetUserArticles()
                .FirstOrDefault(u => u.ArticleId == articleId);
        }
        public void SetDeletedForArticleId(bool deleted, int articleId)
        {
            GetUserArticles().First(u => u.ArticleId == articleId).Deleted = deleted;
        }
    }
}
