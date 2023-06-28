using HiLoGame.Api.Requests;
using HiLoGame.Application.Dtos;
using HiLoGame.Application.Filters;
using HiLoGame.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HiLoGame.Api.Controllers;


[ApiController]
[Route("api/game")]
[Produces("application/json")]
public class GamesController : Controller
{
    private readonly ILogger _logger;
    private readonly IGameService _gameService;

    public GamesController(ILogger<GamesController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    [HttpGet]
    [UserAgentFilter]
    [ProducesResponseType(typeof(GetGameResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetGameById([FromQuery] Guid gameId)
    {
        try
        {
            var game = await _gameService.GetGameAsync(gameId);
            return Ok(game);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [UserAgentFilter]
    [ProducesResponseType(typeof(CreateGameResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request)
    {
        try
        {
            var game = await _gameService.CreateGameAsync(request.MinNumber, request.MaxNumber);
            return Ok(new CreateGameResponse
            {
                GameId = game.Id,
                MinNumber = game.MinNumber,
                MaxNumber = game.MaxNumber,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/{gameId}/start")]
    [UserAgentFilter]
    [ProducesResponseType(typeof(StartGameResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> StartGame([FromRoute] Guid gameId)
    {
        try
        {
            var game = await _gameService.StartGameAsync(gameId);
            return Ok(new StartGameResponse
            {
                GameId = game.Id,
                IsGameStarted = game.IsGameStarted
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{gameId}/join")]
    [UserAgentFilter]
    [ProducesResponseType(typeof(JoinGameResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> JoinGame([FromRoute] Guid gameId, [FromQuery] string playerName)
    {
        try
        {
            var player = await _gameService.JoinGameAsync(gameId, playerName);
            return Ok(new JoinGameResponse
            {
                GameId = gameId,
                PlayerId = player.Id,
                PlayerName = player.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{gameId}/{playerId}/makeguess")]
    [UserAgentFilter]
    [ProducesResponseType(typeof(MakeGuessResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> MakeGuess([FromRoute] Guid gameId, [FromRoute] Guid playerId, [FromQuery] int guessNumber)
    {
        try
        {
            var guess = await _gameService.MakeGuessAsync(gameId, playerId, guessNumber);
            return Ok(new MakeGuessResponse
            {
                GameId = gameId,
                Feedback = guess.Feedback
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
