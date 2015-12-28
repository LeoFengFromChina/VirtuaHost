using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using XmlHelper;

namespace VirtualDualHost
{
    public partial class Form_Main : DockContent
    {
        public Form_Main()
        {
            InitializeComponent();
        }
        ////Form_NDCServer form_NDCServer;
        //Form_NDCServer form_NDCServer;
        //Form_NDCServer_2 form_NDCServer_2;
        //Form_DDCServer form_DDCServer;
        //Form_DualHost form_DualHost;
        private void Form_Main_Load(object sender, EventArgs e)
        {
            XDCUnity.CurrentPath = System.Environment.CurrentDirectory;
            XDCUnity.UserInfoPath = XDCUnity.CurrentPath + XDCUnity.UserInfoPath;
            //NDCserver
            //form_NDCServer = new Form_NDCServer();
            this.Text += XDCUnity.Version;

            Form_NDCServer form_NDCServer1 = new Form_NDCServer();
            form_NDCServer1.Show(this.dockPanel1, DockState.Document);

            //DDCserver
            Form_DDCServer form_DDCServer1 = new Form_DDCServer();
            form_DDCServer1.Show(this.dockPanel1, DockState.Document);

            //双主机
            Form_DualHost form_DualHost = new Form_DualHost();
            form_DualHost.Show(this.dockPanel1, DockState.Document);

            form_NDCServer1.Activate();

            dDCServerToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            nDCServerToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            virtualDualHostToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            parseToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            screenParseToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            errorCodeToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            eCATToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            aboutToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            nDCServerToolStripMenuItem1.Click += DDCServerToolStripMenuItem_Click;
            dDCServerToolStripMenuItem1.Click += DDCServerToolStripMenuItem_Click;
            killeCATToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            starteCATToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            eCATConfigToolToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            exitToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            openeCATToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            openXDCHostToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            openTrueBackToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            CheckeCATPath();

        }

