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

        public IList<SalaryModeViewModel> GetAll()
        {
            IList<SalaryMode> list = _salaryModeRepository.GetAll().ToList();
            IList<SalaryModeViewModel> result = new List<SalaryModeViewModel>();

            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new SalaryModeViewModel
                {
                    Id = list[i].Id,
                    Mode = list[i].Mode,
                });
            }
            return result;
        }
    }

    public interface ISalaryModeService
    {
        Guid Add(SalaryModeCreateModel model);
        IList<SalaryModeViewModel> GetAll();
    }
}
