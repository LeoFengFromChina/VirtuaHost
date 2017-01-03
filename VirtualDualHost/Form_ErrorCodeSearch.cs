using StandardFeature;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace VirtualDualHost
{
    public partial class Form_ErrorCodeSearch : Form
    {
        public Form_ErrorCodeSearch()
        {
            InitializeComponent();
        }


        string logViewPaht = @"C:\Program Files\GrgBanking\GRGXFSSP\LogView\OAnalysisLogSP.ini";
        string eCATErrorCodeEN = XDCUnity.eCATPath + @"\Resource\Common\Text\EN\ErrorCode.xml";
        string eCATErrorCodeCN = XDCUnity.eCATPath + @"\Resource\Common\Text\CN\ErrorCode.xml";
        XmlDocument xmlDocCN = null;
        XmlDocument xmlDocEN = null;
        private void btn_Search_Click(object sender, EventArgs e)
        {
            GetSPlogViewCode();
            GeteCATCode();
        }

        /// <summary>
        /// 获取SP的错误吗
        /// </summary>
        private void GetSPlogViewCode()
        {
            if (!File.Exists(logViewPaht))
            {
                //MessageBox.Show("Can not find SP path.");
                return;
            }
            string cnText_sp = XDCUnity.ReadIniData("DevFail", txt_Code.Text.Trim(), "", logViewPaht);
            string enText_sp = XDCUnity.ReadIniData("DevFail_ENG", txt_Code.Text.Trim(), "", logViewPaht);

            lbl_SP_CN.Text = cnText_sp;
            lbl_SP_EN.Text = enText_sp;
        }

        private void GeteCATCode()
        {
            string cnText_eCAT = string.Empty;
            string enText_eCAT = string.Empty;

            if (File.Exists(eCATErrorCodeEN))
            {
                if (!File.Exists(eCATErrorCodeEN))
                    return;
                if (null == xmlDocEN)
                { //初始化一个xml实例
                    xmlDocEN = new XmlDocument();
                    //导入指定xml文件
                    xmlDocEN.Load(eCATErrorCodeEN);
                }
                XmlNode root = xmlDocEN.SelectSingleNode("/Config");
                foreach (XmlNode subNode in root)
                {
                    if (subNode.NodeType == XmlNodeType.Element && subNode.Attributes["key"].Value.ToString() == txt_Code.Text.Trim())
                    {
                        enText_eCAT = subNode.Attributes["value"].Value.ToString();
                        break;
                    }
                }
            }

            if (File.Exists(eCATErrorCodeCN))
            {
                if (!File.Exists(eCATErrorCodeCN))
                    return;
                //初始化一个xml实例
                if (xmlDocCN == null)
                {
                    xmlDocCN = new XmlDocument();
                    //导入指定xml文件
                    xmlDocCN.Load(eCATErrorCodeCN);
                }
                XmlNode root = xmlDocCN.SelectSingleNode("/Config");
                foreach (XmlNode subNode in root)
                {
                    if (subNode.NodeType == XmlNodeType.Element && subNode.Attributes["key"].Value.ToString() == txt_Code.Text.Trim())
                    {
                        cnText_eCAT = subNode.Attributes["value"].Value.ToString();
                        break;
                    }
                }
            }

            lbl_eCAT_CN.Text = cnText_eCAT;
            lbl_eCAT_EN.Text = enText_eCAT;
        }

        private void Form_ErrorCodeSearch_Load(object sender, EventArgs e)
        {
            btn_Next.Click += Btn_Next_Click;
            btn_Pre.Click += Btn_Next_Click;
        }

        private void Btn_Next_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Code.Text.Trim()))
                return;

            int code;
            if (!int.TryParse(txt_Code.Text.Trim(), out code))
                return;

            Button btn = (Button)sender;

            if (btn.Name == "btn_Next")
            {
                if (System.Math.Abs(code) == code)
                    txt_Code.Text = (code + 1).ToString();
                else
                {
                    txt_Code.Text = "-" + (System.Math.Abs(code) + 1).ToString();
                }

            }
            else
            {
                if (System.Math.Abs(code) == code)
                    txt_Code.Text = (code - 1).ToString();
                else
                {
                    txt_Code.Text = "-" + (System.Math.Abs(code) - 1).ToString();
                }
            }
            GetSPlogViewCode();
            GeteCATCode();
        }
    }
}
