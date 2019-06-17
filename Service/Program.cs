using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Parser.DAL.Entities;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using ParserService;
using Parser.DAL;
using System.IO;

namespace Service
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string connectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            DbContextOptions<ApplicationDbContext> options = dbContextOptionsBuilder.UseSqlServer(connectionString).Options;
            DefaultSites.InitializeSites(options);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ParserService(options)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}