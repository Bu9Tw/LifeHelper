using Model.Crawler;
using System.Collections.Generic;

namespace Service.Crawler.Interface
{
    public interface IOneOFourCrawlerService
    {
        /// <summary>
        /// 更新與存取檔案
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OneOFourHtmlModel> SynAndReadData(int userType);

        /// <summary>
        /// 同步104職缺資料
        /// </summary>
        public void SynchronizeOneOFourXml(int userType);

        /// <summary>
        /// 取得當天已存在的xml檔案資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OneOFourHtmlModel> GetOneOFourLocalXmlInfo(int userType);

        /// <summary>
        /// 取得來源URL
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        /// <returns></returns>
        public string GetSourceUrl(int userType);

    }
}
