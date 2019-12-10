using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryShiftViewModel
    {
        public Guid Id { get; set; }
        public int OriginalHour { get; set; }
        public int OverTimeHour { get; set; }
        public DateTime Date { get; set; }
        public PaySlipViewModel PaySlipViewModel { get; set; }
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
}
