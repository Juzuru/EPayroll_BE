using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PaySlipService : IPaySlipService
    {
        private readonly IPaySlipRepository _paySlipRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IPayItemRepository _payItemRepository;
        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;

        public PaySlipService(IPaySlipRepository paySlipRepository,
            IPayPeriodRepository payPeriodRepository,
            IPayItemRepository payItemRepository,
            IPayTypeCategoryRepository payTypeCategoryRepository,
            IPayTypeRepository payTypeRepository,
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository)
        {
            _paySlipRepository = paySlipRepository;
            _payPeriodRepository = payPeriodRepository;
            _payItemRepository = payItemRepository;
            _payTypeCategoryRepository = payTypeCategoryRepository;
            _payTypeRepository = payTypeRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public Guid Add(PaySlipCreateModel model)
        {
            PaySlip paySlip = new PaySlip
            {
                PayPeriodId = model.PayPeriodId,
                EmployeeId = model.EmployeeId,
                Status = "Draft",
                CreatedDate = DateTime.Now
            };

            _paySlipRepository.Add(paySlip);
            _paySlipRepository.SaveChanges();

            return paySlip.Id;
        }

        public PaySlipCreateResult AddDraft(PayPeriodCreateModel model)
        {
            Position position = _positionRepository.GetById(model.PositionId);
            if (position != null)
            {

                PayPeriod payPeriod = new PayPeriod
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    PayDate = model.PayDate,
                };
                _payPeriodRepository.Add(payPeriod);
                _payPeriodRepository.SaveChanges();

                IList<Employee> employees = _employeeRepository.GetAll().
                    Where(_emp => _emp.PositionId.Equals(model.PositionId)).ToList();

                foreach (var item in employees)
                {
                    PaySlip paySlip = new PaySlip
                    {
                        PayPeriodId = payPeriod.Id,
                        EmployeeId = item.Id,
                        Status = "Draft",
                        CreatedDate = DateTime.Now,
                    };
                    _paySlipRepository.Add(paySlip);
                    _paySlipRepository.SaveChanges();
                }

                return new PaySlipCreateResult
                {
                    PayPeriodId = payPeriod.Id,
                    Position = new PositionViewModel
                    {
                        Id = position.Id,
                        Name = position.Name
                    }
                };
            }
            return null;
        }

        public IList<PaySlipViewModel> GetAll(Guid employeeId)
        {
            IList<PaySlip> list = _paySlipRepository
                .Get(_playSlip => _playSlip.EmployeeId.Equals(employeeId))
                .OrderByDescending(_paySlip => _paySlip.CreatedDate)
                .Reverse().ToList();

            IList<PaySlipViewModel> result = new List<PaySlipViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new PaySlipViewModel
                {
                    Id = list[i].Id,
                    PaySlipCode = list[i].PaySlipCode,
                    Amount = list[i].Amount,
                    Status = list[i].Status
                });
            }

            return result;
        }

        public PaySlipDetailViewModel GetById(Guid paySlipId)
        {
            PaySlip paySlip = _paySlipRepository.GetById(paySlipId);

            if (paySlip != null)
            {
                IList<PayTypeCategory> payTypeCategories = _payTypeCategoryRepository.GetAll().ToList();

                //IList<PayItem> payItems = _payItemRepository.Get(_payItem => _payItem.PaySlipId.Equals(paySlipId)).ToList();

                IList<GroupPayItemViewModel> groupPayItemViewModels =
                    new List<GroupPayItemViewModel>();

                PayPeriod payPeriod = _payPeriodRepository.GetById(paySlip.PayPeriodId);

                foreach (var item in payTypeCategories)
                {

                    IList<PayItem> payItemList = _payItemRepository.Get(_payItem =>
                       _payItem.PayType.PayTypeCategory.Name.Equals(item.Name)).
                       Where(_payItem => _payItem.PaySlipId.Equals(paySlipId)).ToList();

                    IList<PayItemDetailViewModel> payItemDetails = new List<PayItemDetailViewModel>();

                    for (int i = 0; i < payItemList.Count; i++)
                    {
                        PayType payType = _payTypeRepository.GetById(payItemList[i].PayTypeId);
                        payItemDetails.Add(new PayItemDetailViewModel
                        {
                            Amount = payItemList[i].Amount,
                            HourRate = payItemList[i].HourRate,
                            TotalHour = payItemList[i].TotalHour,
                            PayTypeName = payType.Name
                        });

                    }
                    groupPayItemViewModels.Add(new GroupPayItemViewModel
                    {
                        PayTypeCategoryName = item.Name,
                        PayItems = payItemDetails
                    });
                }

                return new PaySlipDetailViewModel
                {
                    Amount = paySlip.Amount,
                    CreatedDate = paySlip.CreatedDate,
                    PaySlipCode = paySlip.PaySlipCode,
                    Status = paySlip.Status,
                    PayPeriod = new PayPeriodDetailViewModel
                    {
                        Id = payPeriod.Id,
                        Name = payPeriod.Name,
                        StartDate = payPeriod.StartDate,
                        EndDate = payPeriod.EndDate,
                        PayDate = payPeriod.PayDate,
                    },
                    GroupPayItems = groupPayItemViewModels
                };
            }
            return null;
        }
    }

    public interface IPaySlipService
    {
        Guid Add(PaySlipCreateModel model);
        IList<PaySlipViewModel> GetAll(Guid employeeId);
        PaySlipDetailViewModel GetById(Guid paySlipId);
        PaySlipCreateResult AddDraft(PayPeriodCreateModel model);
    }
}
