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
    public class FormularsController : ControllerBase
    {
        private readonly IFormularService _formularService;

        public FormularsController(IFormularService formularService)
        {
            _formularService = formularService;
        }

        #region Get
        // Get all
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<FormularViewModel>), Description = "Return all formular  ")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll()
        {
            try
            {
                return Ok(_formularService.GetAll());
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(string), Description = "Return Id of created formular")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]FormularCreateModel model)
        {
            try
            {
                return StatusCode(201, _formularService.Add(model));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Put
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
        [HttpDelete("{formular_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeleteFormular()
        {
            return StatusCode(501);
        }
        #endregion
    }
}