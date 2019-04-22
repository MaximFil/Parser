using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateSetting { get; set; }
        public byte ViewSetting { get; set; }
        public List<UserArticle> userArticle { get; set; }
        public List<UserSite> userSite { get; set; }
        public User()
        {
            userArticle = new List<UserArticle>();
            userSite = new List<UserSite>();
        }
    }
}
