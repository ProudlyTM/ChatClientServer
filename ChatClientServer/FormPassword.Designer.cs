namespace ChatClientServer
{
    partial class FormPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPassword));
            this.lblPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.btnOKPassword = new System.Windows.Forms.Button();
            this.btnCancelPassword = new System.Windows.Forms.Button();
            this.btnShowHidePassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(51, 10);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(128, 15);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "Enter a new password";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(13, 38);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(180, 20);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassword_KeyDown);
            // 
            // btnOKPassword
            // 
            this.btnOKPassword.Location = new System.Drawing.Point(73, 64);
            this.btnOKPassword.Name = "btnOKPassword";
            this.btnOKPassword.Size = new System.Drawing.Size(153, 23);
            this.btnOKPassword.TabIndex = 2;
            this.btnOKPassword.Text = "OK";
            this.btnOKPassword.UseVisualStyleBackColor = true;
            this.btnOKPassword.Click += new System.EventHandler(this.btnOKPassword_Click);
            // 
            // btnCancelPassword
            // 
            this.btnCancelPassword.Location = new System.Drawing.Point(12, 64);
            this.btnCancelPassword.Name = "btnCancelPassword";
            this.btnCancelPassword.Size = new System.Drawing.Size(55, 23);
            this.btnCancelPassword.TabIndex = 3;
            this.btnCancelPassword.Text = "Cancel";
            this.btnCancelPassword.UseVisualStyleBackColor = true;
            this.btnCancelPassword.Click += new System.EventHandler(this.btnCancelPassword_Click);
            // 
            // btnShowHidePassword
            // 
            this.btnShowHidePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowHidePassword.Location = new System.Drawing.Point(198, 35);
            this.btnShowHidePassword.Name = "btnShowHidePassword";
            this.btnShowHidePassword.Size = new System.Drawing.Size(28, 26);
            this.btnShowHidePassword.TabIndex = 4;
            this.btnShowHidePassword.Text = "👁";
            this.btnShowHidePassword.UseVisualStyleBackColor = true;
            this.btnShowHidePassword.Click += new System.EventHandler(this.btnShowHidePassword_Click);
            // 
            // FormPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 99);
            this.Controls.Add(this.btnShowHidePassword);
            this.Controls.Add(this.btnCancelPassword);
            this.Controls.Add(this.btnOKPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.lblPassword);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOKPassword;
        private System.Windows.Forms.Button btnCancelPassword;
        private System.Windows.Forms.Button btnShowHidePassword;
        public System.Windows.Forms.TextBox textBoxPassword;
        public System.Windows.Forms.Label lblPassword;
    }
}