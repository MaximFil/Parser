using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.DAL.Entities;
using Parser.Repository.Repositories;
using Parser.ViewModels;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class HelpController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly SiteRepository _siteRepository;
        private readonly ArticleRepository _articleRepository;
        private readonly UserSiteRepository _userSiteRepository;
        private readonly UserArticleRepository _userArticleRepository;
        private readonly Repository.Repositories.Repository _repository;

        public HelpController(ApplicationDbContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
            _siteRepository = new SiteRepository(_context);
            _articleRepository = new ArticleRepository(_context);
            _userSiteRepository = new UserSiteRepository(_context);
            _userArticleRepository = new UserArticleRepository(_context);
            _repository = new Repository.Repositories.Repository(_context); 
        }

        [HttpGet("[action]")]
        public List<NameSiteViewModel> GetNameSitesUser()
        {
            var nameSitesUser = new List<NameSiteViewModel>();
            using (_context)
            {
                var userSitesIds = _userRepository.GetUser()
                    .Include(u => u.UserSites)
                    .FirstOrDefault()
                    .UserSites
                    .Select(s => s.SiteId);
                var sites = _siteRepository.GetSites();
                foreach (var site in sites)
                {
                    var select = false;
                    foreach (var item in userSitesIds)
                    {
                        if (site.Id == item)
                        {
                            select = true;
                        }
                    }
                    nameSitesUser.Add(new NameSiteViewModel { NameSite = site.Name, Select = select });
                }
            }
            return nameSitesUser;
        }

        [HttpPost("[action]")]
        public IActionResult SaveShowenArticle([FromBody] int articleId)
        {
            using (_context)
            {
                var userId = _userRepository.GetUserId();
                if (ModelState.IsValid)
                {
                    var article = _userArticleRepository.GetUserArticles()
                        .FirstOrDefault(u=>u.ArticleId==articleId && u.UserId==userId);
                    if (article == null)
                    {                    
                    _userArticleRepository.AddUserArticle(
                        new UserArticle
                        {
                            UserId = userId,
                            ArticleId = articleId,
                            Deleted = false
                        });
                        _repository.SaveChanges();
                    }
                    return Ok();
                }
                return BadRequest(ModelState);
            }
        }

        [HttpGet("[action]")]
        public bool GetValueShow()
        {
            return _userRepository.GetUserViewSetting();
        }

        [HttpPost("[action]")]
        public IActionResult SaveFilters([FromBody] List<NameSiteViewModel> nameSites, bool showArticles)
        {
            using (_context)
            {
                _userSiteRepository.RemoveRange();
                if (ModelState.IsValid)
                {
                    foreach (var nameSite in nameSites)
                    {
                        if (nameSite.Select == true)
                        {
                            _userSiteRepository.Add(
                                new UserSite
                                {
                                    UserId = _context.Users.First().Id,
                                    SiteId = _context.Sites.FirstOrDefault(t => t.Name == nameSite.NameSite).Id
                                });
                        }
                    }
                    _userRepository.GetUser().First().ViewSetting = showArticles;
                    _repository.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public IActionResult DeleteArticle([FromBody]int idArticle)
        {
            using (_context)
            {
                if (ModelState.IsValid)
                {              
                    var userArticle = _userArticleRepository.GetUserArticles().FirstOrDefault(u => u.ArticleId == idArticle);
                    if (userArticle==null)
                    {
                        _userArticleRepository.AddUserArticle(
                            new UserArticle
                            {
                                UserId = _context.Users.First().Id,
                                ArticleId = idArticle,
                                Deleted = true
                            });
                    }
                    else
                    {
                        _userArticleRepository.GetUserArticles().First(u => u.ArticleId == idArticle).Deleted = true;
                    }
                    _repository.SaveChanges();
                    return Ok();
                }            
            }
            return BadRequest(ModelState);
        }

        [HttpGet("[action]")]
        public ArticleViewModel GetArticle(int idLastArticle, int idSite)
        {
            var articleSite = new ArticleViewModel();
            Article article;
            using (_context)
            {
                var showArticle = _userRepository.GetUserViewSetting();
                var userId = _userRepository.GetUserId();
                if (showArticle == false)
                {
                    article = _articleRepository.GetArticles()
                        .Include(a => a.UserArticles)
                        .Where(a => a.SiteId == idSite)
                        .Where(a => a.UserArticles.FirstOrDefault(u => u.UserId == userId) == null)
                        .Where(a => a.Id < idLastArticle)
                        .OrderByDescending(a=>a.Id)
                        .FirstOrDefault();
                }
                else
                {
                    article = _articleRepository.GetArticles()
                        .Where(a => a.SiteId == idSite)
                        .Where(a => a.Id < idLastArticle)
                        .Where(a => a.UserArticles.FirstOrDefault(u => u.Deleted == true && u.UserId==userId) == null)
                        .OrderByDescending(a => a.Id)
                        .FirstOrDefault();
                }
                if (article!=null)
                {
                    articleSite =
                        new ArticleViewModel
                        {
                            FullContent = article.Content,
                            Link = article.Url,
                            PartContent = article.PartContent,
                            Title = article.Title,
                            Id = article.Id
                        };
                }
            }
            return articleSite;
        }
    }
}