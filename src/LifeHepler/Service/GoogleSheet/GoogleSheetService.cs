using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using HeplerLibs.ExtLib;
using Microsoft.Extensions.Options;
using Model.GoogleSheet;
using Service.GoogleSheet.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;


namespace Service.GoogleSheet
{
    public class GoogleSheetService : IGoogleSheetService
    {
        private readonly GoogleSheetModel _googleSheet;
        private readonly SheetsService _SheetsService;
        public GoogleSheetService(
            IOptions<GoogleSheetCredential> googleSheetCredential,
            IOptions<GoogleSheetModel> googleSheet
            )
        {
            var credentialString = "{\"installed\":"
                                      + googleSheetCredential.Value.Ext_ToJson()
                                      + "}";

            _SheetsService = OpenSheet(credentialString);
            _googleSheet = googleSheet.Value;

        }

        private SheetsService OpenSheet(string credentialString)
        {

            var credentialBytes = Encoding.UTF8.GetBytes(credentialString);

            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "Line Bot Google Sheet";

            UserCredential credential;

            using (Stream stream = new MemoryStream(credentialBytes))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        public IList<IList<object>> ReadValue(string range)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _SheetsService.Spreadsheets.Values.Get(_googleSheet.SpreadSheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            return values;
        }

        public int GetTotalColumnCount(string tableName, string startColumn)
        {
            //設定讀取最後一行位置
            var range = $"{tableName}!{startColumn}:{startColumn}";

            //最後一行位置
            IList<IList<Object>> values = ReadValue(range);

            return values.Count();
        }

        /// <summary>
        /// 從LineBot寫入資料至GoogleSheet
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="startColumn"></param>
        /// <param name="insertValue"></param>
        /// <param name="sheetId"></param>
        /// <param name="credentialString"></param>
        /// <param name="row"></param>
        public void WriteValue(string range, List<IList<object>> insertValue)
        {
            var writenRange = new ValueRange
            {
                Range = range,
                Values = insertValue,
                MajorDimension = "ROWS"
            };

            SpreadsheetsResource.ValuesResource.UpdateRequest upd
            = _SheetsService.Spreadsheets.Values.Update(writenRange, _googleSheet.SpreadSheetId, range);

            upd.ValueInputOption =
                SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            UpdateValuesResponse responses = upd.Execute();


        }
    }
}
