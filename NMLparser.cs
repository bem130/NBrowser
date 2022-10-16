using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBrowser
{
    internal class NMLParser
    {
        public NMLParser()
        {
        }
        public List<NMLOBJ> parse(string t)
        {
            t = t.Replace("\r\n", "\n");
            int i = 0;
            string nstxt = "";
            List<NMLOBJ> ret = new List<NMLOBJ>();
            while (t.Length > i)
            {
                if ((i < 1 || t[i - 1] != '\\') && (t[i] == '#' || (t[i] == '{' && t[i + 1] == '{'))) // structure
                {
                    if (nstxt.Length > 0)
                    {
                        List<NMLOBJ> child = block(nstxt);
                        for (int ccn = 0; ccn < child.Count; ccn++)
                        {
                            ret.Add(child[ccn]);
                        }
                        nstxt = "";
                    }
                    if (t[i] == '#') // title
                    {
                        int size = -1;
                        i++;
                        if (t[i] == ' ') { i++; }
                        else
                        {
                            switch (t[i])
                            {
                                case '1':
                                    size = 1;
                                    break;
                                case '2':
                                    size = 2;
                                    break;
                                case '3':
                                    size = 2;
                                    break;
                                case '4':
                                    size = 2;
                                    break;
                            }
                            i++;
                            if (t[i] == ' ') { i++; }
                        }
                        string content = "";
                        while (t.Length > i && t[i] != '\n')
                        {
                            content += t[i];
                            i++;
                        }
                        if (size == -1)
                        {
                            ret.Add(new NMLOBJ("dtitle", 0, "", content, ""));
                        }
                        else
                        {
                            ret.Add(new NMLOBJ("title", size, "", content, ""));
                        }
                    }
                    else if (t[i] == '{' && t[i + 1] == '{') // block
                    {
                        i += 2;
                        string type = "";
                        while (t.Length > i && t[i] != '\n')
                        {
                            type += t[i];
                            i++;
                        }
                        i++;
                        string content = "";
                        while (t.Length > i)
                        {
                            if (t.Length>i+2&&t[i] == '\n' && t[i + 1] == '}' && t[i + 2] == '}') { i += 3; break; }
                            i++;
                            content += t[i - 1];
                            if (t.Length>i+3&&t[i] == '\n' && t[i + 1] == '\\' && t[i + 2] == '}' && t[i + 3] == '}') { i += 2; content += "\n"; }
                        }
                        if (type.StartsWith("embed:"))
                        {
                            ret.Add(new NMLOBJ("embed", 0, type.Substring(6), content, ""));
                        }
                        if (type.StartsWith("code:\n"))
                        {
                            ret.Add(new NMLOBJ("cblock", 0, "", content, ""));
                        }
                        else if (type.StartsWith("code:"))
                        {
                            ret.Add(new NMLOBJ("cblock", 0, type.Substring(5), content, ""));
                        }
                    }
                }
                else
                {
                    i++;
                    if (t[i - 1] == '\\' && (t[i] == '#' || (t[i] == '{' && t[i + 1] == '{')))
                    {
                        i++;
                    }
                    i--;
                    if (!(t[i] == '#' || (t[i] == '{' && t[i + 1] == '{')))
                    {
                        nstxt += t[i];
                    }
                    i++;
                }
            }
            if (nstxt.Length > 0)
            {
                List<NMLOBJ> child = block(nstxt);
                for (int ccn = 0; ccn < child.Count; ccn++)
                {
                    ret.Add(child[ccn]);
                }
                nstxt = "";
            }
            return ret;
        }
        List<NMLOBJ> block(string t)
        {
            if (t[0] == '\n')
            {
                t = t.Remove(0, 1);
            }
            if (t.Length > 1 && t[t.Length - 1] == '\n')
            {
                t = t.Remove(t.Length - 1);
            }
            int i = 0;
            string nstxt = "";
            string tag = "";
            List<NMLOBJ> cblk = new List<NMLOBJ>();
            while (t.Length > i)
            {
                if (t[i]=='{') // structure
                {
                    if (nstxt.Length>0)
                    {
                        cblk.Add(new NMLOBJ("text", 0, nstxt, "", ""));
                        nstxt = "";
                    }
                    i++;
                    while (t.Length>i)
                    {
                        if (t[i]==':')
                        {
                            break;
                        }
                        tag += t[i];
                        i++;
                    }
                    List<string> child = new List<string> { };
                    string ctxt = "";
                    i++;
                    while (t.Length>i)
                    {
                        if (t[i]=='}')
                        {
                            child.Add(ctxt);
                            cblk.Add(new NMLOBJ(tag, 0, nstxt, string.Join(';',child), ""));
                        }
                        else if (t[i]==';')
                        {
                            child.Add(ctxt);
                            i++;
                            ctxt = "";
                        }
                        ctxt += t[i];
                        i++;
                    }
                }
                else
                {
                    nstxt += t[i];
                }
                tag = "";
                i++;
            }
            if (nstxt.Length > 0)
            {
                cblk.Add(new NMLOBJ("text", 0, nstxt, "", ""));
                nstxt = "";
            }
            return cblk;
        }
    }
    class NMLOBJ
    {
        public string type { get; set; }
        public string text { get; set; }
        public string content { get; set; }
        public string alias { get; set; }
        public int size { get; set; }
        public NMLOBJ(string type, int size = 0, string text = "", string content = "", string alias = "")
        {
            this.type = type;
            this.size = size;
            this.text = text;
            this.content = content;
            this.alias = alias;
        }
    }
}
