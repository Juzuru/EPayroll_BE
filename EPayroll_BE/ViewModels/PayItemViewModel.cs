using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayItemViewModel
    {
    }

    public class PayItemCreateModel
    {
        public int PaySlipId { get; set; }
        public int PayTypeId { get; set; }
    }
}
