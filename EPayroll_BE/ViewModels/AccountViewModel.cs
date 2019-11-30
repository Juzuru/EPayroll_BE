using NSwag.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPayroll_BE.ViewModels
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsRemove { get; set; }
    }

    public class AccountLoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Picture { get; set; }
    }

    public class AccountTokenModel
    {
        public string TokenType { get; set; }
        public string Token { get; set; }
    }
}
