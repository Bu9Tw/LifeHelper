using Model.Crawler;
using System.Collections.Generic;

namespace Service.Crawler.Interface
{
    public interface IOneOFourCrawlerService
    {
        /// <summary>
        /// 同步104職缺資料
        /// </summary>
        public OneOFourHtmlModel SynchronizeOneOFourXml(int userType);

        /// <summary>
        /// 取得當天已存在的xml檔案資料
        /// </summary>
        public List<OneOFourHtmlModel> GetOneOFourLocalXmlInfo(int userType, bool onlyShowMatch);

        /// <summary>
        /// 取得來源URL
        /// </summary>
        public string GetSourceUrl(int userType);

        /// <summary>
        /// 是否需要同步工作資料
        /// </summary>
        public bool IsNeedToSynJobData(int userType);


    }
}
