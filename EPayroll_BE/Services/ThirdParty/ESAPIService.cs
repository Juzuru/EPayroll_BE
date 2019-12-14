using EPayroll_BE.Services.Base;
using EPayroll_BE.ViewModels.EmployeeShiftAPIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services.ThirdParty
{
    public class ESAPIService : IESAPIService
    {
        private readonly IRequestService _requestService;

        public ESAPIService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public string GetTokenFromESAPI()
        {
            ESAPIAuthorizedModel authorizedModel = _requestService.Post<ESAPIAuthorizedModel>("http://employeeshiftapi.unicode.edu.vn/api/AspNetUsers/login", new ESAPILoginModel
            {
                Password = "1234567890",
                Username = "thuctcao"
            }, null);
            if (authorizedModel != null)
            {
                if (!string.IsNullOrEmpty(authorizedModel.Token))
                {
                    return authorizedModel.Token;
                }
            }
            return null;
        }

        public IList<ESAPIAttendanceReportViewModel> GetAttendanceReport(string token, int esapiEmployeeId, DateTime startDate, DateTime endDate)
        {
            IList<ESAPIAttendanceReportViewModel> reports = new List<ESAPIAttendanceReportViewModel>();
            ESAPIAttendanceReportViewModel report;
            int Total_work_time;
            bool flag = false;
            do
            {
                report = _requestService
                    .Get<ESAPIAttendanceReportViewModel>("http://employeeshiftapi.unicode.edu.vn/api/Attendance/get_list_report?EmployeeId="
                        + esapiEmployeeId.ToString()
                        + "&StartDate=" + startDate.ToString("yyyy/MM/dd")
                        + "&EndDate=" + startDate.ToString("yyyy/MM/dd"), token);
                Total_work_time = (int)Math.Round(double.Parse(report.Total_work_time.Replace(" hours", "")));
                if (Total_work_time != 0)
                {
                    flag = true;

                    if (Total_work_time > 8)
                    {
                        report.OriginalHour = 8;
                        report.OverTime = Total_work_time - 8;
                    }
                    else
                    {
                        report.OriginalHour = Total_work_time;
                        report.OverTime = 0;
                    }
                    report.Date = startDate;
                    reports.Add(report);
                }

                startDate = startDate.AddDays(1);
            } while (startDate <= endDate);

            if (flag) return reports;
            return null;
        }
    }

    public interface IESAPIService
    {
        string GetTokenFromESAPI();
        IList<ESAPIAttendanceReportViewModel> GetAttendanceReport(string token, int esapiEmployeeId, DateTime startDate, DateTime endDate);
    }
}
