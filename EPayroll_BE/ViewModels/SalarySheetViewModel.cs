using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalarySheetViewModel
    {
    }

    public class SalarySheetCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int TotalWorking { get; set; }
        [Required]
        public float WorkingRate { get; set; }
        [Required]
        public float Amount { get; set; }

        [Required]
        public Guid PaySlipId { get; set; }
        [Required]
        public Guid PayTypeId { get; set; }
    }
}
