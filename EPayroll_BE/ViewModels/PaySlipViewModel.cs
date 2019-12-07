using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PaySlipViewModel
    {
        public Guid Id { get; set; }
        public string PaySlipCode { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }
    }

    public class PaySlipCreateModel
    {
        public Guid PayPeriodId { get; set; }
        public Guid EmployeeId { get; set; }

        [Required]
        public IList<PayItemCreateModel> PayItems { get; set; }
    }
}
