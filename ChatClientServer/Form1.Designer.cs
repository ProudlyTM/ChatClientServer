namespace ChatClientServer
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxClientPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.textBoxClientIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblOr = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSrvStart = new System.Windows.Forms.Button();
            this.lblSrvPort = new System.Windows.Forms.Label();
            this.comboBoxSrvIPSel = new System.Windows.Forms.ComboBox();
            this.lblSrvIP = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxChatInput = new System.Windows.Forms.RichTextBox();
            this.textBoxChatScreen = new System.Windows.Forms.RichTextBox();
            this.buttonSendMsg = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(115, 30);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 450);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lblOr);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(317, 412);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Client/Server Controls";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxClientPort);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.textBoxClientIP);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.lblIP);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 116);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connect to a Server";
            // 
            // textBoxClientPort
            // 
            this.textBoxClientPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClientPort.Location = new System.Drawing.Point(216, 26);
            this.textBoxClientPort.MaxLength = 5;
            this.textBoxClientPort.Name = "textBoxClientPort";
            this.textBoxClientPort.Size = new System.Drawing.Size(59, 26);
            this.textBoxClientPort.TabIndex = 5;
            this.textBoxClientPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxClientIPPort_KeyPress);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(175, 29);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(42, 20);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port:";
            // 
            // textBoxClientIP
            // 
            this.textBoxClientIP.Location = new System.Drawing.Point(34, 26);
            this.textBoxClientIP.Name = "textBoxClientIP";
            this.textBoxClientIP.Size = new System.Drawing.Size(141, 26);
            this.textBoxClientIP.TabIndex = 4;
            this.textBoxClientIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxClientIPPort_KeyPress);
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(16, 59);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(260, 45);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(12, 29);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(24, 20);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "IP";
            // 
            // lblOr
            // 
            this.lblOr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblOr.AutoSize = true;
            this.lblOr.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOr.Location = new System.Drawing.Point(119, 183);
            this.lblOr.Name = "lblOr";
            this.lblOr.Size = new System.Drawing.Size(74, 42);
            this.lblOr.TabIndex = 1;
            this.lblOr.Text = "OR";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSrvStart);
            this.groupBox2.Controls.Add(this.lblSrvPort);
            this.groupBox2.Controls.Add(this.comboBoxSrvIPSel);
            this.groupBox2.Controls.Add(this.lblSrvIP);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 279);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 116);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Start a Server";
            // 
            // btnSrvStart
            // 
            this.btnSrvStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSrvStart.Location = new System.Drawing.Point(16, 59);
            this.btnSrvStart.Name = "btnSrvStart";
            this.btnSrvStart.Size = new System.Drawing.Size(260, 45);
            this.btnSrvStart.TabIndex = 3;
            this.btnSrvStart.Text = "START";
            this.btnSrvStart.UseVisualStyleBackColor = true;
            this.btnSrvStart.Click += new System.EventHandler(this.btnSrvStart_Click);
            // 
            // lblSrvPort
            // 
            this.lblSrvPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSrvPort.AutoSize = true;
            this.lblSrvPort.Location = new System.Drawing.Point(181, 28);
            this.lblSrvPort.Name = "lblSrvPort";
            this.lblSrvPort.Size = new System.Drawing.Size(46, 20);
            this.lblSrvPort.TabIndex = 2;
            this.lblSrvPort.Text = "Port: ";
            // 
            // comboBoxSrvIPSel
            // 
            this.comboBoxSrvIPSel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSrvIPSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSrvIPSel.FormattingEnabled = true;
            this.comboBoxSrvIPSel.Location = new System.Drawing.Point(34, 25);
            this.comboBoxSrvIPSel.Name = "comboBoxSrvIPSel";
            this.comboBoxSrvIPSel.Size = new System.Drawing.Size(141, 28);
            this.comboBoxSrvIPSel.TabIndex = 1;
            this.comboBoxSrvIPSel.DropDownClosed += new System.EventHandler(this.comboBoxLocalIPSel_DropDownClosed);
            // 
            // lblSrvIP
            // 
            this.lblSrvIP.AutoSize = true;
            this.lblSrvIP.Location = new System.Drawing.Point(12, 29);
            this.lblSrvIP.Name = "lblSrvIP";
            this.lblSrvIP.Size = new System.Drawing.Size(24, 20);
            this.lblSrvIP.TabIndex = 0;
            this.lblSrvIP.Text = "IP";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxChatInput);
            this.tabPage2.Controls.Add(this.textBoxChatScreen);
            this.tabPage2.Controls.Add(this.buttonSendMsg);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(317, 412);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Chat";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxChatInput
            // 
            this.richTextBoxChatInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxChatInput.Location = new System.Drawing.Point(9, 362);
            this.richTextBoxChatInput.Multiline = false;
            this.richTextBoxChatInput.Name = "richTextBoxChatInput";
            this.richTextBoxChatInput.Size = new System.Drawing.Size(245, 45);
            this.richTextBoxChatInput.TabIndex = 6;
            this.richTextBoxChatInput.Text = "";
            this.richTextBoxChatInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBoxChatInput_KeyDown);
            // 
            // textBoxChatScreen
            // 
            this.textBoxChatScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChatScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChatScreen.Location = new System.Drawing.Point(9, 7);
            this.textBoxChatScreen.Name = "textBoxChatScreen";
            this.textBoxChatScreen.ReadOnly = true;
            this.textBoxChatScreen.Size = new System.Drawing.Size(300, 349);
            this.textBoxChatScreen.TabIndex = 3;
            this.textBoxChatScreen.Text = "";
            this.textBoxChatScreen.TextChanged += new System.EventHandler(this.textBoxChatScreen_TextChanged);
            // 
            // buttonSendMsg
            // 
            this.buttonSendMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSendMsg.Location = new System.Drawing.Point(260, 362);
            this.buttonSendMsg.Name = "buttonSendMsg";
            this.buttonSendMsg.Size = new System.Drawing.Size(49, 45);
            this.buttonSendMsg.TabIndex = 2;
            this.buttonSendMsg.Text = "↵";
            this.buttonSendMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSendMsg.UseVisualStyleBackColor = true;
            this.buttonSendMsg.Click += new System.EventHandler(this.buttonSendMsg_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 450);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(341, 489);
            this.Name = "Form1";
            this.Text = "Chat Client/Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSrvStart;
        private System.Windows.Forms.Label lblSrvPort;
        public System.Windows.Forms.ComboBox comboBoxSrvIPSel;
        public System.Windows.Forms.Label lblSrvIP;
        private System.Windows.Forms.Label lblOr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConnect;
        public System.Windows.Forms.Label lblIP;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.TextBox textBoxClientIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox textBoxClientPort;
        private System.Windows.Forms.Button buttonSendMsg;
        private System.Windows.Forms.RichTextBox textBoxChatScreen;
        private System.Windows.Forms.RichTextBox richTextBoxChatInput;
    }
}

