using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Generators.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Generators.Helpers
{
    public class PlayersHelper : IPlayersHelper
    {
        public bool PlayersCanPlayTogether(PlayerModel player1, PlayerModel player2, IList<MatchModel> previousMatches)
        {
            // Players can play together if they haven't played together yet and
            // each of them didn't play with the same color more then 2 times.

            bool result = false;
            if (!PlayersAlreadyPlayedTogether(player1, player2, previousMatches))
            {
                if (
                    (PlayerCanPlayWithDefinedColor(player1, ChessColor.White, previousMatches)
                    && PlayerCanPlayWithDefinedColor(player2, ChessColor.Black, previousMatches))
                    ||
                    (PlayerCanPlayWithDefinedColor(player1, ChessColor.Black, previousMatches)
                    && PlayerCanPlayWithDefinedColor(player2, ChessColor.White, previousMatches))
                    )
                {
                    result = true;
                }
            }

            return result;
        }

        public bool PlayerCanPlayWithDefinedColor(PlayerModel player, ChessColor color, IEnumerable<MatchModel> previousMatches)
        {
            IList<ChessColor> colors = GetChessColorsForPlayedMatches(player, previousMatches);
            int last = colors.Count - 1;
            return last <= 0 || !(colors[last] == color && colors[last - 1] == color);
        }

        public IEnumerable<PlayerModel> GetPlayersWithCalculatedRating(IList<PlayerModel> players, IList<MatchModel> previousMatches)
        {
            foreach (PlayerModel player in players)
            {
                player.Rate += previousMatches
                    .Where(p => p.Player1.PlayerId == player.PlayerId || p.Player2.PlayerId == player.PlayerId)
                    .Sum(match => GetPlayerRateForGivenMatch(player.PlayerId, match));
            }

            return players;
        }

        private bool PlayersAlreadyPlayedTogether(PlayerModel player1, PlayerModel player2, IEnumerable<MatchModel> previousMatches)
        {
            return previousMatches
                .Where(p => p.Player1.PlayerId == player1.PlayerId || p.Player2.PlayerId == player1.PlayerId)
                .Any(m => m.Player1.PlayerId == player2.PlayerId || m.Player2.PlayerId == player2.PlayerId);
        }

        private int GetPlayerRateForGivenMatch(int playerId, MatchModel match)
        {
            int rate = 0;
            if (match.Winner.PlayerId == playerId)
            {
                rate = Score.PointsForWin;
            }
            else if (match.Winner.PlayerId == SpecialPlayerIds.WinnerIdForDrawnGame)
            {
                rate = Score.PointsForDrawn;
            }

            return rate;
        }

        private IList<ChessColor> GetChessColorsForPlayedMatches(PlayerModel player, IEnumerable<MatchModel> previousMatches)
        {
            var colors = new List<ChessColor>();
            IEnumerable<MatchModel> matches = previousMatches
                .Where(p => p.Player1.PlayerId == player.PlayerId || p.Player2.PlayerId == player.PlayerId);

            foreach (MatchModel matchModel in matches)
            {
                if ((matchModel.Player1.PlayerId == player.PlayerId && matchModel.Player1PlaysWhite) ||
                    (matchModel.Player2.PlayerId == player.PlayerId && !matchModel.Player1PlaysWhite))
                {
                    colors.Add(ChessColor.White);
                }

                if ((matchModel.Player1.PlayerId == player.PlayerId && !matchModel.Player1PlaysWhite) ||
                    (matchModel.Player2.PlayerId == player.PlayerId && matchModel.Player1PlaysWhite))
                {
                    colors.Add(ChessColor.Black);
                }
            }

            return colors;
        }
    }
}
