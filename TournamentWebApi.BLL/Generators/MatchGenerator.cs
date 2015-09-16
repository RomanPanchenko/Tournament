using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Generators
{
    public class MatchGenerator : IMatchGenerator
    {
        private readonly IPlayerService _playerService;
        private readonly IMatchService _matchService;

        private List<PlayerModel> _players;
        private List<PlayerModel> Players
        {
            get
            {
                if (_players == null)
                {
                    _players = _playerService.GetAllPlayers().Where(p => p.PlayerId > 0).ToList();
                    foreach (PlayerModel playerModel in _players)
                    {
                        playerModel.OponentsIds = Matches
                            .Where(p => p.Player1.PlayerId == playerModel.PlayerId && p.Player2.PlayerId != SpecialPlayerIds.WinnerIdForGameWithUndefinedResult)
                            .Select(p => p.Player2.PlayerId).ToList();
                        playerModel.OponentsIds.AddRange(Matches
                            .Where(p => p.Player2.PlayerId == playerModel.PlayerId && p.Player1.PlayerId != SpecialPlayerIds.WinnerIdForGameWithUndefinedResult)
                            .Select(p => p.Player1.PlayerId));
                    }
                }
                return _players;
            }
        }

        private List<MatchModel> _matches;
        private List<MatchModel> Matches
        {
            get { return _matches ?? (_matches = _matchService.GetAllMatches().ToList()); }
        }

        private Dictionary<int, List<ChessColor>> _playerChessColorMap;
        private Dictionary<int, List<ChessColor>> PlayerChessColorMap
        {
            get
            {
                return _playerChessColorMap ?? (_playerChessColorMap = new Dictionary<int, List<ChessColor>>());
            }
        }

        public MatchGenerator(IPlayerService playerService, IMatchService matchService)
        {
            _playerService = playerService;
            _matchService = matchService;
        }

        public void GenerateMatches()
        {
            Generate();
        }


        private void BuildPlayerChessColorMap()
        {
            PlayerChessColorMap.Clear();

            if (Matches.Count > 0)
            {
                foreach (MatchModel matchModel in Matches)
                {
                    BuildColorMapForBothPlayersInMatch(matchModel);
                }
            }
            else
            {
                AssignRandomColorsForPlayers();
            }

            // Reverse valus of the dictionary to get latest history on top
            foreach (List<ChessColor> list in PlayerChessColorMap.Values)
            {
                list.Reverse();
            }
        }

        private void AssignRandomColorsForPlayers()
        {
            var color = ChessColor.White;
            foreach (PlayerModel playerModel in Players)
            {
                PlayerChessColorMap[playerModel.PlayerId] = new List<ChessColor> { color };
                color = (color == ChessColor.White) ? ChessColor.Black : ChessColor.White;
            }
        }

        private void BuildColorMapForBothPlayersInMatch(MatchModel matchModel)
        {
            if (!PlayerChessColorMap.ContainsKey(matchModel.Player1.PlayerId))
            {
                PlayerChessColorMap[matchModel.Player1.PlayerId] = new List<ChessColor>();
            }
            PlayerChessColorMap[matchModel.Player1.PlayerId].Add(matchModel.Player1PlaysWhite
                ? ChessColor.White
                : ChessColor.Black);

            if (!PlayerChessColorMap.ContainsKey(matchModel.Player2.PlayerId))
            {
                PlayerChessColorMap[matchModel.Player2.PlayerId] = new List<ChessColor>();
            }
            PlayerChessColorMap[matchModel.Player2.PlayerId].Add(matchModel.Player1PlaysWhite
                ? ChessColor.Black
                : ChessColor.White);
        }


        private ChessColor InvertChessColor(ChessColor chessColor)
        {
            return chessColor == ChessColor.Black ? ChessColor.White : ChessColor.Black;
        }


        private void UpdateChessColorsForPlayerPair(PlayersPairModel playersPairModel)
        {
            if (playersPairModel != null)
            {
                var player1ChessColors = new List<ChessColor>();
                var player2ChessColors = new List<ChessColor>();
                if (playersPairModel.Player1 != null && PlayerChessColorMap.ContainsKey(playersPairModel.Player1.PlayerId))
                {
                    player1ChessColors = PlayerChessColorMap[playersPairModel.Player1.PlayerId];
                }
                if (playersPairModel.Player2 != null && PlayerChessColorMap.ContainsKey(playersPairModel.Player2.PlayerId))
                {
                    player2ChessColors = PlayerChessColorMap[playersPairModel.Player2.PlayerId];
                }

                if (playersPairModel.Player1 != null && playersPairModel.Player2 == null)
                {
                    playersPairModel.Player1.ChessColor = ChessColor.Undefined;
                }
                if (playersPairModel.Player2 != null && playersPairModel.Player1 == null)
                {
                    playersPairModel.Player2.ChessColor = ChessColor.Undefined;
                }

                if (playersPairModel.Player1 != null && playersPairModel.Player2 != null)
                {
                    if (player1ChessColors.Count == 0 && player2ChessColors.Count == 0)
                    {
                        // No play history, so just give Black to first player and White to second player
                        playersPairModel.Player1.ChessColor = ChessColor.Black;
                        playersPairModel.Player2.ChessColor = ChessColor.White;
                    }
                    else
                    {
                        if (player1ChessColors[0] == ChessColor.Black)
                        {
                            playersPairModel.Player1.ChessColor = ChessColor.White;
                            playersPairModel.Player2.ChessColor = ChessColor.Black;
                        }
                        else
                        {
                            playersPairModel.Player1.ChessColor = ChessColor.Black;
                            playersPairModel.Player2.ChessColor = ChessColor.White;
                        }

                        if (player1ChessColors.Count > 1 && playersPairModel.Player1.ChessColor == player1ChessColors[0] &&
                            playersPairModel.Player1.ChessColor == player1ChessColors[1])
                        {
                            playersPairModel.Player1.ChessColor = InvertChessColor(playersPairModel.Player1.ChessColor);
                            playersPairModel.Player1.StronglyPreferredColor = true;
                        }

                        if (player2ChessColors.Count > 1 && playersPairModel.Player2.ChessColor == player1ChessColors[0] &&
                            playersPairModel.Player2.ChessColor == player1ChessColors[1])
                        {
                            playersPairModel.Player2.ChessColor = InvertChessColor(playersPairModel.Player2.ChessColor);
                            playersPairModel.Player2.StronglyPreferredColor = true;
                        }
                    }
                }
            }
        }


        private void UpdatePlayersWithRightColors(List<PlayersPairModel> playerPairs)
        {
            var playerPairsToReplace = new List<PlayersPairModel>();
            BuildPlayerChessColorMap();
            if (playerPairs.Count > 0)
            {
                foreach (PlayersPairModel playersPairModel in playerPairs)
                {
                    UpdateChessColorsForPlayerPair(playersPairModel);

                    if ((playersPairModel.Player1.StronglyPreferredColor && playersPairModel.Player2.StronglyPreferredColor)
                        && (playersPairModel.Player1.ChessColor == playersPairModel.Player2.ChessColor))
                    {
                        playerPairsToReplace.Add(playersPairModel);
                    }
                }
            }
        }


        public List<PlayersPairModel> GetPlayerPairs()
        {
            var playerPairs = new List<PlayersPairModel>();
            List<PlayerModel> players = CalculateCurrentRatings();
            players = players.OrderByDescending(p => p.Rate).ToList();

            for (int i = 0; i < players.Count - 1; i++)
            {
                PlayerModel playerModel1 = players.FirstOrDefault(p => !p.HasPair);
                if (playerModel1 != null)
                {

                    PlayerModel playerModel2 = players.FirstOrDefault(p => !p.HasPair
                        && p.PlayerId != playerModel1.PlayerId
                        && !playerModel1.OponentsIds.Contains(p.PlayerId));

                    if (playerModel2 != null)
                    {
                        var playersPair = new PlayersPairModel
                        {
                            Player1 = playerModel1,
                            Player2 = playerModel2
                        };

                        playerModel1.HasPair = true;
                        playerModel2.HasPair = true;

                        playerPairs.Add(playersPair);
                    }
                }
            }

            List<PlayerModel> unpairedPlayers = players.Where(p => !p.HasPair).ToList();
            if (unpairedPlayers.Count == 1)
            {
                var playersPair = new PlayersPairModel
                {
                    Player1 = unpairedPlayers.First(),
                    Player2 = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult }
                };
                playerPairs.Add(playersPair);
            }
            else if (unpairedPlayers.Count == 2)
            {
                var playersPair = new PlayersPairModel
                {
                    Player1 = unpairedPlayers.First(),
                    Player2 = unpairedPlayers.Last()
                };
                playerPairs.Add(playersPair);
            }
            else if (unpairedPlayers.Count > 2)
            {
                throw new Exception("Strange number of unpaired players");
            }

            UpdatePlayersWithRightColors(playerPairs);
            return playerPairs;
        }


        private int GetTotalRoundsCount()
        {
            var roundsCount = (int)Math.Floor(Math.Log(Players.Count, 2));
            return roundsCount;
        }


        private List<PlayerModel> CalculateCurrentRatings()
        {
            var players = new List<PlayerModel>();
            if (Matches.Count > 0)
            {
                Players.ForEach(p => p.Rate = 0);
                foreach (MatchModel matchModel in Matches)
                {
                    var player1 = Players.FirstOrDefault(p => p.PlayerId == matchModel.Player1.PlayerId);
                    var player2 = Players.FirstOrDefault(p => p.PlayerId == matchModel.Player2.PlayerId);


                    if (player1 != null && matchModel.Winner.PlayerId == player1.PlayerId)
                    {
                        player1.Rate += Score.PointsForWin;
                    }
                    else if (player2 != null && matchModel.Winner.PlayerId == player2.PlayerId)
                    {
                        player2.Rate += Score.PointsForWin;
                    }
                    else if (matchModel.Winner.PlayerId == SpecialPlayerIds.WinnerIdForDrawnGame)
                    {
                        if (player1 != null)
                        {
                            player1.Rate += Score.PointsForDrawn;
                        }
                        if (player2 != null)
                        {
                            player2.Rate += Score.PointsForDrawn;
                        }
                    }
                }
            }
            players.AddRange(Players.OrderByDescending(p => p.Rate));
            return players;
        }


        private void Generate()
        {
            var random = new Random(DateTime.Now.Millisecond);
            DateTime matchStartTime = DateTime.Now;

            for (int i = 0; i <= GetTotalRoundsCount(); i++)
            {
                _players = null;
                _matches = null;
                List<PlayersPairModel> playersPairModels = GetPlayerPairs();
                foreach (PlayersPairModel playersPairModel in playersPairModels)
                {
                    playersPairModel.Player1 = playersPairModel.Player1 ?? new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult };
                    playersPairModel.Player2 = playersPairModel.Player2 ?? new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult };

                    int rnd = random.Next(0, 2);
                    switch (rnd)
                    {
                        case 1:
                            playersPairModel.Player1.Winner = true;
                            break;
                        case 2:
                            playersPairModel.Player2.Winner = true;
                            break;
                    }

                    if (playersPairModel.Player1.PlayerId == SpecialPlayerIds.WinnerIdForGameWithUndefinedResult)
                    {
                        playersPairModel.Player1.Winner = false;
                        playersPairModel.Player2.Winner = true;
                    }

                    if (playersPairModel.Player2.PlayerId == SpecialPlayerIds.WinnerIdForGameWithUndefinedResult)
                    {
                        playersPairModel.Player1.Winner = true;
                        playersPairModel.Player2.Winner = false;
                    }

                    playersPairModel.MatchStartTime = matchStartTime;
                    playersPairModel.Round = i + 1;
                    matchStartTime = matchStartTime.AddMinutes(1);
                }
                matchStartTime = matchStartTime.AddDays(1);

                var matchModels = Mapper.Map<List<MatchModel>>(playersPairModels);

                _matchService.AddRange(matchModels);

            }
        }

        public IEnumerable<MatchModel> GetMatchesForNextRound(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> pastMatches)
        {
            throw new NotImplementedException();
        }
    }
}
