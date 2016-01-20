using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication;

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
        private Boolean canceled = false;

        private UTF8Encoding _utf8WithoutBom = new UTF8Encoding(false);
        private FlashcardsGenerator flashcardsGenerator = new FlashcardsGenerator();

        public MainForm()
        {
            InitializeComponent();
            EmbededCopy();
            //flashcardsGenerator.MonitorTextCopied(e);

            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = new Point(300, 100);
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

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnRun.Enabled = false;
            btnRun.Text = "Running...";
            comBoxLanguages.Enabled = false;
            chkBoxUseProxy.Enabled = false;
            labelProxyConnection.Enabled = false;
            txtProxyString.Enabled = false;
            btnSave.Enabled = false;
            txtOutputPath.Enabled = false;

            _language = comBoxLanguages.Text;
            _proxy = chkBoxUseProxy.Checked ? txtProxyString.Text : "";

            proBar.Maximum = _words.Length;
            txtOutput.Text = "";
            txtFailed.Text = "";
            txtOutputPath.Text = "";
            txtNumOutput.Text = "0";
            txtFailedNum.Text = "0";
            proBar.Value = 0;
            _successResult = new List<string>();
            _failureResult = new List<string>();

            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < _words.Length; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    canceled = true;
                    return;
                }

                if (i == 0)
                {
                    Notification notification = new Notification();
                    notification.SendEmail();
                }

                _ankiCard = flashcardsGenerator.GenerateFlashCards(_words[i].Replace("\r", "").Replace("\n", ""), _proxy, _language);
                if (_ankiCard.Contains(FlashcardsGenerator.ConNotOk))
                {
                    MessageBox.Show("Cannot get dictionnary's content.\n" +
                                    "Please check your connection.", "Failed");
                    return;
                }

                if (_ankiCard.Contains(FlashcardsGenerator.GenNotOk))
                {
                    _failureResult.Add(_ankiCard.Replace(FlashcardsGenerator.GenNotOk + " - ", ""));
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
            if (_ankiCard.Contains(FlashcardsGenerator.GenNotOk))
            {
                txtFailed.Text += _ankiCard.Replace(FlashcardsGenerator.GenNotOk + " - ", "");
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
            if (!canceled)
            {
                MessageBox.Show("Completed.\n", "Info");
            }
        }

        private void chkBoxUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            labelProxyConnection.Enabled = chkBoxUseProxy.Checked;
            txtProxyString.Enabled = chkBoxUseProxy.Checked;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ankiDeck = @".\AnkiFlashcards\ankiDeck.csv";
            SaveFile(_successResult, ankiDeck);

            string language = @".\AnkiFlashcards\Language.txt";
            List<string> Languages = new List<string>();
            Languages.Add(comBoxLanguages.Text);
            WriteArrayList(Languages, language);

            txtOutputPath.Text = @"Saved to: AnkiFlashcards\ankiDeck.csv";
            MessageBox.Show("Saved.\n", "Info");
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

            if (txtNumOutput.Text == "0")
            {
                btnSave.Enabled = false;
                txtOutputPath.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                txtOutputPath.Enabled = true;
            }
        }

        private void EmbededCopy()
        {
            string layout = @".\AnkiFlashcards\oxlayout";
            string soha = @".\AnkiFlashcards\soha";
            string sound = @".\AnkiFlashcards\sounds";
            string image = @".\AnkiFlashcards\images";

            ChecknCreateFolders(@".\AnkiFlashcards");
            ChecknCreateFolders(layout);
            ChecknCreateFolders(soha);
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
            input2.Save(layout + @"\usonly_audio.png");

            input2 = Properties.Resources.enlarge_img;
            input2.Save(layout + @"\enlarge_img.png");

            input2 = Properties.Resources.entry_arrow;
            input2.Save(layout + @"\entry_arrow.png");

            input2 = Properties.Resources.entry_bullet;
            input2.Save(layout + @"\entry_bullet.png");

            input2 = Properties.Resources.entry_sqbullet;
            input2.Save(layout + @"\entry_sqbullet.png");

            input2 = Properties.Resources.go_to_top;
            input2.Save(layout + @"\go_to_top.png");

            input2 = Properties.Resources.icon_academic;
            input2.Save(layout + @"\icon_academic.png");

            input2 = Properties.Resources.icon_audio_bre;
            input2.Save(layout + @"\icon_audio_bre.png");

            input2 = Properties.Resources.icon_audio_name;
            input2.Save(layout + @"\icon_audio_name.png");

            input2 = Properties.Resources.icon_ox3000;
            input2.Save(layout + @"\icon_ox3000.png");

            input2 = Properties.Resources.icon_plus_minus;
            input2.Save(layout + @"\icon_plus_minus.png");

            input2 = Properties.Resources.icon_plus_minus_grey;
            input2.Save(layout + @"\icon_plus_minus_grey.png");

            input2 = Properties.Resources.icon_plus_minus_orange;
            input2.Save(layout + @"\icon_plus_minus_orange.png");

            input2 = Properties.Resources.icon_select_arrow_circle_blue;
            input2.Save(layout + @"\icon_select_arrow_circle_blue.png");

            input2 = Properties.Resources.login_bg;
            input2.Save(layout + @"\login_bg.png");

            input2 = Properties.Resources.pvarr;
            input2.Save(layout + @"\pvarr.png");

            input2 = Properties.Resources.pvarr_blue;
            input2.Save(layout + @"\pvarr_blue.png");

            input2 = Properties.Resources.search_mag;
            input2.Save(layout + @"\search_mag.png");
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

            #region Anki images & Note Type
            input2 = Properties.Resources.anki;
            input2.Save(image + @"\anki.png");

            Byte[] input3 = Properties.Resources.singleformABCDEFGHLONGLEE;
            File.WriteAllBytes(@".\AnkiFlashcards\singleformABCDEFGHLONGLEE.apkg", input3);

            input3 = Properties.Resources.multiformABCDEFGHLONGLEE;
            File.WriteAllBytes(@".\AnkiFlashcards\multiformABCDEFGHLONGLEE.apkg", input3);
            #endregion
        }

        private void support_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/ankiflashcard/");
        }
    }
}
