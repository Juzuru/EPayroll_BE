using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class SalaryTable : ModelBase
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //public int SalaryModeId { get; set; }

        //[ForeignKey("SalaryModeId")]
        //public SalaryMode SalaryMode { get; set; }
    }
}
