using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAP.Tools
{
    static class ReadHelper
    {
        public static string GetProperty(this IEnumerable<string> vs, string propertyName, string splitter = ":")
        {
            string fullLine = vs.Where(e => e.Contains(propertyName)).First();
            return fullLine.Replace(propertyName + splitter, string.Empty);
        }

        public static bool ToBool(this string value)
        {
            switch(value.ToLower())
            {
                case "false":
                case "0":
                    return false;
                case "true":
                case "1":
                    return true;
                default:
                    throw new InvalidCastException();
            }
        }
    }
}
