using AutoMapper;
using HiLoGame.Application.Dtos;
using HiLoGame.Domain.Entities;
using HiLoGame.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace HiLoGame.Application.Interfaces;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public GameService(IGameRepository gameRepository, IPlayerRepository playerRepository, ILogger<GameService> logger, IMapper mapper)
    {
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GameDto> GetGameAsync(Guid gameId)
    {
        GameEntity gameEntity = await _gameRepository.GetGameByIdAsync(gameId);

        if (gameEntity is null)
        {
            throw new Exception("Game not found");
        }

        return _mapper.Map<GameDto>(gameEntity);
    }

    public async Task<GameDto> CreateGameAsync(int minNumber, int maxNumber)
    {
        if (minNumber >= maxNumber)
        {
            throw new ValidationException("Invalid number range. The minimum number must be smaller than the maximum number.");
        }

        int mysteryNumber = GenerateRandomNumber(minNumber, maxNumber);

        GameEntity game = new GameEntity
        {
            MysteryNumber = mysteryNumber,
            MinNumber = minNumber,
            MaxNumber = maxNumber
        };

        game = await _gameRepository.InsertAsync(game);

        _logger.LogInformation($"New game created. GameId: {game.Id}");

        return _mapper.Map<GameDto>(game);
    }

    public async Task<GameDto> StartGameAsync(Guid gameId)
    {
        var game = await _gameRepository.GetByIdAsync(gameId);
        game.IsGameStarted = true;
        game = await _gameRepository.Update(game);

        _logger.LogInformation($"New game started. GameId: {game.Id} at {DateTime.UtcNow}");

        return _mapper.Map<GameDto>(game);
    }

    public async Task<PlayerDto> JoinGameAsync(Guid gameId, string playerName)
    {
        try
        {
            var gameEntity = await _gameRepository.GetGameByIdAsync(gameId);

            if (gameEntity.IsGameStarted)
            {
                throw new ValidationException($"Game {gameEntity.Id} already started. Cannot join in this game.");
            }

            var playerEntity = await _playerRepository.GetByNameAsync(playerName);
            if (playerEntity is null)
            {
                playerEntity = await _playerRepository.InsertAsync(new PlayerEntity
                {
                    Name = playerName,
                    GameId = gameEntity.Id
                });
            }

            return _mapper.Map<PlayerDto>(playerEntity);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<GuessDto> MakeGuessAsync(Guid gameId, Guid playerId, int guessNumber)
    {
        // Retrieve the game from the repository
        GameEntity gameEntity = await _gameRepository.GetGameByIdAsync(gameId);

        if (gameEntity is null)
        {
            throw new Exception("Game not found");
        }

        // Retrieve the player from the repository
        PlayerEntity playerEntity = gameEntity.Players.FirstOrDefault(p => p.Id == playerId);

        if (playerEntity is null)
        {
            throw new Exception("Player not found");
        }

        if (gameEntity.IsGameFinished)
        {
            throw new ValidationException("The game has already been finished");
        }

        // Validate the guess number
        if (guessNumber < gameEntity.MinNumber || guessNumber > gameEntity.MaxNumber)
        {
            throw new ValidationException("Invalid guess number. The number must be within the range specified for the game.");
        }

        // Update the game with the player's guess
        var guessEntity = new GuessEntity
        {
            Player = playerEntity,
            Game = gameEntity,
            GuessNumber = guessNumber
        };

        // Determine if the guess is correct
        if (guessNumber == gameEntity.MysteryNumber)
        {
            gameEntity.IsGameFinished = true;
            guessEntity.Feedback = "YOU WON!";
        }
        else
        {
            if (guessNumber < gameEntity.MysteryNumber)
            {
                guessEntity.Feedback = "HI";
            }
            else
            {
                guessEntity.Feedback = "LO";
            }
        }

        gameEntity.Guesses.Add(guessEntity);

        // Update the game in the repository
        gameEntity = await _gameRepository.Update(gameEntity);

        _logger.LogInformation($"Player {playerEntity.Name} made a guess in GameId: {gameEntity.Id}. Guess: {guessNumber}");

        return _mapper.Map<GuessDto>(guessEntity);
    }


    #region Private Methods
    private int GenerateRandomNumber(int minNumber, int maxNumber)
    {
        // Generate a random number using a random seed
        Random random = new Random(Guid.NewGuid().GetHashCode());
        return random.Next(minNumber, maxNumber + 1);
    }
    #endregion
}
