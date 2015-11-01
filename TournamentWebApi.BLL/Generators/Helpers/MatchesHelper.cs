using System.Collections.Generic;
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
            List<MatchModel> previousMatches)
        {
            var match = new MatchModel
            {
                Player1 = player,
                Player2 = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult }
            };

            foreach (PlayerModel nextPlayer in playersToProcess)
            {
                if (_playerHelper.PlayersCanPlayTogether(player, nextPlayer, previousMatches))
                {
                    match.Player2 = nextPlayer;
                    break;
                }
            }

            return match;
        }
    }
}
