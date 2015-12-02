using StandardFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlHelper;

namespace MessagePars_NDC
{
    public class MessageOperator_NDC : IMessageOperator
    {
        public List<ParsRowView> GetView(XDCMessage XDCmsg)
        {
            List<TemplateView> xmlList = OperatorHelper.GetXmlConfig(XDCmsg);
            if (null == xmlList)
                return null;
            List<ParsRowView> mprvlist = new List<ParsRowView>();
            int i = 0;
            int J = 0;
            bool alreadyGSorFS = false;
            string alreadGSorFsSpliter = string.Empty;
            List<TemplateView> GSitemList = new List<TemplateView>();
            foreach (TemplateView item in xmlList)
            {
                if (item.FieldName == "FS")
                {
                    mprvlist.Add(new ParsRowView("FS", "", ""));
                    i = 0;
                    J++;
                    alreadyGSorFS = false;
                    alreadGSorFsSpliter = "";
                }
                else if (item.FieldName == "GS"
                    || item.FieldName == "FS*")
                {
                    alreadyGSorFS = true;
                    alreadGSorFsSpliter = item.FieldName.Substring(0, 2);
                }
                else
                {
                    //处理GS情况
                    if (alreadyGSorFS)
                    {
                        GSitemList.Add(item);
                        continue;
                    }

                    #region GS非结尾的情况

                    if (GSitemList.Count > 0)
                    {
                        Char spliterGS = '\u001D';
                        string[] GSstring;
                        if (alreadGSorFsSpliter == "GS")
                            GSstring = XDCmsg.MsgASCIIStringFields[J].Split(spliterGS);
                        else
                        {
                            GSstring = OperatorHelper.GetSubArray(XDCmsg.MsgASCIIStringFields, J + 1);
                        }
                        string tempValueGS = "";
                        string tempComment2 = "";
                        for (int k = alreadGSorFsSpliter == "GS" ? 1 : 0; k < GSstring.Length; k++)
                        {
                            if (null == (GSstring[k]))
                                break;
                            mprvlist.Add(new ParsRowView(alreadGSorFsSpliter, "", ""));
                            for (int l = 0; l < GSitemList.Count; l++)
                            {
                                if (GSitemList[l].FieldSize > 0)
                                {
                                    if (GSstring[k].Length >= GSitemList[l].FieldSize)
                                        tempValueGS = GSstring[k].Substring(l, GSitemList[l].FieldSize);
                                    else
                                    {
                                        tempValueGS = "";
                                    }
                                }
                                else
                                {
                                    tempValueGS = GSstring[k].Substring(l, GSstring[k].Length - l);
                                }
                                if (GSitemList[l].FieldValue != null
                                    && GSitemList[l].FieldValue.ContainsKey(tempValueGS))
                                    tempComment2 = GSitemList[l].FieldValue[tempValueGS];
                                else if (GSitemList[l].FieldValue != null
                                    && GSitemList[l].FieldValue.ContainsKey("*"))
                                    tempComment2 = GSitemList[l].FieldValue["*"];
                                else
                                {
                                    if (string.IsNullOrEmpty(tempValueGS))
                                        tempComment2 = "No Value";
                                    else
                                        tempComment2 = "";
                                }

                                mprvlist.Add(new ParsRowView(GSitemList[l].FieldName, tempValueGS, tempComment2));
                            }
                        }
                    }

                    GSitemList.Clear();
                    #endregion
                    string tempValue = string.Empty;
                    //域的值有指定长度的情况
                    if (item.FieldSize > 0)
                    {
                        try
                        {
                            tempValue = XDCmsg.MsgASCIIStringFields[J].Substring(i, item.FieldSize);
                            i += item.FieldSize;
                        }
                        catch
                        {
                            tempValue = "";
                            i += item.FieldSize;
                        }
                    }
                    else
                    {
                        try
                        {
                            tempValue = XDCmsg.MsgASCIIStringFields[J].Substring(i, XDCmsg.MsgASCIIStringFields[J].Length - i);
                            i += XDCmsg.MsgASCIIStringFields[J].Length - i;
                        }
                        catch
                        {
                            tempValue = "";
                            i += item.FieldSize;
                        }
                    }
                    string tempComment = string.Empty;

                    if (item.FieldValue != null)
                    {
                        if (string.IsNullOrEmpty(tempValue))
                        {
                            tempComment = "No Value";
                        }
                        else if (item.FieldValue.ContainsKey(tempValue))
                            tempComment = item.FieldValue[tempValue];
                        else if (item.FieldValue.ContainsKey("*"))
                            tempComment = item.FieldValue["*"];
                        else
                        {
                            bool isFind = false;
                            foreach (KeyValuePair<string, string> kvpItem in item.FieldValue)
                            {
                                //&运算
                                if (kvpItem.Key.Contains("&amp;"))
                                {
                                    isFind = true;
                                    string ampValue = kvpItem.Key.Substring(0, kvpItem.Key.IndexOf("&amp;"));
                                    string ampOperator = kvpItem.Key.Substring(kvpItem.Key.IndexOf("&amp;") + 5, kvpItem.Key.Length - kvpItem.Key.IndexOf("&amp;") - 5);
                                    int ampResult = int.Parse(tempValue) & int.Parse(ampOperator);
                                    if (ampResult.ToString() == ampValue)
                                    {
                                        tempComment += kvpItem.Value + ";";
                                    }
                                }
                            }
                            if (!isFind)
                                tempComment = "UnKnow Value";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(tempValue))
                            tempComment = "No Value";
                        else
                            tempComment = "";
                    }
                    mprvlist.Add(new ParsRowView(item.FieldName, tempValue, tempComment));

                }
            }
            #region GS结尾的情况

            if (GSitemList.Count > 0)
            {
                Char spliterGS = '\u001D';
                string[] GSstring;
                if (alreadGSorFsSpliter == "GS")
                    GSstring = XDCmsg.MsgASCIIStringFields[J].Split(spliterGS);
                else
                {
                    GSstring = OperatorHelper.GetSubArray(XDCmsg.MsgASCIIStringFields, J + 1);
                }
                string tempValueGS = "";
                string tempComment = "";
                for (int k = alreadGSorFsSpliter == "GS" ? 1 : 0; k < GSstring.Length; k++)
                {
                    if (null == (GSstring[k]))
                        break;
                    mprvlist.Add(new ParsRowView(alreadGSorFsSpliter, "", ""));
                    for (int l = 0; l < GSitemList.Count; l++)
                    {
                        if (GSitemList[l].FieldSize > 0)
                        {
                            if (GSstring[k].Length >= GSitemList[l].FieldSize)
                                tempValueGS = GSstring[k].Substring(l, GSitemList[l].FieldSize);
                            else
                            {
                                tempValueGS = "";
                            }
                        }
                        else
                        {
                            tempValueGS = GSstring[k].Substring(l, GSstring[k].Length - l);
                        }
                        if (GSitemList[l].FieldValue != null
                            && GSitemList[l].FieldValue.ContainsKey(tempValueGS))
                            tempComment = GSitemList[l].FieldValue[tempValueGS];
                        else if (GSitemList[l].FieldValue != null
                            && GSitemList[l].FieldValue.ContainsKey("*"))
                            tempComment = GSitemList[l].FieldValue["*"];
                        else
                        {
                            if (string.IsNullOrEmpty(tempValueGS))
                                tempComment = "No Value";
                            else
                                tempComment = "";
                        }

                        mprvlist.Add(new ParsRowView(GSitemList[l].FieldName, tempValueGS, tempComment));
                    }
                }
            }

            #endregion
            return mprvlist;
        }
    }
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
            //1NDC|2DDC
            string attrProtocolType = "1";
            //1State
            //2Screen
            //3Fit
            //4Message
            string attrDataType = "4";
            XmlNode cur = null;
            string commandCode = "";

