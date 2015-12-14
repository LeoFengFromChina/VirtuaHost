using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
                text = "";
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

        public static string UserInfoPath = System.Environment.CurrentDirectory + @"\Config\Server\UserInfo.ini";

        #region ini文件操作

        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);


        #endregion

        #region 读Ini文件

        public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件

        public static bool WriteIniData(string Section, string Key, string Value, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        public static void RecordLastTransaction(XDCMessage msgContent, int changeAmount)
        {
            string availableBanlance = XDCUnity.ReadIniData(msgContent.PAN, "AvailableBalance", string.Empty, XDCUnity.UserInfoPath);

            double newBalance = double.Parse(availableBanlance);
            newBalance += changeAmount;
            XDCUnity.WriteIniData(msgContent.PAN, "AvailableBalance", newBalance.ToString(), XDCUnity.UserInfoPath);

            //记录账号
            XDCUnity.WriteIniData("LastTransactionNotesDispensed", "Pan", msgContent.PAN, XDCUnity.UserInfoPath);
            //记录金额
            XDCUnity.WriteIniData("LastTransactionNotesDispensed", "Amount", changeAmount.ToString(), XDCUnity.UserInfoPath);

            //记录序列号
            string TSN = XDCUnity.ReadIniData("LastTransactionNotesDispensed", "LastTransactionSerialNumber", "", XDCUnity.UserInfoPath);
            string newTSN = (int.Parse(TSN) + 1).ToString();
            XDCUnity.WriteIniData("LastTransactionNotesDispensed", "LastTransactionSerialNumber", newTSN.ToString(), XDCUnity.UserInfoPath);

        }
    }
}
