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
        private readonly ISalaryTableRepository _salaryTableRepository;

        public PayPeriodService(IPayPeriodRepository payPeriodRepository, ISalaryTableRepository salaryTableRepository)
        {
            _payPeriodRepository = payPeriodRepository;
            _salaryTableRepository = salaryTableRepository;
        }

        public Guid? Add(PayPeriodCreateModel model)
        {
            SalaryTable salaryTable = _salaryTableRepository.GetAll().OrderByDescending(_ => _.EndDate).FirstOrDefault();

            if (salaryTable.EndDate.Date < model.EndDate.Date || salaryTable.StartDate.Date > model.StartDate.Date)
                return null;

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
                    StartDate = payPeriod.StartDate.ToString("dd-MM-yyyy"),
                    EndDate = payPeriod.EndDate.ToString("dd-MM-yyyy"),
                    PayDate = payPeriod.PayDate.ToString("dd-MM-yyyy")
                };
            }
            return null;
        }
        public IList<PayPeriodDetailViewModel> GetAll()
        {
            IList<PayPeriod> list = _payPeriodRepository.GetAll()
                .OrderByDescending(_payPeriod => _payPeriod.StartDate)
                .ToList();
            IList<PayPeriodDetailViewModel> result = new List<PayPeriodDetailViewModel>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new PayPeriodDetailViewModel
                {
                    Id = list[i].Id,
                    Name = list[i].Name,
                    StartDate = list[i].StartDate.ToString("dd-MM-yyyy"),
                    EndDate = list[i].EndDate.ToString("dd-MM-yyyy"),
                    PayDate = list[i].PayDate.ToString("dd-MM-yyyy")
                });
            }
            return result;
        }
    }


    public interface IPayPeriodService
    {
        Guid? Add(PayPeriodCreateModel model);
        PayPeriodDetailViewModel GetDetail(Guid payPeriodId);
        IList<PayPeriodDetailViewModel> GetAll();
    }
}
