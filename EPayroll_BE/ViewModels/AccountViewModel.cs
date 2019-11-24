using System.ComponentModel.DataAnnotations;

namespace EPayroll_BE.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsRemove { get; set; }
    }

    public class AccountCreateModel
    {
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AccountChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

    public class AccountDeleteModel
    {
        [Required]
        public int Id { get; set; }
    }

    public class AccountLoginModel
    {
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AccountAuthorizedModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
    }
}
