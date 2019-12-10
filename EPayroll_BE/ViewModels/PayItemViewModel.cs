using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayItemViewModel
    {
        public Guid Id { get; set; }
        public long Amount { get; set; }
        public bool IsTemplate { get; set; }

        public PaySlipViewModel PaySlip { get; set; }
        public PayTypeViewModel PayType { get; set; }
    }

    public class PayItemDetailViewModel
    {
        public long Amount { get; set; }
        public int TotalHour { get; set; }
        public long HourRate { get; set; }

        public string PayTypeName { get; set; }
    }

    public class GroupPayItemViewModel
    {
        public string PayTypeCategoryName { get; set; }

        public IList<PayItemDetailViewModel> PayItems { get; set; }
    }

    public class PayItemCreateModel
    {
        [Required]
        public long Amount { get; set; }
        public int TotalHour { get; set; }
        public long HourRate { get; set; }

        public Guid PaySlipId { get; set; }
        public Guid PayTypeId { get; set; }
    }

    public class PayItemTemplate
    {
        public Guid PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public float PayTypeAmount { get; set; }

        public string Template { get; set; }
    }
}
