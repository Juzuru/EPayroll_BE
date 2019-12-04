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
    public class PayItemsController : ControllerBase
    {
        private readonly IPayItemService _payItemService;

        public PayItemsController(IPayItemService payItemService)
        {
            _payItemService = payItemService;
        }

        #region Get
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<PayItemViewModel>), Description = "Return all pay item  ")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll()
        {
            try
            {
                return Ok(_payItemService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        
        [HttpGet("{payItemId}")]
        [SwaggerResponse(200, typeof(PayItemViewModel), Description = "Return a pay item")]
        [SwaggerResponse(404, null, Description = "The pay item's id not exist")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetById([FromRoute]Guid payItemId)
        {
            try
            {
                PayItemViewModel result = _payItemService.GetById(payItemId);
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
        [SwaggerResponse(201, typeof(string), Description = "Return Id of created payItem")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]PayItemCreateModel model)
        {
            try
            {
                return StatusCode(201, _payItemService.Add(model, false));
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
        [HttpDelete("{pay_item_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeletePayItem()
        {
            return StatusCode(501);
        }
        #endregion
    }
}