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

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();

            if (string.IsNullOrEmpty(folderDlg.SelectedPath))
                return;
            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");
            node.Attributes["value"].InnerText = folderDlg.SelectedPath;
            doc.Save(Environment.CurrentDirectory + @"\Config\BaseConfig.xml");
            MessageBox.Show("Save.");
        }

        private void From_Seeting_eCATPath_Load(object sender, EventArgs e)
        {

            XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
            XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");
            textBox1.Text = node.Attributes["value"].InnerText;
            XDCUnity.eCATPath = textBox1.Text.Trim();
        }
    }
}
