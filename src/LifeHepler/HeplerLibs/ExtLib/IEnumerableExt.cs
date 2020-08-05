using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeplerLibs.ExtLib
{
    public static class IEnumerableExt
    {
        public static bool Ext_IsNullOrEmpty<T>(this IEnumerable<T> enumeration)
        {
            return enumeration == null || !enumeration.Any();
        }
    }
}
