using AutoMapper;
using NHibernate.Cfg;
using NUnit.Framework;
using TournamentWebApi.BLL.Generators;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Services;
using TournamentWebApi.DAL.Factories;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.DAL.UnitsOfWorks;
using TournamentWebApi.WEB.Mappings;

namespace TournamentWebApi.BLL.Tests.Generator
{
    [TestFixture]
    public class MatchGeneratorTest
    {
        private IMatchService _matchService;
        private IPlayerService _playerService;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            Mapping.InitMapping();
            Mapper.AssertConfigurationIsValid();

            var configuration = new Configuration();
            configuration.Configure();
            var sessionFactory = configuration.BuildSessionFactory();
            _unitOfWork = new UnitOfWork(new RepositoryFactory(sessionFactory));
            _playerService = new PlayerService(_unitOfWork);
            _matchService = new MatchService(_unitOfWork);
        }

        [Test]
        public void GenerateMatches()
        {
            var matchGenetator = new MatchGenerator(_playerService, _matchService);
            matchGenetator.GenerateMatches();
        }

    }
}
