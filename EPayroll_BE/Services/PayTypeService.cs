using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PayTypeService : IPayTypeService
    {
        private readonly IPayTypeRepository _payTypeRepository;

        public PayTypeService(IPayTypeRepository payTypeRepository)
        {
            _payTypeRepository = payTypeRepository;
        }

        public Guid Add(PayTypeCreateModel model)
        {
            PayType payType = new PayType
            {
                Name = model.Name,
                PayTypeCategoryId = model.PayTypeCategoryId,
                IsMultiple = model.IsMultiple
            };

            _payTypeRepository.Add(payType);
            _payTypeRepository.SaveChanges();

            return payType.Id;
        }
    }

    public interface IPayTypeService
    {
        Guid Add(PayTypeCreateModel model);
    }
}
