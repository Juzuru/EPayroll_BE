using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class PayItem : ModelBase
    {
        public float Amount { get; set; }

        public Guid PaySlipId { get; set; }
        public Guid PayTypeId { get; set; }

        [ForeignKey("PaySlipId")]
        public PaySlip PaySlip { get; set; }
        [ForeignKey("PayTypeId")]
        public PayType PayType { get; set; }
    }
}
