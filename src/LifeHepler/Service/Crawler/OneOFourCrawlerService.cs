using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Model.Crawler;
using Service.Crawler.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace Service.Crawler
{
    public class OneOFourCrawlerService : IOneOFourCrawlerService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _dirPath;
        private readonly string _filePath;

        public OneOFourCrawlerService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _dirPath = Path.Combine(_hostingEnvironment.ContentRootPath, "OneOFourXml");
            _filePath = Path.Combine(_dirPath, $"{DateTime.Now:yyyyMMdd}.xml");
        }

        /// <summary>
        /// 取得當天已存在的xml檔案資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OneOFourHtmlModel> GetOneOFourLocalXmlInfo()
        {
            if (!Directory.Exists(_dirPath) || !File.Exists(_filePath))
                return null;

            var oldXmlDoc = XDocument.Load(_filePath);

            return oldXmlDoc.Element("Data").Elements("Block").Select(x => new OneOFourHtmlModel
            {
                SynchronizeDate = Convert.ToDateTime(x.Element("SynchronizeDate").Value),
                OneOFourHtmlJobInfos = x.Elements("Job").Select(y => new OneOFourHtmlModel.OneOFourHtmlJobInfo
                {
                    No = y.Element("No").Value,
                    Name = y.Element("Name").Value,
                    CompanyNo = y.Element("CompanyNo").Value,
                    CompanyName = y.Element("CompanyName").Value,
                    DetailLink = y.Element("DetailLink").Value,
                })
            });
        }

        /// <summary>
        /// 同步104職缺資料
        /// </summary>
        public void SynchronizeOneOFourXml()
        {
            var strUrl = @"https://www.104.com.tw/jobs/search/?ro=0&isnew=0&keyword=.net&jobcatExpansionType=0&area=6001001000%2C6001002000&order=15&asc=0&s9=1&s5=0&wktm=1&page={0}&mode=s&searchTempExclude=2";
            HttpWebRequest request;
            var page = 1;
            var requestUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36";
            var requestAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            var htmlJobInfo = new OneOFourHtmlModel
            {
                SynchronizeDate = DateTime.Now,
                OneOFourHtmlJobInfos = new List<OneOFourHtmlModel.OneOFourHtmlJobInfo>()
            };

            while (true)
            {
                //爬104資料清單
                request = (HttpWebRequest)WebRequest.Create(string.Format(strUrl, page++));

                request.UserAgent = requestUserAgent;
                request.Accept = requestAccept;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    HttpStatusCode code = response.StatusCode;
                    if (code == HttpStatusCode.OK)
                    {
                        HtmlDocument htmlDoc = new HtmlDocument
                        {
                            OptionFixNestedTags = true
                        };
                        using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            htmlDoc.Load(sr);
                        }
                        var articleList = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"js-job-content\"]/article");
                        if (articleList == null)
                            break;
                        //html To model
                        var tmpSimpleJobInfo = new OneOFourHtmlModel(articleList);
                        htmlJobInfo.OneOFourHtmlJobInfos = htmlJobInfo.OneOFourHtmlJobInfos.Concat(tmpSimpleJobInfo.OneOFourHtmlJobInfos);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //取得詳細資料的BaseUrl
            var baseUrl = @"https://www.104.com.tw/job/ajax/content/{0}";

            //判斷詳細工作資訊的condition
            var filterJobCondition = new Func<OneOFourHtmlModel.OneOFourHtmlJobInfo, bool>((x) =>
            {
                //取得該工作的網址編號
                var jobUrlKey = new Uri(x.DetailLink).AbsolutePath.Trim('/').Split('/').LastOrDefault();

                if (jobUrlKey == null)
                    return false;

                var ajaxUrl = string.Format(baseUrl, jobUrlKey);

                request = (HttpWebRequest)WebRequest.Create(ajaxUrl);
                request.UserAgent = requestUserAgent;
                request.Accept = requestAccept;
                //驗證用的
                request.Headers.Add("Referer", x.DetailLink);
                using (var response = (HttpWebResponse)(request.GetResponse()))
                {
                    HttpStatusCode code = response.StatusCode;
                    if (code == HttpStatusCode.OK)
                    {
                        using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            var jobDetailJson = sr.ReadToEnd();
                            var jobData = JsonSerializer.Deserialize<JobDetailInfo>(jobDetailJson);

                            //不找內湖
                            if (jobData.data.jobDetail.addressDetail.Contains("內湖") || jobData.data.jobDetail.addressRegion.Contains("內湖"))
                                return false;
                            //工作標題有沒有net
                            if (!jobData.data.header.jobName.ToLower().Contains("net") &&
                                //工作內容有沒有net
                                !jobData.data.jobDetail.jobDescription.ToLower().Contains("net") &&
                                //需要的技能有沒有net
                                (!jobData.data.condition.specialty.Any() ||
                                !jobData.data.condition.specialty.Any(x => x.description.ToLower().Contains("net"))))
                                return false;

                            return true;
                        }
                    }
                }
                return true;
            });

            htmlJobInfo.OneOFourHtmlJobInfos = htmlJobInfo.OneOFourHtmlJobInfos.Where(x => filterJobCondition(x));

            var oldJobData = GetOneOFourLocalXmlInfo();

            //如果沒有抓到新資料，就直接踢掉
            if (htmlJobInfo.OneOFourHtmlJobInfos == null || !htmlJobInfo.OneOFourHtmlJobInfos.Any())
                return;

            //如果沒有舊資料，就直接存不與新資料比較
            if (oldJobData == null)
            {
                SaveJobDataToLocal(new List<OneOFourHtmlModel> { htmlJobInfo });
                return;
            }

            var existJobNoList = oldJobData.SelectMany(x => x.OneOFourHtmlJobInfos.Select(y => y.No));
            htmlJobInfo.OneOFourHtmlJobInfos = htmlJobInfo.OneOFourHtmlJobInfos.Where(x => !existJobNoList.Contains(x.No));

            var totalJobData = new List<OneOFourHtmlModel> { htmlJobInfo };
            totalJobData.AddRange(oldJobData);

            SaveJobDataToLocal(totalJobData.OrderByDescending(x => x.SynchronizeDate));
        }

        /// <summary>
        /// 將資料存到Local
        /// </summary>
        /// <param name="jobData">The job data.</param>
        private void SaveJobDataToLocal(IEnumerable<OneOFourHtmlModel> jobData)
        {

            if (!Directory.Exists(_dirPath))
                Directory.CreateDirectory(_dirPath);

            var newXmlDoc = new XDocument(new XElement("Data",
                        jobData.Select(x => new XElement("Block",
                                                new XElement("SynchronizeDate", x.SynchronizeDate.Value.ToString("yyyy/MM/dd HH:mm:ss")),
                                                x.OneOFourHtmlJobInfos.Select(y =>
                                                new XElement("Job",
                                                    new XElement("No", y.No),
                                                    new XElement("Name", y.Name),
                                                    new XElement("CompanyNo", y.CompanyNo),
                                                    new XElement("CompanyName", y.CompanyName),
                                                    new XElement("DetailLink", y.DetailLink)))))));

            newXmlDoc.Save(_filePath);
        }

        public string ReadLocalData()
        {
            return string.Empty;
        }
    }
}
