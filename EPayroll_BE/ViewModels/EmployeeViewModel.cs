using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EmployeeDetailViewModel
    {
        public int Id { get; set; }
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
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string IdentifyNumber { get; set; }

        public int PositionId { get; set; }
        public int SalaryModeId { get; set; }
        public int SalaryLevelId { get; set; }

        public AccountCreateModel Account { get; set; }
    }
}
