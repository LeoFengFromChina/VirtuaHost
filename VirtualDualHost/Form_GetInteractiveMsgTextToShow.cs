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
    public partial class Form_GetInteractiveMsgTextToShow : Form
    {
        public Form_GetInteractiveMsgTextToShow(string sourceMsg)
        {
            InitializeComponent();
            rtb_Msg.Text = sourceMsg;
            GetSubAccountList(rtb_Msg.Text);
        }


        void GetSubAccountList(string updateDataSrc)
        {
            string str = updateDataSrc;// string.Empty;
            //str = 088#E>295#E0001-5-004989  #F0#H>295H0001-9-006027  I0

            List<string> resultValues = new List<string>();

            char SO = (char)0x0E;

            char SI = (char)0x0F;
            bool isFindSO = false;
            bool isFindSI = false;
            int currentCharIndex = 0;
            char tempChar;
            string tempStr = string.Empty;
            for (int i = currentCharIndex; i < updateDataSrc.Length; currentCharIndex++)
            {
                if (currentCharIndex >= updateDataSrc.Length)
                {
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        resultValues.Add(tempStr);
                        tempStr = string.Empty;
                    }
                    break;
                }
                tempChar = updateDataSrc[currentCharIndex];
                if (tempChar == SO)
                {
                    isFindSO = true;
                    //isFindSI = false;
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        resultValues.Add(tempStr);
                        tempStr = string.Empty;
                    }
                    currentCharIndex += 3;
                    continue;
                }

                if (tempChar == SI)
                {
                    isFindSI = true;
                    isFindSO = false;
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        resultValues.Add(tempStr);
                        tempStr = string.Empty;
                    }
                    currentCharIndex += 2;
                    continue;
                }
                if (!isFindSI && !isFindSO)
                {
                    i++;
                    continue;
                }
                if (isFindSI)
                {
                    tempStr += tempChar.ToString();
                }

            }

            rtb_Buffers.Clear();
            if (resultValues != null && resultValues.Count > 0)
            {
                for (int i = 0; i < resultValues.Count; i++)
                {
                    rtb_Buffers.AppendText("InteractiveMsgBuffer_" + i + " : " + resultValues[i] + "\r\n");
                }
            }

        }

        private void btn_Parse_Click(object sender, EventArgs e)
        {
            GetSubAccountList(rtb_Msg.Text);
        }
    }
}
