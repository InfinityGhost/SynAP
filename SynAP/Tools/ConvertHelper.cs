using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SynAP.Tools
{
    static class ConvertHelper
    {
        public static double GetDouble(this TextBox box)
        {
            return SafeDoubleParse(box.Text);
        }

        public static double SafeDoubleParse(object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }
    }
}
