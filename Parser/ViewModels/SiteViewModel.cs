﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.ViewModels1
{
    public class SiteViewModel
    {
        public List<ArticleViewModel> Articles { get; set; }
        public int PartNumber { get; set; }
        public int IdLastArticle { get; set; }
        public string NameSite { get; set; }
        public int SiteId { get; set; }
        
        public SiteViewModel()
        {
            Articles = new List<ArticleViewModel>();
        }
    }
}
