using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Parser.ViewModels;
using Parser.DAL.Entities;
using Parser.DAL;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading;
using Parser.Repository.Repositories;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class ParserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly ArticleRepository _articleRepository;
        const int _partSize= 9;

        public ParserController(ApplicationDbContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
            _articleRepository = new ArticleRepository(_context);
        }

        [HttpGet("[action]")]
        public List<SiteViewModel> GetSites()
        {
            var sites = new List<SiteViewModel>();
            IEnumerable<IGrouping<int, Article>> siteArticles;
            using (_context)
            {
                var userSitesIds = _userRepository.GetUserSitesIds();
                var showArticle = _userRepository.GetUserViewSetting();
                var userId = _userRepository.GetUserId();
                var allArticles = _articleRepository.GetArticles();
                if (showArticle)
                {
                    siteArticles = allArticles
                        .Include(a => a.UserArticles)
                        .Include(a => a.Site)
                        .Where(a => userSitesIds.Contains(a.SiteId))
                        .Where(a => a.UserArticles.FirstOrDefault(u => u.Deleted == true && u.UserId == userId) == null)
                        .OrderBy(a => a.SiteId)
                        .GroupBy(a => a.SiteId)
                        .ToList();
                }
                else
                {
                    siteArticles = allArticles
                        .Include(a => a.UserArticles)
                        .Include(a => a.Site)
                        .Where(a => userSitesIds.Contains(a.SiteId))
                        .Where(a => a.UserArticles.FirstOrDefault(f => f.UserId == userId) == null)
                        .OrderBy(a => a.SiteId)
                        .GroupBy(a => a.SiteId)
                        .ToList();
                }
                foreach (var site in siteArticles)
                {
                    var articles = new List<ArticleViewModel>();
                    foreach (var article in site.OrderByDescending(s => s.Id).Take(_partSize))
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
                var userSitesIds = _userRepository.GetUser().Include(u => u.UserSites).FirstOrDefault().UserSites.Select(s => s.SiteId);
                var showArticle = _userRepository.GetUserViewSetting();
                var userId = _userRepository.GetUserId();
                var articles = _articleRepository.GetArticles();

                if (showArticle == false)
                {
                    dbArticles = articles
                        .Include(a => a.UserArticles)
                        .Where(a => a.SiteId==siteId)
                        .Where(a => a.UserArticles.FirstOrDefault(f => f.UserId == userId) == null)
                        .Where(a=>a.Id<idLastArticle)
                        .OrderByDescending(a => a.Id)
                        .Take(_partSize);
                }
                else
                {
                    dbArticles = articles
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