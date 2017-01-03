using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VirtualDualHost
{
    public partial class Form_Managerment : DockContent
    {
        public Form_Managerment()
        {
            InitializeComponent();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Nodes.Count > 0)
                {
                    bool NoFalse = true;
                    foreach (TreeNode tn in e.Node.Nodes)
                    {
                        if (tn.Checked == false)
                        {
                            NoFalse = false;
                        }
                    }
                    if (e.Node.Checked == true || NoFalse)
                    {
                        foreach (TreeNode tn in e.Node.Nodes)
                        {
                            if (tn.Checked != e.Node.Checked)
                            {
                                tn.Checked = e.Node.Checked;
                            }
                        }
                    }
                }
                if (e.Node.Parent != null && e.Node.Parent is TreeNode)
                {
                    bool ParentNode = true;
                    foreach (TreeNode tn in e.Node.Parent.Nodes)
                    {
                        if (tn.Checked == false)
                        {
                            ParentNode = false;
                        }
                    }
                    if (e.Node.Parent.Checked != ParentNode && (e.Node.Checked == false || e.Node.Checked == true && e.Node.Parent.Checked == false))
                    {
                        e.Node.Parent.Checked = ParentNode;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        BindingList<PackListView> PackList = null;
        private void Form_TreeCheck_Load(object sender, EventArgs e)
        {
            runToolStripMenuItem.Click += ToolStripMenuItem_Click;
            collapseToolStripMenuItem.Click += ToolStripMenuItem_Click;
            addToNewPackToolStripMenuItem.Click += ToolStripMenuItem_Click;
            ExtensionIndex.Add("FloderClosed", 0);
            ExtensionIndex.Add(".dll", 7);
            ExtensionIndex.Add(".xml", 8);
            ExtensionIndex.Add(".txt", 6);
            ExtensionIndex.Add(".ini", 0);
            ExtensionIndex.Add(".log", 6);
            ExtensionIndex.Add(".html", 6);
            ExtensionIndex.Add(".htm", 6);
            TreeNode Root = new TreeNode();
            Root.Text = "eCAT";
            GetAllNode(ref Root, XDCUnity.eCATPath);
            Root.Expand();
            treeView1.Nodes.Add(Root);


            PackList = new BindingList<PackListView>();
            PackList.Add(new PackListView("Inservice.dll", "dll", @"C:\\Inservice.dll"));
            dataGridView1.DataSource = PackList;
            
        }

        private void DataGridView1_BindingContextChanged(object sender, EventArgs e)
        {
        }

        public void GetAllNode(ref TreeNode Root, string dirctoryPath)
        {
            DirectoryInfo folder = new DirectoryInfo(dirctoryPath);
            if (!folder.Exists)
                return;


            #region GetDirectories 
            foreach (DirectoryInfo item in folder.GetDirectories())
            {
                TreeNode subNode = new TreeNode();
                subNode.Name = item.FullName;
                subNode.Text = item.Name;
                subNode.ToolTipText = item.FullName;
                subNode.ImageIndex = IconIndexes.ClosedFolder;
                subNode.SelectedImageIndex = IconIndexes.ClosedFolder;
                if (item.GetFiles().Length > 0 || item.GetDirectories().Length > 0)
                {
                    //如果有下级节点，先创造一个空节点。展开的时候则先清楚空节点再加载真实路径的节点。
                    subNode.Nodes.Add(new TreeNode());
                }
                Root.Nodes.Add(subNode);
            }
            #endregion

            #region GetFiles

            foreach (FileInfo fileItem in folder.GetFiles())
            {
                TreeNode tn = new TreeNode();

                tn.Text = fileItem.Name;
                tn.Name = fileItem.FullName;
                tn.ToolTipText = fileItem.FullName;
                tn.ImageIndex = GetFileExtensionIndex(fileItem.Extension);
                tn.SelectedImageIndex = GetFileExtensionIndex(fileItem.Extension);
                if (Root.Checked)
                {
                    tn.Checked = true;
                }
                Root.Nodes.Add(tn);
            }

            #endregion



        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            string path = e.Node.Name;
            if (string.IsNullOrEmpty(path))
                return;

            ShowStatus(e.Node.Name);
            TreeNode tempNode = new TreeNode();
            GetAllNode(ref tempNode, path);
            e.Node.Nodes.Clear();

            foreach (TreeNode item in tempNode.Nodes)
            {
                e.Node.Nodes.Add(item);
            }
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!File.Exists(e.Node.Name)
                || (!e.Node.Name.ToLower().EndsWith(".txt")
                && !e.Node.Name.ToLower().EndsWith(".ini")
                && !e.Node.Name.ToLower().EndsWith(".xml")
                && !e.Node.Name.ToLower().EndsWith(".html")
                && !e.Node.Name.ToLower().EndsWith(".log")
                && !e.Node.Name.ToLower().EndsWith(".bat")
                && !e.Node.Name.ToLower().EndsWith(".htm")))
                return;
            ShowStatus(e.Node.Name);
            //OpenTextFileWith("notepad++.exe", e.Node.Name);
            XDCUnity.OpenTextFileWith("notepad++.exe", e.Node.Name);
        }

        public void OpenTextFileWith(string toolName, string fileName)
        {
            try
            {
                Process.Start(toolName, fileName);
            }
            catch (Exception ex)
            {
                Process.Start("notepad.exe", fileName);

            }
        }
        Dictionary<string, int> ExtensionIndex = new Dictionary<string, int>();
        public int GetFileExtensionIndex(string ExtensionName)
        {
            if (!ExtensionIndex.ContainsKey(ExtensionName))
                return IconIndexes.Txt;
            return ExtensionIndex[ExtensionName];
        }
        private void ShowStatus(string msg)
        {
            lbl_Status.Text = msg;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            ShowStatus(e.Node.Name);
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmi = sender as ToolStripMenuItem;
            switch (tmi.Text)
            {
                case "CollapseAll":
                    {
                        treeView1.CollapseAll();
                    }
                    break;
                case "Collapse":
                    {
                        treeView1.SelectedNode.Parent.Collapse();
                    }
                    break;
                case "Run":
                    {
                        XDCUnity.OpenTextFileWith("", treeView1.SelectedNode.Name);
                    }
                    break;
                case "AddToNewPack":
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PackList.Add(new PackListView("Inservice.dll222", "dll", @"C:\\Inservice.dll"));
            //PackList[PackList.Count - 2].FileName = "XXX";
        }
    }


    public class IconIndexes
    {
        public const int MyComputer = 1;      //我的电脑  
        public const int ClosedFolder = 0;    //文件夹关闭  
        public const int OpenFolder = 5;      //文件夹打开  
        public const int FixedDrive = 1;      //磁盘盘符  
        public const int MyDocuments = 4;     //我的文档  
        public const int Txt = 6;     //我的文档  
        public const int Dll = 7;     //我的文档  
        public const int Xml = 8;     //我的文档  
    }
    public class PackListView
    {
        private string _fileName = string.Empty;
        private string _fileType = string.Empty;
        private string _filePath = string.Empty;
        public PackListView(string fileName,string fileType,string filePath)
        {
            _fileName = fileName;
            _fileType = fileType;
            _filePath = filePath;
        }
        
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        public string FileType
        {
            get
            {
                return _fileType;
            }
            set
            {
                _fileType = value;
            }
        }
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }
    }
}
