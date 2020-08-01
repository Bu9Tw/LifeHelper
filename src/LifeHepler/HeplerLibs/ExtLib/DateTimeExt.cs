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
    }
}
