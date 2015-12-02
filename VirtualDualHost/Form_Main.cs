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
        Form_NDCServer form_NDCServer;
        Form_DDCServer form_DDCServer;
        Form_DualHost form_DualHost;
        private void Form_Main_Load(object sender, EventArgs e)
        {

            //NDCserver
            form_NDCServer = new Form_NDCServer();
            form_NDCServer.Show(this.dockPanel1, DockState.Document);

            //DDCserver
            form_DDCServer = new Form_DDCServer();
            form_DDCServer.Show(this.dockPanel1, DockState.Document);

            //双主机
            form_DualHost = new Form_DualHost();
            form_DualHost.Show(this.dockPanel1, DockState.Document);

            form_NDCServer.Activate();

            dDCServerToolStripMenuItem.Click += DDCServerToolStripMenuItem_Click;

        }

        private void DDCServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmi = sender as ToolStripMenuItem;
            switch (tmi.Text)
            {
                case "DDCServer":
                    {
                        //DDCserver
                        form_DDCServer = new Form_DDCServer();
                        form_DDCServer.Show(this.dockPanel1, DockState.Document);
                    }
                    break;
                case "NDCServer":
                    {
                        form_NDCServer = new Form_NDCServer();
                        form_NDCServer.Show(this.dockPanel1, DockState.Document);
                    }
                    break;
                case "VirtualDualHost":
                    {
                        form_DualHost = new Form_DualHost();
                        form_DualHost.Show(this.dockPanel1, DockState.Document);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
