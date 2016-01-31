namespace FlashcardsGeneratorApplication
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
            this.chkBoxClipboard = new System.Windows.Forms.CheckBox();
            this.labelWords = new System.Windows.Forms.Label();
            this.txtNumInput = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.RichTextBox();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.txtFailedNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFailed = new System.Windows.Forms.RichTextBox();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.labelCards = new System.Windows.Forms.Label();
            this.txtNumOutput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comBoxLanguages = new System.Windows.Forms.ComboBox();
            this.labelLanguages = new System.Windows.Forms.Label();
            this.groupBoxProgress = new System.Windows.Forms.GroupBox();
            this.support = new System.Windows.Forms.Label();
            this.labelProxyConnection = new System.Windows.Forms.Label();
            this.txtProxyString = new System.Windows.Forms.TextBox();
            this.chkBoxUseProxy = new System.Windows.Forms.CheckBox();
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
            this.groupBoxInput.Controls.Add(this.chkBoxClipboard);
            this.groupBoxInput.Controls.Add(this.labelWords);
            this.groupBoxInput.Controls.Add(this.txtNumInput);
            this.groupBoxInput.Controls.Add(this.txtInput);
            this.groupBoxInput.Controls.Add(this.txtInputPath);
            this.groupBoxInput.Controls.Add(this.btnOpen);
            this.groupBoxInput.Location = new System.Drawing.Point(5, 6);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(369, 284);
            this.groupBoxInput.TabIndex = 3;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Input";
            // 
            // chkBoxClipboard
            // 
            this.chkBoxClipboard.AutoSize = true;
            this.chkBoxClipboard.Location = new System.Drawing.Point(7, 55);
            this.chkBoxClipboard.Name = "chkBoxClipboard";
            this.chkBoxClipboard.Size = new System.Drawing.Size(151, 17);
            this.chkBoxClipboard.TabIndex = 16;
            this.chkBoxClipboard.Text = "Copy words from Clipboard";
            this.chkBoxClipboard.UseVisualStyleBackColor = true;
            this.chkBoxClipboard.CheckedChanged += new System.EventHandler(this.chkBoxClipboard_CheckedChanged);
            // 
            // labelWords
            // 
            this.labelWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelWords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelWords.Location = new System.Drawing.Point(231, 50);
            this.labelWords.Name = "labelWords";
            this.labelWords.Size = new System.Drawing.Size(42, 20);
            this.labelWords.TabIndex = 5;
            this.labelWords.Text = "Words";
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
            this.groupBoxOutput.Controls.Add(this.txtFailedNum);
            this.groupBoxOutput.Controls.Add(this.label2);
            this.groupBoxOutput.Controls.Add(this.label1);
            this.groupBoxOutput.Controls.Add(this.txtFailed);
            this.groupBoxOutput.Controls.Add(this.txtOutputPath);
            this.groupBoxOutput.Controls.Add(this.labelCards);
            this.groupBoxOutput.Controls.Add(this.txtNumOutput);
            this.groupBoxOutput.Controls.Add(this.txtOutput);
            this.groupBoxOutput.Controls.Add(this.btnSave);
            this.groupBoxOutput.Location = new System.Drawing.Point(380, 6);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(369, 284);
            this.groupBoxOutput.TabIndex = 4;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // txtFailedNum
            // 
            this.txtFailedNum.Location = new System.Drawing.Point(300, 22);
            this.txtFailedNum.Name = "txtFailedNum";
            this.txtFailedNum.ReadOnly = true;
            this.txtFailedNum.Size = new System.Drawing.Size(62, 20);
            this.txtFailedNum.TabIndex = 15;
            this.txtFailedNum.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(228, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Failed words";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Success words";
            // 
            // txtFailed
            // 
            this.txtFailed.Location = new System.Drawing.Point(228, 46);
            this.txtFailed.Name = "txtFailed";
            this.txtFailed.ReadOnly = true;
            this.txtFailed.Size = new System.Drawing.Size(134, 200);
            this.txtFailed.TabIndex = 12;
            this.txtFailed.Text = "";
            this.txtFailed.WordWrap = false;
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Enabled = false;
            this.txtOutputPath.Location = new System.Drawing.Point(68, 254);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(293, 20);
            this.txtOutputPath.TabIndex = 6;
            // 
            // labelCards
            // 
            this.labelCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelCards.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelCards.Location = new System.Drawing.Point(93, 22);
            this.labelCards.Name = "labelCards";
            this.labelCards.Size = new System.Drawing.Size(38, 20);
            this.labelCards.TabIndex = 7;
            this.labelCards.Text = "Cards";
            this.labelCards.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNumOutput
            // 
            this.txtNumOutput.Location = new System.Drawing.Point(137, 22);
            this.txtNumOutput.Name = "txtNumOutput";
            this.txtNumOutput.ReadOnly = true;
            this.txtNumOutput.Size = new System.Drawing.Size(82, 20);
            this.txtNumOutput.TabIndex = 6;
            this.txtNumOutput.Text = "0";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(6, 46);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(216, 200);
            this.txtOutput.TabIndex = 2;
            this.txtOutput.Text = "";
            this.txtOutput.WordWrap = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(5, 252);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 25);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // comBoxLanguages
            // 
            this.comBoxLanguages.AllowDrop = true;
            this.comBoxLanguages.Enabled = false;
            this.comBoxLanguages.FormattingEnabled = true;
            this.comBoxLanguages.Items.AddRange(new object[] {
            "[en] English",
            "[en] Vietnamese",
            "[en] English & Vietnamese",
            "[en] Vietnamese & English",
            "[fr] Vietnamese",
            "[fr] English",
            "[fr] English & Vietnamese",
            "[fr] Vietnamese & English",
            "[vn] English",
            "[vn] French"});
            this.comBoxLanguages.Location = new System.Drawing.Point(75, 21);
            this.comBoxLanguages.Name = "comBoxLanguages";
            this.comBoxLanguages.Size = new System.Drawing.Size(141, 21);
            this.comBoxLanguages.TabIndex = 7;
            this.comBoxLanguages.Text = "[en] English";
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
            this.groupBoxProgress.Controls.Add(this.txtProxyString);
            this.groupBoxProgress.Controls.Add(this.chkBoxUseProxy);
            this.groupBoxProgress.Controls.Add(this.labelLanguages);
            this.groupBoxProgress.Controls.Add(this.comBoxLanguages);
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
            // txtProxyString
            // 
            this.txtProxyString.BackColor = System.Drawing.SystemColors.Menu;
            this.txtProxyString.Enabled = false;
            this.txtProxyString.Location = new System.Drawing.Point(512, 56);
            this.txtProxyString.Name = "txtProxyString";
            this.txtProxyString.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtProxyString.Size = new System.Drawing.Size(212, 20);
            this.txtProxyString.TabIndex = 13;
            this.txtProxyString.Text = "http://10.10.10.10:8080";
            // 
            // chkBoxUseProxy
            // 
            this.chkBoxUseProxy.AutoSize = true;
            this.chkBoxUseProxy.Location = new System.Drawing.Point(384, 27);
            this.chkBoxUseProxy.Name = "chkBoxUseProxy";
            this.chkBoxUseProxy.Size = new System.Drawing.Size(73, 17);
            this.chkBoxUseProxy.TabIndex = 12;
            this.chkBoxUseProxy.Text = "Use proxy";
            this.chkBoxUseProxy.UseVisualStyleBackColor = true;
            this.chkBoxUseProxy.CheckedChanged += new System.EventHandler(this.chkBoxUseProxy_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(754, 385);
            this.Controls.Add(this.groupBoxInput);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Flashcards Generator v8.1";
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
        private System.Windows.Forms.ComboBox comBoxLanguages;
        private System.Windows.Forms.Label labelCards;
        private System.Windows.Forms.TextBox txtNumOutput;
        private System.Windows.Forms.Label labelLanguages;
        private System.Windows.Forms.GroupBox groupBoxProgress;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelProxyConnection;
        private System.Windows.Forms.TextBox txtProxyString;
        private System.Windows.Forms.CheckBox chkBoxUseProxy;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label support;
        private System.Windows.Forms.RichTextBox txtFailed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFailedNum;
        private System.Windows.Forms.CheckBox chkBoxClipboard;
    }
}

