using Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public partial class Service1 : ServiceBase
    {
        Class1 crudArticles;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            crudArticles = new Class1();
            Thread crudArticlesThread = new Thread(new ThreadStart(crudArticles.Start));
            crudArticlesThread.Start();
        }

        protected override void OnStop()
        {
            crudArticles.Stop();
            Thread.Sleep(1000);
        }
    }
    class Class1
    {
        static bool enabled;
        public Class1()
        {
            enabled = true;
        }
        public void Start()
        {

            display("\n" + DateTime.Now.ToString() + "BeginBegin");
            using (ApplicationContext context = new ApplicationContext())
            {
                User user1 = new User { Name = "Maxim", Age = 18 };
                context.Users.Add(user1);
                context.SaveChanges();
            }
            while (enabled)
            {
                display("\n" + DateTime.Now.ToString() + "StartStart");
                System.Threading.Thread.Sleep(1000);
                // Create();
                display("\n" + DateTime.Now.ToString() + "EndEnd");
            }
        }
        public void Stop()
        {
            enabled = false;
        }

        public void display(string str)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"D:\Text1.txt", true);
            writer.WriteLine(str);
            writer.Close();
        }
    }
    }
