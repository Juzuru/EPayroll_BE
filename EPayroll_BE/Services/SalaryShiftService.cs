using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class SalaryShiftService : ISalaryShiftService
    {
        private readonly ISalaryShiftRepository _salaryShiftRepository;
        private readonly IPaySlipRepository _paySlipRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;

        public SalaryShiftService(ISalaryShiftRepository salaryShiftRepository,
            IPaySlipRepository paySlipRepository,
            IEmployeeRepository employeeRepository,
            IPayPeriodRepository payPeriodRepository)
        {
            _salaryShiftRepository = salaryShiftRepository;
            _paySlipRepository = paySlipRepository;
            _employeeRepository = employeeRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public Guid Add(SalaryShiftCreateModel model)
        {
            SalaryShift salaryShift = new SalaryShift
            {
                Date = model.Date,
                OriginalHour = model.OriginalHour,
                OverTimeHour = model.OverTimeHour,
                PaySlipId = model.PaySlipId
            };

            _salaryShiftRepository.Add(salaryShift);
            _salaryShiftRepository.SaveChanges();

            return salaryShift.Id;
        }

        public IList<SalaryShiftViewModel> GetAll()
        {
            IList<SalaryShift> list = _salaryShiftRepository.GetAll().ToList();
            IList<SalaryShiftViewModel> result = new List<SalaryShiftViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                PaySlip paySlip = _paySlipRepository.GetById(list[i].PaySlipId);
                Employee employee = _employeeRepository.GetById(paySlip.EmployeeId);
                PayPeriod payPeriod = _payPeriodRepository.GetById(paySlip.PayPeriodId);
                result.Add(new SalaryShiftViewModel
                {
                    Id = list[i].Id,
                    Date = list[i].Date,
                    OriginalHour = list[i].OriginalHour,
                    OverTimeHour = list[i].OriginalHour,
                    PaySlipViewModel = new PaySlipViewModel
                    {
                        Id = paySlip.Id,
                        Amount = paySlip.Amount,
                        PaySlipCode = paySlip.PaySlipCode,
                        Status = paySlip.Status,
                        Employee = new EmployeeViewModel
                        {
                            Id = employee.Id,
                            Name = employee.Name
                        },
                        PayPeriod = new PayPeriodViewModel
                        {
                            Id = payPeriod.Id,
                            Name = payPeriod.Name,
                            StartDate = payPeriod.StartDate,
                            EndDate = payPeriod.EndDate,
                            PayDate = payPeriod.PayDate,
                        }
                    }
                });
            }
            return result;
        }
    }

    public interface ISalaryShiftService
    {
        Guid Add(SalaryShiftCreateModel model);
        IList<SalaryShiftViewModel> GetAll();
    }
}
