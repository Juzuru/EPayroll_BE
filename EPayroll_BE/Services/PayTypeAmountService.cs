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
        private readonly ISalaryLevelRepository _salaryLevelRepository;
        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;
        private readonly ISalaryTableRepository _salaryTableRepository;

        public PayTypeAmountService(IPayTypeAmountRepository payTypeAmountRepository, IPayTypeRepository payTypeRepository, ISalaryLevelRepository salaryLevelRepository, IPayTypeCategoryRepository payTypeCategoryRepository, ISalaryTableRepository salaryTableRepository)
        {
            _payTypeAmountRepository = payTypeAmountRepository;
            _payTypeRepository = payTypeRepository;
            _salaryLevelRepository = salaryLevelRepository;
            _payTypeCategoryRepository = payTypeCategoryRepository;
            _salaryTableRepository = salaryTableRepository;
        }

        public Guid Add(PayTypeAmountCreateModel model)
        {
            PayType payType = _payTypeRepository.GetById(model.PayTypeId);

            PayTypeAmount payTypeAmount = new PayTypeAmount
            {
                Amount = model.Amount,
                PayTypeId = model.PayTypeId,
                SalaryLevelId = model.SalaryLevelId,
                IsIsMultiple = payType.IsMultiple
            };

            _payTypeAmountRepository.Add(payTypeAmount);
            _payTypeAmountRepository.SaveChanges();

            return payTypeAmount.Id;
        }

        public IList<PayTypeAmountViewModel> GetAll()
        {
            IList<PayTypeAmount> list = _payTypeAmountRepository.GetAll().ToList();
            IList<PayTypeAmountViewModel> result = new List<PayTypeAmountViewModel>();
            for (int i = 0; i < list.Count; i++)
            {
                PayType payType = _payTypeRepository.GetById(list[i].PayTypeId);
                SalaryLevel salaryLevel = _salaryLevelRepository.GetById(list[i].SalaryLevelId);
                PayTypeCategory payTypeCategory = _payTypeCategoryRepository.GetById(payType.PayTypeCategoryId);
                SalaryTable salaryTable = _salaryTableRepository.GetById(salaryLevel.SalaryTableId);
                result.Add(new PayTypeAmountViewModel
                {
                    Id = list[i].Id,
                    Amount = list[i].Amount,
                    PayType = new PayTypeViewModel
                    {
                        Id = payType.Id,
                        Name = payType.Name,
                        PayTypeCategory = new PayTypeCategoryViewModel
                        {
                            Id = payTypeCategory.Id,
                            Name = payTypeCategory.Name
                        }
                    },
                    SalaryLevel = new SalaryLevelViewModel
                    {
                        Id = salaryLevel.Id,
                        Condition = salaryLevel.Condition,
                        Factor = salaryLevel.Factor,
                        Level = salaryLevel.Level,
                        Order = salaryLevel.Order,
                        SalaryTable = new SalaryTableViewModel
                        {
                            Id = salaryTable.Id,
                            Name = salaryTable.Name,
                            StartDate = salaryTable.StartDate,
                            EndDate = salaryTable.EndDate,
                            CreatedDate = salaryTable.CreatedDate,
                            IsEnable = salaryTable.IsEnable
                        }
                    }
                     
                });
            }
            return result;
        }
    }

    public interface IPayTypeAmountService
    {
        Guid Add(PayTypeAmountCreateModel model);
        IList<PayTypeAmountViewModel> GetAll();
    }
}
