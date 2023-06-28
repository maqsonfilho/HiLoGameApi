using AutoMapper;
using HiLoGame.Application.Dtos;
using HiLoGame.Domain.Entities;

namespace HiLoGame.Domain.MappingProfiles;

public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<EntityBase, DtoBase>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<GameEntity, GameDto>()
            .ForMember(dest => dest.MinNumber, opt => opt.MapFrom(src => src.MinNumber))
            .ForMember(dest => dest.MaxNumber, opt => opt.MapFrom(src => src.MaxNumber))
            .ForMember(dest => dest.MysteryNumber, opt => opt.MapFrom(src => src.MysteryNumber))
            .ForMember(dest => dest.IsGameStarted, opt => opt.MapFrom(src => src.IsGameStarted))
            .ForMember(dest => dest.IsGameFinished, opt => opt.MapFrom(src => src.IsGameFinished))
            .ForMember(dest => dest.Guesses, opt => opt.MapFrom(src => src.Guesses))
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));

        CreateMap<PlayerEntity, PlayerDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<GuessEntity, GuessDto>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId))
            .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.PlayerId))
            .ForMember(dest => dest.GuessNumber, opt => opt.MapFrom(src => src.GuessNumber));
    }
}
