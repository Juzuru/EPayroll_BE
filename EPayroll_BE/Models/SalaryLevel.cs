using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class SalaryLevel : ModelBase
    {
        public string Level { get; set; }
        public int Order { get; set; }
        public string Condition { get; set; }

        public int SalaryTableId { get; set; }

        [ForeignKey("SalaryTableId")]
        public SalaryTable SalaryTable { get; set; }
    }
}
