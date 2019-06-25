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
    public class ArticleRepository
    {
        private readonly ApplicationDbContext _context;
        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Article> GetArticles()
        {
            return _context.Articles;
        }
    }
}