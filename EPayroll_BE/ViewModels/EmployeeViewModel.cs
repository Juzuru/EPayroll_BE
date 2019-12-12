using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class EmployeeDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string IdentifyNumber { get; set; }

        public PositionViewModel Position { get; set; }
        public SalaryModeViewModel SalaryMode { get; set; }
        public SalaryLevelViewModel SalaryLevel { get; set; }
    }

    public class EmployeeCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public bool Gender { get; set; }
        public string IdentifyNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserUID { get; set; }
        [Required]
        public int EsapiEmployeeId { get; set; }

        public Guid PositionId { get; set; }
        public Guid SalaryModeId { get; set; }
        public Guid SalaryLevelId { get; set; }
    }

    public class EmployeeCheckUserModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserUID { get; set; }
    }
    public class EmployeeAuthorizedModel
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public Guid? Id { get; set; }
    }

    public class EmployeeViewModelV2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
