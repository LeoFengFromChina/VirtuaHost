﻿using StandardFeature;
using System.Collections.Generic;
using System.Xml;

namespace StatePars_NDC
{
    public class StateOperator_NDC : IStateOperator
    {
        public List<ParsRowView> GetView(string parsText)
        {
            List<ParsRowView> result = new List<ParsRowView>();
            //NDC
            string attrProtocolType = "1";
            //State
            string attrDataType = "1";
            string attrKey = parsText.Substring(0, 1);

            XmlNode cur = XDCUnity.GetNodeDetail(XDCUnity.Root, attrKey, attrProtocolType, attrDataType);
            if (cur == null)
            {
                //NDC OR DDC
                attrProtocolType = "0";
                cur = XDCUnity.GetNodeDetail(XDCUnity.Root, attrKey, attrProtocolType, attrDataType);
            }
            if (cur != null)
            {
                int curIndex = 0;
                foreach (XmlNode item in cur.ChildNodes)
                {
                    TemplateView tv = new TemplateView();
                    XmlAttribute fieldName = item.Attributes["Name"];
                    XmlAttribute fieldSize = item.Attributes["Size"];
                    ParsRowView prv = new ParsRowView();

                    int size;
                    int.TryParse(fieldSize.Value, out size);

                    string tempComment = "";
                    string tempValue = "";
                    try
                    {
                        tempValue = parsText.Substring(curIndex, size);
                        curIndex += size;
                    }
                    catch
                    {
                        if (parsText.Length - curIndex > 0)
                        {
                            tempValue = parsText.Substring(curIndex, parsText.Length - curIndex);
                        }
                        else
                        {
                            tempValue = "";
                        }
                        tempComment = "Invalid Length";
                        curIndex += size;
                    }

                    if (string.IsNullOrEmpty(tempComment) && item.HasChildNodes)
                    {
                        bool isFindComment = false;
                        foreach (XmlNode commentItem in item.ChildNodes)
                        {
                            string commentValue = commentItem.InnerText;
                            string commentText = commentItem.Attributes["Comment"].Value;
                            XmlAttribute attrOperation = commentItem.Attributes["Operation"];
                            if (attrOperation != null && attrOperation.Value.StartsWith("&"))
                            {
                                //&运算
                                //<Value Comment="Active FDK C" Operation="&amp;4">4</Value>
                                string ampValue = attrOperation.Value.Replace("&", "");
                                int ampResult = int.Parse(ampValue) & int.Parse(tempValue);
                                if (ampResult.ToString() == commentValue)
                                {
                                    tempComment += commentText + ";";
                                    isFindComment = true;
                                }

                            }
                            else if (tempValue.Equals(commentValue.Trim()))
                            {
                                tempComment = commentText;
                                isFindComment = true;
                                break;
                            }
                        }

                        if (!isFindComment)
                        {
                            tempComment = "UnKnow Value";
                        }
                    }
                    result.Add(new ParsRowView(fieldName.Value.ToString(), tempValue, tempComment));
                }
                return result;
            }
            else
                return null;
        }
    }
}
