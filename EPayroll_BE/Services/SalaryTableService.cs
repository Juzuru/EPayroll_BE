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
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public SalaryTableService(ISalaryTableRepository salaryTableRepository, ISalaryLevelRepository salaryLevelRepository, IPayTypeAmountRepository payTypeAmountRepository, IPayTypeRepository payTypeRepository, IEmployeeRepository employeeRepository)
        {
            _salaryTableRepository = salaryTableRepository;
            _salaryLevelRepository = salaryLevelRepository;
            _payTypeAmountRepository = payTypeAmountRepository;
            _payTypeRepository = payTypeRepository;
            _employeeRepository = employeeRepository;
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
                        salaryLevel.Year = model.SalaryLevels[i].Year;

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
                            SalaryTableId = model.Id,
                            Year = model.SalaryLevels[i].Year
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

                                _payTypeAmountRepository.Update(payTypeAmount);
                            }
                            else
                            {
                                _payTypeAmountRepository.Add(new PayTypeAmount
                                {
                                    Amount = model.SalaryLevels[i].PayTypeAmounts[j].Amount,
                                    PayTypeId = model.SalaryLevels[i].PayTypeAmounts[j].PayTypeId,
                                    SalaryLevelId = salaryLevel.Id,
                                    Order = _payTypeRepository.Get(_ => _.Id.Equals(model.SalaryLevels[i].PayTypeAmounts[j].PayTypeId)).FirstOrDefault().Order
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
                var salaryTable = _salaryTableRepository
                    .Get(_ => _.Id.Equals(salaryTableIds[i]))
                    .FirstOrDefault();
                var lastSalaryTable = _salaryTableRepository
                    .Get(_ => _.IsEnable == true && _.PositionId.Equals(salaryTable.PositionId))
                    .FirstOrDefault();
                if (lastSalaryTable != null)
                {
                    lastSalaryTable.IsEnable = false;
                    _salaryTableRepository.Update(lastSalaryTable);
                }
                salaryTable.IsEnable = true;
                _salaryTableRepository.Update(salaryTable);

                var employees = _employeeRepository.Get(_ => _.PositionId.Equals(salaryTable.PositionId));
                var salaryLevels = _salaryLevelRepository.Get(_ => _.SalaryTableId.Equals(salaryTable.Id));
                int year;
                for (int j = 0; j < employees.Count; j++)
                {
                    DateTime now = DateTime.Now;
                    year = now.Year - employees[j].StartWorkDate.Year;
                    if (employees[j].StartWorkDate.AddYears(year) > now)
                        year--;

                    for (int k = 0; k < salaryLevels.Count; k++)
                    {
                        if (year == salaryLevels[k].Year)
                        {
                            employees[j].SalaryLevelId = salaryLevels[k].Id;
                            _employeeRepository.Update(employees[j]);
                            break;
                        }
                    }
                }
            }

            _salaryTableRepository.SaveChanges();
        }
    }

    public interface ISalaryTableService
    {
        Guid Add(SalaryTableCreateModel model);
        void Save(SalaryTableSaveModelV2 model);
        IList<SalaryTableViewModel> GetNonPublic();
        void Public(Guid[] salaryTableIds);
    }
}
