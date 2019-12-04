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
        private readonly IPaySlipRepository _paySlipRepository;
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly IFormularRepository _formularRepository;

        private readonly IPayPeriodRepository _payPeriodRepository;

        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;

        public PayItemService(IPayItemRepository payItemRepository,
            IPaySlipRepository paySlipRepository,
            IPayTypeRepository payTypeRepository,
            IFormularRepository formularRepository,
            IPayTypeCategoryRepository payTypeCategoryRepository,
            IPayPeriodRepository payPeriodRepository)
        {
            _payItemRepository = payItemRepository;
            _paySlipRepository = paySlipRepository;
            _payTypeRepository = payTypeRepository;

            _payPeriodRepository = payPeriodRepository;
            _payTypeCategoryRepository = payTypeCategoryRepository;
        }

        public Guid Add(PayItemCreateModel model, bool isTemplate)
        {
            PayItem payItem = new PayItem
            {
                Amount = model.Amount,
                PaySlipId = model.PaySlipId,
                PayTypeId = model.PayTypeId,
                IsTemplate = isTemplate
            };

            _payItemRepository.Add(payItem);
            _payItemRepository.SaveChanges();

            return payItem.Id;
        }

        public IList<PayItemViewModel> GetAll()
        {
            IList<PayItem> list = _payItemRepository.GetAll().ToList();
            IList<PayItemViewModel> result = new List<PayItemViewModel>();
            PaySlip paySlip;
            PayType payType;
            Formular formular;
            PayPeriod payPeriod;
            PayTypeCategory payTypeCategory;

            for (int i = 0; i < list.Count; i++)
            {
                paySlip = _paySlipRepository.GetById(list[i].PaySlipId);
                payType = _payTypeRepository.GetById(list[i].PayTypeId);
                formular = _formularRepository.GetById(list[i].FormularId);
                payPeriod = _payPeriodRepository.GetById(paySlip.PayPeriodId);
                payTypeCategory = _payTypeCategoryRepository.GetById(payType.PayTypeCategoryId);

                result.Add(new PayItemViewModel
                {
                    Id = list[i].Id,
                    Amount = list[i].Amount,
                    IsTemplate = list[i].IsTemplate,
                    Formular = new FormularViewModel
                    {
                        Id = formular.Id,
                        Description = formular.Description,
                    },
                    PaySlip = new PaySlipViewModel
                    {
                        Id = paySlip.Id,
                        Amount = paySlip.Amount,
                        PaySlipCode = paySlip.PaySlipCode,
                        Status = paySlip.Status,
                        PayPeriod = new PayPeriodViewModel
                        {
                            Id = payPeriod.Id,
                            Name = payPeriod.Name,
                            StartDate = payPeriod.StartDate,
                            EndDate = payPeriod.EndDate,
                            PayDate = payPeriod.PayDate,
                        }
                    },
                    PayType = new PayTypeViewModel
                    {
                        Id = payType.Id,
                        Name = payType.Name,
                        PayTypeCategory = new PayTypeCategoryViewModel
                        {
                            Id = payTypeCategory.Id,
                            Name = payTypeCategory.Name,
                        }
                    }

                });
            }
            return result;
        }

        public PayItemViewModel GetById(Guid payItemId)
        {
            PayItem payItem = _payItemRepository.GetById(payItemId);

            if (payItem != null)
            {

                PaySlip paySlip = _paySlipRepository.GetById(payItem.PaySlipId);
                PayType payType = _payTypeRepository.GetById(payItem.PayTypeId);
                Formular formular = _formularRepository.GetById(payItem.FormularId);
                PayPeriod payPeriod = _payPeriodRepository.GetById(paySlip.PayPeriodId);
                PayTypeCategory payTypeCategory = _payTypeCategoryRepository.GetById(payType.PayTypeCategoryId);

                return new PayItemViewModel
                {
                    Id = payItem.Id,
                    Amount = payItem.Amount,
                    IsTemplate = payItem.IsTemplate,
                    PaySlip = new PaySlipViewModel
                    {
                        Id = paySlip.Id,
                        Amount = paySlip.Amount,
                        PaySlipCode = paySlip.PaySlipCode,
                        Status = paySlip.Status,
                        PayPeriod = new PayPeriodViewModel
                        {
                            Id = payPeriod.Id,
                            Name = payPeriod.Name,
                            StartDate = payPeriod.StartDate,
                            EndDate = payPeriod.EndDate,
                            PayDate = payPeriod.PayDate,
                        }
                    },
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
                    Formular = new FormularViewModel
                    {
                        Id = formular.Id,
                        Description = formular.Description,
                    }
                };
            }
            return null;
        }
    }

    public interface IPayItemService
    {
        
        IList<PayItemViewModel> GetAll();
        PayItemViewModel GetById(Guid payItemId);
        Guid Add(PayItemCreateModel model, bool isTemplate);
    }
}
