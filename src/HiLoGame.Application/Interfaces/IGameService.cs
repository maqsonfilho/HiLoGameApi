using HiLoGame.Application.Dtos;

namespace HiLoGame.Application.Interfaces;

public interface IGameService
{ 
    Task<GameDto> GetGameAsync(Guid gameId);
    Task<GameDto> CreateGameAsync(int minNumber, int maxNumber);
    Task<PlayerDto> JoinGameAsync(Guid gameId, string playerName);
    Task<GameDto> StartGameAsync(Guid gameId);
    Task<GuessDto> MakeGuessAsync(Guid gameId, Guid playerId, int guessNumber);
}