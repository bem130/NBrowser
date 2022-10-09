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
                    return "# home\n[open](https://raw.githubusercontent.com/bem130/markup/master/readme.nml)\nhello\n[https://bem130.github.io/markup/editor]\n[https://bem130.github.io/markup/editor]";
                    break;
                default:
                    return "# not found";
                    break;
            }
        }
    }
}
