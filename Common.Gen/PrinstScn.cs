using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public static class PrinstScn
    {


        public static void WriteLine(string format, params object[] args)
        {
            var separator = string.Empty;

            Console.WriteLine(separator);
            Console.WriteLine(separator);

            Console.WriteLine(format, args);

            Console.WriteLine(separator);
            Console.WriteLine(separator);

        }

    }
}
