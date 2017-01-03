using StandardFeature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VirtualDualHost
{
    public partial class Form_ManuSendLeft : DockContent
    {
        public Form_ManuSendLeft()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        Form_Pars form_Pars;
        private DockPanel dp;

        public static TreeNode Root = new TreeNode();
        public Form_ManuSendLeft(Form_Pars formMain)
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
                    tn_FullDownLoad_sub.Name = item.Name;
                    tn_FullDownLoad_sub.Text = item.Name;
                    //tn_FullDownLoad_sub.Expand();
                    currentNode.Nodes.Add(tn_FullDownLoad_sub);
                    //currentNode.Expand();
                    GetFilesListEx(ref tn_FullDownLoad_sub, item.FullName);
                }
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

            #region FullDownLoadManagement

            string fulldownload_Path = XDCUnity.CurrentPath + @"\Config\ManuSendData";
            GetFilesListEx(ref Root, fulldownload_Path);
           
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
