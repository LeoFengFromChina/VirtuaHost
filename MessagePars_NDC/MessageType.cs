using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessagePars_NDC
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
        HardwareError
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


    public class MessageParsRowView
    {
         public MessageParsRowView()
        {

        }
        public MessageParsRowView(string _name, string _value, string _comment)
        {
            FieldName = _name;
            FieldValue = _value;
            FieldComment = _comment;
        }
        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public string FieldComment { get; set; }
        

        //public int ArrayIndex { get; set; }

        //public int CharIndex { get; set; }

        //public int Length { get; set; }

    }

    public class TemplateView
    {
        public string FieldName { get; set; }
        public int FieldSize { get; set; }
        public Dictionary<string,string> FieldValue { get; set; }

    }
}
