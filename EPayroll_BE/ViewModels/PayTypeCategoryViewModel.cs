using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class PayTypeCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PayTypeCategoryCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
