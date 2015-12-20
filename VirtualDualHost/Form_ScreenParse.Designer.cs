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
            this.ptb_Screen = new System.Windows.Forms.PictureBox();
            this.btn_Parse = new System.Windows.Forms.Button();
            this.rtb_Text = new System.Windows.Forms.RichTextBox();
            this.rb_DDC = new System.Windows.Forms.RadioButton();
            this.rb_NDC = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chb_GridLine = new System.Windows.Forms.CheckBox();
            this.ptb_RowTitle = new System.Windows.Forms.PictureBox();
            this.ptb_ColumnTitle = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Screen)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_RowTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_ColumnTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // ptb_Screen
            // 
            this.ptb_Screen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptb_Screen.Location = new System.Drawing.Point(25, 80);
            this.ptb_Screen.Name = "ptb_Screen";
            this.ptb_Screen.Size = new System.Drawing.Size(600, 450);
            this.ptb_Screen.TabIndex = 0;
            this.ptb_Screen.TabStop = false;
            this.ptb_Screen.Paint += new System.Windows.Forms.PaintEventHandler(this.ptb_Screen_Paint);
            // 
            // btn_Parse
            // 
            this.btn_Parse.Location = new System.Drawing.Point(550, 12);
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
            this.rtb_Text.Size = new System.Drawing.Size(390, 40);
            this.rtb_Text.TabIndex = 0;
            this.rtb_Text.Text = "";
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
            this.chb_GridLine.Location = new System.Drawing.Point(642, 12);
            this.chb_GridLine.Name = "chb_GridLine";
            this.chb_GridLine.Size = new System.Drawing.Size(96, 16);
            this.chb_GridLine.TabIndex = 6;
            this.chb_GridLine.Text = "ViewGridLine";
            this.chb_GridLine.UseVisualStyleBackColor = true;
            // 
            // ptb_RowTitle
            // 
            this.ptb_RowTitle.Location = new System.Drawing.Point(25, 59);
            this.ptb_RowTitle.Name = "ptb_RowTitle";
            this.ptb_RowTitle.Size = new System.Drawing.Size(600, 20);
            this.ptb_RowTitle.TabIndex = 7;
            this.ptb_RowTitle.TabStop = false;
            // 
            // ptb_ColumnTitle
            // 
            this.ptb_ColumnTitle.Location = new System.Drawing.Point(626, 80);
            this.ptb_ColumnTitle.Name = "ptb_ColumnTitle";
            this.ptb_ColumnTitle.Size = new System.Drawing.Size(91, 451);
            this.ptb_ColumnTitle.TabIndex = 8;
            this.ptb_ColumnTitle.TabStop = false;
            // 
            // Form_ScreenParse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.ptb_ColumnTitle);
            this.Controls.Add(this.ptb_RowTitle);
            this.Controls.Add(this.chb_GridLine);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtb_Text);
            this.Controls.Add(this.btn_Parse);
            this.Controls.Add(this.ptb_Screen);
            this.Name = "Form_ScreenParse";
            this.Text = "Form_ScreenParse";
            this.Load += new System.EventHandler(this.Form_ScreenParse_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_ScreenParse_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Screen)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_RowTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_ColumnTitle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ptb_Screen;
        private System.Windows.Forms.Button btn_Parse;
        private System.Windows.Forms.RichTextBox rtb_Text;
        private System.Windows.Forms.RadioButton rb_DDC;
        private System.Windows.Forms.RadioButton rb_NDC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chb_GridLine;
        private System.Windows.Forms.PictureBox ptb_RowTitle;
        private System.Windows.Forms.PictureBox ptb_ColumnTitle;
    }
}