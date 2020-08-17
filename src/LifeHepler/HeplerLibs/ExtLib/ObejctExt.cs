using System;
using System.Collections.Generic;
using System.Text;

namespace HeplerLibs.ExtLib
{
    public static class ObejctExt
    {
        /// <summary>
        /// 複製物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T Ext_Copy<T>(this T obj)
        {
            return ReflectionHelper.ToType<T>(obj);
        }

        /// <summary>
        /// 將物件(或集合)進行JSON序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string Ext_ToJson<T>(this T obj)
        {
            return ReflectionHelper.ToJson(obj);
        }

        /// <summary>
        /// 將物件轉型為特定型別
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonConvert">The json convert.</param>
        /// <returns></returns>
        public static T Ext_ToType<T>(this string jsonConvert)
        {
            return ReflectionHelper.ToType<T>(jsonConvert);
        }
    }
}
