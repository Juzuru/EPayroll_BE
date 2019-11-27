using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryTablePositionViewModel
    {
    }
    
    public class SalaryTablePositionCreateModel
    {
        [Required]
        public Guid PositionId { get; set; }
        [Required]
        public Guid SalaryTableId { get; set; }
    }
}
