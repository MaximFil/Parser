using System;
using AngleSharp.Dom;
using AngleSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.DAL.Entities;
using Parser;

namespace Service
{
    public class ParseHelper
    {
        public async Task<List<Article>> GetTitlesHabr()
        {
            string сontent;
            List<Article> articles = new List<Article>();
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
                    articles.Add(new Article
                    {
                        Title = item.Text(),
                        Url = item.GetAttribute("href"),
                        PartContent = GetContent(сontent),
                        Content = сontent
                    });
                }
                catch (Exception ex)
                {
                    display(ex.Message);
                }             
            }
            return articles;           
        }

        public async Task<List<Article>> GetTitlesTutBy()
        {
            var i = 0;
            List<Article> articles = new List<Article>();
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
                        document = await BrowsingContext.New(config).OpenAsync(item_link[j].GetAttribute("href").ToString());
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
                                articles.Add(new Article
                                {
                                    Title = article.Text(),
                                    PartContent = GetContent(parttext),
                                    Url = item_link[j].GetAttribute("href"),
                                    Content = fulltext
                                });
                            }
                            catch (Exception ex)
                            {
                                display(ex.Message);
                            }
                            i++;
                            if (i == 20) { return articles; }
                        }
                    }
                }
            }
            return articles;
        }

        public async Task<List<Article>> GetTitlesBelta()
        {
            List<Article> articles = new List<Article>();
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync("https://www.belta.by/all_news/");
            var items = document.QuerySelectorAll("div.lenta_info");
            string partContent, link, content;
            for (int i = 0; i < 20; i++)
            {
                link = items[i].QuerySelector("a.lenta_info_title").GetAttribute("href");
                if (!link.Contains("https://www.belta.by"))
                {
                    link = "https://www.belta.by" + link;
                }
                document = await BrowsingContext.New(config).OpenAsync(link);
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
                    items[i].QuerySelector("div.lenta_textsmall").Text();
                    partContent = items[i].QuerySelector("div.lenta_textsmall").Text();
                }
                catch (System.ArgumentNullException)
                {
                    partContent = content;
                }
                try {
                    articles.Add(new Article
                    {
                        Title = items[i].QuerySelector("a.lenta_info_title").Text(),
                        Url = link,
                        PartContent = GetContent(partContent),
                        Content = document.QuerySelector("div.js-mediator-article").TextContent
                    });
                }
                catch (Exception ex)
                {
                    display(ex.Message);
                }
            } 
            return articles;
        }
        public async Task<List<Article>> GetArticles()
        {
            List<Article> listArticles = new List<Article>();
            var model = await GetTitlesHabr();
            listArticles.AddRange(model);
            model = await GetTitlesTutBy();
            listArticles.AddRange(model);
            model = await GetTitlesBelta();
            listArticles.AddRange(model);
            for(int i = 0; i < listArticles.Count; i++)
            {
                display(listArticles.ElementAt(i).ToString());
            }
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
