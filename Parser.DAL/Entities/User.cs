using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateSetting { get; set; }
        public byte ViewSetting { get; set; }
        public List<UserArticle> UserArticles { get; set; }
        public List<UserSite> UserSites { get; set; }
    }
}
