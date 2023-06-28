using AutoMapper;
using HiLoGame.Application.Dtos;
using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.Entities;
using HiLoGame.Domain.MappingProfiles;
using HiLoGame.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace HiLoGame.UnitTests.Services;

public class GameServiceTests
{
    private readonly IGameService _sut;
    private readonly Mock<IGameRepository> _gameRepositoryMock = new();
    private readonly Mock<IPlayerRepository> _playerRepositoryMock = new ();
    private readonly Mock<ILogger<GameService>> _loggerMock = new ();

    public GameServiceTests()
    {
        var mapper = new MapperConfiguration(x => x.AddProfile<GameMappingProfile>()).CreateMapper();

        _sut = new GameService(_gameRepositoryMock.Object, _playerRepositoryMock.Object, _loggerMock.Object, mapper);
    }

    [Fact]
    public async Task GetGameAsync_WithValidGameId_ReturnsGameDto()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameEntity = new GameEntity { Id = gameId };
        var gameDto = new GameDto { Id = gameId };
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);

        // Act
        var result = await _sut.GetGameAsync(gameId);

        // Assert
        Assert.Equal(gameDto.Id, result.Id);
        Assert.Equal(gameDto.MinNumber, result.MinNumber);
        Assert.Equal(gameDto.MaxNumber, result.MaxNumber);
        Assert.Equal(gameDto.Players.Count, result.Players.Count);
        Assert.Equal(gameDto.Guesses.Count, result.Guesses.Count);
        Assert.Equal(gameDto.MysteryNumber, result.MysteryNumber);
    }

    [Fact]
    public async Task GetGameAsync_WithNonExistingGameId_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync((GameEntity)null);

        // Act
        Func<Task> act = () => _sut.GetGameAsync(gameId);
        
        // Assert
        Exception exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Game not found", exception.Message);
    }

    [Fact]
    public async Task CreateGameAsync_WithValidNumberRange_CreatesGameAndReturnsGameDto()
    {
        // Arrange
        var minNumber = 1;
        var maxNumber = 100;
        var mysteryNumber = 42;
        var gameEntity = new GameEntity { Id = Guid.NewGuid(), MysteryNumber = mysteryNumber };
        var gameDto = new GameDto { Id = gameEntity.Id, MysteryNumber = mysteryNumber };
        _gameRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<GameEntity>())).ReturnsAsync(gameEntity);

        // Act
        var result = await _sut.CreateGameAsync(minNumber, maxNumber);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<GameEntity>()), Times.Once);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(gameDto.MinNumber, result.MinNumber);
        Assert.Equal(gameDto.MaxNumber, result.MaxNumber);
        Assert.Equal(gameDto.Players.Count, result.Players.Count);
        Assert.Equal(gameDto.Guesses.Count, result.Guesses.Count);
        Assert.Equal(gameDto.MysteryNumber, result.MysteryNumber);
    }

    [Fact]
    public async Task CreateGameAsync_WithInvalidNumberRange_ThrowsValidationException()
    {
        // Arrange
        var minNumber = 100;
        var maxNumber = 50;

        // Act 
        Func<Task> act = () => _sut.CreateGameAsync(minNumber, maxNumber);
        
        // Assert
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Equal("Invalid number range. The minimum number must be smaller than the maximum number.", exception.Message);
    }

    [Fact]
    public async Task JoinGameAsync_WithValidGameIdAndNewPlayerName_CreatesPlayerAndReturnsPlayerDto()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerName = "John";
        var gameEntity = new GameEntity { Id = gameId, IsGameStarted = false };
        var playerEntity = new PlayerEntity { Id = Guid.NewGuid(), Name = playerName, GameId = gameId };
        var playerDto = new PlayerDto { Id = playerEntity.Id, Name = playerEntity.Name };
        
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);
        _playerRepositoryMock.Setup(repo => repo.GetByNameAsync(playerName)).ReturnsAsync((PlayerEntity)null);
        _playerRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<PlayerEntity>())).ReturnsAsync(playerEntity);

        // Act
        var result = await _sut.JoinGameAsync(gameId, playerName);

        // Assert
        _playerRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<PlayerEntity>()), Times.Once);
        Assert.Equal(playerDto.Id, result.Id);
        Assert.Equal(playerDto.Name, result.Name);
    }

    [Fact]
    public async Task JoinGameAsync_WithValidGameIdAndExistingPlayerName_ReturnsPlayerDto()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerName = "John";
        var gameEntity = new GameEntity { Id = gameId, IsGameStarted = false };
        var playerEntity = new PlayerEntity { Id = Guid.NewGuid(), Name = playerName, GameId = gameId };
        var playerDto = new PlayerDto { Id = playerEntity.Id, Name = playerEntity.Name };

        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);
        _playerRepositoryMock.Setup(repo => repo.GetByNameAsync(playerName)).ReturnsAsync(playerEntity);

        // Act
        var result = await _sut.JoinGameAsync(gameId, playerName);

        // Assert
        _playerRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<PlayerEntity>()), Times.Never);
        Assert.Equal(playerDto.Id, result.Id);
        Assert.Equal(playerDto.Name, result.Name);
    }

    [Fact]
    public async Task JoinGameAsync_WithStartedGame_ThrowsValidationException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerName = "John";
        var gameEntity = new GameEntity { Id = gameId, IsGameStarted = true };
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);

        // Act 
        Func<Task> act = () => _sut.JoinGameAsync(gameId, playerName);

        // Assert
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Equal($"Game {gameEntity.Id} already started. Cannot join in this game.", exception.Message);
    }

    [Fact]
    public async Task MakeGuessAsync_WithValidInputAndLowerNumber_ReturnsGuessDto()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var guessNumber = 50;
        var minNumber = 1;
        var maxNumber = 100;
        var mysteryNumber = 42;
        var gameEntity = new GameEntity
        {
            Id = gameId,
            MinNumber = minNumber,
            MaxNumber = maxNumber,
            MysteryNumber = mysteryNumber,
            IsGameFinished = false,
            Players = new List<PlayerEntity> { new PlayerEntity { Id = playerId } }
        };
        var playerEntity = new PlayerEntity { Id = playerId };
        var guessEntity = new GuessEntity { Player = playerEntity, Game = gameEntity, GuessNumber = guessNumber, Feedback = "LO" };
        var guessDto = new GuessDto { Id = Guid.NewGuid(), PlayerId = playerId, GuessNumber = guessNumber, Feedback = "LO" };
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);
        _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync(playerEntity);
        _gameRepositoryMock.Setup(repo => repo.Update(gameEntity)).ReturnsAsync(gameEntity);

        // Act
        var result = await _sut.MakeGuessAsync(gameId, playerId, guessNumber);

        // Assert
        Assert.Equal(guessDto.GuessNumber, result.GuessNumber);
        Assert.Equal(guessDto.Feedback, result.Feedback);
        Assert.Equal(guessDto.GameId, result.GameId);
    }

    [Fact]
    public async Task MakeGuessAsync_WithNonExistingGameId_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var guessNumber = 50;
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync((GameEntity)null);

        // Act 
        Func<Task> act = () => _sut.MakeGuessAsync(gameId, playerId, guessNumber);

        // Assert
        Exception exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Game not found", exception.Message);
    }

    [Fact]
    public async Task MakeGuessAsync_WithNonExistingPlayerId_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var guessNumber = 50;
        var gameEntity = new GameEntity { Id = gameId };
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);
        _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync((PlayerEntity)null);

        // Act 
        Func<Task> act = () => _sut.MakeGuessAsync(gameId, playerId, guessNumber);

        // Assert
        Exception exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Player not found", exception.Message);
    }

    [Fact]
    public async Task MakeGuessAsync_WithFinishedGame_ThrowsValidationException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var guessNumber = 50;
        var gameEntity = new GameEntity 
        { 
            Id = gameId, 
            IsGameFinished = true,
            Players = new List<PlayerEntity> { new PlayerEntity { Id = playerId } }
        };
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);

        // Act
        Func<Task> act = () => _sut.MakeGuessAsync(gameId, playerId, guessNumber);

        // Assert
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Equal("The game has already been finished", exception.Message);
    }

    [Fact]
    public async Task MakeGuessAsync_WithInvalidGuessNumber_ThrowsValidationException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var guessNumber = 100;
        var minNumber = 1;
        var maxNumber = 50;
        var gameEntity = new GameEntity 
        { 
            Id = gameId, 
            MinNumber = minNumber, 
            MaxNumber = maxNumber,
            Players = new List<PlayerEntity> { new PlayerEntity { Id = playerId, Name = "John" } }
        };
        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(gameEntity);

        // Act & Assert
        // Act
        Func<Task> act = () => _sut.MakeGuessAsync(gameId, playerId, guessNumber);

        // Assert
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Equal("Invalid guess number. The number must be within the range specified for the game.", exception.Message);
    }
}
