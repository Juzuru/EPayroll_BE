using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayItemViewModel
    {
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

    public class PayItemDetailViewModel
    {
        public float Amount { get; set; }
        public int TotalHour { get; set; }
        public float HourRate { get; set; }

        public string PayTypeName { get; set; }
    }

    public class GroupPayItemViewModel
    {
        public string PayTypeCategoryName { get; set; }

        public IList<PayItemDetailViewModel> PayItems { get; set; }
    }
}
