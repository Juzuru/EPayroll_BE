using EPayroll_BE.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPayroll_BE.Models
{
    public class Account : ModelBase
    {
        public string EmployeeCode { get; set; }
        public string Password { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDeleted { get; set; }
    }
}
