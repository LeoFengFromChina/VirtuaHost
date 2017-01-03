using MessagePars_NDC;
using MessagePars_DDC;
using StandardFeature;
using System;
using System.Collections.Generic;
using System.IO;
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

                XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "FixedMsg", item.Value.ReplyMsg, opcIniFilePath);
                richTextBox1.AppendText("FixedMsg=" + item.Value.ReplyMsg + "\r\n");

                if (!string.IsNullOrEmpty(item.Value.InteractiveMsg))
                {
                    XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "InteractiveReply", "1", opcIniFilePath);
                    richTextBox1.AppendText("InteractiveReply= \r\n");
                    XDCUnity.WriteIniData(item.Key.Replace(" ", "_"), "FixedInterActiveMsg", item.Value.InteractiveMsg, opcIniFilePath);
                    richTextBox1.AppendText("FixedInterActiveMsg=" + item.Value.InteractiveMsg + "\r\n");
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
            string preMsgHead = string.Empty;
            OperationCodeObject opcObject = null;
            string tempLine = string.Empty;
            OperationDic.Clear();
            while ((line = sr.ReadLine()) != null)
            {
                tempLine = "";
                //3类消息，交易请求、交互响应、交易回复
                if (!IsMsgWeNeed(preMsgHead) && !line.StartsWith("TRANSMIT: 11"))
                {
                    continue;
                }
                byte[] msgByteArray = null;
                XDCMessage XDCmsg = null;
                if (line.StartsWith("TRANSMIT: 11"))
                {

                    tempLine = line.Replace("TRANSMIT: ", "").TrimStart();
                    msgByteArray = Encoding.ASCII.GetBytes(tempLine);
                    XDCmsg = XDCUnity.MessageFormat.Format(msgByteArray, tempLine.Length);
                    if (!string.IsNullOrEmpty(XDCmsg.OperationCode))
                    {
                        preMsgHead = "11";
                    }
                    else
                    {
                        //上发的交互响应消息，跳过
                        continue;
                    }
                }
                else if (line.StartsWith("RECEIVE: 3") && IsMsgWeNeed(preMsgHead))
                {
                    //交互响应消息
                    tempLine = line.Replace("RECEIVE: ", "").TrimStart();
                }
                else if (line.StartsWith("RECEIVE: 4") && IsMsgWeNeed(preMsgHead))
                {
                    tempLine = line.Replace("RECEIVE: ", "").TrimStart();

                }



                //
                if (line.StartsWith("TRANSMIT: 11"))
                {
                    if (opcObject != null
                        && !string.IsNullOrEmpty(opcObject.Opc)
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
                    XDCmsg = XDCUnity.MessageFormat.Format(msgByteArray, tempLine.Length);
                    opcObject.Opc = XDCmsg.OperationCode;
                }
                else if (line.StartsWith("RECEIVE: 3"))
                {
                    if (opcObject != null
                        && string.IsNullOrEmpty(opcObject.InteractiveMsg))
                    {
                        tempLine = line.Replace("RECEIVE: ", "").TrimStart();
                        opcObject.InteractiveMsg = tempLine;
                    }
                    preMsgHead = "3";
                }
                else if (line.StartsWith("RECEIVE: 4"))
                {
                    if (opcObject != null)
                    {

                        tempLine = line.Replace("RECEIVE: ", "").TrimStart();
                        msgByteArray = Encoding.ASCII.GetBytes(tempLine);
                        XDCmsg = XDCUnity.MessageFormat.Format(msgByteArray, tempLine.Length);
                        XDCmsg.MsgASCIIStringFields[6] = "0" + XDCmsg.MsgASCIIStringFields[6].Substring(1);

                        tempLine = "";
                        foreach (string item in XDCmsg.MsgASCIIStringFields)
                        {
                            tempLine += item + XDCSplictorChar.FS.ToString();
                        }
                        opcObject.ReplyMsg = tempLine.Substring(0, tempLine.Length - 1);
                        preMsgHead = "4";
                    }
                }
                else
                {

                }

            }
            if (opcObject != null
                && !string.IsNullOrEmpty(opcObject.Opc))
            {
                //最后一条有可能没有保存进去。
                if (OperationDic.ContainsKey(opcObject.Opc))
                {
                    OperationDic[opcObject.Opc] = opcObject;
                }
                else
                {
                    OperationDic.Add(opcObject.Opc, opcObject);
                }
            }
        }

        private bool IsMsgWeNeed(string msgHead)
        {
            if (msgHead == "11" || msgHead == "4" || msgHead == "3")
                return true;
            return false;
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
