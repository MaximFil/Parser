using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using AngleSharp;
using AngleSharp.Dom;
using Parser.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using AngleSharp.Html.Parser;
using Parser.DAL.Entities;
using Parser.DAL;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
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
            var siteArticles = new List<SiteViewModel>();
            IEnumerable<IGrouping<int, Article>> dbArticles;
            using (_context)
            {
                var showArticle = _context.Users.FirstOrDefault().ViewSetting;
                if (showArticle == false)
                {
                    dbArticles = _context.Articles.Include(t => t.Site.UserSites).OrderBy(t => t.SiteId).Where(t => t.SiteId == t.Site.UserSites.FirstOrDefault(f => f.SiteId == t.SiteId).SiteId).Where(t => t.UserArticles.FirstOrDefault(f => f.ArticleId == t.Id).Deleted == false).ToList().GroupBy(t => t.SiteId);
                }
                else
                {
                dbArticles = _context.Articles.Include(t=>t.Site.UserSites).OrderBy(t => t.SiteId).Where(t => t.SiteId == t.Site.UserSites.FirstOrDefault(f => f.SiteId == t.SiteId).SiteId).ToList().GroupBy(t=>t.SiteId);
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
                    siteArticles.Add(
                        new SiteViewModel
                        {
                            Articles = articles,
                            IdLastArticle = site.OrderByDescending(t => t.Id).Take(_partSize).Last().Id,
                            NameSite = site.FirstOrDefault(t => t.SiteId == t.Site.Id).Site.Name,
                            SiteId = site.FirstOrDefault().SiteId
                        });
                }
            }
            return siteArticles;
        }

        [HttpGet("[action]")]
        public SiteViewModel GetPartSites(int idLastArticle, int siteNumber)
        {
            var siteArticles = new SiteViewModel();
            IQueryable<Article> dbArticles;
            using (_context)
            {
                var showArticles = _context.Users.First().ViewSetting;
                if(showArticles == false)
                {
                    dbArticles = _context.Articles.OrderByDescending(t => t.Id).Where(t => t.SiteId == siteNumber && t.Id < idLastArticle && t.UserArticles.FirstOrDefault(f=>f.ArticleId==t.Id).Deleted==false).Take(_partSize);
                }
                else
                {
                    dbArticles = _context.Articles.OrderByDescending(t => t.Id).Where(t => t.SiteId == siteNumber && t.Id < idLastArticle).Take(_partSize);
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