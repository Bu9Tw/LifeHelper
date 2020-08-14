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

        [HttpGet("GetJobInfo")]
        public object GetOneOFourXml(int type)
        {
            var sourceUrl = _oneOFourCrawlerService.GetSourceUrl(type);
            var result = new
            {
                sourceUrl,
                jobData = _oneOFourCrawlerService.SynAndReadData(type).Select(x => new
                {
                    SynchronizeDate = x.SynchronizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    x.OneOFourHtmlJobInfos
                })
            };

            return result;
        }
    }
}
