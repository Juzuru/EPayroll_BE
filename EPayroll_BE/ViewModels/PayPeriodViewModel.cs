using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayPeriodViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PayPeriodCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime PayDate { get; set; }
    }
    public class PayPeriodDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PayDate { get; set; }
    }
}
