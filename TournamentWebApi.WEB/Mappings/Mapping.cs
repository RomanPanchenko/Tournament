using AutoMapper;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.Core.Enums;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.WEB.ViewModels;

namespace TournamentWebApi.WEB.Mappings
{
    public static class Mapping
    {
        public static void InitMapping()
        {
            InitPlayerMapping();
            InitMatchMapping();
        }

        private static void InitPlayerMapping()
        {
            Mapper.CreateMap<Player, PlayerModel>()
                .ForMember(x => x.OponentsIds, y => y.Ignore())
                .ForMember(x => x.StronglyPreferredColor, y => y.Ignore())
                .ForMember(x => x.ChessColor, y => y.Ignore())
                .ForMember(x => x.Winner, y => y.Ignore())
                .ForMember(x => x.HasPair, y => y.Ignore());

            Mapper.CreateMap<PlayerModel, Player>()
                .ForMember(x => x.Player1Matches, y => y.Ignore())
                .ForMember(x => x.Player2Matches, y => y.Ignore())
                .ForMember(x => x.WinnerMatches, y => y.Ignore());

            Mapper.CreateMap<Player, ScoreModel>()
                .ForMember(x => x.Score, y => y.Ignore())
                .ForMember(x => x.Position, y => y.Ignore());

            Mapper.CreateMap<ScoreModel, Player>()
                .ForMember(x => x.Player1Matches, y => y.Ignore())
                .ForMember(x => x.Player2Matches, y => y.Ignore())
                .ForMember(x => x.WinnerMatches, y => y.Ignore());

            Mapper.CreateMap<PlayerModel, PlayerViewModel>()
                .ForMember(x => x.Matches, y => y.Ignore());

            Mapper.CreateMap<PlayerViewModel, PlayerModel>()
                .ForMember(x => x.OponentsIds, y => y.Ignore())
                .ForMember(x => x.StronglyPreferredColor, y => y.Ignore())
                .ForMember(x => x.Winner, y => y.Ignore())
                .ForMember(x => x.HasPair, y => y.Ignore());
        }

        private static void InitMatchMapping()
        {
            Mapper.CreateMap<MatchModel, Match>().ReverseMap();

            Mapper.CreateMap<MatchModel, MatchViewModel>()
                .ForMember(x => x.PlayerColor1,
                    y => y.MapFrom(z => z.Player1PlaysWhite ? ChessColor.White : ChessColor.Black))
                .ForMember(x => x.PlayerColor2,
                    y => y.MapFrom(z => z.Player1PlaysWhite ? ChessColor.White : ChessColor.Black))
                .ForMember(x => x.MatchDateTime, y => y.MapFrom(z => z.MatchStartTime));

            Mapper.CreateMap<MatchViewModel, MatchModel>()
                .ForMember(x => x.Player1PlaysWhite, y => y.MapFrom(z => z.PlayerColor1 == ChessColor.White))
                .ForMember(x => x.MatchStartTime, y => y.MapFrom(z => z.MatchDateTime));

            Mapper.CreateMap<PlayersPairModel, MatchModel>()
                .ForMember(x => x.Player1, y => y.MapFrom(z => z.Player1))
                .ForMember(x => x.Player2, y => y.MapFrom(z => z.Player2))
                .ForMember(x => x.Player1PlaysWhite, y => y.MapFrom(z => z.Player1.ChessColor == ChessColor.White))
                .ForMember(x => x.Winner, y => y.MapFrom(z => z.Player1.Winner
                    ? z.Player1
                    : z.Player2.Winner
                        ? z.Player2
                        : new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForDrawnGame }))
                .ForMember(x => x.MatchId, y => y.Ignore())
                .ForMember(x => x.Result, y => y.Ignore());
        }
    }
}