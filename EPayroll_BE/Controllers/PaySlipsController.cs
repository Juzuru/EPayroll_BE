using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPayroll_BE.Services;
using EPayroll_BE.ViewModels;
using EPayroll_BE.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaySlipsController : ControllerBase
    {
        private readonly IPaySlipService _paySlipService;

        public PaySlipsController(IPaySlipService paySlipService)
        {
            _paySlipService = paySlipService;
        }

        #region Get
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<PaySlipViewModel>), Description = "Return all payslip order by created date")]
        [SwaggerResponse(400, null, Description = "Require employeeId in query string")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll([FromQuery]Guid employeeId)
        {
            try
            {
                if (employeeId == null) return BadRequest();

                return Ok(_paySlipService.GetAll(employeeId));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        //
        [HttpGet("{paySlipId}")]
        [SwaggerResponse(200, typeof(PaySlipDetailViewModel), Description = "Return a pay slip by paySlipId")]
        [SwaggerResponse(404, null, Description = "The paySlipId's id not exist")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetById([FromRoute]Guid paySlipId)
        {
            try
            {
                PaySlipDetailViewModel result = _paySlipService.GetById(paySlipId);
                if (result == null) return NotFound();
                else return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(Guid), Description = "Return Id of created paySlip")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]PaySlipCreateModel model)
        {
            try
            {
                return StatusCode(201, _paySlipService.Add(model));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("pay-salary")]
        [SwaggerResponse(200, null, Description = "Successful implemented")]
        [SwaggerResponse(400, null, Description = "Return error employee IDs")]
        [SwaggerResponse(500, null, Description = "Server error")]
        [SwaggerResponse(502, null, Description = "The Employee Shift API not available")]
        public ActionResult PaySalary([FromBody]PaySlipGenerateFullModel model)
        {
            try
            {
                IList<Guid> errorIds = _paySlipService.PaySalary(model);
                if (errorIds == null) return StatusCode(502);
                else if (errorIds.Count == 0) return Ok();
                return StatusCode(400, errorIds);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // CreateWithPositionId
        [HttpPost("/CreateWithPositionId")]
        [SwaggerResponse(201, typeof(PaySlipCreateResult), Description = "Return Id of created pay period and position")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(404, null, Description = "The positionId's id not exist")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult AddDraft([FromBody]PayPeriodCreateModel model)
        {
            try
            {
                PaySlipCreateResult result = _paySlipService.AddDraft(model);
                if (result == null) return NotFound();
                else return Ok(result);
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
        #endregion

        #region Delete
        [HttpDelete("{payslip_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeletePaySlips()
        {
            return StatusCode(501);
        }
        #endregion
    }
}