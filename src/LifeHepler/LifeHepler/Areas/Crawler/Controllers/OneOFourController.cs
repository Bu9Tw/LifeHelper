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
        public object GetOneOFourData(int userType, int page, int pageRow)
        {
            var result = _oneOFourCrawlerService
                    .GetOneOFourLocalXmlInfo(userType, true)
                    .OrderBy(x => x.SynchronizeDate)
                    .SelectMany(x => x.OneOFourHtmlJobInfos);

            return new
            {
                TotalPage = result.Count() / pageRow + 1,
                JobInfo = result.Select(x =>
                {
                    x.No = Guid.NewGuid().ToString();
                    return x;
                }).Skip((page - 1) * pageRow).Take(pageRow)
            };
        }

        [HttpPost("SynJobData")]
        public void SynOneOFourData([FromForm] OneOFourForm model)
        {
            _oneOFourCrawlerService.SynchronizeOneOFourXml(model.UserType);
        }
    }
}
