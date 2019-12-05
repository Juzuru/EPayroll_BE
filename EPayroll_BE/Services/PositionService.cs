using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public Guid Add(PositionCreateModel model)
        {
            Position position = new Position
            {
                Name = model.Name
            };

            _positionRepository.Add(position);
            _positionRepository.SaveChanges();

            return position.Id;
        }

        public IList<PositionViewModel> GetAll()
        {
            IList<Position> list = _positionRepository.GetAll().OrderByDescending(
                _position => _position.Name).Reverse().ToList();
            IList<PositionViewModel> result = new List<PositionViewModel>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new PositionViewModel
                {
                    Id = list[i].Id,
                    Name = list[i].Name
                });
            }
            return result;
        }
    }

    public interface IPositionService
    {
        Guid Add(PositionCreateModel model);
        IList<PositionViewModel> GetAll();
    }
}
