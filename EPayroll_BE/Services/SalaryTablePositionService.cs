using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class SalaryTablePositionService : ISalaryTablePositionService
    {
        private readonly ISalaryTablePositionRepository _salaryTablePositionRepository;

        public SalaryTablePositionService(ISalaryTablePositionRepository salaryTablePositionRepository)
        {
            _salaryTablePositionRepository = salaryTablePositionRepository;
        }

        public Guid Add(SalaryTablePositionCreateModel model)
        {
            SalaryTablePosition salaryTablePosition = new SalaryTablePosition
            {
                PositionId = model.PositionId,
                SalaryTableId = model.SalaryTableId
            };

            _salaryTablePositionRepository.Add(salaryTablePosition);
            _salaryTablePositionRepository.SaveChanges();

            return salaryTablePosition.Id;
        }
    }

    public interface ISalaryTablePositionService
    {
        Guid Add(SalaryTablePositionCreateModel model);
    }
}
