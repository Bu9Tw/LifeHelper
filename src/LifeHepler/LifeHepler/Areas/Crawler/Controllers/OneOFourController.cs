using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model.Crawler;
using Service.Crawler.Interface;
using System;
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
        public string GetSourceUrl(int userType)
        {
            var sourceUrl = _oneOFourCrawlerService.GetSourceUrl(userType);
            return sourceUrl;
        }

        [HttpGet("GetJobInfo")]
        public object GetOneOFourData(int userType)
        {
            var result = _oneOFourCrawlerService
                    .GetOneOFourLocalXmlInfo(userType, true)
                    .OrderByDescending(x => x.SynchronizeDate)
                    .Select(x => new
                    {
                        SynchronizeDate = x.SynchronizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        OneOFourHtmlJobInfos = x.OneOFourHtmlJobInfos.Select(x => { x.No = Guid.NewGuid().ToString(); return x; })
                    });

            return result;
        }

        [HttpPost("SynJobData")]
        public object SynOneOFourData([FromForm] OneOFourForm model)
        {
            var result = _oneOFourCrawlerService.SynchronizeOneOFourXml(model.UserType);
            return new
            {
                SynchronizeDate = result.SynchronizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                OneOFourHtmlJobInfos = result.OneOFourHtmlJobInfos.Select(x => { x.No = Guid.NewGuid().ToString(); return x; })
            };
        }
    }
}
