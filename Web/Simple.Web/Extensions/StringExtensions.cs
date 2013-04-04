using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simple.Web
{
    public static class StringExtensions
    {
        public static string FromSafe64(this string value)
        {
            return value.Replace("-", "+").Replace("_", @"/");
        }
    }
}