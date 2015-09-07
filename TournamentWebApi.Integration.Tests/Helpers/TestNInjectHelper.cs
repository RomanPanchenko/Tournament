using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using TournamentWebApi.DepencyResolver.Modules;

namespace TournamentWebApi.Integration.Tests.Helpers
{
    public class TestNInjectHelper
    {
        public IKernel Kernel { get; private set; }

        public TestNInjectHelper()
        {
            Kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new ResolverModule(),
            };

            Kernel.Load(modules);
        }
    }
}
