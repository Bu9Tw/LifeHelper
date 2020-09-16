using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeplerLibs.Paging
{
    public static class PagingExtension
    {
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> source, int curPageNumber, int pageRow)
        {
            return source.Skip((curPageNumber - 1) * pageRow).Take(pageRow);
        }
    }
}
