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
using WeifenLuo.WinFormsUI.Docking;

namespace VirtualDualHost
{
    public partial class Form_ParsLeft : DockContent
    {
        public Form_ParsLeft()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        Form_Pars form_Pars;
        private DockPanel dp;

        public static TreeNode Root = new TreeNode();
        public Form_ParsLeft(Form_Pars formMain)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            form_Pars = formMain;
            dp = (DockPanel)formMain.Controls["dockPanel1"];
        }
        public delegate void SubForm(object subControl, XDCProtocolType protocolType, DataType dataType);
        public event SubForm SubFormEvent;
        public delegate void BuildTree(object socket);
        public static event BuildTree BuildTreeEvent;
        System.Threading.Thread buildTreeThread;
        private void Form_ParsLeft_Load(object sender, EventArgs e)
        {
            BuildTreeEvent += Form_ParsLeft_BuildTreeEvent;
            treeView1.MouseDoubleClick += TreeView1_MouseDoubleClick;
            treeView1.KeyDown += TreeView1_KeyDown;
            buildTreeThread = new System.Threading.Thread(StartGeteCATFile);
            buildTreeThread.IsBackground = true;
            buildTreeThread.Start();
        }

        private void TreeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                TreeViewSelectItemChange();
            }
        }

        public delegate void InvokeDelegate();
        static object lockObj = new object();
        private void Form_ParsLeft_BuildTreeEvent(object socket)
        {
            lock (lockObj)
            {
                //最后完成树的生成
                if (this.IsHandleCreated)
                    treeView1.BeginInvoke(new InvokeDelegate(InvokeMethod));
                //treeView1.MouseClick += TreeView1_MouseDoubleClick;
            }

        }
        string dataType = string.Empty;
        string protocolType = string.Empty;
        private void TreeViewSelectItemChange()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
                return;
            string nodeText = treeView1.SelectedNode.Text;
            string nodePath = treeView1.SelectedNode.Tag.ToString();
            string nodeName = treeView1.SelectedNode.Name.ToString();
            int indexsLast = nodeName.LastIndexOf('_');
            int indexsFirst = nodeName.IndexOf('_');
            string[] nameArray = nodeName.Split('_');

            if (nameArray.Length > 0)
                protocolType = nameArray[0];
            if (nameArray.Length > 1)
                dataType = nameArray[1];
            #region ProtocolType
            XDCProtocolType currentNodeProtocolDataType = XDCProtocolType.NDC;
            switch (protocolType.ToLower())
            {
                case "ddc":
                    {
                        currentNodeProtocolDataType = XDCProtocolType.DDC;
                    }
                    break;
                case "ndc":
                    {
                        currentNodeProtocolDataType = XDCProtocolType.NDC;
                    }
                    break;
                default:
                    break;
            }
            #endregion

            #region DataType

            DataType currentNodeDataType = DataType.Message;
            switch (dataType)
            {
                case "State":
                    {
                        currentNodeDataType = DataType.State;
                    }
                    break;
                case "Screen":
                    {
                        currentNodeDataType = DataType.Screen;
                    }
                    break;
                case "Fit":
                    {
                        currentNodeDataType = DataType.Fit;
                    }
                    break;
                case "Message":
                    {
                        currentNodeDataType = DataType.Message;
                    }
                    break;
                default:
                    break;
            }

            #endregion
            SubFormEvent(nodePath, currentNodeProtocolDataType, currentNodeDataType);
        }

        private void TreeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeViewSelectItemChange();
        }

        private void InvokeMethod()
        {
            try
            {

                Root.Expand();
                treeView1.Nodes.Add(Root);
                if (!string.IsNullOrEmpty(onlyNode))
                {
                    treeView1.ExpandAll();
                    if (treeView1.Nodes[0].Nodes.Count > 2)
                        treeView1.Nodes[0].Nodes[2].Collapse();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("你已经打开一个SuperParse");
            }
        }
        static string onlyNode = string.Empty;
        static Dictionary<string, List<string>> stateScan_NDC = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> stateScan_DDC = new Dictionary<string, List<string>>();
        private static void GetFilesList(ref TreeNode currentNode, string dirctoryPath)
        {
            DirectoryInfo folder = new DirectoryInfo(XDCUnity.eCATPath + dirctoryPath);
            if (!folder.Exists)
                return;
            foreach (FileInfo fileItem in folder.GetFiles("*.txt"))
            {
                if (!string.IsNullOrEmpty(onlyNode)
                    && !fileItem.Name.StartsWith(onlyNode))
                    continue;
                TreeNode tn = new TreeNode();
                if (currentNode.Name.EndsWith("ndc_state", StringComparison.OrdinalIgnoreCase))
                {
                    #region 读取状态内的信息 20160330----NDC
                    string stateContent = XDCUnity.GetTxtFileText(fileItem.FullName);
                    string stateFlag = stateContent.StartsWith("@") ? stateContent.Substring(0, 2) : stateContent.Substring(0, 1);
                    if (stateFlag.Equals("d"))
                    {

                    }
                    if (!stateScan_NDC.ContainsKey(stateFlag))
                    {
                        stateScan_NDC.Add(stateFlag, new List<string> { fileItem.Name.Substring(0, fileItem.Name.Length - 4) });
                    }
                    else
                    {
                        if (!stateScan_NDC[stateFlag].Contains(fileItem.Name.Substring(0, fileItem.Name.Length - 4)))
                            stateScan_NDC[stateFlag].Add(fileItem.Name.Substring(0, fileItem.Name.Length - 4));
                    }
                    tn.Text = fileItem.Name.Substring(0, fileItem.Name.Length - 4) + "_" + stateFlag;
                    #endregion
                }
                else if (currentNode.Name.EndsWith("ddc_state", StringComparison.OrdinalIgnoreCase))
                {

                    #region 读取状态内的信息 20160330----DDC
                    string stateContent = XDCUnity.GetTxtFileText(fileItem.FullName);
                    string stateFlag = stateContent.StartsWith("@") ? stateContent.Substring(0, 2) : stateContent.Substring(0, 1);
                    if (stateFlag.Equals("d"))
                    {

                    }
                    if (!stateScan_DDC.ContainsKey(stateFlag))
                    {
                        stateScan_DDC.Add(stateFlag, new List<string> { fileItem.Name.Substring(0, fileItem.Name.Length - 4) });
                    }
                    else
                    {
                        if (!stateScan_DDC[stateFlag].Contains(fileItem.Name.Substring(0, fileItem.Name.Length - 4)))
                            stateScan_DDC[stateFlag].Add(fileItem.Name.Substring(0, fileItem.Name.Length - 4));
                    }
                    tn.Text = fileItem.Name.Substring(0, fileItem.Name.Length - 4) + "_" + stateFlag;
                    #endregion
                }
                else
                    tn.Text = fileItem.Name.Substring(0, fileItem.Name.Length - 4);
                tn.Name = currentNode.Name + "_" + fileItem.Name;
                tn.Tag = fileItem.FullName;
                currentNode.Nodes.Add(tn);
            }
        }
        private static void GetFilesListEx(ref TreeNode currentNode, string dirctoryPath)
        {
            DirectoryInfo folder = new DirectoryInfo(dirctoryPath);
            if (!folder.Exists)
                return;
            foreach (FileInfo fileItem in folder.GetFiles("*.txt"))
            {
                if (!string.IsNullOrEmpty(onlyNode)
                    && !fileItem.Name.StartsWith(onlyNode))
                    continue;
                TreeNode tn = new TreeNode();
                //if (currentNode.Name.EndsWith("ndc_state", StringComparison.OrdinalIgnoreCase))
                //{
                //    #region 读取状态内的信息 20160330----NDC
                //    string stateContent = XDCUnity.GetTxtFileText(fileItem.FullName);
                //    string stateFlag = stateContent.StartsWith("@") ? stateContent.Substring(0, 2) : stateContent.Substring(0, 1);
                //    if (!stateScan_NDC.ContainsKey(stateFlag))
                //    {
                //        stateScan_NDC.Add(stateFlag, new List<string> { fileItem.Name.Substring(0, fileItem.Name.Length - 4) });
                //    }
                //    else
                //    {
                //        if (!stateScan_NDC[stateFlag].Contains(fileItem.Name.Substring(0, fileItem.Name.Length - 4)))
                //            stateScan_NDC[stateFlag].Add(fileItem.Name.Substring(0, fileItem.Name.Length - 4));
                //    }
                //    tn.Text = fileItem.Name.Substring(0, fileItem.Name.Length - 4) + "_" + stateFlag;
                //    #endregion
                //}
                //else if (currentNode.Name.EndsWith("ddc_state", StringComparison.OrdinalIgnoreCase))
                //{
                //    #region 读取状态内的信息 20160330----DDC
                //    string stateContent = XDCUnity.GetTxtFileText(fileItem.FullName);
                //    string stateFlag = stateContent.StartsWith("@") ? stateContent.Substring(0, 2) : stateContent.Substring(0, 1);
                //    if (!stateScan_DDC.ContainsKey(stateFlag))
                //    {
                //        stateScan_DDC.Add(stateFlag, new List<string> { fileItem.Name.Substring(0, fileItem.Name.Length - 4) });
                //    }
                //    else
                //    {
                //        if (!stateScan_DDC[stateFlag].Contains(fileItem.Name.Substring(0, fileItem.Name.Length - 4)))
                //            stateScan_DDC[stateFlag].Add(fileItem.Name.Substring(0, fileItem.Name.Length - 4));
                //    }
                //    tn.Text = fileItem.Name.Substring(0, fileItem.Name.Length - 4) + "_" + stateFlag;
                //    #endregion
                //}
                //else
                tn.Text = fileItem.Name.Substring(0, fileItem.Name.Length - 4);
                tn.Name = currentNode.Name + "_" + fileItem.Name;
                tn.Tag = fileItem.FullName;
                currentNode.Nodes.Add(tn);
            }

            DirectoryInfo[] subFolders = folder.GetDirectories();
            if (subFolders != null & subFolders.Length > 0)
            {
                foreach (DirectoryInfo item in subFolders)
                {

                    TreeNode tn_FullDownLoad_sub = new TreeNode();
                    if (item.FullName.Contains("NDC") && item.FullName.Contains("State"))
                    {
                        tn_FullDownLoad_sub.Name = "NDC_State";
                    }
                    else if (item.FullName.Contains("NDC") && item.FullName.Contains("Screen"))
                    {
                        tn_FullDownLoad_sub.Name = "NDC_Screen";
                    }
                    else if (item.FullName.Contains("NDC") && item.FullName.Contains("FIT"))
                    {
                        tn_FullDownLoad_sub.Name = "NDC_Fit";
                    }
                    else if (item.FullName.Contains("DDC") && item.FullName.Contains("State"))
                    {
                        tn_FullDownLoad_sub.Name = "DDC_State";
                    }
                    else if (item.FullName.Contains("DDC") && item.FullName.Contains("Screen"))
                    {
                        tn_FullDownLoad_sub.Name = "DDC_Screen";
                    }
                    else if (item.FullName.Contains("DDC") && item.FullName.Contains("FIT"))
                    {
                        tn_FullDownLoad_sub.Name = "DDC_Fit";
                    }
                    tn_FullDownLoad_sub.Text = item.Name;
                    //COM
                    currentNode.Nodes.Add(tn_FullDownLoad_sub);
                    GetFilesListEx(ref tn_FullDownLoad_sub, item.FullName);
                }
            }

        }
        private static void GetComLogFile(ref TreeNode currentNode, string dirctoryPath)
        {
            DirectoryInfo folder = new DirectoryInfo(XDCUnity.eCATPath + dirctoryPath);
            if (!folder.Exists)
                return;
            foreach (FileInfo fileItem in folder.GetFiles("*.txt"))
            {
                if (!fileItem.Name.StartsWith("COM"))
                    continue;
                TreeNode tn = new TreeNode();
                tn.Name = currentNode.Name + "_" + fileItem.Name;
                tn.Text = fileItem.Name;
                tn.Tag = fileItem.FullName;
                currentNode.Nodes.Add(tn);
            }
        }
        static object lockObj_ddc = new object();
        static object lockObj_ndc = new object();

        private static void StartGeteCATFile()
        {
            Root = null;
            Root = new TreeNode();
            //1.添加根目录
            Root.Name = "Root";
            Root.Text = "Root";

            //2.添加DDC/NDC文件夹

            #region DDC

            TreeNode tn_DDC = new TreeNode();
            tn_DDC.Name = "DDC";
            tn_DDC.Text = "DDC";

            //DDC
            string ddc_Path = @"\XDC\DDC\Scripts";

            //state
            string ddc_state = ddc_Path + @"\State\Host";
            TreeNode tn_ddc_state = new TreeNode();
            tn_ddc_state.Text = "State";
            tn_ddc_state.Name = "DDC_State";
            tn_DDC.Nodes.Add(tn_ddc_state);
            GetFilesList(ref tn_ddc_state, ddc_state);

            //screen
            string ddc_screen = ddc_Path + @"\Screen\Host\000";
            TreeNode tn_ddc_screen = new TreeNode();
            tn_ddc_screen.Text = "Screen";
            tn_ddc_screen.Name = "DDC_Screen";
            tn_DDC.Nodes.Add(tn_ddc_screen);
            GetFilesList(ref tn_ddc_screen, ddc_screen);

            //fit
            string ddc_fit = ddc_Path + @"\FIT";
            TreeNode tn_ddc_fit = new TreeNode();
            tn_ddc_fit.Text = "Fit";
            tn_ddc_fit.Name = "DDC_Fit";
            tn_DDC.Nodes.Add(tn_ddc_fit);
            GetFilesList(ref tn_ddc_fit, ddc_fit);

            lock (lockObj_ddc)
            {

                Root.Nodes.Add(tn_DDC);
            }
            #endregion

            #region NDC

            TreeNode tn_NDC = new TreeNode();
            tn_NDC.Name = "NDC";
            tn_NDC.Text = "NDC";

            //NDC
            string ndc_Path = @"\XDC\NDC\Scripts";

            //state
            string ndc_state = ndc_Path + @"\State\Host";
            TreeNode tn_ndc_state = new TreeNode();
            tn_ndc_state.Text = "State";
            tn_ndc_state.Name = "NDC_State";
            tn_NDC.Nodes.Add(tn_ndc_state);
            GetFilesList(ref tn_ndc_state, ndc_state);

            //screen
            string ndc_screen = ndc_Path + @"\Screen\Host\000";
            TreeNode tn_ndc_screen = new TreeNode();
            tn_ndc_screen.Text = "Screen";
            tn_ndc_screen.Name = "NDC_Screen";
            tn_NDC.Nodes.Add(tn_ndc_screen);
            GetFilesList(ref tn_ndc_screen, ndc_screen);

            //fit
            string ndc_fit = ndc_Path + @"\FIT";
            TreeNode tn_ndc_fit = new TreeNode();
            tn_ndc_fit.Text = "Fit";
            tn_ndc_fit.Name = "NDC_Fit";
            tn_NDC.Nodes.Add(tn_ndc_fit);
            GetFilesList(ref tn_ndc_fit, ndc_fit);

            lock (lockObj_ndc)
            {
                Root.Nodes.Add(tn_NDC);
            }

            #endregion

            #region Log
            TreeNode tn_COMLog = new TreeNode();
            tn_COMLog.Name = "COM";
            tn_COMLog.Text = "COM.Log";
            //COM
            string log_Path = @"\log";
            GetComLogFile(ref tn_COMLog, log_Path);

            Root.Nodes.Add(tn_COMLog);
            #endregion

            #region FullDownLoadManagement

            TreeNode tn_FullDownLoad = new TreeNode();
            tn_FullDownLoad.Name = "FullDownLoad";
            tn_FullDownLoad.Text = "FullDownLoad";
            //COM
            string fulldownload_Path = XDCUnity.CurrentPath + @"\Config\Server";
            GetFilesListEx(ref tn_FullDownLoad, fulldownload_Path);

            Root.Nodes.Add(tn_FullDownLoad);


            #endregion
            //完成 树的生成
            BuildTreeEvent(null);
        }

        List<TreeNode> treenodeList = new List<TreeNode>();
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            onlyNode = textBox1.Text;
            buildTreeThread.Abort();
            buildTreeThread = null;
            buildTreeThread = new System.Threading.Thread(StartGeteCATFile);
            treeView1.Nodes.Clear();
            buildTreeThread.Start();

            System.Threading.Thread.Sleep(50);
        }

        private void checkAllStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_StateCheck(stateScan_NDC, stateScan_DDC).Show();

        }
    }
}
