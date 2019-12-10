using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.Services.Base;
using EPayroll_BE.Utilities;
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

        public IList<Guid> PaySalary(PaySlipPaySalaryModel model)
        {
            IList<Guid> errorIds = new List<Guid>();

            string token = GetTokenFromESAI();
            if (token == null) return null;

            Employee employee;
            PayPeriod payPeriod = _payPeriodRepository.GetById(model.PayPeriodId);
            PaySlip paySlip;

            for (int i = 0; i < model.EmployeeIds.Count; i++)
            {
                employee = _employeeRepository.GetById(model.EmployeeIds[i]);

                var reports = GetAttendanceReport(token, employee.EsapiEmployeeId, payPeriod.StartDate.Date, payPeriod.EndDate.Date, out int[] workHoursOfDay);
                paySlip = _paySlipRepository
                    .Get(_ => _.PayPeriodId.Equals(payPeriod.Id)
                            && _.EmployeeId.Equals(model.EmployeeIds[i]))
                    .FirstOrDefault();

                if ((workHoursOfDay[0] + workHoursOfDay[1] + workHoursOfDay[2] + workHoursOfDay[3]) == 0)
                {
                    _paySlipRepository.Delete(paySlip);
                    _paySlipRepository.SaveChanges();
                }
                else
                {
                    if (AddReportToSalaryShift(reports, paySlip.Id))
                    {
                        IList<PayTypeAmount> payTypeAmounts = _payTypeAmountRepository
                            .Get(_ => _.SalaryLevelId.Equals(employee.SalaryLevelId));

                        if (AddPayItem(paySlip.Id, workHoursOfDay, payTypeAmounts, reports.Count, out long totalAmount))
                        {
                            if (UpdatePayslip(paySlip, totalAmount))
                            {
                                _paySlipRepository.SaveChanges();
                            }
                            else errorIds.Add(model.EmployeeIds[i]);
                        }
                        else errorIds.Add(model.EmployeeIds[i]);
                    }
                    else errorIds.Add(model.EmployeeIds[i]);
                }
            }

            return errorIds;
        }

        public int AddDraft(PaySlipDraftCreateModel model)
        {
            Position position = _positionRepository.GetById(model.PositionId);
            if (position != null)
            {
                PayPeriod payPeriod = _payPeriodRepository.GetById(model.PayPeriodId);
                if (payPeriod == null) return -2;

                IList<Employee> employees = _employeeRepository.GetAll().
                    Where(_emp => _emp.PositionId.Equals(model.PositionId)).ToList();
                string paySlipCode = payPeriod.EndDate.Month < 9 ? "0" + payPeriod.EndDate.Month : "" + payPeriod.EndDate.Month;
                paySlipCode += payPeriod.EndDate.Year % 100 + StringGenerationUtility.GenerateCode();

                foreach (var item in employees)
                {
                    PaySlip paySlip = new PaySlip
                    {
                        PayPeriodId = model.PayPeriodId,
                        EmployeeId = item.Id,
                        Status = "Draft",
                        CreatedDate = DateTime.Now,
                        PaySlipCode = paySlipCode + item.Email.Substring(item.Email.IndexOf("@") - 7, 7).ToUpper()
                    };
                    _paySlipRepository.Add(paySlip);
                    _paySlipRepository.SaveChanges();
                }
                return 0;
            }
            return -1;
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

        private string GetTokenFromESAI()
        {
            ESAPIAuthorizedModel authorizedModel = _requestService.Post<ESAPIAuthorizedModel>("http://employeeshiftapi.unicode.edu.vn/api/AspNetUsers/login", new ESAPILoginModel
            {
                Password = "1234567890",
                Username = "thuctcao"
            }, null);
            if (authorizedModel != null)
            {
                if (!string.IsNullOrEmpty(authorizedModel.Token))
                {
                    return authorizedModel.Token;
                }
            }
            return null;
        }
        private IList<ESAPIAttendanceReportViewModel> GetAttendanceReport(string token, int esapiEmployeeId, DateTime startDate, DateTime endDate, out int[] workHoursOfDay)
        {
            IList<ESAPIAttendanceReportViewModel> reports = new List<ESAPIAttendanceReportViewModel>();
            ESAPIAttendanceReportViewModel report;
            workHoursOfDay = new int[4] { 0, 0, 0, 0};

            int Total_work_time;
            do
            {
                report = _requestService
                    .Get<ESAPIAttendanceReportViewModel>("http://employeeshiftapi.unicode.edu.vn/api/Attendance/get_list_report?EmployeeId=" 
                        + esapiEmployeeId.ToString() 
                        + "&StartDate=" + startDate.ToString("yyyy/MM/dd") 
                        + "&EndDate=" + startDate.ToString("yyyy/MM/dd"), token);
                Total_work_time = (int)Math.Round(Double.Parse(report.Total_work_time.Replace(" hours", "")));
                if (Total_work_time != 0)
                {
                    if (Total_work_time > 8)
                    {
                        report.OriginalHour = 8;
                        report.OverTime = Total_work_time - 8;
                    }
                    else
                    {
                        report.OriginalHour = Total_work_time;
                        report.OverTime = 0;
                    }
                    report.Date = startDate;
                    reports.Add(report);

                    if (startDate.DayOfWeek.ToString().StartsWith("S"))
                    {
                        workHoursOfDay[2] += report.OriginalHour;
                        workHoursOfDay[3] += report.OverTime;
                    }
                    else
                    {
                        workHoursOfDay[0] += report.OriginalHour;
                        workHoursOfDay[1] += report.OverTime;
                    }
                }
                
                startDate = startDate.AddDays(1);
            } while (startDate <= endDate);

            return reports;
        }
        private bool AddReportToSalaryShift(IList<ESAPIAttendanceReportViewModel> reports, Guid payslipId)
        {
            try
            {
                for (int i = 0; i < reports.Count; i++)
                {
                    _salaryShiftRepository.Add(new SalaryShift
                    {
                        Date = reports[i].Date,
                        OriginalHour = reports[i].OriginalHour,
                        OverTimeHour = reports[i].OverTime,
                        PaySlipId = payslipId
                    });
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool AddPayItem(Guid payslipId, int[] workHoursOfDay, IList<PayTypeAmount> payTypeAmounts, int numberOfDayWork, out long totalAmount)
        {
            totalAmount = 0;
            try
            {
                for (int i = 0; i < payTypeAmounts.Count; i++)
                {
                    if (payTypeAmounts[i].PayTypeId.Equals(new Guid("46c7d713-7eab-4cf1-b213-08d778b68b17")))
                    {
                        if (workHoursOfDay[0] > 0)
                        {
                            totalAmount += workHoursOfDay[0] * payTypeAmounts[i].Amount;
                            _payItemRepository.Add(new PayItem
                            {
                                PaySlipId = payslipId,
                                HourRate = payTypeAmounts[i].Amount,
                                PayTypeId = payTypeAmounts[i].PayTypeId,
                                Amount = workHoursOfDay[0] * payTypeAmounts[i].Amount,
                                TotalHour = workHoursOfDay[0]
                            });
                        }
                    }
                    else if (payTypeAmounts[i].PayTypeId.Equals(new Guid("1af87aee-e305-4dbd-cde7-08d77955b16e")))
                    {
                        if (workHoursOfDay[1] > 0)
                        {
                            totalAmount += workHoursOfDay[1] * payTypeAmounts[i].Amount;
                            _payItemRepository.Add(new PayItem
                            {
                                PaySlipId = payslipId,
                                HourRate = payTypeAmounts[i].Amount,
                                PayTypeId = payTypeAmounts[i].PayTypeId,
                                Amount = workHoursOfDay[1] * payTypeAmounts[i].Amount,
                                TotalHour = workHoursOfDay[1]
                            });
                        }
                    }
                    else if (payTypeAmounts[i].PayTypeId.Equals(new Guid("e4ccffc7-3226-483c-537b-08d77db52365")))
                    {
                        if (workHoursOfDay[2] > 0)
                        {
                            totalAmount += workHoursOfDay[2] * payTypeAmounts[i].Amount;
                            _payItemRepository.Add(new PayItem
                            {
                                PaySlipId = payslipId,
                                HourRate = payTypeAmounts[i].Amount,
                                PayTypeId = payTypeAmounts[i].PayTypeId,
                                Amount = workHoursOfDay[2] * payTypeAmounts[i].Amount,
                                TotalHour = workHoursOfDay[2]
                            });
                        }
                    }
                    else if (payTypeAmounts[i].PayTypeId.Equals(new Guid("2b2a58aa-ee42-450a-537c-08d77db52365")))
                    {
                        if (workHoursOfDay[3] > 0)
                        {
                            totalAmount += workHoursOfDay[3] * payTypeAmounts[i].Amount;
                            _payItemRepository.Add(new PayItem
                            {
                                PaySlipId = payslipId,
                                HourRate = payTypeAmounts[i].Amount,
                                PayTypeId = payTypeAmounts[i].PayTypeId,
                                Amount = workHoursOfDay[3] * payTypeAmounts[i].Amount,
                                TotalHour = workHoursOfDay[3]
                            });
                        }
                    }
                    else
                    {
                        long l = (payTypeAmounts[i].Amount * numberOfDayWork / 30);
                        l -= (l % 1000);

                        totalAmount += l;
                        _payItemRepository.Add(new PayItem
                        {
                            PaySlipId = payslipId,
                            HourRate = l,
                            PayTypeId = payTypeAmounts[i].PayTypeId,
                            Amount = l,
                            TotalHour = 1
                        });
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool UpdatePayslip(PaySlip paySlip, long totalAmount)
        {
            try
            {
                paySlip.Status = "Unpaid";
                paySlip.Amount = totalAmount;

                _paySlipRepository.Update(paySlip);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public interface IPaySlipService
    {
        Guid Add(PaySlipCreateModel model);
        IList<PaySlipViewModel> GetAll(Guid employeeId);
        PaySlipDetailViewModel GetById(Guid paySlipId);
        int AddDraft(PaySlipDraftCreateModel model);
    }
}
