﻿using System;
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
        [Required]
        public string IdentifyNumber { get; set; }

        [Required]
        public Guid PositionId { get; set; }
        [Required]
        public Guid SalaryModeId { get; set; }
        [Required]
        public Guid SalaryLevelId { get; set; }
    }
}
