using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using StandardFeature;
using WeifenLuo.WinFormsUI.Docking;

namespace VirtualDualHost
{
    public partial class Form_DualHost : DockContent
    {
        public Form_DualHost()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        #region Base

        public delegate void SendMsgToeCAT(object socket, string msg);
        public static event SendMsgToeCAT SendMsgToeCATEvent;
        public static event SendMsgToeCAT SendMsgToeCATEvent_2;



        public delegate void SendMsgToGM(object socket, string msg);
        public static event SendMsgToGM SendMsgToGM01_Event;
        public static event SendMsgToGM SendMsgToGM02_Event;

        public delegate void GMReceiveMst(string msg);
        public static event GMReceiveMst ReceiveMsg_GM01;
        public static event GMReceiveMst ReceiveMsg_GM02;
        public static event GMReceiveMst ReceiveMsg_Unknow;


        static Socket socket_eCAT;
        static Socket socket_GM01;
        static Socket socket_GM02;

        static byte[] result_eCAT = new byte[2048];
        static byte[] result_GM01 = new byte[2048];
        static byte[] result_GM02 = new byte[2048];

        static IPAddress ip_eCAT = IPAddress.Parse("127.0.0.1");
        static int port_eCAT = 4070;
        static string LUNO_eCATBase = "";

        static IPAddress ip_GM01 = IPAddress.Parse("127.0.0.1");
        static int port_GM01 = 4071;
        static string LUNO_GM01 = "";
        static HostState GM01_HostState = HostState.Unknow;

        static IPAddress ip_GM02 = IPAddress.Parse("127.0.0.1");
        static int port_GM02 = 4072;
        static string LUNO_GM02 = "";
        static HostState GM02_HostState = HostState.Unknow;


        #endregion


        #region Control Event

        RichTextBox tb;
        private void Form1_Load(object sender, EventArgs e)
        {

            contextMenuStrip1.Items.Add("Clear");
            contextMenuStrip1.Items.Add("Pars");
            contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            contextMenuStrip1.Items[0].Click += Form_DualHost_Click;
            contextMenuStrip1.Items[1].Click += Form_DualHost_Click1;
            //msgDebugToolStripMenuItem.Click += MsgDebugToolStripMenuItem_Click;

            SendMsgToeCATEvent += new SendMsgToeCAT(Program_SendMsgToeCATEvent);
            SendMsgToGM01_Event += new SendMsgToGM(Program_SendMsgToGM01_Event);
            SendMsgToGM02_Event += new SendMsgToGM(Program_SendMsgToGM02_Event);
            SendMsgToeCATEvent_2 += new SendMsgToeCAT(Program_SendMsgToeCATEvent_2);

            ReceiveMsg_GM01 += new GMReceiveMst(Form1_ReceiveMsg_GM01);
            ReceiveMsg_GM02 += new GMReceiveMst(Form1_ReceiveMsg_GM02);
            ReceiveMsg_Unknow += new GMReceiveMst(Form_Main_ReceiveMsg_Unknow);
        }

        private void Form_DualHost_Click1(object sender, EventArgs e)
        {
            if (null != tb)
            {
                string msgText = string.Empty;
                msgText = tb.SelectedText;
                Form_MsgDebug debugForm = new Form_MsgDebug(msgText, XDCProtocolType.NDC);
                debugForm.Show();
            }
        }

        private void Form_DualHost_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (null != tb)
            {
                tb.Clear();
            }
        }

