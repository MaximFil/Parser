using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int SiteId { get; set; }
        public string Title { get; set; }
        public List<UserArticle> userArticle { get; set; }
        public Article()
        {
            userArticle = new List<UserArticle>();
        }
    }
}