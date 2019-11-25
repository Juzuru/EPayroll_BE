using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PaySlipViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public PayPeriodViewModel PayPeriod { get; set; }
    }

    public class PaySlipCreateModel
    {
        public string Name { get; set; }

        public int PayPeriodId { get; set; }

        public IList<PayItemCreateModel> PayItems { get; set; }
    }
}
