using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parser.Models;

namespace Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (Context context=new Context())
            //{
            //    User user1 = new User { Id = 0, FirstName = "Maxim", LastName = "Fil", DateSetting = DateTime.Now, ViewSetting = 2 };
            //    User user2 = new User { Id=1, FirstName = "Misha", LastName = "Mil", DateSetting = DateTime.Now, ViewSetting = 5 };

            //    context.DbUser.Add(user1);
            //    context.DbUser.Add(user2);
            //    context.SaveChanges();

            //    var users = context.DbUser.ToList();
            //    foreach(User u in users)
            //    {
            //        Console.WriteLine($"{u.Id}   {u.FirstName}  {u.LastName}  {u.DateSetting}  {u.ViewSetting}");
            //    }

            //}
            //Console.ReadLine();
            Context context = new Context();
                CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
