using NSwag.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPayroll_BE.ViewModels
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
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
        [Required]
        public string ConfirmNewPassword { get; set; }
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
        public string TokenType { get; set; }
        public string Token { get; set; }
    }
}
