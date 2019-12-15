using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.Services.Base;
using EPayroll_BE.Services.ThirdParty;
using EPayroll_BE.Utilities;
using EPayroll_BE.ViewModels;
using EPayroll_BE.ViewModels.EmployeeShiftAPIViewModel;
using Newtonsoft.Json;
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
        private readonly IRequestService _requestService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISalaryShiftRepository _salaryShiftRepository;
        private readonly IPayTypeAmountRepository _payTypeAmountRepository;
        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IFirebaseCloudMessagingService _firebaseCloudMessagingService;
        private readonly IESAPIService _eSAPIService;

        public PaySlipService(IPaySlipRepository paySlipRepository, IPayPeriodRepository payPeriodRepository, IPayItemRepository payItemRepository, IRequestService requestService, IEmployeeRepository employeeRepository, ISalaryShiftRepository salaryShiftRepository, IPayTypeAmountRepository payTypeAmountRepository, IPayTypeCategoryRepository payTypeCategoryRepository, IPayTypeRepository payTypeRepository, IPositionRepository positionRepository, IFirebaseCloudMessagingService firebaseCloudMessagingService, IESAPIService eSAPIService)
        {
            _paySlipRepository = paySlipRepository;
            _payPeriodRepository = payPeriodRepository;
            _payItemRepository = payItemRepository;
            _requestService = requestService;
            _employeeRepository = employeeRepository;
            _salaryShiftRepository = salaryShiftRepository;
            _payTypeAmountRepository = payTypeAmountRepository;
            _payTypeCategoryRepository = payTypeCategoryRepository;
            _payTypeRepository = payTypeRepository;
            _positionRepository = positionRepository;
            _firebaseCloudMessagingService = firebaseCloudMessagingService;
            _eSAPIService = eSAPIService;
        }

        public Guid Add(PaySlipCreateModel model)
        {
            PaySlip paySlip = new PaySlip
            {
                PayPeriodId = model.PayPeriodId,
                EmployeeId = model.EmployeeId,
                Status = "Draft",
                CreatedDate = DateTime.Now,
                IsPublic = false
            };
            _paySlipRepository.Add(paySlip);

            for (int i = 0; i < model.PayItems.Count; i++)
            {
                _payItemRepository.Add(new PayItem { 
                    Amount = model.PayItems[i].Amount,
                    HourRate = model.PayItems[i].HourRate,
                    PaySlipId = paySlip.Id,
                    PayTypeId = model.PayItems[i].PayTypeId,
                    TotalHour = model.PayItems[i].TotalHour
                });
            }

            _paySlipRepository.SaveChanges();

            return paySlip.Id;
        }

        public IList<PaySlipPaySalaryErrorViewModel> PaySalary(PaySlipPaySalaryModel model)
        {
            IList<PaySlipPaySalaryErrorViewModel> errors = new List<PaySlipPaySalaryErrorViewModel>();
            PaySlipPaySalaryErrorViewModel serverError = new PaySlipPaySalaryErrorViewModel
            {
                Error = "Server error",
                EmployeeIds = new List<Guid>()
            };
            PaySlipPaySalaryErrorViewModel salaryPaidError = new PaySlipPaySalaryErrorViewModel
            {
                Error = "Employee has payslip for this pay period",
                EmployeeIds = new List<Guid>()
            };
            PaySlipPaySalaryErrorViewModel noWorkError = new PaySlipPaySalaryErrorViewModel
            {
                Error = "Employee didn't work in this pay period",
                EmployeeIds = new List<Guid>()
            };

            string token = _eSAPIService.GetTokenFromESAPI();
            if (token == null) return null;

            Employee employee;
            PayPeriod payPeriod = _payPeriodRepository.GetById(model.PayPeriodId);
            PaySlip paySlip;
            DateTime createdDate = DateTime.Now;
            IList<PayTypeAmount> payTypeAmounts;

            bool flag = false;
            for (int i = 0; i < model.EmployeeIds.Count; i++)
            {
                flag = false;
                paySlip = _paySlipRepository
                    .Get(_ => _.PayPeriodId.Equals(model.PayPeriodId) && _.EmployeeId.Equals(model.EmployeeIds[i]))
                    .FirstOrDefault();
                if (paySlip == null)
                {
                    employee = _employeeRepository.GetById(model.EmployeeIds[i]);
                    payTypeAmounts = _payTypeAmountRepository
                                .Get(_ => _.SalaryLevelId.Equals(employee.SalaryLevelId));
                    var reports = _eSAPIService.GetAttendanceReport(token, employee.EsapiEmployeeId, payPeriod.StartDate.Date, payPeriod.EndDate.Date);
                    
                    if (reports != null)
                    {
                        string paySlipCode = payPeriod.EndDate.Month < 10 ? "0" + payPeriod.EndDate.Month : "" + payPeriod.EndDate.Month;
                        paySlipCode += (payPeriod.EndDate.Year % 100).ToString();

                        paySlip = new PaySlip
                        {
                            Amount = 0,
                            CreatedDate = createdDate,
                            EmployeeId = model.EmployeeIds[i],
                            PayPeriodId = model.PayPeriodId,
                            Status = "Waiting",
                            PaySlipCode = StringGenerationUtility.GenerateCode() + paySlipCode,
                            IsPublic = false
                        };
                        _paySlipRepository.Add(paySlip);
                        _paySlipRepository.SaveChanges();

                        try
                        {
                            long totalAmount = 0;
                            int originalHours = 0;
                            int overTimeHours = 0;
                            int originalWeekendHours = 0;
                            int overTimeWeekendHours = 0;

                            for (int j = 0; j < reports.Count; j++)
                            {
                                _salaryShiftRepository.Add(new SalaryShift
                                {
                                    Date = reports[j].Date,
                                    OriginalHour = reports[j].OriginalHour,
                                    OverTimeHour = reports[j].OverTime,
                                    PaySlipId = paySlip.Id
                                });

                                if (reports[j].Date.DayOfWeek.ToString().StartsWith("S"))
                                {
                                    originalWeekendHours += reports[j].OriginalHour;
                                    overTimeWeekendHours += reports[j].OverTime;
                                }
                                else
                                {
                                    originalHours += reports[j].OriginalHour;
                                    overTimeHours += reports[j].OverTime;
                                }
                            }

                            for (int k = 0; k < payTypeAmounts.Count; k++)
                            {
                                int totalHours = 0;
                                switch (payTypeAmounts[k].Order)
                                {
                                    case 0:
                                        long l = payTypeAmounts[k].Amount * reports.Count / 30;
                                        l -= l % 1000;
                                        totalAmount += l;
                                        _payItemRepository.Add(new PayItem
                                        {
                                            Amount = l,
                                            HourRate = l,
                                            PaySlipId = paySlip.Id,
                                            PayTypeId = payTypeAmounts[k].PayTypeId,
                                            TotalHour = 1
                                        });
                                        break;
                                    case 1:
                                        totalHours = originalHours;
                                        break;
                                    case 2:
                                        totalHours = overTimeHours;
                                        break;
                                    case 3:
                                        totalHours = originalWeekendHours;
                                        break;
                                    case 4:
                                        totalHours = overTimeWeekendHours;
                                        break;
                                }

                                if (totalHours != 0)
                                {
                                    totalAmount += totalHours * payTypeAmounts[k].Amount;
                                    _payItemRepository.Add(new PayItem
                                    {
                                        Amount = totalHours * payTypeAmounts[k].Amount,
                                        HourRate = payTypeAmounts[k].Amount,
                                        PaySlipId = paySlip.Id,
                                        PayTypeId = payTypeAmounts[k].PayTypeId,
                                        TotalHour = totalHours
                                    });
                                }
                            }

                            paySlip.Status = "Waiting";
                            paySlip.Amount = totalAmount;

                            _paySlipRepository.Update(paySlip);
                        }
                        catch (Exception e)
                        {
                            _paySlipRepository.Delete(_ => _.Id.Equals(paySlip.Id));
                            serverError.EmployeeIds.Add(model.EmployeeIds[i]);
                        }
                        _paySlipRepository.SaveChanges();
                    }
                    else
                    {
                        noWorkError.EmployeeIds.Add(model.EmployeeIds[i]);
                        flag = true;
                    }
                }
                else
                {
                    salaryPaidError.EmployeeIds.Add(model.EmployeeIds[i]);
                    flag = true;
                }
            }

            if (flag)
            {
                if (serverError.EmployeeIds.Count > 0) errors.Add(serverError);
                if (noWorkError.EmployeeIds.Count > 0) errors.Add(noWorkError);
                if (salaryPaidError.EmployeeIds.Count > 0) errors.Add(salaryPaidError);

                return errors;
            }
            return null;
        }

        public int AddDraft(PaySlipDraftCreateModel model)
        {
            Position position = _positionRepository.GetById(model.PositionId);
            if (position != null)
            {
                PayPeriod payPeriod = _payPeriodRepository.GetById(model.PayPeriodId);
                if (payPeriod != null)
                {
                    IList<Employee> employees = _employeeRepository.GetAll().
                    Where(_emp => _emp.PositionId.Equals(model.PositionId)).ToList();
                    string paySlipCode = payPeriod.EndDate.Month < 10 ? "0" + payPeriod.EndDate.Month : "" + payPeriod.EndDate.Month;
                    paySlipCode += (payPeriod.EndDate.Year % 100).ToString();

                    foreach (var item in employees)
                    {
                        PaySlip paySlip = new PaySlip
                        {
                            PayPeriodId = model.PayPeriodId,
                            EmployeeId = item.Id,
                            Status = "Draft",
                            CreatedDate = DateTime.Now,
                            PaySlipCode = StringGenerationUtility.GenerateCode() + paySlipCode
                        };
                        _paySlipRepository.Add(paySlip);
                        _paySlipRepository.SaveChanges();
                    }

                    return 1;
                }
                return -2;
            }
            return -1;
        }

        public IList<PaySlipViewModel> GetAll(Guid employeeId)
        {
            IList<PaySlip> list = _paySlipRepository
                .Get(_playSlip => _playSlip.EmployeeId.Equals(employeeId) && _playSlip.IsPublic == true)
                .OrderByDescending(_paySlip => _paySlip.CreatedDate)
                .ToList();

            IList<PaySlipViewModel> result = new List<PaySlipViewModel>();
            PayPeriod payPeriod;

            for (int i = 0; i < list.Count; i++)
            {
                payPeriod = _payPeriodRepository.GetById(list[i].PayPeriodId);

                result.Add(new PaySlipViewModel
                {
                    Id = list[i].Id,
                    PaySlipCode = list[i].PaySlipCode,
                    Amount = list[i].Amount,
                    Status = list[i].Status,
                    PayPeriod = new PayPeriodViewModel
                    {
                        Id = payPeriod.Id,
                        Name = payPeriod.Name
                    }
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
                        StartDate = payPeriod.StartDate.ToString("dd-MM-yyyy"),
                        EndDate = payPeriod.EndDate.ToString("dd-MM-yyyy"),
                        PayDate = payPeriod.PayDate.ToString("dd-MM-yyyy")
                    },
                    GroupPayItems = groupPayItemViewModels
                };
            }
            return null;
        }

        public bool Confirm(PaySlipConfirmViewModel model)
        {
            PaySlip paySlip = _paySlipRepository.GetById(model.Id);
            if (paySlip == null) return false;

            paySlip.Status = model.Accepted == true ? "Accepted" : "Unaccepted";

            _paySlipRepository.Update(paySlip);
            _paySlipRepository.SaveChanges();

            return true;
        }

        public IList<Guid> Public(PayslipPublicModel model)
        {
            var payslips = _paySlipRepository
                .Get(_ => _.PayPeriodId.Equals(model.PayPeriodId) 
                    && _.Employee.PositionId.Equals(model.PositionId)
                    && _.IsPublic == false);

            PayPeriod payPeriod = _payPeriodRepository.Get(_ => _.Id.Equals(model.PayPeriodId)).FirstOrDefault();
            IList<Guid> errorIds = new List<Guid>();
            for (int i = 0; i < payslips.Count; i++)
            {
                for (int j = 0; j < model.EmployeeIds.Count; j++)
                {
                    if (payslips[i].EmployeeId.Equals(model.EmployeeIds[j]))
                    {
                        string fcmToken = _employeeRepository.Get(_ => _.Id.Equals(payslips[i].EmployeeId)).FirstOrDefault().FCMToken;
                        if (!string.IsNullOrEmpty(fcmToken))
                        {
                            try
                            {
                                payslips[i].IsPublic = true;
                                _paySlipRepository.SaveChanges();

                                _firebaseCloudMessagingService.SendNotification(new Dictionary<string, object>
                            {
                                { "collapse_key", "PaySlip" },
                                { "title", "Phiếu lương" },
                                { "data", new Dictionary<string, Guid>
                                    {
                                        { "paySlipId", payslips[i].Id }
                                    }
                                },
                                { "to", fcmToken },
                                { "delay_while_idle", true },
                                //androidMessageDic.Add("time_to_live", 125);
                                { "notification", new Dictionary<string, string>
                                    {
                                        { "title", "Phiếu Lương"},
                                        { "subtitle", "subtitle"},
                                        { "body", "Phiếu lương mới cho kì lương " + payPeriod.Name.ToLower()}
                                    }
                                },
                                { "dry_run", false }
                            });

                                break;
                            }
                            catch (Exception e)
                            {
                                payslips[i].IsPublic = false;
                                _paySlipRepository.SaveChanges();
                                errorIds.Add(payslips[i].Id);
                            }
                        }
                    }
                }
            }

            

            return errorIds;
        }

        public IList<PaySlipNonPublicViewModel> GetNonPublic(Guid payPeriodId, Guid positionId)
        {
            var paySlips = _paySlipRepository
                .Get(_ => _.PayPeriodId.Equals(payPeriodId)
                    && _.Employee.PositionId.Equals(positionId)
                    && _.IsPublic == false);

            var employees = _employeeRepository
                .Get(_ => _.PositionId.Equals(positionId));

            IList<PaySlipNonPublicViewModel> result = new List<PaySlipNonPublicViewModel>();

            for (int i = 0; i < paySlips.Count; i++)
            {
                if (paySlips[i].Amount != 0)
                {
                    for (int j = 0; j < employees.Count; j++)
                    {
                        if (paySlips[i].EmployeeId.Equals(employees[j].Id))
                        {
                            result.Add(new PaySlipNonPublicViewModel
                            {
                                Id = paySlips[i].Id,
                                Amount = paySlips[i].Amount,
                                PaySlipCode = paySlips[i].PaySlipCode,
                                Employee = new EmployeeViewModel
                                {
                                    Id = employees[j].Id,
                                    Name = employees[j].Name
                                }
                            });
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }

    public interface IPaySlipService
    {
        Guid Add(PaySlipCreateModel model);
        bool Confirm(PaySlipConfirmViewModel model);
        IList<PaySlipPaySalaryErrorViewModel> PaySalary(PaySlipPaySalaryModel model);
        IList<PaySlipViewModel> GetAll(Guid employeeId);
        PaySlipDetailViewModel GetById(Guid paySlipId);
        int AddDraft(PaySlipDraftCreateModel model);
        IList<Guid> Public(PayslipPublicModel model);
        IList<PaySlipNonPublicViewModel> GetNonPublic(Guid payPeriodId, Guid positionId);
    }
}
