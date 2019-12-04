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
        public int TotalHour { get; set; }
        public float HourRate { get; set; }

        [Required]
        public Guid PaySlipId { get; set; }
        [Required]
        public Guid PayTypeId { get; set; }
    }
}
