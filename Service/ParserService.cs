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
        ArticlesRefresher articlesRefresher;
        const int interval = 1000;
        public ParserService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;           
        }

        protected override void OnStart(string[] args)
        {
            articlesRefresher = new ArticlesRefresher();    
            var crudArticlesThread = new Thread(new ThreadStart(articlesRefresher.Start));
            crudArticlesThread.Start();
        }

        protected override void OnStop()
        {
            articlesRefresher.Stop();
            //waiting 1 seconds
            Thread.Sleep(interval);
        }
    }
}
