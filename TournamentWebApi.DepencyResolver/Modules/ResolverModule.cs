using NHibernate;
using NHibernate.Cfg;
using Ninject.Modules;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Services;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.DAL.Repositories;
using TournamentWebApi.DAL.UnitsOfWorks;
using TournamentWebApi.DAL.Wrappers;

namespace TournamentWebApi.DepencyResolver.Modules
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISession>().ToMethod(context =>
            {
                var configuration = new Configuration();
                configuration.Configure();
                ISessionFactory sessionFactory = configuration.BuildSessionFactory();
                return sessionFactory.OpenSession();
            }).InSingletonScope();

            Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
            Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            Bind<IPlayerService>().To<PlayerService>();
            Bind<IMatchService>().To<MatchService>();
            Bind(typeof(INhSession<>)).To(typeof(NhSession<>));
        }
    }
}