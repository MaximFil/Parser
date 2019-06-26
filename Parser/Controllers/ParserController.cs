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
                if (showArticle)
                {
                    siteArticles = _articleRepository.GetShowArticlesUser(userSitesIds,userId).ToList();                       
                }
                else
                {
                    siteArticles = _articleRepository.GetAllArticlesUser(userSitesIds,userId).ToList();
                }
                foreach (var site in siteArticles)
                {
                    var articles = new List<ArticleViewModel>();
                    var _site = _articleRepository.GetPartArticlesSite(site, _partSize);
                    foreach (var article in _site)
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
                            IdLastArticle = _articleRepository.GetLastId(site,_partSize),
                            NameSite = _articleRepository.GetSiteName(site),
                            SiteId = _articleRepository.GetSiteId(site)
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
                var userSitesIds = _userRepository.GetUserSitesIds();
                var showArticle = _userRepository.GetUserViewSetting();
                var userId = _userRepository.GetUserId();
                var articles = _articleRepository.GetArticles();

                if (showArticle)
                {
                    dbArticles = _articleRepository.GetMoreShowArticles(siteId, idLastArticle, _partSize, userId);                 
                }
                else
                {
                    dbArticles = _articleRepository.GetMoreAllArticles(siteId, idLastArticle, _partSize, userId);
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
                    siteArticles.IdLastArticle = _articleRepository.GetMoreLastId(dbArticles);
                }
            }
            return siteArticles;
        }
    }
}