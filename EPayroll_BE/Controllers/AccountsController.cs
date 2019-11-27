using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPayroll_BE.Models;
using EPayroll_BE.Services;
using EPayroll_BE.Utilities;
using EPayroll_BE.ViewModels;
using EPayroll_BE.ViewModels.Base;
using EPayroll_BE.ViewModels.Pager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NSwag;
using NSwag.Annotations;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Get
        [HttpGet]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult Get()
        {
            return StatusCode(501);
        }
        #endregion

        #region Post
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(201, typeof(string), Description = "Return Id of created account")]
        [SwaggerResponse(400, typeof(AccountCreateErrorModel), Description = "Return an error model")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]AccountCreateModel model)
        {
            try
            {
                AccountCreateErrorModel errors = new AccountCreateErrorModel();
                bool error = false;

                if (model.EmployeeCode.Contains(" "))
                {
                    error = true;
                    errors.EmployeeCodeError = "EmployeeCode must not have any space";
                }
                if (model.Password.Contains(" ")) {
                    error = true;
                    errors.EmployeeCodeError = "Password must not have any space";
                }

                if (!error)
                {
                    if (_accountService.ContainsEmployeeCode(model.EmployeeCode))
                    {
                        errors.EmployeeCodeError = "EmployeeCode existed!!!";
                    }
                    else
                    {
                        return StatusCode(201, _accountService.Add(model));
                    }
                }
                return BadRequest(errors);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerResponse(200, typeof(AccountAuthorizedModel), Description = "Return an authorized model")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(401, null, Description = "Unauthorized user")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Login([FromBody]AccountLoginModel model)
        {
            try
            {
                Guid accountId = _accountService.CheckLogin(model);

                if (accountId == null) return Unauthorized();
                else
                {
                    return Ok(new AccountAuthorizedModel
                    {
                        TokenType = "Bearer",
                        Token = JWTUtilities.GenerateJwtToken(accountId.ToString(), new Claim[] {
                                new Claim("AccountId", accountId.ToString())
                            })
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("logout")]
        [SwaggerResponse(200, null, Description = "User logged out")]
        [SwaggerResponse(500, null, Description = "Server error")]
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
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult Update()
        {
            return StatusCode(501);
        }
        #endregion

        #region Patch
        [HttpPut("changePassword")]
        [SwaggerResponse(204, null, Description = "Password changed")]
        [SwaggerResponse(400, typeof(AccountChangePasswordErrorModel), Description = "Return an error model")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult ChangePassword([FromBody]AccountChangePasswordModel model)
        {
            try
            {
                AccountChangePasswordErrorModel errors = new AccountChangePasswordErrorModel();
                bool error = false;

                if (model.OldPassword.Contains(" "))
                {
                    error = true;
                    errors.OldPasswordError = "Old password must not have any space";
                }
                if (model.NewPassword.Contains(" "))
                {
                    error = true;
                    errors.NewPasswordError = "New password must have any space";
                }
                if (model.ConfirmNewPassword.Contains(" "))
                {
                    error = true;
                    errors.ConfirmNewPasswordError = "Confirm new password must have any space";
                }
                if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                {
                    error = true;
                    errors.ConfirmNewPasswordError = "Confirm new password must match the new password";
                }

                if (!error)
                {
                    if (Request.Headers.TryGetValue("Authorization", out StringValues value))
                    {
                        string accountId = JWTUtilities.GetClaimValueFromToken("AccountId", value.ToString());
                        _accountService.ChangePassword(new Guid(accountId), model.NewPassword);
                        return NoContent();
                    }
                    return StatusCode(500);
                }
                return BadRequest(errors);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{accountId}")]
        [SwaggerResponse(204, null, Description = "Account deleted")]
        [SwaggerResponse(404, null, Description = "Account Id is not found")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult DeleteById([FromRoute]Guid accountId)
        {
            try
            {
                if (_accountService.Delete(accountId)) return NoContent();
                else return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        #endregion
    }
}