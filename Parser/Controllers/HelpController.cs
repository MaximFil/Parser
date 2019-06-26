using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.DAL.Entities;
using Parser.Repository.Repositories;
using Parser.ViewModels;
using System.Drawing;

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
                var userSitesIds = _userRepository.GetUserSitesIds();
                var sites = _siteRepository.GetSites().ToList();
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
                    var article = _userArticleRepository.GetUserArticle(userId, articleId);
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
                                    UserId = _userRepository.GetUserId(),
                                    SiteId = _siteRepository.GetSiteidForNameSite(nameSite.NameSite)
                                });
                        }
                    }
                    _userRepository.SetShowArticle(showArticles);
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
                    var userArticle = _userArticleRepository.GetUserArticle(idArticle);
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
                        _userArticleRepository.SetDeletedForArticleId(true,idArticle);
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
                if (showArticle)
                {
                    article = _articleRepository.GetShowArticle(idSite,idLastArticle, userId);                 
                }
                else
                {
                    article = _articleRepository.GetAllArticle(idSite, idLastArticle, userId);
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