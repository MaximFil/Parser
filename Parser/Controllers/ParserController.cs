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
        private static int[] _partArticles;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly string _connectionString;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IConfigurationRoot _configurationRoot;

        public ParserController() : base()
        {
            _configurationRoot = ConfigurationHelper.GetConfiguration(Directory.GetCurrentDirectory());
            _connectionString = _configurationRoot.GetConnectionString("ParserDB");
            _dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            _options = _dbContextOptionsBuilder.UseSqlServer(_connectionString).Options;
        }

        static ParserController()
        {
            _partArticles=new int[4] { 0, 0, 0, 0 };
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public List<SiteViewModel> GetSites(int numberPart=3)
        {
            _partArticles[numberPart]++;
            var counter = 0;
            var siteArticles = new List<SiteViewModel>();
            using (var context = new ApplicationDbContext(_options))
            {
               var dbArticles = context.Articles.GroupBy(t=>t.SiteId);
               string srt= dbArticles.GetType().ToString();
                foreach (var site in dbArticles )
                {
                    var articles = new List<ArticleViewModel>();
                    foreach (var article in site)
                    {
                        articles.Add(new ArticleViewModel { Link = article.Url, Title = article.Title, PartContent = article.PartContent, FullContent = article.Content });
                        if (articles.Count() == _partArticles[counter]*9+9)
                        {
                            break;
                        }
                    }
                    counter++;
                    siteArticles.Add(new SiteViewModel { Articles=articles});
                }
            }           
            return siteArticles;
        }

        public void Display(string str)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter("D:\\Test.txt", true);
            writer.WriteLine("\n" + DateTime.Now.ToString() + str);
            writer.Close();
        }
    }
}