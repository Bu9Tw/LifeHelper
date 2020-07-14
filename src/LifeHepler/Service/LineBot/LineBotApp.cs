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

        public LineBotApp(LineMessagingClient lineMessagingClient,
            IGoogleSheetService googleSheetService,
            GoogleSheetModel googleSheet
            )
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
                        var twToday = DateTime.Now.AddHours(8);

                        if (textSplitData.Count > 4)
                            result.Add(new TextMessage("格式錯誤 ! "));
                        else if (textSplitData.Count == 1)
                        {
                            if (int.TryParse((string)textSplitData[0], out int price))
                            {
                                var weekDate = twToday.DayOfWeek.ToString("d");

                                var range = "賣菜農!";
                                if (weekDate == "0")
                                {
                                    //資料洗白
                                    range += "D2:J3";
                                    WorkSheetData.Add(new List<object> { textSplitData[0], "", "", "", "", "", "" });
                                    WorkSheetData.Add(new List<object> { "", "", "", "", "", "" });
                                }
                                else
                                {
                                    var startColumn = char.ToString((char)(68 + Convert.ToInt64(weekDate)));
                                    var columnNumber = Convert.ToInt32(twToday.ToString("HH")) < 12 ? "2" : "3";
                                    WorkSheetData.Add(textSplitData);
                                    range += $"{startColumn}{columnNumber}";
                                }

                                _googleSheetService.WriteValue(range, WorkSheetData);

                                result.Add(new TextMessage($"大頭菜{textSplitData[0]}元，輸入成功 ! "));
                            }

                            var readResult = _googleSheetService.ReadValue("賣菜農!B2:B3");

                            result.AddRange(new TextMessage[]
                            {
                                new TextMessage($"可能類型 : {(string)readResult[0][0]}"),
                                new TextMessage($"結果 : {(string)readResult[1][0]}"),
                            });

                        }
                        else
                        {
                            if (textSplitData.Count == 2)
                            {
                                var tmp = new List<object>
                                {
                                    twToday.ToString("dd"),
                                    "現金",
                                };
                                tmp.AddRange(textSplitData);

                                WorkSheetData.Add(tmp);
                            }
                            else if (textSplitData.Count == 3)
                            {

                                var tmp = new List<object>
                                {
                                    twToday.ToString("dd"),
                                };
                                tmp.AddRange(textSplitData);

                                WorkSheetData.Add(tmp);
                            }
                            else
                            {
                                WorkSheetData.Add(textSplitData);
                            }

                            var tableName = twToday.ToString("MM月");

                            var startColumn = (_googleSheet.Bo == userId ? "A" : "F");
                            var columnNumber = (_googleSheetService.GetTotalColumnCount(tableName, startColumn) + 1).ToString();
                            var endColumn = char.ToString((char)(Convert.ToInt32(startColumn[0]) + 3)) + columnNumber;
                            startColumn += columnNumber;

                            var range = $"{tableName}!{startColumn}:{ endColumn}";
                            _googleSheetService.WriteValue(range, WorkSheetData);

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
