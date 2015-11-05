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
            InitAccountMapping();
            InitRoleMapping();
            InitPlayerMapping();
            InitMatchMapping();
        }

        private static void InitPlayerMapping()
        {
            Mapper.CreateMap<Player, PlayerModel>()
                .ForMember(x => x.ChessColor, y => y.Ignore())
                .ForMember(x => x.Winner, y => y.Ignore())
                .ForMember(x => x.IsLoaded, y => y.MapFrom(z => true));

            Mapper.CreateMap<Player, ScoreModel>()
                .ForMember(x => x.Score, y => y.Ignore())
                .ForMember(x => x.Position, y => y.Ignore());

            Mapper.CreateMap<PlayerModel, PlayerViewModel>()
                .ForMember(x => x.Matches, y => y.Ignore());

            Mapper.CreateMap<PlayerViewModel, PlayerModel>()
                .ForMember(x => x.Winner, y => y.Ignore())
                .ForMember(x => x.IsLoaded, y => y.MapFrom(z => true));
        }

        private static void InitMatchMapping()
        {
            // MatchModel -> Match
            // Create Match from MatchModel
            Mapper.CreateMap<MatchModel, Match>()
                .ForMember(x => x.PlayerForeignKey1, y => y.Ignore())
                .ForMember(x => x.PlayerForeignKey2, y => y.Ignore())
                .ForMember(x => x.PlayerForeignKey3, y => y.Ignore())
                .ForMember(x => x.PlayerId1, y => y.MapFrom(z => z.Player1.PlayerId))
                .ForMember(x => x.PlayerId2, y => y.MapFrom(z => z.Player2.PlayerId))
                .ForMember(x => x.WinnerId, y => y.MapFrom(z => z.Player1.Winner
                    ? z.Player1.PlayerId
                    : z.Player2.Winner
                        ? z.Player2.PlayerId
                        : SpecialPlayerIds.WinnerIdForDrawnGame))
                .ForMember(x => x.Player1PlaysWhite, y => y.MapFrom(z => z.Player1.ChessColor == ChessColor.White));

            // Create MatchModel from Match
            Mapper.CreateMap<Match, MatchModel>()
                .ForMember(x => x.Player1, y => y.MapFrom(z => new PlayerModel { PlayerId = z.PlayerId1 }))
                .ForMember(x => x.Player2, y => y.MapFrom(z => new PlayerModel { PlayerId = z.PlayerId2 }))
                .ForMember(x => x.Winner, y => y.MapFrom(z => new PlayerModel { PlayerId = z.WinnerId }))
                .AfterMap((match, matchModel) => matchModel.Player1.Winner = match.PlayerId1 == match.WinnerId)
                .AfterMap((match, matchModel) => matchModel.Player2.Winner = match.PlayerId2 == match.WinnerId)
                .AfterMap((match, matchModel) => matchModel.Player1.ChessColor = match.Player1PlaysWhite ? ChessColor.White : ChessColor.Black)
                .AfterMap((match, matchModel) => matchModel.Player2.ChessColor = match.Player1PlaysWhite ? ChessColor.Black : ChessColor.White);

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
                .ForMember(x => x.Winner, y => y.MapFrom(z => z.Player1.Winner
                    ? z.Player1
                    : z.Player2.Winner
                        ? z.Player2
                        : new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForDrawnGame }))
                .ForMember(x => x.Player1, y => y.MapFrom(z => z.Player1))
                .ForMember(x => x.Player2, y => y.MapFrom(z => z.Player2))
                .ForMember(x => x.Player1PlaysWhite, y => y.MapFrom(z => z.Player1.ChessColor == ChessColor.White))
                .ForMember(x => x.Winner, y => y.MapFrom(z => z.Player1.Winner
                    ? z.Player1
                    : z.Player2.Winner
                        ? z.Player2
                        : new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForDrawnGame }))
                .ForMember(x => x.MatchId, y => y.Ignore());
        }

        private static void InitAccountMapping()
        {
            Mapper.CreateMap<AccountModel, Account>().ReverseMap();
        }

        private static void InitRoleMapping()
        {
            Mapper.CreateMap<RoleModel, Role>()
                .ForMember(x => x.Accounts, y => y.Ignore());

            Mapper.CreateMap<Role, RoleModel>();
        }
    }
}