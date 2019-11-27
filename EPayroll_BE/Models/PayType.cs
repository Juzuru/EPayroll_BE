using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class PayType : ModelBase
    {
        public string Name { get; set; }

        public Guid PayTypeCategoryId { get; set; }

        [ForeignKey("PayTypeCategoryId")]
        public PayTypeCategory PayTypeCategory { get; set; }
    }
}
