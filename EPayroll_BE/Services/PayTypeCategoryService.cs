using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PayTypeCategoryService : IPayTypeCategoryService
    {
        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;

        public PayTypeCategoryService(IPayTypeCategoryRepository payTypeCategoryRepository)
        {
            _payTypeCategoryRepository = payTypeCategoryRepository;
        }

        public Guid Add(PayTypeCategoryCreateModel model)
        {
            PayTypeCategory payTypeCategory = new PayTypeCategory
            {
                Name = model.Name
            };

            _payTypeCategoryRepository.Add(payTypeCategory);
            _payTypeCategoryRepository.SaveChanges();

            return payTypeCategory.Id;
        }
    }

    public interface IPayTypeCategoryService
    {
        Guid Add(PayTypeCategoryCreateModel model);
    }
}
