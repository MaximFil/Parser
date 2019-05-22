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

namespace Service
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ParserService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}