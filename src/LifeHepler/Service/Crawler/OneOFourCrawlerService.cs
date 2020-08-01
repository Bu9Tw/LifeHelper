﻿using HeplerLibs.ExtLib;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace Service.Crawler
{
    public class OneOFourCrawlerService : IOneOFourCrawlerService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private int _userType { get; set; }
        private string _dirPath => Path.Combine(_hostingEnvironment.ContentRootPath, "OneOFourXml");
        private string _filePath => Path.Combine(_dirPath, $"{GetTime.TwNow:yyyyMMdd}_{_userType}.xml");
        private string _sourceUrl => _userType == 1 ?
                @"https://www.104.com.tw/jobs/search/?ro=0&isnew=0&keyword=.net&jobcatExpansionType=0&area=6001001000%2C6001002000&order=15&asc=0&s9=1&s5=0&wktm=1&page={0}&mode=s&searchTempExclude=2" :
                @"https://www.104.com.tw/jobs/search/?ro=0&jobcat=2003001006%2C2003001010%2C2002001000&isnew=0&jobcatExpansionType=1&area=6001001003%2C6001002003%2C6001002015%2C6001002021%2C6001002018%2C6001002020&order=11&asc=0&sctp=M&scmin=30000&scstrict=1&scneg=0&s9=1&wktm=1&page={0}&mode=s&searchTempExclude=2";


        public OneOFourCrawlerService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _userType = 1;
        }

        /// <summary>
        /// 取得當天已存在的xml檔案資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OneOFourHtmlModel> GetOneOFourLocalXmlInfo(int userType)
        {
            _userType = userType;
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
        public void SynchronizeOneOFourXml(int userType)
        {
            var oldJobData = GetOneOFourLocalXmlInfo(userType);

            if (oldJobData != null && oldJobData.Any() && oldJobData.Max(x => x.SynchronizeDate.Value).AddMinutes(10) > GetTime.TwNow)
                return;

            HttpWebRequest request;
            var page = 1;
            var requestUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            var requestAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            var htmlJobInfo = new OneOFourHtmlModel
            {
                SynchronizeDate = GetTime.TwNow,
                OneOFourHtmlJobInfos = new List<OneOFourHtmlModel.OneOFourHtmlJobInfo>()
            };

            while (true)
            {
                Thread.Sleep(100);
                //爬104資料清單
                request = (HttpWebRequest)WebRequest.Create(string.Format(_sourceUrl, page++));

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
                Thread.Sleep(100);
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

                            if (_userType != 1)
                                return true;

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

            htmlJobInfo.OneOFourHtmlJobInfos = htmlJobInfo.OneOFourHtmlJobInfos.Where(x => filterJobCondition(x)).ToList();

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

            //沒有新資料就不存
            if (htmlJobInfo.OneOFourHtmlJobInfos == null || !htmlJobInfo.OneOFourHtmlJobInfos.Any())
                return;

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
                                                    new XElement("No", ReplaceHexadecimalSymbols(y.No)),
                                                    new XElement("Name", ReplaceHexadecimalSymbols(y.Name)),
                                                    new XElement("CompanyNo", ReplaceHexadecimalSymbols(y.CompanyNo)),
                                                    new XElement("CompanyName", ReplaceHexadecimalSymbols(y.CompanyName)),
                                                    new XElement("DetailLink", ReplaceHexadecimalSymbols(y.DetailLink))))))));

            newXmlDoc.Save(_filePath);
        }

        private string ReplaceHexadecimalSymbols(string txt)
        {
            if (txt != "")
            {
                string r = "[\x00-\x08\x0B\x0C\x0E-\x1F]";
                return Regex.Replace(txt, r, "", RegexOptions.Compiled);
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 更新與存取檔案
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public IEnumerable<OneOFourHtmlModel> SynAndReadData(int userType)
        {
            SynchronizeOneOFourXml(userType);

            return GetOneOFourLocalXmlInfo(userType).OrderByDescending(x => x.SynchronizeDate);
        }


    }
}
