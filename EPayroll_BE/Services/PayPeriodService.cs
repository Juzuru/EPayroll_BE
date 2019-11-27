using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PayPeriodService : IPayPeriodService
    {
        private readonly IPayPeriodRepository _payPeriodRepository;

        public PayPeriodService(IPayPeriodRepository payPeriodRepository)
        {
            _payPeriodRepository = payPeriodRepository;
        }

        public Guid Add(PayPeriodCreateModel model)
        {
            PayPeriod payPeriod = new PayPeriod
            {
                Name = model.Name,
                EndDate = model.EndDate,
                PayDate = model.PayDate,
                StartDate = model.StartDate
            };

            _payPeriodRepository.Add(payPeriod);
            _payPeriodRepository.SaveChanges();

            return payPeriod.Id;
        }
    }

    public interface IPayPeriodService
    {
        Guid Add(PayPeriodCreateModel model);
    }
}
