using EPayroll_BE.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace EPayroll_BE.Models
{
    public class Account : ModelBase
    {
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsRemove { get; set; }
    }
}
