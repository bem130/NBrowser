using System.Net.Http;
using System.Reflection;
using System.Text;

namespace NBrowser
{
    public partial class Form1 : Form
    {
        string path;
        List<NMLOBJ> obj;
        HttpClient client;
        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
            setPath("https://raw.githubusercontent.com/bem130/markup/master/readme.nml");
            getData();
        }
        public void setPath(string newpath)
        {
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
        }
        void Clear()
        {
        }

        // for NML

        void NMLpage(string data)
        {
            NMLParser NMLparser = new NMLParser();
            obj = NMLparser.parse(data);
            NMLShow();
        }
        public void NMLShow()
        {
            FlowLayoutPanel nmlb = new FlowLayoutPanel();
            nmlb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            nmlb.Size = new System.Drawing.Size(1138, 1203);

            Label label;
            Button button;
            for (int i = 0; i < obj.Count; i++)
            {
                NMLOBJ objItem = obj[i];
                switch (objItem.type)
                {
                    case "text":
                        label = new Label();
                        label.Name = "text";
                        label.Text = objItem.text;
                        label.Size = new System.Drawing.Size(1000, 30);
                        label.Font = new System.Drawing.Font("Meiryo UI", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        nmlb.Controls.Add(label);
                        break;
                    case "dtitle":
                        label = new Label();
                        label.Name = "text";
                        label.Text = objItem.content;
                        label.Size = new System.Drawing.Size(1000, 50);
                        label.Font = new System.Drawing.Font("Meiryo UI", 20, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        nmlb.Controls.Add(label);
                        break;
                    case "title":
                        label = new Label();
                        label.Name = "text";
                        label.Text = objItem.content;
                        label.Size = new System.Drawing.Size(1000, 40);
                        label.Font = new System.Drawing.Font("Meiryo UI", 15, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                        nmlb.Controls.Add(label);
                        break;
                }
            }
            pagebody.Controls.Add(nmlb);
        }

    }
}