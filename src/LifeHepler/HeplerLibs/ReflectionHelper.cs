using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HeplerLibs
{
    public class ReflectionHelper
    {
        public static T ToType<T>(object obj)
        {
            var jsonContent = JsonSerializer.Serialize(obj);
            var result = JsonSerializer.Deserialize<T>(jsonContent);

            if (result == null)
                throw new Exception(string.Format("ToType<T> 無法對「{0}」進行反序列化(JSON.NET)", typeof(T).ToString()));

            return result;
        }

         public static string ToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T ToType<T>(string jsonConvert)
        {
            return JsonSerializer.Deserialize<T>(jsonConvert);
        }

    }
}
