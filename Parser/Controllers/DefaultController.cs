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
using Parser.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using AngleSharp.Html.Parser;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        private URLs urls { get; set; }
        public DefaultController(IOptions<URLs> options)
        {
            urls = options.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<List<ViewModel>> GetTitlesHabr()
        {
            
            var i = 0;
            string сontent;
            //ListModels listModels = new ListModels();
            List<ViewModel> listModels = new List<ViewModel>();
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //1.7
            var document = await context.OpenAsync(urls.Habr);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            watch.Restart();
            //0.033
            var items = document.QuerySelectorAll("a.post__title_link");
            elapsedMs = watch.ElapsedMilliseconds;
           
            
            foreach (var item in items)
            {
                watch.Restart();
                //0.002
                var link = item.GetAttribute("href").ToString();
                elapsedMs = watch.ElapsedMilliseconds;

                watch.Restart();
                //0.439
                document = await context.OpenAsync(link);
                elapsedMs = watch.ElapsedMilliseconds;
                
                watch.Restart();
                //0.002
                сontent = document.QuerySelector("div.post__text").TextContent.ToString();
                elapsedMs = watch.ElapsedMilliseconds;

                watch.Restart();
                //0.001
                listModels.Add(new ViewModel {
                    Article = item.Text(),
                    Link = item.GetAttribute("href"),
                    FullContent = сontent,
                    PartContent =GetContent(сontent)
                });
                elapsedMs = watch.ElapsedMilliseconds;
                i++;
            }
            return listModels;
        }

        [HttpGet("[action]")]
        public async Task<List<ViewModel>> GetTitlesTutBy()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var i = 0;
            List<ViewModel> listModels = new List<ViewModel>();
            //ListModels listModels = new ListModels();
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(urls.TutBy);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            watch.Restart();
            var Items = document.QuerySelectorAll("div.m-sorted");
            elapsedMs=watch.ElapsedMilliseconds;

            string parttext = "";
            if (Items != null)
            {
                for (int k = 0; k < Items.Length; k++)
                {
                    var item_link = Items[k].QuerySelectorAll("a.entry__link");
                    for (int j = 0; j<item_link.Length; j += +2)
                    {
                        watch.Restart();
                        //переходит на статью по ссылке
                        document = await BrowsingContext.New(config).OpenAsync(item_link[j].GetAttribute("href").ToString());
                        //получает DOM объект в котором хранится заголовок
                        elapsedMs = watch.ElapsedMilliseconds;
                        watch.Restart();
                        var article = document.QuerySelector("h1");
                        var blockContent = document.QuerySelector("div.js-mediator-article");
                        if (blockContent != null)
                        {
                            var full = blockContent.QuerySelectorAll("p");
                            var h = 0;
                            var fulltext = "";
                            foreach (var text in full)
                            {
                                if (h == 0) { parttext = text.Text(); h++; }
                                fulltext += (text.Text() + "\n");
                            }
                            listModels.Add(new ViewModel
                            {
                                Article = article.Text(),
                                Link = item_link[j].GetAttribute("href"),
                                FullContent =fulltext,
                                PartContent =GetContent(parttext)
                            });
                         i++;
                            if (i == 20) { return listModels; }
                        }
                        elapsedMs = watch.ElapsedMilliseconds;
                    }
                }
            }
            return listModels;
        }

        [HttpGet("[action]")]
        public async Task<List<ViewModel>> GetTitlesBelta()
        {
            List<ViewModel> listModels = new List<ViewModel>();
            //ListModels listModels = new ListModels();
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(urls.Belta);
            var items = document.QuerySelectorAll("div.lenta_info");
            string partContent, link,content;
            for (int i=0;i<20;i++)
            {
                //ссылка
                link = items[i].QuerySelector("a.lenta_info_title").GetAttribute("href");
                if (!link.Contains("https://www.belta.by"))
                {
                    link = "https://www.belta.by" + link;
                }
                document = await BrowsingContext.New(config).OpenAsync(link);
                //содеражание статьи
                try { 
                content = document.QuerySelector("div.js-mediator-article").TextContent;
                }
                catch(Exception ex)
                {
                    continue;
                }
                try
                {
                    //краткое описание
                    items[i].QuerySelector("div.lenta_textsmall").Text();
                    partContent = items[i].QuerySelector("div.lenta_textsmall").Text();
                }
                catch (System.ArgumentNullException)
                {
                    partContent = content;
                }
                listModels.Add(new ViewModel
                {
                    Article = items[i].QuerySelector("a.lenta_info_title").Text(),
                    Link = link,
                    FullContent = document.QuerySelector("div.js-mediator-article").TextContent,
                    PartContent = GetContent(partContent) });
            }
            return listModels;
        }

        [HttpGet("[action]")]
        public async Task<List<List<ViewModel>>> GetTitles()
        {
            List<List<ViewModel>> listModels = new List<List<ViewModel>>();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var model = await GetTitlesHabr();
            listModels.Add(model);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            watch.Restart();
            model = await GetTitlesTutBy();
            listModels.Add(model);
            elapsedMs = watch.ElapsedMilliseconds;

            watch.Restart();
            model = await GetTitlesBelta();
            listModels.Add(model);
            elapsedMs = watch.ElapsedMilliseconds;

            return listModels;
        }

        public string GetContent(string Content)
        {
            int count = 100;
            string str;
            if (Content.Length > 100)
            {
                while (Content.Substring(count, 1) != " " && Content.Substring(count, 1) != ".")
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
    }
}