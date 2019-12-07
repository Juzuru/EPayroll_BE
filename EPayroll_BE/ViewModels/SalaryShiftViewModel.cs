using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryShiftViewModel
    {
    }

    public class SalaryShiftCreateModel
    {
        [Required]
        public int OriginalHour { get; set; }
        [Required]
        public int OverTimeHour { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Guid PaySlipId { get; set; }
    }

    public class SalaryShiftTemplateViewModel
    {
        public Guid PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public float PayTypeAmount { get; set; }

        public int Hour { get; set; }
    }
}
