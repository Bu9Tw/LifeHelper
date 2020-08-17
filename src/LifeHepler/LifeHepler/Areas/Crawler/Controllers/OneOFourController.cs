using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model.Crawler;
using Service.Crawler.Interface;
using System.Collections.Generic;
using System.Linq;

namespace LifeHepler.Areas.Crawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OneOFourController : ControllerBase
    {
        private readonly IOneOFourCrawlerService _oneOFourCrawlerService;

        public OneOFourController(IOneOFourCrawlerService oneOFourCrawlerService)
        {
            _oneOFourCrawlerService = oneOFourCrawlerService;
        }

        [HttpGet("GetSourceUrl")]
        public string GetSourceUrl(int type)
        {
            var sourceUrl = _oneOFourCrawlerService.GetSourceUrl(type);
            return sourceUrl;
        }

        [HttpGet("GetJobInfo")]
        public object GetOneOFourData(int type)
        {
            var result = _oneOFourCrawlerService
                    .GetOneOFourLocalXmlInfo(type)
                    .Select(x => new
                    {
                        SynchronizeDate = x.SynchronizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.OneOFourHtmlJobInfos
                    });

            return result;
        }

        [HttpPost("SynJobData")]
        public object SynOneOFourData(int type)
        {
            var result = _oneOFourCrawlerService
                    .SynAndReadData(type)
                    .Select(x => new
                    {
                        SynchronizeDate = x.SynchronizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.OneOFourHtmlJobInfos
                    });

            return result;
        }
    }
}
