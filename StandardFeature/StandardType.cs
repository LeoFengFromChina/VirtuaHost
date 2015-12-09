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
        PowerFailure
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
        /// 标识[消息的第4个分隔符内容]
        /// </summary>
        public string Identification { get; set; }

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

    public enum TcpHead
    {
        L2L1=0,
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

    public class Host
    {
        public int ID { get; set; }

        public XDCProtocolType ProtocolType { get; set; }

        public TcpHead TCPHead { get; set; }

        public ServerState State { get; set; }
    }
}
