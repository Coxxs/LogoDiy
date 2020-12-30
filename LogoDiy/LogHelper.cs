using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LogoDiy
{
    class LogHelper
    {
        public static void Info(string info)
        {
            Console.WriteLine(info);
            Debug.WriteLine(info);
        }

        public static void Error(string info)
        {
            Info(info);
        }

        public static void Error(Exception ex)
        {
            Info(ex.ToString());
        }
    }
}
