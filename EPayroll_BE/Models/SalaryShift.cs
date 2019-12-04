using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class SalaryShift : ModelBase
    {
        public int OriginalHour { get; set; }
        public int OverTimeHour { get; set; }
        public DateTime Date { get; set; }

        public Guid PaySlipId { get; set; }
        
        [ForeignKey("PaySlipId")]
        public PaySlip PaySlip { get; set; }
    }
}
