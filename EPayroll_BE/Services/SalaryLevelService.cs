using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class SalaryLevelService : ISalaryLevelService
    {
        private readonly ISalaryLevelRepository _salaryLevelRepository;

        public SalaryLevelService(ISalaryLevelRepository salaryLevelRepository)
        {
            _salaryLevelRepository = salaryLevelRepository;
        }

        public Guid Add(SalaryLevelCreateModel model)
        {
            SalaryLevel salaryLevel = new SalaryLevel
            {
                Level = model.Level,
                Order = model.Order,
                Factor = model.Factor,
                Condition = model.Condition,
                SalaryTableId = model.SalaryTableId,
            };

            _salaryLevelRepository.Add(salaryLevel);
            _salaryLevelRepository.SaveChanges();

            return salaryLevel.Id;
        }
    }

    public interface ISalaryLevelService
    {
        Guid Add(SalaryLevelCreateModel model);
    }
}
