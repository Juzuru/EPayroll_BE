using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels
{
    public class FormularViewModel
    {
    }

    public class FormularCreateModel
    {
        [Required]
        public string Description { get; set; }
    }
}
