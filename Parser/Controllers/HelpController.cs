using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.DAL.Entities;
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
                var userSitesIds = _context.Users.Include(u => u.UserSites).FirstOrDefault().UserSites.Select(s => s.SiteId);
                var sites = _context.Sites.ToList();
                foreach (var site in sites)
                {
                    var select = false;
                    foreach (var item in userSitesIds)
                    {
                        if (site.Id == item)
                        {
                            select = true;
                        }
                    }
                    nameSitesUser.Add(new NameSiteViewModel { NameSite = site.Name, Select = select });
                }
            }
            return nameSitesUser;
        }

        [HttpGet("[action]")]
        public bool GetValueShow()
        {
            return _context.Users.First().ViewSetting;
        }

        [HttpPost("[action]")]
        public IActionResult SaveFilters([FromBody] List<NameSiteViewModel> nameSites, bool showArticles)
        {
            using (_context)
            {
                _context.UserSites.RemoveRange(_context.UserSites);
                if (ModelState.IsValid)
                {
                    foreach (var nameSite in nameSites)
                    {
                        if (nameSite.Select == true)
                        {
                            _context.UserSites.Add(
                                new UserSite
                                {
                                    UserId = _context.Users.First().Id,
                                    SiteId = _context.Sites.FirstOrDefault(t => t.Name == nameSite.NameSite).Id
                                });
                        }
                    }
                    _context.Users.First().ViewSetting = showArticles;
                    _context.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public IActionResult SaveShowArticle([FromBody] int idArticle)
        {
            using (_context)
            {
                if (ModelState.IsValid)
                {
                    _context.UserArticles.Add(new UserArticle { UserId=_context.Users.FirstOrDefault().Id, ArticleId=idArticle});
                    _context.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }
    }
}