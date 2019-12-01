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
        private readonly IPositionRepository _positionRepository;
        private readonly ISalaryTableRepository _salaryTableRepository;

        public SalaryTablePositionService(ISalaryTablePositionRepository salaryTablePositionRepository, IPositionRepository positionRepository,
            ISalaryTableRepository salaryTableRepository)
        {
            _salaryTablePositionRepository = salaryTablePositionRepository;
            _positionRepository = positionRepository;
            _salaryTableRepository = salaryTableRepository;

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

        public IList<SalaryTablePositionViewModel> GetAll()
        {
            IList<SalaryTablePosition> list = _salaryTablePositionRepository.GetAll()
                .OrderByDescending(_salaryTablePosition => _salaryTablePosition.Position).Reverse().ToList();
            IList<SalaryTablePositionViewModel> result = new List<SalaryTablePositionViewModel>();
            Position position;
            SalaryTable salaryTable;
            for (int i = 0; i < list.Count; i++)
            {
                position = _positionRepository.GetById(list[i].PositionId);
                salaryTable = _salaryTableRepository.GetById(list[i].SalaryTableId);
                result.Add(new SalaryTablePositionViewModel
                {
                    Id = list[i].Id,
                   Position = new PositionViewModel
                   {
                       Id = position.Id,
                       Name = position.Name
                   },
                   SalaryTable = new SalaryTableViewModel
                   {
                    Id = salaryTable.Id,
                    Name = salaryTable.Name,
                    StartDate = salaryTable.StartDate,
                    EndDate = salaryTable.EndDate,
                    CreatedDate = salaryTable.CreatedDate,
                    IsEnable= salaryTable.IsEnable
                   }
                });
            }
            return result;
        }
    }

    public interface ISalaryTablePositionService
    {
        Guid Add(SalaryTablePositionCreateModel model);
        IList<SalaryTablePositionViewModel> GetAll();
    }
}
