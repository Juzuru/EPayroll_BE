using EPayroll_BE.Data;
using EPayroll_BE.Models;
using EPayroll_BE.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Repositories
{
    public class SalaryModeRepository : RepositoryBase<SalaryMode>, ISalaryModeRepository
    {
        public SalaryModeRepository(EPayrollContext context) : base(context) { }
    }

    public interface ISalaryModeRepository : IRepositoryBase<SalaryMode>
    {

    }
}
