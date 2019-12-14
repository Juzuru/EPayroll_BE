﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayTypeViewModel
    {
    }

    public class PayTypeCreateModel
    {
        [Required]
        public string Name { get; set; }
        public int Order { get; set; }

        public Guid PayTypeCategoryId { get; set; }
    }
}
