using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.Commands;
using HiLoGame.Domain.Dtos;
using MediatR;

namespace HiLoGame.Application.Handlers.CommandHandlers;

public class MakeGuessCommandHandler : IRequestHandler<MakeGuessCommand, GameDto>
{
    private readonly IGameService _gameService;

    public MakeGuessCommandHandler(IGameService gameservice)
    {
        _gameService = gameservice;
    }

    public async Task<GameDto> Handle(MakeGuessCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameService.MakeGuessAsync(request.GameId, request.PlayerId, request.GuessNumber);
        return game;
    }
}
