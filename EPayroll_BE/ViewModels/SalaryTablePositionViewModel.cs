using EPayroll_BE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryTablePositionViewModel
    {
        public Guid Id { get; set; }
        public PositionViewModel Position { get; set; }
        public SalaryTableViewModel SalaryTable { get; set; }
    }
    
    public class SalaryTablePositionCreateModel
    {
        [Required]
        public Guid PositionId { get; set; }
        [Required]
        public Guid SalaryTableId { get; set; }
    }
}
