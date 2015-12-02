using StandardFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessagePars_NDC
{
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
                default:
                    break;
            }

            return result;

        }
    }

}
