using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.Commands;
using HiLoGame.Domain.Dtos;
using MediatR;

namespace HiLoGame.Application.Handlers.CommandHandlers;

public class StartGameCommandHandler : IRequestHandler<StartGameCommand, GameDto>
{
    private readonly IGameService _gameService;

    public StartGameCommandHandler(IGameService gameservice)
    {
        _gameService = gameservice;
    }
    public async Task<GameDto> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameService.StartGameAsync(request.GameId);
        return game;
    }
}
