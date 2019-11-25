using EPayroll_BE.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Models
{
    public class Employee : ModelBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string IdentifyNumber { get; set; }

        public int PositionId { get; set; }
        public int SalaryModeId { get; set; }
        public int SalaryLevelId { get; set; }

        [ForeignKey("PositionId")]
        public Position Position { get; set; }
        [ForeignKey("SalaryModeId")]
        public SalaryMode SalaryMode { get; set; }
        [ForeignKey("SalaryLevelId")]
        public SalaryLevel SalaryLevel { get; set; }
    }
}
