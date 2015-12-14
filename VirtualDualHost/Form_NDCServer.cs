﻿using StandardFeature;
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
        static OperationCode CurrentOperationCode = new OperationCode();
        private static bool isFullDownLoad = false;
        private static bool isFencth = false;
        private static bool FencthFoundUnknow = false;
        public delegate void GMReceiveMst(string header, string msg);
        public static event GMReceiveMst ReceiveMsg;

        public delegate void ReBingCassetteStatus();
        public static event ReBingCassetteStatus ReBingCassette;

        public static List<NDCCassetteView> NDCCVList = new List<NDCCassetteView>();
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
            ReBingCassette += Form_NDCServer_ReBingCassette;
            this.btn_Start.Click += Btn_Start_Click;
            this.btn_FetchConfig.Click += Btn_Start_Click;
            this.btn_FullDownLoad.Click += Btn_Start_Click;
            this.btn_ManuSendData.Click += Btn_Start_Click;
            this.btn_ClearLog.Click += Btn_Start_Click;

            //Thread LoadBaseConfigThread = new Thread(LoadBaseConfig);
            //LoadBaseConfigThread.IsBackground = true;
            //LoadBaseConfigThread.Start();
        }

        private void Form_NDCServer_ReBingCassette()
        {
            if (dgv_Cassette.DataSource == null)
                dgv_Cassette.DataSource = NDCCVList;
            dgv_Cassette.Refresh();

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
                this.lsb_Log.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + header + Encoding.ASCII.GetString(Convert.FromBase64String(msg)));
            }
            catch
            {
                this.lsb_Log.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " :" + header + msg);
            }
            this.lsb_Log.TopIndex = lsb_Log.Items.Count - (int)(lsb_Log.Height / lsb_Log.ItemHeight);// lsb_Log.Items.Count - 1;// 
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
                InitialCassette();
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
                    CurrentOperationCode = new OperationCode();
                    XDCMessage msgContent = XDCUnity.MessageFormat.Format(result_eCAT, receiveNumber, TcpHead.L2L1);
                    string msg = msgContent.MsgBase64String;
                    if (msgContent.MsgCommandType == MessageCommandType.TerminalState)
                    {
                        //查看是否需要更新钱箱数据
                        CheckCassetteStatus(msgContent);
                        ReBingCassette();
                    }
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

                                ReBingCassette();
                            }
                            #endregion}
                        }
                        else
                        {
                            FencthFoundUnknow = true;
                        }
                    }
                    else if (!string.IsNullOrEmpty(msgContent.OperationCode))
                    {
                        string replyMsg = ProcessOperationCode(msgContent);
                        if (!string.IsNullOrEmpty(replyMsg))
                        {
                            byte[] msgBytes = XDCUnity.EnPackageMsg(replyMsg, CurrentHostServer, ref headContext);
                            myClientSocket.Send(msgBytes);
                            ReceiveMsg("Send(" + headContext + ") : ", replyMsg);
                            Thread.Sleep(100);
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

        public static string ProcessOperationCode(XDCMessage msgContent)
        {
            int resultIndex = 0;
            string msgTemplate = string.Empty;
            string path = System.Environment.CurrentDirectory + @"\Config\Server\NDC\Host_1\OperationCodeConfig.ini";

            CurrentOperationCode.Comment = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.Comment, string.Empty, path);
            if (!string.IsNullOrEmpty(CurrentOperationCode.Comment))
            {
                CurrentOperationCode.CheckPin = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.CheckPin, string.Empty, path);

                //获取消息模板
                string RPpath = System.Environment.CurrentDirectory + @"\Config\Server\NDC\Host_1\TransactionReply.ini";
                msgTemplate = XDCUnity.ReadIniData("Template", "Msg", string.Empty, RPpath);

                if (CurrentOperationCode.Comment.ToLower() == "pinentry" && !CheckUserInfo(msgContent))
                {
                    //当前为输入密码，但是用户信息又不匹配，退出
                    resultIndex = 1;
                }

                CurrentOperationCode.InteractiveReply = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.InteractiveReply, string.Empty, path);

                CurrentOperationCode.NextState = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.NextState, string.Empty, path).Split(';');
                CurrentOperationCode.FunctionIdentifier = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.FunctionIdentifier, string.Empty, path).Split(';');
                CurrentOperationCode.FunctionScreenNumber = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.FunctionScreenNumber, string.Empty, path).Split(';');
                CurrentOperationCode.ScreenDisplayUpdate = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.ScreenDisplayUpdate, string.Empty, path).Split(';');
                CurrentOperationCode.CardReturnFlag = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.CardReturnFlag, string.Empty, path).Split(';');
                CurrentOperationCode.PrintData = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.PrintData, string.Empty, path).Split(';');
                CurrentOperationCode.InterResponseDisplayFlag = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.InterResponseDisplayFlag, string.Empty, path).Split(';');
                CurrentOperationCode.InterResponseActiveKeys = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.InterResponseActiveKeys, string.Empty, path).Split(';');
                CurrentOperationCode.InterResponseScreenTimer = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.InterResponseScreenTimer, string.Empty, path).Split(';');
                CurrentOperationCode.InterResponseScreenData = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.InterResponseScreenData, string.Empty, path).Split(';');
                CurrentOperationCode.GroupFunctionIdentifier = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.GroupFunctionIdentifier, string.Empty, path).Split(';');
                CurrentOperationCode.OptionPrintData = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.OptionPrintData, string.Empty, path).Split(';');
                CurrentOperationCode.EnhancedFunction = XDCUnity.ReadIniData(msgContent.OperationCode, ResponseMessage.EnhancedFunction, string.Empty, path).Split(';');

                string printDataPath = System.Environment.CurrentDirectory + @"\Config\Server\NDC\Host_1\PrintData\";
                string screenDisplayUpdatePath = System.Environment.CurrentDirectory + @"\Config\Server\NDC\Host_1\ScreenUpdate\";


                string NotesToDispense = "00000000";
                if (CurrentOperationCode.Comment.ToLower().Contains("withdraw"))
                {
                    //取款配钞
                    NotesToDispense = GetNotesToDispense(msgContent);
                }
                else if (CurrentOperationCode.Comment.ToLower().Contains("deposit"))
                {
                    RecordUserInfo(msgContent, int.Parse(msgContent.AmountField));
                }

                string updateDataStr = XDCUnity.GetTxtFileText(screenDisplayUpdatePath + CurrentOperationCode.ScreenDisplayUpdate[resultIndex]);
                UpdateUserDataReplyToTerminal(msgContent, ref updateDataStr);
                string printData = XDCUnity.GetTxtFileText(printDataPath + CurrentOperationCode.PrintData[resultIndex]);

                msgTemplate = msgTemplate.Replace("[MsgClass]", "4")
                    .Replace("[ResponseFlag]", "")
                    .Replace("[LUNO]", "000")
                    .Replace("[MSN]", "1200")
                    .Replace("[NextStateID]", CurrentOperationCode.NextState[resultIndex])
                    .Replace("[NotesToDispense]", NotesToDispense)
                    .Replace("[TSN]", "1400")
                    .Replace("[FunctionId]", CurrentOperationCode.FunctionIdentifier[resultIndex])
                    .Replace("[ScreenNumber]", CurrentOperationCode.FunctionScreenNumber[resultIndex])
                    .Replace("[ScreenUpdateData]", updateDataStr)
                    .Replace("[Msg-Co-Number]", "0")
                    .Replace("[CardFlag]", CurrentOperationCode.CardReturnFlag[resultIndex])
                    .Replace("[PrintFlat]", "3")
                    .Replace("[PrintData]", printData);
            }
            return msgTemplate;
        }

        /// <summary>
        /// 配钞
        /// </summary>
        /// <param name="amountField"></param>
        /// <returns></returns>
        public static string GetNotesToDispense(XDCMessage msgContent)
        {
            string result = string.Empty;

            string tempAmount = string.Empty;

            int amount = msgContent.AmountField.Length == 12 ? int.Parse(msgContent.AmountField.Substring(0, msgContent.AmountField.Length - 2)) : int.Parse(msgContent.AmountField);

            int noteCount = amount / 100;

            #region 暂时先所有都从第一个钞箱出，日后完善配钞算法

            result = noteCount.ToString().PadLeft(2, '0');
            result = result.PadRight(8, '0');
            NDCCVList[0].LoadCount = (int.Parse(NDCCVList[0].LoadCount) - noteCount).ToString();
            ReBingCassette();
            #endregion

            RecordUserInfo(msgContent, -amount);

            return result;
        }


        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public static bool CheckUserInfo(XDCMessage msgContent)
        {
            bool result = false;

            string UserName = XDCUnity.ReadIniData(msgContent.PAN, "UserName", string.Empty, XDCUnity.UserInfoPath);
            result = string.IsNullOrEmpty(UserName) ? false : true;

            return result;
        }

        /// <summary>
        /// 更新回复消息中的用户信息（不写入本地）
        /// </summary>
        /// <param name="msgContent"></param>
        /// <param name="replyData"></param>
        public static void UpdateUserDataReplyToTerminal(XDCMessage msgContent, ref string replyData)
        {
            string UserName = XDCUnity.ReadIniData(msgContent.PAN, "UserName", string.Empty, XDCUnity.UserInfoPath);
            string Pan = XDCUnity.ReadIniData(msgContent.PAN, "Pan", string.Empty, XDCUnity.UserInfoPath);
            string Currency = XDCUnity.ReadIniData(msgContent.PAN, "Currency", string.Empty, XDCUnity.UserInfoPath);
            string availableBanlance = XDCUnity.ReadIniData(msgContent.PAN, "AvailableBalance", string.Empty, XDCUnity.UserInfoPath);
            replyData = replyData.Replace("[USERNAME]", UserName)
                                                .Replace("[CURRENCY]", Currency)
                                                .Replace("[AVAILABLEBAL]", availableBanlance);

        }


        public static void RecordUserInfo(XDCMessage msgContent, int changeAmount)
        {
            string availableBanlance = XDCUnity.ReadIniData(msgContent.PAN, "AvailableBalance", string.Empty, XDCUnity.UserInfoPath);

            double newBalance = double.Parse(availableBanlance);
            newBalance += changeAmount;
            XDCUnity.WriteIniData(msgContent.PAN, "AvailableBalance", newBalance.ToString(), XDCUnity.UserInfoPath);

        }

        public static void InitialCassette()
        {
            NDCCVList.Clear();
            NDCCVList.Add(new NDCCassetteView("TypeA", "100", "", "", ""));
            NDCCVList.Add(new NDCCassetteView("TypeB", "100", "", "", ""));
            NDCCVList.Add(new NDCCassetteView("TypeC", "100", "", "", ""));
            NDCCVList.Add(new NDCCassetteView("TypeD", "100", "", "", ""));

            ReBingCassette();
        }

        public static void CheckCassetteStatus(XDCMessage msgContent)
        {
            List<ParsRowView> view = XDCUnity.MessageOperator.GetView(msgContent);

            foreach (ParsRowView item in view)
            {
                #region LoadCout

                if (item.FieldName.StartsWith("Notes In Cassettes (cassette type 1"))
                {
                    NDCCVList[0].LoadCount = int.Parse(item.FieldValue).ToString();
                }
                else if (item.FieldName.StartsWith("Notes In Cassettes (cassette type 2"))
                {
                    NDCCVList[1].LoadCount = int.Parse(item.FieldValue).ToString();
                }
                else if (item.FieldName.StartsWith("Notes In Cassettes (cassette type 3"))
                {
                    NDCCVList[2].LoadCount = int.Parse(item.FieldValue).ToString();
                }
                else if (item.FieldName.StartsWith("Notes In Cassettes (cassette type 4"))
                {
                    NDCCVList[3].LoadCount = int.Parse(item.FieldValue).ToString();
                }
                #endregion

                #region Status
                //Type 1 Currency Cassettes   1 
                else if (item.FieldName.StartsWith("Type 1 Currency Cassettes"))
                {
                    NDCCVList[0].Status = item.FieldComment;
                }
                else if (item.FieldName.StartsWith("Type 2 Currency Cassettes"))
                {
                    NDCCVList[1].Status = item.FieldComment;
                }
                else if (item.FieldName.StartsWith("Type 3 Currency Cassettes"))
                {
                    NDCCVList[2].Status = item.FieldComment;
                }
                else if (item.FieldName.StartsWith("Type 4 Currency Cassettes"))
                {
                    NDCCVList[3].Status = item.FieldComment;
                }
                #endregion

                #region Severity
                else if (item.FieldName.StartsWith("Cassette type 1"))
                {
                    NDCCVList[0].Severity = item.FieldComment;
                }
                else if (item.FieldName.StartsWith("Cassette type 2"))
                {
                    NDCCVList[1].Severity = item.FieldComment;
                }
                else if (item.FieldName.StartsWith("Cassette type 3"))
                {
                    NDCCVList[2].Severity = item.FieldComment;
                }
                else if (item.FieldName.StartsWith("Cassette type 4"))
                {
                    NDCCVList[3].Severity = item.FieldComment;
                }
                #endregion
            }
        }
        #endregion

        private void dgv_Cassette_Leave(object sender, EventArgs e)
        {
            dgv_Cassette.ClearSelection();
        }
    }
}
