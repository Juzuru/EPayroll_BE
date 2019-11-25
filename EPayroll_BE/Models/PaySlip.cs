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
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public int PayPeriodId { get; set; }
        public int EmployeeId { get; set; }

        [ForeignKey("PayPeriodId")]
        public PayPeriod PayPeriod { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

    }
}
