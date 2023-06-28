using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.Dtos;
using HiLoGame.Domain.Queries;
using MediatR;

namespace HiLoGame.Application.Handlers.CommandHandlers;

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GameDto>
{
    private readonly IGameService _gameService;

    public GetGameQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<GameDto> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await _gameService.GetGameAsync(request.Id);
        return game;
    }
}
