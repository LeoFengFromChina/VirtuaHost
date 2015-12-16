namespace VirtualDualHost
{
    partial class Form_DualHost
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DualHost));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_eCAT_BaseLUNO = new System.Windows.Forms.TextBox();
            this.btn_eCAT_Start = new System.Windows.Forms.Button();
            this.txt_eCAT_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lsb_Log_GM01 = new System.Windows.Forms.ListBox();
            this.txt_H1_LUNO = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_H1_Start = new System.Windows.Forms.Button();
            this.txt_H1_Port = new System.Windows.Forms.TextBox();
            this.txt_H1_IP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lsb_Log_GM02 = new System.Windows.Forms.ListBox();
            this.txt_H2_LUNO = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_H2_Start = new System.Windows.Forms.Button();
            this.txt_H2_Port = new System.Windows.Forms.TextBox();
            this.txt_H2_IP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_eCAT_BaseLUNO);
            this.groupBox1.Controls.Add(this.btn_eCAT_Start);
            this.groupBox1.Controls.Add(this.txt_eCAT_Port);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(792, 60);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ATMC";
            // 
            // txt_eCAT_BaseLUNO
            // 
            this.txt_eCAT_BaseLUNO.Location = new System.Drawing.Point(136, 28);
            this.txt_eCAT_BaseLUNO.Name = "txt_eCAT_BaseLUNO";
            this.txt_eCAT_BaseLUNO.Size = new System.Drawing.Size(61, 21);
            this.txt_eCAT_BaseLUNO.TabIndex = 5;
            this.txt_eCAT_BaseLUNO.Text = "A21";
            // 
            // btn_eCAT_Start
            // 
            this.btn_eCAT_Start.Location = new System.Drawing.Point(212, 27);
            this.btn_eCAT_Start.Name = "btn_eCAT_Start";
            this.btn_eCAT_Start.Size = new System.Drawing.Size(67, 21);
            this.btn_eCAT_Start.TabIndex = 4;
            this.btn_eCAT_Start.Text = "Start";
            this.btn_eCAT_Start.UseVisualStyleBackColor = true;
            this.btn_eCAT_Start.Click += new System.EventHandler(this.btn_eCAT_Start_Click);
            // 
            // txt_eCAT_Port
            // 
            this.txt_eCAT_Port.Location = new System.Drawing.Point(60, 28);
            this.txt_eCAT_Port.Name = "txt_eCAT_Port";
            this.txt_eCAT_Port.Size = new System.Drawing.Size(70, 21);
            this.txt_eCAT_Port.TabIndex = 3;
            this.txt_eCAT_Port.Text = "4070";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port ：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(792, 513);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lsb_Log_GM01);
            this.groupBox2.Controls.Add(this.txt_H1_LUNO);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btn_H1_Start);
            this.groupBox2.Controls.Add(this.txt_H1_Port);
            this.groupBox2.Controls.Add(this.txt_H1_IP);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 513);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Host-1:";
            // 
            // lsb_Log_GM01
            // 
            this.lsb_Log_GM01.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsb_Log_GM01.FormattingEnabled = true;
            this.lsb_Log_GM01.HorizontalScrollbar = true;
            this.lsb_Log_GM01.ItemHeight = 12;
            this.lsb_Log_GM01.Location = new System.Drawing.Point(14, 73);
            this.lsb_Log_GM01.Name = "lsb_Log_GM01";
            this.lsb_Log_GM01.Size = new System.Drawing.Size(378, 424);
            this.lsb_Log_GM01.TabIndex = 14;
            this.lsb_Log_GM01.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsb_Log_GM01_MouseDoubleClick);
            // 
            // txt_H1_LUNO
            // 
            this.txt_H1_LUNO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_H1_LUNO.Location = new System.Drawing.Point(254, 47);
            this.txt_H1_LUNO.Name = "txt_H1_LUNO";
            this.txt_H1_LUNO.Size = new System.Drawing.Size(61, 21);
            this.txt_H1_LUNO.TabIndex = 13;
            this.txt_H1_LUNO.Text = "A21000";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(210, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "LUNO：";
            // 
            // btn_H1_Start
            // 
            this.btn_H1_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_H1_Start.Location = new System.Drawing.Point(321, 20);
            this.btn_H1_Start.Name = "btn_H1_Start";
            this.btn_H1_Start.Size = new System.Drawing.Size(71, 48);
            this.btn_H1_Start.TabIndex = 9;
            this.btn_H1_Start.Text = "Start";
            this.btn_H1_Start.UseVisualStyleBackColor = true;
            this.btn_H1_Start.Click += new System.EventHandler(this.btn_H1_Start_Click);
            // 
            // txt_H1_Port
            // 
            this.txt_H1_Port.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_H1_Port.Location = new System.Drawing.Point(60, 47);
            this.txt_H1_Port.Name = "txt_H1_Port";
            this.txt_H1_Port.Size = new System.Drawing.Size(122, 21);
            this.txt_H1_Port.TabIndex = 8;
            this.txt_H1_Port.Text = "4071";
            // 
            // txt_H1_IP
            // 
            this.txt_H1_IP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_H1_IP.Location = new System.Drawing.Point(60, 20);
            this.txt_H1_IP.Name = "txt_H1_IP";
            this.txt_H1_IP.Size = new System.Drawing.Size(255, 21);
            this.txt_H1_IP.TabIndex = 7;
            this.txt_H1_IP.Text = "127.0.0.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port ：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "IP ：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lsb_Log_GM02);
            this.groupBox3.Controls.Add(this.txt_H2_LUNO);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.btn_H2_Start);
            this.groupBox3.Controls.Add(this.txt_H2_Port);
            this.groupBox3.Controls.Add(this.txt_H2_IP);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(388, 513);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Host-2";
            // 
            // lsb_Log_GM02
            // 
            this.lsb_Log_GM02.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsb_Log_GM02.FormattingEnabled = true;
            this.lsb_Log_GM02.HorizontalScrollbar = true;
            this.lsb_Log_GM02.ItemHeight = 12;
            this.lsb_Log_GM02.Location = new System.Drawing.Point(10, 73);
            this.lsb_Log_GM02.Name = "lsb_Log_GM02";
            this.lsb_Log_GM02.Size = new System.Drawing.Size(368, 424);
            this.lsb_Log_GM02.TabIndex = 20;
            // 
            // txt_H2_LUNO
            // 
            this.txt_H2_LUNO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_H2_LUNO.Location = new System.Drawing.Point(243, 46);
            this.txt_H2_LUNO.Name = "txt_H2_LUNO";
            this.txt_H2_LUNO.Size = new System.Drawing.Size(62, 21);
            this.txt_H2_LUNO.TabIndex = 19;
            this.txt_H2_LUNO.Text = "A21999";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(201, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "LUNO：";
            // 
            // btn_H2_Start
            // 
            this.btn_H2_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_H2_Start.Location = new System.Drawing.Point(311, 19);
            this.btn_H2_Start.Name = "btn_H2_Start";
            this.btn_H2_Start.Size = new System.Drawing.Size(67, 48);
            this.btn_H2_Start.TabIndex = 16;
            this.btn_H2_Start.Text = "Start";
            this.btn_H2_Start.UseVisualStyleBackColor = true;
            this.btn_H2_Start.Click += new System.EventHandler(this.btn_H2_Start_Click);
            // 
            // txt_H2_Port
            // 
            this.txt_H2_Port.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_H2_Port.Location = new System.Drawing.Point(58, 46);
            this.txt_H2_Port.Name = "txt_H2_Port";
            this.txt_H2_Port.Size = new System.Drawing.Size(116, 21);
            this.txt_H2_Port.TabIndex = 15;
            this.txt_H2_Port.Text = "4072";
            // 
            // txt_H2_IP
            // 
            this.txt_H2_IP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_H2_IP.Location = new System.Drawing.Point(58, 19);
            this.txt_H2_IP.Name = "txt_H2_IP";
            this.txt_H2_IP.Size = new System.Drawing.Size(247, 21);
            this.txt_H2_IP.TabIndex = 14;
            this.txt_H2_IP.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "Port ：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "IP ：";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tssl_Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusLabel1.Text = "Status：";
            // 
            // tssl_Status
            // 
            this.tssl_Status.Name = "tssl_Status";
            this.tssl_Status.Size = new System.Drawing.Size(0, 17);
            // 
            // Form_DualHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_DualHost";
            this.TabText = "Dual-Host";
            this.Text = "Dual-Host";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_eCAT_Start;
        private System.Windows.Forms.TextBox txt_eCAT_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_H1_Start;
        private System.Windows.Forms.TextBox txt_H1_Port;
        private System.Windows.Forms.TextBox txt_H1_IP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_H2_Start;
        private System.Windows.Forms.TextBox txt_H2_Port;
        private System.Windows.Forms.TextBox txt_H2_IP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_H1_LUNO;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_H2_LUNO;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txt_eCAT_BaseLUNO;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Status;
        private System.Windows.Forms.ListBox lsb_Log_GM01;
        private System.Windows.Forms.ListBox lsb_Log_GM02;
    }
}

