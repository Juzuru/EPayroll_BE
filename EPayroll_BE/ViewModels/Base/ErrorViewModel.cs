using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels.Base
{
    public class Error400BadRequestBase
    {
        public Errors Errors { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public Guid TraceId { get; set; }
    }

    public class Errors
    {
        public IList<string> ErrorField { get; set; }
    }
}
