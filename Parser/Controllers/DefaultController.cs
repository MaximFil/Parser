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
        public async Task<ArticleLink[]> GetTitleArticlesHabr()
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
            string str;
            if (Content.Length > 100) { 
                while(Content.Substring(count,1)!=" " && Content.Substring(count, 1) != ".")
            {
                count++;
            }
            
            str = Content.Substring(0, count);
            }
            else
            {
                str = Content;
            }
            return str;
        }
        [HttpGet("[action]")]
        public async Task<ArticleLink[]> GetTitleArticlesTutBy()
        {
            int i = 0;
            ArticleLink[] articleLinks=new ArticleLink[20];
            var config = Configuration.Default.WithDefaultLoader();
            var URL = "https://news.tut.by/?sort=time";
            var document = await BrowsingContext.New(config).OpenAsync(URL);
            var Items = document.QuerySelectorAll("div.m-sorted");
            string parttext = "";
            if (Items != null)
            {
                for (int k = 0; k < Items.Length; k++)
                {
                    var item_link = Items[k].QuerySelectorAll("a.entry__link");
                    for (int j = 0; j<item_link.Length; j += +2)
                    {
                        //переходит на статью по ссылке
                        document = await BrowsingContext.New(config).OpenAsync(item_link[j].GetAttribute("href").ToString());
                        //получает DOM объект в котором хранится заголовок
                        var article = document.QuerySelector("h1");
                        var blockContent = document.QuerySelector("div.js-mediator-article");
                        if (blockContent != null)
                        {
                            var full = blockContent.QuerySelectorAll("p");
                            int h = 0;
                            string fulltext = "";
                            foreach (var text in full)
                            {
                                if (h == 0) { parttext = text.Text(); h++; }
                                fulltext += (text.Text() + "\n");
                            }
                            articleLinks[i] = new ArticleLink { Article = article.Text(), Link= item_link[j].GetAttribute("href"),
                                FullContent =fulltext,PartContent=GetContent(parttext)};
                         i++;
                            if (i == 20) { return articleLinks; }
                        }
                    }
                }
            }
            return articleLinks;
        }
        [HttpGet("[action]")]
        public async Task<ArticleLink[]> GetTitleArticlesBelta()
        {
            ArticleLink[] articleLinks = new ArticleLink[20];
            var config = Configuration.Default.WithDefaultLoader();
            var URL = "https://www.belta.by/all_news/";
            var document = await BrowsingContext.New(config).OpenAsync(URL);
            var Items = document.QuerySelectorAll("div.lenta_info");
            string partContent, link,imer;
            for (int i=0;i<20;i++)
            {
                //ссылка
                link = Items[i].QuerySelector("a.lenta_info_title").GetAttribute("href");
                if (!link.Contains("https://www.belta.by"))
                {
                    link = "https://www.belta.by" + link;
                }
                document = await BrowsingContext.New(config).OpenAsync(link);
                //содеражание статьи
                imer = document.QuerySelector("div.js-mediator-article").TextContent;
               
                try
                {
                    //краткое описание
                    Items[i].QuerySelector("div.lenta_textsmall").Text();
                    partContent = Items[i].QuerySelector("div.lenta_textsmall").Text();
                }
                catch (System.ArgumentNullException)
                {
                    partContent = imer;
                }
                articleLinks[i] = new ArticleLink { Article = Items[i].QuerySelector("a.lenta_info_title").Text(), Link = link, FullContent = document.QuerySelector("div.js-mediator-article").TextContent, PartContent = GetContent(partContent) };
            }
            return articleLinks;
        }
        [HttpGet("[action]")]
        public async Task<ArticleLinkSite[]> GetTitleArticlesSite()
        {
            int counter = 0;
        ArticleLinkSite[] articleLinksSite = new ArticleLinkSite[3];
        articleLinksSite[counter].articleLink = await GetTitleArticlesHabr();
        articleLinksSite[counter].Site = "Habr";
            counter++;
            articleLinksSite[counter].articleLink = await GetTitleArticlesTutBy();
        articleLinksSite[counter].Site = "TutBy";
            counter++;
            articleLinksSite[counter].articleLink = await GetTitleArticlesBelta();
            return articleLinksSite;
        }

    public class ArticleLink
        {
            public string Article { get; set; }
            public string Link { get; set; }
            public string FullContent { get; set; }
            public string PartContent { get; set; }
        }
        public class ArticleLinkSite
        {
            public ArticleLink[] articleLink;
            public string Site;
        }
    }
}