        private void MsgDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form debugForm = new Form_MsgDebug("", XDCProtocolType.NDC);
            debugForm.Show();
        }

        void Form_Main_ReceiveMsg_Unknow(string msg)
        {
            tssl_Status.Text = msg;
        }

        bool iseCATStart = false;
        private void btn_eCAT_Start_Click(object sender, EventArgs e)
        {
            if (!iseCATStart)
            {
                ip_eCAT = IPAddress.Parse(txt_eCAT_IP.Text.Trim());
                port_eCAT = int.Parse(txt_eCAT_Port.Text.Trim());
                LUNO_eCATBase = txt_eCAT_BaseLUNO.Text.Trim();
                txt_eCAT_IP.Enabled = false;
                txt_eCAT_Port.Enabled = false;
                txt_eCAT_BaseLUNO.Enabled = false;
                btn_eCAT_Start.Text = "Stop";
                iseCATStart = true;
                GM01_HostState = HostState.Unknow;
                ConnecteCAT();
            }
            else
            {
                btn_eCAT_Start.Text = "Start";
                txt_eCAT_IP.Enabled = true;
                txt_eCAT_Port.Enabled = true;
                txt_eCAT_BaseLUNO.Enabled = true;
                iseCATStart = false;
                eCATThread.Suspend();
                eCATThread.Abort();
                eCATThread = null;

                if (receiveThread != null)
                {
                    receiveThread.Suspend();
                    receiveThread.Abort();
                    receiveThread = null;
                }

                socket_eCAT.Close();
                socket_eCAT = null;

                if (clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket = null;
                }
                if (myClientSocket != null)
                {
                    myClientSocket.Close();
                    myClientSocket = null;
                }
            }
        }

        bool isGM01Start = false;
        private void btn_H1_Start_Click(object sender, EventArgs e)
        {
            if (!isGM01Start)
            {
                btn_H1_Start.Text = "Stop";
                txt_H1_IP.Enabled = false;
                txt_H1_Port.Enabled = false;
                txt_H1_LUNO.Enabled = false;
                isGM01Start = true;
                ip_GM01 = IPAddress.Parse(txt_H1_IP.Text.Trim());
                port_GM01 = int.Parse(txt_H1_Port.Text.Trim());
                LUNO_GM01 = txt_H1_LUNO.Text.Trim();
                GM01_HostState = HostState.Unknow;

                ConnectGM01();
            }
            else
            {
                btn_H1_Start.Text = "Start";
                GM01_HostState = HostState.Unknow;
                txt_H1_IP.Enabled = true;
                txt_H1_Port.Enabled = true;
                txt_H1_LUNO.Enabled = true;
                isGM01Start = false;
                GM01Thread.Suspend();
                GM01Thread = null;
                socket_GM01.Disconnect(false);
                socket_GM01 = null;

                if (isGM01Start)
                {
                    btn_H1_Start.PerformClick();
                }
            }
        }

        bool isGM02Start = false;
        private void btn_H2_Start_Click(object sender, EventArgs e)
        {
            if (!isGM02Start)
            {
                btn_H2_Start.Text = "Stop";
                txt_H2_IP.Enabled = false;
                txt_H2_Port.Enabled = false;
                txt_H2_LUNO.Enabled = false;
                isGM02Start = true;
                ip_GM02 = IPAddress.Parse(txt_H2_IP.Text.Trim());
                port_GM02 = int.Parse(txt_H2_Port.Text.Trim());
                LUNO_GM02 = txt_H2_LUNO.Text.Trim();
                GM01_HostState = HostState.Unknow;

                ConnectGM02();
            }
            else
            {
                btn_H2_Start.Text = "Start";
                GM01_HostState = HostState.Unknow;
                txt_H2_IP.Enabled = true;
                txt_H2_Port.Enabled = true;
                txt_H2_LUNO.Enabled = true;
                isGM02Start = false;
                GM02Thread.Suspend();
                GM02Thread = null;
                socket_GM02.Disconnect(false);
                socket_GM02 = null;

            }
        }

        private void rtb_GM01_Msg_TextChanged(object sender, EventArgs e)
        {
            rtb_GM01_Msg.SelectionStart = rtb_GM01_Msg.Text.Length;
            rtb_GM01_Msg.ScrollToCaret();
        }

        private void rtb_GM02_Msg_TextChanged(object sender, EventArgs e)
        {
            rtb_GM02_Msg.SelectionStart = rtb_GM02_Msg.Text.Length;
            rtb_GM02_Msg.ScrollToCaret();
        }

        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            tb = ((RichTextBox)((ContextMenuStrip)sender).SourceControl);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        #endregion

        #region Send Event

        void Program_SendMsgToGM02_Event(object socket, string msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                tempSock.Send(Convert.FromBase64String(msg));
            }
        }

        void Program_SendMsgToGM01_Event(object socket, string msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                tempSock.Send(Convert.FromBase64String(msg));
            }
        }

        void Program_SendMsgToeCATEvent(object socket, string msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                tempSock.Send(Convert.FromBase64String(msg));
                rtb_GM01_Msg.Text += "Send :" + Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2) + "\r\n";
            }
        }

        void Program_SendMsgToeCATEvent_2(object socket, string msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                tempSock.Send(Convert.FromBase64String(msg));
                rtb_GM02_Msg.Text += "Send :" + Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2) + "\r\n";
            }
        }

        void Form1_ReceiveMsg_GM02(string msg)
        {
            rtb_GM02_Msg.Text += "Recv :" + Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2) + "\r\n";
        }

        void Form1_ReceiveMsg_GM01(string msg)
        {
            rtb_GM01_Msg.Text += "Recv :" + Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2) + "\r\n";
        }

        #endregion

        #region eCAT
        static Thread eCATThread;
        static void ConnecteCAT()
        {

            socket_eCAT = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket_eCAT.Bind(new IPEndPoint(ip_eCAT, port_eCAT));
            socket_eCAT.Listen(10);
            Console.WriteLine("启动监听eCAT...");
            eCATThread = new Thread(eCAT_ListenClientConnect);
            eCATThread.Start();
        }

        static Socket clientSocket;
        static Thread receiveThread;
        private static void eCAT_ListenClientConnect()
        {

            while (true)
            {
                clientSocket = socket_eCAT.Accept();
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);

            }
        }
        static Socket myClientSocket;
        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)
        {
            myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    result_eCAT = new byte[2048];
                    int receiveNumber = myClientSocket.Receive(result_eCAT);
                    XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_eCAT, receiveNumber);
                    string msg = msgContent.MsgBase64String;
                    if (!string.IsNullOrEmpty(msg.TrimEnd('\0')))
                    {
                        #region 解析消息，判断LUNO号
                        //签到消息，还没有带terminalID,只有配置文件中的LUNO
                        if (msgContent.LUNO.Equals(LUNO_eCATBase))
                        {
                            if (GM01_HostState != HostState.InService)
                            {
                                //签到消息，分别发送给GM01和GM02
                                if (socket_GM01.Connected)
                                {
                                    ReceiveMsg_GM01(msg);
                                    SendMsgToGM01_Event(socket_GM01, msg);
                                }

                                if (GM01_HostState == HostState.WaitForReadyToInservice && msgContent.MsgCommandType == MessageCommandType.ReadyB)
                                {
                                    GM01_HostState = HostState.InService;
                                }
                            }
                            else
                            {
                                //发送给GM02
                                if (socket_GM02.Connected)
                                {
                                    if (XDCUnity.MessageFormat.NeedSendToBothHost != null && XDCUnity.MessageFormat.NeedSendToBothHost.Count > 0)
                                    {
                                        string QueueMsg = "";
                                        for (int i = 0; i < XDCUnity.MessageFormat.NeedSendToBothHost.Count; i++)
                                        {
                                            //消息队列里的消息是需要发送到各个主机的，如fulldownload消息，故障消息等。
                                            QueueMsg = XDCUnity.MessageFormat.NeedSendToBothHost.Dequeue();
                                            if (!string.IsNullOrEmpty(QueueMsg))
                                            {
                                                ReceiveMsg_GM02(QueueMsg);
                                                SendMsgToGM02_Event(socket_GM02, QueueMsg);
                                            }
                                        }
                                    }
                                    ReceiveMsg_GM02(msg);
                                    SendMsgToGM02_Event(socket_GM02, msg);
                                }
                            }

                        }
                        else if (msgContent.LUNO.Equals(LUNO_GM01))
                        {
                            ReceiveMsg_GM01(msg);
                            SendMsgToGM01_Event(socket_GM01, msg);
                        }
                        else if (msgContent.LUNO.Equals(LUNO_GM02))
                        {
                            ReceiveMsg_GM02(msg);
                            SendMsgToGM02_Event(socket_GM02, msg);
                        }
                        else
                        {
                            //捕获到未知去向消息
                            ReceiveMsg_Unknow("Receive Message :" + msg + " ,But unknow whice host to send.");
                        }

                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    ReceiveMsg_Unknow("接收eCAT消息出现异常:" + ex.Message.ToString());
                }
            }
        }
        #endregion

        #region GM01
        static Thread GM01Thread;
        static void ConnectGM01()
        {
            while (true)
            {
                try
                {
                    socket_GM01 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket_GM01.Connect(new IPEndPoint(ip_GM01, port_GM01));  //绑定IP地址：端口  
                    break;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }

            //Console.WriteLine("成功连接到GM-01");
            //通过Clientsoket发送数据  
            GM01Thread = new Thread(GM01_ListenClientConnect);
            GM01Thread.Start();
        }

        private static void GM01_ListenClientConnect()
        {
            while (true && socket_GM01 != null && socket_GM01.Connected)
            {
                int receiveLength = socket_GM01.Receive(result_GM01);
                XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_GM01, receiveLength);
                string msg = msgContent.MsgBase64String;

                if (!string.IsNullOrEmpty(msg.TrimEnd('\0')))
                {
                    while (true)
                    {
                        if (myClientSocket != null)
                        {
                            if (msgContent.MsgCommandType == MessageCommandType.GoInService
                                && GM01_HostState != HostState.InService)
                            {
                                //这条1消息代表GM01要进入服务了，但是此时不能进入，要等GM02就位了才能进入。
                                GM01_HostState = HostState.WaitForReadyToInservice;
                            }
                            SendMsgToeCATEvent(myClientSocket, msg);
                            break;
                        }
                        else
                        {
                            ReceiveMsg_Unknow("GM01-Connectting to eCAT...");
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }

        #endregion

        #region GM02
        static Thread GM02Thread;
        static void ConnectGM02()
        {
            //Console.WriteLine("正在连接GM-02....");
            while (true)
            {
                try
                {
                    socket_GM02 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket_GM02.Connect(new IPEndPoint(ip_GM02, port_GM02));  //绑定IP地址：端口  
                    break;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }

            //Console.WriteLine("成功连接到GM-02");
            //通过Clientsoket发送数据  
            GM02Thread = new Thread(GM02_ListenClientConnect);
            GM02Thread.Start();
        }

        private static void GM02_ListenClientConnect()
        {
            while (true && socket_GM02 != null && socket_GM02.Connected)
            {
                int receiveLength = socket_GM02.Receive(result_GM02);
                XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_GM02, receiveLength);
                string msg = msgContent.MsgBase64String;

                if (!string.IsNullOrEmpty(msg.TrimEnd('\0')))
                {
                    //Console.WriteLine("接收到GM-02消息：" + Encoding.ASCII.GetString(Convert.FromBase64String(msg)));

                    while (true)
                    {
                        //与eCAT连接了，并且，GM01已经进入服务了 
                        if (myClientSocket != null && GM01_HostState == HostState.InService)
                        {
                            SendMsgToeCATEvent_2(myClientSocket, msg);
                            break;
                        }
                        else
                        {
                            ReceiveMsg_Unknow("GM02-Connectting to eCAT...");
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }


        #endregion

        private void seteCATPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new From_Seeting_eCATPath().Show();
        }
    }

}
