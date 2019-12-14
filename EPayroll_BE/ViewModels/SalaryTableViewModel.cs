using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryTableViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnable { get; set; }
    }

    public class SalaryTableCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public Guid PositionId { get; set; }
    }

    public class SalaryTableSaveModelV2
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public IList<SalaryLevelCreateModelV2> SalaryLevels { get; set; }
    }
}
