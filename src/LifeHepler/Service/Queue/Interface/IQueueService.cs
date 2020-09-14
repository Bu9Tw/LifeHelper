using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Queue.Interface
{
    public enum QueueType
    {
        LineBot,
        OneOFour,
        UnitTest
    }

    public interface IQueueService
    {
        /// <summary>
        /// 排隊
        /// </summary>
        public string AddQueue(QueueType type);

        /// <summary>
        /// 能不能開始處理
        /// </summary>
        public bool CanProcess(string fileFullPath);

        /// <summary>
        /// 結束
        /// </summary>
        bool ProcessDone(string fileFullPath);
    }
}
