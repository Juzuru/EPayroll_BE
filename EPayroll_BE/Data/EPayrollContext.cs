using EPayroll_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EPayroll_BE.Data
{
    public class EPayrollContext : DbContext
    {
        public EPayrollContext(DbContextOptions<EPayrollContext> option) : base(option) { }

        public virtual DbSet<Account> Account { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=DRP;persist security info=True;Integrated Security=False;TrustServerCertificate=False;uid=sa;password=zaq@123;Trusted_Connection=False;MultipleActiveResultSets=true;");
                optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=EPayroll;
                    persist security info=True;Integrated Security=False;TrustServerCertificate=False;
                    uid=sa;password=maxsulapro0701;Trusted_Connection=False;MultipleActiveResultSets=true;");
            }
        }
    }
}
