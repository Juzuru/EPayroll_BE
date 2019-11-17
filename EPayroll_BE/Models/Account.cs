using System.ComponentModel.DataAnnotations;

namespace EPayroll_BE.Models
{
    public class Account
    {
        [Key]
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public bool IsRemove { get; set; }
    }
}
