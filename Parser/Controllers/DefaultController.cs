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
        public ArticleLink[] GetTitleArticles()
        {
            int i = 0;
            ArticleLink[] articleLinks;// = new ArticleLink[9];
            var config = Configuration.Default.WithDefaultLoader();
            var URL = "https://habr.com/ru/news/";
            var document = BrowsingContext.New(config).OpenAsync(URL).Result;
            var Items = document.QuerySelectorAll("a.post__title_link");
            articleLinks = new ArticleLink[Items.Count()];
            foreach (var item in Items)
            {
                document = BrowsingContext.New(config).OpenAsync(item.GetAttribute("href").ToString()).Result;
                articleLinks[i] = new ArticleLink { Article = item.Text(), Link = item.GetAttribute("href"),Content=GetContent(document) };
                i++;
            }
            return articleLinks;
        }
        public string GetContent(IDocument document)
        {
            int count = 100;
            var Content = document.QuerySelector("div.post__text").TextContent.ToString();
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
            public string Content { get; set; }
        }
    }
}