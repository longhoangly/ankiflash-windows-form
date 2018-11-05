namespace FlashcardGenerator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtInputPath = new System.Windows.Forms.TextBox();
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
            this.labelWords = new System.Windows.Forms.Label();
            this.txtNumInput = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.RichTextBox();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.txtFailureNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFailure = new System.Windows.Forms.RichTextBox();
            this.txtOutputNumber = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxLanguages = new System.Windows.Forms.ComboBox();
            this.labelLanguages = new System.Windows.Forms.Label();
            this.groupBoxProgress = new System.Windows.Forms.GroupBox();
            this.support = new System.Windows.Forms.Label();
            this.labelProxyConnection = new System.Windows.Forms.Label();
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBoxInput.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.AutoSize = true;
            this.btnOpen.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOpen.Location = new System.Drawing.Point(9, 19);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(59, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtInputPath
            // 
            this.txtInputPath.BackColor = System.Drawing.SystemColors.Menu;
            this.txtInputPath.Location = new System.Drawing.Point(74, 20);
            this.txtInputPath.Name = "txtInputPath";
            this.txtInputPath.Size = new System.Drawing.Size(287, 20);
            this.txtInputPath.TabIndex = 2;
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Controls.Add(this.labelWords);
            this.groupBoxInput.Controls.Add(this.txtNumInput);
            this.groupBoxInput.Controls.Add(this.txtInput);
            this.groupBoxInput.Controls.Add(this.txtInputPath);
            this.groupBoxInput.Controls.Add(this.btnOpen);
            this.groupBoxInput.Location = new System.Drawing.Point(5, 6);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(369, 277);
            this.groupBoxInput.TabIndex = 3;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Input";
            // 
            // labelWords
            // 
            this.labelWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelWords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelWords.Location = new System.Drawing.Point(209, 50);
            this.labelWords.Name = "labelWords";
            this.labelWords.Size = new System.Drawing.Size(65, 19);
            this.labelWords.TabIndex = 5;
            this.labelWords.Text = "Total Words";
            this.labelWords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNumInput
            // 
            this.txtNumInput.Location = new System.Drawing.Point(279, 50);
            this.txtNumInput.Name = "txtNumInput";
            this.txtNumInput.ReadOnly = true;
            this.txtNumInput.Size = new System.Drawing.Size(82, 20);
            this.txtNumInput.TabIndex = 4;
            this.txtNumInput.Text = "0";
            // 
            // txtInput
            // 
            this.txtInput.BackColor = System.Drawing.SystemColors.Control;
            this.txtInput.Location = new System.Drawing.Point(6, 78);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(355, 193);
            this.txtInput.TabIndex = 3;
            this.txtInput.Text = "";
            this.txtInput.WordWrap = false;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Controls.Add(this.txtFailureNumber);
            this.groupBoxOutput.Controls.Add(this.label2);
            this.groupBoxOutput.Controls.Add(this.label1);
            this.groupBoxOutput.Controls.Add(this.txtFailure);
            this.groupBoxOutput.Controls.Add(this.txtOutputNumber);
            this.groupBoxOutput.Controls.Add(this.txtOutput);
            this.groupBoxOutput.Location = new System.Drawing.Point(380, 6);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(369, 277);
            this.groupBoxOutput.TabIndex = 4;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // txtFailureNumber
            // 
            this.txtFailureNumber.Location = new System.Drawing.Point(312, 22);
            this.txtFailureNumber.Name = "txtFailureNumber";
            this.txtFailureNumber.ReadOnly = true;
            this.txtFailureNumber.Size = new System.Drawing.Size(49, 20);
            this.txtFailureNumber.TabIndex = 15;
            this.txtFailureNumber.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(184, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Failures";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Success";
            // 
            // txtFailure
            // 
            this.txtFailure.Location = new System.Drawing.Point(184, 46);
            this.txtFailure.Name = "txtFailure";
            this.txtFailure.ReadOnly = true;
            this.txtFailure.Size = new System.Drawing.Size(178, 225);
            this.txtFailure.TabIndex = 12;
            this.txtFailure.Text = "";
            this.txtFailure.WordWrap = false;
            // 
            // txtOutputNumber
            // 
            this.txtOutputNumber.Location = new System.Drawing.Point(126, 22);
            this.txtOutputNumber.Name = "txtOutputNumber";
            this.txtOutputNumber.ReadOnly = true;
            this.txtOutputNumber.Size = new System.Drawing.Size(50, 20);
            this.txtOutputNumber.TabIndex = 6;
            this.txtOutputNumber.Text = "0";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(6, 46);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(172, 225);
            this.txtOutput.TabIndex = 2;
            this.txtOutput.Text = "";
            this.txtOutput.WordWrap = false;
            // 
            // proBar
            // 
            this.proBar.Location = new System.Drawing.Point(7, 56);
            this.proBar.Name = "proBar";
            this.proBar.Size = new System.Drawing.Size(354, 19);
            this.proBar.TabIndex = 5;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Word List.txt";
            this.openFileDialog.Multiselect = true;
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnRun.Enabled = false;
            this.btnRun.Location = new System.Drawing.Point(237, 19);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(59, 25);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(302, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbxLanguages
            // 
            this.cbxLanguages.AllowDrop = true;
            this.cbxLanguages.Enabled = false;
            this.cbxLanguages.FormattingEnabled = true;
            this.cbxLanguages.Items.AddRange(new object[] {
            "English to English",
            "English to Vietnamese",
            "English to Chinese",
            "English to English & Vietnamese",
            "English to Vietnamese & English",
            "French to Vietnamese",
            "French to English",
            "French to English & Vietnamese",
            "French to Vietnamese & English",
            "Vietnamese to English",
            "Vietnamese to French"});
            this.cbxLanguages.Location = new System.Drawing.Point(75, 21);
            this.cbxLanguages.Name = "cbxLanguages";
            this.cbxLanguages.Size = new System.Drawing.Size(156, 21);
            this.cbxLanguages.TabIndex = 7;
            this.cbxLanguages.Text = "English to English";
            this.cbxLanguages.SelectedIndexChanged += new System.EventHandler(this.cbxLanguages_SelectedIndexChanged);
            // 
            // labelLanguages
            // 
            this.labelLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLanguages.Enabled = false;
            this.labelLanguages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelLanguages.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelLanguages.Location = new System.Drawing.Point(6, 21);
            this.labelLanguages.Name = "labelLanguages";
            this.labelLanguages.Size = new System.Drawing.Size(63, 21);
            this.labelLanguages.TabIndex = 8;
            this.labelLanguages.Text = "Languages";
            this.labelLanguages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxProgress
            // 
            this.groupBoxProgress.Controls.Add(this.support);
            this.groupBoxProgress.Controls.Add(this.labelProxyConnection);
            this.groupBoxProgress.Controls.Add(this.txtProxy);
            this.groupBoxProgress.Controls.Add(this.chkUseProxy);
            this.groupBoxProgress.Controls.Add(this.labelLanguages);
            this.groupBoxProgress.Controls.Add(this.cbxLanguages);
            this.groupBoxProgress.Controls.Add(this.proBar);
            this.groupBoxProgress.Controls.Add(this.btnCancel);
            this.groupBoxProgress.Controls.Add(this.btnRun);
            this.groupBoxProgress.Location = new System.Drawing.Point(5, 289);
            this.groupBoxProgress.Name = "groupBoxProgress";
            this.groupBoxProgress.Size = new System.Drawing.Size(744, 87);
            this.groupBoxProgress.TabIndex = 9;
            this.groupBoxProgress.TabStop = false;
            this.groupBoxProgress.Text = "Progress";
            // 
            // support
            // 
            this.support.AutoSize = true;
            this.support.Location = new System.Drawing.Point(509, 28);
            this.support.Name = "support";
            this.support.Size = new System.Drawing.Size(173, 13);
            this.support.TabIndex = 15;
            this.support.Text = "Support: hoanglongtc7@gmail.com";
            this.support.Click += new System.EventHandler(this.support_Click);
            // 
            // labelProxyConnection
            // 
            this.labelProxyConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProxyConnection.Enabled = false;
            this.labelProxyConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelProxyConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelProxyConnection.Location = new System.Drawing.Point(381, 56);
            this.labelProxyConnection.Name = "labelProxyConnection";
            this.labelProxyConnection.Size = new System.Drawing.Size(125, 19);
            this.labelProxyConnection.TabIndex = 14;
            this.labelProxyConnection.Text = "Proxy Connection String";
            this.labelProxyConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtProxy
            // 
            this.txtProxy.BackColor = System.Drawing.SystemColors.Menu;
            this.txtProxy.Enabled = false;
            this.txtProxy.Location = new System.Drawing.Point(512, 56);
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtProxy.Size = new System.Drawing.Size(212, 20);
            this.txtProxy.TabIndex = 13;
            this.txtProxy.Text = "http://10.10.10.10:8080";
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Location = new System.Drawing.Point(384, 27);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(73, 17);
            this.chkUseProxy.TabIndex = 12;
            this.chkUseProxy.Text = "Use proxy";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            this.chkUseProxy.CheckedChanged += new System.EventHandler(this.chkBoxUseProxy_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(754, 380);
            this.Controls.Add(this.groupBoxInput);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Flashcards Generator v9.2.1";
            this.groupBoxInput.ResumeLayout(false);
            this.groupBoxInput.PerformLayout();
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxProgress.ResumeLayout(false);
            this.groupBoxProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txtInputPath;
        private System.Windows.Forms.GroupBox groupBoxInput;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.ProgressBar proBar;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label labelWords;
        private System.Windows.Forms.TextBox txtNumInput;
        private System.Windows.Forms.RichTextBox txtInput;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbxLanguages;
        private System.Windows.Forms.TextBox txtOutputNumber;
        private System.Windows.Forms.Label labelLanguages;
        private System.Windows.Forms.GroupBox groupBoxProgress;
        private System.Windows.Forms.Label labelProxyConnection;
        private System.Windows.Forms.TextBox txtProxy;
        private System.Windows.Forms.CheckBox chkUseProxy;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label support;
        private System.Windows.Forms.RichTextBox txtFailure;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFailureNumber;
    }
}

