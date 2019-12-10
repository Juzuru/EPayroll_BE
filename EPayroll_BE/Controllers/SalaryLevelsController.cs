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
    public class SalaryLevelsController : ControllerBase
    {
        private readonly ISalaryLevelService _salaryLevelService;

        public SalaryLevelsController(ISalaryLevelService salaryLevelService)
        {
            _salaryLevelService = salaryLevelService;
        }

        #region Get
        [HttpGet]
        [SwaggerResponse(200, typeof(IList<SalaryLevelViewModel>), Description = "Return all salary levels  ")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult GetAll()
        {
            try
            {
                return Ok(_salaryLevelService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [SwaggerResponse(201, typeof(Guid), Description = "Return Id of created salaryLevel")]
        [SwaggerResponse(400, typeof(Error400BadRequestBase), Description = "Return fields require")]
        [SwaggerResponse(500, null, Description = "Server error")]
        public ActionResult Add([FromBody]SalaryLevelCreateModel model)
        {
            try
            {
                return StatusCode(201, _salaryLevelService.Add(model));
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
        [HttpDelete("{salary_level_id}")]
        [SwaggerResponse(501, null, Description = "Request not implemented")]
        public ActionResult DeleteSalaryLevel()
        {
            return StatusCode(501);
        }
        #endregion
    }
}