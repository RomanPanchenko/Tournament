using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Generators.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;

namespace TournamentWebApi.BLL.Generators.Helpers
{
    public class MatchesHelper : IMatchesHelper
    {
        private readonly IPlayersHelper _playerHelper;

        public MatchesHelper(IPlayersHelper playerHelper)
        {
            _playerHelper = playerHelper;
        }

        public MatchModel GetMatchModelForNextRound(
            PlayerModel player,
            IEnumerable<PlayerModel> playersToProcess,
            IList<MatchModel> previousMatches)
        {
            var match = new MatchModel
            {
                Player1 = player,
                Player2 = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult }
            };

            match.Player2 = playersToProcess.FirstOrDefault(p => _playerHelper.PlayersCanPlayTogether(player, p, previousMatches));
            return match;
        }
    }
}