using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.ViewModels;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class HelpController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HelpController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public List<NameSiteViewModel> GetNameSitesUser()
        {
            var nameSitesUser = new List<NameSiteViewModel>();
            using (_context)
            {
                var userSites = _context.Sites.Where(t=>t.Id==t.UserSites.FirstOrDefault(f=>f.SiteId==t.Id).SiteId).ToList();
                var sites = _context.Sites.ToList();
                foreach (var site in sites)
                {
                    var select = false;
                    foreach (var item in userSites)
                    {
                        if (site.Id==item.Id)
                        {
                            select = true;
                        }
                    }
                    nameSitesUser.Add(new NameSiteViewModel { NameSite=site.Name,Select=select});
                }
            }
            return nameSitesUser;
        }
    }
}