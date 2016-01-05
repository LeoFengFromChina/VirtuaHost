using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static string CurrentPath
        { get; set; }
        public static string TrueBackPath
        {
            get; set;
        }
        
        public static string Version = " V0.1";
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

        public static List<string> GetXmlValueList(string xmlStr)
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
        public static bool WriteTextFileText(string path, string content)
        {
            if (!File.Exists(path))
                return false;
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                fs.Flush();
                fs.Close();
            }
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

        #region NDC-host-2
        private static Queue<string> _ndc_2FentchMessage = new Queue<string>();
        public static Queue<string> NDC_2FentchMessage
        {
            get
            {
                return _ndc_2FentchMessage;
            }
            set
            {
                _ndc_2FentchMessage = value;
            }
        }
        private static Queue<string> _ndc_2FentchResponseMessage = new Queue<string>();
        public static Queue<string> NDC_2FentchResponseMessage
        {
            get
            {
                return _ndc_2FentchResponseMessage;
            }
            set
            {
                _ndc_2FentchResponseMessage = value;
            }
        }
        #endregion



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
        public static byte[] EnPackageMsg(string msg, TcpHead tcpHead, ref string headContext)
        {
            byte[] resultBytes = null;
            int baseCount = Encoding.ASCII.GetByteCount(msg);
            if (tcpHead == TcpHead.L2L1)
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

        public static string UserInfoPath = @"\Config\Server\UserInfo.ini";

        public static bool isFulldownLoadEnqueue = false;

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
            string path = iniFilePath;
            if (File.Exists(path))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, path);
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

        public static void DoReversal()
        {
            //1.取出上次交易的金额
            int lastAmount = 0;
            string lastAmountStr = XDCUnity.ReadIniData("LastTransactionNotesDispensed", "Amount", "", XDCUnity.UserInfoPath);
            int.TryParse(lastAmountStr, out lastAmount);

            //2，取出账号
            string Account = XDCUnity.ReadIniData("LastTransactionNotesDispensed", "Pan", "", XDCUnity.UserInfoPath);

            //3.取出用户当前余额
            string AmountStr = XDCUnity.ReadIniData(Account, "AvailableBalance", "", XDCUnity.UserInfoPath);

            //4.计算充正后的余额
            int baseAmount = 0;
            int.TryParse(AmountStr, out baseAmount);

            //5.写入用户数据
            XDCUnity.WriteIniData(Account, "AvailableBalance", (-lastAmount + baseAmount).ToString(), XDCUnity.UserInfoPath);

        }
        /// <summary>
        /// KilleCAT
        /// </summary>
        public static void KilleCAT()
        {
            if (string.IsNullOrEmpty(eCATPath))
            {
                XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
                XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");
                eCATPath = node.Attributes["value"].InnerText;
            }
            string batPath = eCATPath + @"\KillATMC.bat";
            System.Threading.Thread killeCAT = new System.Threading.Thread(ProcessFile);
            killeCAT.IsBackground = true;
            killeCAT.Start(batPath);
            //ProcessBAT(batPath);
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void OpenPath(string path)
        {
            Process.Start("Explorer.exe", path);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void OpenFile(string fileName)
        {
            Process.Start(fileName);
        }
        public static void StarteCAT()
        {
            if (string.IsNullOrEmpty(TrueBackPath))
            {
                XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
                XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/TrueBackPath");
                TrueBackPath = node.Attributes["value"].InnerText;
            }
            string batPath = TrueBackPath;
            System.Threading.Thread killeCAT = new System.Threading.Thread(ProcessFile);
            killeCAT.IsBackground = true;
            killeCAT.Start(batPath);

            System.Threading.Thread.Sleep(200);

            if (string.IsNullOrEmpty(eCATPath))
            {
                XmlDocument doc = XMLHelper.instance.XMLFiles["BaseConfig"].XmlDoc;
                XmlNode node = doc.SelectSingleNode("BaseConfig/Settings/eCATPath");
                eCATPath = node.Attributes["value"].InnerText;
            }
            batPath = eCATPath + @"\eCAT.exe";
            System.Threading.Thread startCAT = new System.Threading.Thread(ProcessFile);
            startCAT.IsBackground = true;
            startCAT.Start(batPath);
        }
        /// <summary>
        /// 执行bat文件
        /// </summary>
        /// <param name="path"></param>
        public static void ProcessFile(object path)
        {
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.FileName = path.ToString();
                proc.StartInfo.Arguments = string.Format("10");//this is argument
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }
    }
}
