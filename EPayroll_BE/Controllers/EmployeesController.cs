using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using EPayroll_BE.ViewModels.Pager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private static readonly int itemPerPage = 10;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly ISalaryModeRepository _salaryModeRepository;
        private readonly ISalaryLevelRepository _salaryLevelRepository;
        private readonly IPaySlipRepository _paySlipRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IAccountRepository _accountRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, ISalaryModeRepository salaryModeRepository, ISalaryLevelRepository salaryLevelRepository, IPaySlipRepository paySlipRepository, IPayPeriodRepository payPeriodRepository, IAccountRepository accountRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _salaryModeRepository = salaryModeRepository;
            _salaryLevelRepository = salaryLevelRepository;
            _paySlipRepository = paySlipRepository;
            _payPeriodRepository = payPeriodRepository;
            _accountRepository = accountRepository;
        }

        #region Get
        [HttpGet]
        //[SwaggerResponse(200, typeof(PagingModel<EmployeeViewModel>), Description = "Return a list of 10 newest employee")]
        //[SwaggerResponse(500, null, Description = "Server error")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult Get()
        {
            return StatusCode(501);
        }

        [HttpGet("{employee_id}")]
        [SwaggerResponse(200, typeof(EmployeeDetailViewModel), Description = "Return an employee")]
        [SwaggerResponse(404, null, Description = "The employee's id not exist")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetById([FromRoute]int employee_id)
        {
            try
            {
                Employee employee = _employeeRepository.GetById(employee_id);
                if (employee != null)
                {
                    Position position = _positionRepository.GetById(employee.PositionId);
                    SalaryMode salaryMode = _salaryModeRepository.GetById(employee.SalaryModeId);
                    SalaryLevel salaryLevel = _salaryLevelRepository.GetById(employee.SalaryLevelId);

                    return Ok(new EmployeeDetailViewModel
                    {
                        Id = employee_id,
                        Age = employee.Age,
                        Gender = employee.Gender,
                        IdentifyNumber = employee.IdentifyNumber,
                        Name = employee.Name,
                        Position = new PositionViewModel
                        {
                            Id = position.Id,
                            Name = position.Name
                        },
                        SalaryLevel = new SalaryLevelViewModel
                        {
                            Id = salaryLevel.Id,
                            Level = salaryLevel.Level
                        },
                        SalaryMode = new SalaryModeViewModel
                        {
                            Id = salaryMode.Id,
                            Mode = salaryMode.Mode
                        }
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{employee_id}/PaySlips/")]
        [SwaggerResponse(200, typeof(PagingModel<PaySlipViewModel>), Description = "Return all payslips of an employee order by date in descending")]
        [SwaggerResponse(404, null, Description = "The employee's id not exist")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetPaySlips([FromRoute]int employee_id, [FromQuery]int page = 1)
        {
            try
            {
                Employee employee = _employeeRepository.GetById(employee_id);
                if (employee != null)
                {
                    IList<PaySlip> list = _paySlipRepository.Get(_ => _.EmployeeId == employee_id);
                    int count = list.Count;
                    list = list.SkipLast((page - 1) * itemPerPage).TakeLast(itemPerPage).Reverse().ToList();
                    IList<PaySlipViewModel> listModel = new List<PaySlipViewModel>();
                    PayPeriod payPeriod;

                    foreach (PaySlip paySlip in list)
                    {
                        payPeriod = _payPeriodRepository.GetById(paySlip.PayPeriodId);

                        listModel.Add(new PaySlipViewModel
                        {
                            CreatedDate = paySlip.CreatedDate,
                            Id = paySlip.Id,
                            Name = paySlip.Name,
                            PayPeriod = new PayPeriodViewModel
                            {
                                Id = payPeriod.Id,
                                Name = payPeriod.Name
                            }
                        });
                    }

                    return Ok(new PagingModel<PaySlipViewModel>
                    {
                        ItemCount = listModel.Count,
                        ItemList = listModel,
                        RequestedPage = page,
                        TotalPage = count % itemPerPage == 0 ? count / itemPerPage : count / itemPerPage + 1
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Post
        [HttpPost("create_employee")]
        [SwaggerResponse(201, typeof(int), Description = "Return created employee's id")]
        [SwaggerResponse(400, typeof(string), Description = "Return below error message:\n - The employee code is existed")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult CreateEmployee([FromBody]EmployeeCreateModel model)
        {
            try
            {
                Account account = _accountRepository.Get(_ => _.EmployeeCode.Equals(model.Account.EmployeeCode)).FirstOrDefault();
                if (account == null)
                {
                    Employee employee = new Employee
                    {
                        Age = model.Age,
                        Gender = model.Gender,
                        IdentifyNumber = model.IdentifyNumber,
                        Name = model.Name,
                        PositionId = model.PositionId,
                        SalaryLevelId = model.SalaryLevelId,
                        SalaryModeId = model.SalaryModeId
                    };
                    _employeeRepository.Add(employee);

                    _accountRepository.Add(new Account
                    {
                        EmployeeCode = model.Account.EmployeeCode,
                        EmployeeId = employee.Id,
                        IsRemove = false,
                        Password = model.Account.Password
                    });

                    _employeeRepository.SaveChanges();

                    return CreatedAtAction("CreateEmployee", employee.Id);
                }
                else
                {
                    return BadRequest("The employee code is existed");
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Put
        [HttpPut("{employee_id}/Positions/change")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult ChangePosition([FromBody]int position_id)
        {
            return StatusCode(501);
        }

        [HttpPut("{employee_id}/SalaryLevels/change")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult ChangeSalaryLevel([FromBody]int salaryLevelId)
        {
            return StatusCode(501);
        }
        #endregion

        #region Delete
        [HttpDelete("{employee_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeleteEmployee()
        {
            return StatusCode(501);
        }
        #endregion
    }
}