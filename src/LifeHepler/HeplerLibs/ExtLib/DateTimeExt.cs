using System;
using System.Collections.Generic;
using System.Text;

namespace HeplerLibs.ExtLib
{
    public class DateTimeExt
    {

    }

    public static class GetTime
    {
        public static DateTime TwNow => DateTime.Now.ToUniversalTime().AddHours(8);
        public static DateTime UtcNow => DateTime.Now.ToUniversalTime();

        public static string TwNowString(string format = "yyyyMMddHHmmssfff")
        {
            return TwNow.ToString(format);
        }

    }
}
