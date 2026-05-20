using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardController : ControllerBase
{
    private readonly BoardService _boardService;

    public BoardController(BoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(BoardStateResponse), StatusCodes.Status200OK)]
    public ActionResult<BoardStateResponse> GetState()
    {
        _boardService.ApplyDueBotMove();

        return Ok(BuildState());
    }

    [HttpPost("games")]
    [ProducesResponseType(typeof(BoardStateResponse), StatusCodes.Status201Created)]
    public ActionResult<BoardStateResponse> StartNewGame()
    {
        _boardService.StartNewGame();

        return Created("/api/board", BuildState());
    }

    [HttpPost("moves")]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public ActionResult<BoardResponse> PlayMove([FromBody] MoveRequest request)
    {
        if (request.Cell < 0 || request.Cell > 8)
        {
            return Problem(
                detail: "Cell index must be between 0 and 8.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (_boardService.GetCurrentTurn() != Turn.Human)
        {
            return Problem(
                detail: "It is not your turn: the bot is playing, wait for it.",
                statusCode: StatusCodes.Status409Conflict);
        }

        if (!_boardService.TryPlayHumanMove(request.Cell))
        {
            return Problem(
                detail: "Cell is already occupied.",
                statusCode: StatusCodes.Status409Conflict);
        }

        return Ok(new BoardResponse { Board = _boardService.GetBoard() });
    }

    private BoardStateResponse BuildState()
    {
        return new BoardStateResponse
        {
            Board = _boardService.GetBoard(),
            CurrentTurn = TurnToMark(_boardService.GetCurrentTurn()),
            XPlayer = BoardService.HumanName,
            OPlayer = BoardService.BotName
        };
    }

    private static string TurnToMark(Turn turn)
    {
        return turn == Turn.Human ? BoardService.HumanMark : BoardService.BotMark;
    }
}
