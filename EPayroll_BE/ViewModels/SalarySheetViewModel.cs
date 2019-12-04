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

        [Required]
        public Guid PaySlipId { get; set; }
    }
}
