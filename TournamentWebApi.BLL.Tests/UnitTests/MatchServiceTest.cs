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
    public class MatchServiceTest
    {
        private IMatchService _matchService;

        [SetUp]
        public void SetUp()
        {
            Mapping.InitMapping();
            Mapper.AssertConfigurationIsValid();

            _matchService = new MatchService(new UnitOfWork());
        }

        [Test]
        public void GetCollectionOfAllMatchModels_Passed()
        {
            IEnumerable<MatchModel> matchModels = _matchService.GetAllMatches();
            Assert.NotNull(matchModels);
        }

        [Test]
        public void GetMatchModelWithId2_Passed()
        {
            MatchModel matchModel = _matchService.Get(2);
            Assert.NotNull(matchModel);
        }

        [Test]
        public void GetMatchModelWithId2000_Failed()
        {
            MatchModel matchModel = _matchService.Get(2000);
            Assert.Null(matchModel);
        }

    }
}
