using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

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

            //NDCserver
            //form_NDCServer = new Form_NDCServer();

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
                case "eCAT":
                    {
                        From_Seeting_eCATPath form_eCAT = new From_Seeting_eCATPath();
                        form_eCAT.Show();
                    }
                    break;
                case "About":
                    {
                        new Form_About().ShowDialog();
                    }
                    break;
                default:
                    break;
            }
        }

        private void Form_Main_LocationChanged(object sender, EventArgs e)
        {

        }
    }
}
