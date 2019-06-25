using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class UserArticle
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public bool Deleted { get; set; }
    }
}
