using System.Linq;
using AutoMapper;
using Ninject;
using Ninject.Modules;
using NUnit.Framework;
using System.Collections.Generic;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DepencyResolver.Modules;
using TournamentWebApi.WEB.Mappings;

namespace TournamentWebApi.BLL.Tests.UnitTests
{
    [TestFixture]
    class MatchGeneratorTests
    {
        private ITournamentServiceProvider _tournamentServiceProvider;
        public IKernel Kernel { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new ResolverModule(),
            };

            Kernel.Load(modules);

            Mapping.InitMapping();
            Mapper.AssertConfigurationIsValid();

            _tournamentServiceProvider = Kernel.Get<ITournamentServiceProvider>();
        }

        [Test]
        public void GetNextMatches()
        {
            var players = _tournamentServiceProvider.PlayerService.GetAllPlayers().ToList();
            var matchesForNextRound = new List<MatchModel>();

            for (int i = 0; i < _tournamentServiceProvider.MatchService.GetTotalRoundsCount(players.Count); i++)
            {
                var matchesWithoutResults = _tournamentServiceProvider.MatchService.GenerateMatchesForNextRound(players, matchesForNextRound);
                var matchesWithResults = _tournamentServiceProvider.MatchService.AssignRandomResultsForGeneratedMatches(matchesWithoutResults);
                matchesForNextRound.AddRange(matchesWithResults);
            }

            Assert.IsTrue(matchesForNextRound.Count == 45);
        }
    }
}
