using StandardFeature;
using System;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using XmlHelper;

namespace VirtualDualHost
{
    public partial class Form_Pars : Form
    {
        public Form_Pars()
        {
            InitializeComponent();
        }

        public delegate void ParentFormDelegate(object path, DataType dataType);
        public event ParentFormDelegate ParentFormEvent;
        Form_MsgDebug form_MsgDebug;
        Form_COMviewList form_COMviewList;
        Form_ParsLeft form_Left;
        private void Form_Pars_Load(object sender, EventArgs e)
        {
            ParentFormEvent += Form_Pars_ParentFormEvent;
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");

            XDCUnity.eCATPath = node.Attributes["value"].InnerText;

            //左侧工具
            form_COMviewList = new Form_COMviewList();
            form_COMviewList.SubFormEvent += Form_COMviewList_SubFormEvent;
            form_COMviewList.Show(this.dockPanel1, DockState.DockLeft);

            form_Left = new Form_ParsLeft();
            form_Left.SubFormEvent += Form_Left_SubFormEvent;
            form_Left.Show(this.dockPanel1, DockState.DockLeft);


            //正文内容
            form_MsgDebug = new Form_MsgDebug("", XDCProtocolType.NDC);
            form_MsgDebug.Show(this.dockPanel1, DockState.Document);
        }

        private void Form_COMviewList_SubFormEvent(object dataContext, XDCProtocolType protocolType, DataType dataType)
        {
            form_MsgDebug.ParsFromSubForm(dataContext.ToString(), protocolType, dataType, "");
        }

        private void Form_Pars_ParentFormEvent(object path, DataType dataType)
        {
            ParentFormEvent(path, dataType);
        }

        private void Form_Left_SubFormEvent(object subControl, XDCProtocolType protocolType, DataType dataType)
        {
            if (dataType == DataType.Message)
            {
                //双击消息的时候，选中另外一个
                form_COMviewList.Activate();
                form_COMviewList.ParsMessage(subControl.ToString());
            }
            else
            {
                string text = XDCUnity.GetTxtFileText(subControl.ToString());
                //双击树节点，将数据传至主窗体并格式化显示
                if (dataType == DataType.Screen)
                {
                    Form_ScreenParse form_ScreenParse = new Form_ScreenParse(text, protocolType, subControl.ToString());
                    form_ScreenParse.Show();
                }
                else
                {
                    form_MsgDebug.ParsFromSubForm(text, protocolType, dataType, subControl.ToString());
                }
            }
        }

        private void Form_Pars_FormClosing(object sender, FormClosingEventArgs e)
        {
            form_COMviewList.Dispose();
            form_MsgDebug.Dispose();
            form_Left.Dispose();
            this.Dispose();
        }
    }
}
