using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HeplerLibs.ExtLib
{
    public static class IEnumerableExt
    {
        public static bool Ext_IsNullOrEmpty<T>(this IEnumerable<T> enumeration)
        {
            return enumeration == null || !enumeration.Any();
        }

        /// <summary>
        /// 以Key去除重複資料
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Ext_DistinctByKey<TSource, TKey>(this IEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            var keyGroup = source.GroupBy(keySelector.Compile()).Select(x => x.Key);
            var result = new List<TSource>();

            foreach (var item in keyGroup)
            {
                var bd = Expression.Equal(keySelector.Body, Expression.Constant(item));
                var lambda = Expression.Lambda<Func<TSource, bool>>(bd, keySelector.Parameters);
                result.Add(source.FirstOrDefault(lambda.Compile()));
            }

            return result;
        }
    }
}
