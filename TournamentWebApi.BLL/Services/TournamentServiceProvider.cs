using System;
using TournamentWebApi.BLL.Interfaces;

namespace TournamentWebApi.BLL.Services
{
    public class TournamentServiceProvider : ITournamentServiceProvider
    {
        private readonly Lazy<IAccountService> _lazyAccountService;
        private readonly Lazy<IMatchService> _lazyMatchService;
        private readonly Lazy<IPlayerService> _lazyPlayerService;
        private readonly Lazy<IRoleService> _lazyRoleService;

        public IAccountService AccountService
        {
            get { return _lazyAccountService.Value; }
        }

        public IMatchService MatchService
        {
            get { return _lazyMatchService.Value; }
        }

        public IPlayerService PlayerService
        {
            get { return _lazyPlayerService.Value; }
        }

        public IRoleService RoleService
        {
            get { return _lazyRoleService.Value; }
        }

        public TournamentServiceProvider(
            Lazy<IAccountService> lazyAccountService,
            Lazy<IMatchService> lazyMatchService,
            Lazy<IPlayerService> lazyPlayerService,
            Lazy<IRoleService> lazyRoleService)
        {
            _lazyAccountService = lazyAccountService;
            _lazyMatchService = lazyMatchService;
            _lazyPlayerService = lazyPlayerService;
            _lazyRoleService = lazyRoleService;
        }
    }
}
