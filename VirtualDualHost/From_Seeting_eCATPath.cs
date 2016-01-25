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
using XmlHelper;

namespace VirtualDualHost
{
    public partial class From_Seeting_eCATPath : Form
    {
        public From_Seeting_eCATPath()
        {
            InitializeComponent();
        }
        XmlDocument doc = null;

        string currentProcessPath = string.Empty;
        private void From_Seeting_eCATPath_Load(object sender, EventArgs e)
        {
            //1.当前路径，不知为何，在XP上，如果在当前窗体选择了文件后，使用system.Enviroment获取路径会有差异。所以用这个方法
            //由一个A进程启动B进程，在B进程中使用system.Enviroment获取路径会得到A进程的当前路径。
            string processName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            currentProcessPath = processName.Substring(0, processName.LastIndexOf('\\'));

            //2.获取eCAT路径列表
            string resultXML_eCAT = XMLHelper.instance.XMLFiles["BaseConfig"].GetXmlAttributeValue("BaseConfig.Settings.eCATPath.[*].{*}");
            if (!string.IsNullOrEmpty(resultXML_eCAT))
            {
                List<string> eCATPathList = XDCUnity.GetXmlValueList(resultXML_eCAT);
                cmb_eCAT.DataSource = eCATPathList;
            }
            string resultXML_TrueBack = XMLHelper.instance.XMLFiles["BaseConfig"].GetXmlAttributeValue("BaseConfig.Settings.TrueBackPath.[*].{*}");
            if (!string.IsNullOrEmpty(resultXML_TrueBack))
            {
                List<string> trueBackPathList = XDCUnity.GetXmlValueList(resultXML_TrueBack);
                cmb_Trueback.DataSource = trueBackPathList;
            }

            //3.获取当前默认的eCAT路径
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode eCATnode = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");
            XmlNode trueBacknode = doc.SelectSingleNode("BaseConfig/Settings/TrueBackPath");

            //默认选中eCAT
            string currenteCATPath = eCATnode.Attributes["value"].InnerText;
            if (cmb_eCAT.Items.Contains(currenteCATPath))
                cmb_eCAT.Text = currenteCATPath;

            //默认选中TrueBack
            string currentTruebackPath = trueBacknode.Attributes["value"].InnerText;
            if (cmb_Trueback.Items.Contains(currentTruebackPath))
                cmb_Trueback.Text = currentTruebackPath;

        }
        XmlNode neweCATPathNode = null;
        XmlNode currenteCATnode = null;
        XmlNode newTruebackPathNode = null;
        XmlNode currentTruebacknode = null;
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(cmb_eCAT.Text))
                folderDlg.SelectedPath = cmb_eCAT.Text;
            folderDlg.ShowDialog();

            string SelectPath = folderDlg.SelectedPath;

            if (string.IsNullOrEmpty(SelectPath))
                return;
            cmb_eCAT.Text = SelectPath;
        }

        private void btn_TrueBack_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderDlg = new OpenFileDialog();
            if (!string.IsNullOrEmpty(cmb_Trueback.Text))
                folderDlg.InitialDirectory = cmb_Trueback.Text;
            folderDlg.ShowDialog();

            string SelectFile = folderDlg.FileName;

            if (string.IsNullOrEmpty(SelectFile))
                return;
            cmb_Trueback.Text = SelectFile;
        }


        private void btn_OK_Click(object sender, EventArgs e)
        {
            ProcesseCATPath(cmb_eCAT.Text);

            if (currenteCATnode != null
                && neweCATPathNode != null)
            {
                //是否有新增节点
                currenteCATnode.PrependChild(neweCATPathNode);
                ProcessTailNode(ref currenteCATnode);
            }

            ProcessTrueBackPath(cmb_Trueback.Text);
            if (currentTruebacknode != null
                && newTruebackPathNode != null)
            {
                //是否有新增节点
                currentTruebacknode.PrependChild(newTruebackPathNode);
                ProcessTailNode(ref currentTruebacknode);
            }


            XDCUnity.eCATPath = cmb_eCAT.Text;
            currenteCATnode.Attributes["value"].InnerText = cmb_eCAT.Text;

            XDCUnity.TrueBackPath = cmb_Trueback.Text;
            currentTruebacknode.Attributes["value"].InnerText = cmb_Trueback.Text;
            try
            {
                doc.Save(currentProcessPath + @"\Config\BaseConfig.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Error:" + ex.ToString());
            }
            //doc.Save();
            MessageBox.Show("Save Successed.");
            currenteCATnode = null;
            neweCATPathNode = null;
            this.Close();
        }

        private void ProcesseCATPath(string path)
        {
            if (null == doc)
                doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            currenteCATnode = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");

            if (!cmb_eCAT.Items.Contains(path))
            {
                //如果不存在，新增
                neweCATPathNode = doc.CreateElement("item");
                XmlAttribute attr = doc.CreateAttribute("value");
                attr.Value = path;
                neweCATPathNode.Attributes.SetNamedItem(attr);
            }
        }
        private void ProcessTrueBackPath(string path)
        {
            if (null == doc)
                doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            currentTruebacknode = doc.SelectSingleNode("BaseConfig/Settings/TrueBackPath");

            if (!cmb_Trueback.Items.Contains(path))
            {
                //如果不存在，新增
                newTruebackPathNode = doc.CreateElement("item");
                XmlAttribute attr = doc.CreateAttribute("value");
                attr.Value = path;
                newTruebackPathNode.Attributes.SetNamedItem(attr);
            }
        }

        private void ProcessTailNode(ref XmlNode node)
        {
            if (node.ChildNodes.Count > 10)
            {
                for (int i = 10; i < node.ChildNodes.Count; i++)
                {
                    node.RemoveChild(node.ChildNodes[i]);
                }
            }
        }
    }
}
