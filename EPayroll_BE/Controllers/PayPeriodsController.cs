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
    public class PayPeriodsController : ControllerBase
    {
        private readonly IPayPeriodService _payPeriodService;

        public PayPeriodsController(IPayPeriodService payPeriodService)
        {
            _payPeriodService = payPeriodService;
        }

        #region Get

        // Get a pay period by Id
        [HttpGet("{payPeriodId}")]
        [SwaggerResponse(200, typeof(PayPeriodDetailViewModel), Description ="Return a pay period")]
        [SwaggerResponse(404, null, Description ="The pay period 's id not exist")]
        [SwaggerResponse(500, null, Description ="Server error")]
        public ActionResult GetById([FromRoute]Guid payPeriodId)
        {
            try
            {
                PayPeriodDetailViewModel result = _payPeriodService.GetDetail(payPeriodId);
                if (result == null) return NotFound();
                else return Ok(result);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        // Get all pay periods
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<PayPeriodDetailViewModel>), Description = "Return all pay periods")]
       [SwaggerResponse(500, null, Description ="Server error")]
       public ActionResult GetAll()
        {
            try
            {
                return Ok(_payPeriodService.GetAll());
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(Guid), Description = "Return Id of created payPeriod")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(400, null, Description = "End date of pay period exceeds the end date of the last salary table. Please create a salary table first")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]PayPeriodCreateModel model)
        {
            try
            {
                var result = _payPeriodService.Add(model);
                if (result == null) return BadRequest("End date of pay period exceeds the end date of the last salary table. Please create a salary table first");
                return StatusCode(201, _payPeriodService.Add(model));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        [SwaggerResponse(501, null, Description ="Request not implemented")]
        public ActionResult Put()
        {
            return StatusCode(501);
        }
        #endregion

        #region Patch
        [HttpPatch]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult Patch()
        {
            return StatusCode(501);
        }
        #endregion

        #region Delete
        [HttpDelete("{payperiod_id}")]
        [SwaggerResponse(501, null, Description ="Request not implemented")]
        public ActionResult DeletePayPeriod()
        {
            return StatusCode(501);
        }
        #endregion
    }
}