
namespace SaveGolds
{
    partial class ConnectForm
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
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.ipLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(75, 36);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(100, 20);
            this.ipTextBox.TabIndex = 1;
            this.ipTextBox.Text = "localhost";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(75, 62);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(100, 20);
            this.portTextBox.TabIndex = 2;
            this.portTextBox.Text = "16834";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(52, 36);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(17, 13);
            this.ipLabel.TabIndex = 4;
            this.ipLabel.Text = "IP";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(43, 62);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 5;
            this.portLabel.Text = "Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "LiveSplit Server Details (leave blank if default)";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(75, 98);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 137);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.ipTextBox);
            this.Name = "ConnectForm";
            this.Text = "Connect To LiveSplit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connectButton;
    }
}