using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class SalaryTablePosition : ModelBase
    {
        public int PositionId { get; set; }
        public int SalaryTableId { get; set; }

        [ForeignKey("PositionId")]
        public Position Position { get; set; }
        [ForeignKey("SalaryTableId")]
        public SalaryTable SalaryTable { get; set; }
    }
}
