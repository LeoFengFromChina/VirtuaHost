using StandardFeature;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using XmlHelper;
using System.Collections.Generic;

namespace VirtualDualHost
{
    public partial class Form_Main : DockContent
    {
        public Form_Main()
        {
            InitializeComponent();
        }
        Form_NDCServer_2 form_NDCServer2 = new Form_NDCServer_2();
        Form_DDCServer form_DDCServer1 = new Form_DDCServer();
        Form_NDCServer form_NDCServer1 = new Form_NDCServer();
        Form_Managerment form_managerMentMain = new Form_Managerment();
        private void Form_Main_Load(object sender, EventArgs e)
        {

            XDCUnity.CurrentPath = System.Environment.CurrentDirectory;
            XDCUnity.UserInfoPath = XDCUnity.CurrentPath + XDCUnity.UserInfoPath;
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");

            XDCUnity.eCATPath = node.Attributes["value"].InnerText;
            //NDCserver
            //form_NDCServer = new Form_NDCServer();
            this.Text += XDCUnity.Version;

            form_NDCServer1.Show(this.dockPanel1, DockState.Document);


            //NDC Host 2            
            form_NDCServer2.Show(this.dockPanel1, DockState.Document);

            //DDCserver
            form_DDCServer1.Show(this.dockPanel1, DockState.Document);

            //ManagerMent
            form_managerMentMain.Show(this.dockPanel1, DockState.Document);

            form_NDCServer1.Activate();

            Dictionary<string, string> extensionTools = new Dictionary<string, string>();

            extensionTools = GetExtensionTool();
            if (extensionTools != null)
            {
                foreach (KeyValuePair<string, string> kp in extensionTools)
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(kp.Key);
                    tsmi.ToolTipText = kp.Value;
                    tsmi.Click += Form_Main_Click;
                    tsddb_Extension.DropDownItems.Add(tsmi);
                }
            }
            aTMToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            dDCServerToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            nDCServerToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
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
            InteractiveBufferMenuItem.Click += DDCServerToolStripMenuItem_Click;
            importLogsToAutoReplyToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            c09ToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;
            CheckeCATPath();
            GetCurrentFormatCode();
        }

        private void Form_Main_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            string path = ((ToolStripMenuItem)sender).ToolTipText;
            string currentPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            currentPath = currentPath.Substring(0, currentPath.LastIndexOf('\\'));
            if (!string.IsNullOrEmpty(path)
                && path.Length > 2
                && path[1].ToString().Equals(":"))
            {
                //路径是类似c:\dd\..\..这样的绝对路径
                XDCUnity.OpenPath(path);
            }
            else
            {
                //相对路径
                XDCUnity.OpenPath(currentPath + path);
            }
        }

        private void Tsi_Click(object sender, EventArgs e)
        {

        }

        bool isAlreadyNDC_1 = false;
        bool isAlreadyNDC_2 = false;
        bool isAlareadyDualHost = false;
        bool isAlreadyDDC_1 = false;
        bool isAlreadyDDC_2 = false;
        bool isATMC = false;
        private void DDCServerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem tmi = sender as ToolStripMenuItem;
            switch (tmi.Text)
            {
                case "ATMC":
                case "DDCServer":
                case "DDCServer-2":
                case "NDCServer":
                case "NDCServer-2":
                case "VirtualDualHost":
                    {
                        isAlreadyNDC_1 = false;
                        isAlreadyNDC_2 = false;
                        isAlareadyDualHost = false;
                        isAlreadyDDC_1 = false;
                        isAlreadyDDC_2 = false;
                        isATMC = false;
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
                            else if (dockContent.Name.Equals("Form_Managerment"))
                                isATMC = true;
                            if (tmi.Text.Equals(dockContent.Text))
                            {
                                dockContent.Select();
                                break;
                            }

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
                        else if (tmi.Text == "ATMC" && !isATMC)
                        {
                            form_managerMentMain.Show(this.dockPanel1, DockState.Document);
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
                case "InteractiveBuffer":
                    {
                        new Form_GetInteractiveMsgTextToShow("").Show();
                    }
                    break;
                case "About":
                    {
                        new Form_About().ShowDialog();
                    }
                    break;
                case "ImportLogsToAutoReply":
                    {
                        new Form_ImportLogs().Show();
                    }
                    break;
                case "Exit":
                    {
                        this.Close();
                    }
                    break;
                case "C09":
                    {
                        new Form_C09().Show();
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
        /// <summary>
        /// 扩展工具
        /// </summary>
        private static Dictionary<string, string> GetExtensionTool()
        {
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/Extension");
            if (node.HasChildNodes)
            {

                Dictionary<string, string> extensionTools = new Dictionary<string, string>();
                foreach (XmlNode item in node.ChildNodes)
                {
                    extensionTools.Add(item.Attributes["name"].InnerText, item.Attributes["value"].InnerText);
                }
                return extensionTools;
            }
            return null;
        }
        private static void GetCurrentFormatCode()
        {
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/FormatCode");
            string currentCode = node.Attributes["value"].InnerText;
            if (string.IsNullOrEmpty(currentCode))
            {
                XDCUnity.CurrentFormatCode = "ASCII";
            }
            else
            {
                XDCUnity.CurrentFormatCode = currentCode;
            }
        }
    }
}
