using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPayroll_BE.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPayroll_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryTablePositionsController : ControllerBase
    {
        private readonly ISalaryTablePositionRepository salaryTablePositionRepository;

        public SalaryTablePositionsController(ISalaryTablePositionRepository salaryTablePositionRepository)
        {
            this.salaryTablePositionRepository = salaryTablePositionRepository;
        }
    }
}