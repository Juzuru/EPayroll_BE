using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PaySlipViewModel
    {
        public Guid Id { get; set; }
        public string PaySlipCode { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }
        public PayPeriodViewModel PayPeriod { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }
   

    public class PaySlipCreateModel
    {
        public Guid PayPeriodId { get; set; }
        public Guid EmployeeId { get; set; }

        [Required]
        public IList<PayItemCreateModel> PayItems { get; set; }
    }

    public class PaySlipTemplate
    {
        public IList<PayItemTemplate> PayItemTemplates { get; set; }
        public IList<SalaryShiftTemplateViewModel> SalaryShiftTemplates { get; set; }
    }
    public class PaySlipCreateResult
    {
        public Guid PayPeriodId { get; set; }
        public PositionViewModel Position { get; set; }
    }
    public class PaySlipDetailViewModel
    {
        public string PaySlipCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }

        public PayPeriodDetailViewModel PayPeriod { get; set; }

        public IList<GroupPayItemViewModel> GroupPayItems { get; set; }
    }
}