            string MessageIdentifier = "";
            int sdIndex = 3;
            root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");
            switch (CurrentMessage.MsgType)
            {
                case MessageType.DataCommand:
                    {
                        #region 3x_DataCommand

                        commandCode = tempArrary[3].Substring(0, 1);
                        MessageIdentifier = commandCode;
                        attrID = "3FSFSFS" + commandCode;
                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            //通常查询xml文件中ID=1的配置
                            attrID = tempArrary[0].Substring(0, 1);
                            cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
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
                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            //通常查询xml文件中ID=1的配置
                            attrID = tempArrary[0].Substring(0, 1);
                            cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        }
                        #endregion
                    }
                    break;
                case MessageType.SolicitedMessage:
                    {
                        #region 22||23_SolicitedMessage
                        attrID = tempArrary[0] + "FSFSFS";
                        int fsCount = 3;
                        try
                        {
                            commandCode = tempArrary[sdIndex].Substring(0, 1);
                        }
                        catch
                        {
                            commandCode = "";
                        }
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

                        int maxFsCountRear = tempArrary.Length - fsCount - 1;
                        //最多的FS，并逐个递减
                        while (maxFsCountRear > -1)
                        {
                            tempXMLid = attrID + miString + XDCUnity.GetFS(maxFsCountRear);
                            cur = XDCUnity.GetNodeDetail(root, tempXMLid, attrProtocolType, attrDataType);
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
                            cur = XDCUnity.GetNodeDetail(root, tempXMLid, attrProtocolType, attrDataType);
                            if (null == cur)
                            {
                                tempXMLid = tempArrary[0];
                                cur = XDCUnity.GetNodeDetail(root, tempXMLid, attrProtocolType, attrDataType);
                            }
                        }
                        #endregion
                    }
                    break;
                case MessageType.UnSolicitedMessage:
                    {
                        #region 12||11_UnSolicitedMessage
                        try
                        {
                            commandCode = tempArrary[3].Substring(0, 1);
                        }
                        catch
                        {
                            commandCode = "";
                        }
                        attrID = tempArrary[0] + "FSFSFS" + commandCode;
                        if (tempArrary[0].Equals("11"))
                        {
                            try
                            {
                                MessageIdentifier = tempArrary[13].Substring(0, 1);
                            }
                            catch
                            {
                                MessageIdentifier = "";
                            }
                        }

                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            attrID = tempArrary[0];
                            cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        }
                        #endregion
                    }
                    break;
                case MessageType.TransactionReplyCommand:
                    {
                        #region 4_TransactionReplyCommand

                        attrID = "4";
                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);

                        #endregion
                    }
                    break;
                case MessageType.UploadEJMessage:
                    {
                        #region 6x_UploadEJMessage
                        try
                        {
                            commandCode = tempArrary[3].Substring(0, 1);
                            MessageIdentifier = commandCode;
                        }
                        catch
                        {
                            commandCode = "";
                            MessageIdentifier = commandCode;
                        }
                        attrID = tempArrary[0].Substring(0, tempArrary[0].Length);
                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            attrID = tempArrary[0].Substring(0, 1);
                            cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        }
                        #endregion
                    }
                    break;
                case MessageType.ExitToHostMessages:
                    {

                    }
                    break;
                case MessageType.HostToExitMessages:
                    {
                        #region 7_HostToExitMessages

                        attrID = tempArrary[0].Substring(0, 1);
                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);

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
                    else if (tv.FieldName.Equals("FS" + "*"))
                    {
                        //找到FS*
                        tv.FieldName = "FS*";
                        alreadyFS_X = true;
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
                    bool isNumeric = int.TryParse(fieldSize.Value, out fSize);
                    if (isNumeric)
                        tv.FieldSize = fSize;
                    else
                    {
                        //非数字的情况，如*
                        tv.FieldSize = -1;
                    }
                }
                if (item.HasChildNodes)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (XmlNode valueItem in item.ChildNodes)
                    {
                        XmlAttribute attrComment = valueItem.Attributes["Comment"];
                        XmlAttribute attrOperation = valueItem.Attributes["Operation"];
                        if (attrOperation != null)
                        {
                            //进行&运算
                            if (attrOperation.Value.StartsWith("&"))
                            {
                                //&运算
                                string ampValue = attrOperation.Value.Replace("&", "");
                                dic.Add(valueItem.InnerText + "&amp;" + ampValue, attrComment.Value);
                            }

                        }
                        else if (null != attrComment
                             && !dic.ContainsKey(valueItem.InnerText))
                        {
                            dic.Add(valueItem.InnerText, attrComment.Value);
                        }
                        tv.FieldSize = valueItem.InnerText.Length > tv.FieldSize ? valueItem.InnerText.Length : tv.FieldSize;
                    }
                    tv.FieldValue = dic;
                }
                TvList.Add(tv);
            }


            return TvList;
        }

        public static string[] GetSubArray(string[] sourceArray, int fromIndex)
        {
            string[] resultArray = null;

            List<string> listArray = new List<string>();
            for (int i = fromIndex; i < sourceArray.Length; i++)
            {
                listArray.Add(sourceArray[i]);
            }
            resultArray = listArray.ToArray<string>();


            return resultArray;
        }
    }
}
