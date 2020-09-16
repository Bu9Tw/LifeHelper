using HeplerLibs.Paging;
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
                    .SelectMany(x => x.OneOFourHtmlJobInfos)
                    .OrderBy(x => x.IsReaded);

            return new
            {
                TotalPage = result.Count() / pageRow + 1,
                JobInfo = result.Paging(page, pageRow)
            };
        }

        [HttpPost("SynJobData")]
        public bool SynOneOFourData([FromForm] SynOneOFourForm model)
        {
            _oneOFourCrawlerService.SynchronizeOneOFourXml(model.UserType);
            return true;
        }

        [HttpPost("UpdateToReaded")]
        public OneOFourHtmlModel.OneOFourHtmlJobInfo UpdateToReaded([FromForm] OneOFourForm model)
        {
            _oneOFourCrawlerService.UpdateJobInfoToReaded(model);

            return _oneOFourCrawlerService
                .GetOneOFourLocalXmlInfo(model.UserType, true)
                .OrderBy(x => x.SynchronizeDate)
                .SelectMany(x => x.OneOFourHtmlJobInfos)
                .OrderBy(x => x.IsReaded)
                .Paging(model.CurPageNumber, model.PageRow)
                .LastOrDefault();
        }

    }
}
