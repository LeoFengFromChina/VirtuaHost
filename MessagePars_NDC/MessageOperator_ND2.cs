using StandardFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlHelper;

namespace MessagePars_NDC
{
    public class MessageOperator_NDC2 : IMessageOperator
    {
        public List<ParsRowView> GetView(XDCMessage XDCmsg)
        {
            List<ParsRowView> mprvlist = new List<ParsRowView>();
            OperatorHelper.CurrentNode = null;
            OperatorHelper.GetXmlConfig(XDCmsg);
            int tempXMLIndex = 0;
            for (int i = 0; i < XDCmsg.MsgASCIIStringFields.Length; i++)
            {
                List<ParsRowView> ilist = OperatorHelper.GetViewList(XDCmsg, XDCmsg.MsgASCIIStringFields[i], i, ref tempXMLIndex);
                if (ilist != null)
                    mprvlist.AddRange(ilist);

                if (i != XDCmsg.MsgASCIIStringFields.Length - 1)
                {
                    mprvlist.Add(new ParsRowView("FS", "", ""));
                }
            }
            return mprvlist;
        }
    }
    public static class OperatorHelper
    {

        public static XmlNode CurrentNode = null;
        /// <summary>
        /// 获取xml节点列表
        /// </summary>
        /// <returns></returns>
        public static void GetXmlConfig(XDCMessage CurrentMessage)
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
                case MessageType.EMVConfigMessages:
                    {
                        #region 8x_EmvConfigMessages

                        commandCode = tempArrary[2].Substring(0, 1);
                        MessageIdentifier = commandCode;
                        attrID = "8FSFS" + commandCode;
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
                case MessageType.Unknow:
                    break;
                default:
                    break;
            }

            CurrentNode = cur;
        }

