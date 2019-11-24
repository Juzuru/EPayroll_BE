using EPayroll_BE.Data;
using EPayroll_BE.Models;
using EPayroll_BE.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Repositories
{
    public class PayTypeAmountRepository : RepositoryBase<PayTypeAmount>, IPayTypeAmountRepository
    {
        public PayTypeAmountRepository(EPayrollContext context) : base(context) { }
    }

    public interface IPayTypeAmountRepository : IRepositoryBase<PayTypeAmount>
    {

    }
}
