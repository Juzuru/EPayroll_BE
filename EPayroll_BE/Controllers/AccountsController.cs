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
        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerResponse(200, typeof(AccountTokenModel), Description = "Return a token model")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Login([FromBody]AccountLoginModel model)
        {
            try
            {
                Guid? accountId = _accountService.GetByEmail(model.Email);
                if (accountId == null) accountId = _accountService.Add(model);
                return Ok(new AccountTokenModel
                {
                    TokenType = "Bearer",
                    Token = JWTUtilities.GenerateJwtToken(accountId.ToString(), new Claim[] {
                        new Claim("AccountId", accountId.ToString())
                    })
                });
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
        [HttpPatch]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult ChangePassword()
        {
            return StatusCode(501);
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