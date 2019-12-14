using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PayTypeAmountService : IPayTypeAmountService
    {
        private readonly IPayTypeAmountRepository _payTypeAmountRepository;
        private readonly IPayTypeRepository _payTypeRepository;

        public PayTypeAmountService(IPayTypeAmountRepository payTypeAmountRepository, IPayTypeRepository payTypeRepository)
        {
            _payTypeAmountRepository = payTypeAmountRepository;
            _payTypeRepository = payTypeRepository;
        }

        public Guid Add(PayTypeAmountCreateModel model)
        {
            PayType payType = _payTypeRepository.GetById(model.PayTypeId);

            PayTypeAmount payTypeAmount = new PayTypeAmount
            {
                Amount = model.Amount,
                PayTypeId = model.PayTypeId,
                SalaryLevelId = model.SalaryLevelId,
                 Order = payType.Order
            };

            _payTypeAmountRepository.Add(payTypeAmount);
            _payTypeAmountRepository.SaveChanges();

            return payTypeAmount.Id;
        }
    }

    public interface IPayTypeAmountService
    {
        Guid Add(PayTypeAmountCreateModel model);
    }
}
