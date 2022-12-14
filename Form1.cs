using System.Drawing;
using System.Windows.Forms;

namespace NBrowser
{
    public partial class Form1 : Form
    {
        string wintitle;
        string path;
        string nowdoc;
        List<NMLOBJ> NMLobj;
        List<HTMLOBJ> HTMLobj;
        HttpClient client;
        nBbi nbbi;
        string beforepath;
        string mode;
        ScrollableControl sbody;
        Size size;
        int top;
        int left;
        public Form1()
        {
            InitializeComponent();
            Opacity = 0.9;
            wintitle = this.Text;
            client = new HttpClient();
            nbbi = new nBbi();
            mode = "NML";
            setPath("nbrowser://home");
            setPath("nbrowser://home");
            textBox1.Text = "nbrowser://home";
            getData();
            comboBox1.SelectedIndex = 1;
        }
        public void setPath(string newpath)
        {
            textBox1.Text = newpath;
            beforepath = path;
            path = newpath;
        }
        public void Reload()
        {
        }
        async void getData()
        {
            string res = "not found";
            if (path.StartsWith("http://") || path.StartsWith("https://"))
            {
                var getReult = await client.GetAsync(path);
                res = await getReult.Content.ReadAsStringAsync();
            }
            else if (path.StartsWith("nbrowser"))
            {
                res = nbbi.get(path);
            }
            nowdoc = res;
            showData(res);
        }
        void settitle(string title)
        {
            if (title.Length>0)
            {
                this.Text = wintitle + " - " + title;
            }
            else
            {
                this.Text = wintitle;
            }
            return;
        }
        void showData(string res)
        {
            Clear();
            switch (mode)
            {
                case "NML":
                    NMLpage(res);
                    break;
                case "HTML":
                    HTMLpage(res);
                    break;
                case "Text":
                    TXTpage(res);
                    break;
                default:
                    TXTpage(res);
                    break;
            }
        }
        void Clear()
        {
            pagebody.Controls.Clear();
        }
        // for TXT
        void TXTpage(string data)
        {
            NMLobj = new List<NMLOBJ>();
            {
                int i = 0;
                string t = data;
                string nstxt = "";
                while (t.Length > i)
                {
                    if (t[i] == '\n')
                    {
                        if (nstxt.Length > 0)
                        {
                            NMLobj.Add(new NMLOBJ("text", 0, nstxt, "", ""));
                        }
                        NMLobj.Add(new NMLOBJ("br", 0, "", "", ""));
                        nstxt = "";
                    }
                    else
                    {
                        nstxt += t[i];
                    }
                    i++;
                }
                if (nstxt.Length > 0)
                {
                    NMLobj.Add(new NMLOBJ("text", 0, nstxt, "", ""));
                    nstxt = "";
                }
            }
            TXTShow();
        }
        public void TXTShow()
        {
            settitle(path);
            sbody = new ScrollableControl();
            sbody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sbody.AutoScroll = true;
            var psize = pagebody.Size;
            sbody.Size = new Size(psize.Width - 2, psize.Height - 2);
            Label label;
            Button button;
            top = 0;
            left = 0;
            for (int i = 0; i < NMLobj.Count; i++)
            {
                NMLOBJ objItem = NMLobj[i];
                switch (objItem.type)
                {
                    case "text":
                        label = new Label();
                        label.Name = "text";
                        label.Text = objItem.text;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(left, top);
                        label.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        sbody.Controls.Add(label);
                        size = label.Size;
                        left += size.Width;
                        top += size.Height - 25;
                        break;
                    case "br":
                        left = 0;
                        top += 25;
                        break;
                }
                pagebody.Controls.Add(sbody);
            }
        }


        // for HTML

        void HTMLpage(string data)
        {
            HTMLParser HTMLparser = new HTMLParser();
            HTMLobj = HTMLparser.parse(data);
            HTMLShow();
        }
        public void HTMLShow()
        {
            settitle(path);
            sbody = new ScrollableControl();
            sbody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sbody.AutoScroll = true;
            var psize = pagebody.Size;
            sbody.Size = new Size(psize.Width - 2, psize.Height - 2);
            top = 0;
            left = 0;
            _htmlshow(HTMLobj);
        }
        void _htmlshow(List<HTMLOBJ> thtmlobj)
        {
            Label label;
            Button button;
            for (int i = 0; i < thtmlobj.Count; i++)
            {
                HTMLOBJ objItem = thtmlobj[i];
                switch (objItem.tag)
                {
                    case "text":
                        label = new Label();
                        label.Name = "text";
                        label.Text = objItem.textchild;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(left, top);
                        label.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        sbody.Controls.Add(label);
                        size = label.Size;
                        left += size.Width;
                        top += size.Height - 25;
                        left = 0;
                        top += 25;
                        break;
                    case "br":
                        left = 0;
                        top += 25;
                        break;
                    default:
                        _htmlshow(objItem.child);
                        break;
                }
                pagebody.Controls.Add(sbody);
            }
        }

        // for NML

