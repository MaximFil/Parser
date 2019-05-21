using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DAL.ViewModels
{
    public class ArticlesViewModel
    {
        public string Article { get; set; }
        public string Link { get; set; }
        public string FullContent { get; set; }
        public string PartContent { get; set; }
    }
}
