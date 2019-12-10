using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.ViewModels.EmployeeShiftAPIViewModel
{
    public class ESAPIAttendanceReportViewModel
    {
        public string Total_work_time { get; set; }
        public int OriginalHour { get; set; }
        public int OverTime { get; set; }
        public DateTime Date { get; set; }
    }
}
