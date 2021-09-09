
namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnetServer = new System.Windows.Forms.Button();
            this.InputNickname = new System.Windows.Forms.TextBox();
            this.textHistory = new System.Windows.Forms.TextBox();
            this.SendMeessageToServer = new System.Windows.Forms.Button();
            this.InputText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnetServer
            // 
            this.ConnetServer.Location = new System.Drawing.Point(613, 65);
            this.ConnetServer.Name = "ConnetServer";
            this.ConnetServer.Size = new System.Drawing.Size(129, 23);
            this.ConnetServer.TabIndex = 0;
            this.ConnetServer.Text = "Connect to Server";
            this.ConnetServer.UseVisualStyleBackColor = true;
            this.ConnetServer.Click += new System.EventHandler(this.ConnetServer_Click);
            // 
            // InputNickname
            // 
            this.InputNickname.Location = new System.Drawing.Point(450, 36);
            this.InputNickname.Name = "InputNickname";
            this.InputNickname.Size = new System.Drawing.Size(292, 23);
            this.InputNickname.TabIndex = 1;
            this.InputNickname.TextChanged += new System.EventHandler(this.InputNickname_TextChanged);
            // 
            // textHistory
            // 
            this.textHistory.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.textHistory.Location = new System.Drawing.Point(57, 109);
            this.textHistory.Multiline = true;
            this.textHistory.Name = "textHistory";
            this.textHistory.ReadOnly = true;
            this.textHistory.Size = new System.Drawing.Size(685, 234);
            this.textHistory.TabIndex = 2;
            this.textHistory.TextChanged += new System.EventHandler(this.textHistory_TextChanged);
            // 
            // SendMeessageToServer
            // 
            this.SendMeessageToServer.Location = new System.Drawing.Point(613, 378);
            this.SendMeessageToServer.Name = "SendMeessageToServer";
            this.SendMeessageToServer.Size = new System.Drawing.Size(129, 23);
            this.SendMeessageToServer.TabIndex = 3;
            this.SendMeessageToServer.Text = "Send Message";
            this.SendMeessageToServer.UseVisualStyleBackColor = true;
            this.SendMeessageToServer.Click += new System.EventHandler(this.button2_Click);
            // 
            // InputText
            // 
            this.InputText.Location = new System.Drawing.Point(57, 349);
            this.InputText.Name = "InputText";
            this.InputText.Size = new System.Drawing.Size(685, 23);
            this.InputText.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Chat Name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputText);
            this.Controls.Add(this.SendMeessageToServer);
            this.Controls.Add(this.textHistory);
            this.Controls.Add(this.InputNickname);
            this.Controls.Add(this.ConnetServer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnetServer;
        private System.Windows.Forms.TextBox InputNickname;
        private System.Windows.Forms.TextBox textHistory;
        private System.Windows.Forms.Button SendMeessageToServer;
        private System.Windows.Forms.TextBox InputText;
        private System.Windows.Forms.Label label1;
    }
}

