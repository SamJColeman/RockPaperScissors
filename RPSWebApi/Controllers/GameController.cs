using System;
using Microsoft.AspNetCore.Mvc;
using RPSCore;
using System.Threading.Tasks;
using Contracts;

namespace RPSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRPSPlayer rpsPlayer;

        public GameController(IRPSPlayer rpsPlayer)
        {
            this.rpsPlayer = rpsPlayer;
        }

        [HttpPost("GetReady")]
        public async Task<ActionResult<string>> GetReady(int numOfGames, int numOfDynamite)
        {
            return Created(nameof(GetReady), await rpsPlayer.GetReady(numOfGames, numOfDynamite));
        }

        [HttpGet("MakeMove")]
        public async Task<ActionResult<Move>> GetMove()
        {
            return Ok(await rpsPlayer.MakeMove());
        }

        [HttpPatch("GameResult")]
        public async Task<ActionResult> AddGameResult(Outcome yourOutcome, Move opponentMove)
        {
            await rpsPlayer.GameResult(yourOutcome, opponentMove);
            return Ok();
        }

        [HttpPatch("Result")]
        public async Task<ActionResult<string>> AddResult(Outcome yourOutcome)
        {
            return Ok(await rpsPlayer.Result(yourOutcome));
        }
    }
}
