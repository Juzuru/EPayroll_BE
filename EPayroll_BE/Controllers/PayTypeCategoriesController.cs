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
    public class PayTypeCategoriesController : ControllerBase
    {
        private readonly IPayTypeCategoryRepository _payTypeCategoryRepository;

        public PayTypeCategoriesController(IPayTypeCategoryRepository payTypeCategoryRepository)
        {
            _payTypeCategoryRepository = payTypeCategoryRepository;
        }
    }
}