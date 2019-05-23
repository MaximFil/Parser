using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class ArticlesRefresher
    {
        static bool enabled;
        const int interval = 100000;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly string _connectionString;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly string _file;
        public ArticlesRefresher()
        {
            
            enabled = true;
            _dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            _connectionString = ConfigurationManager.
                ConnectionStrings["Context"].ConnectionString;
            _options = _dbContextOptionsBuilder.UseSqlServer(_connectionString).Options;
            _file = ConfigurationManager.AppSettings["ArticlesFile"];
        }

        public async void Start()
        {
            display("\n" + DateTime.Now.ToString() + "Begin");
            while (enabled)
            {
                display("\n" + DateTime.Now.ToString() + "Start");
                try
                {
                    await Create();
                }
                catch (Exception ex)
                {
                    display("\n" + DateTime.Now.ToString() + "Error" + ex.Message);
                }
                display("\n" + DateTime.Now.ToString() + "End");
                //waiting 5 minutes
                Thread.Sleep(interval);
            }
        }
        public void Stop()
        {
            enabled = false;
        }

        public async Task Create()
        {
            display("Method Create begin working");
            var parse = new ParserHelper();
            display("Method Create begin pars articles");
            var articles = await parse.GetArticles();
            display("Method Create finish pars articles");
            display("Method Create begin refresh articles in database");
            using (var context = new ApplicationDbContext(_options))
            {
                foreach (var article in articles)
                {
                    var dbArticle = context.Articles.FirstOrDefault(a => a.Url == article.Url);
                    if (dbArticle == null)
                    {
                        await context.Articles.AddAsync(article);
                    }
                }
                await context.SaveChangesAsync();
            }
            display("Method Create finish refresh articles in database");
        }

        public void display(string str)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(_file, true);
            writer.WriteLine("\n" + DateTime.Now.ToString() + str);
            writer.Close();
        }
    }
}
