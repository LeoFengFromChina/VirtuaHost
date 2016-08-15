using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualDualHost
{
    public partial class Form_C09 : Form
    {
        public Form_C09()
        {
            InitializeComponent();
        }
        C09 C09Ojb = new C09();
        private void button1_Click(object sender, EventArgs e)
        {
            Parse(richTextBox1.Text);
            txt_ActiveTransactionCount.Text = C09Ojb.aActiveTransactionNo;
            txt_Track1.Text = C09Ojb.bTrack1;
            txt_Track2.Text = C09Ojb.cTrack2;
            txt_Track3.Text = C09Ojb.dTrack3;
            txt_PINFlag.Text = C09Ojb.ePinFlag;
            lbl_PINFlag.Text = C09Ojb.ePinFlag == "0" ? "No PIN collect." : "PIN collect";
            txt_TRSNO.Text = C09Ojb.fTransactionRquestStateNo;
            List<StatesView> StatesViewList = new List<StatesView>();
            for (int i = 0; i < C09Ojb.gNextStateNumberTable.Count; i++)
            {
                StatesView sv = new StatesView();
                sv.NextStateNumber = C09Ojb.gNextStateNumberTable[i];
                #region hNextStateActionTable

                if (C09Ojb.hNextStateActionTable[i].Equals("0"))
                {
                    sv.NextStateAction = "0 - Function complete, prompt for nexttransaction.";
                }
                else if(C09Ojb.hNextStateActionTable[i].Equals("1"))
                {
                    sv.NextStateAction = "1 - Function failed, prompt for next transaction.";
                }
                else if (C09Ojb.hNextStateActionTable[i].Equals("2"))
                {
                    sv.NextStateAction = "2 - Function complete, return to normal operation.";
                }
                else if (C09Ojb.hNextStateActionTable[i].Equals("3"))
                {
                    sv.NextStateAction = "3 - Function failed, return to normal operation.";
                }
                #endregion
                StatesViewList.Add(sv);
            }

            int TransactionCount = C09Ojb.aActiveTransactionNo == ":" ? 10 : int.Parse(C09Ojb.aActiveTransactionNo);
            List<OperationView> OperationViewList = new List<OperationView>();
            for (int i = 0; i < TransactionCount; i++)
            {
                OperationView ov = new OperationView();
                ov.OperationCode = C09Ojb.OperationDic[i].iOPC;
                ov.BufferBLen = C09Ojb.OperationDic[i].jBufferBLen;
                ov.BufferBData = C09Ojb.OperationDic[i].kBufferBData;
                ov.BufferCLen = C09Ojb.OperationDic[i].lBufferCLen;
                ov.BufferCData = C09Ojb.OperationDic[i].mBufferCData;
                ov.AmountBufferFlag = C09Ojb.nAmountBufferFlag[i];
                OperationViewList.Add(ov);

            }
            dgv_States.DataSource = StatesViewList;
            dgv_OPC.DataSource = OperationViewList;
        }
        private void Parse(string tempC09)
        {

            string getText = string.Empty;
            string remainText = string.Empty;

            //a
            C09Ojb.aActiveTransactionNo = GetSubString(ref tempC09, 1);
            if (C09Ojb.aActiveTransactionNo.Equals(":"))
                C09Ojb.aActiveTransactionNo = "10";
            //b            
            //c
            //d
            //三个磁道同步处理
            int findSentenlCount = 3;
            if (tempC09.IndexOf("?") >= 0)
            {
                if (tempC09.IndexOf("?") == 0)
                    findSentenlCount = 3;
                else
                    findSentenlCount = 2;//存在track1

                //78+39+106=223
                int currentIndex = tempC09.IndexOf("?");
                int count = 1;
                while (currentIndex < 223 && count < findSentenlCount)
                {
                    currentIndex++;
                    if (count > 0 && tempC09.IndexOf("?", currentIndex) < 0)
                    {
                        currentIndex--;
                        break;
                    }
                    currentIndex = tempC09.IndexOf("?", currentIndex);
                    count++;
                }
                int lastIndexOFendSentinel = currentIndex;


                //存在？
                //找到最后一个问号
                //int lastIndexOFendSentinel = tempC09.LastIndexOf("?");
                getText = GetSubString(ref tempC09, lastIndexOFendSentinel + 1); // tempC09.Substring(0, lastIndexOFendSentinel + 1);
                string[] trackArray = getText.Split('?');
                if (trackArray.Length > 0)
                    C09Ojb.bTrack1 = trackArray[0].Length > 1 ? trackArray[0] + "?" : string.Empty;
                if (trackArray.Length > 1)
                    C09Ojb.cTrack2 = trackArray[1].Length > 1 ? trackArray[1] + "?" : string.Empty;
                if (trackArray.Length > 2)
                    C09Ojb.dTrack3 = trackArray[2].Length > 1 ? trackArray[2] + "?" : string.Empty;

            }

            //e
            C09Ojb.ePinFlag = GetSubString(ref tempC09, 1); ;
            //f
            C09Ojb.fTransactionRquestStateNo = GetSubString(ref tempC09, 3); ;
            //g
            getText = GetSubString(ref tempC09, 30);
            C09Ojb.gNextStateNumberTable = GetListByEachLen(getText, 3);
            //h
            getText = GetSubString(ref tempC09, 10);
            C09Ojb.hNextStateActionTable = GetListByEachLen(getText, 1);


            //n
            getText = GetSubString(ref tempC09, int.Parse(C09Ojb.aActiveTransactionNo), true);
            C09Ojb.nAmountBufferFlag = GetListByEachLen(getText, 1);

            //opcode
            C09Ojb.OperationDic = GetOPCode(tempC09, int.Parse(C09Ojb.aActiveTransactionNo));
        }
        #region Method

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="ContenStr">原始字符串</param>
        /// <param name="length">截取的长度</param>
        /// <param name="isFromRear">是否从尾部开始截取，默认false从头部开始</param>
        /// <returns></returns>
        private string GetSubString(ref string ContenStr, int length, bool isFromRear = false)
        {
            string resultStr = string.Empty;

            if (ContenStr.Length < length)
                return string.Empty;

            if (!isFromRear)
                resultStr = ContenStr.Substring(0, length);
            else
                resultStr = ContenStr.Substring(ContenStr.Length - length, length);


            if (ContenStr.Length == length)
                ContenStr = string.Empty;
            else
            {
                if (!isFromRear)
                    ContenStr = ContenStr.Substring(length);
                else
                    ContenStr = ContenStr.Substring(0, ContenStr.Length - length);
            }
            return resultStr;
        }

        private List<string> GetListByEachLen(string ContentStr, int eachLen)
        {
            List<string> resultList = new List<string>();
            while (!string.IsNullOrEmpty(ContentStr) && ContentStr.Length > 0)
            {
                if (ContentStr.Length >= eachLen)
                {
                    resultList.Add(ContentStr.Substring(0, eachLen));
                    ContentStr = ContentStr.Substring(eachLen);
                }
                else
                {
                    resultList.Add(ContentStr);
                    ContentStr = string.Empty;
                }

            }
            return resultList;
        }

        private List<Operation> GetOPCode(string ContentStr, int Count)
        {
            List<Operation> result = new List<Operation>();
            for (int i = 0; i < Count; i++)
            {
                Operation operation = new Operation();
                operation.iOPC = GetSubString(ref ContentStr, 8);
                operation.jBufferBLen = GetSubString(ref ContentStr, 1);
                if (!operation.jBufferBLen.Equals("?"))
                    operation.kBufferBData = GetSubString(ref ContentStr, int.Parse(operation.jBufferBLen));
                else
                {
                    operation.kBufferBData = "Not present";
                }
                operation.lBufferCLen = GetSubString(ref ContentStr, 1);
                if (!operation.lBufferCLen.Equals("?"))
                    operation.mBufferCData = GetSubString(ref ContentStr, int.Parse(operation.lBufferCLen));
                else
                {
                    operation.mBufferCData = "Not present";
                }
                result.Add(operation);
            }

            return result;
        }
        #endregion
    }

    public class C09
    {
        public string aActiveTransactionNo
        {
            get; set;
        }
        public string bTrack1
        {
            get;
            set;
        }
        public string cTrack2
        {
            get; set;
        }
        public string dTrack3
        { get; set; }
        public string ePinFlag
        { get; set; }
        public string fTransactionRquestStateNo
        { get; set; }
        public List<string> gNextStateNumberTable
        { get; set; }
        public List<string> hNextStateActionTable
        { get; set; }
        public List<Operation> OperationDic
        { get; set; }
        public List<string> nAmountBufferFlag
        { get; set; }
    }
    public class Operation
    {
        public string iOPC
        { get; set; }

        public string jBufferBLen
        { get; set; }

        public string kBufferBData
        { get; set; }

        public string lBufferCLen
        { get; set; }

        public string mBufferCData
        { get; set; }

    }
    public class StatesView
    {
        public StatesView()
        { }
        public string NextStateNumber { get; set; }
        public string NextStateAction { get; set; }
    }
    public class OperationView
    {
        public OperationView()
        { }
        public string OperationCode { get; set; }
        public string BufferBLen { get; set; }
        public string BufferBData { get; set; }
        public string BufferCLen { get; set; }
        public string BufferCData { get; set; }
        public string AmountBufferFlag { get; set; }
    }
}
