using AutoMapper;
using Ninject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.Integration.Tests.Helpers;
using TournamentWebApi.WEB.Mappings;

namespace TournamentWebApi.Integration.Tests.Integratioin
{
    [TestFixture]
    public class AccountRepositoryTest
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Role> _roleRepository;

        public AccountRepositoryTest()
        {
            //var testDbHelper = new TestDbHelper();
            //testDbHelper.CreateDbStructure();
            //testDbHelper.SeedDbWithTestData();

            var testNInjectHelper = new TestNInjectHelper();
            var unitOfWork = testNInjectHelper.Kernel.Get<IUnitOfWork>();
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
            Mapping.InitMapping();
        }

        [Test]
        public void GetAll_Returns4Records_Valid()
        {
            // Arrange
            // Act
            IEnumerable<Account> accounts = _accountRepository.GetAll();
            List<AccountModel> accountModels = Mapper.Map<IEnumerable<AccountModel>>(accounts).ToList();
            var roles = accountModels.First().Roles.ToList();
            var role1 = roles[0].Name;
            var role2 = roles[1].Name;

            // Assert
            Assert.IsTrue(accounts.Count() == 4);
        }

        [Test]
        public void GetAdmins_Returns2Records_Valid()
        {
            // Arrange
            // Act
            IEnumerable<Role> roles = _roleRepository.GetAll();
            var adminRole = roles.First();
            var accName1 = adminRole.Accounts.First().FirstName;
            var accName2 = adminRole.Accounts.Last().FirstName;
            

            // Assert
            Assert.IsTrue(adminRole.Accounts.Count() == 2);
        }
    }
}