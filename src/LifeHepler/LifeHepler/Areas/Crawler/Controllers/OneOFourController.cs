using Microsoft.AspNetCore.Mvc;
using Service.Crawler.Interface;

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
        public string GetOneOFourXml()
        {
            return _oneOFourCrawlerService.SynAndReadData(1);
        }

        [HttpGet("GetJobInfoForChien")]
        public string GetOneOFourXml_2()
        {
            return _oneOFourCrawlerService.SynAndReadData(2);
        }
    }
}
