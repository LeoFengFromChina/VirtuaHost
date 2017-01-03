using System.Collections.Generic;
using StandardFeature;
using System.Xml;

namespace FitPars_DDC
{
    public class FitOperator_DDC : IFitOperator
    {

        public List<ParsRowView> GetView(string parsText)
        {
            List<ParsRowView> result = new List<ParsRowView>();
            //NDC OR DDC
            string attrProtocolType = "0";
            //State
            string attrDataType = "3";

            XmlNode cur = XDCUnity.GetNodeDetail(XDCUnity.Root, attrProtocolType, attrDataType);
            if (cur == null)
            {
                //NDC OR DDC
                attrProtocolType = "0";
                cur = XDCUnity.GetNodeDetail(XDCUnity.Root, attrProtocolType, attrDataType);
            }
            if (cur != null)
            {
                //each 3 bit for one group,this values will be used as the base value to calculate for hex.
                List<string> parsTextInList = new List<string>();
                string SingFieldText = string.Empty;
                for (int i = 0; i < parsText.Length; i++)
                {
                    SingFieldText += parsText[i];
                    if ((i + 1) % 3 == 0)
                    {
                        parsTextInList.Add(SingFieldText);
                        SingFieldText = "";
                    }
                }

                string tempValue = "";
                string tempValueBeforHex = "";
                int listIndex = 0;
                foreach (XmlNode item in cur.ChildNodes)
                {
                    TemplateView tv = new TemplateView();
                    XmlAttribute fieldName = item.Attributes["Name"];
                    XmlAttribute fieldSize = item.Attributes["Size"];
                    ParsRowView prv = new ParsRowView();
                    tempValue = "";
                    tempValueBeforHex = "";
                    int size;
                    int.TryParse(fieldSize.Value, out size);
                    int realSize = size / 3;// 3 bit to calculate for hex value--frde 20151117

                    string tempComment = "";
                    for (int i = 0; i < realSize; i++)
                    {
                        try
                        {
                            //可能缺少数据的情况
                            tempValue += int.Parse(parsTextInList[listIndex]).ToString("X2");
                            tempValueBeforHex += parsTextInList[listIndex];
                        }
                        catch
                        {
                            tempComment = "Invalid Length";
                        }
                        finally
                        {
                            listIndex++;
                        }
                    }

                    if (string.IsNullOrEmpty(tempComment) && item.HasChildNodes)
                    {
                        foreach (XmlNode commentItem in item.ChildNodes)
                        {
                            string commentValue = commentItem.InnerText;
                            string commentText = commentItem.Attributes["Comment"].Value;
                            XmlAttribute commentIsOperation = commentItem.Attributes["Operation"];
                            if (null != commentIsOperation)
                            {
                                if (commentIsOperation.Value.StartsWith("&"))
                                {
                                    //&运算
                                    string ampValue = commentIsOperation.Value.Replace("&", "");
                                    int ampResult = int.Parse(tempValueBeforHex) & int.Parse(ampValue);
                                    //string hexValue = ampResult.ToString("X2");
                                    if (ampResult == int.Parse(commentValue))
                                    {
                                        tempComment = commentText;
                                        break;
                                    }
                                }
                            }
                            else if (tempValue.Equals(commentValue.Trim()))
                            {
                                tempComment = commentText;
                            }
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
