﻿namespace VirtualDualHost
{
    partial class From_Seeting_eCATPath
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(From_Seeting_eCATPath));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_eCAT = new System.Windows.Forms.Button();
            this.btn_TrueBack = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.cmb_eCAT = new System.Windows.Forms.ComboBox();
            this.cmb_Trueback = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbt_ASCII = new System.Windows.Forms.RadioButton();
            this.rbt_EBCD = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "    eCAT Path：";
            // 
            // btn_eCAT
            // 
            this.btn_eCAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_eCAT.Location = new System.Drawing.Point(370, 70);
            this.btn_eCAT.Name = "btn_eCAT";
            this.btn_eCAT.Size = new System.Drawing.Size(31, 23);
            this.btn_eCAT.TabIndex = 2;
            this.btn_eCAT.Text = "...";
            this.btn_eCAT.UseVisualStyleBackColor = true;
            this.btn_eCAT.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_TrueBack
            // 
            this.btn_TrueBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_TrueBack.Location = new System.Drawing.Point(370, 113);
            this.btn_TrueBack.Name = "btn_TrueBack";
            this.btn_TrueBack.Size = new System.Drawing.Size(31, 23);
            this.btn_TrueBack.TabIndex = 5;
            this.btn_TrueBack.Text = "...";
            this.btn_TrueBack.UseVisualStyleBackColor = true;
            this.btn_TrueBack.Click += new System.EventHandler(this.btn_TrueBack_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "TrueBack Path：";
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_OK.Location = new System.Drawing.Point(194, 144);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(62, 23);
            this.btn_OK.TabIndex = 102;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // cmb_eCAT
            // 
            this.cmb_eCAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_eCAT.FormattingEnabled = true;
            this.cmb_eCAT.Location = new System.Drawing.Point(108, 72);
            this.cmb_eCAT.Name = "cmb_eCAT";
            this.cmb_eCAT.Size = new System.Drawing.Size(256, 20);
            this.cmb_eCAT.TabIndex = 103;
            // 
            // cmb_Trueback
            // 
            this.cmb_Trueback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Trueback.FormattingEnabled = true;
            this.cmb_Trueback.Location = new System.Drawing.Point(108, 113);
            this.cmb_Trueback.Name = "cmb_Trueback";
            this.cmb_Trueback.Size = new System.Drawing.Size(256, 20);
            this.cmb_Trueback.TabIndex = 104;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 105;
            this.label3.Text = "   FormatCode：";
            // 
            // rbt_ASCII
            // 
            this.rbt_ASCII.AutoSize = true;
            this.rbt_ASCII.Location = new System.Drawing.Point(108, 43);
            this.rbt_ASCII.Name = "rbt_ASCII";
            this.rbt_ASCII.Size = new System.Drawing.Size(53, 16);
            this.rbt_ASCII.TabIndex = 106;
            this.rbt_ASCII.TabStop = true;
            this.rbt_ASCII.Text = "ASCII";
            this.rbt_ASCII.UseVisualStyleBackColor = true;
            // 
            // rbt_EBCD
            // 
            this.rbt_EBCD.AutoSize = true;
            this.rbt_EBCD.Location = new System.Drawing.Point(194, 43);
            this.rbt_EBCD.Name = "rbt_EBCD";
            this.rbt_EBCD.Size = new System.Drawing.Size(47, 16);
            this.rbt_EBCD.TabIndex = 107;
            this.rbt_EBCD.TabStop = true;
            this.rbt_EBCD.Text = "EBCD";
            this.rbt_EBCD.UseVisualStyleBackColor = true;
            // 
            // From_Seeting_eCATPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 191);
            this.Controls.Add(this.rbt_EBCD);
            this.Controls.Add(this.rbt_ASCII);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmb_Trueback);
            this.Controls.Add(this.cmb_eCAT);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_TrueBack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_eCAT);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "From_Seeting_eCATPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "eCAT Setting";
            this.Load += new System.EventHandler(this.From_Seeting_eCATPath_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_eCAT;
        private System.Windows.Forms.Button btn_TrueBack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.ComboBox cmb_eCAT;
        private System.Windows.Forms.ComboBox cmb_Trueback;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbt_ASCII;
        private System.Windows.Forms.RadioButton rbt_EBCD;
    }
}