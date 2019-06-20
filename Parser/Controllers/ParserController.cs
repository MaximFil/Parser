using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Parser.ViewModels;
using Parser.DAL.Entities;
using Parser.DAL;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class ParserController : Controller
    {
        private readonly ApplicationDbContext _context;
        const int _partSize= 9;

        public ParserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public List<SiteViewModel> GetSites()
        {
            var sites = new List<SiteViewModel>();
            IEnumerable<IGrouping<int, Article>> dbArticles;
            using (_context)
            {
                var userSitesIds = _context.Users.Include(u => u.UserSites).FirstOrDefault().UserSites.Select(s => s.SiteId);
                var showArticle = _context.Users.FirstOrDefault().ViewSetting;
                var userId = _context.Users.FirstOrDefault().Id;
                if (showArticle == false)
                {
                    dbArticles = _context.Articles
                        .Include(a => a.UserArticles)
                        .Include(a=>a.Site)
                        .Where(a => userSitesIds.Contains(a.SiteId))
                        .Where(a => a.UserArticles.FirstOrDefault(f => f.UserId == userId) == null)                       
                        .OrderBy(t => t.SiteId)
                        .GroupBy(t => t.SiteId)
                        .ToList();                      
                }
                else
                {
                    dbArticles = _context.Articles
                        .Include(a => a.Site)
                        .Where(a => userSitesIds.Contains(a.SiteId))
                        .Where(a => a.UserArticles.FirstOrDefault(u => u.Deleted == true && u.UserId==userId) == null)
                        .OrderBy(a => a.SiteId)
                        .GroupBy(a => a.SiteId)
                        .ToList();
                }
                foreach (var site in dbArticles)
                {
                    var articles = new List<ArticleViewModel>();
                    foreach (var article in site.OrderByDescending(t => t.Id).Take(_partSize))
                    {
                        articles.Add(
                            new ArticleViewModel
                            {
                                Link = article.Url,
                                Title = article.Title,
                                PartContent = article.PartContent,
                                FullContent = article.Content,
                                Id = article.Id
                            });
                    }
                    sites.Add(
                        new SiteViewModel
                        {
                            Articles = articles,
                            IdLastArticle = site.OrderByDescending(t => t.Id).Take(_partSize).Last().Id,
                            NameSite = site.FirstOrDefault(t => t.SiteId == t.Site.Id).Site.Name,
                            SiteId = site.FirstOrDefault().SiteId
                        });
                }
            }
            return sites;
        }

        [HttpGet("[action]")]
        public SiteViewModel getMoreArticles(int idLastArticle, int siteId)
        {
            var siteArticles = new SiteViewModel();
            IQueryable<Article> dbArticles;
            using (_context)
            {
                var userSitesIds = _context.Users.Include(u => u.UserSites).FirstOrDefault().UserSites.Select(s => s.SiteId);
                var showArticle = _context.Users.FirstOrDefault().ViewSetting;
                var userId = _context.Users.FirstOrDefault().Id;

                if (showArticle == false)
                {
                    dbArticles = _context.Articles
                        .Include(a => a.UserArticles)
                        .Where(a => a.SiteId==siteId)
                        .Where(a => a.UserArticles.FirstOrDefault(f => f.UserId == userId) == null)
                        .Where(a=>a.Id<idLastArticle)
                        .OrderByDescending(a => a.Id)
                        .Take(_partSize);
                }
                else
                {
                    dbArticles = _context.Articles
                        .Include(a => a.UserArticles)
                        .Where(a => a.SiteId == siteId)
                        .Where(a => a.Id < idLastArticle)
                        .Where(a => a.UserArticles.FirstOrDefault(u => u.Deleted == true && u.UserId==userId) == null)
                        .OrderByDescending(a => a.Id)
                        .Take(_partSize);
                }
                foreach (var article in dbArticles)
                {
                    siteArticles.Articles.Add(
                        new ArticleViewModel
                        {
                            Link = article.Url,
                            Title = article.Title,
                            PartContent = article.PartContent,
                            FullContent = article.Content,
                            Id = article.Id
                        });
                }
                if (dbArticles.Count() > 0)
                {
                    siteArticles.IdLastArticle = dbArticles.Last().Id;
                }
            }
            return siteArticles;
        }
    }
}