namespace Kodinator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.codeOutput = new System.Windows.Forms.RichTextBox();
            this.codeInput = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.branchSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.branchDestination = new System.Windows.Forms.TextBox();
            this.branchOutput = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 305);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.codeOutput);
            this.tabPage1.Controls.Add(this.codeInput);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(608, 279);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Code Analyzer";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(319, 240);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 32);
            this.button3.TabIndex = 3;
            this.button3.Text = "&Convert to C/C++";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(448, 240);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 33);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Analyze";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // codeOutput
            // 
            this.codeOutput.BackColor = System.Drawing.SystemColors.Window;
            this.codeOutput.Location = new System.Drawing.Point(217, 6);
            this.codeOutput.Name = "codeOutput";
            this.codeOutput.ReadOnly = true;
            this.codeOutput.Size = new System.Drawing.Size(385, 228);
            this.codeOutput.TabIndex = 1;
            this.codeOutput.Text = "";
            this.codeOutput.WordWrap = false;
            // 
            // codeInput
            // 
            this.codeInput.Location = new System.Drawing.Point(6, 6);
            this.codeInput.Name = "codeInput";
            this.codeInput.Size = new System.Drawing.Size(205, 228);
            this.codeInput.TabIndex = 0;
            this.codeInput.Text = "6209DB88 00000000\nB209DB88 00000000\nDC000000 0000000C\n60000000 00000000\nB0000000 " +
                "00000000\n60000000 00000000\nB0000000 00000000\n10000000 0000270F\nD2000000 00000000" +
                "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(608, 279);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Generator";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.branchSource);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.branchDestination);
            this.groupBox1.Controls.Add(this.branchOutput);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(596, 105);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Branch Generator";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(147, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 21);
            this.button2.TabIndex = 6;
            this.button2.Text = "&Generate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // branchSource
            // 
            this.branchSource.Location = new System.Drawing.Point(106, 19);
            this.branchSource.Name = "branchSource";
            this.branchSource.Size = new System.Drawing.Size(108, 20);
            this.branchSource.TabIndex = 1;
            this.branchSource.Text = "0x02000000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Source Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(231, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Output:";
            // 
            // branchDestination
            // 
            this.branchDestination.Location = new System.Drawing.Point(107, 47);
            this.branchDestination.Name = "branchDestination";
            this.branchDestination.Size = new System.Drawing.Size(106, 20);
            this.branchDestination.TabIndex = 2;
            this.branchDestination.Text = "0x02000000";
            // 
            // branchOutput
            // 
            this.branchOutput.Location = new System.Drawing.Point(279, 13);
            this.branchOutput.Name = "branchOutput";
            this.branchOutput.Size = new System.Drawing.Size(311, 54);
            this.branchOutput.TabIndex = 4;
            this.branchOutput.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Destination:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 365);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "&Kodinator - sent from the future to destroy your NDS!";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox codeOutput;
        private System.Windows.Forms.RichTextBox codeInput;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox branchOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox branchDestination;
        private System.Windows.Forms.TextBox branchSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
    }
}

