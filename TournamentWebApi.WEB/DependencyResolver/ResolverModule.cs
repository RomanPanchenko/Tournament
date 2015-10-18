using Ninject.Modules;
using TournamentWebApi.WEB.Interfaces;
using TournamentWebApi.WEB.Logging;
using TournamentWebApi.WEB.Security;

namespace TournamentWebApi.WEB.DependencyResolver
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITournamentWebApiLogger>().To<TournamentWebApiLogger>();
            Bind<IAuthenticationService>().To<AuthenticationService>();
        }
    }
}