using EPayroll_BE.Data;
using EPayroll_BE.Models;

namespace EPayroll_BE.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(EPayrollContext context) : base(context) { }
    }

    public interface IAccountRepository : IRepositoryBase<Account>
    {

    }
}
