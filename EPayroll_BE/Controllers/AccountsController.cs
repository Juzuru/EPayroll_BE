using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.Utilities;
using EPayroll_BE.ViewModels;
using EPayroll_BE.ViewModels.Pager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NSwag;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private static readonly int itemPerPage = 10;

        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        #region Get
        [HttpGet]
        [ProducesResponseType(200, StatusCode = 200, Type = typeof(PagingModel<AccountViewModel>))]
        [ProducesResponseType(400, StatusCode = 400, Type = typeof(string))]
        public ActionResult Get([FromQuery]int page = 1)
        {
            try
            {
                IList<Account> list = _accountRepository.TakeLast((page-1) * itemPerPage, itemPerPage);

                IList<AccountViewModel> listModel = new List<AccountViewModel>();
                foreach (Account account in list)
                {
                    listModel.Add(new AccountViewModel
                    {
                        Id = account.Id,
                        EmployeeCode = account.EmployeeCode,
                        IsRemove = account.IsRemove
                    });
                }

                return Ok(new PagingModel<AccountViewModel> {
                    RequestedPage = page,
                    ItemCount = listModel.Count,
                    ItemList = listModel,
                    TotalPage = _accountRepository.Count()/10
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{account_id}")]
        public ActionResult GetById(int account_id)
        {
            try
            {
                Account account = _accountRepository.GetById(account_id);

                return Ok(new AccountViewModel
                {
                    Id = account_id,
                    EmployeeCode = account.EmployeeCode,
                    IsRemove = account.IsRemove
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public ActionResult Post()
        {
            return StatusCode(501);
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody]AccountCreateModel model)
        {
            try
            {
                _accountRepository.Add(new Account
                {
                    EmployeeCode = model.EmployeeCode,
                    Password = model.Password,
                    IsRemove = false
                });
                _accountRepository.SaveChanges();

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody]AccountLoginModel model)
        {
            try
            {
                Account account = _accountRepository
                    .Get(_ => _.EmployeeCode.Equals(model.EmployeeCode) && _.Password.Equals(model.Password))
                    .FirstOrDefault();
                if (account == null) return Unauthorized();

                return Ok(new AccountAuthorizedModel
                {
                    Token = JWTUtilities.GenerateJwtToken(account.EmployeeCode, new Claim[] {
                        new Claim("EmployeeCode", account.EmployeeCode)
                    })
                });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out StringValues value))
                {
                    JWTUtilities.RemoveAudien(value.ToString());
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public ActionResult Update()
        {
            return StatusCode(501);
        }

        [HttpPut("change_password")]
        public ActionResult ChangePassword([FromBody]AccountChangePasswordModel model)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out StringValues value))
                {
                    string employeeCode = JWTUtilities.GetClaimValueFromToken("EmployeeCode", value.ToString());

                    Account account = _accountRepository
                        .Get(_ => _.EmployeeCode.Equals(employeeCode) && _.Password.Equals(model.OldPassword))
                        .FirstOrDefault();
                    if (account == null) return Unauthorized("Invalid employee's code or password!!!");

                    account.Password = model.NewPassword;
                    _accountRepository.Update(account);
                    _accountRepository.SaveChanges();

                    return NoContent();
                }
                return Unauthorized("Token is null!!!");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        public ActionResult Delete()
        {
            return StatusCode(501);
        }

        [HttpDelete("{account_id}")]
        public ActionResult DeleteById(int account_id)
        {
            try
            {
                Account account = _accountRepository.GetById(account_id);
                if (account == null) return BadRequest();

                account.IsRemove = true;

                _accountRepository.Update(account);
                _accountRepository.SaveChanges();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        #endregion
    }
}