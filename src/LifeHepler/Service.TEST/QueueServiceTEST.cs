using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Queue;
using Service.Queue.Interface;
using System;
using System.IO;

namespace Service.TEST
{
    [TestClass]
    public class QueueServiceTEST
    {
        private IQueueService _queueService;
        private IHostingEnvironment _hostingEnvironment;

        //public QueueServiceTEST(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        [TestInitialize]
        public void Initial()
        {
            var hostingEnvironment = new HostingEnvironment
            {
                ContentRootPath = @"D:\"
            };
            _hostingEnvironment = hostingEnvironment;
            _queueService = new QueueService(_hostingEnvironment);
        }

        [TestMethod]
        public void Queue()
        {
            var rootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "locker", "UnitTest");
            if (Directory.Exists(rootPath))
            {
                var existFile = Directory.GetFiles(rootPath);
                foreach (var item in existFile)
                    File.Delete(item);
            }

            var queueFilePath = _queueService.AddQueue(QueueType.UnitTest);
            var canDo = _queueService.CanProcess(queueFilePath);

            if (!canDo)
                throw new NotImplementedException($"Can Do Is {canDo}");

            Assert.IsTrue(_queueService.ProcessDone(queueFilePath));
        }
    }
}
