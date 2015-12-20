using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualDualHost
{
    public partial class Form_ScreenParse : Form
    {
        public Form_ScreenParse()
        {
            InitializeComponent();
        }
        string[] columnsArray = new string[] { "@", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ":", ";", "<", "=", ">", "?", "P", "Q", "R", "S", "T", "U", "V", "W" };
        //string rows = "@A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,0,1,2,3";

        Dictionary<string, int> rowDic = new Dictionary<string, int>();
        Dictionary<string, int> columnDic = new Dictionary<string, int>();

        private void Form_ScreenParse_Load(object sender, EventArgs e)
        {
            //string[] columnsArray = columns.Split(',');
            for (int i = 0; i < columnsArray.Length; i++)
            {
                columnDic.Add(columnsArray[i].ToString(), i);
                if (i < 20)
                    rowDic.Add(columnsArray[i].ToString(), i);
            }


            ScreenWidth = ptb_Screen.Width;
            ScreenHeigh = ptb_Screen.Height;

            ddcSignalRowHeiht = ScreenHeigh / DDCRowCount;

            ddcSignalColumnWidth = ScreenWidth / DDCColumnCount;

            rb_DDC.CheckedChanged += Rb_DDC_CheckedChanged;
            rb_NDC.CheckedChanged += Rb_DDC_CheckedChanged;



        }

        private void Rb_DDC_CheckedChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void btn_Parse_Click(object sender, EventArgs e)
        {


            //string toPaintStr = "WELCOME TO GRGBANKING";
            //string startRow = "C";
            //string startColumn = "C";
            //DrawString(g, toPaintStr, startRow, startColumn);
            BaseFunction.Intial(XDCProtocolType.DDC, DataType.Screen);
            List<object> result = XDCUnity.ScreenOperator.GetView(rtb_Text.Text);
            if (result != null && result.Count > 0)
                ExcuteScreenCmd(result);

        }


        int ScreenWidth = 600;
        int ScreenHeigh = 540;
        int DDCRowCount = 20;
        int DDCColumnCount = 40;

        int ddcSignalRowHeiht;
        int ddcSignalColumnWidth;

        private void DrowDDCScreen()
        {

            Graphics g = ptb_Screen.CreateGraphics();

            int ddcCurrentRowBegin = 0;

            int ddcCurrentWidthBegin = 0;

            if (chb_GridLine.Checked)
            {
                //画行
                for (int i = 0; i < DDCRowCount; i++)
                {
                    g.DrawLine(new Pen(Color.Gray, 1), 0, ddcCurrentRowBegin, ScreenWidth, ddcCurrentRowBegin);
                    ddcCurrentRowBegin += ddcSignalRowHeiht;
                }

                //画列
                for (int i = 0; i < DDCColumnCount; i++)
                {
                    g.DrawLine(new Pen(Color.Gray, 1), ddcCurrentWidthBegin, 0, ddcCurrentWidthBegin, ScreenHeigh);
                    ddcCurrentWidthBegin += ddcSignalColumnWidth;
                }

                //包边,减去2是因为不与边线重合
                g.DrawLine(new Pen(Color.Gray, 1), 0, ScreenHeigh - 2, ScreenWidth - 2, ScreenHeigh - 2);
                g.DrawLine(new Pen(Color.Gray, 1), ScreenWidth - 2, 0, ScreenWidth - 2, ScreenHeigh - 2);
            }

            //string toPaintStr = "WELCOME TO GRGBANKING";
            //string startRow = "C";
            //string startColumn = "C";
            //DrawString(g, toPaintStr, startRow, startColumn);

            //g.Dispose();
        }

        private void DrawRowTitle()
        {
            Graphics grt = ptb_RowTitle.CreateGraphics();
            string columns = "@,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,0,1,2,3,4,5,6,7,8,9,:,;,<,=,>,?,P,Q,R,S,T,U,V,W";
            //string temp=column
            foreach (string str in columnsArray)
            {
                DrawString(grt, str, "@", str);
            }
            grt.Dispose();
        }
        private void DrawColumnTitle()
        {
            Graphics gct = ptb_ColumnTitle.CreateGraphics();
            string columns = "@,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,0,1,2,3,4,5,6,7,8,9,:,;,<,=,>,?,P,Q,R,S,T,U,V,W";
            //string temp=column
            for (int i = 0; i < 20; i++)
            {
                DrawString(gct, columnsArray[i], columnsArray[i], "@");
            }
            gct.Dispose();
        }
        /// <summary>
        /// 绘制字符串
        /// </summary>
        /// <param name="toDrawString"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        private void DrawString(Graphics g, string toDrawString, string startRow, string startColumn)
        {
            int startPositionY = 0;
            int startPositionX = 0;
            if (rowDic.ContainsKey(startRow))
            {
                startPositionY = rowDic[startRow] * ddcSignalRowHeiht;
            }
            if (columnDic.ContainsKey(startColumn))
            {
                startPositionX = columnDic[startColumn] * ddcSignalColumnWidth;
            }
            for (int i = 0; i < toDrawString.Length; i++)
            {
                CreateImage(g, toDrawString[i].ToString(), "微软雅黑", startPositionX, startPositionY, ddcSignalColumnWidth, ddcSignalRowHeiht);
                startPositionX += ddcSignalColumnWidth;
            }

        }


        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="g">画笔</param>
        /// <param name="reStr">需要绘制的文字</param>
        /// <param name="fontstr">字型</param>
        /// <param name="x">起始X轴</param>
        /// <param name="y">起始Y轴</param>
        /// <param name="width">宽</param>
        /// <param name="heigh">高</param>
        private void CreateImage(Graphics g, string reStr, string fontstr, int x, int y, int width, int heigh)
        {
            try
            {
                Font font = new System.Drawing.Font(fontstr, 11, (System.Drawing.FontStyle.Regular));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(x, y, width, heigh), Color.Blue, Color.DarkRed, 1.8f, true);
                g.DrawString(reStr, font, brush, x, y);
            }
            catch (Exception ex)
            {

            }
        }

        private void ExcuteScreenCmd(List<object> cmdList)
        {
            Graphics g = ptb_Screen.CreateGraphics();
            foreach (object cmd in cmdList)
            {
                Type currType = cmd.GetType();
                switch (currType.Name)
                {
                    case "DDC_SI_Command":
                        {
                            string toPaintStr = ((DDC_SI_Command)cmd).Content;
                            string startRow = ((DDC_SI_Command)cmd).StartRow;
                            string startColumn = ((DDC_SI_Command)cmd).StartColumn;
                            if (!string.IsNullOrEmpty(toPaintStr))
                            {
                                DrawString(g, toPaintStr, startRow, startColumn);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            g.Dispose();
        }



        bool isPanit = false;
        private void Form_ScreenParse_Paint(object sender, PaintEventArgs e)
        {
            //if (!isPanit)
            //{
            DrowDDCScreen();
            DrawRowTitle();
            DrawColumnTitle();
            isPanit = true;

            //}
        }

        private void ptb_Screen_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
