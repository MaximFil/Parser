using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.ViewModels
{
    public class PartViewModel
    {
        public List<int> PartNumber { get; private set; } = new List<int>();
        public PartViewModel(int count,int partNumber)
        {
            
        }
    }
}
