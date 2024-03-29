﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PositionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PositionCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
