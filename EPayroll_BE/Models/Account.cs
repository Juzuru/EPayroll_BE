using EPayroll_BE.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPayroll_BE.Models
{
    public class Account : ModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDeleted { get; set; }
    }
}
