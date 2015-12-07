namespace VirtualDualHost
{
    partial class Form_NDCServer
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
            this.btn_FullDownLoad = new System.Windows.Forms.Button();
            this.btn_ManuSendData = new System.Windows.Forms.Button();
            this.btn_FetchConfig = new System.Windows.Forms.Button();
            this.gb_Debug = new System.Windows.Forms.GroupBox();
            this.chb_ReceiveMsgDebug = new System.Windows.Forms.CheckBox();
            this.chb_SendMsgDebug = new System.Windows.Forms.CheckBox();
            this.rtb_Log = new System.Windows.Forms.RichTextBox();
            this.dgv_Cassette = new System.Windows.Forms.DataGridView();
            this.btn_Start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.gb_Server = new System.Windows.Forms.GroupBox();
            this.Cassette = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Denomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dispense = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gb_Debug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Cassette)).BeginInit();
            this.gb_Server.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_FullDownLoad
            // 
            this.btn_FullDownLoad.Location = new System.Drawing.Point(98, 73);
            this.btn_FullDownLoad.Name = "btn_FullDownLoad";
            this.btn_FullDownLoad.Size = new System.Drawing.Size(96, 23);
            this.btn_FullDownLoad.TabIndex = 7;
            this.btn_FullDownLoad.Text = "FullDownLoad";
            this.btn_FullDownLoad.UseVisualStyleBackColor = true;
            // 
            // btn_ManuSendData
            // 
            this.btn_ManuSendData.Location = new System.Drawing.Point(98, 45);
            this.btn_ManuSendData.Name = "btn_ManuSendData";
            this.btn_ManuSendData.Size = new System.Drawing.Size(96, 23);
            this.btn_ManuSendData.TabIndex = 6;
            this.btn_ManuSendData.Text = "ManuSendData";
            this.btn_ManuSendData.UseVisualStyleBackColor = true;
            // 
            // btn_FetchConfig
            // 
            this.btn_FetchConfig.Location = new System.Drawing.Point(98, 16);
            this.btn_FetchConfig.Name = "btn_FetchConfig";
            this.btn_FetchConfig.Size = new System.Drawing.Size(96, 23);
            this.btn_FetchConfig.TabIndex = 5;
            this.btn_FetchConfig.Text = "FetchConfig";
            this.btn_FetchConfig.UseVisualStyleBackColor = true;
            // 
            // gb_Debug
            // 
            this.gb_Debug.Controls.Add(this.btn_FullDownLoad);
            this.gb_Debug.Controls.Add(this.btn_ManuSendData);
            this.gb_Debug.Controls.Add(this.btn_FetchConfig);
            this.gb_Debug.Controls.Add(this.chb_ReceiveMsgDebug);
            this.gb_Debug.Controls.Add(this.chb_SendMsgDebug);
            this.gb_Debug.Location = new System.Drawing.Point(12, 75);
            this.gb_Debug.Name = "gb_Debug";
            this.gb_Debug.Size = new System.Drawing.Size(200, 96);
            this.gb_Debug.TabIndex = 9;
            this.gb_Debug.TabStop = false;
            this.gb_Debug.Text = "Debug";
            // 
            // chb_ReceiveMsgDebug
            // 
            this.chb_ReceiveMsgDebug.AutoSize = true;
            this.chb_ReceiveMsgDebug.Location = new System.Drawing.Point(8, 20);
            this.chb_ReceiveMsgDebug.Name = "chb_ReceiveMsgDebug";
            this.chb_ReceiveMsgDebug.Size = new System.Drawing.Size(84, 16);
            this.chb_ReceiveMsgDebug.TabIndex = 3;
            this.chb_ReceiveMsgDebug.Text = "ReceiveMsg";
            this.chb_ReceiveMsgDebug.UseVisualStyleBackColor = true;
            // 
            // chb_SendMsgDebug
            // 
            this.chb_SendMsgDebug.AutoSize = true;
            this.chb_SendMsgDebug.Location = new System.Drawing.Point(8, 45);
            this.chb_SendMsgDebug.Name = "chb_SendMsgDebug";
            this.chb_SendMsgDebug.Size = new System.Drawing.Size(66, 16);
            this.chb_SendMsgDebug.TabIndex = 4;
            this.chb_SendMsgDebug.Text = "SendMsg";
            this.chb_SendMsgDebug.UseVisualStyleBackColor = true;
            // 
            // rtb_Log
            // 
            this.rtb_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_Log.Location = new System.Drawing.Point(12, 177);
            this.rtb_Log.Name = "rtb_Log";
            this.rtb_Log.Size = new System.Drawing.Size(760, 372);
            this.rtb_Log.TabIndex = 8;
            this.rtb_Log.Text = "";
            // 
            // dgv_Cassette
            // 
            this.dgv_Cassette.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Cassette.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Cassette,
            this.Denomination,
            this.LoadCount,
            this.Dispense,
            this.Remain,
            this.Status});
            this.dgv_Cassette.Location = new System.Drawing.Point(218, 21);
            this.dgv_Cassette.Name = "dgv_Cassette";
            this.dgv_Cassette.ReadOnly = true;
            this.dgv_Cassette.RowHeadersVisible = false;
            this.dgv_Cassette.RowTemplate.Height = 23;
            this.dgv_Cassette.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Cassette.Size = new System.Drawing.Size(554, 150);
            this.dgv_Cassette.TabIndex = 7;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(147, 20);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(47, 23);
            this.btn_Start.TabIndex = 2;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port:";
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(46, 20);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(95, 21);
            this.txt_Port.TabIndex = 0;
            this.txt_Port.Text = "4070";
            // 
            // gb_Server
            // 
            this.gb_Server.Controls.Add(this.btn_Start);
            this.gb_Server.Controls.Add(this.label1);
            this.gb_Server.Controls.Add(this.txt_Port);
            this.gb_Server.Location = new System.Drawing.Point(12, 13);
            this.gb_Server.Name = "gb_Server";
            this.gb_Server.Size = new System.Drawing.Size(200, 56);
            this.gb_Server.TabIndex = 6;
            this.gb_Server.TabStop = false;
            this.gb_Server.Text = "Server";
            // 
            // Cassette
            // 
            this.Cassette.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cassette.HeaderText = "Cassette";
            this.Cassette.Name = "Cassette";
            this.Cassette.ReadOnly = true;
            // 
            // Denomination
            // 
            this.Denomination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Denomination.HeaderText = "Denomination";
            this.Denomination.Name = "Denomination";
            this.Denomination.ReadOnly = true;
            // 
            // LoadCount
            // 
            this.LoadCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LoadCount.HeaderText = "LoadCount";
            this.LoadCount.Name = "LoadCount";
            this.LoadCount.ReadOnly = true;
            // 
            // Dispense
            // 
            this.Dispense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Dispense.HeaderText = "Dispense";
            this.Dispense.Name = "Dispense";
            this.Dispense.ReadOnly = true;
            // 
            // Remain
            // 
            this.Remain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remain.HeaderText = "Remain";
            this.Remain.Name = "Remain";
            this.Remain.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Form_NDCServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.gb_Debug);
            this.Controls.Add(this.rtb_Log);
            this.Controls.Add(this.dgv_Cassette);
            this.Controls.Add(this.gb_Server);
            this.Name = "Form_NDCServer";
            this.TabText = "NDCServer";
            this.Text = "NDCServer";
            this.gb_Debug.ResumeLayout(false);
            this.gb_Debug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Cassette)).EndInit();
            this.gb_Server.ResumeLayout(false);
            this.gb_Server.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_FullDownLoad;
        private System.Windows.Forms.Button btn_ManuSendData;
        private System.Windows.Forms.Button btn_FetchConfig;
        private System.Windows.Forms.GroupBox gb_Debug;
        private System.Windows.Forms.CheckBox chb_ReceiveMsgDebug;
        private System.Windows.Forms.CheckBox chb_SendMsgDebug;
        private System.Windows.Forms.RichTextBox rtb_Log;
        private System.Windows.Forms.DataGridView dgv_Cassette;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.GroupBox gb_Server;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cassette;
        private System.Windows.Forms.DataGridViewTextBoxColumn Denomination;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dispense;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remain;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}