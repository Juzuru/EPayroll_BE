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
        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;

        public PayTypeService(IPayTypeRepository payTypeRepository,
            IPayTypeCategoryRepository payTypeCategoryRepository)
        {
            _payTypeRepository = payTypeRepository;
            _payTypeCategoryRepository = payTypeCategoryRepository;


        }

        public Guid Add(PayTypeCreateModel model)
        {
            PayType payType = new PayType
            {
                Name = model.Name,
                PayTypeCategoryId = model.PayTypeCategoryId,
                Order = model.Order
            };

            _payTypeRepository.Add(payType);
            _payTypeRepository.SaveChanges();

            return payType.Id;
        }

        public IList<PayTypeViewModel> GetAll()
        {
            IList<PayType> list = _payTypeRepository.GetAll().ToList();
            IList<PayTypeViewModel> result = new List<PayTypeViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                PayTypeCategory payTypeCategory = _payTypeCategoryRepository.GetById(list[i].PayTypeCategoryId);
                result.Add(new PayTypeViewModel
                {
                    Id = list[i].Id,
                    Name = list[i].Name,
                    PayTypeCategory = new PayTypeCategoryViewModel
                    {
                        Id = payTypeCategory.Id,
                        Name = payTypeCategory.Name
                    }
                });
            }
            return result;
        }
    }


    public interface IPayTypeService
    {
        Guid Add(PayTypeCreateModel model);
        IList<PayTypeViewModel> GetAll();
    }
}
