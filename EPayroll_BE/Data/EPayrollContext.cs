using EPayroll_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EPayroll_BE.Data
{
    public class EPayrollContext : DbContext
    {
        public EPayrollContext(DbContextOptions<EPayrollContext> option) : base(option) { }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Formular> Formular { get; set; }
        public virtual DbSet<PayItem> PayItem { get; set; }
        public virtual DbSet<PayPeriod> PayPeriod { get; set; }
        public virtual DbSet<PaySlip> PaySlip { get; set; }
        public virtual DbSet<PayType> PayType { get; set; }
        public virtual DbSet<PayTypeAmount> PayTypeAmount { get; set; }
        public virtual DbSet<PayTypeCategory> PayTypeCategory { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<SalaryLevel> SalaryLevel { get; set; }
        public virtual DbSet<SalaryMode> SalaryMode { get; set; }
        public virtual DbSet<SalarySheet> SalarySheet { get; set; }
        public virtual DbSet<SalaryTable> SalaryTable { get; set; }
        public virtual DbSet<SalaryTablePosition> SalaryTablePosition { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=DRP;persist security info=True;Integrated Security=False;TrustServerCertificate=False;uid=sa;password=zaq@123;Trusted_Connection=False;MultipleActiveResultSets=true;");
                optionsBuilder.UseSqlServer(@"Data Source=45.119.83.107;Initial Catalog=EPayroll;
                    persist security info=True;Integrated Security=False;TrustServerCertificate=False;
                    uid=sa;password=sa@123456;Trusted_Connection=False;MultipleActiveResultSets=true;");
            }
        }
    }
}