        public static List<ParsRowView> GetViewList(XDCMessage XDCmsg, string msgCurrentFieldContent, int currentFSindex, ref int currXmlNodeIndex)
        {
            #region 修复12消息解析错误的问题

            //if (string.IsNullOrEmpty(msgCurrentFieldContent)
            //    && CurrentNode !=null && CurrentNode.ChildNodes.Count > currXmlNodeIndex)
            //{
            //    XmlAttribute fieldName = CurrentNode.ChildNodes[currXmlNodeIndex].Attributes["Name"];
            //    if (fieldName != null && fieldName.Value.StartsWith("FS"))
            //    {
            //        //解决存款消息中，310，311等buffer先生不正确的问题。edit by frde 20160106
            //        currXmlNodeIndex += 1;
            //        return null;
            //    }
            //}
            #endregion
            List<ParsRowView> rowViewList = new List<ParsRowView>();
            List<TemplateView> TvList = new List<TemplateView>();

            int FSCountIndex = 0;
            bool isFSrepeat = false;
            bool isFoundFS_x = false;
            bool isOccurFS_x = false;
            int tempIndex = currXmlNodeIndex;
            if (CurrentNode == null)
                return rowViewList;
            for (int i = tempIndex; i < CurrentNode.ChildNodes.Count; i++)
            {
                if (!isFSrepeat)
                    currXmlNodeIndex++;
                TemplateView tv = new TemplateView();
                XmlAttribute fieldName = CurrentNode.ChildNodes[i].Attributes["Name"];
                XmlAttribute fieldSize = CurrentNode.ChildNodes[i].Attributes["Size"];
                if (fieldName != null)
                {
                    if (fieldName.Value.ToString() == "FS")
                    {
                        isOccurFS_x = false;
                        FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);
                        break;

                    }
                    else if (fieldName.Value.ToString().StartsWith("FS"))
                    {
                        isOccurFS_x = true;
                        //if(string.IsNullOrEmpty(msgCurrentFieldContent))
                        //{
                        //    FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);
                        //    currXmlNodeIndex = currXmlNodeIndex - 1;
                        //    isOccurFS_x = false;
                        //    break;
                        //}
                        if (TvList != null && TvList.Count > 0)
                        {
                            FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);
                            currXmlNodeIndex = currXmlNodeIndex - 1;
                            isOccurFS_x = false;
                            break;
                        }
                        //FS*表示除了枚举的情况之外的所有通用情况
                        if (fieldName.Value == "FS*")
                        {
                            isOccurFS_x = false;
                            if (TvList != null && TvList.Count > 0)
                            {
                                FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);
                                break;
                            }

                        }
                        else if (fieldName.Value == "FS**")
                        {
                            currXmlNodeIndex = currXmlNodeIndex - 1;
                            isFSrepeat = true;

                            isOccurFS_x = false;
                            if (TvList != null && TvList.Count > 0)
                            {
                                FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);
                                break;
                            }
                        }
                        else
                        {
                            //如果当前消息域为空，且接下来都不是直接的FS，则直接跳过，XML节点下标向上跳回一个
                            //if (string.IsNullOrEmpty(msgCurrentFieldContent))
                            //{
                            //    currXmlNodeIndex = currXmlNodeIndex - 1;
                            //    break;
                            //    //isFoundFS_x = false;
                            //    //continue;
                            //}

                            isFoundFS_x = false;
                            string tempcontent = msgCurrentFieldContent;
                            string tempStringBehind = fieldName.Value.Substring(2, fieldName.Value.Length - 2);
                            string[] identifyArray = tempStringBehind.Split('|');
                            int tempLength = 0;
                            string tempIde = "";
                            foreach (string ideItem in identifyArray)
                            {
                                tempLength = ideItem.Length;
                                try
                                {
                                    tempIde = tempcontent.Substring(0, tempLength);
                                }
                                catch
                                {
                                    tempIde = "";
                                }
                                if (tempIde == ideItem)
                                {
                                    isFoundFS_x = true;
                                    TvList.Clear();
                                    break;
                                }
                            }
                            if (isFoundFS_x)
                            {
                                if (TvList != null && TvList.Count > 0)
                                {
                                    FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);

                                    TvList.Clear();
                                    break;
                                }
                                FSCountIndex++;
                            }
                            else
                            {
                            }
                        }
                    }
                    else
                    {

                        //出现FS_x但是又没匹配上，接下来的都跳过
                        if (isOccurFS_x && !isFoundFS_x)
                        {
                            continue;
                        }

                        #region 普通节点，加入列表中( 永远为当前一个FS中的内容 ）
                        tv.FieldName = fieldName.Value;
                        int size;
                        int.TryParse(fieldSize.Value, out size);
                        tv.FieldSize = size;
                        if (CurrentNode.ChildNodes[i].HasChildNodes)
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            foreach (XmlNode valueItem in CurrentNode.ChildNodes[i].ChildNodes)
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
                            }
                            tv.FieldValue = dic;
                        }
                        TvList.Add(tv);
                        #endregion
                    }
                }
                else
                {

                }
            }
            if (TvList.Count > 0)
            {
                FindFinalFS(ref rowViewList, ref TvList, ref isFoundFS_x, msgCurrentFieldContent);
                isOccurFS_x = false;
            }
            return rowViewList;
        }

        private static void FindFinalFS(ref List<ParsRowView> rowViewList, ref List<TemplateView> TvList, ref bool isFoundFS_x, string msgCurrentFieldContent)
        {
            string[] gsGroup = msgCurrentFieldContent.Split(XDCSplictorChar.GS);
            if (gsGroup.Length > 1)
            {
                //GS情况
                FindFinalGS(ref rowViewList, ref TvList, msgCurrentFieldContent);
                //SetFS_GS_(ref rowViewList, TvList, "GS", gsGroup);
            }
            else
            {
                #region 正是当前要找的FS

                int currentContentIndex = 0;
                foreach (TemplateView tvItem in TvList)
                {
                    if (tvItem.FieldName == "ICC data objects (and Further ICC data objects) requested by Central")
                    {
                        #region 生成IC卡数据提示行

                        ParsRowView ic = new ParsRowView();
                        ic.FieldName = "IC";
                        ic.FieldComment = tvItem.FieldName;
                        rowViewList.Add(ic);

                        #endregion

                        string tempvalueAll = msgCurrentFieldContent.Substring(currentContentIndex, msgCurrentFieldContent.Length - currentContentIndex);
                        string tempAll = tempvalueAll;
                        string key = "tagMeaning";
                        bool isFirst = true;

                        string sub_4 = tempAll.Substring(0, 4);
                        string sub_2 = tempAll.Substring(0, 2);
                        string tagIniPath = XDCUnity.CurrentPath + @"\Config\Protocol\TagList.ini";
                        string tagMeaning = XDCUnity.ReadIniData(sub_4, key, "", tagIniPath);
                        int currentLen = -1;
                        string tagName = string.Empty;
                        while (tempAll.Length > 0)
                        {
                            if (!isFirst)
                            {
                                ParsRowView tg = new ParsRowView();
                                tg.FieldName = "TG";
                                rowViewList.Add(tg);
                            }
                            isFirst = false;
                            sub_4 = tempAll.Substring(0, 4);
                            sub_2 = tempAll.Substring(0, 2);
                            tagMeaning = XDCUnity.ReadIniData(sub_4, key, "", tagIniPath);
                            currentLen = -1;
                            tagName = string.Empty;
                            if (!string.IsNullOrEmpty(tagMeaning))
                            {
                                currentLen = 4;
                                tagName = sub_4;
                            }
                            else
                            {
                                //长度为4的tag找不到，找长度为2的。
                                tagMeaning = XDCUnity.ReadIniData(sub_2, key, "", tagIniPath);
                                if (string.IsNullOrEmpty(tagMeaning))
                                {
                                    break;
                                }
                                currentLen = 2;
                                tagName = sub_2;
                            }

                            #region 增加行
                            //tagName
                            tempAll = tempAll.Substring(currentLen, tempAll.Length - currentLen);
                            ParsRowView prv_tagName = new ParsRowView();
                            prv_tagName.FieldName = "TagName";
                            prv_tagName.FieldComment = tagMeaning;
                            prv_tagName.FieldValue = tagName;
                            rowViewList.Add(prv_tagName);

                            //tagLen
                            string len = tempAll.Substring(0, 2);
                            tempAll = tempAll.Substring(2, tempAll.Length - 2);
                            ParsRowView prv_tagLen = new ParsRowView();
                            prv_tagLen.FieldName = "ValueLength";
                            prv_tagLen.FieldComment = "Hex";
                            prv_tagLen.FieldValue = len;
                            rowViewList.Add(prv_tagLen);

                            //tagValue
                            int valueLen = Convert.ToInt32(len, 16);
                            string value = tempAll.Substring(0, valueLen * 2);
                            tempAll = tempAll.Substring(valueLen * 2, tempAll.Length - valueLen * 2);
                            ParsRowView prv_tagValue = new ParsRowView();
                            prv_tagValue.FieldName = "Value";
                            prv_tagValue.FieldComment = "";
                            prv_tagValue.FieldValue = value;
                            rowViewList.Add(prv_tagValue);
                            #endregion
                        }
                    }
                    else
                    {
                        #region MyRegion
                        ParsRowView prv = new ParsRowView();
                        prv.FieldName = tvItem.FieldName;
                        //加上prv.FieldName == "GS**"的判断，解决显示GS**行而内容又为空的情况。edit by frde 20160106
                        if (/*(prv.FieldName == "GS" || prv.FieldName == "GS**")*/
                            prv.FieldName.StartsWith("GS")
                           /* && msgCurrentFieldContent.Length == currentContentIndex*/)
                        {
                            break;
                        }
                        try
                        {
                            if (tvItem.FieldSize <= 0)
                            {
                                prv.FieldValue = msgCurrentFieldContent.Substring(currentContentIndex, msgCurrentFieldContent.Length - currentContentIndex);
                            }
                            else
                                prv.FieldValue = msgCurrentFieldContent.Substring(currentContentIndex, tvItem.FieldSize);
                        }
                        catch
                        {
                            prv.FieldValue = "";
                        }
                        if (tvItem.FieldValue != null)
                        {
                            if (string.IsNullOrEmpty(prv.FieldValue))
                            {
                                prv.FieldComment = "";
                            }
                            else if (tvItem.FieldValue.ContainsKey(prv.FieldValue))
                                prv.FieldComment = tvItem.FieldValue[prv.FieldValue];
                            else if (tvItem.FieldValue.ContainsKey("*"))
                                prv.FieldComment = tvItem.FieldValue["*"];
                            else
                            {
                                bool isFind = false;
                                foreach (KeyValuePair<string, string> kvpItem in tvItem.FieldValue)
                                {
                                    //&运算
                                    if (kvpItem.Key.Contains("&amp;"))
                                    {
                                        try
                                        {

                                            isFind = true;
                                            string ampValue = kvpItem.Key.Substring(0, kvpItem.Key.IndexOf("&amp;"));
                                            string ampOperator = kvpItem.Key.Substring(kvpItem.Key.IndexOf("&amp;") + 5, kvpItem.Key.Length - kvpItem.Key.IndexOf("&amp;") - 5);
                                            int ampResult = int.Parse(prv.FieldValue) & int.Parse(ampOperator);
                                            if (ampResult.ToString() == ampValue)
                                            {
                                                prv.FieldComment += kvpItem.Value + ";";
                                            }
                                        }
                                        catch
                                        {
                                            prv.FieldComment = "";
                                        }
                                    }
                                }
                                if (!isFind)
                                    prv.FieldComment = "UnKnow Value";
                            }
                        }
                        else
                        {
                            prv.FieldComment = "";
                        }
                        rowViewList.Add(prv);

                        if (tvItem.FieldSize <= 0)
                        {
                            currentContentIndex += msgCurrentFieldContent.Length - currentContentIndex;
                        }
                        else
                            currentContentIndex += tvItem.FieldSize;
                        #endregion
                    }
                }
                TvList.Clear();
                #endregion
            }
            TvList.Clear();
            isFoundFS_x = false;
        }

        private static void FindFinalGS(ref List<ParsRowView> rowViewList, ref List<TemplateView> TvList, string msgCurrentFieldContent)
        {
            string[] gsGroup = msgCurrentFieldContent.Split(XDCSplictorChar.GS);
            int dataArrayIndex = 0;
            string tempDataValue = gsGroup[dataArrayIndex];
            string deviceID = null;
            bool isFoundGS = false;
            bool occurGS = false;
            int currentContextIndex = 0;
            int xmlIndex = 0;
            bool emptyAfterVT = false;
            bool isVTRepeat = false;
            int vtRepeatXMLnodeCount = 0;
            foreach (string gsDataItem in gsGroup)
            {
                DDCdeviceID.CheckDeviceID(gsDataItem, out deviceID);
                isFoundGS = false;
                occurGS = false;
                currentContextIndex = 0;
                string TempgsDataItem = gsDataItem;
                string[] rsDataItemList = gsDataItem.Split(XDCSplictorChar.RS);
                foreach (string rsData in rsDataItemList)
                {
                    vtRepeatXMLnodeCount = 0;
                    isVTRepeat = false;
                    string[] vtDataArray;
                    //if (rsData.Contains(XDCSplictorChar.VT2))
                    vtDataArray = rsData.Split(XDCSplictorChar.VT2);
                    //else
                    //    vtDataArray = rsData.Split(XDCSplictorChar.VT);
                    foreach (string vtDataItem in vtDataArray)
                    {
                        vtRepeatXMLnodeCount = 0;
                        deviceID = "";
                        currentContextIndex = 0;
                        DDCdeviceID.CheckDeviceID(vtDataItem, out deviceID);
                        for (int i = xmlIndex; i < TvList.Count; i++)
                        {
                            if (vtDataItem.Length > 0 && (currentContextIndex > vtDataItem.Length - 1)
                                && (TvList[i].FieldName.StartsWith("GS")
                                || TvList[i].FieldName.StartsWith("RS")
                                 || TvList[i].FieldName.StartsWith("VT")))
                            {
                                if (!TvList[i].FieldName.StartsWith("GS"))
                                {

                                }
                                if (isVTRepeat)
                                {
                                    xmlIndex = xmlIndex - vtRepeatXMLnodeCount - 1;
                                    //vtRepeatXMLnodeCount = 0;
                                    //isVTRepeat = false;
                                }
                                break;
                            }
                            xmlIndex++;
                            if (!TvList[i].FieldName.StartsWith("GS") && occurGS && !isFoundGS)
                            {
                                //如果之前遇到了GS开头的，但是又没找到匹配的，且，此时，又不是GS开头。则跳过这一段GS_x
                                continue;
                            }
                            if (TvList[i].FieldName.StartsWith("GS"))
                            {
                                //遇到GS开头，记录出现GS_
                                occurGS = true;
                                if (TvList[i].FieldName == "GS**")
                                {
                                    isVTRepeat = true;
                                    //如果匹配
                                    isFoundGS = true;
                                    //新建一行GS
                                    rowViewList.Add(new ParsRowView("GS", "", ""));
                                }
                                else if (TvList[i].FieldName == "GS" + deviceID)
                                {
                                    //如果匹配
                                    isFoundGS = true;
                                    //新建一行GS
                                    rowViewList.Add(new ParsRowView("GS", "", ""));
                                    //currentContextIndex += deviceID.Length;
                                }
                            }
                            else if (TvList[i].FieldName.StartsWith("RS"))
                            {
                                //遇到GS开头，记录出现GS_
                                occurGS = true;
                                if (TvList[i].FieldName == "RS" + deviceID)
                                {
                                    //如果匹配
                                    isFoundGS = true;
                                    //新建一行GS
                                    rowViewList.Add(new ParsRowView("RS", "", ""));
                                    //currentContextIndex += deviceID.Length;
                                }
                            }
                            else if (TvList[i].FieldName == "RS")
                            {
                                rowViewList.Add(new ParsRowView("RS", "", ""));
                            }
                            else if (TvList[i].FieldName.StartsWith("VT"))
                            {
                                if (TvList[i].FieldName == "VT**")
                                {
                                    //可以循环遍历
                                    isVTRepeat = true;
                                }
                                if (emptyAfterVT)
                                {
                                    xmlIndex--;
                                    emptyAfterVT = false;
                                    break;
                                }
                                rowViewList.Add(new ParsRowView("VT", "", ""));
                                if (string.IsNullOrEmpty(vtDataItem))
                                    emptyAfterVT = true;
                            }
                            else
                            {
                                if (isVTRepeat)
                                {
                                    vtRepeatXMLnodeCount++;
                                }
                                string tempText = "";
                                if (TvList[i].FieldSize > 0)
                                {
                                    try
                                    {
                                        tempText = vtDataItem.Substring(currentContextIndex, TvList[i].FieldSize);
                                    }
                                    catch
                                    {
                                        tempText = "";
                                    }
                                    finally
                                    {
                                        currentContextIndex += TvList[i].FieldSize;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        tempText = vtDataItem.Substring(currentContextIndex, vtDataItem.Length - currentContextIndex);
                                        currentContextIndex += vtDataItem.Length - currentContextIndex;
                                    }
                                    catch
                                    {
                                    }
                                }
                                string tempComment = "";
                                if (TvList[i].FieldValue != null)
                                {

                                    if (TvList[i].FieldValue.ContainsKey(tempText))
                                        tempComment = TvList[i].FieldValue[tempText];
                                    else if (TvList[i].FieldValue.ContainsKey("*"))
                                        tempComment = TvList[i].FieldValue["*"];
                                    else
                                        tempComment = "";
                                }
                                else
                                {
                                    tempComment = "";
                                }
                                rowViewList.Add(new ParsRowView(TvList[i].FieldName, tempText, tempComment));
                            }

                            if (isVTRepeat && i == TvList.Count - 1)
                            {
                                xmlIndex = xmlIndex - vtRepeatXMLnodeCount - 1;
                                isVTRepeat = false;
                            }
                        }
                    }
                    if (isVTRepeat)
                    {
                        xmlIndex = xmlIndex + vtRepeatXMLnodeCount + 1;
                        isVTRepeat = false;
                    }
                }
            }
        }

    }
}
