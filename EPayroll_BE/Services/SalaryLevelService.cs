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
        private readonly ISalaryTableRepository _salaryTableRepository;

        public SalaryLevelService(ISalaryLevelRepository salaryLevelRepository,
            ISalaryTableRepository salaryTableRepository)
        {
            _salaryLevelRepository = salaryLevelRepository;
            _salaryTableRepository = salaryTableRepository;
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

        public IList<SalaryLevelViewModel> GetAll()
        {
            IList<SalaryLevel> list = _salaryLevelRepository.GetAll().OrderByDescending(
                _salaryLevel => _salaryLevel.Factor).ToList();
            IList<SalaryLevelViewModel> result = new List<SalaryLevelViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                SalaryTable salaryTable = _salaryTableRepository.GetById(list[i].SalaryTableId);
                result.Add(new SalaryLevelViewModel
                {
                    Id = list[i].Id,
                    Condition = list[i].Condition,
                    Factor = list[i].Factor,
                    Level = list[i].Level,
                    Order = list[i].Order,
                    SalaryTable = new SalaryTableViewModel
                    {
                        Id = salaryTable.Id,
                        Name = salaryTable.Name,
                        StartDate = salaryTable.StartDate,
                        EndDate = salaryTable.EndDate,
                        CreatedDate = salaryTable.CreatedDate,
                        IsEnable = salaryTable.IsEnable,
                    }
                });
            }
            return result;
        }
    }

    public interface ISalaryLevelService
    {
        Guid Add(SalaryLevelCreateModel model);
        IList<SalaryLevelViewModel> GetAll();
    }
}
