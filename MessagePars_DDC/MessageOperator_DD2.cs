using StandardFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlHelper;

namespace MessagePars_DDC
{
    public class MessageOperator_DDC2 : IMessageOperator
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
            string attrProtocolType = "2";
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

                        //DDC的状态、屏幕、FIT等，有 tempArrary[3]做为commandCode
                        commandCode = tempArrary[3].Substring(0, 1);
                        MessageIdentifier = tempArrary[3].Substring(1, 1);
                        attrID = "3FSFSFS" + commandCode + MessageIdentifier;
                        cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                        if (null == cur)
                        {
                            attrID = "3FSFSFS" + commandCode;
                            cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                            if (null == cur)
                            //通常查询xml文件中ID=1的配置
                            {
                                attrID = tempArrary[0].Substring(0, 1);
                                cur = XDCUnity.GetNodeDetail(root, attrID, attrProtocolType, attrDataType);
                            }
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
                        MessageIdentifier = commandCode;
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
                        //扩张功能命令：‘
                        attrID = "4";
                        if (CurrentMessage.MsgASCIIString.Contains(XDCSplictorChar.FS + "'"))
                            MessageIdentifier = "'";
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

            CurrentNode = cur;
        }

        public static List<ParsRowView> GetViewList(XDCMessage XDCmsg, string msgCurrentFieldContent, int currentFSindex, ref int currXmlNodeIndex)
        {
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
                    ParsRowView prv = new ParsRowView();
                    prv.FieldName = tvItem.FieldName;
                    if (prv.FieldName == "GS"
                        && msgCurrentFieldContent.Length == currentContentIndex)
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
                    if (rsData.Contains(XDCSplictorChar.VT2))
                        vtDataArray = rsData.Split(XDCSplictorChar.VT2);
                    else
                        vtDataArray = rsData.Split(XDCSplictorChar.VT);
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
