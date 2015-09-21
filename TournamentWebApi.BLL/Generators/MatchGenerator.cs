using System;
using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Generators
{
    public class MatchGenerator : IMatchGenerator
    {
        public IEnumerable<MatchModel> GetMatchesForNextRound(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> previousMatches)
        {
            var matchesForNextRound = new List<MatchModel>();

            List<MatchModel> matchModels = previousMatches.ToList();
            List<PlayerModel> playerModels = players.ToList();

            int round = matchModels.Count == 0 ? 1 : matchModels.OrderByDescending(p => p.Round).First().Round + 1;

            players = GetPlayersWithCalculatedRating(playerModels, matchModels);
            playerModels = players.OrderByDescending(p => p.Rate).ToList();

            while (playerModels.Count > 0)
            {
                PlayerModel player = playerModels.First();
                playerModels.Remove(player);

                MatchModel matchModel = GetMatchModelForNextRound(player, playerModels, matchModels);
                matchModel.Round = round;
                matchModel.Player1PlaysWhite = PlayerCanPlayWithDefinedColor(player, ChessColor.White, matchModels);
                matchModel.MatchStartTime = DateTime.Now;
                matchModel.Winner = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult };

                playerModels.Remove(matchModel.Player2);
                matchesForNextRound.Add(matchModel);
            }

            return matchesForNextRound;
        }

        public IEnumerable<MatchModel> AssignRandomResultsForGeneratedMatches(IEnumerable<MatchModel> roundMatches)
        {
            List<MatchModel> matches = roundMatches.ToList();
            var random = new Random(DateTime.Now.Millisecond);
            foreach (MatchModel match in matches)
            {
                match.Winner = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForDrawnGame };
                int rnd = random.Next(0, 2);
                switch (rnd)
                {
                    case 1:
                        match.Player1.Winner = true;
                        match.Winner.PlayerId = match.Player1.PlayerId;
                        break;
                    case 2:
                        match.Player2.Winner = true;
                        match.Winner.PlayerId = match.Player2.PlayerId;
                        break;
                }
            }

            return matches;
        }

        private MatchModel GetMatchModelForNextRound(
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
                if (PlayersCanPlayTogether(player, nextPlayer, previousMatches))
                {
                    match.Player2 = nextPlayer;
                    break;
                }
            }

            return match;
        }

        private bool PlayersCanPlayTogether(PlayerModel player1, PlayerModel player2, List<MatchModel> previousMatches)
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

        private bool PlayerCanPlayWithDefinedColor(PlayerModel player, ChessColor color, IEnumerable<MatchModel> previousMatches)
        {
            bool result;

            List<ChessColor> colors = GetChessColorsForPlayedMatches(player, previousMatches);

            if (colors.Count > 1)
            {
                colors.Reverse();
                result = !(colors[0] == color && colors[1] == color);
            }
            else
            {
                result = true;
            }

            return result;
        }

        private List<ChessColor> GetChessColorsForPlayedMatches(PlayerModel player, IEnumerable<MatchModel> previousMatches)
        {
            var colors = new List<ChessColor>();
            var matches = previousMatches.Where(p => p.Player1.PlayerId == player.PlayerId || p.Player2.PlayerId == player.PlayerId);
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

        private bool PlayersAlreadyPlayedTogether(PlayerModel player1, PlayerModel player2, IEnumerable<MatchModel> previousMatches)
        {
            return previousMatches
                .Where(p => p.Player1.PlayerId == player1.PlayerId || p.Player2.PlayerId == player1.PlayerId)
                .Any(m => m.Player1.PlayerId == player2.PlayerId || m.Player2.PlayerId == player2.PlayerId);
        }

        private IEnumerable<PlayerModel> GetPlayersWithCalculatedRating(List<PlayerModel> players, List<MatchModel> previousMatches)
        {
            foreach (PlayerModel player in players)
            {
                player.Rate += previousMatches
                    .Where(p => p.Player1.PlayerId == player.PlayerId || p.Player2.PlayerId == player.PlayerId)
                    .Sum(match => GetPlayerRateForGivenMatch(player.PlayerId, match));
            }

            return players;
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
    }
}
