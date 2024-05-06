using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Spacifications
{
    public class TaskParam 
    {
        private const int MaxSize = 10;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxSize ? MaxSize : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }

        private string? Search;

        public string? SearchItem
        {
            get { return Search; }
            set { Search = value.ToLower(); }
        }
    }
}
