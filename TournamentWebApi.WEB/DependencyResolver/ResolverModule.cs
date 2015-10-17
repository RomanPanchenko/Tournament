using Ninject.Modules;
using TournamentWebApi.WEB.Interfaces;
using TournamentWebApi.WEB.Security;

namespace TournamentWebApi.WEB.DependencyResolver
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthenticationService>().To<AuthenticationService>();
        }
    }
}