using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XmlHelper;
using StandardFeature;
using WeifenLuo.WinFormsUI.Docking;

namespace VirtualDualHost
{
    public partial class Form_MsgDebug : DockContent
    {
        public bool isAlreadyLoad = false;
        public Form_MsgDebug(string msgText, XDCProtocolType protocolType)
        {
            InitializeComponent();
            XDCUnity.Initial();
            rtb_Msg.Text = msgText;
            if (protocolType == XDCProtocolType.DDC)
            {
                rb_DDC.Checked = true;
            }
            else
            {
                rb_NDC.Checked = true;
            }
            BeginPars();
        }
        Form_Pars form_Pars;
        private DockPanel dp;
        public Form_MsgDebug(Form_Pars formMain)
        {
            form_Pars = formMain;
            dp = (DockPanel)formMain.Controls["dockPanel1"];
            form_Pars.ParentFormEvent += Form_Pars_ParentFormEvent;
        }

        private void Form_Pars_ParentFormEvent(object path, DataType dataType)
        {
            MessageBox.Show("path");
        }
        public void ParsFromSubForm(object dataContext, XDCProtocolType protocolType, DataType dataType)
        {
            #region 要格式化的内容

            rtb_Msg.Text = dataContext.ToString();
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
                    }
                    break;
                case XDCProtocolType.DDC:
                    {
                        rb_DDC.Checked = true;
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
            dgv_Fileds.ClearSelection();
            rb_DDC.MouseClick += Rb_DDC_Click;
            rb_NDC.MouseClick += Rb_DDC_Click;
            rb_State.MouseClick += Rb_DDC_Click;
            rb_Screen.MouseClick += Rb_DDC_Click;
            rb_Fit.MouseClick += Rb_DDC_Click;
            rb_Message.MouseClick += Rb_DDC_Click;
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
            string parsText = rtb_Msg.Text.Trim();
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
                        XDCMessage XDCmsg = XDCUnity.MessageFormat.Format(msgByteArray, parsText.Length);
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
            dgv_Fileds.ClearSelection();
        }
    }
}
