using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class PaySlip : ModelBase
    {
        public string PaySlipCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public long Amount { get; set; }
        public bool IsPublic { get; set; }

        public Guid PayPeriodId { get; set; }
        public Guid EmployeeId { get; set; }

        [ForeignKey("PayPeriodId")]
        public PayPeriod PayPeriod { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

    }
}
