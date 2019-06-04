using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DAL.Entities
{
    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public List<UserSite> UserSites { get; set; }
        public Site()
        {
            UserSites = new List<UserSite>();
        }
    }
}
