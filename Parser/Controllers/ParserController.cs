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
            using (_context)
            {
                var dbArticles = _context.Articles.OrderByDescending(t=>t.Id).GroupBy(t => t.SiteId);
                foreach (var site in dbArticles)
                {
                    var articles = new List<ArticleViewModel>();
                    foreach (var article in site.Take(_partSize))
                    {
                        articles.Add(
                            new ArticleViewModel
                            {
                                Link = article.Url,
                                Title = article.Title,
                                PartContent = article.PartContent,
                                FullContent = article.Content
                            });
                    }
                    siteArticles.Add(new SiteViewModel { Articles = articles,IdLastArticle=site.Take(_partSize).Last().Id, PartNumber=0 });
                }
            }
            return siteArticles;
        }

        [HttpGet("[action]")]
        public SiteViewModel GetPartSites(int idLastArticle, int siteNumber)
        {
            var siteArticles = new SiteViewModel();
            using (_context)
            {
                var dbArticles = _context.Articles.OrderByDescending(t => t.Id).Where(t => t.SiteId == siteNumber && t.Id < idLastArticle).Take(_partSize);
                foreach (var article in dbArticles)
                {
                    siteArticles.Articles.Add(
                        new ArticleViewModel
                        {
                            Link = article.Url,
                            Title = article.Title,
                            PartContent = article.PartContent,
                            FullContent = article.Content
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