using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBrowser
{
    internal class nBbi
    {
        public nBbi()
        {
            return;
        }
        public string get(string path)
        {
            switch (path.Remove(0,11))
            {
                case "home":
                    return "# home\n\n#1 about neknaj markup\n[https://raw.githubusercontent.com/bem130/markup/master/readme.nml]";
                    break;
                default:
                    return "# not found";
                    break;
            }
        }
    }
}
