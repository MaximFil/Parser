using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DAL.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int SiteId { get; set; }
        public string Title { get; set; }
        public string PartContent { get; set; }
        public string Content { get; set; }
        public List<UserArticle> UserArticle { get; set; }
    }
}
