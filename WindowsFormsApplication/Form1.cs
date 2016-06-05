using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FlashcardsGeneratorApplication
{
    public partial class MainForm : Form
    {
        private string _ankiCard;
        private string _language;
        private string _proxy;
        private string[] _words;
        private List<string> _successResult;
        private List<string> _failureResult;
        private bool _canceled = false;
        private string key = "9GC3P-GH4JR-KH7JE-X8BQB-9VDE0-v92016";
        private string licenseFile = @".\License\License";

        private readonly UTF8Encoding _utf8WithoutBom = new UTF8Encoding(false);
        private readonly FlashcardsGenerator _flashcardsGenerator = new FlashcardsGenerator();

        [DllImport("user32.dll")]
        static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll")]
        static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private IntPtr _clipboardViewerNext;

        private const int WM_DRAWCLIPBOARD = 0x0308;
        private const int WM_CHANGECBCHAIN = 0x030D;

        public MainForm()
        {
            InitializeComponent();
            EmbededCopy();

            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            Shown += Form1_Shown;
            FormClosing += Form1_FormClosing;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            txtInputPath.Text = "";
            txtInput.Text = "";

            openFileDialog.CheckFileExists = true;
            openFileDialog.RestoreDirectory = true;

            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Browse Text Files";

            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    try
                    {
                        txtInputPath.Text += file + "; ";
                        txtInput.Text += File.ReadAllText(file);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnRun.Enabled = false;
            btnRun.Text = "Running...";
            comBoxLanguages.Enabled = false;
            chkBoxUseProxy.Enabled = false;
            labelProxyConnection.Enabled = false;
            txtProxyString.Enabled = false;

            _language = comBoxLanguages.Text;
            _proxy = chkBoxUseProxy.Checked ? txtProxyString.Text : "";

            proBar.Maximum = _words.Length;
            txtOutput.Text = "";
            txtFailed.Text = "";
            txtNumOutput.Text = "0";
            txtFailedNum.Text = "0";
            proBar.Value = 0;
            _successResult = new List<string>();
            _failureResult = new List<string>();

            backgroundWorker.RunWorkerAsync();
        }


        private void chkBoxClipboard_CheckedChanged(object sender, EventArgs e)
        {
            Clipboard.Clear();
            if (chkBoxClipboard.Checked)
            {
                // Adds our form to the chain of clipboard viewers.
                _clipboardViewerNext = SetClipboardViewer(this.Handle);
            }
            else
            {
                ChangeClipboardChain(this.Handle, _clipboardViewerNext);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    IDataObject iData = Clipboard.GetDataObject();      // Clipboard's data

                    if (iData.GetDataPresent(DataFormats.Text))
                    {
                        string text = (string)iData.GetData(DataFormats.Text);      // Clipboard text
                        txtInput.Text += text + "\r\n";
                    }
                    else if (iData.GetDataPresent(DataFormats.Bitmap))
                    {
                        Bitmap image = (Bitmap)iData.GetData(DataFormats.Bitmap);   // Clipboard image
                    }
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == _clipboardViewerNext)
                        _clipboardViewerNext = m.LParam;
                    else
                        SendMessage(_clipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clipboard.Clear();
            // Removes our from the chain of clipboard viewers when the form closes.
            ChangeClipboardChain(this.Handle, _clipboardViewerNext);
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            _words = txtInput.Text.Split('\n').Where(w => !string.IsNullOrEmpty(w) && !w.Equals("\r")).ToArray();
            txtNumInput.Text = _words.Count().ToString();

            if (_words.Any())
            {
                btnRun.Enabled = true;
                labelLanguages.Enabled = true;
                comBoxLanguages.Enabled = true;
            }
            else
            {
                btnRun.Enabled = false;
                labelLanguages.Enabled = false;
                comBoxLanguages.Enabled = false;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }

            RestoreLayout();
            MessageBox.Show("Canceled.\n", "Info");
        }


        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < _words.Length; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    _canceled = true;
                    return;
                }

                _ankiCard = _flashcardsGenerator.GenerateFlashCards(_words[i].Replace("\r", "").Replace("\n", ""), _proxy, _language);
                if (_ankiCard.Contains(FlashcardsGenerator.CONNECTION_FAILED))
                {
                    MessageBox.Show("Cannot get dictionnary's content.\n" +
                                    "Please check your connection.", "Failed");
                    return;
                }

                if (_ankiCard.Contains(FlashcardsGenerator.GENERATING_FAILED))
                {
                    _failureResult.Add(_ankiCard.Replace(FlashcardsGenerator.GENERATING_FAILED + " - ", ""));
                }
                else
                {
                    _successResult.Add(_ankiCard.Replace("\r\n", ""));
                }
                backgroundWorker.ReportProgress(i + 1);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            proBar.Value = e.ProgressPercentage;
            if (_ankiCard.Contains(FlashcardsGenerator.GENERATING_FAILED))
            {
                txtFailed.Text += _ankiCard.Replace(FlashcardsGenerator.GENERATING_FAILED + " - ", "");
            }
            else
            {
                txtOutput.Text += _ankiCard.Substring(0, 100) + "\r\n";
            }
            txtNumOutput.Text = txtOutput.Text.Split('\n').Count(w => !string.IsNullOrEmpty(w) && !w.Equals("\r")).ToString();
            txtFailedNum.Text = txtFailed.Text.Split('\n').Count(w => !string.IsNullOrEmpty(w) && !w.Equals("\r")).ToString();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RestoreLayout();

            string ankiDeck = @".\AnkiFlashcards\ankiDeck.csv";
            SaveFile(_successResult, ankiDeck);

            string language = @".\AnkiFlashcards\Language.txt";
            List<string> Languages = new List<string>();
            Languages.Add(comBoxLanguages.Text);
            WriteArrayList(Languages, language);

            if (!_canceled)
            {
                MessageBox.Show("Completed.\n", "Info");
            }
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (btnRegister.Text.Equals("Register"))
            {
                btnRegister.Text = "Validate";
                System.Diagnostics.Process.Start("http://goo.gl/forms/J3eFTiHLBZ7HtiLQ2");
                txtLicenseKey.Text = "Input license key here!";
                txtLicenseKey.Enabled = true;
            }
            else
            {
                List<string> licenses = new List<string>();
                licenses.Add(txtLicenseKey.Text);
                SaveFile(licenses, licenseFile);
                Thread.Sleep(1000);

                if (IsValidLicenseKey())
                    MessageBox.Show("Congratulation, you registered successfully.\n", "Info");
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            IsValidLicenseKey();
        }

        private bool IsValidLicenseKey()
        {
            if (File.Exists(licenseFile))
            {
                string check = File.ReadAllText(licenseFile).Replace("\n", "").Replace("\r", "");
                if (!key.Equals(check))
                {
                    MessageBox.Show("Your current key is invalid or expired, register a new one.", "Warning");
                    btnRegister.Enabled = true;
                    btnRegister.Text = "Register";
                    txtInput.Enabled = false;
                    btnOpen.Enabled = false;
                    txtInputPath.Enabled = false;
                    btnRun.Enabled = false;
                    return false;
                }
                else
                {
                    txtLicenseKey.Text = "Registered :)";
                    btnRegister.Enabled = false;
                    btnRegister.Text = "Validated";
                    txtLicenseKey.Enabled = false;
                    txtInput.Enabled = true;
                    btnOpen.Enabled = true;
                    txtInputPath.Enabled = true;
                    btnRun.Enabled = true;
                    return true;
                }
            }
            else
            {
                MessageBox.Show("You haven't registered yet! Please register to run Flashcard Generator.\n", "Message");
                btnRegister.Enabled = true;
                btnRegister.Text = "Register";
                txtInput.Enabled = false;
                btnOpen.Enabled = false;
                txtInputPath.Enabled = false;
                btnRun.Enabled = false;
                return false;
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to uninstall Flashcard Generator?", "Uninstall Confirmation!!!", MessageBoxButtons.YesNo);
            
            if (confirmResult != DialogResult.Yes) return;
            Application.Exit();
            System.Diagnostics.Process.Start(@"Uninstall.bat");
        }


        private void chkBoxUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            labelProxyConnection.Enabled = chkBoxUseProxy.Checked;
            txtProxyString.Enabled = chkBoxUseProxy.Checked;
        }

        private void support_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/ankiflashcard/");
        }


        private void WriteArrayList(List<string> arrayList, string filePath)
        {
            TextWriter tw = null;

            try
            {
                tw = File.CreateText(filePath);
                foreach (var a in arrayList)
                    tw.WriteLine(a);
                tw.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file!");
            }
            finally
            {
                if (tw != null)
                    tw.Close();
            }
        }

        private void SaveFile(List<string> arrayList, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                StreamWriter outputFile = new StreamWriter(filePath, false, _utf8WithoutBom);
                foreach (var content in arrayList)
                {
                    outputFile.WriteLine(content);
                }
                outputFile.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERR: " + ex.Message);
            }
        }

        private void ChecknCreateFolders(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    ClearDirectory(path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        private void ClearDirectory(string dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearDirectory(di.FullName);
                di.Delete();
            }

            Directory.Delete(dirPath, false);
        }

        private void RestoreLayout()
        {
            btnCancel.Enabled = false;
            btnRun.Enabled = true;
            btnRun.Text = "Run";
            comBoxLanguages.Enabled = true;
            chkBoxUseProxy.Enabled = true;
            labelProxyConnection.Enabled = chkBoxUseProxy.Checked;
            txtProxyString.Enabled = chkBoxUseProxy.Checked;
        }

        private void EmbededCopy()
        {
            string layout = @".\AnkiFlashcards\oxlayout";
            string soha = @".\AnkiFlashcards\soha";
            string lacViet = @".\AnkiFlashcards\lacViet";
            string cambridge = @".\AnkiFlashcards\cambridge";
            string collins = @".\AnkiFlashcards\collins";
            string sound = @".\AnkiFlashcards\sounds";
            string image = @".\AnkiFlashcards\images";

            ChecknCreateFolders(@".\AnkiFlashcards");
            ChecknCreateFolders(layout);
            ChecknCreateFolders(soha);
            ChecknCreateFolders(lacViet);
            ChecknCreateFolders(cambridge);
            ChecknCreateFolders(collins);
            ChecknCreateFolders(sound);
            ChecknCreateFolders(image);

            #region Oxlayout folder
            string input = Properties.Resources._interface;
            File.WriteAllText(layout + @"\interface.css", input);

            input = Properties.Resources.oxford;
            File.WriteAllText(layout + @"\oxford.css", input);

            input = Properties.Resources.responsive;
            File.WriteAllText(layout + @"\responsive.css", input);

            Bitmap input2 = Properties.Resources.btn_wordlist;
            input2.Save(layout + @"\btn-wordlist.png");

            input2 = Properties.Resources.usonly_audio;
            input2.Save(layout + @"\usonly-audio.png");

            input2 = Properties.Resources.enlarge_img;
            input2.Save(layout + @"\enlarge-img.png");

            input2 = Properties.Resources.entry_arrow;
            input2.Save(layout + @"\entry-arrow.png");

            input2 = Properties.Resources.entry_bullet;
            input2.Save(layout + @"\entry-bullet.png");

            input2 = Properties.Resources.entry_sqbullet;
            input2.Save(layout + @"\entry-sqbullet.png");

            input2 = Properties.Resources.go_to_top;
            input2.Save(layout + @"\go-to-top.png");

            input2 = Properties.Resources.icon_academic;
            input2.Save(layout + @"\icon-academic.png");

            input2 = Properties.Resources.icon_audio_bre;
            input2.Save(layout + @"\icon-audio-bre.png");

            input2 = Properties.Resources.icon_audio_name;
            input2.Save(layout + @"\icon-audio-name.png");

            input2 = Properties.Resources.icon_ox3000;
            input2.Save(layout + @"\icon-ox3000.png");

            input2 = Properties.Resources.icon_plus_minus;
            input2.Save(layout + @"\icon-plus-minus.png");

            input2 = Properties.Resources.icon_plus_minus_grey;
            input2.Save(layout + @"\icon-plus-minus-grey.png");

            input2 = Properties.Resources.icon_plus_minus_orange;
            input2.Save(layout + @"\icon-plus-minus-orange.png");

            input2 = Properties.Resources.icon_select_arrow_circle_blue;
            input2.Save(layout + @"\icon-select-arrow-circle-blue.png");

            input2 = Properties.Resources.login_bg;
            input2.Save(layout + @"\login-bg.png");

            input2 = Properties.Resources.pvarr;
            input2.Save(layout + @"\pvarr.png");

            input2 = Properties.Resources.pvarr_blue;
            input2.Save(layout + @"\pvarr-blue.png");

            input2 = Properties.Resources.search_mag;
            input2.Save(layout + @"\search-mag.png");
            #endregion

            #region Soha folder
            input = Properties.Resources.main_min;
            File.WriteAllText(soha + @"\main_min.css", input);

            input2 = Properties.Resources.dot;
            input2.Save(soha + @"\dot.jpg");

            input2 = Properties.Resources.minus_section;
            input2.Save(soha + @"\minus_section.jpg");

            input2 = Properties.Resources.plus_section;
            input2.Save(soha + @"\plus_section.jpg");

            input2 = Properties.Resources.hidden;
            input2.Save(soha + @"\hidden.jpg");

            input2 = Properties.Resources.external;
            input2.Save(soha + @"\external.png");
            #endregion

            #region LacViet folder
            input = Properties.Resources.main;
            File.WriteAllText(lacViet + @"\main.css", input);

            input2 = Properties.Resources.icons_right;
            input2.Save(lacViet + @"\icons-right.png");
            #endregion

            #region Collins folder
            input = Properties.Resources.home;
            File.WriteAllText(collins + @"\home.css", input);

            input2 = Properties.Resources.Icon_6_7;
            input2.Save(collins + @"\Icon_6_7.png");

            input2 = Properties.Resources.Icon_7_4;
            input2.Save(collins + @"\Icon_7_4.png");
            #endregion

            #region Cambridge folder
            input = Properties.Resources.common;
            File.WriteAllText(cambridge + @"\common.css", input);

            input2 = Properties.Resources.star;
            input2.Save(cambridge + @"\star.png");
            #endregion

            #region Anki images & Note Type
            input2 = Properties.Resources.anki;
            input2.Save(image + @"\anki.png");

            Byte[] input3 = Properties.Resources._en_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(@".\AnkiFlashcards\[EN]singleformABCDEFGHLONGLEE123.apkg", input3);

            input3 = Properties.Resources._en_multiformABCDEFGHLONGLEE123;
            File.WriteAllBytes(@".\AnkiFlashcards\[EN]multiformABCDEFGHLONGLEE123.apkg", input3);

            input3 = Properties.Resources._fr_multiformABCDEFGHLONGLEE123;
            File.WriteAllBytes(@".\AnkiFlashcards\[FR]multiformABCDEFGHLONGLEE123.apkg", input3);

            input3 = Properties.Resources._fr_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(@".\AnkiFlashcards\[FR]singleformABCDEFGHLONGLEE123.apkg", input3);

            input3 = Properties.Resources._vn_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(@".\AnkiFlashcards\[VN]singleformABCDEFGHLONGLEE123.apkg", input3);
            #endregion
        }
    }
}
