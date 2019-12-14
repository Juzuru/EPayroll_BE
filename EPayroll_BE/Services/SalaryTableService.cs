using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class SalaryTableService : ISalaryTableService
    {
        private readonly ISalaryTableRepository _salaryTableRepository;

        public SalaryTableService(ISalaryTableRepository salaryTableRepository)
        {
            _salaryTableRepository = salaryTableRepository;
        }

        public Guid Add(SalaryTableCreateModel model)
        {
            SalaryTable salaryTable = new SalaryTable
            {
                IsEnable = false,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CreatedDate = DateTime.Now,
                IsDraft = true
            };

            _salaryTableRepository.Add(salaryTable);
            _salaryTableRepository.SaveChanges();

            return salaryTable.Id;
        }
    }

    public interface ISalaryTableService
    {
        Guid Add(SalaryTableCreateModel model);
    }
}