        bool isAlreadyNDC_1 = false;
        bool isAlreadyNDC_2 = false;
        bool isAlareadyDualHost = false;
        bool isAlreadyDDC_1 = false;
        bool isAlreadyDDC_2 = false;
        private void DDCServerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem tmi = sender as ToolStripMenuItem;
            switch (tmi.Text)
            {
                case "DDCServer":
                case "DDCServer_2":
                case "NDCServer":
                case "NDCServer_2":
                case "VirtualDualHost":
                    {
                        isAlreadyNDC_1 = false;
                        isAlreadyNDC_2 = false;
                        isAlareadyDualHost = false;
                        isAlreadyDDC_1 = false;
                        isAlreadyDDC_2 = false;
                        foreach (DockContent dockContent in dockPanel1.Contents)
                        {
                            if (dockContent.Name.Equals("Form_NDCServer"))
                                isAlreadyNDC_1 = true;
                            else if (dockContent.Name.Equals("Form_NDCServer_2"))
                                isAlreadyNDC_2 = true;
                            else if (dockContent.Name.Equals("Form_DDCServer"))
                                isAlreadyDDC_1 = true;
                            else if (dockContent.Name.Equals("Form_DDCServe2"))
                                isAlreadyDDC_2 = true;
                            else if (dockContent.Name.Equals("Form_DualHost"))
                                isAlareadyDualHost = true;
                        }

                        if (tmi.Text == "NDCServer_2" && !isAlreadyNDC_2)
                        {
                            Form_NDCServer_2 form_NDCServer2 = new Form_NDCServer_2();
                            form_NDCServer2.Show(this.dockPanel1, DockState.Document);
                        }
                        else if (tmi.Text == "NDCServer" && !isAlreadyNDC_1)
                        {
                            Form_NDCServer form_NDCServer1 = new Form_NDCServer();
                            form_NDCServer1.Show(this.dockPanel1, DockState.Document);
                        }
                        else if (tmi.Text == "DDCServer" && !isAlreadyDDC_1)
                        {
                            Form_DDCServer form_DDCServer1 = new Form_DDCServer();
                            form_DDCServer1.Show(this.dockPanel1, DockState.Document);
                        }
                        else if (tmi.Text == "DDCServer_2" && !isAlreadyDDC_2)
                        {
                            MessageBox.Show("Comming Soon...");
                        }
                        else if (tmi.Text == "VirtualDualHost" && !isAlareadyDualHost)
                        {
                            Form_DualHost form_DualHost = new Form_DualHost();
                            form_DualHost.Show(this.dockPanel1, DockState.Document);
                        }
                    }
                    break;
                case "SuperParse":
                    {
                        Form_Pars form_Pars = new Form_Pars();
                        form_Pars.Show();
                    }
                    break;
                case "ScreenParse":
                    {
                        Form_ScreenParse form_screenPars = new Form_ScreenParse();
                        form_screenPars.Show();
                    }
                    break;
                case "eCAT":
                    {
                        From_Seeting_eCATPath form_eCAT = new From_Seeting_eCATPath();
                        form_eCAT.Show();
                    }
                    break;
                case "KilleCAT":
                    {
                        KilleCATFunc();
                    }
                    break;
                case "StarteCAT":
                    {
                        StarteCATFunc();
                    }
                    break;
                case "eCATConfigTool":
                    {
                        System.Threading.Thread eCATToolThread = new System.Threading.Thread(eCATConfigToolFunc);
                        eCATToolThread.IsBackground = true;
                        eCATToolThread.Start();
                        //eCATConfigToolFunc();
                    }
                    break;
                case "Open-eCAT":
                    {
                        //打开eCAT路径
                        XDCUnity.OpenPath(XDCUnity.eCATPath);
                    }
                    break;
                case "Open-XDCHost":
                    {
                        //打开当前主机路径
                        XDCUnity.OpenPath(Environment.CurrentDirectory);
                    }
                    break;
                case "Open-TrueBack":
                    {
                        //打开当前主机路径
                        CheckTrueBackPath();
                        string truebackPath = XDCUnity.TrueBackPath;
                        truebackPath = truebackPath.Substring(0, truebackPath.LastIndexOf('\\'));
                        XDCUnity.OpenPath(truebackPath);
                    }
                    break;
                case "ErrorCode":
                    {
                        Form_ErrorCodeSearch form_errorcode = new Form_ErrorCodeSearch();
                        form_errorcode.Show();
                    }
                    break;
                case "About":
                    {
                        new Form_About().ShowDialog();
                    }
                    break;
                case "Exit":
                    {
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 启动eCAT
        /// </summary>
        private void StarteCATFunc()
        {
            #region start
            CheckeCATPath();

            CheckTrueBackPath();

            XDCUnity.StarteCAT();

            #endregion
        }

        /// <summary>
        /// 关闭eCAT
        /// </summary>
        private void KilleCATFunc()
        {
            CheckeCATPath();
            XDCUnity.KilleCAT();
        }

        /// <summary>
        /// 启动eCAT-XDC配置
        /// </summary>
        private static void eCATConfigToolFunc()
        {
            #region eCATConfigTool

            //检查eCAT路径
            CheckeCATPath();

            string path = XDCUnity.eCATPath + @"\eCATConfigTool";
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {

                }
            }
            path = XDCUnity.eCATPath + @"\eCATConfigTool.exe";
            XDCUnity.ProcessFile(path);
            #endregion
        }

        /// <summary>
        /// 检查eCAT路径是否有配置
        /// </summary>
        private static void CheckeCATPath()
        {
            if (string.IsNullOrEmpty(XDCUnity.eCATPath))
            {
                XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
                XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");
                if (string.IsNullOrEmpty(node.Attributes["value"].InnerText))
                    MessageBox.Show("Please Set eCAT Path First in [Config--eCAT].");
                else
                    XDCUnity.eCATPath = node.Attributes["value"].InnerText;
            }
        }

        private static void CheckTrueBackPath()
        {
            if (string.IsNullOrEmpty(XDCUnity.TrueBackPath))
            {
                XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
                XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/TrueBackPath");
                if (string.IsNullOrEmpty(node.Attributes["value"].InnerText))
                    MessageBox.Show("Please Set eCAT Path First in [Config--eCAT].");
                else
                    XDCUnity.TrueBackPath = node.Attributes["value"].InnerText;
            }
        }

    }
}
