using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlHelper;
using System.Xml;

namespace VirtualDualHost
{
    public static class MsgDebugOperator
    {

        /// <summary>
        /// 根据消息格式配置获取显示视图
        /// </summary>
        /// <param name="RowXml"></param>
        /// <param name="XDCmsg"></param>
        /// <returns></returns>
        public static List<MessageParsRowView> GetView(XDCMessage XDCmsg)
        {
            string RowXml = GetXMLConfig(XDCmsg);
            if (string.IsNullOrEmpty(RowXml))
            {
                return null;
            }
            string[] fields = RowXml.Split(';');
            List<MessageParsRowView> mprvlist = new List<MessageParsRowView>();
            for (int i = 0; i < XDCmsg.MsgASCIIStringFields.Length; i++)
            {
                foreach (string item in fields)
                {
                    string[] itemArray = item.TrimStart('[').TrimEnd(']').Split(',');
                    int FSIndex = int.Parse(itemArray[2]);
                    int CharIndex = int.Parse(itemArray[3]);
                    int Length = int.Parse(itemArray[4]);

                    if (FSIndex == i)
                    {
                        MessageParsRowView mprv = new MessageParsRowView();
                        mprv.FieldName = itemArray[0];
                        try
                        {
                            if (Length > -1)
                                mprv.FieldValue = XDCmsg.MsgASCIIStringFields[FSIndex].Substring(CharIndex, Length);
                            else
                            {
                                //var的情况，不确定长度的
                                string tempStr = XDCmsg.MsgASCIIStringFields[FSIndex];
                                mprv.FieldValue = tempStr.Substring(CharIndex, tempStr.Length - CharIndex );// XDCmsg.MsgASCIIStringFields[FSIndex].Substring(CharIndex, Length);
                            }
                        }
                        catch
                        {
                            //错误则不处理。
                        }

                        #region 关联备注

                        mprv.FieldComment = itemArray[1];
                        if (mprv.FieldComment.Length > 0)
                        {
                            if (mprv.FieldComment.StartsWith("[")
                                && mprv.FieldComment.EndsWith("]"))
                            {
                                string key = mprv.FieldComment.TrimStart('[').TrimEnd(']');
                                if (GlobalConfig.CommentDic.ContainsKey(key))
                                {
                                    Dictionary<string, string> CurStatus = GlobalConfig.CommentDic[key];
                                    if (null != mprv.FieldValue
                                       && CurStatus.ContainsKey(mprv.FieldValue))
                                        mprv.FieldComment = CurStatus[mprv.FieldValue];
                                }
                            }
                            else if (mprv.FieldComment.StartsWith("{")
                                && mprv.FieldComment.EndsWith("}"))
                            {
                                //内关联情况
                                string key = mprv.FieldComment.TrimStart('{').TrimEnd('}') + "-" + mprvlist[mprvlist.Count - 1].FieldValue;
                                if (GlobalConfig.CommentDic.ContainsKey(key))
                                {
                                    Dictionary<string, string> CurStatus = GlobalConfig.CommentDic[key];
                                    if (null != mprv.FieldValue
                                       && CurStatus.ContainsKey(mprv.FieldValue))
                                        mprv.FieldComment = CurStatus[mprv.FieldValue];
                                    else if (null == mprv.FieldValue
                                        && CurStatus.ContainsKey(""))
                                    {
                                        mprv.FieldComment = CurStatus[""];
                                    }
                                    else
                                    {
                                        mprv.FieldComment = "Error Data";
                                    }
                                }
                                else
                                {
                                    mprv.FieldComment = "";
                                }
                            }
                        }
                        #endregion

                        mprvlist.Add(mprv);
                    }
                }
                if (i != XDCmsg.MsgASCIIStringFields.Length - 1)
                    mprvlist.Add(new MessageParsRowView("FS", "", ""));
            }

            return mprvlist;
        }

        /// <summary>
        /// 根据消息类型获取对应的消息格式配置
        /// </summary>
        /// <param name="xdcMsg"></param>
        /// <returns></returns>
        private static string GetXMLConfig(XDCMessage xdcMsg)
        {
            string result = string.Empty;

            string xmlName = string.Empty;
            string xmlPath = string.Empty;
            string xx = xdcMsg.MsgType.ToString();
            switch (xdcMsg.MsgType)
            {
                case MessageType.DataCommand:
                    break;
                case MessageType.TerminalComman:
                    {

                        xmlName = "MessagePars";
                        xmlPath = "MessagePars.TerminalCommand.[*].{*}";

                        result = XMLHelper.instance.XMLFiles[xmlName].GetXmlAttributeValue(xmlPath);
                    }
                    break;
                case MessageType.SolicitedMessage_Status:
                    break;
                case MessageType.UnSolicitedMessage_Status:
                    {
                        xmlName = "MessagePars";
                        xmlPath = "MessagePars.UnSolicitedMessage." + xdcMsg.Identification + ".[*].{*}";

                        result = XMLHelper.instance.XMLFiles[xmlName].GetXmlAttributeValue(xmlPath);
                    }
                    break;
                case MessageType.SolicitedMessage_Encryptor:
                    break;
                case MessageType.Unknow:
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取所有关联的备注
        /// </summary>
        public static void GetGlobalComment()
        {
            XmlNode node = XMLHelper.instance.XMLFiles["Comments"].XmlDoc.SelectSingleNode("Comments");
            foreach (XmlNode item in node.ChildNodes)
            {
                string nodeName = item.Name;
                string path = "Comments." + nodeName + ".[*].{*}";
                string va = XMLHelper.instance.XMLFiles["Comments"].GetXmlAttributeValue(path);

                //[E,Cash Handle,];[B,Power Failure,]
                string[] statusArray = va.Split(';');
                Dictionary<string, string> newDic = new Dictionary<string, string>();
                foreach (string statusStr in statusArray)
                {
                    string statusTemp = statusStr.TrimStart('[').TrimEnd(']');
                    string[] tempArray = statusTemp.Split(',');
                    newDic.Add(tempArray[0], tempArray[1]);
                }

                GlobalConfig.CommentDic.Add(nodeName, newDic);
            }
        }
    }
}
