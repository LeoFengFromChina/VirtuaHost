using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardFeature
{
    /// <summary>
    /// 主机状态
    /// </summary>
    public enum HostState
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow = 0,
        /// <summary>
        /// 正在通信
        /// </summary>
        Communicating,
        /// <summary>
        /// 等待readyB消息进入服务
        /// </summary>
        WaitForReadyToInservice,
        /// <summary>
        /// Inservice状态
        /// </summary>
        InService,
        /// <summary>
        /// Out-of-service状态
        /// </summary>
        OutOfService
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageCommandType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow = 0,
        /// <summary>
        /// 进入服务
        /// </summary>
        GoInService,
        /// <summary>
        /// 退出服务
        /// </summary>
        GoOutOfService,
        /// <summary>
        /// 断网
        /// </summary>
        GoOfLine,
        /// <summary>
        /// FulldownLoad
        /// </summary>
        FullDownLoad,
        /// <summary>
        /// NotFullDownLoad
        /// </summary>
        NotFullDownLoad,
        /// <summary>
        /// ReadyB
        /// </summary>
        ReadyB,
        /// <summary>
        /// CashHandler
        /// </summary>
        CashHandler,
        /// <summary>
        /// 交易请求消息
        /// </summary>
        TransactionMessage,
        /// <summary>
        /// 硬件故障
        /// </summary>
        HardwareError,
        /// <summary>
        /// PowerUp
        /// </summary>
        PowerFailure,
        #region DDC
        /// <summary>
        /// 双屏操作
        /// </summary>
        SupervisorAndSupplySwitchOFF,
        #endregion
        #region NDC
        /// <summary>
        /// 3,NDC,新密码验证
        /// </summary>
        NewKeyVerification,
        /// <summary>
        /// F,NDC,终端状态
        /// </summary>
        TerminalState,
        /// <summary>
        /// 配置信息
        /// </summary>
        DeviceFault
        #endregion
    }

    public enum MessageType
    {
        /// <summary>
        /// 30,其中0表示Response Flag
        /// </summary>
        DataCommand = 0,

        /// <summary>
        /// 10,其中0表示Response Flag
        /// </summary>
        TerminalCommand,

        /// <summary>
        /// 22||23
        /// </summary>
        SolicitedMessage,

        /// <summary>
        /// 12
        /// </summary>
        UnSolicitedMessage,

        /// <summary>
        /// 交互响应功能命令
        /// </summary>
        TransactionReplyCommand,

        /// <summary>
        /// 5
        /// </summary>
        ExitToHostMessages,

        /// <summary>
        /// 6，上传EJ日志的消息
        /// </summary>
        UploadEJMessage,

        /// <summary>
        /// 7
        /// </summary>
        HostToExitMessages,

        /// <summary>
        /// 未知
        /// </summary>
        Unknow
    }

    /// <summary>
    /// 消息格式
    /// </summary>
    public class XDCMessage
    {
        /// <summary>
        /// Luno号
        /// </summary>
        public string LUNO { get; set; }

        /// <summary>
        /// 消息类别，用于鉴别消息类型
        /// </summary>
        public MessageType MsgType { get; set; }

        /// <summary>
        /// 操作码
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// 金额域
        /// </summary>
        public string AmountField { get; set; }

        /// <summary>
        /// Pan
        /// </summary>
        public string PAN { get; set; }

        /// <summary>
        /// 标识[消息的第4个分隔符内容]
        /// </summary>
        public string Identification { get; set; }

        public char MsgCoodinationNumber { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageCommandType MsgCommandType { get; set; }

        /// <summary>
        /// 消息的ASCII字符串形式(用于显示和查看)
        /// </summary>
        public string MsgASCIIString { get; set; }

        /// <summary>
        /// 消息的Base64字符串形式(用于发给eCAT)
        /// </summary>
        public string MsgBase64String { get; set; }

        /// <summary>
        /// 消息的ASCII字符串格式化后的各域
        /// </summary>
        public string[] MsgASCIIStringFields { get; set; }

        /// <summary>
        /// 消息的字节数组
        /// </summary>
        public byte[] MsgByteArray { get; set; }
    }

    /// <summary>
    /// 格式化结果试图类
    /// </summary>
    public class ParsRowView
    {
        public ParsRowView()
        {

        }
        public ParsRowView(string _name, string _value, string _comment)
        {
            FieldName = _name;
            FieldValue = _value;
            FieldComment = _comment;
        }
        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public string FieldComment { get; set; }
    }

    /// <summary>
    /// 模板试图类
    /// </summary>
    public class TemplateView
    {
        public string FieldName { get; set; }
        public int FieldSize { get; set; }
        public Dictionary<string, string> FieldValue { get; set; }

    }

    /// <summary>
    /// 数据类型[状态，屏幕，FIT,消息]
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 状态
        /// </summary>
        State = 1,
        /// <summary>
        /// 屏幕
        /// </summary>
        Screen,
        /// <summary>
        /// Fit
        /// </summary>
        Fit,
        /// <summary>
        /// 消息
        /// </summary>
        Message
    }

    /// <summary>
    /// 协议类型
    /// </summary>
    public enum XDCProtocolType
    {
        /// <summary>
        /// DDC/NDC
        /// </summary>
        DDCorNDC = 0,
        /// <summary>
        /// NDC
        /// </summary>
        NDC = 1,
        /// <summary>
        /// DDC
        /// </summary>
        DDC = 2,
    }

    public enum CassetteStatus
    {
        /// <summary>
        /// 
        /// </summary>
        NoNewState,
        /// <summary>
        /// 
        /// </summary>
        Good,
        /// <summary>
        /// 
        /// </summary>
        NoteLow,
        /// <summary>
        /// 
        /// </summary>
        OutOfNote,
        /// <summary>
        /// 
        /// </summary>
        UnKnow
    }

    public enum CassetteSeverity
    {
        /// <summary>
        /// 
        /// </summary>
        NoError,
        /// <summary>
        /// 
        /// </summary>
        Routine,
        /// <summary>
        /// 
        /// </summary>
        Warning,
        /// <summary>
        /// 
        /// </summary>
        Suspend,
        /// <summary>
        /// 
        /// </summary>
        Fatal,
        /// <summary>
        /// 
        /// </summary>
        UnKnow
    }
    public enum TcpHead
    {
        L2L1 = 0,
        L1L2,
        L4L3L2L1,
        L1L2L3L4,
        NoHead
    }
    /// <summary>
    /// 五大模式
    /// </summary>
    public enum ServerState
    {
        PowerUp = 0,
        OffLine,
        OutOfService,
        InService,
        Maintance,

    }

    public static class XDCSplictorChar
    {
        public static Char FS = '\u001C';
        public static Char GS = '\u001D';
        public static Char RS = '\u001E';
        public static Char BlankRow_1 = '\u000D';
        public static Char BlankRow_2 = '\u000A';
        public static Char VT = '\n';
        public static Char VT2 = '\u000B';
    }

    public static class DDCdeviceID
    {
        public static string DP01 = "DP01";
        public static string CI01 = "CI01";
        public static string CR01 = "CR01";
        public static string DI01 = "DI01";
        public static string CN01 = "CN01";
        public static string AH01 = "AH01";
        public static string SD01 = "SD01";
        public static string CB4 = "CB4";
        public static string CB3 = "CB3";
        public static string CB2 = "CB2";
        public static string RE4 = "RE4";
        public static string RE3 = "RE3";
        public static string RE2 = "RE2";
        public static string A62 = "A62";
        public static string A63 = "A63";

        public static string B = "B";
        public static string C = "C";
        public static string D = "D";

        private static List<string> DeviceList = new List<string>() { DP01, CI01, CR01, DI01, CN01, AH01, SD01, CB4, CB3, CB2, RE4, RE3, RE2, A62, A63, B, C, D };
        public static bool CheckDeviceID(string containDeviceIDText, out string deviceID)
        {
            deviceID = null;
            foreach (string idItem in DeviceList)
            {
                if (containDeviceIDText.Contains(idItem))
                {
                    deviceID = idItem;
                    break;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 当前主机情况
    /// </summary>
    public class Host
    {
        public int ID { get; set; }

        public XDCProtocolType ProtocolType { get; set; }

        public TcpHead TCPHead { get; set; }

        public ServerState State { get; set; }

        public Cassette TypeA { get; set; }
        public Cassette TypeB { get; set; }
        public Cassette TypeC { get; set; }
        public Cassette TypeD { get; set; }
        public Cassette TypeE { get; set; }


        public Dictionary<string, OperationCode> OperationCodeDic { get; set; }
    }

    /// <summary>
    /// 钱箱类
    /// </summary>
    public class Cassette
    {
        public string CassetteName { get; set; }

        public int Denomination { get; set; }

        public CassetteStatus Status { get; set; }

        #region NDC

        public CassetteSeverity Severity { get; set; }

        public int Count { get; set; }

        #endregion

        #region DDC

        public int LoadCount { get; set; }

        public int Dispense { get; set; }

        public int Remain { get; set; }

        #endregion

    }

    public class OperationCode
    {
        public string Comment { get; set; }
        public string InteractiveReply { get; set; }
        public string CheckPin { get; set; }
        public string[] NextState { get; set; }
        public string[] FunctionIdentifier { get; set; }
        public string[] FunctionScreenNumber { get; set; }
        public string[] ScreenDisplayUpdate { get; set; }
        public string[] CardReturnFlag { get; set; }
        public string[] PrintData { get; set; }
        public string[] InterResponseDisplayFlag { get; set; }
        public string[] InterResponseActiveKeys { get; set; }
        public string[] InterResponseScreenTimer { get; set; }
        public string[] InterResponseScreenData { get; set; }
        public string[] GroupFunctionIdentifier { get; set; }
        public string[] OptionPrintData { get; set; }
        public string[] EnhancedFunction { get; set; }
    }

    public static class ResponseMessage
    {
        public static string Comment = "Comment";
        public static string InteractiveReply = "InteractiveReply";
        public static string CheckPin = "CheckPin";
        public static string NextState = "NextState";
        public static string FunctionIdentifier = "FunctionIdentifier";
        public static string FunctionScreenNumber = "FunctionScreenNumber";
        public static string ScreenDisplayUpdate = "ScreenDisplayUpdate";
        public static string CardReturnFlag = "CardReturnFlag";
        public static string PrintData = "PrintData";
        public static string InterResponseDisplayFlag = "InterResponseDisplayFlag";
        public static string InterResponseActiveKeys = "InterResponseActiveKeys";
        public static string InterResponseScreenTimer = "InterResponseScreenTimer";
        public static string InterResponseScreenData = "InterResponseScreenData";
        public static string GroupFunctionIdentifier = "GroupFunctionIdentifier";
        public static string OptionPrintData = "OptionPrintData";
        public static string EnhancedFunction = "EnhancedFunction";
    }

    public class NDCCassetteView
    {
        public NDCCassetteView()
        {

        }
        public NDCCassetteView(string _cassette, string _denomination, string _loadCount, string _status, string _severity)
        {
            Cassette = _cassette;
            Denomination = _denomination;
            LoadCount = _loadCount;
            Status = _status;
            Severity = _severity;
        }
        public string Cassette { get; set; }

        public string Denomination { get; set; }

        public string LoadCount { get; set; }

        public string Status { get; set; }

        public string Severity { get; set; }
    }
}
