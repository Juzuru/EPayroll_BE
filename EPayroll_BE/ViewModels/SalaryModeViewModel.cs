using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class SalaryModeViewModel
    {
        public Guid Id { get; set; }
        public string Mode { get; set; }
    }

    public class SalaryModeCreateModel
    {
        public string Mode { get; set; }
    }
}
