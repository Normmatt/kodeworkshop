namespace Kode_Workshop
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
            this.codeInput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.codeOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // codeInput
            // 
            this.codeInput.Location = new System.Drawing.Point(12, 12);
            this.codeInput.Multiline = true;
            this.codeInput.Name = "codeInput";
            this.codeInput.Size = new System.Drawing.Size(207, 290);
            this.codeInput.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(489, 308);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Analyze";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // codeOutput
            // 
            this.codeOutput.Location = new System.Drawing.Point(225, 12);
            this.codeOutput.Name = "codeOutput";
            this.codeOutput.Size = new System.Drawing.Size(402, 290);
            this.codeOutput.TabIndex = 3;
            this.codeOutput.Text = "";
            this.codeOutput.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 351);
            this.Controls.Add(this.codeOutput);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.codeInput);
            this.Name = "Form1";
            this.Text = "Kode Workshop";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox codeInput;
        private System.Windows.Forms.RichTextBox codeOutput;
    }
}

