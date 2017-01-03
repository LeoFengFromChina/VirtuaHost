using System;
using System.Windows.Forms;
using StandardFeature;
namespace VirtualDualHost
{
    public partial class Form_FentchConfig : Form
    {
        public Form_FentchConfig()
        {
            InitializeComponent();
            // use for testing
            XDCUnity.CurrentPath = @"D:\Develop\VirtualDualHost_20151116\VirtualDualHost_New\VirtualDualHost\bin\Debug";
        }
        string CurrentHost = "Host_1";
        XDCProtocolType CurrentProtocol = XDCProtocolType.NDC;
        string CurrentIniPath = XDCUnity.CurrentPath + @"\Config\Server\NDC\Host_1\CommonConfig.ini";
        string ProtocolString = "NDC";
        public Form_FentchConfig(string argCurrentHost, XDCProtocolType argProtocolType)
        {
            InitializeComponent();
            CurrentHost = argCurrentHost;
            CurrentProtocol = argProtocolType;
            ProtocolString = argProtocolType == XDCProtocolType.NDC ? "NDC" : "DDC";
            CurrentIniPath = XDCUnity.CurrentPath + @"\Config\Server\" + ProtocolString + "\\" + CurrentHost + "\\CommonConfig.ini";
        }

        private void Form_FentchConfig_Load(object sender, EventArgs e)
        {
            btn_OK.Click += Btn_OK_Click;
            btn_Cancel.Click += Btn_Cancel_Click;
            InitialFentchConfig();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            //fentchconfig
            ProcessSetGroupBox(gb_FentchConfig.Controls, "BeforeGoInServiceSend", "");
            //keydevice
            ProcessSetGroupBox(gb_KeyDevices.Controls, "GoOutOfServiceConfig", "ErrorForOutOfService");
            MessageBox.Show("Save Successed.");
        }

        private void InitialFentchConfig()
        {
            CurrentIniPath = XDCUnity.CurrentPath + @"\Config\Server\" + ProtocolString + "\\" + CurrentHost + "\\CommonConfig.ini";
            string flag = string.Empty;

            //FentchConfig
            ProcessInitialGroupBox(gb_FentchConfig.Controls, "BeforeGoInServiceSend", "");
            //KeyDevice
            ProcessInitialGroupBox(gb_KeyDevices.Controls, "GoOutOfServiceConfig", "ErrorForOutOfService");
        }

        private void ProcessInitialGroupBox(Control.ControlCollection artControls, string argSection, string argKeyAppendText)
        {
            CurrentIniPath = XDCUnity.CurrentPath + @"\Config\Server\" + ProtocolString + "\\" + CurrentHost + "\\CommonConfig.ini";
            string flag = string.Empty;
            foreach (Control item in artControls)
            {
                flag = string.Empty;
                if (item.GetType().Name == "CheckBox")
                {
                    flag = XDCUnity.ReadIniData(argSection, ((CheckBox)item).Text + argKeyAppendText, "", CurrentIniPath);

                    if (flag.Equals("1"))
                        ((CheckBox)item).Checked = true;
                    else
                    {
                        ((CheckBox)item).Checked = false;
                    }
                }
            }
        }

        private void ProcessSetGroupBox(Control.ControlCollection artControls, string argSection, string argKeyAppendText)
        {

            CurrentIniPath = XDCUnity.CurrentPath + @"\Config\Server\" + ProtocolString + "\\" + CurrentHost + "\\CommonConfig.ini";
            string flag = string.Empty;
            foreach (Control item in artControls)
            {
                flag = "1"; ;
                if (item.GetType().Name == "CheckBox")
                {
                    if (((CheckBox)item).Checked)
                    {
                        flag = "1";
                    }
                    else
                    {
                        flag = "0";
                    }
                    XDCUnity.WriteIniData(argSection, ((CheckBox)item).Text + argKeyAppendText, flag, CurrentIniPath);
                }
            }

        }

    }
}
