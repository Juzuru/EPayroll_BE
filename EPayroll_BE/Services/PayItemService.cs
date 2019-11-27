using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PayItemService : IPayItemService
    {
        private readonly IPayItemRepository _payItemRepository;

        public PayItemService(IPayItemRepository payItemRepository)
        {
            _payItemRepository = payItemRepository;
        }

        public Guid Add(PayItemCreateModel model)
        {
            PayItem payItem = new PayItem
            {
                Amount = model.Amount,
                PaySlipId = model.PaySlipId,
                PayTypeId = model.PayTypeId
            };

            _payItemRepository.Add(payItem);
            _payItemRepository.SaveChanges();

            return payItem.Id;
        }
    }

    public interface IPayItemService
    {
        Guid Add(PayItemCreateModel model);
    }
}
