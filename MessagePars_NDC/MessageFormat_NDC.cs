using StandardFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessagePars_NDC
{
    public class MessageFormat_NDC : IMessageFormat
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

        public XDCMessage Format(byte[] msgByte, int msgLength, TcpHead HeadType = TcpHead.NoHead)
        {
            XDCMessage result = new XDCMessage();

            //0.原字节数组
            result.MsgByteArray = msgByte;

            //1.Base64字符串
            //if (HeadType == TcpHead.L2L1)
            //    result.MsgBase64String = Convert.ToBase64String(msgByte, 2, msgLength - 2);
            //else
            //    result.MsgBase64String = Convert.ToBase64String(msgByte, 0, msgLength);

            //2.ASCII字符串
            if (HeadType == TcpHead.L2L1)
                result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 2, msgLength - 2);
            else
                result.MsgASCIIString = Encoding.ASCII.GetString(msgByte, 0, msgLength);

            char MsgFS = '\u001C';//域分隔符
            string[] msgFields = result.MsgASCIIString.Split(MsgFS);

            //3.分解消息得各域的字符数组
            result.MsgASCIIStringFields = msgFields;

            //4.标识
            if (msgFields.Length > 3 && msgFields[3].Length > 0)
                result.Identification = msgFields[3].Substring(0, 1);
            //5.消息类别
            string tempField_0 = msgFields[0].Length > 3 ? msgFields[0].Substring(2, msgFields[0].Length - 2) : msgFields[0];
            result.MsgType = FormatHelper.ParsMessageClass(tempField_0);

            if (result.MsgType == MessageType.UnSolicitedMessage
                && msgFields.Length > 7 && msgFields[7].Length > 0)
            {
                //操作码
                result.OperationCode = msgFields[7].Replace(" ", "_");
                //金额域
                if (msgFields.Length > 8)
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
                        //NDC
                        if (result.MsgType == MessageType.SolicitedMessage)
                            result.MsgCommandType = MessageCommandType.NewKeyVerification;
                        //NeedSendToBothHost.Enqueue(result.MsgBase64String);
                    }
                    break;
                case "8":
                    {
                        //NDC
                        if (result.MsgType == MessageType.SolicitedMessage)
                            result.MsgCommandType = MessageCommandType.DeviceFault;
                        if (msgFields.Length > 4 && msgFields[4].Length > 2 && msgFields[4].Substring(0, 2) == "E2")
                            result.MsgCommandType = MessageCommandType.Reversal;
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
                            {
                                result.MsgCommandType = MessageCommandType.FullDownLoad;
                                if (!XDCUnity.isFulldownLoadEnqueue)
                                {
                                    NeedSendToBothHost.Enqueue(result.MsgASCIIString);
                                    XDCUnity.isFulldownLoadEnqueue = true;
                                }
                            }
                            else
                                result.MsgCommandType = MessageCommandType.NotFullDownLoad;
                        }
                    }
                    break;
                case "E":
                    {
                        if (result.MsgType == MessageType.UnSolicitedMessage)
                        {
                            //cash handle
                            result.MsgCommandType = MessageCommandType.CashHandler;
                            //NeedSendToBothHost.Enqueue(result.MsgBase64String);
                        }
                    }
                    break;
                case "F":
                    {
                        //NDC
                        if (result.MsgType == MessageType.SolicitedMessage)
                        {
                            //TerminalState
                            result.MsgCommandType = MessageCommandType.TerminalState;
                            //NeedSendToBothHost.Enqueue(result.MsgBase64String);
                        }
                    }
                    break;
                case "P":
                    {
                        if (result.MsgType == MessageType.UnSolicitedMessage
                            && msgFields[3].Length > 2)
                        {
                            if ("20" == msgFields[3].Substring(1, 2))
                            {
                                result.MsgCommandType = MessageCommandType.SupervisorAndSupplySwitchOFF;
                            }
                            else if ("21" == msgFields[3].Substring(1, 2))
                            {
                                result.MsgCommandType = MessageCommandType.SupervisorAndSupplySwitchON;
                            }
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
            switch (field_0.Trim())
            {
                case "30":
                case "3":
                    {
                        result = MessageType.DataCommand;
                    }
                    break;
                case "1":
                case "10":
                    {
                        result = MessageType.TerminalCommand;
                    }
                    break;
                case "12":
                case "11":
                    {
                        result = MessageType.UnSolicitedMessage;
                    }
                    break;
                case "22":
                case "23":
                    {
                        result = MessageType.SolicitedMessage;
                    }
                    break;
                case "4":
                case "40":
                    {
                        result = MessageType.TransactionReplyCommand;
                    }
                    break;
                case "5":
                case "50":
                    {
                        result = MessageType.ExitToHostMessages;
                    }
                    break;
                case "6":
                case "60":
                    {
                        result = MessageType.UploadEJMessage;
                    }
                    break;
                case "7":
                case "70":
                    {
                        result = MessageType.HostToExitMessages;
                    }
                    break;
                case "8":
                case "80":
                    {
                        result = MessageType.EMVConfigMessages;
                    }
                    break;
                default:
                    break;
            }

            return result;

        }
    }
}
