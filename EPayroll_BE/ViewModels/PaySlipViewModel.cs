﻿using System;
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
        public PayPeriodViewModel PayPeriod { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public long Amount { get; set; }
    }
   

    public class PaySlipCreateModel
    {
        public Guid PayPeriodId { get; set; }
        public Guid EmployeeId { get; set; }

        [Required]
        public IList<PayItemCreateModel> PayItems { get; set; }
    }

    public class PaySlipPaySalaryModel
    {
        public IList<Guid> EmployeeIds { get; set; }
        public Guid PayPeriodId { get; set; }
    }
    public class PaySlipCreateResult
    {
        public IList<Guid> EmployeeIds { get; set; }
        public Guid PayPeriodId { get; set; }
    }

    public class PaySlipDetailViewModel
    {
        public string PaySlipCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public long Amount { get; set; }

        public PayPeriodDetailViewModel PayPeriod { get; set; }

        public IList<GroupPayItemViewModel> GroupPayItems { get; set; }
    }
    public class PaySlipDraftCreateModel
    {
        [Required]
        public Guid PositionId { get; set; }
        [Required]
        public Guid PayPeriodId { get; set; }
    }
    public class PaySlipConfirmViewModel
    {
        public Guid Id { get; set; }
        public bool Accepted { get; set; }
    }

    public class PaySlipPaySalaryErrorViewModel
    {
        public string Error { get; set; }
        public IList<Guid> EmployeeIds { get; set; }
    }

    public class PayslipPublicModel
    {
        public Guid PayPeriodId { get; set; }
        public Guid PositionId { get; set; }
        public IList<Guid> EmployeeIds { get; set; }
    }

    public class PaySlipNonPublicViewModel
    {
        public Guid Id { get; set; }
        public string PaySlipCode { get; set; }
        public long Amount { get; set; }

        public EmployeeViewModel Employee { get; set; }
    }
}
