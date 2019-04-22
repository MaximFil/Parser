using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parser.Models;

namespace Parser.Controllers
{
   
    public class DefaultController : Controller
    {
        Context context = new Context();
        public IActionResult Index()
        {
            return View();
        }

        public void UpdateUsersTable(int id)
        {
            ViewBag.Id = id;
            var UpdateUsers = context.DbUser.Where(c => c.Id == id).FirstOrDefault();
            ViewBag.FirstName = UpdateUsers.FirstName;
            ViewBag.LastName = UpdateUsers.LastName;
            ViewBag.DateSetting = UpdateUsers.DateSetting;
            ViewBag.ViewSetting = UpdateUsers.ViewSetting;
        }
        public void UpdateSitesTable(int id)
        {
            ViewBag.Id = id;
            var UpdateSites = context.DbSite.Where(c => c.Id == id).FirstOrDefault();
            ViewBag.Name = UpdateSites.Name;
            ViewBag.Domain = UpdateSites.Domain;
        }
    }
}