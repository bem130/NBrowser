using System.Windows.Forms;

namespace NBrowser
{
    public partial class Form1 : Form
    {
        string path;
        List<NMLOBJ> obj;
        HttpClient client;
        nBbi nbbi;
        string beforepath;
        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
            nbbi = new nBbi();
            setPath("nbrowser://home");
            setPath("nbrowser://home");
            textBox1.Text = "nbrowser://home";
            getData();
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
            Clear();
            if (path.StartsWith("http://") || path.StartsWith("https://"))
            {
                var getReult = await client.GetAsync(path);
                var res = await getReult.Content.ReadAsStringAsync();
                NMLpage(res);
            }
            else if (path.StartsWith("nbrowser"))
            {
                string res = nbbi.get(path);
                NMLpage(res);
            }
        }
        void Clear()
        {
            pagebody.Controls.Clear();
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
    }
}