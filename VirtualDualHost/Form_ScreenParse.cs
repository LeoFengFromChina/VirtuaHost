using StandardFeature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace VirtualDualHost
{
    public partial class Form_ScreenParse : Form
    {
        XDCProtocolType currentProtocol;
        string currentScreenPath = string.Empty;
        public Form_ScreenParse(string parseText = "", XDCProtocolType protocol = XDCProtocolType.NDC, string screenNum = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(screenNum))
            {
                this.Text += " - [" + screenNum + "]";
                currentScreenPath = screenNum;
            }
            if (!string.IsNullOrEmpty(parseText))
            {
                rtb_Text.Text = parseText;

                if (protocol == XDCProtocolType.NDC)
                {
                    rb_NDC.Checked = true;
                    FontSize = NDCFontSize;
                }
                else
                {
                    rb_DDC.Checked = true;
                    FontSize = DDCFontSize;
                }
                currentProtocol = protocol;
            }
            btn_Pre.Click += btn_Next_Click;
        }

        #region Field

        List<object> result = new List<object>();

        string[] columnsArray = new string[] { "@", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ":", ";", "<", "=", ">", "?", "P", "Q", "R", "S", "T", "U", "V", "W" };

        Dictionary<string, int> rowDic = new Dictionary<string, int>();
        Dictionary<string, int> columnDic = new Dictionary<string, int>();

        int FontSize = 12;

        int DDCFontSize = 12;
        int NDCFontSize = 14;
        int ScreenWidth = 640;
        int ScreenHeigh = 480;

        int DDCRowCount = 20;
        int DDCColumnCount = 40;

        int NDCRowCount = 16;
        int NDCColumnCount = 32;

        int currentRowCount;
        int currentColumnCount;
        int currentSignalRowHeiht;
        int currentSignalColumnWidth;

        List<DDC_SI_Command> toPaintSI = null;

        string startColumn = "@";
        string startRow = "@";
        #endregion

        #region Event

        private void Form_ScreenParse_Load(object sender, EventArgs e)
        {
            CalculateProcotolScreen();
            rb_DDC.CheckedChanged += Rb_DDC_CheckedChanged;
            rb_NDC.CheckedChanged += Rb_DDC_CheckedChanged;
            chb_GridLine.CheckedChanged += Chb_GridLine_CheckedChanged;
        }

        private void Chb_GridLine_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                DrawNetScreen();
            }
            else
            {
                pnl_Screen.Refresh();
            }
        }

        private void Rb_DDC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked)
                return;
            if (rb.Text == "DDC")
            {
                currentProtocol = XDCProtocolType.DDC;
                FontSize = DDCFontSize;
            }
            else
            {
                currentProtocol = XDCProtocolType.NDC;
                FontSize = NDCFontSize;
            }
            CalculateProcotolScreen();
            DrawXDCScreen();
            if (!string.IsNullOrEmpty(rtb_Text.Text.Trim()))
                BegionScreenParse();
        }

        private void btn_Parse_Click(object sender, EventArgs e)
        {
            DrawXDCScreen();
            if (!string.IsNullOrEmpty(rtb_Text.Text.Trim()))
            {
                BegionScreenParse();
            }
        }

        private void pnl_Final_Paint_1(object sender, PaintEventArgs e)
        {
            DrawXDCScreen();
            BegionScreenParse();
        }

        private void rtb_Text_TextChanged(object sender, EventArgs e)
        {
            DrawNetScreen();
            BegionScreenParse();
        }

        #endregion

        #region Func

        /// <summary>
        /// 开始解析屏幕
        /// </summary>
        private void BegionScreenParse()
        {
            if (toPaintSI != null && toPaintSI.Count > 0)
                toPaintSI.Clear();
            pnl_Screen.BackgroundImage = null;
            pnl_Screen.Controls.Clear();
            startColumn = "@";
            startRow = "@";
            if (!string.IsNullOrEmpty(rtb_Text.Text.Trim()))
            {
                CalculateProcotolScreen();
                List<object> result = XDCUnity.ScreenOperator.GetView(rtb_Text.Text);
                if (result != null && result.Count > 0)
                    ExcuteScreenCmd(result);
                else
                {
                    DrawXDCScreen();
                }
            }
        }

        /// <summary>
        /// 画整个页面
        /// </summary>
        private void DrawXDCScreen()
        {
            //计算对应协议的屏幕
            DrawColumnTitle();
            DrawRowTitle();
            DrawNetScreen();
        }

        /// <summary>
        /// 画网装
        /// </summary>
        private void DrawNetScreen()
        {
            #region 画网块

            lbl_Notice.Text = "";
            lbl_Notice.ForeColor = Color.Black;
            pnl_Screen.Controls.Clear();
            pnl_Screen.Refresh();

            Graphics g = pnl_Screen.CreateGraphics();

            int xdcCurrentRowBegin = 0;
            int xdcCurrentWidthBegin = 0;

            if (chb_GridLine.Checked)
            {
                //画行
                for (int i = 0; i < currentRowCount; i++)
                {
                    g.DrawLine(new Pen(Color.Gray, 1), 0, xdcCurrentRowBegin, ScreenWidth, xdcCurrentRowBegin);
                    xdcCurrentRowBegin += currentSignalRowHeiht;
                }

                //画列
                for (int i = 0; i < currentColumnCount; i++)
                {
                    g.DrawLine(new Pen(Color.Gray, 1), xdcCurrentWidthBegin, 0, xdcCurrentWidthBegin, ScreenHeigh);
                    xdcCurrentWidthBegin += currentSignalColumnWidth;
                }

                //包边,减去2是因为不与边线重合
                g.DrawLine(new Pen(Color.Gray, 1), 0, ScreenHeigh - 1, ScreenWidth - 1, ScreenHeigh - 1);
                g.DrawLine(new Pen(Color.Gray, 1), ScreenWidth - 1, 0, ScreenWidth - 1, ScreenHeigh - 1);
            }

            g.Dispose();

            #endregion
        }

        /// <summary>
        /// 根据当前协议，计算屏幕当前的行、列数和对应的宽、高度
        /// </summary>
        private void CalculateProcotolScreen()
        {
            if (rb_DDC.Checked)
            {
                currentRowCount = DDCRowCount;
                currentColumnCount = DDCColumnCount;

                BaseFunction.Intial(XDCProtocolType.DDC, DataType.Screen);
            }
            else
            {
                currentRowCount = NDCRowCount;
                currentColumnCount = NDCColumnCount;
                BaseFunction.Intial(XDCProtocolType.NDC, DataType.Screen);
            }

            currentSignalColumnWidth = ScreenWidth / currentColumnCount;
            currentSignalRowHeiht = ScreenHeigh / currentRowCount;

            for (int i = 0; i < currentColumnCount; i++)
            {
                if (!columnDic.ContainsKey(columnsArray[i].ToString()))
                    columnDic.Add(columnsArray[i].ToString(), i);
                if (i < currentRowCount && !rowDic.ContainsKey(columnsArray[i].ToString()))
                    rowDic.Add(columnsArray[i].ToString(), i);
            }
        }

        /// <summary>
        /// 绘制行标题
        /// </summary>
        private void DrawRowTitle()
        {
            pnl_Row.Refresh();
            Graphics grt = pnl_Row.CreateGraphics();
            for (int i = 0; i < currentColumnCount; i++)
            {
                DrawString(grt, columnsArray[i], "@", columnsArray[i], Color.Blue);
            }
            grt.Dispose();
        }

        /// <summary>
        /// 画列标题
        /// </summary>
        private void DrawColumnTitle()
        {
            pnl_Column.Refresh();
            Graphics gct = pnl_Column.CreateGraphics();
            for (int i = 0; i < currentRowCount; i++)
            {
                if (rb_DDC.Checked &&
                    (columnsArray[i] == "I"
                    || columnsArray[i] == "L"
                    || columnsArray[i] == "O"
                    || columnsArray[i] == "2"))
                    DrawString(gct, columnsArray[i] + "<FDK", columnsArray[i], "@", Color.Red, FontStyle.Bold);
                else if (rb_NDC.Checked && (columnsArray[i] == "F"
                    || columnsArray[i] == "I"
                    || columnsArray[i] == "L"
                    || columnsArray[i] == "O"))
                    DrawString(gct, columnsArray[i] + "<FDK", columnsArray[i], "@", Color.Red, FontStyle.Bold);
                else
                    DrawString(gct, columnsArray[i], columnsArray[i], "@", Color.Blue);
            }
            gct.Dispose();
        }

        /// <summary>
        /// 执行屏幕
        /// </summary>
        /// <param name="cmdList"></param>
        private void ExcuteScreenCmd(List<object> cmdList)
        {
            foreach (object cmd in cmdList)
            {
                Type currType = cmd.GetType();
                switch (currType.Name)
                {
                    case "DDC_SI_Command":
                        {
                            if (toPaintSI == null)
                                toPaintSI = new List<DDC_SI_Command>();
                            Graphics g = pnl_Screen.CreateGraphics();
                            DDC_SI_Command cur_SI = (DDC_SI_Command)cmd;
                            string toPaintStr = cur_SI.Content;
                            startRow = cur_SI.StartRow;
                            startColumn = cur_SI.StartColumn;
                            if (!string.IsNullOrEmpty(toPaintStr))
                            {
                                DrawString(g, toPaintStr, startRow, startColumn, Color.Blue);
                            }

                        }
                        break;
                    case "DDC_ESCP_Command":
                        {
                            DDC_ESCP_Command cur_ESCP = (DDC_ESCP_Command)cmd;

                            SetBackgroundImage_ESCP(cur_ESCP.Content);

                        }
                        break;
                    case "DDC_SO_Command":
                        {
                            DDC_SO_Command cur_SO = (DDC_SO_Command)cmd;
                            Insert_SO(cur_SO.Content);
                        }
                        break;
                    default:
                        break;
                }
            }
            //DrawSI_Text();
        }

        /// <summary>
        /// ESCP设置背景图片
        /// </summary>
        /// <param name="imageID"></param>
        private void SetBackgroundImage_ESCP(string imageID)
        {
            string resFilePath = XDCUnity.eCATPath + @"\Resource\XDC\ResFileMap.xml";
            //初始化一个xml实例
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(resFilePath))
                return;
            //导入指定xml文件
            xml.Load(resFilePath);
            //指定一个节点
            XmlNode root = xml.SelectSingleNode("/Root/MapSetting");
            string imageFileName = string.Empty;
            foreach (XmlNode subNode in root)
            {
                if (subNode.NodeType == XmlNodeType.Element && subNode.Attributes["key"].Value.ToString() == imageID)
                    imageFileName = subNode.Attributes["value"].Value.ToString();
            }

            if (!string.IsNullOrEmpty(imageFileName))
            {
                string imagePath = XDCUnity.eCATPath + @"\Resource\XDC\Image\000\" + imageFileName;

                if (File.Exists(imagePath))
                {
                    Image img = Image.FromFile(imagePath);
                    if (img.Width < ScreenWidth && img.Height < ScreenHeigh)
                    {
                        PictureBox pb = new PictureBox();
                        pb.BackgroundImage = Image.FromFile(imagePath);
                        int Sx = currentSignalColumnWidth * columnDic[startColumn];
                        int Sy = currentSignalRowHeiht * rowDic[startRow];
                        pb.Location = new Point(Sx, Sy);
                        pb.Width = img.Width * 640 / 800;
                        pb.Height = img.Height * 480 / 600;
                        pb.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl_Screen.Controls.Add(pb);
                    }
                    else
                    {
                        pnl_Screen.BackgroundImage = img;
                        DrawNetScreen();
                    }
                }
                else
                {
                    lbl_Notice.Text = "Can't Find Image:" + imagePath;
                    lbl_Notice.ForeColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// 重新嵌入一个页面
        /// </summary>
        /// <param name="content"></param>
        private void Insert_SO(string content)
        {
            string protocolFolder = string.Empty;
            if (currentProtocol == XDCProtocolType.NDC)
            {
                protocolFolder = "NDC";
            }
            else
            {
                protocolFolder = "DDC";
            }
            string filePath = XDCUnity.eCATPath + @"\XDC\" + protocolFolder + @"\Scripts\Screen\Host\000\" + content + ".txt";
            if (!File.Exists(filePath))
                return;
            string txt = XDCUnity.GetTxtFileText(filePath);
            List<object> result = XDCUnity.ScreenOperator.GetView(txt);
            if (result != null && result.Count > 0)
                ExcuteScreenCmd(result);

        }

        #region BaseDraw

        /// <summary>
        /// 绘制字符串
        /// </summary>
        /// <param name="toDrawString"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        private void DrawString(Graphics g, string toDrawString, string startRow, string startColumn, Color textColor, System.Drawing.FontStyle fontStyle = FontStyle.Regular)
        {
            int startPositionY = 0;
            int startPositionX = 0;
            if (rowDic.ContainsKey(startRow))
            {
                startPositionY = rowDic[startRow] * currentSignalRowHeiht;
            }
            if (columnDic.ContainsKey(startColumn))
            {
                startPositionX = columnDic[startColumn] * currentSignalColumnWidth;
            }

            for (int i = 0; i < toDrawString.Length; i++)
            {
                CreateImage(g, toDrawString[i].ToString(), startPositionX, startPositionY, currentSignalColumnWidth, currentSignalRowHeiht, textColor, fontStyle);
                startPositionX += currentSignalColumnWidth;
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
        private void CreateImage(Graphics g, string reStr, int x, int y, int width, int heigh, Color textColor, System.Drawing.FontStyle fontStyle = FontStyle.Regular)
        {
            try
            {
                Font font = new System.Drawing.Font("微软雅黑", FontSize, (fontStyle));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(x, y, width, heigh), textColor, Color.DarkRed, 1.8f, true);
                g.DrawString(reStr, font, brush, x, y);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #endregion

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rtb_Text.Text.Trim()))
                XDCUnity.WriteTextFileText(currentScreenPath, rtb_Text.Text);
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            if (string.IsNullOrEmpty(currentScreenPath))
                return;
            int flit_1 = currentScreenPath.LastIndexOf('\\');
            int flit_2 = currentScreenPath.LastIndexOf('.');
            int newScreenNum = int.Parse(currentScreenPath.Substring(flit_1 + 1, flit_2 - flit_1 - 1));
            string newPath = string.Empty;
            switch (currentButton.Name)
            {
                case "btn_Pre":
                    {
                        newPath = currentScreenPath.Substring(0, flit_1 + 1) + string.Format("{0:D3}", newScreenNum - 1) + ".txt";
                    }
                    break;
                case "btn_Next":
                    {
                        newPath = currentScreenPath.Substring(0, flit_1 + 1) + string.Format("{0:D3}", newScreenNum + 1) + ".txt";
                    }
                    break;
                default:
                    break;
            }


            if (File.Exists(newPath))
            {
                rtb_Text.Text = XDCUnity.GetTxtFileText(newPath);
                currentScreenPath = newPath;
                this.Text = "ScreenParse - [" + newPath + "]";
            }

        }
    }
}
