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
        IEnumerable<MatchModel> GetAllMatches();

        /// <summary>
        ///     Gets specific match model by its Id
        /// </summary>
        /// <param name="matchId">MatchId</param>
        MatchModel Get(int matchId);

        /// <summary>
        ///     Gets all matches filtered by filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns></returns>
        IEnumerable<MatchModel> GetRange(Expression<Func<Match, bool>> filterCondition);
    }
}