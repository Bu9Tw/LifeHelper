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

        private IEnumerable<object> SynAndReadData(int type)
        {
            var result = _oneOFourCrawlerService.SynAndReadData(type).Select(x => new
            {
                SynchronizeDate = x.SynchronizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                x.OneOFourHtmlJobInfos
            });

            return result;
        }

        [HttpGet("GetJobInfo")]
        public IEnumerable<object> GetOneOFourXml(int type)
        {
            return SynAndReadData(type);
        }

        [HttpGet("GetJobInfoForChien")]
        public IEnumerable<object> GetOneOFourXml_2()
        {
            return SynAndReadData(2);
        }
    }
}
