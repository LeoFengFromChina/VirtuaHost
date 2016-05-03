namespace VirtualDualHost
{
    partial class Form_MsgDebug
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MsgDebug));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_DDC = new System.Windows.Forms.RadioButton();
            this.rb_NDC = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_Message = new System.Windows.Forms.RadioButton();
            this.rb_Fit = new System.Windows.Forms.RadioButton();
            this.rb_Screen = new System.Windows.Forms.RadioButton();
            this.rb_State = new System.Windows.Forms.RadioButton();
            this.rtb_Msg = new System.Windows.Forms.RichTextBox();
            this.cms_Save = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getInteractiveMsgBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dgv_Fileds = new System.Windows.Forms.DataGridView();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Paras = new System.Windows.Forms.Button();
            this.button_Go = new System.Windows.Forms.Button();
            this.cms_GetMsgStructure = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.getMsgStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.cms_Save.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Fileds)).BeginInit();
            this.cms_GetMsgStructure.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_DDC);
            this.groupBox1.Controls.Add(this.rb_NDC);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Protocol Type";
            // 
            // rb_DDC
            // 
            this.rb_DDC.AutoSize = true;
            this.rb_DDC.Location = new System.Drawing.Point(67, 17);
            this.rb_DDC.Name = "rb_DDC";
            this.rb_DDC.Size = new System.Drawing.Size(41, 16);
            this.rb_DDC.TabIndex = 1;
            this.rb_DDC.Text = "DDC";
            this.rb_DDC.UseVisualStyleBackColor = true;
            // 
            // rb_NDC
            // 
            this.rb_NDC.AutoSize = true;
            this.rb_NDC.Checked = true;
            this.rb_NDC.Location = new System.Drawing.Point(10, 17);
            this.rb_NDC.Name = "rb_NDC";
            this.rb_NDC.Size = new System.Drawing.Size(41, 16);
            this.rb_NDC.TabIndex = 0;
            this.rb_NDC.TabStop = true;
            this.rb_NDC.Text = "NDC";
            this.rb_NDC.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_Message);
            this.groupBox2.Controls.Add(this.rb_Fit);
            this.groupBox2.Controls.Add(this.rb_Screen);
            this.groupBox2.Controls.Add(this.rb_State);
            this.groupBox2.Location = new System.Drawing.Point(141, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 40);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Type";
            // 
            // rb_Message
            // 
            this.rb_Message.AutoSize = true;
            this.rb_Message.Checked = true;
            this.rb_Message.Location = new System.Drawing.Point(185, 17);
            this.rb_Message.Name = "rb_Message";
            this.rb_Message.Size = new System.Drawing.Size(65, 16);
            this.rb_Message.TabIndex = 3;
            this.rb_Message.TabStop = true;
            this.rb_Message.Text = "Message";
            this.rb_Message.UseVisualStyleBackColor = true;
            // 
            // rb_Fit
            // 
            this.rb_Fit.AutoSize = true;
            this.rb_Fit.Location = new System.Drawing.Point(138, 17);
            this.rb_Fit.Name = "rb_Fit";
            this.rb_Fit.Size = new System.Drawing.Size(41, 16);
            this.rb_Fit.TabIndex = 2;
            this.rb_Fit.Text = "Fit";
            this.rb_Fit.UseVisualStyleBackColor = true;
            // 
            // rb_Screen
            // 
            this.rb_Screen.AutoSize = true;
            this.rb_Screen.Location = new System.Drawing.Point(73, 17);
            this.rb_Screen.Name = "rb_Screen";
            this.rb_Screen.Size = new System.Drawing.Size(59, 16);
            this.rb_Screen.TabIndex = 1;
            this.rb_Screen.Text = "Screen";
            this.rb_Screen.UseVisualStyleBackColor = true;
            // 
            // rb_State
            // 
            this.rb_State.AutoSize = true;
            this.rb_State.Location = new System.Drawing.Point(14, 17);
            this.rb_State.Name = "rb_State";
            this.rb_State.Size = new System.Drawing.Size(53, 16);
            this.rb_State.TabIndex = 0;
            this.rb_State.Text = "State";
            this.rb_State.UseVisualStyleBackColor = true;
            // 
            // rtb_Msg
            // 
            this.rtb_Msg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_Msg.ContextMenuStrip = this.cms_Save;
            this.rtb_Msg.Location = new System.Drawing.Point(12, 58);
            this.rtb_Msg.Name = "rtb_Msg";
            this.rtb_Msg.Size = new System.Drawing.Size(568, 96);
            this.rtb_Msg.TabIndex = 2;
            this.rtb_Msg.Text = "";
            // 
            // cms_Save
            // 
            this.cms_Save.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.getInteractiveMsgBufferToolStripMenuItem});
            this.cms_Save.Name = "cms_Save";
            this.cms_Save.Size = new System.Drawing.Size(216, 48);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // getInteractiveMsgBufferToolStripMenuItem
            // 
            this.getInteractiveMsgBufferToolStripMenuItem.Name = "getInteractiveMsgBufferToolStripMenuItem";
            this.getInteractiveMsgBufferToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.getInteractiveMsgBufferToolStripMenuItem.Text = "GetInteractiveMsgBuffer";
            this.getInteractiveMsgBufferToolStripMenuItem.Click += new System.EventHandler(this.getInteractiveMsgBufferToolStripMenuItem_Click);
            // 
            // dgv_Fileds
            // 
            this.dgv_Fileds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Fileds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Fileds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldName,
            this.FieldValue,
            this.FieldComment});
            this.dgv_Fileds.ContextMenuStrip = this.cms_GetMsgStructure;
            this.dgv_Fileds.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgv_Fileds.Location = new System.Drawing.Point(12, 160);
            this.dgv_Fileds.Name = "dgv_Fileds";
            this.dgv_Fileds.RowHeadersVisible = false;
            this.dgv_Fileds.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.dgv_Fileds.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Fileds.RowTemplate.Height = 16;
            this.dgv_Fileds.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Fileds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Fileds.Size = new System.Drawing.Size(568, 281);
            this.dgv_Fileds.TabIndex = 3;
            this.dgv_Fileds.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgv_Fileds_CellBeginEdit);
            this.dgv_Fileds.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Fileds_CellEndEdit);
            this.dgv_Fileds.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_Fileds_CellMouseDoubleClick);
            this.dgv_Fileds.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_Fileds_RowPostPaint);
            // 
            // FieldName
            // 
            this.FieldName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FieldName.DataPropertyName = "FieldName";
            this.FieldName.HeaderText = "Name";
            this.FieldName.Name = "FieldName";
            // 
            // FieldValue
            // 
            this.FieldValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FieldValue.DataPropertyName = "FieldValue";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.FieldValue.DefaultCellStyle = dataGridViewCellStyle1;
            this.FieldValue.HeaderText = "Value";
            this.FieldValue.Name = "FieldValue";
            // 
            // FieldComment
            // 
            this.FieldComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FieldComment.DataPropertyName = "FieldComment";
            this.FieldComment.HeaderText = "Comment";
            this.FieldComment.Name = "FieldComment";
            // 
            // button_Paras
            // 
            this.button_Paras.Location = new System.Drawing.Point(409, 18);
            this.button_Paras.Name = "button_Paras";
            this.button_Paras.Size = new System.Drawing.Size(75, 33);
            this.button_Paras.TabIndex = 4;
            this.button_Paras.Text = "Paras";
            this.button_Paras.UseVisualStyleBackColor = true;
            this.button_Paras.Click += new System.EventHandler(this.button_Paras_Click);
            // 
            // button_Go
            // 
            this.button_Go.Location = new System.Drawing.Point(490, 18);
            this.button_Go.Name = "button_Go";
            this.button_Go.Size = new System.Drawing.Size(75, 33);
            this.button_Go.TabIndex = 5;
            this.button_Go.Text = "Go";
            this.button_Go.UseVisualStyleBackColor = true;
            this.button_Go.Click += new System.EventHandler(this.button_Go_Click);
            // 
            // cms_GetMsgStructure
            // 
            this.cms_GetMsgStructure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getMsgStructureToolStripMenuItem});
            this.cms_GetMsgStructure.Name = "cms_GetMsgStructure";
            this.cms_GetMsgStructure.Size = new System.Drawing.Size(173, 48);
            // 
            // getMsgStructureToolStripMenuItem
            // 
            this.getMsgStructureToolStripMenuItem.Name = "getMsgStructureToolStripMenuItem";
            this.getMsgStructureToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.getMsgStructureToolStripMenuItem.Text = "GetMsgStructure";
            this.getMsgStructureToolStripMenuItem.Click += new System.EventHandler(this.getMsgStructureToolStripMenuItem_Click);
            // 
            // Form_MsgDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 453);
            this.Controls.Add(this.button_Go);
            this.Controls.Add(this.button_Paras);
            this.Controls.Add(this.dgv_Fileds);
            this.Controls.Add(this.rtb_Msg);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_MsgDebug";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TabText = "DataParse";
            this.Text = "DataParse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_MsgDebug_FormClosing);
            this.Load += new System.EventHandler(this.Form_MsgDebug_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.cms_Save.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Fileds)).EndInit();
            this.cms_GetMsgStructure.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_DDC;
        private System.Windows.Forms.RadioButton rb_NDC;
        private System.Windows.Forms.RadioButton rb_Message;
        private System.Windows.Forms.RadioButton rb_Fit;
        private System.Windows.Forms.RadioButton rb_Screen;
        private System.Windows.Forms.RadioButton rb_State;
        private System.Windows.Forms.RichTextBox rtb_Msg;
        private System.Windows.Forms.DataGridView dgv_Fileds;
        private System.Windows.Forms.Button button_Paras;
        private System.Windows.Forms.Button button_Go;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldComment;
        private System.Windows.Forms.ContextMenuStrip cms_Save;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getInteractiveMsgBufferToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cms_GetMsgStructure;
        private System.Windows.Forms.ToolStripMenuItem getMsgStructureToolStripMenuItem;
    }
}