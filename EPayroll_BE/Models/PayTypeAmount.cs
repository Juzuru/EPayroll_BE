using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class PayTypeAmount : ModelBase
    {
        public long Amount { get; set; }
        public int Order { get; set; }

        public Guid SalaryLevelId { get; set; }
        public Guid PayTypeId { get; set; }

        [ForeignKey("SalaryLevelId")]
        public SalaryLevel SalaryLevel { get; set; }
        [ForeignKey("PayTypeId")]
        public PayType PayType { get; set; }
    }
}
