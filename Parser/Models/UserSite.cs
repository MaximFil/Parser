using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class UserSite
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
