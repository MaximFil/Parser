using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parser.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Parser.DAL;

namespace Service
{
    public partial class ParserService : ServiceBase
    {
        CrudArticles crudArticles;
        public ParserService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;           
        }

        protected override void OnStart(string[] args)
        {
            crudArticles = new CrudArticles();    
            Thread crudArticlesThread = new Thread(new ThreadStart(crudArticles.Start));
            crudArticlesThread.Start();
        }

        protected override void OnStop()
        {
            crudArticles.Stop();
            Thread.Sleep(1000);
        }
    }
    class CrudArticles
    {
        static bool enabled;
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        private string connectionString;
        public CrudArticles()
        {
            enabled = true;
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            connectionString = ConfigurationManager.
                ConnectionStrings["Context"].ConnectionString;
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
                Thread.Sleep(100000);
            }
        }
        public void Stop()
        {
            enabled = false;
        }

        public async Task Create()
        {
            display("Метод креате начал работу");
            ParseHelper parse = new ParseHelper();
            display("Метод креате начал сбор новостей");
            var listArticles = await parse.GetArticles();
            display("Метод креате закончил сбор новостей");
            display("Метод креате начинает запись в бд статей");
            bool check = false;          
            optionsBuilder.UseSqlServer(connectionString);
            using (ApplicationDbContext context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var articles = context.Articles.ToList();
                if (articles.Count != 0)
                {
                    foreach (var listArticle in listArticles)
                    {
                        check = false;
                        foreach (var article in articles)
                        {
                            if (article.Url == listArticle.Url)
                            {
                                check = false;
                                break;
                            }
                            else { check = true; }
                        }
                        if (check)
                        {
                            await context.Articles.AddAsync(listArticle);
                        }
                    }
                }
                else { await context.AddRangeAsync(listArticles);}                                                    
                await context.SaveChangesAsync();
            }
            display("Метод креате закончил запись статей в бд");
        }

        public void display(string str)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"D:\Text1.txt", true);
            writer.WriteLine("\n" + DateTime.Now.ToString() + str);
            writer.Close();
        }
    }
}
