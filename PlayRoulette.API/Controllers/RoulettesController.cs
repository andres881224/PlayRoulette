using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Enum;
using PlayRoulette.API.Helpers;
using PlayRoulette.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayRoulette.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly IRouletteHelper _rouletteHelper;
        private readonly ILogger<RoulettesController> _logger;

        public RoulettesController(IRouletteHelper rouletteHelper, ILogger<RoulettesController> logger)
        {
            _rouletteHelper = rouletteHelper;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateRoulette")]
        public async Task<ActionResult<Guid>> CreateRoulette()
        {
            _logger.LogInformation(Constants.MessageLogCreateRoulette);
            return await _rouletteHelper.Create();
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("OpenRoulette")]
        public async Task<ActionResult<string>> OpenRoulette(RouletteRequest model)
        {
            _logger.LogInformation(Constants.MessageLogOpenRoulette);
            if (await _rouletteHelper.Open(model.Name))
            {
                return Constants.MessageSuccessful;
            }

            return Constants.MessageDenied;
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("CloseRoulette")]
        public async Task<ActionResult<IEnumerable<HistoryRouletteWinners>>> CloseRoulette(RouletteRequest model)
        {
            _logger.LogInformation(Constants.MessageLogCloseRoulette);
            return await _rouletteHelper.Close(model.Name);
        }

        [HttpPost]
        [Route("BetColor")]
        public async Task<ActionResult<string>> BetColor(HistoryRouletteColor model)
        {
            HistoryRoulette bet = new HistoryRoulette()
            {
                RouletteId = (await _rouletteHelper.GetStatus()).Where(x => x.Name == model.RouletteId).FirstOrDefault().Id,
                TypeBet = TypeBet.Color,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Number = 0,
                Color = model.Color,
                BetValue = model.BetValue
            };

            return await _rouletteHelper.Bet(bet);
        }

        [HttpPost]
        [Route("BetNumber")]
        public async Task<ActionResult<string>> BetNumber(HistoryRouletteNumber model)
        {
            HistoryRoulette bet = new HistoryRoulette()
            {
                RouletteId = (await _rouletteHelper.GetStatus()).Where(x => x.Name == model.RouletteId).FirstOrDefault().Id,
                TypeBet = TypeBet.Number,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Number = model.Number,
                Color = Colors.NA,
                BetValue = model.BetValue
            };

            return await _rouletteHelper.Bet(bet);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetStatus")]
        public async Task<ActionResult<IEnumerable<Roulette>>> GetStatus()
        {
            _logger.LogInformation(Constants.MessageLogGetStatus);
            return await _rouletteHelper.GetStatus();
        }

    }
}

