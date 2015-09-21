using System;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface ITournamentServiceProvider
    {
        IAccountService AccountService { get; }
        IMatchService MatchService { get; }
        IPlayerService PlayerService { get; }
        IRoleService RoleService { get; }
    }
}
