using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayTypeAmountViewModel
    {
        public Guid Id { get; set; }
        public long Amount { get; set; }


        public SalaryLevelViewModel SalaryLevel { get; set; }

        public PayTypeViewModel PayType { get; set; }
    }

    public class PayTypeAmountCreateModel
    {
        [Required]
        public long Amount { get; set; }

        public Guid SalaryLevelId { get; set; }
        public Guid PayTypeId { get; set; }
    }
}
