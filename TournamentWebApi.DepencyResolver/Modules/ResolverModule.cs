using NHibernate;
using NHibernate.Cfg;
using Ninject.Modules;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Services;
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
            Bind<ISessionFactory>().ToMethod<ISessionFactory>(context =>
            {
                var configuration = new Configuration();
                configuration.Configure();
                return configuration.BuildSessionFactory();
            });

            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind(typeof(IRepositoryFactory)).To(typeof(RepositoryFactory));
            Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            Bind<IPlayerService>().To<PlayerService>();
            Bind<IMatchService>().To<MatchService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IRoleService>().To<RoleService>();
        }
    }
}