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

        [HttpPost]
        [SwaggerResponse(200, null, Description = "All payslips have been fully genarated")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GenerateFullPayslip([FromBody]Guid[] employeeIds)
        {
            try
            {
                _paySlipService.GenerateFullPayslip(employeeIds);
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