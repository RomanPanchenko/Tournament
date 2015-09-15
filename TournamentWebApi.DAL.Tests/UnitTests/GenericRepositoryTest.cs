using Moq;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TournamentWebApi.Core.Enums;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Repositories;
using TournamentWebApi.DAL.Tests.AuxiliaryClasses;

namespace TournamentWebApi.DAL.Tests.UnitTests
{
    [TestFixture]
    public class GenericRepositoryTest
    {
        private IEnumerable<Player> GetPlayers()
        {
            var players = new List<Player>
            {
                new Player
                {
                    PlayerId = 1,
                    FirstName = "Roman",
                    LastName = "Panchenko",
                    RegistrationTime = new DateTime(2015, 07, 28, 14, 29, 0),
                    Rate = 0
                },
                new Player
                {
                    PlayerId = 2,
                    FirstName = "Nick",
                    LastName = "Lototskiy",
                    RegistrationTime = new DateTime(2015, 07, 28, 14, 30, 0),
                    Rate = 0
                },
                new Player
                {
                    PlayerId = 3,
                    FirstName = "Alexey",
                    LastName = "Kravtsov",
                    RegistrationTime = new DateTime(2015, 07, 28, 14, 31, 0),
                    Rate = 0
                },
                new Player
                {
                    PlayerId = 4,
                    FirstName = "Kirill",
                    LastName = "Antonov",
                    RegistrationTime = new DateTime(2015, 07, 28, 15, 11, 0),
                    Rate = 0
                },
                new Player
                {
                    PlayerId = 5,
                    FirstName = "Varvara",
                    LastName = "Strichenko",
                    RegistrationTime = new DateTime(2015, 07, 29, 11, 15, 0),
                    Rate = 0
                },
                new Player
                {
                    PlayerId = 6,
                    FirstName = "Harry",
                    LastName = "Kasparov",
                    RegistrationTime = new DateTime(2015, 07, 29, 11, 20, 0),
                    Rate = 5
                },
            };

            return players;
        }

        [Test]
        public void AddRange_Adds_Entities_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactionMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactionMock.Object);
            sessionMock.Setup(s => s.Save(It.IsAny<object>()));

            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            playerRepository.AddRange(players);

