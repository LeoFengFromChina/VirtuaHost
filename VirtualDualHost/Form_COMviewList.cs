using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;
using System.IO;

namespace VirtualDualHost
{
    public partial class Form_COMviewList : DockContent
    {
        public delegate void SubForm(object dataContext, XDCProtocolType protocolType, DataType dataType);
        public event SubForm SubFormEvent;

        public delegate void BindDelegate(object msgList);
        public static event BindDelegate BindEvent;
        public Form_COMviewList()
        {
            InitializeComponent();
            BindEvent += Form_COMviewList_BindEvent;
        }

        private void Form_COMviewList_BindEvent(object msgList)
        {
            List<MessageView> MVList = msgList as List<MessageView>;
            dataGridView1.DataSource = MVList;

        }

        public static string comLogPath = string.Empty;
        Thread parsThread = null;
        public static string MsgHeaderRecv = "RECEIVE: ";
        public static string MsgHeaderTrans = "TRANSMIT: ";

        public void ParsMessage(string msgPath)
        {
            comLogPath = msgPath;
            //parsThread = new Thread(ParsFunc);
            //parsThread.Start();
            ParsFunc();
        }

        private static void ParsFunc()
        {
            //避免被占用就
            FileStream fs = new FileStream(comLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default); //new StreamReader( /*new StreamReader(comLogPath, Encoding.Default);*/
            String line;
            MessageView mv = null;
            DateTime dt = new DateTime();
            List<MessageView> MsgList = new List<MessageView>();
            string dateTemp = string.Empty;
            int dateStartChar;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    mv.DataC += XDCSplictorChar.BlankRow_1.ToString() + XDCSplictorChar.BlankRow_2.ToString();
                    continue;
                }
                dateTemp = line;
                if (int.TryParse(dateTemp.Substring(0, 1), out dateStartChar) && line.Contains(" "))
                    dateTemp = line.Substring(0, line.LastIndexOf(" "));
                if (DateTime.TryParse(dateTemp, out dt))
                {
                    if (mv != null)
                    {
                        MsgList.Add(mv);
                    }
                    //当前行为日期时间
                    mv = new MessageView();
                    mv.DateTimeC = line;
                }
                else if (line.StartsWith(MsgHeaderRecv)
                     || line.StartsWith(MsgHeaderTrans))
                {
                    //消息的开头行
                    mv.DataC = line;
                }
                else
                {
                    //带有换行的消息
                    mv.DataC += line;
                }
            }
            BindEvent(MsgList);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (null == dataGridView1.CurrentRow.Cells[1].Value)
                return;
            string msgContext = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //MessageBox.Show(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            msgContext = msgContext.Replace(MsgHeaderRecv, "").Replace(MsgHeaderTrans, "");
            SubFormEvent(msgContext, XDCProtocolType.DDCorNDC, DataType.Message);
        }
    }

    public class MessageView
    {
        public string DateTimeC { get; set; }
        public string DataC { get; set; }
    }
}
