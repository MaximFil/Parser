using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parser.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using AngleSharp;
using AngleSharp.Dom;
namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[action]")]
        public async Task<ArticleLink[]> GetTitleArticles()
        {
            int i = 0;
            string Content;
            ArticleLink[] articleLinks;// = new ArticleLink[9];
            var config = Configuration.Default.WithDefaultLoader();
            var URL = "https://habr.com/ru/news/";
            var document = await BrowsingContext.New(config).OpenAsync(URL);
            var Items = document.QuerySelectorAll("a.post__title_link");
            articleLinks = new ArticleLink[Items.Count()];
            foreach (var item in Items)
            {
                document = await BrowsingContext.New(config).OpenAsync(item.GetAttribute("href").ToString());
                Content = document.QuerySelector("div.post__text").TextContent.ToString();
                articleLinks[i] = new ArticleLink { Article = item.Text(), Link = item.GetAttribute("href"),FullContent = Content,PartContent=GetContent(Content) };
                i++;
            }
            return articleLinks;
        }
        public string GetContent(string Content)
        {
            int count = 100;           
                while(Content.Substring(count,1)!=" ")
            {
                count++;
            }
            string str = Content.Substring(0, count);
            return str;
        }

        public class ArticleLink
        {
            public string Article { get; set; }
            public string Link { get; set; }
            public string FullContent { get; set; }
            public string PartContent { get; set; }
        }
    }
}