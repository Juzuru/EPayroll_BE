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

        public EmployeeService(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, ISalaryModeRepository salaryModeRepository, ISalaryLevelRepository salaryLevelRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _salaryModeRepository = salaryModeRepository;
            _salaryLevelRepository = salaryLevelRepository;
        }

        public void Add(EmployeeCreateModel model, Guid accountId)
        {
            _employeeRepository.Add(new Employee
            {
                Age = model.Age,
                Gender = model.Gender,
                Id = accountId,
                IdentifyNumber = model.IdentifyNumber,
                IsDeleted = false,
                Name = model.Name,
                PositionId = model.PositionId,
                SalaryLevelId = model.SalaryLevelId,
                SalaryModeId = model.SalaryModeId
            });
            _employeeRepository.SaveChanges();
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
    }

    public interface IEmployeeService
    {
        void Add(EmployeeCreateModel model, Guid accountId);
        EmployeeDetailViewModel GetDetail(Guid employeeId);
    }
}
