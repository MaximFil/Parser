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
using Service.Controllers;

namespace Service
{
    public partial class Service1 : ServiceBase
    {
        CrudArticles crudArticles;
        public Service1()
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
        private DbContextOptionsBuilder<Context> optionsBuilder;
        private string connectionString;
        public CrudArticles()
        {
            enabled = true;
            optionsBuilder = new DbContextOptionsBuilder<Context>();
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
                Thread.Sleep(300000);
            }
        }
        public void Stop()
        {
            enabled = false;
        }

        public async Task Create()
        {
            display("Метод креате начал работу");
            Parse parse = new Parse();
            display("Метод креате начал сбор новостей");
            var list = await parse.GetArticles();
            display("Метод креате закончил сбор новостей");
            List<Article> articles = new List<Article>();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].listArticles.Count; j++)
                {
                    articles.Add(new Article
                    {
                        Url = list[i].listArticles[j].Link,
                        Content = list[i].listArticles[j].FullContent,
                        PartContent = list[i].listArticles[j].PartContent,
                        Title = list[i].listArticles[j].Article,
                        SiteId = 1
                    });
                    display(list[i].listArticles[j].Link + "   " + list[i].listArticles[j].FullContent + "    " + list[i].listArticles[j].PartContent + "     " + list[i].listArticles[j].Article);
                }
            }
            display("Метод креате начинает запись в бд статей");
            optionsBuilder.UseSqlServer(connectionString);
            using (Context context = new Context(optionsBuilder.Options))
            {
                await context.Articles.AddRangeAsync(articles);
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
