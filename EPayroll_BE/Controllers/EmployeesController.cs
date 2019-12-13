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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NSwag.Annotations;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #region Get
        [HttpGet("{employeeId}")]
        [SwaggerResponse(200, typeof(EmployeeDetailViewModel), Description = "Return an employee")]
        [SwaggerResponse(404, null, Description = "The employee's id not exist")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetById([FromRoute]Guid employeeId)
        {
            try
            {
                EmployeeDetailViewModel result = _employeeService.GetDetail(employeeId);
                if (result == null) return NotFound();
                else return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // Get all
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<EmployeeListViewModel>), Description = "Return all employees")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll([FromQuery]Guid? positionId = null)
        {
            try
            {
                return Ok(_employeeService.GetAll(positionId));
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpGet("no-payslip")]
        [SwaggerResponse(400, null, Description = "Return missing parameters")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetNoPayslipEmployee([FromQuery]Guid? payPeriodId, [FromQuery]Guid? positionId)
        {
            try
            {
                if (payPeriodId == null) return BadRequest("Pay period ID is required");
                if (positionId == null) return BadRequest("Position ID is required");
                return Ok(_employeeService.GetNoPayslipEmployee(new Guid(payPeriodId.ToString()), new Guid(positionId.ToString())));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(Guid), Description = "Return created employee's id")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult CreateEmployee([FromBody]EmployeeCreateModel model)
        {
            try
            {
                return StatusCode(201, _employeeService.Add(model));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("check-user")]
        [SwaggerResponse(200, typeof(EmployeeAuthorizedModel), Description = "Return a token and employee's id corresponding with the email and user UID")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(404, null, Description = "The email is not allowed to access")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult CheckUser([FromBody]EmployeeCheckUserModel model)
        {
            try
            {
                Guid? employeeId = _employeeService.CheckUser(model);
                if (employeeId == null) return NotFound();

                return Ok(new EmployeeAuthorizedModel { 
                    Id = employeeId,
                    Token = JWTUtility.GenerateJwtToken(model.UserUID, new Claim[] {
                        new Claim("EmployeeId", employeeId.ToString()),
                    }),
                    TokenType = "Bearer"
                });
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
        public ActionResult Put()
        {
            return StatusCode(501);
        }
        #endregion

        #region Patch
        [HttpPatch]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult Update()
        {
            return StatusCode(501);
        }

        [HttpPatch("send-fcm-token")]
        [SwaggerResponse(200, null, Description = "Save Firebase Cloud Messaging successfully")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult SendFCMToken([FromBody]EmployeeSendFCMTokenModel model)
        {
            try
            {
                var result = _employeeService.UpdateFCMToken(model);
                if (result) return Ok();
                return StatusCode(500, "Could not save Firebase Cloud Messaging Token");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
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