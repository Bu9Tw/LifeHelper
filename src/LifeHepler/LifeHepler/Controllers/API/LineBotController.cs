using System;
using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.GoogleSheet;
using Model.LineBot;
using Service.GoogleSheet.Interface;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using HeplerLibs.LineBot;
using Service.LineBot;
using Service.Queue.Interface;

namespace LifeHepler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        private readonly LineBotSecretKeyModel _lineBotSecretKey;
        private readonly GoogleSheetModel _googleSheet;
        private readonly IGoogleSheetService _googleSheetService;
        private readonly IQueueService _queueService;
        private readonly LineMessagingClient _lineMessagingClient;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public LineBotController(IOptions<LineBotSecretKeyModel> lineBotSecretKey,
            IOptions<GoogleSheetModel> googleSheet,
            IGoogleSheetService googleSheetService,
            IServiceProvider serviceProvider,
            IQueueService queueService)
        {
            _lineBotSecretKey = lineBotSecretKey.Value;
            _googleSheet = googleSheet.Value;
            _googleSheetService = googleSheetService;
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = _httpContextAccessor.HttpContext;
            _lineMessagingClient = new LineMessagingClient(_lineBotSecretKey.AccessToken);
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var events = await _httpContext.Request.GetWebhookEventsAsync(_lineBotSecretKey.ChannelSecret);
            var lineMessagingClient = new LineMessagingClient(_lineBotSecretKey.AccessToken);
            var lineBotApp = new LineBotApp(lineMessagingClient, _googleSheetService, _googleSheet, _queueService);
            await lineBotApp.RunAsync(events);
            return Ok();

        }

    }
}
