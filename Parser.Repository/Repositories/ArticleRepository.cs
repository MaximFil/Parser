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
        private object userSitesIds;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Article> GetArticles()
        {
            return _context.Articles;
        }
        public void Add(Article article)
        {
            _context.Articles.Add(article);
        }
        public Article GetArticleForUrl(string url)
        {
            return GetArticles().FirstOrDefault(a => a.Url == url);
        }
        public IEnumerable<IGrouping<int,Article>> GetShowArticlesUser(IEnumerable<int> userSitesIds,int userId)
        {
            return GetArticles()
                .Include(a => a.UserArticles)
                .Include(a => a.Site)
                .Where(a => userSitesIds.Contains(a.SiteId))
                .Where(a => a.UserArticles.FirstOrDefault(u => u.Deleted == true && u.UserId == userId) == null)
                .OrderBy(a => a.SiteId)
                .GroupBy(a => a.SiteId);
        }
        public IEnumerable<IGrouping<int, Article>> GetAllArticlesUser(IEnumerable<int> userSitesIds, int userId)
        {
            return GetArticles()
                .Include(a => a.UserArticles)
                .Include(a => a.Site)
                .Where(a => userSitesIds.Contains(a.SiteId))
                .Where(a => a.UserArticles.FirstOrDefault(f => f.UserId == userId) == null)
                .OrderBy(a => a.SiteId)
                .GroupBy(a => a.SiteId);
        }
        public IEnumerable<Article> GetPartArticlesSite(IGrouping<int,Article> site,int partSize)
        {
            return site.OrderByDescending(s => s.Id).Take(partSize);
        }
        public int GetLastId(IGrouping<int, Article> site, int partSize)
        {
            return GetPartArticlesSite(site, partSize).Last().Id;
        }
        public string GetSiteName(IGrouping<int, Article> site)
        {
            return site.FirstOrDefault(t => t.SiteId == t.Site.Id).Site.Name;
        }
        public int GetSiteId(IGrouping<int, Article> site)
        {
            return site.FirstOrDefault().SiteId;
        }
        public IQueryable<Article> GetMoreShowArticles(int siteId, int idLastArticle, int partSize, int userId)
        {
            return GetShowIfArticles(siteId,idLastArticle,userId)
                .Take(partSize);
        }
        public IQueryable<Article> GetMoreAllArticles(int siteId, int idLastArticle, int partSize, int userId)
        {
            return GetAllIfArticles(siteId, idLastArticle,userId)
                .Take(partSize);
        }
        public IQueryable<Article> GetAllIfArticles(int siteId, int idLastArticle, int userId)
        {
            return GetArticles()
                .Include(a => a.UserArticles)
                .Where(a => a.SiteId == siteId)
                .Where(a => a.UserArticles.FirstOrDefault(f => f.UserId == userId) == null)
                .Where(a => a.Id < idLastArticle)
                .OrderByDescending(a => a.Id);
        }
        public IQueryable<Article> GetShowIfArticles(int siteId, int idLastArticle, int userId)
        {
            return GetArticles()
                .Include(a => a.UserArticles)
                .Where(a => a.SiteId == siteId)
                .Where(a => a.Id < idLastArticle)
                .Where(a => a.UserArticles.FirstOrDefault(u => u.Deleted == true && u.UserId == userId) == null)
                .OrderByDescending(a => a.Id);
        }
        public Article GetShowArticle(int siteId, int idLastArticle, int userId)
        {
            return GetShowIfArticles(siteId, idLastArticle, userId).FirstOrDefault();
        }
        public Article GetAllArticle(int siteId, int idLastArticle, int userId)
        {
            return GetAllIfArticles(siteId,idLastArticle,userId).FirstOrDefault();
        }
        public int GetMoreLastId(IQueryable<Article> dbArticles)
        {
            return dbArticles.Last().Id;
        }
    }
}