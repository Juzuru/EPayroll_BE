﻿using System;
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
    public class PaySlipsController : ControllerBase
    {
        private readonly IPaySlipRepository _paySlipRepository;

        public PaySlipsController(IPaySlipRepository paySlipRepository)
        {
            _paySlipRepository = paySlipRepository;
        }
    }
}