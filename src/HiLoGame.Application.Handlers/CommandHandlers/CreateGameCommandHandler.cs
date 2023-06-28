using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.Commands;
using HiLoGame.Domain.Dtos;
using MediatR;

namespace HiLoGame.Application.Handlers.CommandHandlers;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameDto>
{
    private readonly IGameService _gameService;

    public CreateGameCommandHandler(IGameService gameservice)
    {
        _gameService = gameservice;
    }

    public async Task<GameDto> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameService.CreateGameAsync(request.MinNumber, request.MaxNumber);
        return game;
    }
}
