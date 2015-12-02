using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlHelper;
using System.Xml;
using StandardFeature;

namespace MessagePars_NDC
{
    public static class OperatorHelper
    {

        /// <summary>
        /// 获取xml节点列表
        /// </summary>
        /// <returns></returns>
        public static List<TemplateView> GetXmlConfig(XDCMessage CurrentMessage)
        {
            string[] tempArrary = CurrentMessage.MsgASCIIStringFields;
            XmlNode root = null;
            string attrID = "";
            string attrProtocolType = "1";
            string attrDataType = "4";
            XmlNode cur = null;
            string commandCode = "";

            string MessageIdentifier = "";
            int sdIndex = 3;
            switch (CurrentMessage.MsgType)
            {
                case MessageType.DataCommand:
                    {
                        #region 3x_DataCommand

                        commandCode = tempArrary[3].Substring(0, 1);
                        //TerminalCommand消息中，MessageIdentifier的值等于commandCode，因为
                        //TerminalCommand消息LUNO号后只有一个FS
                        MessageIdentifier = commandCode;
                        attrID = "3FSFSFS" + commandCode;
                        attrProtocolType = "1";
                        attrDataType = "4";
                        root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");
                        cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            //通常查询xml文件中ID=1的配置
                            attrID = tempArrary[0].Substring(0, 1);
                            cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        }
                        #endregion
                    }
                    break;
                case MessageType.TerminalCommand:
                    {
                        #region 1x_TerminalCommand

                        commandCode = tempArrary[3].Substring(0, 1);
                        //TerminalCommand消息中，MessageIdentifier的值等于commandCode，因为
                        //TerminalCommand消息LUNO号后只有一个FS
                        MessageIdentifier = commandCode;
                        attrID = "1FSFSFS" + commandCode;
                        attrProtocolType = "1";
                        attrDataType = "4";
                        root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");
                        cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            //通常查询xml文件中ID=1的配置
                            attrID = tempArrary[0].Substring(0, 1);
                            cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        }
                        #endregion
                    }
                    break;
                case MessageType.SolicitedMessage:
                    {
                        #region 22||23_SolicitedMessage

                        attrProtocolType = "1";
                        attrDataType = "4";
                        attrID = tempArrary[0] + "FSFSFS";
                        int fsCount = 3;
                        commandCode = tempArrary[sdIndex].Substring(0, 1);
                        if (tempArrary.Length > sdIndex + 1)
                            MessageIdentifier = tempArrary[sdIndex + 1].Substring(0, 1);
                        attrID += commandCode;
                        string tempXMLid = "";
                        string miString = "";
                        if (commandCode == "8"
                            || commandCode == "C"
                            || commandCode == "F")
                        {
                            miString = "FS" + MessageIdentifier;
                            fsCount++;
                        }

                        root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");
                        int maxFsCountRear = tempArrary.Length - fsCount - 1;
                        //最多的FS，并逐个递减
                        while (maxFsCountRear > -1)
                        {
                            tempXMLid = attrID + miString + GetFS(maxFsCountRear);
                            cur = GetNodeDetail(root, tempXMLid, attrProtocolType, attrDataType);
                            if (null != cur)
                            {
                                break;
                            }
                            maxFsCountRear--;
                        }
                        //如果还未空，去掉MessageIdentifier
                        if (null == cur)
                        {
                            tempXMLid = attrID;
                            cur = GetNodeDetail(root, tempXMLid, attrProtocolType, attrDataType);
                            if (null == cur)
                            {
                                tempXMLid = tempArrary[0];
                                cur = GetNodeDetail(root, tempXMLid, attrProtocolType, attrDataType);
                            }
                        }
                        #endregion
                    }
                    break;
                case MessageType.UnSolicitedMessage:
                    {
                        #region 12||11_UnSolicitedMessage
                        commandCode = !string.IsNullOrEmpty(tempArrary[3]) ? tempArrary[3].Substring(0, 1) : "";
                        attrID = tempArrary[0] + "FSFSFS" + commandCode;
                        if (tempArrary[0].Equals("11"))
                        {
                            MessageIdentifier = !string.IsNullOrEmpty(tempArrary[13]) ? tempArrary[13].Substring(0, 1) : "";
                        }
                        attrProtocolType = "1";
                        attrDataType = "4";
                        root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");
                        cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            attrID = tempArrary[0];
                            cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        }
                        #endregion
                    }
                    break;
                case MessageType.TransactionReplyCommand:
                    {
                        #region 4_TransactionReplyCommand

                        attrID = "4";
                        attrProtocolType = "1";
                        attrDataType = "4";
                        root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");
                        cur = GetNodeDetail(root, attrID, attrProtocolType, attrDataType);

                        #endregion
                    }
                    break;
                case MessageType.Unknow:
                    break;
                default:
                    break;
            }
            if (null == cur)
                return null;
            bool alreadyFS_X = true; ;
            List<TemplateView> TvList = new List<TemplateView>();
            foreach (XmlNode item in cur.ChildNodes)
            {
                TemplateView tv = new TemplateView();
                XmlAttribute fieldName = item.Attributes["Name"];
                XmlAttribute fieldSize = item.Attributes["Size"];

                if (fieldName != null)
                    tv.FieldName = fieldName.Value;
                if ((tv.FieldName.StartsWith("FS") && tv.FieldName.Length > 2)
                    || (tv.FieldName.StartsWith("GS") && tv.FieldName.Length > 2))
                {
                    if (tv.FieldName.Contains('|'))
                    {
                        bool isFoundInnerFS = false;
                        string[] fsSpliterArray = tv.FieldName.Replace("FS", "").Split('|');
                        foreach (string fsSpliterStr in fsSpliterArray)
                        {
                            if (MessageIdentifier == fsSpliterStr)
                            {
                                tv.FieldName = "FS";
                                isFoundInnerFS = true;
                                alreadyFS_X = true;
                                break;
                            }
                            else
                            {
                                isFoundInnerFS = false;
                            }
                        }
                        if (!isFoundInnerFS)
                            continue;
                    }
                    //已FS开头，但是不等於FS/GS的需要越过两行
                    else if (!tv.FieldName.Equals("FS" + MessageIdentifier)
                        && !tv.FieldName.Equals("GS" + MessageIdentifier))
                    {
                        alreadyFS_X = false;
                        continue;
                    }
                    else
                    {
                        tv.FieldName = tv.FieldName.Substring(0, 2);
                        alreadyFS_X = true;
                    }
                }
                else if (tv.FieldName.Equals("FS"))
                {
                    alreadyFS_X = true;
                }
                if (!alreadyFS_X)
                { continue; }

                if (fieldSize != null)
                {
                    int fSize;
                    int.TryParse(fieldSize.Value, out fSize);
                    tv.FieldSize = fSize;
                }
                if (item.HasChildNodes)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (XmlNode valueItem in item.ChildNodes)
                    {
                        XmlAttribute attrComment = valueItem.Attributes["Comment"];
                        //XmlAttribute attrOperation = valueItem.Attributes["Operation"];
                        //if (attrOperation != null)
                        //{
                        //    string tempOperation = attrOperation.Value;
                        //    //进行&运算

                        //}
                        if (null != attrComment
                            && !dic.ContainsKey(valueItem.InnerText))
                        {
                            dic.Add(valueItem.InnerText, attrComment.Value);
                        }
                    }
                    tv.FieldValue = dic;
                }
                TvList.Add(tv);
            }


            return TvList;
        }

        /// <summary>
        /// 根据给定的属性至获取指定的XML节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="keyID"></param>
        /// <param name="keyProrocolType"></param>
        /// <param name="keyDataType"></param>
        /// <returns></returns>
        private static XmlNode GetNodeDetail(XmlNode node, string keyID, string keyProrocolType, string keyDataType)
        {
            XmlNode result = null;
            XmlNodeList xnl = node.ChildNodes;
            foreach (XmlNode item in xnl)
            {
                if (item.Attributes["ID"] != null && item.Attributes["ID"].Value == keyID
                    && item.Attributes["ProtocolType"] != null && item.Attributes["ProtocolType"].Value == keyProrocolType
                    && item.Attributes["DataType"] != null && item.Attributes["DataType"].Value == keyDataType)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 结尾添加N个FS
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetFS(int count)
        {
            string resultStr = "";
            while (count > 0)
            {
                resultStr += "FS";
                count--;
            }
            return resultStr;
        }
    }
}
