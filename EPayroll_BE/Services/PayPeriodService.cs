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
        public PayPeriodDetailViewModel GetDetail(Guid payPeriodId)
        {
            PayPeriod payPeriod = _payPeriodRepository.GetById(payPeriodId);
            if(payPeriod != null)
            {
                return new PayPeriodDetailViewModel
                {
                    Id = payPeriod.Id,
                    Name = payPeriod.Name,
                    StartDate = payPeriod.StartDate,
                    EndDate = payPeriod.EndDate,
                    PayDate = payPeriod.PayDate
                };
            }
            return null;
        }
        public IList<PayPeriodDetailViewModel> GetAll()
        {
            IList<PayPeriod> list = _payPeriodRepository.GetAll()
                .OrderByDescending(_payPeriod => _payPeriod.StartDate)
                .Reverse().ToList();
            IList<PayPeriodDetailViewModel> result = new List<PayPeriodDetailViewModel>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new PayPeriodDetailViewModel
                {
                    Id = list[i].Id,
                    Name = list[i].Name,
                    StartDate = list[i].StartDate,
                    EndDate = list[i].EndDate,
                    PayDate = list[i].PayDate
                });
            }
            return result;
        }
    }


    public interface IPayPeriodService
    {
        Guid Add(PayPeriodCreateModel model);
        PayPeriodDetailViewModel GetDetail(Guid payPeriodId);
        IList<PayPeriodDetailViewModel> GetAll();
    }
}
