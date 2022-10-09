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
                    return "# home\n\n#1 about neknaj markup\n[https://raw.githubusercontent.com/bem130/markup/master/readme.nml]\n\n\n#1 welcome page\n[welcome-page](nbrowser://welcome)";
                    break;
                case "welcome":
                    return "# welcome\nthis is a web browser for Neknaj Markup Languages";
                    break;
                default:
                    return "# not found";
                    break;
            }
        }
    }
}
