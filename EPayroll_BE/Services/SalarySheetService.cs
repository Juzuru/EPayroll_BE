using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class SalarySheetService : ISalarySheetService
    {
        private readonly ISalarySheetRepository _salarySheetRepository;

        public SalarySheetService(ISalarySheetRepository salarySheetRepository)
        {
            _salarySheetRepository = salarySheetRepository;
        }

        public Guid Add(SalarySheetCreateModel model)
        {
            SalarySheet salarySheet = new SalarySheet
            {
                Name = model.Name,
                Amount = model.Amount,
                PaySlipId = model.PaySlipId,
                PayTypeId = model.PayTypeId,
                TotalWorking = model.TotalWorking,
                WorkingRate = model.WorkingRate
            };

            _salarySheetRepository.Add(salarySheet);
            _salarySheetRepository.SaveChanges();

            return salarySheet.Id;
        }
    }

    public interface ISalarySheetService
    {
        Guid Add(SalarySheetCreateModel model);
    }
}
