using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryLevelViewModel
    {
        public Guid Id { get; set; }
        public string Level { get; set; }
    }

    public class SalaryLevelCreateModel
    {
        [Required]
        public string Level { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public double Factor { get; set; }
        [Required]
        public string Condition { get; set; }

        public Guid SalaryTableId { get; set; }
        public Guid PositionId { get; set; }
    }
}
