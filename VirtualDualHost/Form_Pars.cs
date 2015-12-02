﻿using StandardFeature;
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
        private void Form_Pars_Load(object sender, EventArgs e)
        {
            ParentFormEvent += Form_Pars_ParentFormEvent;
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");

            XDCUnity.eCATPath = node.Attributes["value"].InnerText;
            //左侧工具
            Form_ParsLeft form_Left = new Form_ParsLeft();
            form_Left.SubFormEvent += Form_Left_SubFormEvent;
            form_Left.Show(this.dockPanel1, DockState.DockLeftAutoHide);

            form_COMviewList = new Form_COMviewList();
            form_COMviewList.SubFormEvent += Form_COMviewList_SubFormEvent;
            form_COMviewList.Show(this.dockPanel1, DockState.DockLeftAutoHide);


            //正文内容
            form_MsgDebug = new Form_MsgDebug("");
            form_MsgDebug.Show(this.dockPanel1, DockState.Document);
        }

        private void Form_COMviewList_SubFormEvent(object dataContext, XDCProtocolType protocolType, DataType dataType)
        {
            form_MsgDebug.ParsFromSubForm(dataContext.ToString(), protocolType, dataType);
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
                //双击树节点，将数据传至主窗体并格式化显示
                string text = XDCUnity.GetTxtFileText(subControl.ToString());
                form_MsgDebug.ParsFromSubForm(text, protocolType, dataType);
            }
        }

    }
}
