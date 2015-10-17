using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        ///     Gets Account by Id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountModel Get(int id);

        /// <summary>
        ///     Gets Account by Login and Password from database
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AccountModel> Get(string login, string password);

        /// <summary>
        ///     Gets all accounts from database
        /// </summary>
        /// <returns></returns>
        IEnumerable<AccountModel> GetAllAccounts();

        /// <summary>
        ///     Gets accounts range using filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns></returns>
        Task<IEnumerable<AccountModel>> GetRange(Expression<Func<Account, bool>> filterCondition);


    }
}