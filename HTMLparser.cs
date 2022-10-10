using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBrowser
{
    internal class HTMLParser
    {
        public HTMLParser()
        {
        }
        public List<HTMLOBJ> parse(string t)
        {
            List<HTMLOBJ> ret = block(t);
            return ret;
        }
        List<HTMLOBJ> block(string t)
        {
            int i = 0;
            List<HTMLOBJ> cblk = new List<HTMLOBJ>();
            string tag = "";
            string att = "";
            string eatt = "";
            string child = "";
            bool tfflag = false;
            while (t.Length>i)
            {
                
                if (t[i]=='<')
                {
                    tfflag = true;
                    i++;
                    while (!(t[i] == '>' || t[i]==' ')&&t.Length>i)
                    {
                        tag += t[i];
                        i++;
                    }
                    if (t[i]==' ') { i++; }
                    while (t[i]!='>'&&t.Length>i)
                    {
                        att += t[i];
                        i++;
                    }
                    i++;
                    int ocnt = 0;
                    while (t.Length>i)
                    {
                        if (t[i] == '<' && t[i+1]=='/')
                        {
                            if (ocnt==0) { i++;break; }
                            child += "</";
                            i++;
                            ocnt--;
                        }
                        else if (t[i]=='<') { ocnt++; }
                        child += t[i];
                        i++;
                    }
                    i += 2;
                    while (t.Length > i&&t[i]!='>')
                    {
                        eatt += t[i];
                        i++;
                    }
                    if (tag != "script" && tag != "style")
                    {
                        cblk.Add(new HTMLOBJ(tag, att,"", block(child)));
                    }
                    tag = "";
                    att = "";
                    eatt = "";
                    child = "";
                }
                i++;
            }
            if (tfflag) { return cblk; }
            else { return new List<HTMLOBJ> { new HTMLOBJ("text", "", t, null)}; }
        }
    }
    class HTMLOBJ
    {
        public string tag { get; set; }
        public string att { get; set; }
        public string textchild { get; set; }
        public List<HTMLOBJ>? child { get; set; }
        public HTMLOBJ(string tag, string att,string textchild, List<HTMLOBJ>? child = null)
        {
            this.tag = tag;
            this.att = att;
            this.child = child;
            this.textchild = textchild;
        }
    }
}
