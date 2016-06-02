using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using XmlHelper;

namespace VirtualDualHost
{
    public partial class Form_ManuSendDataMain : Form
    {

        public Form_ManuSendDataMain(XDCProtocolType curProtocol= XDCProtocolType.NDC)
        {
            InitializeComponent();
            currentProtocolType = curProtocol;
        }
        XDCProtocolType currentProtocolType;
        public delegate void SubForm(object dataContent);
        public event SubForm SubFormEvent;

        public delegate void ParentFormDelegate(object path, DataType dataType);
        public event ParentFormDelegate ParentFormEvent;
        Form_MsgDebug form_MsgDebug;
        Form_ManuSendLeft form_manuLeft;
        private void Form_Pars_Load(object sender, EventArgs e)
        {
            ParentFormEvent += Form_Pars_ParentFormEvent;
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");

            XDCUnity.eCATPath = node.Attributes["value"].InnerText;

            form_manuLeft = new Form_ManuSendLeft();
            form_manuLeft.SubFormEvent += Form_Left_SubFormEvent;
            form_manuLeft.Show(this.dockPanel1, DockState.DockLeft);


            //正文内容
            form_MsgDebug = new Form_MsgDebug("", currentProtocolType);
            form_MsgDebug.SubFormEvent += Form_MsgDebug_SubFormEvent;
            form_MsgDebug.SubFormCloseEvent += Form_MsgDebug_SubFormCloseEvent;
            form_MsgDebug.Show(this.dockPanel1, DockState.Document);
        }

        private void Form_MsgDebug_SubFormCloseEvent()
        {
            this.Close();
        }

        private void Form_MsgDebug_SubFormEvent(object dataContent)
        {
            //子窗体的事件触发，引起本父窗体的触发。在Server主窗体中同样也捕获了本事件。
            SubFormEvent(dataContent);
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
            form_MsgDebug.Dispose();
            form_manuLeft.Dispose();
            this.Dispose();
        }
    }
}
