using AutoMapper;
using NUnit.Framework;
using System.Collections.Generic;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.BLL.Services;
using TournamentWebApi.DAL.UnitsOfWorks;
using TournamentWebApi.WEB.Mappings;

namespace TournamentWebApi.BLL.Tests.UnitTests
{
    [TestFixture]
    public class PlayerServiceTest
    {
        private IPlayerService _playerService;

        [SetUp]
        public void SetUp()
        {
            Mapping.InitMapping();
            Mapper.AssertConfigurationIsValid();

            _playerService = new PlayerService(new UnitOfWork());
        }

        [Test]
        public void GetCollectionOfAllPlayerModels_Passed()
        {
            IEnumerable<PlayerModel> playerModels = _playerService.GetAllPlayers();
            Assert.NotNull(playerModels);
        }

        [Test]
        public void GetPlayerModelWithId2_Passed()
        {
            PlayerModel playerModel = _playerService.Get(2);
            Assert.NotNull(playerModel);
        }

        [Test]
        public void GetPlayerModelWithId2000_Failed()
        {
            PlayerModel playerModel = _playerService.Get(2000);
            Assert.Null(playerModel);
        }
    }
}
