using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface IMatchService
    {
        /// <summary>
        ///     Adds the specified match to the database.
        /// </summary>
        /// <param name="matchModel">The match model.</param>
        void Add(MatchModel matchModel);

        /// <summary>
        ///     Adds the specified matches to the database.
        /// </summary>
        /// <param name="matchModels">List of match models.</param>
        void AddRange(IEnumerable<MatchModel> matchModels);

        /// <summary>
        ///     Gets all matches from database
        /// </summary>
        /// <returns>MathModel entities collection</returns>
        IEnumerable<MatchModel> GetAllMatches();

        /// <summary>
        ///     Gets specific match model by its Id
        /// </summary>
        /// <param name="matchId">MatchId</param>
        /// <returns>MathModel entity</returns>
        MatchModel Get(int matchId);

        /// <summary>
        ///     Gets all matches filtered by filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns>MathModel entities collection filtered by filterCondition</returns>
        IEnumerable<MatchModel> GetRange(Expression<Func<Match, bool>> filterCondition);

        /// <summary>
        ///     Generates matches for next round
        /// </summary>
        /// <returns>MathModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatchesForNextRound();

        /// <summary>
        ///     Gets player score
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>ScoreModel entity</returns>
        ScoreModel GetPlayerScore(int playerId);

        /// <summary>
        ///     Gets scores for all players
        /// </summary>
        /// <returns>ScoreModel entities collection for all players</returns>
        IEnumerable<ScoreModel> GetScoreForAllPlayers();

        /// <summary>
        ///     Gets all matches for specified player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>MatchModel entities collection for the given player id</returns>
        IEnumerable<MatchModel> GetPlayerMatches(int playerId);
    }
}