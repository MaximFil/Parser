using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.ViewModels
{
    public class ArticleViewModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string FullContent { get; set; }
        public string PartContent { get; set; }
        public int Id { get; set; }

    }
}