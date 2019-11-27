using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class AccountCreateErrorModel
    {
        public string EmployeeCodeError { get; set; }
        public string PasswordError { get; set; }
    }

    public class AccountChangePasswordErrorModel
    {
        public string OldPasswordError { get; set; }
        public string NewPasswordError { get; set; }
        public string ConfirmNewPasswordError { get; set; }
    }
}
