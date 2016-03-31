namespace VirtualDualHost
{
    partial class Form_StateCheck
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_StateCheck));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtb_NDC = new System.Windows.Forms.RichTextBox();
            this.dgv_StateCanView_NDC = new System.Windows.Forms.DataGridView();
            this.StateType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateNumberList = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtb_DDC = new System.Windows.Forms.RichTextBox();
            this.dgv_StateCanView_DDC = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StateCanView_NDC)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StateCanView_DDC)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(21, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(746, 537);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.rtb_NDC);
            this.tabPage1.Controls.Add(this.dgv_StateCanView_NDC);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(738, 511);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "NDC_StateView";
            // 
            // rtb_NDC
            // 
            this.rtb_NDC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_NDC.Location = new System.Drawing.Point(6, 39);
            this.rtb_NDC.Name = "rtb_NDC";
            this.rtb_NDC.Size = new System.Drawing.Size(726, 72);
            this.rtb_NDC.TabIndex = 1;
            this.rtb_NDC.Text = "";
            // 
            // dgv_StateCanView_NDC
            // 
            this.dgv_StateCanView_NDC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_StateCanView_NDC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_StateCanView_NDC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StateType,
            this.StateNumberList});
            this.dgv_StateCanView_NDC.Location = new System.Drawing.Point(6, 117);
            this.dgv_StateCanView_NDC.Name = "dgv_StateCanView_NDC";
            this.dgv_StateCanView_NDC.RowHeadersVisible = false;
            this.dgv_StateCanView_NDC.RowTemplate.Height = 23;
            this.dgv_StateCanView_NDC.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_StateCanView_NDC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_StateCanView_NDC.Size = new System.Drawing.Size(726, 388);
            this.dgv_StateCanView_NDC.TabIndex = 0;
            // 
            // StateType
            // 
            this.StateType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StateType.DataPropertyName = "StateType";
            this.StateType.FillWeight = 50.76142F;
            this.StateType.HeaderText = "StateType";
            this.StateType.Name = "StateType";
            // 
            // StateNumberList
            // 
            this.StateNumberList.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StateNumberList.DataPropertyName = "StateNums";
            this.StateNumberList.FillWeight = 149.2386F;
            this.StateNumberList.HeaderText = "StateNumberList";
            this.StateNumberList.Name = "StateNumberList";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.rtb_DDC);
            this.tabPage2.Controls.Add(this.dgv_StateCanView_DDC);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(738, 511);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DDC_StateView";
            // 
            // rtb_DDC
            // 
            this.rtb_DDC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_DDC.Location = new System.Drawing.Point(6, 39);
            this.rtb_DDC.Name = "rtb_DDC";
            this.rtb_DDC.Size = new System.Drawing.Size(726, 72);
            this.rtb_DDC.TabIndex = 3;
            this.rtb_DDC.Text = "";
            // 
            // dgv_StateCanView_DDC
            // 
            this.dgv_StateCanView_DDC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_StateCanView_DDC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_StateCanView_DDC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgv_StateCanView_DDC.Location = new System.Drawing.Point(6, 117);
            this.dgv_StateCanView_DDC.Name = "dgv_StateCanView_DDC";
            this.dgv_StateCanView_DDC.RowHeadersVisible = false;
            this.dgv_StateCanView_DDC.RowTemplate.Height = 23;
            this.dgv_StateCanView_DDC.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_StateCanView_DDC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_StateCanView_DDC.Size = new System.Drawing.Size(726, 388);
            this.dgv_StateCanView_DDC.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "StateType";
            this.dataGridViewTextBoxColumn1.FillWeight = 50.76142F;
            this.dataGridViewTextBoxColumn1.HeaderText = "StateType";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "StateNums";
            this.dataGridViewTextBoxColumn2.FillWeight = 149.2386F;
            this.dataGridViewTextBoxColumn2.HeaderText = "StateNumberList";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "All State Types Used in Current DDC:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "All State Types Used in Current NDC:";
            // 
            // Form_StateCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_StateCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StateView";
            this.Load += new System.EventHandler(this.Form_StateCheck_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StateCanView_NDC)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StateCanView_DDC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtb_NDC;
        private System.Windows.Forms.DataGridView dgv_StateCanView_NDC;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateType;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateNumberList;
        private System.Windows.Forms.RichTextBox rtb_DDC;
        private System.Windows.Forms.DataGridView dgv_StateCanView_DDC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}