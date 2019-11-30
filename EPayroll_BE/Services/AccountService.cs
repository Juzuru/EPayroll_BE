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

        public Guid Add(AccountLoginModel model)
        {
            Account account = new Account
            {
                Name = model.Name,
                Email = model.Email,
                Picture = model.Picture,
                IsDeleted = false,
                IsEnable = false,
            };

            _accountRepository.Add(account);
            _accountRepository.SaveChanges();

            return account.Id;
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

        public Guid? GetByEmail(string email)
        {
            Account account = _accountRepository.Get(_account => _account.Email.Equals(email)).FirstOrDefault();
            if (account == null) return null;
            return account.Id;
        }
    }

    public interface IAccountService
    {
        Guid Add(AccountLoginModel model);
        bool Delete(Guid accountId);
        Guid? GetByEmail(string email);
    }
}
