using System;
using AngleSharp.Dom;
using AngleSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.DAL.ViewModels;

namespace Service.Controllers
{
    public class Parse
    {
        public async Task<ListArticlesViewModel> GetTitlesHabr()
        {
            int i = 0;
            string сontent;
            ListArticlesViewModel listModels = new ListArticlesViewModel();
            listModels.listArticles = new List<ArticlesViewModel>();
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync("https://habr.com/ru/news/");
            var items = document.QuerySelectorAll("a.post__title_link");
            foreach (var item in items)
            {
                var link = item.GetAttribute("href").ToString();
                document = await context.OpenAsync(link);
                сontent = document.QuerySelector("div.post__text").TextContent.ToString();
                try { 
                listModels.listArticles.Add(new ArticlesViewModel
                {
                    Article = item.Text(),
                    Link = item.GetAttribute("href"),
                    FullContent = сontent,
                    PartContent = GetContent(сontent)
                });
                }
                catch (Exception ex)
                {
                    display(ex.Message);
                }
               
                i++;
            }
            return listModels;
        }

        public async Task<ListArticlesViewModel> GetTitlesTutBy()
        {
            var i = 0;
            ListArticlesViewModel listModels = new ListArticlesViewModel();
            listModels.listArticles = new List<ArticlesViewModel>();
            //ListModels listModels = new ListModels();
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync("https://news.tut.by/?sort=time");
            var Items = document.QuerySelectorAll("div.m-sorted");
            string parttext = "";
            if (Items != null)
            {
                for (int k = 0; k < Items.Length; k++)
                {
                    var item_link = Items[k].QuerySelectorAll("a.entry__link");
                    for (int j = 0; j < item_link.Length; j += +2)
                    {

                        //переходит на статью по ссылке
                        document = await BrowsingContext.New(config).OpenAsync(item_link[j].GetAttribute("href").ToString());
                        //получает DOM объект в котором хранится заголовок

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
                            try
                            {
                                listModels.listArticles.Add(new ArticlesViewModel
                                {
                                    Article = article.Text(),
                                    Link = item_link[j].GetAttribute("href"),
                                    FullContent = fulltext,
                                    PartContent = GetContent(parttext)
                                });
                            }
                            catch (Exception ex)
                            {
                                display(ex.Message);
                            }
                            i++;
                            if (i == 20) { return listModels; }
                        }
                    }
                }
            }
            return listModels;
        }

        public async Task<ListArticlesViewModel> GetTitlesBelta()
        {
            ListArticlesViewModel listModels = new ListArticlesViewModel();
            listModels.listArticles = new List<ArticlesViewModel>();
            //ListModels listModels = new ListModels();
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync("https://www.belta.by/all_news/");
            var items = document.QuerySelectorAll("div.lenta_info");
            string partContent, link, content;
            for (int i = 0; i < 20; i++)
            {
                //ссылка
                link = items[i].QuerySelector("a.lenta_info_title").GetAttribute("href");
                if (!link.Contains("https://www.belta.by"))
                {
                    link = "https://www.belta.by" + link;
                }
                document = await BrowsingContext.New(config).OpenAsync(link);
                //содеражание статьи
                try
                {
                    content = document.QuerySelector("div.js-mediator-article").TextContent;
                }
                catch (Exception ex)
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
                try { 
                listModels.listArticles.Add(new ArticlesViewModel
                {
                    Article = items[i].QuerySelector("a.lenta_info_title").Text(),
                    Link = link,
                    FullContent = document.QuerySelector("div.js-mediator-article").TextContent,
                    PartContent = GetContent(partContent)
                });
                }
                catch (Exception ex)
                {
                    display(ex.Message);
                }
            }
            return listModels;
        }
        public async Task<List<ListArticlesViewModel>> GetArticles()
        {
            List<ListArticlesViewModel> listArticles = new List<ListArticlesViewModel>();
            var model = await GetTitlesHabr();
            listArticles.Add(model);
            model = await GetTitlesTutBy();
            listArticles.Add(model);
            model = await GetTitlesBelta();
            listArticles.Add(model);
            return listArticles;
        }
        private string GetContent(string Content)
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
        private void display(string str)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"D:\Text.txt", true);
            writer.WriteLine("\n" + DateTime.Now.ToString() + str);
            writer.Close();
        }

    }
}
