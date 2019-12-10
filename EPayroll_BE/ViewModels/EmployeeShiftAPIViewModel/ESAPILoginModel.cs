using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels.EmployeeShiftAPIViewModel
{
    public class ESAPILoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ESAPIAuthorizedModel
    {
        public string Token { get; set; }
    }
}
