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
    public class SalaryTablesController : ControllerBase
    {
        private readonly ISalaryTableService _salaryTableService;

        public SalaryTablesController(ISalaryTableService salaryTableService)
        {
            _salaryTableService = salaryTableService;
        }

        #region Get
        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(Guid), Description = "Return Id of created salaryTable")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]SalaryTableCreateModel model)
        {
            try
            {
                return StatusCode(201, _salaryTableService.Add(model));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("save")]
        [SwaggerResponse(200, null, Description = "Save successfully")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Save([FromBody]SalaryTableSaveModelV2 model)
        {
            try
            {
                _salaryTableService.Save(model);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Put
        #endregion

        #region Patch
        #endregion

        #region Delete
        #endregion
    }
}