using HeplerLibs.LineBot;
using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.GoogleSheet;
using Model.LineBot;
using Service.GoogleSheet.Interface;
using Service.LineBot;
using System;
using System.Threading.Tasks;

namespace LifeHepler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        private readonly LineBotSecretKeyModel _lineBotSecretKey;
        private readonly GoogleSheetModel _googleSheet;
        private readonly IGoogleSheetService _googleSheetService;
        private readonly LineMessagingClient _lineMessagingClient;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public LineBotController(IOptions<LineBotSecretKeyModel> lineBotSecretKey,
            IOptions<GoogleSheetModel> googleSheet,
            IGoogleSheetService googleSheetService,
            IServiceProvider serviceProvider)
        {
            _lineBotSecretKey = lineBotSecretKey.Value;
            _googleSheet = googleSheet.Value;
            _googleSheetService = googleSheetService;
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = _httpContextAccessor.HttpContext;
            _lineMessagingClient = new LineMessagingClient(_lineBotSecretKey.AccessToken);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var events = await _httpContext.Request.GetWebhookEventsAsync(_lineBotSecretKey.ChannelSecret);
            var lineMessagingClient = new LineMessagingClient(_lineBotSecretKey.AccessToken);
            var lineBotApp = new LineBotApp(lineMessagingClient, _googleSheetService, _googleSheet);
            await lineBotApp.RunAsync(events);
            return Ok();

        }

    }
}
