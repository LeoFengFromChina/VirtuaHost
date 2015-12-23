namespace VirtualDualHost
{
    partial class Form_ScreenParse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ScreenParse));
            this.btn_Parse = new System.Windows.Forms.Button();
            this.rtb_Text = new System.Windows.Forms.RichTextBox();
            this.rb_DDC = new System.Windows.Forms.RadioButton();
            this.rb_NDC = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chb_GridLine = new System.Windows.Forms.CheckBox();
            this.pnl_Final = new System.Windows.Forms.Panel();
            this.pnl_Column = new System.Windows.Forms.Panel();
            this.pnl_Row = new System.Windows.Forms.Panel();
            this.pnl_Screen = new System.Windows.Forms.Panel();
            this.lbl_Notice = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Parse
            // 
            this.btn_Parse.Location = new System.Drawing.Point(590, 12);
            this.btn_Parse.Name = "btn_Parse";
            this.btn_Parse.Size = new System.Drawing.Size(75, 40);
            this.btn_Parse.TabIndex = 1;
            this.btn_Parse.Text = "Parse";
            this.btn_Parse.UseVisualStyleBackColor = true;
            this.btn_Parse.Click += new System.EventHandler(this.btn_Parse_Click);
            // 
            // rtb_Text
            // 
            this.rtb_Text.Location = new System.Drawing.Point(154, 12);
            this.rtb_Text.Name = "rtb_Text";
            this.rtb_Text.Size = new System.Drawing.Size(430, 40);
            this.rtb_Text.TabIndex = 0;
            this.rtb_Text.Text = "";
            this.rtb_Text.TextChanged += new System.EventHandler(this.rtb_Text_TextChanged);
            // 
            // rb_DDC
            // 
            this.rb_DDC.AutoSize = true;
            this.rb_DDC.Checked = true;
            this.rb_DDC.Location = new System.Drawing.Point(67, 17);
            this.rb_DDC.Name = "rb_DDC";
            this.rb_DDC.Size = new System.Drawing.Size(41, 16);
            this.rb_DDC.TabIndex = 1;
            this.rb_DDC.TabStop = true;
            this.rb_DDC.Text = "DDC";
            this.rb_DDC.UseVisualStyleBackColor = true;
            // 
            // rb_NDC
            // 
            this.rb_NDC.AutoSize = true;
            this.rb_NDC.Location = new System.Drawing.Point(10, 17);
            this.rb_NDC.Name = "rb_NDC";
            this.rb_NDC.Size = new System.Drawing.Size(41, 16);
            this.rb_NDC.TabIndex = 0;
            this.rb_NDC.Text = "NDC";
            this.rb_NDC.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_DDC);
            this.groupBox1.Controls.Add(this.rb_NDC);
            this.groupBox1.Location = new System.Drawing.Point(25, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 40);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Protocol Type";
            // 
            // chb_GridLine
            // 
            this.chb_GridLine.AutoSize = true;
            this.chb_GridLine.Checked = true;
            this.chb_GridLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_GridLine.Location = new System.Drawing.Point(668, 14);
            this.chb_GridLine.Name = "chb_GridLine";
            this.chb_GridLine.Size = new System.Drawing.Size(96, 16);
            this.chb_GridLine.TabIndex = 6;
            this.chb_GridLine.Text = "ViewGridLine";
            this.chb_GridLine.UseVisualStyleBackColor = true;
            // 
            // pnl_Final
            // 
            this.pnl_Final.Location = new System.Drawing.Point(768, 79);
            this.pnl_Final.Name = "pnl_Final";
            this.pnl_Final.Size = new System.Drawing.Size(10, 480);
            this.pnl_Final.TabIndex = 7;
            this.pnl_Final.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_Final_Paint_1);
            // 
            // pnl_Column
            // 
            this.pnl_Column.Location = new System.Drawing.Point(668, 79);
            this.pnl_Column.Name = "pnl_Column";
            this.pnl_Column.Size = new System.Drawing.Size(94, 480);
            this.pnl_Column.TabIndex = 8;
            // 
            // pnl_Row
            // 
            this.pnl_Row.Location = new System.Drawing.Point(25, 54);
            this.pnl_Row.Name = "pnl_Row";
            this.pnl_Row.Size = new System.Drawing.Size(640, 22);
            this.pnl_Row.TabIndex = 9;
            // 
            // pnl_Screen
            // 
            this.pnl_Screen.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnl_Screen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnl_Screen.Location = new System.Drawing.Point(25, 79);
            this.pnl_Screen.Name = "pnl_Screen";
            this.pnl_Screen.Size = new System.Drawing.Size(640, 480);
            this.pnl_Screen.TabIndex = 10;
            // 
            // lbl_Notice
            // 
            this.lbl_Notice.AutoSize = true;
            this.lbl_Notice.Location = new System.Drawing.Point(23, 567);
            this.lbl_Notice.Name = "lbl_Notice";
            this.lbl_Notice.Size = new System.Drawing.Size(41, 12);
            this.lbl_Notice.TabIndex = 11;
            this.lbl_Notice.Text = "label1";
            // 
            // Form_ScreenParse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 601);
            this.Controls.Add(this.lbl_Notice);
            this.Controls.Add(this.pnl_Screen);
            this.Controls.Add(this.pnl_Row);
            this.Controls.Add(this.pnl_Column);
            this.Controls.Add(this.pnl_Final);
            this.Controls.Add(this.chb_GridLine);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtb_Text);
            this.Controls.Add(this.btn_Parse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_ScreenParse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScreenParse";
            this.Load += new System.EventHandler(this.Form_ScreenParse_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_Parse;
        private System.Windows.Forms.RichTextBox rtb_Text;
        private System.Windows.Forms.RadioButton rb_DDC;
        private System.Windows.Forms.RadioButton rb_NDC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chb_GridLine;
        private System.Windows.Forms.Panel pnl_Final;
        private System.Windows.Forms.Panel pnl_Column;
        private System.Windows.Forms.Panel pnl_Row;
        private System.Windows.Forms.Panel pnl_Screen;
        private System.Windows.Forms.Label lbl_Notice;
    }
}