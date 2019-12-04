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

        public SalaryShiftService(ISalaryShiftRepository salaryShiftRepository)
        {
            _salaryShiftRepository = salaryShiftRepository;
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
    }

    public interface ISalaryShiftService
    {
        Guid Add(SalaryShiftCreateModel model);
    }
}
