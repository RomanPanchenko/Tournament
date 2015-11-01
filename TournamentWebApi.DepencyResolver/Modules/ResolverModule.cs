using System;
using NHibernate;
using NHibernate.Cfg;
using Ninject;
using Ninject.Modules;
using TournamentWebApi.BLL.Generators;
using TournamentWebApi.BLL.Generators.Helpers;
using TournamentWebApi.BLL.Generators.Interfaces;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Services;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Factories;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.DAL.Repositories;
using TournamentWebApi.DAL.UnitsOfWorks;

namespace TournamentWebApi.DepencyResolver.Modules
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPlayersHelper>().To<PlayersHelper>();
            Bind<IMatchesHelper>().To<MatchesHelper>();

            Bind<ISessionFactory>().ToMethod<ISessionFactory>(context =>
            {
                var configuration = new Configuration();
                configuration.Configure();
                return configuration.BuildSessionFactory();
            });

            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind(typeof(IRepositoryFactory)).To(typeof(RepositoryFactory));
            Bind(typeof(IGenericRepository<Match>)).To(typeof(MatchRepository));
            Bind(typeof(IGenericRepository<Player>)).To(typeof(PlayerRepository));
            Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));

            Bind<IMatchGenerator>().To<MatchGenerator>();
            Bind<ITournamentServiceProvider>().To<TournamentServiceProvider>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IMatchService>().To<MatchService>();
            Bind<IPlayerService>().To<PlayerService>();
            Bind<IRoleService>().To<RoleService>();
            Bind(typeof(Lazy<IAccountService>)).ToMethod(context => new Lazy<IAccountService>(() => context.Kernel.Get<IAccountService>()));
            Bind(typeof(Lazy<IMatchService>)).ToMethod(context => new Lazy<IMatchService>(() => context.Kernel.Get<IMatchService>()));
            Bind(typeof(Lazy<IPlayerService>)).ToMethod(context => new Lazy<IPlayerService>(() => context.Kernel.Get<IPlayerService>()));
            Bind(typeof(Lazy<IRoleService>)).ToMethod(context => new Lazy<IRoleService>(() => context.Kernel.Get<IRoleService>()));
        }
    }
}