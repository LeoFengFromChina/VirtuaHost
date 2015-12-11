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
    public partial class Form_NDCServer : DockContent
    {
        private static Host CurrentHostServer = new Host();
        public Form_NDCServer()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        static Thread eCATThread;
        static Socket socket_eCAT;
        static private Queue<string> currentFentch = new Queue<string>();
        static private Queue<string> currentFentchResponse = new Queue<string>();
        static private Queue<string> currentFullDownLoad = new Queue<string>();
        static int port_eCAT;
        static byte[] result_eCAT = new byte[2048];

        private static bool isFullDownLoad = false;
        private static bool isFencth = false;
        private static bool FencthFoundUnknow = false;
        private delegate void GMReceiveMst(string header, string msg);
        private static event GMReceiveMst ReceiveMsg;

        static string CurrentFencthResponse = string.Empty;

        string SendHead = "Send(";
        string RecvHead = "Recv(";
        #region Event

        private void Form_MainServer_Load(object sender, EventArgs e)
        {
            CurrentHostServer.ProtocolType = XDCProtocolType.DDC;
            CurrentHostServer.State = ServerState.OffLine;
            cmb_Header.SelectedIndex = 0;
            ReceiveMsg += new GMReceiveMst(Form1_ReceiveMsg);

            this.btn_Start.Click += Btn_Start_Click;
            this.btn_FetchConfig.Click += Btn_Start_Click;
            this.btn_FullDownLoad.Click += Btn_Start_Click;
            this.btn_ManuSendData.Click += Btn_Start_Click;
            this.btn_ClearLog.Click += Btn_Start_Click;
            //BaseFunction.Intial(XDCProtocolType.DDC, DataType.Message);
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            Button curButton = (Button)sender;
            switch (curButton.Name)
            {
                case "btn_Start":
                    {
                        isFullDownLoad = false;
                        isFencth = false;
                        FencthFoundUnknow = false;
                        int.TryParse(txt_Port.Text.Trim(), out port_eCAT);
                        if (txt_Port.Enabled)
                        {

                            if (socket_eCAT != null && socket_eCAT.LocalEndPoint.ToString().Contains(port_eCAT.ToString()))
                            {
                                lsb_Log.Items.Add("Error : Already Listen to Port: " + port_eCAT);
                                return;
                            }
                            //先清空
                            currentFentch.Clear();
                            currentFentchResponse.Clear();
                            GetFentch();
                            if (cmb_Header.SelectedIndex == 0)
                            {
                                CurrentHostServer.TCPHead = TcpHead.L2L1;
                            }
                            else
                            {
                                CurrentHostServer.TCPHead = TcpHead.NoHead;
                            }
                            lsb_Log.Items.Add("Start Server Port = " + txt_Port.Text);
                            //开启
                            ConnecteCAT();
                            BaseFunction.Intial(XDCProtocolType.NDC, DataType.Message);
                        }
                        else
                        {
                            //关闭
                            DisConnecteCAT();
                            lsb_Log.Items.Add("Close DisConnect.");
                        }

                        ControlsOperation.SetTextBoxEnable(txt_Port);
                        btn_Start.Text = ControlsOperation.GetCurrentButtonText(curButton);
                    }
                    break;
                case "btn_FetchConfig":
                    {

                    }
                    break;
                case "btn_FullDownLoad":
                    {
                        #region FullDownLoad
                        if (clientSocket != null && clientSocket.Connected && !FencthFoundUnknow)
                        {
                            currentFullDownLoad.Clear();
                            GetFullDownLoad();
                            isFullDownLoad = true;
                            //1.发送go-out-of-service消息
                            string out_of_service = currentFullDownLoad.Dequeue();
                            string headContext = string.Empty;
                            byte[] msgBytes = XDCUnity.EnPackageMsg(out_of_service, CurrentHostServer, ref headContext);
                            clientSocket.Send(msgBytes);
                            ReceiveMsg("Send(" + headContext + ") : ", out_of_service);
                        }

                        #endregion
                    }
                    break;
                case "btn_ManuSendData":
                    {

                    }
                    break;
                case "btn_ClearLog":
                    {
                        //rtb_Log.Clear();
                        lsb_Log.Items.Clear();
                    }
                    break;
                default:
                    break;
            }
        }

        void Form1_ReceiveMsg(string header, string msg)
        {
            if (txt_Port.Enabled)
            {
                return;
            }
            try
            {
                this.lsb_Log.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + header + Encoding.ASCII.GetString(Convert.FromBase64String(msg)).Substring(2));
            }
            catch
            {
                this.lsb_Log.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + header + msg);
            }
            this.lsb_Log.TopIndex = (int)(lsb_Log.Height / lsb_Log.ItemHeight);// lsb_Log.Items.Count - 1;// 
        }
        private void lsb_Log_Leave(object sender, EventArgs e)
        {
            lsb_Log.ClearSelected();
        }

        private void lsb_Log_DoubleClick(object sender, EventArgs e)
        {
            if (lsb_Log == null || lsb_Log.SelectedItem == null)
                return;
            string currentItemStr = lsb_Log.SelectedItem.ToString();
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

        #region Receive/Send_Message

        static void ConnecteCAT()
        {
            try
            {
                socket_eCAT = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket_eCAT.Bind(new IPEndPoint(IPAddress.Any, port_eCAT));
            }
            catch (Exception ex)
            {
                ReceiveMsg("", ex.Message);
                return;
            }
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
            CurrentHostServer.State = ServerState.OffLine;
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
                CurrentHostServer.State = ServerState.OutOfService;
                //有连接来了
                ReceiveMsg("", "New Connection " + clientSocket.RemoteEndPoint.ToString());

                GetFentch();

                //1.发送go-out-of-service消息
                isFencth = true;
                string out_of_service = currentFentch.Dequeue();
                CurrentFencthResponse = currentFentchResponse.Dequeue();
                string headContext = string.Empty;
                byte[] msgBytes = XDCUnity.EnPackageMsg(out_of_service, CurrentHostServer, ref headContext);
                clientSocket.Send(msgBytes);
                ReceiveMsg("Send(" + headContext + ") : ", out_of_service);

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
                    string msg = msgContent.MsgBase64String;

                    if (!string.IsNullOrEmpty(msgContent.MsgASCIIString.TrimEnd('\0')))
                    {
                        ReceiveMsg("Recv(" + (msgContent.MsgASCIIString.Length - 2).ToString().PadLeft(4, '0') + ") : ", msgContent.MsgBase64String);
                    }
                    if (msgContent.MsgCommandType == MessageCommandType.FullDownLoad)
                    {
                        isFullDownLoad = true;
                    }
                    string headContext = string.Empty;
                    if (isFullDownLoad)
                    {
                        if (msgContent.MsgCommandType != MessageCommandType.ReadyB)
                            continue;
                        #region FullDownLoad
                        if (currentFullDownLoad.Count == 0)
                        {
                            string inservicemsg = "10A210001";
                            byte[] msgBytes = XDCUnity.EnPackageMsg(inservicemsg, CurrentHostServer, ref headContext);
                            myClientSocket.Send(msgBytes);
                            ReceiveMsg("Send(" + headContext + "): ", inservicemsg);
                            isFullDownLoad = false;
                            Thread.Sleep(100);
                        }
                        else
                        {
                            string FulldownLoadMsg = string.Empty;
                            if (currentFullDownLoad.Count >= 1)
                            {
                                FulldownLoadMsg = currentFullDownLoad.Dequeue();
                            }
                            if (!string.IsNullOrEmpty(FulldownLoadMsg))
                            {
                                byte[] msgBytes = XDCUnity.EnPackageMsg(FulldownLoadMsg, CurrentHostServer, ref headContext);//Encoding.ASCII.GetBytes(FulldownLoadMsg);// 
                                myClientSocket.Send(msgBytes);
                                ReceiveMsg("Send(" + headContext + ") : ", FulldownLoadMsg);
                            }
                        }
                        #endregion
                    }
                    else if (isFencth)
                    {
                        if (msgContent.Identification == CurrentFencthResponse)
                        { //ndc的Fencth只有F，3消息才处理
                            #region Fencth

                            string fencthMsg = string.Empty;
                            if (currentFentch.Count >= 1)
                            {
                                fencthMsg = currentFentch.Dequeue();
                                CurrentFencthResponse = currentFentchResponse.Dequeue();
                            }
                            if (!string.IsNullOrEmpty(fencthMsg))
                            {
                                byte[] msgBytes = XDCUnity.EnPackageMsg(fencthMsg, CurrentHostServer, ref headContext);
                                myClientSocket.Send(msgBytes);
                                ReceiveMsg("Send(" + headContext + ") : ", fencthMsg);
                                Thread.Sleep(100);
                            }
                            if (currentFentch.Count <= 0
                                && CurrentHostServer.State == ServerState.OutOfService)
                            {
                                //已经是最后一条go-in-service了
                                CurrentHostServer.State = ServerState.InService;
                                isFencth = false;
                            }
                            #endregion}
                        }
                        else
                        {
                            FencthFoundUnknow = true;
                        }
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
            try
            {
                checktheloginuser = new Thread(new ThreadStart(iAmAWatcher));
                checktheloginuser.IsBackground = true;
                checktheloginuser.Start();
            }
            catch (Exception ex)
            {

            }
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

        static string[] FieldspliterStr = new string[] { "[FIELD]" };
        private static void GetFentch()
        {

            #region NDC

            if (XDCUnity.NDCFentchMessage.Count <= 0)
            {
                string path = System.Environment.CurrentDirectory + @"\Config\Server\NDC\Host_1\Raw\FentchConfig.txt";

                StreamReader sr = new StreamReader(path, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] FencthArray = line.Split(FieldspliterStr, StringSplitOptions.None);
                        XDCUnity.NDCFentchResponseMessage.Enqueue(FencthArray[0]);
                        XDCUnity.NDCFentchMessage.Enqueue(FencthArray[1]);
                    }
                }
                sr.Close();
                sr.Dispose();
            }
            currentFentch = XDCUnity.NDCFentchMessage;
            currentFentchResponse = XDCUnity.NDCFentchResponseMessage;
            #endregion

        }

        public static void GetFullDownLoad()
        {
            if (currentFullDownLoad.Count <= 0)
            {
                string path = System.Environment.CurrentDirectory + @"\Config\Server\NDC\Host_1\FullDownData\Cust.data";
                string fulldownLoadData = XDCUnity.GetTxtFileText(path);
                string[] dataArray = fulldownLoadData.Split(FieldspliterStr, StringSplitOptions.None);
                foreach (string dataItem in dataArray)
                {
                    if (!string.IsNullOrEmpty(dataItem))
                        currentFullDownLoad.Enqueue(dataItem);
                }
            }
        }
        #endregion

    }
}