            // Assert
            //sessionMock.Verify(s => s.Save(It.IsAny<object>()), Times.Exactly(players.Count));
            transactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Test]
        public void Add_Adds_Entity_Pass()
        {
            // Arrange
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactionMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactionMock.Object);
            sessionMock.Setup(s => s.Save(It.IsAny<object>()));

            var player = new Player();
            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            playerRepository.Add(player);

            // Assert
            sessionMock.Verify(s => s.Save(It.IsAny<object>()), Times.Once);
            transactionMock.Verify(s => s.Commit(), Times.Once);
        }

        [Test]
        public void DeleteRange_Deletes_Entities_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactonMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactonMock.Object);
            sessionMock.Setup(s => s.Delete(It.IsAny<Player>()));
            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            playerRepository.DeleteRange(players);

            // Assert
            sessionMock.Verify(s => s.Delete(It.IsAny<Player>()), Times.Exactly(players.Count));
            transactonMock.Verify(s => s.Commit(), Times.Once);
        }

        [Test]
        public void Delete_Deletes_Entity_ById_Pass()
        {
            // Arrange
            const int someId = 5;
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactonMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactonMock.Object);
            sessionMock.Setup(s => s.Get<Player>(It.IsAny<int>())).Returns(new Player());
            sessionMock.Setup(s => s.Delete(It.IsAny<int>()));

            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            playerRepository.Delete(someId);

            // Assert
            sessionMock.Verify(s => s.Get<Player>(It.IsAny<int>()), Times.Once);
            sessionMock.Verify(s => s.Delete(It.IsAny<object>()), Times.Once);
            transactonMock.Verify(s => s.Commit(), Times.Once);
        }

        [Test]
        public void Delete_Deletes_Entity_Pass()
        {
            // Arrange
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactonMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactonMock.Object);
            sessionMock.Setup(s => s.Delete(It.IsAny<Player>()));

            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);
            var player = new Player();

            // Act
            playerRepository.Delete(player);

            // Assert
            sessionMock.Verify(s => s.Delete(It.IsAny<Player>()), Times.Once);
            transactonMock.Verify(s => s.Commit(), Times.Once);
        }

        [Test]
        public void GetAll_Gets_All_Entities_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            // Act
            IEnumerable<Player> playersColl = playerRepository.GetAll();

            // Assert
            Assert.IsTrue(playersColl.Count() == players.Count());
        }

        [Test]
        public void GetAll_Gets_All_Entities_Sorted_Asc_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, object>> sortFunc = s => s.FirstName;

            // Act
            List<Player> playersSortedByAsc = playerRepository.GetAll(sortFunc, SortDirection.Ascending).ToList();

            // Assert
            Assert.IsTrue(playersSortedByAsc.Count() == players.Count());
            Assert.IsTrue(string.Equals(playersSortedByAsc.First().FirstName, "Alexey"));
            Assert.IsTrue(string.Equals(playersSortedByAsc.Last().FirstName, "Varvara"));
        }

        [Test]
        public void GetAll_Gets_All_Entities_Sorted_Desc_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, object>> sortFunc = s => s.FirstName;

            // Act
            List<Player> playersSortedByDesc = playerRepository.GetAll(sortFunc, SortDirection.Descending).ToList();

            // Assert
            Assert.IsTrue(playersSortedByDesc.Count() == players.Count());
            Assert.IsTrue(string.Equals(playersSortedByDesc.First().FirstName, "Varvara"));
            Assert.IsTrue(string.Equals(playersSortedByDesc.Last().FirstName, "Alexey"));
        }

        [Test]
        public void GetAll_Gets_All_Entities_Sorted_Desc_With_NULL_Arg_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, object>> sortFunc = null;

            // Act
            List<Player> playersSortedByDesc = playerRepository.GetAll(sortFunc, SortDirection.Descending).ToList();

            // Assert
            Assert.IsTrue(playersSortedByDesc.Count() == players.Count);
        }

        [Test]
        public void GetPagesCount_Gets_Filtered_Pages_Count_Pass()
        {
            // Arrange
            const int pageCapacity = 1;
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> funcFilter = p => p.PlayerId > 4;

            // Act
            int pagesCount = playerRepository.GetPagesCount(funcFilter, pageCapacity);

            // Assert
            Assert.AreEqual(pagesCount, 2);
        }

        [Test]
        [ExpectedException(typeof(DivideByZeroException))]
        public void GetPagesCount_Gets_Pages_Count_Fail()
        {
            // Arrange
            const int pageCapacity = 0;
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            // Act
            int pagesCount = playerRepository.GetPagesCount(pageCapacity);

            // Assert
        }

        [Test]
        public void GetPagesCount_Gets_Pages_Count_Pass()
        {
            // Arrange
            const int pageCapacity = 5;
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            // Act
            int pagesCount = playerRepository.GetPagesCount(pageCapacity);

            // Assert
            Assert.AreEqual(pagesCount, 2);
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_And_Sorted_And_PageNumberOutOfRange_Pass()
        {
            // Arrange
            const int pageCapacity = 3;
            const int pageNumber = 300;

            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 4;
            Expression<Func<Player, object>> sortFunc = p => p.FirstName;

            // Act
            List<Player> playersResultCollection =
                playerRepository.GetRange(filterFunc, pageCapacity, pageNumber, sortFunc, SortDirection.Ascending)
                    .ToList();

            // Assert
            Assert.IsTrue(playersResultCollection.Count == 0);
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_And_Sorted_And_Paged_Pass()
        {
            // Arrange
            const int pageCapacity = 3;
            const int pageNumber = 2;

            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 0;
            Expression<Func<Player, object>> sortFunc = p => p.FirstName;

            // Act
            List<Player> playersResultCollection =
                playerRepository.GetRange(filterFunc, pageCapacity, pageNumber, sortFunc, SortDirection.Ascending)
                    .ToList();

            // Assert
            Assert.IsTrue(playersResultCollection.Count == 3);
            Assert.IsTrue(string.Equals(playersResultCollection.First().LastName, "Lototskiy"));
            Assert.IsTrue(string.Equals(playersResultCollection.Last().LastName, "Strichenko"));
        }


        [Test]
        public void GetRange_Gets_Entities_Filtered_And_Sorted_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 4;
            Expression<Func<Player, object>> sortFunc = p => p.FirstName;

            // Act
            List<Player> playersResultCollection =
                playerRepository.GetRange(filterFunc, sortFunc, SortDirection.Ascending).ToList();

            // Assert
            Assert.IsTrue(playersResultCollection.Count == 2);
            Assert.IsTrue(string.Equals(playersResultCollection.First().FirstName, "Harry",
                StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(playersResultCollection.Last().FirstName, "Varvara",
                StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_And_Sorted_With_Filter_NULL_and_Sort_NULL_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = null;
            Expression<Func<Player, object>> sortFunc = null;

            // Act
            List<Player> playersResultCollection =
                playerRepository.GetRange(filterFunc, sortFunc, SortDirection.Ascending).ToList();

            // Assert
            Assert.IsTrue(playersResultCollection.Count == players.Count);
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_And_Sorted_With_Filtered_NULL_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = null;
            Expression<Func<Player, object>> sortFunc = p => p.FirstName;

            // Act
            List<Player> playersResultCollection =
                playerRepository.GetRange(filterFunc, sortFunc, SortDirection.Ascending).ToList();

            // Assert
            Assert.IsTrue(playersResultCollection.Count == players.Count);
            Assert.IsTrue(string.Equals(playersResultCollection.First().FirstName, "Alexey",
                StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(playersResultCollection.Last().FirstName, "Varvara",
                StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_And_Sorted_With_Sort_NULL_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 4;
            Expression<Func<Player, object>> sortFunc = null;

            // Act
            List<Player> playersResultCollection =
                playerRepository.GetRange(filterFunc, sortFunc, SortDirection.Ascending).ToList();

            // Assert
            Assert.IsTrue(playersResultCollection.Count == 2);
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = p => p.PlayerId > 4;

            // Act
            IEnumerable<Player> playersFiltered = playerRepository.GetRange(filterFunc);

            // Assert
            Assert.IsTrue(playersFiltered.Count() == 2);
        }

        [Test]
        public void GetRange_Gets_Entities_Filtered_With_NULL_Arg_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> filterFunc = null;

            // Act
            IEnumerable<Player> playersFiltered = playerRepository.GetRange(filterFunc);

            // Assert
            Assert.IsTrue(playersFiltered.Count() == players.Count);
        }

        [Test]
        public void GetRecordsCount_Gets_Entities_Count_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            // Act
            int recordsCount = playerRepository.GetRecordsCount();

            // Assert
            Assert.AreEqual(recordsCount, players.Count);
        }

        [Test]
        public void GetRecordsCount_Gets_Filtered_Entities_Count_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);

            var playerRepository = new PlayerRepository(sessionFactoryMock.Object) { Players = players };

            Expression<Func<Player, bool>> funcFilter = p => p.PlayerId > 4;

            // Act
            int recordsCount = playerRepository.GetRecordsCount(funcFilter);

            // Assert
            Assert.AreEqual(recordsCount, 2);
        }

        [Test]
        public void Get_Gets_Entity_ById_Pass()
        {
            // Arrange
            const int someId = 5;
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.Get<Player>(It.IsAny<int>())).Returns(new Player());

            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            Player player = playerRepository.Get(someId);

            // Assert
            sessionMock.Verify(s => s.Get<Player>(It.IsAny<object>()), Times.Once);
            Assert.NotNull(player);
        }

        [Test]
        public void UpdateRange_Updates_Entities_Pass()
        {
            // Arrange
            List<Player> players = GetPlayers().ToList();
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactionMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactionMock.Object);
            sessionMock.Setup(s => s.SaveOrUpdate(It.IsAny<object>()));

            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            playerRepository.UpdateRange(players);

            // Assert
            sessionMock.Verify(s => s.SaveOrUpdate(It.IsAny<object>()), Times.Exactly(players.Count));
            transactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Test]
        public void Update_Updates_Entity_Pass()
        {
            // Arrange
            var sessionFactoryMock = new Mock<ISessionFactory>();
            var sessionMock = new Mock<ISession>();
            var transactionMock = new Mock<ITransaction>();

            sessionFactoryMock.Setup(s => s.OpenSession()).Returns(sessionMock.Object);
            sessionMock.Setup(s => s.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(transactionMock.Object);
            sessionMock.Setup(s => s.SaveOrUpdate(It.IsAny<object>()));

            var player = new Player();
            var playerRepository = new GenericRepository<Player>(sessionFactoryMock.Object);

            // Act
            playerRepository.Update(player);

            // Assert
            sessionMock.Verify(s => s.SaveOrUpdate(It.IsAny<object>()), Times.Once);
            transactionMock.Verify(s => s.Commit(), Times.Once);
        }
    }
}