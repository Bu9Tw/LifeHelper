﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.GoogleSheet.Interface;

namespace LifeHepler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MethodTestController : ControllerBase
    {
        private readonly IGoogleSheetService _googleSheetService;

        public MethodTestController(IGoogleSheetService googleSheetService)
        {
            _googleSheetService = googleSheetService;

        }

        [HttpGet("WriteGoogleSheet")]
        public string WriteGoogleSheet()
        {
            return "";
            var twToday = DateTime.Now.AddHours(8);

            var WorkSheetData1 = new List<IList<object>>
            {
                new List<object>
                {
                    twToday.ToString("dd"),
                    "現金1",
                    "100",
                    "測試_1"
                }
            };
            var WorkSheetData2 = new List<IList<object>>
            {
                new List<object>
                {
                    twToday.ToString("dd"),
                    "現金_2",
                    "100",
                    "測試_2"
                }
            };
            var WorkSheetData3 = new List<IList<object>>
            {
                new List<object>
                {
                    twToday.ToString("dd"),
                    "現金_3",
                    "100",
                    "測試_3"
                }
            };

            var tableName = "Test";
            var column = "A";

            var columnNumber = (_googleSheetService.GetTotalColumnCount(tableName, column) + 1).ToString();
            var endColumn = char.ToString((char)(Convert.ToInt32(column[0]) + 3)) + columnNumber;
            column += columnNumber;

            var range = $"{tableName}!{column}:{ endColumn}";
            _googleSheetService.WriteValue(range, WorkSheetData1);


            _googleSheetService.WriteValue(range, WorkSheetData1);
            _googleSheetService.WriteValue(range, WorkSheetData2);
            _googleSheetService.WriteValue(range, WorkSheetData3);

            WorkSheetData1 = new List<IList<object>>
            {
                new List<object>
                {
                    twToday.ToString("dd"),
                    "現金_4",
                    "400",
                    "測試_4",
                },
                new List<object>
                {
                    twToday.ToString("dd"),
                    "現金_5",
                    "500",
                    "測試_5",
                },
                new List<object>
                {
                    twToday.ToString("dd"),
                    "現金_6",
                    "600",
                    "測試_6"
                }
            };

            _googleSheetService.WriteValue("Test!F8:I11", WorkSheetData1);

            return "";
        }
    }
}