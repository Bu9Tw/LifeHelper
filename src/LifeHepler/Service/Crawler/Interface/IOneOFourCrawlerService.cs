using Model.Crawler;
using System.Collections.Generic;

namespace Service.Crawler.Interface
{
    public interface IOneOFourCrawlerService
    {
        /// <summary>
        /// 存取實體資料
        /// </summary>
        /// <returns></returns>
        public string ReadLocalData();

        /// <summary>
        /// 同步104職缺資料
        /// </summary>
        public void SynchronizeOneOFourXml();

        /// <summary>
        /// 取得當天已存在的xml檔案資料
        /// </summary>
        /// <returns></returns>
        IEnumerable<OneOFourHtmlModel> GetOneOFourLocalXmlInfo();

    }
}
