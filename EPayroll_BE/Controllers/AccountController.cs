using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.Utilities;
using EPayroll_BE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                List<AccountViewModel> result = new List<AccountViewModel>();
                foreach (var account in _accountRepository.GetAll().ToList())
                {
                    result.Add(new AccountViewModel
                    {
                        EmployeeId = account.EmployeeId,
                        IsRemove = account.IsRemove
                    });
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetById(string id)
        {
            try
            {
                Account account = _accountRepository.GetById(id);

                return Ok(new AccountViewModel
                { 
                    EmployeeId = account.EmployeeId,
                    IsRemove = account.IsRemove
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody]AccountCreateModel model)
        {
            try
            {
                _accountRepository.Add(new Account
                {
                    EmployeeId = model.EmployeeId,
                    Password = "123",
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

        [HttpPut]
        public ActionResult Update([FromBody]AccountUpdateModel model)
        {
            try
            {
                _accountRepository.Update(new Account
                {
                    EmployeeId = model.EmployeeId,
                    Password = model.Password
                });
                _accountRepository.SaveChanges();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            try
            {
                Account account = _accountRepository.GetById(id);
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

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody]AccountLoginModel model)
        {
            try
            {
                Account account = _accountRepository
                    .Get(_ => _.EmployeeId.Equals(model.EmployeeId) && _.Password.Equals(model.Password))
                    .FirstOrDefault();
                if (account == null) return Unauthorized();

                return Ok(new AccountAuthorizedModel {
                    Token = JWTUtilities.GenerateJwtToken(account.EmployeeId, new Claim[] { 
                        new Claim("EmployeeId", account.EmployeeId) 
                    })
                });

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("logout")]
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
    }
}