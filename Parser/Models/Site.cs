using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public List<UserSite> UserSites { get; set; }
        public List<Article> Articles { get; set; }
        public Site()
        {
            UserSites = new List<UserSite>();
            Articles = new List<Article>();
        }
    }
}
