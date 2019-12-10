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
    public class PayTypeCategoriesController : ControllerBase
    {
        private readonly IPayTypeCategoryService _payTypeCategoryService;

        public PayTypeCategoriesController(IPayTypeCategoryService payTypeCategoryService)
        {
            _payTypeCategoryService = payTypeCategoryService;
        }

        #region Get
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<PayTypeCategoryViewModel>), Description = "Return all pay type categories  ")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll()
        {
            try
            {
                return Ok(_payTypeCategoryService.GetAll());
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(Guid), Description = "Return Id of created payTypeCategory")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]PayTypeCategoryCreateModel model)
        {
            try
            {
                return StatusCode(201, _payTypeCategoryService.Add(model));
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
        [HttpDelete("{pay_type_category_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeletePayTypeCategory()
        {
            return StatusCode(501);
        }
        #endregion
    }
}