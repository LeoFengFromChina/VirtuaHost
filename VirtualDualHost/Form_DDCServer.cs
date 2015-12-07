using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VirtualDualHost
{
    public partial class Form_DDCServer : DockContent
    {
        public Form_DDCServer()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        static Thread eCATThread;
        static Socket socket_eCAT;
        static private Queue<string> ddcFentch = new Queue<string>();
        static int port_eCAT;
        static byte[] result_eCAT = new byte[2048];

        public delegate void GMReceiveMst(string header, string msg);
        public static event GMReceiveMst ReceiveMsg;

        #region Event

        private void Form_DDCServer_Load(object sender, EventArgs e)
        {
            ReceiveMsg += new GMReceiveMst(Form1_ReceiveMsg);
            btn_Start.Click += Btn_Start_Click;
            btn_FetchConfig.Click += Btn_Start_Click;
            btn_FullDownLoad.Click += Btn_Start_Click;
            btn_ManuSendData.Click += Btn_Start_Click;
            BaseFunction.Intial(XDCProtocolType.DDC, DataType.Message);
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            Button curButton = (Button)sender;
            switch (curButton.Name)
            {
                case "btn_Start":
                    {
                        int.TryParse(txt_Port.Text.Trim(), out port_eCAT);
                        ControlsOperation.SetTextBoxEnable(txt_Port);
                        btn_Start.Text = ControlsOperation.GetCurrentButtonText(curButton);
                        if (!txt_Port.Enabled)
                        {
                            GetFentch();
                            //开启
                            ConnecteCAT();
                        }
                        else
                        {
                            //关闭
                            DisConnecteCAT();
                        }

                    }
                    break;
                case "btn_FetchConfig":
                    {
                        string temp = "OK";
                        byte[] tempByte = Encoding.ASCII.GetBytes(temp);
                        myClientSocket.Send(tempByte);
                    }
                    break;
                case "btn_FullDownLoad":
                    {

                    }
                    break;
                case "btn_ManuSendData":
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        void Form1_ReceiveMsg(string header, string msg)
        {
            try
            {
                rtb_Log.Text += "\r\n" + header + " :" + Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2) + "\r\n";
                //rtb_Log.Text += "Recv :" + msg + "\r\n";// Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2) + "\r\n";
            }
            catch
            {
                rtb_Log.Text += "\r\n" + header + " :" + msg;
            }

        }
        #endregion

        #region Receive/Send_Message

        static void ConnecteCAT()
        {

            socket_eCAT = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket_eCAT.Bind(new IPEndPoint(IPAddress.Any, port_eCAT));
            socket_eCAT.Listen(50);
            eCATThread = new Thread(eCAT_ListenClientConnect);
            eCATThread.IsBackground = true;
            eCATThread.Start();
        }
        static void DisConnecteCAT()
        {
            if (eCATThread != null)
            {
                if (eCATThread != null && eCATThread.ThreadState == ThreadState.Background)
                {
                    eCATThread.Suspend();
                    eCATThread = null;
                    //eCATThread.Abort();
                }

                stopWatcher();
            }
            if (clientSocket != null)
            {
                if (clientSocket.Connected)
                {
                    clientSocket.Disconnect(false);
                }
                clientSocket.Dispose();
                clientSocket = null;
            }
            if (socket_eCAT != null)
            {
                if (socket_eCAT.Connected)
                {
                    socket_eCAT.Disconnect(false);
                }
                socket_eCAT.Dispose();
                socket_eCAT = null;
            }

        }
        static Socket clientSocket;
        static Thread receiveThread;
        private static void eCAT_ListenClientConnect()
        {

            while (true)
            {
                if (socket_eCAT == null)
                    break; ;
                clientSocket = socket_eCAT.Accept();
                //有连接来了
                ReceiveMsg("", clientSocket.RemoteEndPoint.ToString() + " is Connected.");
                ddcFentch = XDCUnity.DDCFentchMessage;
                //1.发送go-out-of-service消息
                string out_of_service = ddcFentch.Dequeue();
                //byte[] tempByte = Encoding.Default.GetBytes(out_of_service);
                //XDCMessage msgContent = XDCUnity.MessageFormat.Format(tempByte, tempByte.Length);
                //clientSocket.Send(Convert.FromBase64String(msgContent.MsgBase64String));
                char headChar_1 = (char)0;
                char headChar_2 = (char)12;
                clientSocket.Send(Encoding.ASCII.GetBytes(headChar_1.ToString() + headChar_2.ToString() + out_of_service));

                ReceiveMsg("Send :", out_of_service);
                //2.心跳包
                LoadTheTimer();

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
                    int receiveNumber = 0;
                    receiveNumber = myClientSocket.Receive(result_eCAT);
                    XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_eCAT, receiveNumber);
                    string msg = msgContent.MsgBase64String;//Encoding.ASCII.GetString(result_eCAT, 0, receiveNumber); // 

                    if (!string.IsNullOrEmpty(msg.TrimEnd('\0')))
                    {
                        ReceiveMsg("Recv :", msgContent.MsgBase64String);
                    }
                    if (msgContent.MsgCommandType == MessageCommandType.ReadyB)
                    {
                        char headChar_1 = (char)0;
                        char headChar_2 = (char)12;
                        string fencthMsg = ddcFentch.Dequeue();
                        myClientSocket.Send(Encoding.ASCII.GetBytes(headChar_1.ToString() + headChar_2.ToString() + fencthMsg));
                        ReceiveMsg("Send :", fencthMsg);
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }


        #endregion

        #region HeartBreath
        static int loginedCount = 0;
        static System.Threading.Timer timer;
        static Thread checktheloginuser;
        //启动记时器
        public static void LoadTheTimer()
        {
            loginedCount = 0;
            object o = (object)loginedCount++;
            //暂时设定为1秒钟启动一次！
            timer = new System.Threading.Timer
            (new System.Threading.TimerCallback(watchTheLoginUser), o, 1000, 1000);
        }
        //启动监视"已登录用户通信情况"的线程
        public static void watchTheLoginUser(object o)
        {
            //UserPassport up=new UserPassport();
            checktheloginuser = new Thread(new ThreadStart(iAmAWatcher));
            checktheloginuser.IsBackground = true;
            checktheloginuser.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void iAmAWatcher()
        {
            if (clientSocket != null && clientSocket.Connected == false)
            {
                loginedCount++;
                //掉线了
                if (loginedCount <= 2)
                {
                    ReceiveMsg("", "Connection is Closed." + "\r\n");
                    clientSocket = null;
                    stopWatcher();
                }
            }
        }

        public static void stopWatcher()
        {
            if (checktheloginuser != null)
            {
                checktheloginuser.Abort();
            }
            if (timer != null)
                timer.Dispose();
        }
        #endregion

        #region Func
        private void GetFentch()
        {
            if (XDCUnity.DDCFentchMessage.Count <= 0)
            {
                string path = System.Environment.CurrentDirectory + @"\Config\Server\DDC\Host_1\Raw\FentchConfig.txt";

                StreamReader sr = new StreamReader(path, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        XDCUnity.DDCFentchMessage.Enqueue(line);
                    }
                }
            }
            ddcFentch = XDCUnity.DDCFentchMessage;
        }
        #endregion
    }
}
