using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Guid Add(AccountCreateModel model)
        {
            Account account = new Account
            {
                EmployeeCode = model.EmployeeCode,
                IsDeleted = false,
                IsEnable = false,
                Password = model.Password
            };

            _accountRepository.Add(account);
            _accountRepository.SaveChanges();

            return account.Id;
        }

        public void ChangePassword(Guid accountId, string newPassword)
        {
            Account account = _accountRepository.GetById(accountId);
            account.Password = newPassword;

            _accountRepository.Update(account);
            _accountRepository.SaveChanges();
        }

        public Guid CheckLogin(AccountLoginModel model)
        {
            Account account = _accountRepository
                .Get(_account => _account.EmployeeCode.Equals(model.EmployeeCode) && _account.Password.Equals(model.Password))
                .FirstOrDefault();

            return account.Id;
        }

        public bool ContainsEmployeeCode(string employeeCode)
        {
            Account account = _accountRepository
                .Get(_account => _account.EmployeeCode.Equals(employeeCode))
                .FirstOrDefault();

            return account != null;
        }

        public bool Delete(Guid accountId)
        {
            Account account = _accountRepository.GetById(accountId);

            if (account != null)
            {
                account.IsDeleted = true;

                _accountRepository.Update(account);
                _accountRepository.SaveChanges();

                return true;
            }
            return false;
        }
    }

    public interface IAccountService
    {
        Guid Add(AccountCreateModel model);
        void ChangePassword(Guid accountId, string newPassword);
        Guid CheckLogin(AccountLoginModel model);
        bool ContainsEmployeeCode(string employeeCode);
        bool Delete(Guid accountId);
    }
}
