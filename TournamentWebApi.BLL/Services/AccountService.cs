using System.Linq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepository;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _accountRepository = unitOfWork.AccountRepository;
        }

        public AccountModel Get(int id)
        {
            Account account = _accountRepository.Get(id);
            var accountModel = Mapper.Map<AccountModel>(account);
            return accountModel;
        }

        public AccountModel Get(string login, string password)
        {
            IEnumerable<Account> accounts = _accountRepository.GetRange(p =>
                p.Login.Equals(login, StringComparison.OrdinalIgnoreCase) && p.Password.Equals(password));

            var accountModels = Mapper.Map<IEnumerable<AccountModel>>(accounts);
            AccountModel accountModel = accountModels.FirstOrDefault();

            return accountModel;
        }

        public IEnumerable<AccountModel> GetAllAccounts()
        {
            IEnumerable<Account> accounts = _accountRepository.GetAll();
            var accountModels = Mapper.Map<IEnumerable<AccountModel>>(accounts);
            return accountModels;
        }

        public IEnumerable<AccountModel> GetRange(Expression<Func<Account, bool>> filterCondition)
        {
            IEnumerable<Account> accounts = _accountRepository.GetRange(filterCondition);
            var accountModels = Mapper.Map<IEnumerable<AccountModel>>(accounts);
            return accountModels;
        }
    }
}