using StandardFeature;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePars_DDC
{
    public class MessageFormat_DDC : IMessageFormat
    {
        private Queue<string> _needSendToBothHost = new Queue<string>();
        public Queue<string> NeedSendToBothHost
        {
            get
            {
                return _needSendToBothHost;
            }
            set
            {
                _needSendToBothHost = value;
            }
        }

        public XDCMessage Format(byte[] msgByte, int msgLength, TcpHead HeadType = TcpHead.NoHead, bool isDebug = false)
        {
            XDCMessage result = new XDCMessage();

            try
            {
                #region EBCDIC
                long blong = 0;
                XDCUnity.PackMsgCode(ref msgByte, ref blong, "Recv", isDebug);
                #endregion
                //0.原字节数组
                result.MsgByteArray = msgByte;

                //2.ASCII字符串
                if (HeadType == TcpHead.L2L1)
                {
                    //L2L1，the len is in index 1
                    int megLen = msgByte[1] + (msgByte[0] << 8);
                    result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 2, megLen);
                    //result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 2, msgLength - 2);
                }
                else if (HeadType == TcpHead.L1L2)
                {
                    //L1L2, the len is in index 0
                    int megLen = msgByte[0] + (msgByte[1] << 8);
                    result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 2, megLen);
                    //result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 2, msgLength - 2);
                }
                else if (HeadType == TcpHead.L4L3L2L1)
                {
                    int megLen = msgByte[3] + (msgByte[2] << 8) + (msgByte[1] << 16) + (msgByte[0] << 24);
                    result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 4, megLen);
                    //result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 4, msgLength - 4);
                }
                else if (HeadType == TcpHead.L4L3L2L1_ASCII)
                {
                    byte[] m_head = new byte[] { msgByte[0], msgByte[1], msgByte[2], msgByte[3] };
                    int megLen = int.Parse(Encoding.ASCII.GetString(m_head));// msgByte[3] + (msgByte[2] << 8) + (msgByte[1] << 16) + (msgByte[0] << 24);
                    result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 4, megLen);
                    //result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 4, msgLength - 4);
                }
                else if (HeadType == TcpHead.L1L2L3L4)
                {
                    int megLen = msgByte[0] + (msgByte[1] << 8) + (msgByte[2] << 16) + (msgByte[3] << 24); ;
                    result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 4, megLen);
                }
                else
                    result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 0, msgLength);

                char MsgFS = '\u001C';//域分隔符
                string[] msgFields = result.MsgASCIIString.Split(MsgFS);

                //3.分解消息得各域的字符数组
                result.MsgASCIIStringFields = msgFields;

                //4.标识
                if (msgFields.Length > 3 && msgFields[3].Length > 0)
                    result.Identification = msgFields[3].Substring(0, 1);

                if (msgFields.Length > 4 && msgFields[4].Length > 0)
                    result.MsgCoodinationNumber = msgFields[4].Substring(1, 1)[0];

                //5.消息类别
                string tempField_0 = msgFields[0].Length > 3 ? msgFields[0].Substring(2, msgFields[0].Length - 2) : msgFields[0];
                result.MsgType = FormatHelper.ParsMessageClass(tempField_0);

                if (result.MsgType == MessageType.UnSolicitedMessage
                    && msgFields.Length > 7 && msgFields[7].Length > 0)
                {
                    //操作码
                    result.OperationCode = msgFields[7].Replace(" ", "_");
                    //金额域
                    result.AmountField = msgFields[8];
                    //pan
                    if (msgFields[5].IndexOf('=') > 0)
                        result.PAN = msgFields[5].Substring(1, msgFields[5].IndexOf('=') - 1);
                }
                string msgResult = string.Empty;
                result.MsgCommandType = MessageCommandType.Unknow;

                #region 5.判断消息类型

                switch (result.Identification)
                {
                    case "1":
                        {
                            if (result.MsgType == MessageType.TerminalCommand)
                                result.MsgCommandType = MessageCommandType.GoInService;
                            else if (result.MsgType == MessageType.UnSolicitedMessage)
                                result.MsgCommandType = MessageCommandType.PowerFailure;
                        }
                        break;
                    case "2":
                        {
                            if (result.MsgType == MessageType.TerminalCommand)
                                result.MsgCommandType = MessageCommandType.GoOutOfService;
                        }
                        break;
                    case "3":
                        {
                            if (result.MsgType == MessageType.UnSolicitedMessage)
                            {
                                if (msgFields[3].Length > 2 && "00" == msgFields[3].Substring(1, 2))
                                    result.MsgCommandType = MessageCommandType.SupervisorAndSupplySwitchOFF;
                            }
                        }
                        break;
                    case "8":
                        {
                            if (result.MsgType == MessageType.SolicitedMessage)
                                result.MsgCommandType = MessageCommandType.DeviceFault;
                            if (result.MsgType == MessageType.SolicitedMessage
                                && msgFields.Length > 4 && msgFields[4].Length > 4 /*&& msgFields[4].Substring(0, 2) == "E2"*/)
                            {
                                string DID = msgFields[4].Substring(0, 1);
                                string PartialDispenseStatus = msgFields[4].Substring(4, 1);
                                if (DID == "?" && PartialDispenseStatus != "0")
                                    result.MsgCommandType = MessageCommandType.Reversal;
                            }
                        }
                        break;
                    case "9":
                        {
                            if (result.MsgType == MessageType.SolicitedMessage)
                                result.MsgCommandType = MessageCommandType.ReadyB;
                        }
                        break;
                    case "B":
                        {
                            if (result.MsgType == MessageType.UnSolicitedMessage)
                            {
                                if (msgFields[3].Equals("B0000"))
                                    result.MsgCommandType = MessageCommandType.FullDownLoad;
                                else
                                    result.MsgCommandType = MessageCommandType.NotFullDownLoad;
                                NeedSendToBothHost.Enqueue(result.MsgASCIIString);
                            }
                        }
                        break;
                    case "E":
                        {
                            if (result.MsgType == MessageType.UnSolicitedMessage)
                            {
                                //cash handle
                                result.MsgCommandType = MessageCommandType.CashHandler;
                                NeedSendToBothHost.Enqueue(result.MsgASCIIString);
                            }
                        }
                        break;
                    case "":
                        {
                            result.MsgCommandType = MessageCommandType.TransactionMessage;
                        }
                        break;
                    default:
                        break;
                }

                #endregion

                //6.LUNO
                if (msgFields.Length > 1 && msgFields[1].Length > 0)
                {
                    if (msgFields[1].Length > 6)
                        result.LUNO = msgFields[1].Substring(0, 6);
                    else
                        result.LUNO = msgFields[1];
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(this.GetType().Name, "Format Error:" + ex.Message);
            }
            return result;
        }
    }

    public static class FormatHelper
    {
        /// <summary>
        /// 获取消息类型
        /// </summary>
        /// <param name="field_0"></param>
        /// <returns></returns>
        public static MessageType ParsMessageClass(string field_0)
        {
            MessageType result = MessageType.Unknow;
            switch (field_0)
            {
                case "10":
                case "1":
                    {
                        //DDC的Optional command 等同于NDC的TerminalCommand
                        result = MessageType.TerminalCommand;
                    }
                    break;
                case "11":
                case "12":
                    {

                        result = MessageType.UnSolicitedMessage;
                    }
                    break;
                case "22":
                    {
                        result = MessageType.SolicitedMessage;
                    }
                    break;
                case "3":
                case "30":
                    {
                        //DDC的WriteCommandMessage 等同于NDC的DataCommand
                        result = MessageType.DataCommand;
                    }
                    break;
                case "4":
                case "40":
                    {
                        //DDC的FunctionCommandMessage 等同于NDC的TransactionReplyCommand
                        result = MessageType.TransactionReplyCommand;
                    }
                    break;
                case "7":
                    {

                    }
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
