using MessagePars_NDC;
using MessagePars_DDC;
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

namespace VirtualDualHost
{
    public partial class Form_ImportLogs : Form
    {
        public Form_ImportLogs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderDlg = new OpenFileDialog();
            if (!string.IsNullOrEmpty(txt_LogPath.Text))
                folderDlg.InitialDirectory = txt_LogPath.Text;
            folderDlg.ShowDialog();

            string SelectFile = folderDlg.FileName;

            if (string.IsNullOrEmpty(SelectFile))
                return;
            txt_LogPath.Text = SelectFile;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txt_LogPath.Text.Trim()))
            { return; }


            string curProtocol = rbt_DDC.Checked ? "DDC" : "NDC";
            XDCUnity.MessageFormat = rbt_DDC.Checked ? new MessageFormat_DDC() as IMessageFormat : new MessageFormat_NDC() as IMessageFormat;
            string curHost = rbt_Server_1.Checked ? "Host_1" : "Host_2";
            string opcIniFilePath = string.Empty;
            string iniFileName = "OperationCodeConfig.ini";

            opcIniFilePath = Environment.CurrentDirectory + @"\Config\Server\" + curProtocol + "\\" + curHost + "\\" + iniFileName;

            ReadLogs(txt_LogPath.Text);
            foreach (KeyValuePair<string, OperationCodeObject> item in OperationDic)
            {
                richTextBox1.AppendText("[" + item.Key.Replace(" ", "_") + "]\r\n");

                XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "Comment", "{Input Comment}", opcIniFilePath);
                richTextBox1.AppendText("Comment=" + "{ Input Comment}\r\n");

                XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "DeadMsg", item.Value.ReplyMsg, opcIniFilePath);
                richTextBox1.AppendText("DeadMsg=" + item.Value.ReplyMsg + "\r\n");

                if (!string.IsNullOrEmpty(item.Value.InteractiveMsg))
                {
                    XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "InteractiveReply", "1", opcIniFilePath);
                    richTextBox1.AppendText("InteractiveReply= 1\r\n");
                    XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "DeadInterActiveMsg", item.Value.InteractiveMsg, opcIniFilePath);
                    richTextBox1.AppendText("DeadInterActiveMsg=" + item.Value.InteractiveMsg + "\r\n");
                }

                richTextBox1.AppendText("===========================\r\n");
            }

            MessageBox.Show("Successed.");
        }

        public Dictionary<string, OperationCodeObject> OperationDic = new Dictionary<string, OperationCodeObject>();
        private void ReadLogs(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;

            OperationCodeObject opcObject = null;
            string tempLine = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                //3类消息，交易请求、交互响应、交易回复
                if (!line.StartsWith("TRANSMIT: 11")
                    && !line.StartsWith("RECEIVE: 3")
                    && !line.StartsWith("RECEIVE: 4"))
                {
                    continue;
                }
                if (line.StartsWith("TRANSMIT: "))
                {
                    tempLine = line.Replace("TRANSMIT: ", "").TrimStart();
                }

                if (line.StartsWith("RECEIVE: "))
                {
                    tempLine = line.Replace("RECEIVE: ", "").TrimStart();
                }
                byte[] msgByteArray = Encoding.ASCII.GetBytes(tempLine);
                XDCMessage XDCmsg = XDCUnity.MessageFormat.Format(msgByteArray, tempLine.Length);


                //
                if (line.StartsWith("TRANSMIT: 11"))
                {
                    if (opcObject != null
                        && !string.IsNullOrEmpty(opcObject.ReplyMsg))
                    {

                        if (OperationDic.ContainsKey(opcObject.Opc))
                        {
                            OperationDic[opcObject.Opc] = opcObject;
                        }
                        else
                        {
                            OperationDic.Add(opcObject.Opc, opcObject);
                        }
                    }
                    //交易请求
                    opcObject = null;
                    opcObject = new OperationCodeObject();
                    opcObject.Opc = XDCmsg.OperationCode;
                }
                else if (line.StartsWith("RECEIVE: 3"))
                {
                    if (opcObject != null)
                    {
                        opcObject.InteractiveMsg = line;
                    }
                }
                else if (line.StartsWith("RECEIVE: 4"))
                {
                    if (opcObject != null)
                    {
                        opcObject.ReplyMsg = tempLine;
                    }
                }
                else
                {

                }

            }
        }
    }

    public class OperationCodeObject
    {
        public OperationCodeObject()
        { }
        public OperationCodeObject(string _opc, string _interactiveMsg, string _replymsg)
        {
            Opc = _opc;
            InteractiveMsg = _interactiveMsg;
            ReplyMsg = _replymsg;
        }

        public string Opc { get; set; }

        public string InteractiveMsg { get; set; }

        public string ReplyMsg { get; set; }
    }
}
