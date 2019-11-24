using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels.Pager
{
    public class PagingModel<T> where T : class
    {
        public int RequestedPage { get; set; }
        public int TotalPage { get; set; }
        public int ItemCount { get; set; }
        public IList<T> ItemList { get; set; }
    }
}
