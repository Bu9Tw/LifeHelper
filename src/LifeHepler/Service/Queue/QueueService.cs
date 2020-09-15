using HeplerLibs.ExtLib;
using Microsoft.AspNetCore.Hosting;
using Service.Queue.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Service.Queue
{
    public class QueueService : IQueueService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public QueueService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 排隊
        /// </summary>
        /// <param name="type">The type.</param>
        public string AddQueue(QueueType type)
        {
            var now = GetTime.TwNowString();
            var guid = Guid.NewGuid().ToString();
            var fileName = $"{now}-{guid}";

            var dirPath = Path.Combine(_hostingEnvironment.ContentRootPath, "locker", type.ToString());

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var fullPath = Path.Combine(dirPath, fileName);

            using (var file = File.Create(fullPath))
            {
                var dot = new UTF8Encoding(true).GetBytes(".");
                file.Write(dot, 0, dot.Length);
            }

            return fullPath;
        }

        /// <summary>
        /// 能不能開始處理
        /// </summary>
        /// <param name="fileName"></param>
        public bool CanProcess(string fileFullPath)
        {
            var dirPath = Path.GetDirectoryName(fileFullPath);
            if (dirPath.Ext_IsNullOrEmpty())
                return false;

            var allFile = Directory.GetFiles(dirPath).OrderBy(x => x);

            if (Path.GetFileName(allFile.FirstOrDefault()) == Path.GetFileName(fileFullPath))
                return true;

            return false;

        }

        public bool ProcessDone(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
                File.Delete(fileFullPath);
            return true;
        }
    }
}
