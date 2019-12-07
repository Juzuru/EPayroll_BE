using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayItemViewModel
    {
    }

    public class PayItemCreateModel
    {
        [Required]
        public float Amount { get; set; }
        [Required]
        public int TotalHour { get; set; }
        [Required]
        public float HourRate { get; set; }

        public Guid PaySlipId { get; set; }
        public Guid PayTypeId { get; set; }
    }

    public class PayItemTemplate
    {
        public Guid PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public float PayTypeAmount { get; set; }

        public string Template { get; set; }
    }
}
