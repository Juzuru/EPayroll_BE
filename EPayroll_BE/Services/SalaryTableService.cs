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
                CreatedDate = DateTime.Now
            };

            _salaryTableRepository.Add(salaryTable);
            _salaryTableRepository.SaveChanges();

            return salaryTable.Id;
        }

        public IList<SalaryTableViewModel> GetAll()
        {
            IList<SalaryTable> list = _salaryTableRepository.GetAll().ToList();
            IList<SalaryTableViewModel> result = new List<SalaryTableViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new SalaryTableViewModel
                {
                    Id = list[i].Id,
                    Name = list[i].Name,
                    StartDate = list[i].StartDate,
                    EndDate = list[i].EndDate,
                    CreatedDate = list[i].CreatedDate,
                    IsEnable = list[i].IsEnable,
                });
            }
            return result;
        }
    }

    public interface ISalaryTableService
    {
        Guid Add(SalaryTableCreateModel model);
        IList<SalaryTableViewModel> GetAll();
    }
}
