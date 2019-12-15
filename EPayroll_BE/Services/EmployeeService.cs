using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly ISalaryModeRepository _salaryModeRepository;
        private readonly ISalaryLevelRepository _salaryLevelRepository;
        private readonly IPaySlipRepository _paySlipRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, ISalaryModeRepository salaryModeRepository, ISalaryLevelRepository salaryLevelRepository, IPaySlipRepository paySlipRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _salaryModeRepository = salaryModeRepository;
            _salaryLevelRepository = salaryLevelRepository;
            _paySlipRepository = paySlipRepository;
        }

        public Guid Add(EmployeeCreateModel model)
        {
            Employee employee = new Employee
            {
                Age = model.Age,
                Gender = model.Gender,
                IdentifyNumber = model.IdentifyNumber,
                IsDeleted = false,
                Name = model.Name,
                PositionId = model.PositionId,
                SalaryLevelId = model.SalaryLevelId,
                SalaryModeId = model.SalaryModeId,
                Email = model.Email,
                UserUID = model.UserUID,
                EsapiEmployeeId = model.EsapiEmployeeId,
                StartWorkDate = model.StartWorkDate == null ? DateTime.Now : model.StartWorkDate.GetValueOrDefault()
            };

            _employeeRepository.Add(employee);
            _employeeRepository.SaveChanges();

            return employee.Id;
        }

        public Guid? CheckUser(EmployeeCheckUserModel model)
        {
            Employee employee = _employeeRepository
                .Get(_employee => _employee.Email.Equals(model.Email) && _employee.UserUID.Equals(model.UserUID))
                .FirstOrDefault();
            if (employee == null) return null;
            return employee.Id;
        }

        public EmployeeDetailViewModel GetDetail(Guid employeeId)
        {
            Employee employee = _employeeRepository.GetById(employeeId);
            if (employee != null)
            {
                Position position = _positionRepository.GetById(employee.PositionId);
                SalaryMode salaryMode = _salaryModeRepository.GetById(employee.SalaryModeId);
                SalaryLevel salaryLevel = _salaryLevelRepository.GetById(employee.SalaryLevelId);

                return new EmployeeDetailViewModel
                {
                    Id = employee.Id,
                    Age = employee.Age,
                    Gender = employee.Gender,
                    IdentifyNumber = employee.IdentifyNumber,
                    Name = employee.Name,
                    Position = new PositionViewModel
                    {
                        Id = position.Id,
                        Name = position.Name
                    },
                    SalaryLevel = new SalaryLevelViewModel
                    {
                        Id = salaryLevel.Id,
                        Level = salaryLevel.Level
                    },
                    SalaryMode = new SalaryModeViewModel
                    {
                        Id = salaryMode.Id,
                        Mode = salaryMode.Mode
                    }
                };
            }
            return null;
        }

        public IList<EmployeeViewModelV2> GetNoPayslipEmployee(Guid payPeriodId, Guid positionId)
        {
            var payslips = _paySlipRepository.Get(_ => _.PayPeriodId.Equals(payPeriodId) && _.Employee.PositionId.Equals(positionId));
            var employees = _employeeRepository.Get(_ => _.PositionId.Equals(positionId));

            IList<EmployeeViewModelV2> result = new List<EmployeeViewModelV2>();
            bool flag;
            for (int i = 0; i < employees.Count; i++)
            {
                flag = false;
                for (int j = 0; j < payslips.Count; j++)
                {
                    if (employees[i].Id.Equals(payslips[j].EmployeeId))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    result.Add(new EmployeeViewModelV2
                    {
                        Id = employees[i].Id,
                        Email = employees[i].Email,
                        Name = employees[i].Name
                    });
                }
            }

            return result;
        }

        public bool UpdateFCMToken(EmployeeSendFCMTokenModel model)
        {
            try
            {
                var employee = _employeeRepository.Get(_ => _.Id.Equals(model.EmployeeId)).FirstOrDefault();

                employee.FCMToken = model.FCMToken;

                _employeeRepository.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public interface IEmployeeService
    {
        Guid Add(EmployeeCreateModel model);
        Guid? CheckUser(EmployeeCheckUserModel model);
        EmployeeDetailViewModel GetDetail(Guid employeeId);
        IList<EmployeeViewModelV2> GetNoPayslipEmployee(Guid payPeriodId, Guid positionId);
        bool UpdateFCMToken(EmployeeSendFCMTokenModel model);
    }
}
