
namespace SaveGolds
{
    partial class SaveGolds
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
            this.messageLabel = new System.Windows.Forms.Label();
            this.endButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.gameListBox = new System.Windows.Forms.ListBox();
            this.startSessionButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.endSessionButton = new System.Windows.Forms.Button();
            this.deleteSplitsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(283, 212);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(115, 13);
            this.messageLabel.TabIndex = 0;
            this.messageLabel.Text = "SaveGolds is Running!";
            this.messageLabel.Visible = false;
            // 
            // endButton
            // 
            this.endButton.Location = new System.Drawing.Point(160, 335);
            this.endButton.Name = "endButton";
            this.endButton.Size = new System.Drawing.Size(150, 23);
            this.endButton.TabIndex = 1;
            this.endButton.Text = "End LiveSplit Connection";
            this.endButton.UseVisualStyleBackColor = true;
            this.endButton.Click += new System.EventHandler(this.endButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 176);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(241, 139);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // gameListBox
            // 
            this.gameListBox.FormattingEnabled = true;
            this.gameListBox.Location = new System.Drawing.Point(12, 12);
            this.gameListBox.Name = "gameListBox";
            this.gameListBox.Size = new System.Drawing.Size(241, 147);
            this.gameListBox.TabIndex = 3;
            // 
            // startSessionButton
            // 
            this.startSessionButton.Location = new System.Drawing.Point(276, 101);
            this.startSessionButton.Name = "startSessionButton";
            this.startSessionButton.Size = new System.Drawing.Size(131, 23);
            this.startSessionButton.TabIndex = 4;
            this.startSessionButton.Text = "Start Game Session";
            this.startSessionButton.UseVisualStyleBackColor = true;
            this.startSessionButton.Click += new System.EventHandler(this.startSessionButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(276, 43);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(131, 23);
            this.registerButton.TabIndex = 5;
            this.registerButton.Text = "Register New Splits";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // endSessionButton
            // 
            this.endSessionButton.Location = new System.Drawing.Point(276, 257);
            this.endSessionButton.Name = "endSessionButton";
            this.endSessionButton.Size = new System.Drawing.Size(131, 23);
            this.endSessionButton.TabIndex = 6;
            this.endSessionButton.Text = "End Game Session";
            this.endSessionButton.UseVisualStyleBackColor = true;
            this.endSessionButton.Visible = false;
            this.endSessionButton.Click += new System.EventHandler(this.endSessionButton_Click);
            // 
            // deleteSplitsButton
            // 
            this.deleteSplitsButton.Location = new System.Drawing.Point(276, 72);
            this.deleteSplitsButton.Name = "deleteSplitsButton";
            this.deleteSplitsButton.Size = new System.Drawing.Size(131, 23);
            this.deleteSplitsButton.TabIndex = 7;
            this.deleteSplitsButton.Text = "Delete Selected Splits";
            this.deleteSplitsButton.UseVisualStyleBackColor = true;
            this.deleteSplitsButton.Click += new System.EventHandler(this.deleteSplitsButton_Click);
            // 
            // SaveGolds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 370);
            this.Controls.Add(this.deleteSplitsButton);
            this.Controls.Add(this.endSessionButton);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.startSessionButton);
            this.Controls.Add(this.gameListBox);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.endButton);
            this.Controls.Add(this.messageLabel);
            this.Name = "SaveGolds";
            this.Text = "SaveGolds";
            this.Load += new System.EventHandler(this.SaveGolds_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button endButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListBox gameListBox;
        private System.Windows.Forms.Button startSessionButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button endSessionButton;
        private System.Windows.Forms.Button deleteSplitsButton;
    }
}