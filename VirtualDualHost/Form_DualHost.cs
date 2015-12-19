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

        public delegate void SendMsgToeCAT(object socket, XDCMessage msg);
        public static event SendMsgToeCAT SendMsgToeCATEvent;
        public static event SendMsgToeCAT SendMsgToeCATEvent_2;



        public delegate void SendMsgToGM(object socket, string msg);
        public static event SendMsgToGM SendMsgToGM01_Event;
        public static event SendMsgToGM SendMsgToGM02_Event;

        public delegate void GMReceiveMst(XDCMessage msg);
        public static event GMReceiveMst ReceiveMsg_GM01;
        public static event GMReceiveMst ReceiveMsg_GM02;


        public delegate void GMReceivePureMst(string msg);
        public static event GMReceivePureMst ReceiveMsg_Unknow;

        public delegate void ButtonClick();
        public static event ButtonClick Button_Start_Click;

        public delegate void ConnectToGM();
        public static event ConnectToGM ConnectToGMEvent;
        public static event ConnectToGM DisConnectToGMEvent;

        static Socket socket_eCAT;
        static Socket socket_GM01;
        static Socket socket_GM02;

        static byte[] result_eCAT = new byte[2048];
        static byte[] result_GM01 = new byte[2048];
        static byte[] result_GM02 = new byte[2048];

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

        string SendHead = "Send(";
        string RecvHead = "Recv(";
        #endregion

        #region Control Event

        private void Form1_Load(object sender, EventArgs e)
        {
            SendMsgToeCATEvent += new SendMsgToeCAT(Program_SendMsgToeCATEvent);
            SendMsgToGM01_Event += new SendMsgToGM(Program_SendMsgToGM01_Event);
            SendMsgToGM02_Event += new SendMsgToGM(Program_SendMsgToGM02_Event);
            SendMsgToeCATEvent_2 += new SendMsgToeCAT(Program_SendMsgToeCATEvent_2);

            ReceiveMsg_GM01 += new GMReceiveMst(Form1_ReceiveMsg_GM01);
            ReceiveMsg_GM02 += new GMReceiveMst(Form1_ReceiveMsg_GM02);
            ReceiveMsg_Unknow += new GMReceivePureMst(Form_Main_ReceiveMsg_Unknow);

            lsb_Log_GM02.MouseDoubleClick += lsb_Log_GM01_MouseDoubleClick;
            Button_Start_Click += Form_DualHost_Button_Start_Click;
            ConnectToGMEvent += Form_DualHost_ConnectToGMEvent;
            DisConnectToGMEvent += Form_DualHost_DisConnectToGMEvent;
        }

        private void Form_DualHost_DisConnectToGMEvent()
        {
            DisConnectGM01Func();
            DisConnectGM02Func();
            IsLostConnectFromTerminal = true;
        }

        private void Form_DualHost_ConnectToGMEvent()
        {
            ConnectGM01Func();
            ConnectGM02Func();
            IsLostConnectFromTerminal = false;
        }

        public static bool IsLostConnectFromTerminal = false;
        private void Form_DualHost_Button_Start_Click()
        {
            btn_eCAT_Start.PerformClick();
            DisConnectGM01Func();
            DisConnectGM02Func();
        }

        void Form_Main_ReceiveMsg_Unknow(string msg)
        {
            tssl_Status.Text = msg;
        }

        bool iseCATStart = false;

        bool isGM01Start = false;
        static Thread GM01_StartThread;

        bool isGM02Start = false;
        static Thread GM02_StartThread;

        private void btn_eCAT_Start_Click(object sender, EventArgs e)
        {
            if (!iseCATStart)
            {
                #region eCAT

                port_eCAT = int.Parse(txt_eCAT_Port.Text.Trim());
                LUNO_eCATBase = txt_eCAT_BaseLUNO.Text.Trim();
                txt_eCAT_Port.Enabled = false;
                txt_eCAT_BaseLUNO.Enabled = false;
                btn_eCAT_Start.Text = "Stop";
                iseCATStart = true;
                GM01_HostState = HostState.Unknow;
                ConnecteCAT();

                #endregion

                #region GM01

                ConnectGM01Func();

                #endregion

                #region GM02
                ConnectGM02Func();
                #endregion

            }
            else
            {
                #region eCAT

                btn_eCAT_Start.Text = "Start";
                txt_eCAT_Port.Enabled = true;
                txt_eCAT_BaseLUNO.Enabled = true;
                iseCATStart = false;
                IsLostConnectFromTerminal = true;
                eCATThread.Suspend();
                eCATThread = null;

                if (receiveThread != null)
                {
                    receiveThread.Suspend();
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

                #endregion

                #region GM01

                DisConnectGM01Func();

                #endregion

                #region GM02

                DisConnectGM02Func();

                #endregion
            }
        }
        private void ConnectGM01Func()
        {
            //btn_H1_Start.Text = "Stop";
            txt_H1_IP.Enabled = false;
            txt_H1_Port.Enabled = false;
            txt_H1_LUNO.Enabled = false;
            isGM01Start = true;
            ip_GM01 = IPAddress.Parse(txt_H1_IP.Text.Trim());
            port_GM01 = int.Parse(txt_H1_Port.Text.Trim());
            LUNO_GM01 = txt_H1_LUNO.Text.Trim();
            GM01_HostState = HostState.Unknow;

            GM01_StartThread = new Thread(ConnectGM01);
            GM01_StartThread.IsBackground = true;
            GM01_StartThread.Start();
        }
        private void DisConnectGM01Func()
        {
            //btn_H1_Start.Text = "Start";
            GM01_HostState = HostState.Unknow;
            txt_H1_IP.Enabled = true;
            txt_H1_Port.Enabled = true;
            txt_H1_LUNO.Enabled = true;
            isGM01Start = false;
            if (GM01_ListenThread != null
                && GM01_ListenThread.ThreadState == System.Threading.ThreadState.Running)
                GM01_ListenThread.Suspend();
            GM01_ListenThread = null;

            if (GM01_StartThread.ThreadState == System.Threading.ThreadState.Running)
                GM01_StartThread.Suspend();
            GM01_StartThread = null;
            if (socket_GM01.Connected)
                socket_GM01.Disconnect(false);
            socket_GM01 = null;

            //if (isGM01Start)
            //{
            //    btn_H1_Start.PerformClick();
            //}
        }
        private void ConnectGM02Func()
        {
            //btn_H2_Start.Text = "Stop";
            txt_H2_IP.Enabled = false;
            txt_H2_Port.Enabled = false;
            txt_H2_LUNO.Enabled = false;
            isGM02Start = true;
            ip_GM02 = IPAddress.Parse(txt_H2_IP.Text.Trim());
            port_GM02 = int.Parse(txt_H2_Port.Text.Trim());
            LUNO_GM02 = txt_H2_LUNO.Text.Trim();
            GM01_HostState = HostState.Unknow;

            GM02_StartThread = new Thread(ConnectGM02);
            GM02_StartThread.IsBackground = true;
            GM02_StartThread.Start();
        }
        private void DisConnectGM02Func()
        {
            //btn_H2_Start.Text = "Start";
            GM01_HostState = HostState.Unknow;
            txt_H2_IP.Enabled = true;
            txt_H2_Port.Enabled = true;
            txt_H2_LUNO.Enabled = true;
            isGM02Start = false;
            if (GM02Thread != null
                && GM02Thread.ThreadState == System.Threading.ThreadState.Running)
                GM02Thread.Suspend();
            GM02Thread = null;
            if (GM02_StartThread != null
                && GM02_StartThread.ThreadState == System.Threading.ThreadState.Running)
                GM02_StartThread.Suspend();
            GM02_StartThread = null;
            if (socket_GM02.Connected)
                socket_GM02.Disconnect(false);
            socket_GM02 = null;
        }
        private void btn_H1_Start_Click(object sender, EventArgs e)
        {
            lsb_Log_GM01.Items.Clear();
        }

        private void btn_H2_Start_Click(object sender, EventArgs e)
        {
            lsb_Log_GM02.Items.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void seteCATPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new From_Seeting_eCATPath().Show();
        }

        private void lsb_Log_GM01_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListBox)sender) == null || ((ListBox)sender).SelectedItem == null)
                return;
            string currentItemStr = ((ListBox)sender).SelectedItem.ToString();
            string msg = string.Empty;
            int flagIndex = -1;
            if ((flagIndex = currentItemStr.IndexOf(SendHead)) >= 0)
            {
                msg = currentItemStr.Substring(flagIndex + 13, currentItemStr.Length - flagIndex - 13);
            }
            else if ((flagIndex = currentItemStr.IndexOf(RecvHead)) >= 0)
            {
                msg = currentItemStr.Substring(flagIndex + 13, currentItemStr.Length - flagIndex - 13);
            }

            Form_MsgDebug msd = new Form_MsgDebug(msg, XDCProtocolType.NDC);
            msd.Show();
        }

        #endregion

        #region Send Event

        void Program_SendMsgToGM02_Event(object socket, string msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                string headContext = string.Empty;
                byte[] msgBytes = XDCUnity.EnPackageMsg(msg, TcpHead.L2L1, ref headContext);
                tempSock.Send(msgBytes);
            }
        }

        void Program_SendMsgToGM01_Event(object socket, string msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                string headContext = string.Empty;
                byte[] msgBytes = XDCUnity.EnPackageMsg(msg, TcpHead.L2L1, ref headContext);
                tempSock.Send(msgBytes);
            }
        }

        void Program_SendMsgToeCATEvent(object socket, XDCMessage msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                string headContext = string.Empty;
                byte[] msgBytes = XDCUnity.EnPackageMsg(msg.MsgASCIIString, TcpHead.L2L1, ref headContext);
                tempSock.Send(msgBytes);
                this.lsb_Log_GM01.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + "Send(" + (msg.MsgASCIIString.Length).ToString().PadLeft(4, '0') + ") : " + msg.MsgASCIIString);
            }
            this.lsb_Log_GM01.TopIndex = lsb_Log_GM01.Items.Count - (int)(lsb_Log_GM01.Height / lsb_Log_GM01.ItemHeight);
        }

        void Program_SendMsgToeCATEvent_2(object socket, XDCMessage msg)
        {
            Socket tempSock = socket as Socket;
            if (tempSock != null)
            {
                string headContext = string.Empty;
                byte[] msgBytes = XDCUnity.EnPackageMsg(msg.MsgASCIIString, TcpHead.L2L1, ref headContext); //Encoding.ASCII.GetBytes(msg.MsgASCIIString);

                tempSock.Send(msgBytes);
                this.lsb_Log_GM02.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + "Send(" + (msg.MsgASCIIString.Length).ToString().PadLeft(4, '0') + ") : " + msg.MsgASCIIString);
            }

            this.lsb_Log_GM02.TopIndex = lsb_Log_GM02.Items.Count - (int)(lsb_Log_GM02.Height / lsb_Log_GM02.ItemHeight);
        }

        void Form1_ReceiveMsg_GM02(XDCMessage msg)
        {
            this.lsb_Log_GM02.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + "Recv(" + (msg.MsgASCIIString.Length).ToString().PadLeft(4, '0') + ") : " + msg.MsgASCIIString);

            this.lsb_Log_GM02.TopIndex = lsb_Log_GM02.Items.Count - (int)(lsb_Log_GM02.Height / lsb_Log_GM02.ItemHeight);
        }

        void Form1_ReceiveMsg_GM01(XDCMessage msg)
        {
            this.lsb_Log_GM01.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + "Recv(" + (msg.MsgASCIIString.Length).ToString().PadLeft(4, '0') + ") : " + msg.MsgASCIIString);

            this.lsb_Log_GM01.TopIndex = lsb_Log_GM01.Items.Count - (int)(lsb_Log_GM01.Height / lsb_Log_GM01.ItemHeight);
        }

        #endregion

        #region eCAT

        static Thread eCATThread;
        static void ConnecteCAT()
        {
            try
            {
                socket_eCAT = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket_eCAT.Bind(new IPEndPoint(IPAddress.Any, port_eCAT));
            }
            catch (Exception ex)
            {
                return;
            }
            socket_eCAT.Listen(50);
            eCATThread = new Thread(eCAT_ListenClientConnect);
            eCATThread.IsBackground = true;
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
                receiveThread.IsBackground = true;
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
                    if (IsLostConnectFromTerminal)
                    {
                        ConnectToGMEvent();

                    }
                    XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_eCAT, receiveNumber, TcpHead.L2L1);
                    string msg = msgContent.MsgASCIIString;
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
                                    ReceiveMsg_GM01(msgContent);
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
                                                XDCMessage mssg = XDCUnity.MessageFormat.Format(Encoding.ASCII.GetBytes(QueueMsg), Encoding.ASCII.GetBytes(QueueMsg).Length, TcpHead.L2L1);
                                                ReceiveMsg_GM02(mssg);
                                                SendMsgToGM02_Event(socket_GM02, QueueMsg);
                                            }
                                        }
                                    }
                                    ReceiveMsg_GM02(msgContent);
                                    SendMsgToGM02_Event(socket_GM02, msg);
                                }
                            }

                        }
                        else if (msgContent.LUNO.Equals(LUNO_GM01))
                        {
                            ReceiveMsg_GM01(msgContent);
                            SendMsgToGM01_Event(socket_GM01, msg);
                        }
                        else if (msgContent.LUNO.Equals(LUNO_GM02))
                        {
                            ReceiveMsg_GM02(msgContent);
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
                    ReceiveMsg_Unknow("Disconnect From Termnal.");
                    GM01_HostState = HostState.Unknow;
                    DisConnectToGMEvent();
                    break;
                }
            }
        }

        #endregion

        #region GM01

        static Thread GM01_ListenThread;
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

            GM01_ListenThread = new Thread(GM01_ListenClientConnect);
            GM01_ListenThread.Start();
        }

        private static void GM01_ListenClientConnect()
        {
            while (true && socket_GM01 != null && socket_GM01.Connected)
            {
                int receiveLength = socket_GM01.Receive(result_GM01);
                XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_GM01, receiveLength, TcpHead.L2L1);
                string msg = msgContent.MsgASCIIString;

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
                            SendMsgToeCATEvent(myClientSocket, msgContent);
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
                if (receiveLength <= 0)
                    continue;
                XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_GM02, receiveLength, TcpHead.L2L1);
                string msg = msgContent.MsgASCIIString;

                if (!string.IsNullOrEmpty(msg.TrimEnd('\0')))
                {

                    while (true)
                    {
                        //与eCAT连接了，并且，GM01已经进入服务了 
                        if (myClientSocket != null && myClientSocket.Connected
                            && GM01_HostState == HostState.InService)
                        {
                            SendMsgToeCATEvent_2(myClientSocket, msgContent);
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

    }

}
