using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.Commands;
using HiLoGame.Domain.Dtos;
using MediatR;

namespace HiLoGame.Application.Handlers.CommandHandlers;

public class joingGameCommandHandler : IRequestHandler<JoinGameCommand, GameDto>
{
    private readonly IGameService _gameService;

    public joingGameCommandHandler(IGameService gameservice)
    {
        _gameService = gameservice;
    }
    public async Task<GameDto> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameService.JoinGameAsync(request.GameId, request.PlayerName);
        return game;
    }
}
