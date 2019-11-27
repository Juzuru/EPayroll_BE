using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class SalaryModeService : ISalaryModeService
    {
        private readonly ISalaryModeRepository _salaryModeRepository;

        public SalaryModeService(ISalaryModeRepository salaryModeRepository)
        {
            _salaryModeRepository = salaryModeRepository;
        }

        public Guid Add(SalaryModeCreateModel model)
        {
            SalaryMode salaryMode = new SalaryMode
            {
                Mode = model.Mode
            };

            _salaryModeRepository.Add(salaryMode);
            _salaryModeRepository.SaveChanges();

            return salaryMode.Id;
        }
    }

    public interface ISalaryModeService
    {
        Guid Add(SalaryModeCreateModel model);
    }
}
