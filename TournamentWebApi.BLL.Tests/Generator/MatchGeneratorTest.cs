using AutoMapper;
using Ninject;
using Ninject.Modules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.DepencyResolver.Modules;
using TournamentWebApi.WEB.Mappings;

namespace TournamentWebApi.BLL.Tests.Generator
{
    [TestFixture]
    public class MatchGeneratorTest
    {
        private IServicesProvider _servicesProvider;
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

            _servicesProvider = Kernel.Get<IServicesProvider>();
        }

        [Test]
        public void GenerateMatches()
        {
            var players = _servicesProvider.PlayerService.GetAllPlayers().ToList();
            Assert.IsTrue(players.Count == 21);
        }

    }
}
