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
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<SalaryTableViewModel>), Description = "Return all salary table  ")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll()
        {
            try
            {
                return Ok(_salaryTableService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("non-public")]
        [SwaggerResponse(200, typeof(IList<SalaryTableViewModel>), Description = "Return a list od non-public salary table")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetNonPublic()
        {
            try
            {
                return Ok(_salaryTableService.GetNonPublic());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
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

        [HttpPost("public")]
        [SwaggerResponse(200, null, Description = "public successfully")]
        [SwaggerResponse(400, null, Description = "Missing salary table Id")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Public([FromBody]Guid[] salaryTableIds = null)
        {
            try
            {
                if (salaryTableIds == null) return BadRequest("Missing salary table Ids");
                else if (salaryTableIds.Length == 0) return BadRequest("Missing salary table Ids");

                _salaryTableService.Public(salaryTableIds);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
        [HttpDelete("{salary_table_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeleteSalaryTable()
        {
            return StatusCode(501);
        }
        #endregion
    }
}