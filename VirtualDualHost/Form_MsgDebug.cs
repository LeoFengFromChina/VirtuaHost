using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using StandardFeature;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace VirtualDualHost
{
    public partial class Form_MsgDebug : DockContent
    {
        public bool isAlreadyLoad = false;
        public bool isDebug = false;
        public delegate void SubForm(object dataContent);
        public event SubForm SubFormEvent;

        public delegate void SubFormClose();
        public event SubFormClose SubFormCloseEvent;
        public string currentFilePath = string.Empty;
        public Form_MsgDebug(string msgText, XDCProtocolType protocolType, DataType dataType = DataType.Message, string subTitle = "", bool isdebug = false)
        {
            InitializeComponent();
            XDCUnity.Initial();
            rtb_Msg.Text = msgText;
            currentProtocolType = protocolType;
            isDebug = isdebug;
            if (!string.IsNullOrEmpty(subTitle))
            {
                this.Text += " - [" + subTitle + "]";
            }
            if (protocolType == XDCProtocolType.DDC)
            {
                rb_DDC.Checked = true;
                folderName = "DDC";
            }
            else
            {
                rb_NDC.Checked = true;
                folderName = "NDC";
            }
            if (dataType == DataType.Message)
            {
                rb_Message.Checked = true;
            }
            else if (dataType == DataType.State)
            {
                rb_State.Checked = true;
            }
            else if (dataType == DataType.Screen)
            {
                rb_Screen.Checked = true;
            }
            else if (dataType == DataType.Fit)
            {
                rb_Fit.Checked = true;
            }
            BeginPars();
        }
        Form_Pars form_Pars;
        private DockPanel dp;
        bool IsSendBack = false;
        string folderName = string.Empty;
        XDCProtocolType currentProtocolType;
        public Form_MsgDebug(Form_Pars formMain)
        {
            if (null != formMain)
            {
                form_Pars = formMain;
                dp = (DockPanel)formMain.Controls["dockPanel1"];
                form_Pars.ParentFormEvent += Form_Pars_ParentFormEvent;
            }
        }

        private void Form_Pars_ParentFormEvent(object path, DataType dataType)
        {
            MessageBox.Show("path");
        }
        public void ParsFromSubForm(object dataContext, XDCProtocolType protocolType, DataType dataType, string filePath)
        {
            #region 要格式化的内容

            rtb_Msg.Text = dataContext.ToString();
            currentFilePath = filePath;
            #endregion
            #region 协议选择
            switch (protocolType)
            {
                case XDCProtocolType.DDCorNDC:
                    {
                        //如果是DDCorNDC，不做处理。原来是什么就什么
                    }
                    break;
                case XDCProtocolType.NDC:
                    {
                        rb_NDC.Checked = true;
                        folderName = "NDC";
                        currentProtocolType = XDCProtocolType.NDC;
                    }
                    break;
                case XDCProtocolType.DDC:
                    {
                        rb_DDC.Checked = true;
                        folderName = "DDC";
                        currentProtocolType = XDCProtocolType.DDC;
                    }
                    break;
                default:
                    break;
            }
            #endregion
            #region 数据类型RadioButton

            switch (dataType)
            {
                case DataType.State:
                    {
                        rb_State.Checked = true;
                    }
                    break;
                case DataType.Screen:
                    {
                        rb_Screen.Checked = true;
                    }
                    break;
                case DataType.Fit:
                    {
                        rb_Fit.Checked = true;
                    }
                    break;
                case DataType.Message:
                    {
                        rb_Message.Checked = true;
                    }
                    break;
                default:
                    break;
            }

            #endregion


            BeginPars();
        }

        private void Form_MsgDebug_Load(object sender, EventArgs e)
        {
            IsSendBack = false;
            dgv_Fileds.ClearSelection();
            rb_DDC.MouseClick += Rb_DDC_Click;
            rb_NDC.MouseClick += Rb_DDC_Click;
            rb_State.MouseClick += Rb_DDC_Click;
            rb_Screen.MouseClick += Rb_DDC_Click;
            rb_Fit.MouseClick += Rb_DDC_Click;
            rb_Message.MouseClick += Rb_DDC_Click;
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            dgv_Fileds.KeyDown += Dgv_Fileds_KeyDown;
            this.dgv_Fileds.Columns[1].HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.rtb_Msg.DragDrop += Rtb_Msg_DragDrop;
            
        }
        
        private void Rtb_Msg_DragDrop(object sender, DragEventArgs e)
        {
            Array fileName = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (null != fileName)
                rtb_Msg.Text = XDCUnity.GetTxtFileText(fileName.GetValue(0).ToString());
            else
                rtb_Msg.Text = e.Data.GetData(DataFormats.Text).ToString();
            e.Effect = DragDropEffects.None;
            BeginPars();
        }


        /// <summary>
        /// 保存内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Sure to Save?", "XDC Virtual Host", MessageBoxButtons.OKCancel))
            {
                if (XDCUnity.WriteTextFileText(currentFilePath, rtb_Msg.Text.Trim()))
                {
                    MessageBox.Show("Save Successed.");
                }
                else
                {
                    MessageBox.Show("Save Failed.");
                }
            }
        }

        private void Rb_DDC_Click(object sender, EventArgs e)
        {
            BeginPars();
        }

        private void button_Paras_Click(object sender, EventArgs e)
        {
            BeginPars();
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void ResetFields()
        {
            dgv_Fileds.DataSource = null;

            List<ParsRowView> ViewList = new List<ParsRowView>();
            for (int i = 0; i < 20; i++)
            {
                ViewList.Add(new ParsRowView());
            }
            dgv_Fileds.DataSource = ViewList;
            for (int i = 0; i < dgv_Fileds.Columns.Count; i++)
            {
                dgv_Fileds.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgv_Fileds.Columns[0].HeaderText = "Name";
            dgv_Fileds.Columns[1].HeaderText = "Value";
            dgv_Fileds.Columns[2].HeaderText = "Comment";
            dgv_Fileds.Columns[0].FillWeight = 35;
            dgv_Fileds.Columns[1].FillWeight = 30;
            dgv_Fileds.Columns[2].FillWeight = 35;
            dgv_Fileds.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }

        private void dgv_Fileds_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            object CommentObj = null;
            object FiledNameObj = dgv_Fileds.Rows[e.RowIndex].Cells[0].Value;
            if (null != FiledNameObj && XDCUnity.BaseConfig["NoColorColumns"].Contains(FiledNameObj.ToString()))
                return;
            if (dgv_Fileds.Rows[e.RowIndex].Cells.Count > 2)
            {
                CommentObj = dgv_Fileds.Rows[e.RowIndex].Cells[2].Value;
            }
            if (null != FiledNameObj && FiledNameObj.ToString().Equals("FS"))
            {
                dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Lime;
            }
            else if (null != FiledNameObj && FiledNameObj.ToString().Equals("GS"))
            {
                dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Turquoise;
            }
            else if (null != FiledNameObj && FiledNameObj.ToString().Equals("RS"))
            {
                dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
            }
            else if (null != FiledNameObj && FiledNameObj.ToString().Equals("VT"))
            {
                dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Silver;
            }
            else if (null != FiledNameObj && FiledNameObj.ToString().Equals("TG"))
            {
                dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.SlateBlue;
            }
            else if (null != FiledNameObj && FiledNameObj.ToString().Equals("IC"))
            {
                dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            }
            else if (null != CommentObj)
            {
                //根据配置，指定说明内容时，颜色匹配
                if (XDCUnity.BaseConfig["ColorYellow"].Contains(CommentObj.ToString()))
                {
                    dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (XDCUnity.BaseConfig["ColorRed"].Contains(CommentObj.ToString()))
                {
                    dgv_Fileds.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void dgv_Fileds_Leave(object sender, EventArgs e)
        {
            dgv_Fileds.ClearSelection();
        }

        private void BeginPars()
        {
            isAlreadyLoad = false;
            XDCProtocolType pType = rb_NDC.Checked ? XDCProtocolType.NDC : XDCProtocolType.DDC;
            DataType dType = rb_State.Checked ? DataType.State : (rb_Screen.Checked ? DataType.Screen : (rb_Fit.Checked ? DataType.Fit : DataType.Message));
            BaseFunction.Intial(pType, dType);
            string parsText = rtb_Msg.Text;//.Trim();
            if (string.IsNullOrEmpty(parsText))
            {
                ResetFields();
                return;
            }
            List<ParsRowView> view = null;
            switch (XDCUnity.CurrentDataType)
            {
                case DataType.State:
                    {
                        view = XDCUnity.StateOperator.GetView(parsText);
                    }
                    break;
                case DataType.Screen:
                    break;
                case DataType.Fit:
                    {
                        view = XDCUnity.FitOperator.GetView(parsText);
                    }
                    break;
                case DataType.Message:
                    {
                        byte[] msgByteArray = Encoding.ASCII.GetBytes(parsText);
                        XDCMessage XDCmsg = XDCUnity.MessageFormat.Format(msgByteArray, parsText.Length, TcpHead.NoHead, true);
                        view = XDCUnity.MessageOperator.GetView(XDCmsg);
                    }
                    break;
                default:
                    break;
            }
            if (null != view)
                dgv_Fileds.DataSource = view;
            else
            {
                ResetFields();
            }
            dgv_Fileds.Columns[0].FillWeight = 35;
            dgv_Fileds.Columns[1].FillWeight = 30;
            dgv_Fileds.Columns[2].FillWeight = 35;
            dgv_Fileds.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Fileds.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Fileds.ClearSelection();
        }

        private void button_Go_Click(object sender, EventArgs e)
        {
            if (SubFormEvent != null)
            {
                SubFormEvent(rtb_Msg.Text);
                IsSendBack = true;
                if (isDebug)
                    this.Close();
                //if (SubFormCloseEvent != null)
                //    SubFormCloseEvent();
            }
        }

        private void Form_MsgDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SubFormEvent != null && !IsSendBack)
            {
                SubFormEvent(rtb_Msg.Text.Trim());
                IsSendBack = true;
            }
        }

        private void dgv_Fileds_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 2
                || e.ColumnIndex == 0)  // 代表第一、三列
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 编辑内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Fileds_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<ParsRowView> viewList = dgv_Fileds.DataSource as List<ParsRowView>;

            string MsgContent = string.Empty;

            foreach (ParsRowView prv in viewList)
            {
                if (prv.FieldName == "FS")
                    MsgContent += XDCSplictorChar.FS;
                else if (prv.FieldName == "GS")
                    MsgContent += XDCSplictorChar.GS;
                else if (prv.FieldName == "RS")
                    MsgContent += XDCSplictorChar.RS;
                else if (prv.FieldName == "VT")
                    MsgContent += XDCSplictorChar.VT2;
                else
                    MsgContent += prv.FieldValue;
            }
            if (rb_Fit.Checked)
            {
                //fit 表要转换。Edit by frde 20160919
                string tempContent = string.Empty; ;
                int startIndex = 0;
                bool isfailed = false;
                while (startIndex <= MsgContent.Length - 2)
                {
                    if (MsgContent.Length % 2 != 0)
                    {
                        isfailed = true;
                        MessageBox.Show("the Len is Wrong after your edit.");
                        break;
                    }
                    tempContent += Convert.ToInt32(MsgContent.Substring(startIndex, 2), 16).ToString("D3");
                    startIndex += 2;
                }

                if (!isfailed)
                {
                    rtb_Msg.Text = tempContent;
                }
            }
            else
                rtb_Msg.Text = MsgContent;
        }

        private void dgv_Fileds_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
                return;
            string FieldName = (dgv_Fileds.DataSource as List<ParsRowView>)[e.RowIndex].FieldName;
            string FieldValue = (dgv_Fileds.DataSource as List<ParsRowView>)[e.RowIndex].FieldValue;
            ShowDetailStateOrScreen(FieldName, FieldValue);
        }
        private void Dgv_Fileds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string FieldName = (dgv_Fileds.DataSource as List<ParsRowView>)[dgv_Fileds.CurrentRow.Index].FieldName;
                string FieldValue = (dgv_Fileds.DataSource as List<ParsRowView>)[dgv_Fileds.CurrentRow.Index].FieldValue;
                ShowDetailStateOrScreen(FieldName, FieldValue);
            }
        }

        private void ShowDetailStateOrScreen(string FieldName, string FieldValue)
        {
            if (FieldName.Contains("Screen"))
            {
                string screenPath = XDCUnity.eCATPath + @"\XDC\" + folderName + @"\Scripts\Screen" + XDCUnity.CurrentResourceIndex + @"\Host\000\" + FieldValue + ".txt";
                if (File.Exists(screenPath))
                {
                    string screenText = XDCUnity.GetTxtFileText(screenPath);
                    Form_ScreenParse form_ScreenParse = new Form_ScreenParse(screenText, currentProtocolType, FieldValue + ".txt");
                    form_ScreenParse.Show();
                }
                else
                {
                    MessageBox.Show("Do note Exists Screen File:" + screenPath);
                }
            }
            else if (FieldName.Contains("State"))
            {
                string statePath = XDCUnity.eCATPath + @"\XDC\" + folderName + @"\Scripts\State" + XDCUnity.CurrentResourceIndex + @"\Host\" + FieldValue + ".txt";
                if (File.Exists(statePath))
                {
                    string stateText = XDCUnity.GetTxtFileText(statePath);
                    Form_MsgDebug form_Debug = new Form_MsgDebug(stateText, currentProtocolType, DataType.State, FieldValue + ".txt");
                    form_Debug.Show();
                }
                else
                {
                    MessageBox.Show("Do note Exists State File:" + statePath);
                }
            }
        }
        private void getInteractiveMsgBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_GetInteractiveMsgTextToShow(rtb_Msg.Text).Show();
        }

        private void getMsgStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ParsRowView> rowViewS = dgv_Fileds.DataSource as List<ParsRowView>;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<Msg MessageID=\"{MessageID}\" NextMessageID=\"{NextMessageID}\">");
            sb.AppendLine("\t<FieldList>");
            string tempRow = "\t\t<Field FieldName=\"{#FieldName#}\" FieldValue=\"{#FieldValue#}\"  />";
            foreach (ParsRowView item in rowViewS)
            {
                sb.AppendLine(tempRow.Replace("{#FieldName#}", item.FieldName).Replace("{#FieldValue#}", item.FieldValue));
            }
            sb.AppendLine("\t</FieldList>");
            sb.AppendLine("\t<ReplyFields>");
            sb.AppendLine("\t</ReplyFields>");
            sb.AppendLine("</Msg>");

            string tempResult = sb.ToString();
            Clipboard.SetDataObject(tempResult);
            MessageBox.Show("Successed ! ");
        }
    }
}
