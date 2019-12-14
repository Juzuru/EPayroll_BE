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
        private readonly ISalaryLevelRepository _salaryLevelRepository;
        private readonly IPayTypeAmountRepository _payTypeAmountRepository;

        public SalaryTableService(ISalaryTableRepository salaryTableRepository, ISalaryLevelRepository salaryLevelRepository, IPayTypeAmountRepository payTypeAmountRepository)
        {
            _salaryTableRepository = salaryTableRepository;
            _salaryLevelRepository = salaryLevelRepository;
            _payTypeAmountRepository = payTypeAmountRepository;
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
                PositionId = model.PositionId
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

        public void Save(SalaryTableSaveModelV2 model)
        {
            if (model.SalaryLevels.Count != 0)
            {
                SalaryLevel salaryLevel;
                PayTypeAmount payTypeAmount;

                for (int i = 0; i < model.SalaryLevels.Count; i++)
                {
                    if (model.SalaryLevels[i].Id != null)
                    {
                        salaryLevel = _salaryLevelRepository.Get(_ => _.Id.Equals(model.SalaryLevels[i].Id)).FirstOrDefault();
                        salaryLevel.Condition = model.SalaryLevels[i].Condition;
                        salaryLevel.Factor = model.SalaryLevels[i].Factor;
                        salaryLevel.Level = model.SalaryLevels[i].Level;
                        salaryLevel.Order = i + 1;

                        _salaryLevelRepository.Update(salaryLevel);
                    }
                    else
                    {
                        salaryLevel = new SalaryLevel
                        {
                            Condition = model.SalaryLevels[i].Condition,
                            Factor = model.SalaryLevels[i].Factor,
                            Level = model.SalaryLevels[i].Level,
                            Order = i + 1,
                            SalaryTableId = model.Id
                        };
                        _salaryLevelRepository.Add(salaryLevel);
                    }

                    if (model.SalaryLevels[i].PayTypeAmounts.Count != 0)
                    {
                        for (int j = 0; j < model.SalaryLevels[i].PayTypeAmounts.Count; j++)
                        {
                            if (model.SalaryLevels[i].PayTypeAmounts[j].Id != null)
                            {
                                payTypeAmount = _payTypeAmountRepository.Get(_ => _.Id.Equals(model.SalaryLevels[i].PayTypeAmounts[j].Id)).FirstOrDefault();

                                payTypeAmount.Amount = model.SalaryLevels[i].PayTypeAmounts[j].Amount;
                                //IsIsMultiple
                                payTypeAmount.PayTypeId = model.SalaryLevels[i].PayTypeAmounts[j].PayTypeId;

                                _payTypeAmountRepository.Update(payTypeAmount);
                            }
                            else
                            {
                                _payTypeAmountRepository.Add(new PayTypeAmount
                                {
                                    Amount = model.SalaryLevels[i].PayTypeAmounts[j].Amount,
                                    //IsIsMultiple
                                    PayTypeId = model.SalaryLevels[i].PayTypeAmounts[j].PayTypeId,
                                    SalaryLevelId = salaryLevel.Id
                                });
                            }
                        }
                    }
                }

                _salaryTableRepository.SaveChanges();
            }
        }

        public IList<SalaryTableViewModel> GetNonPublic()
        {
            var salaryTables = _salaryTableRepository.Get(_ => _.IsEnable == false);

            IList<SalaryTableViewModel> result = new List<SalaryTableViewModel>();
            for (int i = 0; i < salaryTables.Count; i++)
            {
                result.Add(new SalaryTableViewModel
                {
                    Name = salaryTables[i].Name,
                    IsEnable = false,
                    CreatedDate = salaryTables[i].CreatedDate,
                    EndDate = salaryTables[i].EndDate,
                    Id = salaryTables[i].Id,
                    StartDate = salaryTables[i].StartDate
                });
            }

            return result;
        }

        public void Public(Guid[] salaryTableIds)
        {
            for (int i = 0; i < salaryTableIds.Length; i++)
            {
                var salaryTable = _salaryTableRepository.Get(_ => _.Id.Equals(salaryTableIds[i])).FirstOrDefault();
                salaryTable.IsEnable = true;
                _salaryTableRepository.Update(salaryTable);
            }

            _salaryTableRepository.SaveChanges();
        }
    }

    public interface ISalaryTableService
    {
        Guid Add(SalaryTableCreateModel model);
        IList<SalaryTableViewModel> GetAll();
        void Save(SalaryTableSaveModelV2 model);
        IList<SalaryTableViewModel> GetNonPublic();
        void Public(Guid[] salaryTableIds);
    }
}
