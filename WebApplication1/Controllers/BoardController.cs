using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
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

    [HttpPost("moves")]
    [ProducesResponseType(typeof(MovePlayedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public ActionResult<MovePlayedResponse> PlayMove([FromBody] MoveRequest request)
    {
        if (request.Cell < 0 || request.Cell > 8)
        {
            return Problem(
                detail: "Cell index must be between 0 and 8.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (!_boardService.TryPlayHumanMove(request.Cell))
        {
            return Problem(
                detail: "Cell is already occupied.",
                statusCode: StatusCodes.Status409Conflict);
        }

        return Ok(new MovePlayedResponse
        {
            Cell = request.Cell,
            Mark = BoardService.HumanMark,
            Board = _boardService.GetBoard()
        });
    }

    [HttpPost("bot-move")]
    [ProducesResponseType(typeof(MovePlayedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public ActionResult<MovePlayedResponse> PlayBotMove()
    {
        int? chosenCell = _boardService.PlayBotMoveOnRandomEmptyCell();
        if (chosenCell is null)
        {
            return Problem(
                detail: "The board is full, no move available.",
                statusCode: StatusCodes.Status409Conflict);
        }

        return Ok(new MovePlayedResponse
        {
            Cell = chosenCell.Value,
            Mark = BoardService.BotMark,
            Board = _boardService.GetBoard()
        });
    }
}
