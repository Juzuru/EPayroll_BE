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

        public PayPeriodViewModel PayPeriod { get; set; }
    }

    public class PaySlipCreateModel
    {
        [Required]
        public string PaySlipCode { get; set; }

        [Required]
        public Guid PayPeriodId { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
    }
}
