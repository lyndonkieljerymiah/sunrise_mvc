using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Helpers.Extension
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string stringHelper)
        {   
            StringBuilder sb = new StringBuilder();
            string firstLetter = stringHelper.Substring(0, 1);
            string rest = stringHelper.Substring(1, stringHelper.Length-1);
            sb.Append(firstLetter.ToLower() + rest);
            return sb.ToString().Substring(0, sb.ToString().Length);
        }
    }
}
