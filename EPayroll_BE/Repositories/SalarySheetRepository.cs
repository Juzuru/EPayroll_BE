using EPayroll_BE.Data;
using EPayroll_BE.Models;
using EPayroll_BE.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Repositories
{
    public class SalarySheetRepository : RepositoryBase<SalarySheet>, ISalarySheetRepository
    {
        public SalarySheetRepository(EPayrollContext context) : base(context) { }
    }

    public interface ISalarySheetRepository : IRepositoryBase<SalarySheet>
    {

    }
}
