using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeTracker
{
    public static class Util
    {
        public static Random rand = new Random();

        public static string RandomlyCapitalize(this string src)
        {
            StringBuilder build = new StringBuilder();

            foreach (char c in src)
            {
                bool doCaps = rand.Next(0, 2) == 0;
                build.Append((doCaps ? Char.ToUpper(c) : Char.ToLower(c)));
            }

            return build.ToString();
        }
    }
}
