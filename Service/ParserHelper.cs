using System;
using AngleSharp.Dom;
using AngleSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser.DAL.Entities;
using System.Configuration;

namespace Service
{
    public class ParserHelper
    {
        private readonly string _urlHabr;
        private readonly string _urlTutBy;
        private readonly string _urlBelta;
        private readonly string _file;
        private readonly string _domainBelta;

        public ParserHelper()
        {
            _urlHabr = ConfigurationManager.AppSettings["UrlHabr"];
            _urlTutBy = ConfigurationManager.AppSettings["UrlTutBy"];
            _urlBelta = ConfigurationManager.AppSettings["UrlBelta"];
            _domainBelta = ConfigurationManager.AppSettings["DomainBelta"];
            _file = ConfigurationManager.AppSettings["ParsingFile"];
        }

        public async Task<List<Article>> GetHabrArticles()
        {
            string сontent;
            var articles = new List<Article>();
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(_urlHabr);
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
                        SiteId=1,
                        Url = item.GetAttribute("href"),
                        PartContent = GetShortenedArticleDescription(сontent),
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
            var document = await BrowsingContext.New(config).OpenAsync(_urlTutBy);
            var items = document.QuerySelectorAll("div.m-sorted");
            var parttext = "";
            if (items != null)
            {
                for (int k = 0; k < items.Length; k++)
                {
                    var item_link = items[k].QuerySelectorAll("a.entry__link");
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
                                    SiteId=2,
                                    PartContent = GetShortenedArticleDescription(parttext),
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
            var document = await BrowsingContext.New(config).OpenAsync(_urlBelta);
            var items = document.QuerySelectorAll("div.lenta_info");
            display("Get DOM object");
            string partContent, link, content;
            foreach (var item in items)
            {
                link = item.QuerySelector("a.lenta_info_title").GetAttribute("href");
                if (!link.Contains(_domainBelta))
                {
                    link = _domainBelta + link;
                }
                document = await BrowsingContext.New(config).OpenAsync(link);
                try
                {
                    content = document.QuerySelector("div.js-mediator-article").TextContent;
                }
                catch (Exception ex)
                {
                    display(ex.Message);
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
                try
                {
                    articles.Add(new Article
                    {
                        Title = item.QuerySelector("a.lenta_info_title").Text(),
                        SiteId = 3,
                        Url = link,
                        PartContent = GetShortenedArticleDescription(partContent),
                        Content = document.QuerySelector("div.js-mediator-article").TextContent
                    }) ;
                }
                catch (Exception ex)
                {
                    display(ex.Message);
                }
                display(link);
            } 
            return articles;
        }

        public async Task<List<Article>> GetArticles()
        {
            display("Begin");
            var articles = new List<Article>();
            display("Begin habr parsing");
            var model = await GetHabrArticles();
            display("Finish habr parsing");
            articles.AddRange(model);
            display("Begin TutBy parsing");
            model = await GetTutByArticles();
            display("Finish TutBy parsing");
            articles.AddRange(model);
            display("Begin belta parsing");
            model = await GetBeltaArticles();
            display("Finish belta parsing");
            articles.AddRange(model);
            for(int i = 0; i < articles.Count; i++)
            {
                display(articles.ElementAt(i).ToString());
            }
            display("All articles are collected");
            return articles;
        }

        private string GetShortenedArticleDescription(string сontent)
        {
            var count = 100;
            string str;
            if (сontent.Length > 100)
            {
                while (сontent.Substring(count, 1) != " " && сontent.Substring(count, 1) != ".")
                {
                    count++;
                }

                str = сontent.Substring(0, count);
            }
            else
            {
                str = сontent;
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
