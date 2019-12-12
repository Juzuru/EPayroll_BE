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
        private readonly ISalaryTableRepository _salaryTableRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, ISalaryModeRepository salaryModeRepository, ISalaryLevelRepository salaryLevelRepository, IPaySlipRepository paySlipRepository, ISalaryTableRepository salaryTableRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _salaryModeRepository = salaryModeRepository;
            _salaryLevelRepository = salaryLevelRepository;
            _paySlipRepository = paySlipRepository;
            _salaryTableRepository = salaryTableRepository;
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
                EsapiEmployeeId = model.EsapiEmployeeId
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

        public IList<EmployeeListViewModel> GetAll(Guid? positionId = null)
        {
            IList<Employee> list;

            if (positionId != null)
            {
                list = _employeeRepository.Get(_ => _.PositionId.Equals(positionId));
            }
            else list = _employeeRepository.GetAll().ToList();

            if (list == null) return new List<EmployeeListViewModel>();
            
            IList<EmployeeListViewModel> result = new List<EmployeeListViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                Position position = _positionRepository.GetById(list[i].PositionId);
                SalaryMode salaryMode = _salaryModeRepository.GetById(list[i].SalaryModeId);
                SalaryLevel salaryLevel = _salaryLevelRepository.GetById(list[i].SalaryLevelId);
                SalaryTable salaryTable = _salaryTableRepository.GetById(salaryLevel.SalaryTableId);
                result.Add(new EmployeeListViewModel
                {
                    Id = list[i].Id,
                    Name = list[i].Name,
                    Gender = list[i].Gender,
                    Age = list[i].Age,
                    Email = list[i].Email,
                    UserUID = list[i].UserUID,
                    IdentifyNumber = list[i].IdentifyNumber,
                    IsDeleted = list[i].IsDeleted,
                    Position = new PositionViewModel
                    {
                        Id = position.Id,
                        Name = position.Name
                    },
                    SalaryLevel = new SalaryLevelViewModel
                    {
                        Id = salaryLevel.Id,
                        Level = salaryLevel.Level,
                        Condition = salaryLevel.Condition,
                        Factor =salaryLevel.Factor,
                        Order = salaryLevel.Order,
                        SalaryTable = new SalaryTableViewModel
                        {
                            Id = salaryTable.Id,
                            Name = salaryTable.Name,
                            StartDate = salaryTable.StartDate,
                            EndDate = salaryTable.EndDate,
                            CreatedDate =salaryTable.CreatedDate,
                            IsEnable = salaryTable.IsEnable
                        }
                    },
                    SalaryMode = new SalaryModeViewModel
                    {
                        Id = salaryMode.Id,
                        Mode = salaryMode.Mode
                    }

                });
            }
            return result;
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
                        Level = salaryLevel.Level,
                        Condition = salaryLevel.Condition,
                        Factor = salaryLevel.Factor,
                        Order = salaryLevel.Order,
                        SalaryTable = new SalaryTableViewModel
                        {

                        }
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
    }

    public interface IEmployeeService
    {
        Guid Add(EmployeeCreateModel model);
        Guid? CheckUser(EmployeeCheckUserModel model);
        EmployeeDetailViewModel GetDetail(Guid employeeId);
        IList<EmployeeListViewModel> GetAll(Guid? positionId = null);
        IList<EmployeeViewModelV2> GetNoPayslipEmployee(Guid payPeriodId, Guid positionId);
    }
}
