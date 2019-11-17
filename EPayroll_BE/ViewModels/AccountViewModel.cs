namespace EPayroll_BE.ViewModels
{
    public class AccountViewModel
    {
        public string EmployeeId { get; set; }
        public bool IsRemove { get; set; }
    }

    public class AccountCreateModel
    {
        public string EmployeeId { get; set; }
    }

    public class AccountUpdateModel
    {
        public string EmployeeId { get; set; }
        public string Password { get; set; }
    }

    public class AccountDeleteModel
    {
        public string EmployeeId { get; set; }
    }

    public class AccountLoginModel
    {
        public string EmployeeId { get; set; }
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
