using System;
using AngleSharp.Dom;
using AngleSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.DAL.Entities;
using Parser;
using System.Configuration;

namespace Service
{
    public class ParserHelper
    {
        private readonly string[] _sites;
        private readonly string _file;
        public ParserHelper()
        {
            _sites = ConfigurationManager.AppSettings["Sites"].Split(' ');
            _file = ConfigurationManager.AppSettings["ParsingFile"];
        }
        public async Task<List<Article>> GetHabrArticles()
        {
            string сontent;
            var articles = new List<Article>();
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(_sites[0]);
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

        public async Task<List<Article>> GetTutByArticles()
        {
            var articles = new List<Article>();
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(_sites[1]);
            var Items = document.QuerySelectorAll("div.m-sorted");
            var parttext = "";
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
                        }
                    }
                }
            }
            return articles;
        }

        public async Task<List<Article>> GetBeltaArticles()
        {
            var articles = new List<Article>();
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(_sites[2]);
            var items = document.QuerySelectorAll("div.lenta_info");
            string partContent, link, content;
            foreach (var item in items)
            {
                link = item.QuerySelector("a.lenta_info_title").GetAttribute("href");
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
                    item.QuerySelector("div.lenta_textsmall").Text();
                    partContent = item.QuerySelector("div.lenta_textsmall").Text();
                }
                catch (System.ArgumentNullException)
                {
                    partContent = content;
                }
                try {
                    articles.Add(new Article
                    {
                        Title = item.QuerySelector("a.lenta_info_title").Text(),
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
            display("Begin");
            var listArticles = new List<Article>();
            display("Begin habr pars");
            var model = await GetHabrArticles();
            display("Finish habr pars");
            listArticles.AddRange(model);
            display("Begin TutBy pars");
            model = await GetTutByArticles();
            display("Finish TutBy pars");
            listArticles.AddRange(model);
            display("Begin belta pars");
            model = await GetBeltaArticles();
            display("Finish belta pars");
            listArticles.AddRange(model);
            for(int i = 0; i < listArticles.Count; i++)
            {
                display(listArticles.ElementAt(i).ToString());
            }
            display("Collected all articles");
            return listArticles;
        }
        private string GetContent(string Content)
        {
            var count = 100;
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
            System.IO.StreamWriter writer = new System.IO.StreamWriter(_file, true);
            writer.WriteLine("\n" + DateTime.Now.ToString() + str);
            writer.Close();
        }

    }
}
