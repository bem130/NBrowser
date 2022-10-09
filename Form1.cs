using System.Windows.Forms;

namespace NBrowser
{
    public partial class Form1 : Form
    {
        string path;
        string nowdoc;
        List<NMLOBJ> obj;
        HttpClient client;
        nBbi nbbi;
        string beforepath;
        string mode;
        public Form1()
        {
            InitializeComponent();
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
        void showData(string res)
        {
            Clear();
            switch (mode)
            {
                case "NML":
                    NMLpage(res);
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
            obj = new List<NMLOBJ>();
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
                            obj.Add(new NMLOBJ("text", 0, nstxt, "", ""));
                        }
                        obj.Add(new NMLOBJ("br", 0, "", "", ""));
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
                    obj.Add(new NMLOBJ("text", 0, nstxt, "", ""));
                    nstxt = "";
                }
            }
            TXTShow();
        }
        public void TXTShow()
        {
            ScrollableControl sbody = new ScrollableControl();
            sbody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sbody.AutoScroll = true;
            var psize = pagebody.Size;
            sbody.Size = new Size(psize.Width - 2, psize.Height - 2);
            Label label;
            Button button;
            Size size;
            int top = 0;
            int left = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                NMLOBJ objItem = obj[i];
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


        // for NML

        void NMLpage(string data)
        {
            NMLParser NMLparser = new NMLParser();
            obj = NMLparser.parse(data);
            obj.Add(new NMLOBJ("text"));
            NMLShow();
        }
        public void NMLShow()
        {
            ScrollableControl sbody = new ScrollableControl();
            sbody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            sbody.AutoScroll = true;
            var psize = pagebody.Size;
            sbody.Size = new Size(psize.Width - 2, psize.Height - 2);
            Label label;
            Button button;
            Size size;
            int top = 0;
            int left = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                NMLOBJ objItem = obj[i];
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
                    case "link":
                        LinkLabel linklabel;
                        linklabel = new LinkLabel();
                        linklabel.Name = "text";
                        linklabel.Text = objItem.text;
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