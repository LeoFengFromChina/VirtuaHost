namespace VirtualDualHost
{
    partial class Form_GetInteractiveMsgTextToShow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GetInteractiveMsgTextToShow));
            this.rtb_Msg = new System.Windows.Forms.RichTextBox();
            this.rtb_Buffers = new System.Windows.Forms.RichTextBox();
            this.btn_Parse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtb_Msg
            // 
            this.rtb_Msg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_Msg.Location = new System.Drawing.Point(23, 12);
            this.rtb_Msg.Name = "rtb_Msg";
            this.rtb_Msg.Size = new System.Drawing.Size(386, 134);
            this.rtb_Msg.TabIndex = 0;
            this.rtb_Msg.Text = "";
            // 
            // rtb_Buffers
            // 
            this.rtb_Buffers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_Buffers.Location = new System.Drawing.Point(23, 152);
            this.rtb_Buffers.Name = "rtb_Buffers";
            this.rtb_Buffers.Size = new System.Drawing.Size(386, 138);
            this.rtb_Buffers.TabIndex = 1;
            this.rtb_Buffers.Text = "";
            // 
            // btn_Parse
            // 
            this.btn_Parse.Location = new System.Drawing.Point(334, 305);
            this.btn_Parse.Name = "btn_Parse";
            this.btn_Parse.Size = new System.Drawing.Size(75, 23);
            this.btn_Parse.TabIndex = 2;
            this.btn_Parse.Text = "Parse";
            this.btn_Parse.UseVisualStyleBackColor = true;
            this.btn_Parse.Click += new System.EventHandler(this.btn_Parse_Click);
            // 
            // Form_GetInteractiveMsgTextToShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 340);
            this.Controls.Add(this.btn_Parse);
            this.Controls.Add(this.rtb_Buffers);
            this.Controls.Add(this.rtb_Msg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_GetInteractiveMsgTextToShow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GetInteractiveMsgText";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_Msg;
        private System.Windows.Forms.RichTextBox rtb_Buffers;
        private System.Windows.Forms.Button btn_Parse;
    }
}