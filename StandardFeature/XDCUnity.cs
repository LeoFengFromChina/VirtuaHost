using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlHelper;

namespace StandardFeature
{
    public static class XDCUnity
    {
        public static XmlNode Root
        {
            get; set;
        }
        public static string eCATPath
        {
            get; set;
        }
        public static Dictionary<string, List<string>> BaseConfig
        {
            get;
            set;
        }
        public static XDCProtocolType CurrentProtocolType
        {
            get;
            set;
        }
        public static DataType CurrentDataType
        {
            get;
            set;
        }
        public static IMessageFormat MessageFormat
        {
            get;
            set;
        }

        public static IMessageOperator MessageOperator
        {
            get;
            set;
        }

        public static IStateOperator StateOperator
        {
            get;
            set;
        }

        public static IScreenOperator ScreenOperator
        {
            get;
            set;
        }
        public static IFitOperator FitOperator
        {
            get;
            set;
        }

        /// <summary>
        /// 根据给定的属性至获取指定的XML节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="keyID"></param>
        /// <param name="keyProrocolType"></param>
        /// <param name="keyDataType"></param>
        /// <returns></returns>
        public static XmlNode GetNodeDetail(XmlNode node, string keyID, string keyProrocolType, string keyDataType)
        {
            XmlNode result = null;
            XmlNodeList xnl = node.ChildNodes;
            string IDstr = "";
            string protocolTypestr = "";
            string dataTypestr = "";
            foreach (XmlNode item in xnl)
            {
                if (item.Attributes["ID"] != null)
                    IDstr = item.Attributes["ID"].Value;
                if (item.Attributes["ProtocolType"] != null)
                    protocolTypestr = item.Attributes["ProtocolType"].Value;
                if (item.Attributes["DataType"] != null)
                    dataTypestr = item.Attributes["DataType"].Value;

                if (IDstr.Trim().Equals(keyID.Trim())
                     && protocolTypestr == keyProrocolType
                     && dataTypestr == keyDataType)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Fit表是没有ID的
        /// </summary>
        /// <param name="node"></param>
        /// <param name="keyProrocolType"></param>
        /// <param name="keyDataType"></param>
        /// <returns></returns>
        public static XmlNode GetNodeDetail(XmlNode node, string keyProrocolType, string keyDataType)
        {
            XmlNode result = null;
            XmlNodeList xnl = node.ChildNodes;
            foreach (XmlNode item in xnl)
            {
                if (item.Attributes["ProtocolType"] != null && item.Attributes["ProtocolType"].Value == keyProrocolType
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
        public static string GetFS(int count)
        {
            string resultStr = "";
            while (count > 0)
            {
                resultStr += "FS";
                count--;
            }
            return resultStr;
        }

        public static void Initial()
        {
            //1.Root
            Root = XMLHelper.instance.XMLFiles["ProtocolTemplate"].XmlDoc.SelectSingleNode("ProtocolTemplate");

            //2.BaseConfig
            GetBaseConfig();
        }

        private static void GetBaseConfig()
        {
            string resultXML = XMLHelper.instance.XMLFiles["BaseConfig"].GetXmlAttributeValue("BaseConfig.NoColorColumns.[*].{*}");
            BaseConfig = new Dictionary<string, List<string>>();

            List<string> baseConfigList = GetXmlValueList(resultXML);
            BaseConfig.Add("NoColorColumns", baseConfigList);

            string ColorYellowXML = XMLHelper.instance.XMLFiles["BaseConfig"].GetXmlAttributeValue("BaseConfig.ColorYellow.[*].{*}");
            List<string> colorYellowConfigList = GetXmlValueList(ColorYellowXML);
            BaseConfig.Add("ColorYellow", colorYellowConfigList);

            string ColorRedXML = XMLHelper.instance.XMLFiles["BaseConfig"].GetXmlAttributeValue("BaseConfig.ColorRed.[*].{*}");
            List<string> ColorRedConfigList = GetXmlValueList(ColorRedXML);
            BaseConfig.Add("ColorRed", ColorRedConfigList);

            string eCATPath = XMLHelper.instance.XMLFiles["BaseConfig"].GetXmlAttributeValue("BaseConfig.Settings.[eCATPath].{value}");

            BaseConfig.Add("eCATPath", new List<string> { eCATPath });
        }

        private static List<string> GetXmlValueList(string xmlStr)
        {
            List<string> resultList = new List<string>();
            string[] resultXMLarray = xmlStr.Split(';');
            foreach (string baseConfigItem in resultXMLarray)
            {
                resultList.Add(baseConfigItem.TrimStart('[').TrimEnd(']').TrimEnd(','));
            }
            return resultList;
        }

        public static string GetTxtFileText(string path)
        {
            string text = string.Empty;
            try
            {
                text = System.IO.File.ReadAllText(@path);
            }
            catch
            {
                text = "Exception";
            }

            return text;
        }

        private static Queue<string> _ddcFentchMessage = new Queue<string>();
        public static Queue<string> DDCFentchMessage
        {
            get
            {
                return _ddcFentchMessage;
            }
            set
            {
                _ddcFentchMessage = value;
            }
        }
        private static Queue<string> _ddcFentchResponseMessage = new Queue<string>();
        public static Queue<string> DDCFentchResponseMessage
        {
            get
            {
                return _ddcFentchResponseMessage;
            }
            set
            {
                _ddcFentchResponseMessage = value;
            }
        }

        private static Queue<string> _ndcFentchMessage = new Queue<string>();
        public static Queue<string> NDCFentchMessage
        {
            get
            {
                return _ndcFentchMessage;
            }
            set
            {
                _ndcFentchMessage = value;
            }
        }
        private static Queue<string> _ndcFentchResponseMessage = new Queue<string>();
        public static Queue<string> NDCFentchResponseMessage
        {
            get
            {
                return _ndcFentchResponseMessage;
            }
            set
            {
                _ndcFentchResponseMessage = value;
            }
        }

        private static List<Host> _currentHosts = new List<Host>();
        public static List<Host> CurrentHost
        {
            get
            {
                return _currentHosts;
            }
            set
            {
                _currentHosts = value;
            }
        }

        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EnPackageMsg(string msg, Host currHost, ref string headContext)
        {
            byte[] resultBytes = null;
            int baseCount = Encoding.ASCII.GetByteCount(msg);
            if (currHost.TCPHead == TcpHead.L2L1)
            {
                char char_1 = new char();
                char char_2 = new char();
                headContext = baseCount.ToString().PadLeft(4, '0');

                //1.长度L右移8位，等到A，
                int A = baseCount >> 8;
                //2char_1=A,.A左移8位，得到B，L-B=C,char_2=C                
                char_1 = (char)A;
                int B = A << 8;
                int C = baseCount - B;
                char_2 = (char)C;

                resultBytes = Encoding.ASCII.GetBytes(msg);
                byte[] bigBytes = new byte[resultBytes.Length + 2];
                bigBytes[0] = (byte)A;
                bigBytes[1] = (byte)C;
                resultBytes.CopyTo(bigBytes, 2);
                resultBytes = bigBytes;
            }
            else
                resultBytes = Encoding.ASCII.GetBytes(msg);
            return resultBytes;
        }
    }
}