        void NMLpage(string data)
        {
            NMLParser NMLparser = new NMLParser();
            NMLobj = NMLparser.parse(data);
            NMLobj.Add(new NMLOBJ("text"));
            NMLShow();
        }
        public void NMLShow()
        {
            sbody = new ScrollableControl();
            sbody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sbody.AutoScroll = true;
            var psize = pagebody.Size;
            sbody.Size = new Size(psize.Width - 2, psize.Height - 2);
            Label label;
            Button button;
            Size size;
            bool tflag = false;
            top = 0;
            left = 0;
            for (int i = 0; i < NMLobj.Count; i++)
            {
                NMLOBJ objItem = NMLobj[i];
                switch (objItem.type)
                {
                    case "text":
                        label = new Label();
                        label.Name = "text";
                        label.Text = objItem.text;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(30 + left, top);
                        label.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        sbody.Controls.Add(label);
                        size = label.Size;
                        left += size.Width;
                        top += size.Height - 25;
                        break;
                    case "br":
                        left = 0;
                        top += 25;
                        break;
                    case "url":
                        LinkLabel linklabel;
                        linklabel = new LinkLabel();
                        linklabel.Name = "text";
                        linklabel.Text = objItem.content;
                        linklabel.Tag = objItem;
                        linklabel.AutoSize = true;
                        linklabel.Location = new System.Drawing.Point(30 + left, top);
                        linklabel.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        linklabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._LinkClicked);
                        size = linklabel.Size;
                        left += size.Width;
                        sbody.Controls.Add(linklabel);
                        top += size.Height - 25;
                        break;
                    case "dtitle":
                        if (!tflag)
                        {
                            settitle(objItem.content);
                        }
                        tflag = true;
                        top += 30;
                        left = 0;
                        label = new Label();
                        label.TabIndex = 0;
                        label.Name = "text";
                        label.Text = objItem.content;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(3, top);
                        label.Font = new System.Drawing.Font("Meiryo UI", 20, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        sbody.Controls.Add(label);
                        top += 50;
                        break;
                    case "title":
                        top += 30;
                        left = 0;
                        label = new Label();
                        label.Name = "text";
                        label.TabIndex = 0;
                        label.Text = objItem.content;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(3, top);
                        label.Font = new System.Drawing.Font("Meiryo UI", 15, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        sbody.Controls.Add(label);
                        top += 40;
                        break;
                    case "cblock":
                        top += 30;
                        left = 0;
                        label = new Label();
                        label.Name = "text";
                        label.TabIndex = 0;
                        label.Text = objItem.content;
                        label.BackColor = System.Drawing.Color.Gainsboro;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(30, top);
                        label.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        sbody.Controls.Add(label);
                        top += label.Size.Height;
                        break;
                    case "image":
                        top += 30;
                        left = 0;
                        PictureBox picbox = new PictureBox();
                        picbox.ImageLocation = objItem.content;
                        picbox.SizeMode = PictureBoxSizeMode.StretchImage;
                        picbox.Location = new System.Drawing.Point(3, top);
                        Size pbs = picbox.ClientSize;
                        double aratio = pbs.Width / pbs.Height;
                        if (aratio>1)
                        {
                            picbox.Width = (int)(sbody.Height * 0.3 * aratio);
                            picbox.Height = (int)(sbody.Height * 0.3);
                            if (sbody.Height*0.3>300)
                            {
                                picbox.Width = (int)(300 * aratio);
                                picbox.Height = (int)(300);
                            }
                        }
                        else
                        {
                            picbox.Width = (int)(sbody.Width * 0.8);
                            picbox.Height = (int)(sbody.Width * 0.8 / aratio);
                            if (sbody.Width * 0.8 > 1000)
                            {
                                picbox.Width = (int)(1000 * aratio);
                                picbox.Height = (int)(1000);
                            }
                        }
                        sbody.Controls.Add(picbox);
                        top += picbox.Size.Height;
                        break;
                    case "embed":
                        switch (objItem.text)
                        {
                            case "table":
                                top += 30;
                                left = 0;
                                TableLayoutPanel tlp = new TableLayoutPanel();
                                tlp.Location = new System.Drawing.Point(3, top);
                                List<string> rows = new List<string> (objItem.content.Split('\n'));
                                int rcnt = 0;
                                int ccnt = 0;
                                foreach (string row in rows)
                                {
                                    List<string> cols = new List<string>(row.Split(','));
                                    int maxsize = 0;
                                    foreach (string col in cols)
                                    {
                                        label = new Label();
                                        label.Name = "text";
                                        label.Text = col;
                                        label.BackColor = System.Drawing.Color.Gainsboro;
                                        label.AutoSize = true;
                                        label.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                                        tlp.Controls.Add(label,ccnt,rcnt);
                                        ccnt++;
                                    }
                                    rcnt++;
                                }
                                sbody.Controls.Add(tlp);
                                break;
                        }
                        break;
                }
                pagebody.Controls.Add(sbody);
            }
        }

        private void _LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NMLOBJ linklabel = (NMLOBJ)((LinkLabel)sender).Tag;
            setPath(linklabel.content);
            getData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setPath(textBox1.Text);
            getData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            var newpath = beforepath;
            setPath(newpath);
            getData();
            ((Button)sender).Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setPath("nbrowser://home");
            getData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mode = ((ComboBox)sender).Text;
            showData(nowdoc);
        }
    }
}