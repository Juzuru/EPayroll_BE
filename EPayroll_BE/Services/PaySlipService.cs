using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PaySlipService : IPaySlipService
    {
        private readonly IPaySlipRepository _paySlipRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;

        public PaySlipService(IPaySlipRepository paySlipRepository, IPayPeriodRepository payPeriodRepository)
        {
            _paySlipRepository = paySlipRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public Guid Add(PaySlipCreateModel model)
        {
            PaySlip paySlip = new PaySlip
            {
                PaySlipCode = model.PaySlipCode,
                PayPeriodId = model.PayPeriodId,
                EmployeeId = model.EmployeeId,
                Status = "Draft",
                CreatedDate = DateTime.Now
            };

            _paySlipRepository.Add(paySlip);
            _paySlipRepository.SaveChanges();

            return paySlip.Id;
        }

        public IList<PaySlipViewModel> GetAll()
        {
            IList<PaySlip> list = _paySlipRepository.GetAll()
                .OrderByDescending(_paySlip => _paySlip.CreatedDate)
                .Reverse().ToList();
            IList<PaySlipViewModel> result = new List<PaySlipViewModel>();
            PayPeriod payPeriod;

            for (int i = 0; i < list.Count; i++)
            {
                payPeriod = _payPeriodRepository.GetById(list[i].PayPeriodId);
                result.Add(new PaySlipViewModel
                {
                    Id = list[i].Id,
                    PaySlipCode = list[i].PaySlipCode,
                    PayPeriod = new PayPeriodViewModel
                    {
                        Id = payPeriod.Id,
                        Name = payPeriod.Name
                    },
                    Amount = list[i].Amount,
                    Status = list[i].Status
                });
            }

            return result;
        }
    }

    public interface IPaySlipService
    {
        Guid Add(PaySlipCreateModel model);
        IList<PaySlipViewModel> GetAll();
    }
}
