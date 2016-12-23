namespace FlashcardsGenerator
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
            btnOpen = new System.Windows.Forms.Button();
            txtInputPath = new System.Windows.Forms.TextBox();
            groupBoxInput = new System.Windows.Forms.GroupBox();
            labelWords = new System.Windows.Forms.Label();
            txtNumInput = new System.Windows.Forms.TextBox();
            txtInput = new System.Windows.Forms.RichTextBox();
            groupBoxOutput = new System.Windows.Forms.GroupBox();
            txtFailureNumber = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            txtFailure = new System.Windows.Forms.RichTextBox();
            txtOutputNumber = new System.Windows.Forms.TextBox();
            txtOutput = new System.Windows.Forms.RichTextBox();
            proBar = new System.Windows.Forms.ProgressBar();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            btnRun = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            cbxLanguages = new System.Windows.Forms.ComboBox();
            labelLanguages = new System.Windows.Forms.Label();
            groupBoxProgress = new System.Windows.Forms.GroupBox();
            support = new System.Windows.Forms.Label();
            labelProxyConnection = new System.Windows.Forms.Label();
            txtProxy = new System.Windows.Forms.TextBox();
            chkUseProxy = new System.Windows.Forms.CheckBox();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            groupBoxInput.SuspendLayout();
            groupBoxOutput.SuspendLayout();
            groupBoxProgress.SuspendLayout();
            SuspendLayout();
            // 
            // btnOpen
            // 
            btnOpen.AutoSize = true;
            btnOpen.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            btnOpen.Location = new System.Drawing.Point(9, 19);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new System.Drawing.Size(59, 23);
            btnOpen.TabIndex = 1;
            btnOpen.Text = "Open";
            btnOpen.UseVisualStyleBackColor = false;
            btnOpen.Click += new System.EventHandler(btnOpen_Click);
            // 
            // txtInputPath
            // 
            txtInputPath.BackColor = System.Drawing.SystemColors.Menu;
            txtInputPath.Location = new System.Drawing.Point(74, 20);
            txtInputPath.Name = "txtInputPath";
            txtInputPath.Size = new System.Drawing.Size(287, 20);
            txtInputPath.TabIndex = 2;
            // 
            // groupBoxInput
            // 
            groupBoxInput.Controls.Add(labelWords);
            groupBoxInput.Controls.Add(txtNumInput);
            groupBoxInput.Controls.Add(txtInput);
            groupBoxInput.Controls.Add(txtInputPath);
            groupBoxInput.Controls.Add(btnOpen);
            groupBoxInput.Location = new System.Drawing.Point(5, 6);
            groupBoxInput.Name = "groupBoxInput";
            groupBoxInput.Size = new System.Drawing.Size(369, 277);
            groupBoxInput.TabIndex = 3;
            groupBoxInput.TabStop = false;
            groupBoxInput.Text = "Input";
            // 
            // labelWords
            // 
            labelWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            labelWords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            labelWords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelWords.Location = new System.Drawing.Point(209, 50);
            labelWords.Name = "labelWords";
            labelWords.Size = new System.Drawing.Size(65, 19);
            labelWords.TabIndex = 5;
            labelWords.Text = "Total Words";
            labelWords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNumInput
            // 
            txtNumInput.Location = new System.Drawing.Point(279, 50);
            txtNumInput.Name = "txtNumInput";
            txtNumInput.ReadOnly = true;
            txtNumInput.Size = new System.Drawing.Size(82, 20);
            txtNumInput.TabIndex = 4;
            txtNumInput.Text = "0";
            // 
            // txtInput
            // 
            txtInput.BackColor = System.Drawing.SystemColors.Control;
            txtInput.Location = new System.Drawing.Point(6, 78);
            txtInput.Name = "txtInput";
            txtInput.Size = new System.Drawing.Size(355, 193);
            txtInput.TabIndex = 3;
            txtInput.Text = string.Empty;
            txtInput.WordWrap = false;
            txtInput.TextChanged += new System.EventHandler(txtInput_TextChanged);
            // 
            // groupBoxOutput
            // 
            groupBoxOutput.Controls.Add(txtFailureNumber);
            groupBoxOutput.Controls.Add(label2);
            groupBoxOutput.Controls.Add(label1);
            groupBoxOutput.Controls.Add(txtFailure);
            groupBoxOutput.Controls.Add(txtOutputNumber);
            groupBoxOutput.Controls.Add(txtOutput);
            groupBoxOutput.Location = new System.Drawing.Point(380, 6);
            groupBoxOutput.Name = "groupBoxOutput";
            groupBoxOutput.Size = new System.Drawing.Size(369, 277);
            groupBoxOutput.TabIndex = 4;
            groupBoxOutput.TabStop = false;
            groupBoxOutput.Text = "Output";
            // 
            // txtFailedNum
            // 
            txtFailureNumber.Location = new System.Drawing.Point(312, 22);
            txtFailureNumber.Name = "txtFailedNum";
            txtFailureNumber.ReadOnly = true;
            txtFailureNumber.Size = new System.Drawing.Size(49, 20);
            txtFailureNumber.TabIndex = 15;
            txtFailureNumber.Text = "0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label2.Location = new System.Drawing.Point(184, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(43, 13);
            label2.TabIndex = 14;
            label2.Text = "Failures";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label1.Location = new System.Drawing.Point(8, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 13);
            label1.TabIndex = 13;
            label1.Text = "Success";
            // 
            // txtFailed
            // 
            txtFailure.Location = new System.Drawing.Point(184, 46);
            txtFailure.Name = "txtFailed";
            txtFailure.ReadOnly = true;
            txtFailure.Size = new System.Drawing.Size(178, 225);
            txtFailure.TabIndex = 12;
            txtFailure.Text = string.Empty;
            txtFailure.WordWrap = false;
            // 
            // txtNumOutput
            // 
            txtOutputNumber.Location = new System.Drawing.Point(126, 22);
            txtOutputNumber.Name = "txtNumOutput";
            txtOutputNumber.ReadOnly = true;
            txtOutputNumber.Size = new System.Drawing.Size(50, 20);
            txtOutputNumber.TabIndex = 6;
            txtOutputNumber.Text = "0";
            // 
            // txtOutput
            // 
            txtOutput.Location = new System.Drawing.Point(6, 46);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new System.Drawing.Size(172, 225);
            txtOutput.TabIndex = 2;
            txtOutput.Text = string.Empty;
            txtOutput.WordWrap = false;
            // 
            // proBar
            // 
            proBar.Location = new System.Drawing.Point(7, 56);
            proBar.Name = "proBar";
            proBar.Size = new System.Drawing.Size(354, 19);
            proBar.TabIndex = 5;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "Word List.txt";
            openFileDialog.Multiselect = true;
            // 
            // btnRun
            // 
            btnRun.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            btnRun.Enabled = false;
            btnRun.Location = new System.Drawing.Point(237, 19);
            btnRun.Name = "btnRun";
            btnRun.Size = new System.Drawing.Size(59, 25);
            btnRun.TabIndex = 3;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = false;
            btnRun.Click += new System.EventHandler(btnRun_Click);
            // 
            // btnCancel
            // 
            btnCancel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            btnCancel.Enabled = false;
            btnCancel.Location = new System.Drawing.Point(302, 19);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(59, 25);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // comBoxLanguages
            // 
            cbxLanguages.AllowDrop = true;
            cbxLanguages.Enabled = false;
            cbxLanguages.FormattingEnabled = true;
            cbxLanguages.Items.AddRange(new object[] {
            "[EN] English",
            "[EN] Vietnamese",
            "[EN] Chinese",
            "[EN] English & Vietnamese",
            "[EN] Vietnamese & English",
            "[FR] Vietnamese",
            "[FR] English",
            "[FR] English & Vietnamese",
            "[FR] Vietnamese & English",
            "[VN] English",
            "[VN] French"});
            cbxLanguages.Location = new System.Drawing.Point(75, 21);
            cbxLanguages.Name = "comBoxLanguages";
            cbxLanguages.Size = new System.Drawing.Size(141, 21);
            cbxLanguages.TabIndex = 7;
            cbxLanguages.Text = "[en] English";
            // 
            // labelLanguages
            // 
            labelLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            labelLanguages.Enabled = false;
            labelLanguages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            labelLanguages.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelLanguages.Location = new System.Drawing.Point(6, 21);
            labelLanguages.Name = "labelLanguages";
            labelLanguages.Size = new System.Drawing.Size(63, 21);
            labelLanguages.TabIndex = 8;
            labelLanguages.Text = "Languages";
            labelLanguages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxProgress
            // 
            groupBoxProgress.Controls.Add(support);
            groupBoxProgress.Controls.Add(labelProxyConnection);
            groupBoxProgress.Controls.Add(txtProxy);
            groupBoxProgress.Controls.Add(chkUseProxy);
            groupBoxProgress.Controls.Add(labelLanguages);
            groupBoxProgress.Controls.Add(cbxLanguages);
            groupBoxProgress.Controls.Add(proBar);
            groupBoxProgress.Controls.Add(btnCancel);
            groupBoxProgress.Controls.Add(btnRun);
            groupBoxProgress.Location = new System.Drawing.Point(5, 289);
            groupBoxProgress.Name = "groupBoxProgress";
            groupBoxProgress.Size = new System.Drawing.Size(744, 87);
            groupBoxProgress.TabIndex = 9;
            groupBoxProgress.TabStop = false;
            groupBoxProgress.Text = "Progress";
            // 
            // support
            // 
            support.AutoSize = true;
            support.Location = new System.Drawing.Point(509, 28);
            support.Name = "support";
            support.Size = new System.Drawing.Size(173, 13);
            support.TabIndex = 15;
            support.Text = "Support: hoanglongtc7@gmail.com";
            support.Click += new System.EventHandler(support_Click);
            // 
            // labelProxyConnection
            // 
            labelProxyConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            labelProxyConnection.Enabled = false;
            labelProxyConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            labelProxyConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelProxyConnection.Location = new System.Drawing.Point(381, 56);
            labelProxyConnection.Name = "labelProxyConnection";
            labelProxyConnection.Size = new System.Drawing.Size(125, 19);
            labelProxyConnection.TabIndex = 14;
            labelProxyConnection.Text = "Proxy Connection String";
            labelProxyConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtProxyString
            // 
            txtProxy.BackColor = System.Drawing.SystemColors.Menu;
            txtProxy.Enabled = false;
            txtProxy.Location = new System.Drawing.Point(512, 56);
            txtProxy.Name = "txtProxyString";
            txtProxy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            txtProxy.Size = new System.Drawing.Size(212, 20);
            txtProxy.TabIndex = 13;
            txtProxy.Text = "http://10.10.10.10:8080";
            // 
            // chkBoxUseProxy
            // 
            chkUseProxy.AutoSize = true;
            chkUseProxy.Location = new System.Drawing.Point(384, 27);
            chkUseProxy.Name = "chkBoxUseProxy";
            chkUseProxy.Size = new System.Drawing.Size(73, 17);
            chkUseProxy.TabIndex = 12;
            chkUseProxy.Text = "Use proxy";
            chkUseProxy.UseVisualStyleBackColor = true;
            chkUseProxy.CheckedChanged += new System.EventHandler(chkBoxUseProxy_CheckedChanged);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(754, 380);
            Controls.Add(groupBoxInput);
            Controls.Add(groupBoxOutput);
            Controls.Add(groupBoxProgress);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = ((System.Drawing.Icon)(resources.GetObject("$Icon")));
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Flashcards Generator v9.1";
            groupBoxInput.ResumeLayout(false);
            groupBoxInput.PerformLayout();
            groupBoxOutput.ResumeLayout(false);
            groupBoxOutput.PerformLayout();
            groupBoxProgress.ResumeLayout(false);
            groupBoxProgress.PerformLayout();
            ResumeLayout(false);

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

