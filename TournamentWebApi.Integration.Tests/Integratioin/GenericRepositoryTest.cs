using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TournamentWebApi.Core.Enums;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.Integration.Tests.Helpers;

namespace TournamentWebApi.Integration.Tests.Integratioin
{
    [TestFixture]
    public class GenericRepositoryTest
    {
        [SetUp]
        public void TestSetup()
        {
        }

        private readonly IGenericRepository<Player> _playerRepository;

        public GenericRepositoryTest()
        {
            var testDbHelper = new TestDbHelper();
            testDbHelper.CreateDbStructure();
            testDbHelper.SeedDbWithTestData();

            var testNInjectHelper = new TestNInjectHelper();
            var unitOfWork = testNInjectHelper.Kernel.Get<IUnitOfWork>();
            _playerRepository = unitOfWork.PlayerRepository;
        }

        [Test]
        public void GetAll_Returns23Records_Valid()
        {
            // Arrange
            // Act
            IEnumerable<Player> players = _playerRepository.GetAll();

            // Assert
            Assert.IsTrue(players.Count() == 23);
        }

        [Test]
        public void GetAll_Sorted_Ascending_CheckOrder_Valid()
        {
            // Arrange
            Expression<Func<Player, object>> sortFunc = p => p.PlayerId;

            // Act
            IEnumerable<Player> players = _playerRepository.GetAll(sortFunc, SortDirection.Ascending);
            List<Player> playersList = players.ToList();
            string lastPlayerLastName = playersList.Last().LastName;

            // Assert
            Assert.IsTrue(string.Equals(lastPlayerLastName, "Tkachenko", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetAll_Sorted_Descending_CheckOrder_Valid()
        {
            // Arrange
            Expression<Func<Player, object>> sortFunc = p => p.PlayerId;

            // Act
            IEnumerable<Player> players = _playerRepository.GetAll(sortFunc, SortDirection.Descending);
            List<Player> playersList = players.ToList();
            string firstPlayerLastName = playersList.First().LastName;

            // Assert
            Assert.IsTrue(string.Equals(firstPlayerLastName, "Tkachenko", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetRange_Filtered_And_Sorted_Ascending_CheckOrder_Valid()
        {
            // Arrange
            Expression<Func<Player, bool>> filterFunc = p => p.Rate > 0;
            Expression<Func<Player, object>> sortFunc = p => p.Rate;

            // Act
            IEnumerable<Player> players = _playerRepository.GetRange(filterFunc, sortFunc, SortDirection.Ascending);
            List<Player> playersList = players.ToList();
            string firstPlayerLastName = playersList.First().LastName;
            string lastPlayerLastName = playersList.Last().LastName;

            // Assert
            Assert.IsTrue(string.Equals(firstPlayerLastName, "Watson", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(lastPlayerLastName, "Aliokhin", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetRange_Filtered_And_Sorted_Ascending_With_Paging_CheckPageContentAndOrder_Valid()
        {
            // Arrange
            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 0;
            const int pageCapacity = 9;
            const int pageNumber = 3;
            Expression<Func<Player, object>> sortFunc = p => p.Rate;

            // Act
            IEnumerable<Player> players = _playerRepository.GetRange(filterFunc, pageCapacity, pageNumber, sortFunc,
                SortDirection.Ascending);
            List<Player> playersList = players.ToList();
            string firstPlayerLastName = playersList.First().LastName;
            string lastPlayerLastName = playersList.Last().LastName;

            // Assert
            Assert.IsTrue(string.Equals(firstPlayerLastName, "Gurkin", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(lastPlayerLastName, "Aliokhin", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetRange_Filtered_And_Sorted_Descending_CheckOrder_Valid()
        {
            // Arrange
            Expression<Func<Player, bool>> filterFunc = p => p.Rate > 0;
            Expression<Func<Player, object>> sortFunc = p => p.Rate;

            // Act
            IEnumerable<Player> players = _playerRepository.GetRange(filterFunc, sortFunc, SortDirection.Descending);
            List<Player> playersList = players.ToList();
            string firstPlayerLastName = playersList.First().LastName;
            string lastPlayerLastName = playersList.Last().LastName;

            // Assert
            Assert.IsTrue(string.Equals(firstPlayerLastName, "Aliokhin", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(lastPlayerLastName, "Watson", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetRange_Filtered_And_Sorted_Descending_With_Paging_CheckPageContentAndOrder_Valid()
        {
            // Arrange
            const int firstPlayer = 0;
            const int secondPlayer = 1;

            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 0;
            const int pageCapacity = 9;
            const int pageNumber = 1;
            Expression<Func<Player, object>> sortFunc = p => p.Rate;

            // Act
            IEnumerable<Player> players = _playerRepository.GetRange(filterFunc, pageCapacity, pageNumber, sortFunc,
                SortDirection.Descending);
            List<Player> playersList = players.ToList();
            string firstPlayerLastName = playersList[firstPlayer].LastName;
            string secondPlayerLastName = playersList[secondPlayer].LastName;

            // Assert
            Assert.IsTrue(string.Equals(firstPlayerLastName, "Aliokhin", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(secondPlayerLastName, "Lasker", StringComparison.OrdinalIgnoreCase));
        }
    }
}