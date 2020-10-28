using HeplerLibs.ExtLib;
using Line.Messaging;
using Line.Messaging.Webhooks;
using Model.GoogleSheet;
using Service.GoogleSheet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.LineBot
{
    public class LineBotApp : WebhookApplication
    {
        private readonly LineMessagingClient _messagingClient;
        private readonly IGoogleSheetService _googleSheetService;
        private readonly GoogleSheetModel _googleSheet;

        private static readonly object LockObj = new object();

        public LineBotApp(LineMessagingClient lineMessagingClient,
            IGoogleSheetService googleSheetService,
            GoogleSheetModel googleSheet)
        {
            _messagingClient = lineMessagingClient;
            _googleSheet = googleSheet;
            _googleSheetService = googleSheetService;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            //使用者Id
            var userId = ev.Source.UserId;
            var result = new List<ISendMessage>();

            if (userId == _googleSheet.Bo || userId == _googleSheet.Chien)
            {
                switch (ev.Message)
                {
                    //文字訊息
                    case TextEventMessage textMessage:

                        //切開分隔，然後只篩不為空白的
                        var textSplitData = textMessage.Text.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList<object>();
                        var WorkSheetData = new List<IList<object>>();
                        var twToday = GetTime.TwNow;

                        if (textSplitData.Count > 4)
                            result.Add(new TextMessage("格式錯誤 ! "));
                        else
                        {
                            var tmp = new List<object>();
                            switch (textSplitData.Count)
                            {
                                case 1:
                                    tmp = new List<object>
                                    {
                                        twToday.ToString("dd"),
                                        "現金",
                                    };
                                    tmp.AddRange(textSplitData);
                                    tmp.Add("其他");
                                    break;
                                case 2:
                                    tmp = new List<object>
                                    {
                                        twToday.ToString("dd"),
                                        "現金",
                                    };
                                    tmp.AddRange(textSplitData);
                                    break;
                                case 3:
                                    tmp.Add(twToday.ToString("dd"));
                                    tmp.AddRange(textSplitData);
                                    break;
                                case 4:
                                    tmp.AddRange(textSplitData);
                                    break;
                            }

                            WorkSheetData.Add(tmp);

                            var tableName = twToday.ToString("MM月");

                            var startColumn = (_googleSheet.Bo == userId ? "A" : "F");
                            lock (LockObj)
                            {
                                var columnNumber = (_googleSheetService.GetTotalColumnCount(tableName, startColumn) + 1).ToString();
                                var endColumn = char.ToString((char)(Convert.ToInt32(startColumn[0]) + 3)) + columnNumber;
                                startColumn += columnNumber;

                                var range = $"{tableName}!{startColumn}:{ endColumn}";
                                _googleSheetService.WriteValue(range, WorkSheetData);
                            }
                            result.Add(new TextMessage("輸入成功 ! "));

                        }
                        break;
                }
            }
            if (result.Any())
                await _messagingClient.ReplyMessageAsync(ev.ReplyToken, result);
        }
    }
}
