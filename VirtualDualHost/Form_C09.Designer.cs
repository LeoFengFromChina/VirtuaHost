namespace VirtualDualHost
{
    partial class Form_C09
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_C09));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_ActiveTransactionCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv_OPC = new System.Windows.Forms.DataGridView();
            this.OperationCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BufferBLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BufferBData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BufferCLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BufferCData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_States = new System.Windows.Forms.DataGridView();
            this.NextStateNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextStateAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_TRSNO = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_PINFlag = new System.Windows.Forms.Label();
            this.txt_PINFlag = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Track3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Track2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Track1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_States)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(512, 55);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(530, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 55);
            this.button1.TabIndex = 1;
            this.button1.Text = "Parse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_ActiveTransactionCount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dgv_OPC);
            this.groupBox1.Controls.Add(this.dgv_States);
            this.groupBox1.Controls.Add(this.txt_TRSNO);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbl_PINFlag);
            this.groupBox1.Controls.Add(this.txt_PINFlag);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txt_Track3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_Track2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_Track1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 422);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Content:";
            // 
            // txt_ActiveTransactionCount
            // 
            this.txt_ActiveTransactionCount.Location = new System.Drawing.Point(361, 81);
            this.txt_ActiveTransactionCount.Name = "txt_ActiveTransactionCount";
            this.txt_ActiveTransactionCount.Size = new System.Drawing.Size(173, 21);
            this.txt_ActiveTransactionCount.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "Count:";
            // 
            // dgv_OPC
            // 
            this.dgv_OPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_OPC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OperationCode,
            this.BufferBLen,
            this.BufferBData,
            this.BufferCLen,
            this.BufferCData,
            this.dataGridViewTextBoxColumn3});
            this.dgv_OPC.Location = new System.Drawing.Point(8, 264);
            this.dgv_OPC.Name = "dgv_OPC";
            this.dgv_OPC.RowHeadersVisible = false;
            this.dgv_OPC.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_OPC.RowTemplate.Height = 23;
            this.dgv_OPC.RowTemplate.ReadOnly = true;
            this.dgv_OPC.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_OPC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_OPC.Size = new System.Drawing.Size(579, 150);
            this.dgv_OPC.TabIndex = 12;
            // 
            // OperationCode
            // 
            this.OperationCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OperationCode.DataPropertyName = "OperationCode";
            this.OperationCode.HeaderText = "OperationCode";
            this.OperationCode.Name = "OperationCode";
            // 
            // BufferBLen
            // 
            this.BufferBLen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BufferBLen.DataPropertyName = "BufferBLen";
            this.BufferBLen.HeaderText = "BufferBLen";
            this.BufferBLen.Name = "BufferBLen";
            // 
            // BufferBData
            // 
            this.BufferBData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BufferBData.DataPropertyName = "BufferBData";
            this.BufferBData.HeaderText = "BufferBData";
            this.BufferBData.Name = "BufferBData";
            // 
            // BufferCLen
            // 
            this.BufferCLen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BufferCLen.DataPropertyName = "BufferCLen";
            this.BufferCLen.HeaderText = "BufferCLen";
            this.BufferCLen.Name = "BufferCLen";
            // 
            // BufferCData
            // 
            this.BufferCData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BufferCData.DataPropertyName = "BufferCData";
            this.BufferCData.HeaderText = "BufferCData";
            this.BufferCData.Name = "BufferCData";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "AmountBufferFlag";
            this.dataGridViewTextBoxColumn3.HeaderText = "AmountBufferFlag";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dgv_States
            // 
            this.dgv_States.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_States.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NextStateNumber,
            this.NextStateAction});
            this.dgv_States.Location = new System.Drawing.Point(8, 108);
            this.dgv_States.Name = "dgv_States";
            this.dgv_States.RowHeadersVisible = false;
            this.dgv_States.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_States.RowTemplate.Height = 23;
            this.dgv_States.RowTemplate.ReadOnly = true;
            this.dgv_States.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_States.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_States.Size = new System.Drawing.Size(579, 150);
            this.dgv_States.TabIndex = 11;
            // 
            // NextStateNumber
            // 
            this.NextStateNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NextStateNumber.DataPropertyName = "NextStateNumber";
            this.NextStateNumber.HeaderText = "NextStateNumber";
            this.NextStateNumber.Name = "NextStateNumber";
            // 
            // NextStateAction
            // 
            this.NextStateAction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NextStateAction.DataPropertyName = "NextStateAction";
            this.NextStateAction.HeaderText = "NextStateAction";
            this.NextStateAction.Name = "NextStateAction";
            // 
            // txt_TRSNO
            // 
            this.txt_TRSNO.Location = new System.Drawing.Point(361, 29);
            this.txt_TRSNO.Name = "txt_TRSNO";
            this.txt_TRSNO.Size = new System.Drawing.Size(173, 21);
            this.txt_TRSNO.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(308, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "TRSNO:";
            // 
            // lbl_PINFlag
            // 
            this.lbl_PINFlag.AutoSize = true;
            this.lbl_PINFlag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lbl_PINFlag.Location = new System.Drawing.Point(464, 61);
            this.lbl_PINFlag.Name = "lbl_PINFlag";
            this.lbl_PINFlag.Size = new System.Drawing.Size(47, 12);
            this.lbl_PINFlag.TabIndex = 8;
            this.lbl_PINFlag.Text = "Track1:";
            // 
            // txt_PINFlag
            // 
            this.txt_PINFlag.Location = new System.Drawing.Point(361, 56);
            this.txt_PINFlag.Name = "txt_PINFlag";
            this.txt_PINFlag.Size = new System.Drawing.Size(98, 21);
            this.txt_PINFlag.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "PINFlag:";
            // 
            // txt_Track3
            // 
            this.txt_Track3.Location = new System.Drawing.Point(59, 81);
            this.txt_Track3.Name = "txt_Track3";
            this.txt_Track3.Size = new System.Drawing.Size(210, 21);
            this.txt_Track3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Track3:";
            // 
            // txt_Track2
            // 
            this.txt_Track2.Location = new System.Drawing.Point(59, 53);
            this.txt_Track2.Name = "txt_Track2";
            this.txt_Track2.Size = new System.Drawing.Size(210, 21);
            this.txt_Track2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Track2:";
            // 
            // txt_Track1
            // 
            this.txt_Track1.Location = new System.Drawing.Point(59, 26);
            this.txt_Track1.Name = "txt_Track1";
            this.txt_Track1.Size = new System.Drawing.Size(210, 21);
            this.txt_Track1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Track1:";
            // 
            // Form_C09
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 498);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_C09";
            this.Text = "C09_Parse";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_States)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgv_States;
        private System.Windows.Forms.TextBox txt_TRSNO;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_PINFlag;
        private System.Windows.Forms.TextBox txt_PINFlag;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Track3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Track2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Track1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_OPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn BufferBLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn BufferBData;
        private System.Windows.Forms.DataGridViewTextBoxColumn BufferCLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn BufferCData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TextBox txt_ActiveTransactionCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextStateNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextStateAction;
    }
}