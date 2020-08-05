using System;
using System.Collections.Generic;
using System.Text;

namespace HeplerLibs.ExtLib
{
    public static class StringExt
    {
        /// <summary>
        /// 將字串轉換成Boolean值
        /// 允許的字串為(不分大小寫)：「true/false」、「t/f」、「yes/no」、「y/n」、「1/0」
        /// 如果轉換失敗，也會回傳false
        /// </summary>
        /// <param name="value">欲轉換的字串</param>
        /// <returns></returns>
        public static bool Ext_IsTrue(this string value)
        {

            if (ExtBoolean.Ext_TryParseBool(value, out bool result))
                return result;

            return default;
        }

        /// <summary>
        /// 即string.IsNullOrEmpty()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Ext_IsNullOrEmpty(this string value)
        {
            value = value ?? "";
            return string.IsNullOrEmpty(value.Trim());
        }

    }

    public static class ExtBoolean
    {
        /// <summary>
        /// 可對「true/false」、「t/f」、「yes/no」、「y/n」、「1/0」進行boolean轉換(不分大小寫)
        /// 原本的bool.TryParse，只能接受string 「true/false」
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="retVal"></param>
        /// <returns>回傳Boolean</returns>
        /// 參考資料：http://stackoverflow.com/questions/9191924/why-bool-try-parse-not-parsing-value-to-true-or-false
        public static bool Ext_TryParseBool(object obj, out bool boolResult)
        {
            //There are a couple of built-in ways to convert values to boolean, but unfortunately they skip things like YES/NO, 1/0, T/F
            //bool.TryParse(string, out bool retVal) (.NET 4.0 Only); Convert.ToBoolean(object) (requires try/catch)
            //「??」 運算子稱為 null 聯合運算子，用來定義可為 null 的實值型別和參考型別的預設值。 運算子如果不是 null 就會傳回左方運算元，否則傳回右運算元
            string value = (obj ?? "").ToString().Trim().ToUpper();
            switch (value)
            {
                case "TRUE":
                case "T":
                case "YES":
                case "Y":
                    boolResult = true;
                    return true;
                case "FALSE":
                case "F":
                case "NO":
                case "N":
                    boolResult = false;
                    return true;
                default:
                    // If value can be parsed as a number, 0==false, non-zero==true (old C/C++ usage)
                    double number;
                    if (double.TryParse(value, out number))
                    {
                        boolResult = (number != 0);
                        return true;
                    }
                    // If not a valid value for conversion, return false (not parsed)
                    boolResult = false;
                    return false;
            }
        }
    }
}
