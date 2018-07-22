using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlashcardsGenerator.Source;

namespace FlashcardsGenerator
{
    public partial class MainForm : Form
    {
        private readonly UTF8Encoding utf8WithoutBom = new UTF8Encoding(false);

        private readonly FlashcardsGenerator flashcardsGenerator = new FlashcardsGenerator();

        private bool canceled = false;

        private string AnkiCard { get; set; }

        private string Language { get; set; }

        private string Proxy { get; set; }

        private string[] Words { get; set; }

        private List<string> SuccessfulResult { get; set; }

        private List<string> FailedResult { get; set; }

        public MainForm()
        {
            InitializeComponent();
            InitializeFolders();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < Words.Length; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    canceled = true;
                    return;
                }

                AnkiCard = flashcardsGenerator.GenerateFlashCards(Words[i].Trim(), Proxy, Language);
                if (AnkiCard.Contains(GeneratingStatus.CONNECTION_FAILED))
                {
                    MessageBox.Show(MessageBoxProps.CANNOT_CONNECT_TO_DICTIONARY, Source.MessageBoxProps.FAILED);
                    return;
                }
                else if (AnkiCard.Contains(GeneratingStatus.GENERATING_FAILED))
                {
                    FailedResult.Add(AnkiCard.Replace(GeneratingStatus.GENERATING_FAILED, string.Empty));
                }
                else if (AnkiCard.Contains(GeneratingStatus.SPELLING_WRONG))
                {
                    FailedResult.Add(AnkiCard.Replace(GeneratingStatus.SPELLING_WRONG, string.Empty));
                }
                else
                {
                    SuccessfulResult.Add(AnkiCard.Replace(Environment.NewLine, string.Empty));
                }

                backgroundWorker.ReportProgress(i + 1);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            proBar.Value = e.ProgressPercentage;
            if (AnkiCard.Contains(GeneratingStatus.GENERATING_FAILED))
            {
                txtFailure.Text += AnkiCard.Replace(GeneratingStatus.GENERATING_FAILED, "[word not found] ");
            }
            else if (AnkiCard.Contains(GeneratingStatus.SPELLING_WRONG))
            {
                txtFailure.Text += AnkiCard.Replace(GeneratingStatus.SPELLING_WRONG, "[wrong spelling] ");
            }
            else
            {
                txtOutput.Text += AnkiCard.Substring(0, 100) + Environment.NewLine;
            }

            txtOutputNumber.Text = txtOutput.Text.Split('\n').Count(w => !string.IsNullOrEmpty(w) && !w.Equals(Constant.CR)).ToString();
            txtFailureNumber.Text = txtFailure.Text.Split('\n').Count(w => !string.IsNullOrEmpty(w) && !w.Equals(Constant.CR)).ToString();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                RestoreDefaultLayout();
                WriteStringListToFile(SuccessfulResult, Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.ANKI_DECK));
                WriteStringListToFile(new List<string> { cbxLanguages.Text }, Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.LANGUAGE));

                if (!canceled)
                {
                    MessageBox.Show(MessageBoxProps.COMPLETED, MessageBoxProps.INFO);
                }
            }
            else
            {
                MessageBox.Show(MessageBoxProps.ERROR_OCCUR + Environment.NewLine + e.Error.ToString(), MessageBoxProps.ERROR);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            txtInputPath.Text = string.Empty;
            txtInput.Text = string.Empty;

            openFileDialog.CheckFileExists = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = Source.OpenFileDialog.TITLE;
            openFileDialog.InitialDirectory = Source.OpenFileDialog.INITIAL_PATH;
            openFileDialog.DefaultExt = Source.OpenFileDialog.DEFAULT_EXT;
            openFileDialog.Filter = Source.OpenFileDialog.FILTER;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    txtInputPath.Text += file + "; ";
                    txtInput.Text += File.ReadAllText(file);
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnRun.Text = Source.Button.RUNNING;

            btnCancel.Enabled = true;

            cbxLanguages.Enabled = false;
            chkUseProxy.Enabled = false;
            txtProxy.Enabled = false;
            labelProxyConnection.Enabled = false;

            txtOutputNumber.Text = "0";
            txtFailureNumber.Text = "0";
            txtOutput.Text = string.Empty;
            txtFailure.Text = string.Empty;
            proBar.Value = 0;
            proBar.Maximum = Words.Length;

            Language = cbxLanguages.Text;
            Proxy = chkUseProxy.Checked ? txtProxy.Text : string.Empty;
            SuccessfulResult = new List<string>();
            FailedResult = new List<string>();

            backgroundWorker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }

            RestoreDefaultLayout();
            MessageBox.Show(Source.MessageBoxProps.CANCELED, Source.MessageBoxProps.INFO);
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            Words = txtInput.Text.Split('\n').Where(w => !string.IsNullOrEmpty(w) && !w.Equals(Constant.CR)).ToArray();
            txtNumInput.Text = Words.Count().ToString();

            if (Words.Any())
            {
                btnRun.Enabled = true;
                labelLanguages.Enabled = true;
                cbxLanguages.Enabled = true;
            }
            else
            {
                btnRun.Enabled = false;
                labelLanguages.Enabled = false;
                cbxLanguages.Enabled = false;
            }
        }

        private void chkBoxUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            labelProxyConnection.Enabled = chkUseProxy.Checked;
            txtProxy.Enabled = chkUseProxy.Checked;
        }

        private void support_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/ankiflashcard/");
        }

        private void WriteStringListToFile(List<string> stringList, string filePath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath, false, utf8WithoutBom);
                foreach (var content in stringList)
                {
                    writer.WriteLine(content);
                }

                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(MessageBoxProps.ERROR_OCCUR + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace, MessageBoxProps.ERROR);
            }
        }

        private void CleanUpDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    DeleteDirectory(path);
                }
                catch (IOException e)
                {
                    MessageBox.Show(MessageBoxProps.ERROR_OCCUR + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace, Source.MessageBoxProps.ERROR);
                }

                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        private void DeleteDirectory(string dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                DeleteDirectory(di.FullName);
            }

            Directory.Delete(dirPath, false);
        }

        private void RestoreDefaultLayout()
        {
            btnRun.Enabled = true;
            btnRun.Text = Source.Button.RUN;
            btnCancel.Enabled = false;
            cbxLanguages.Enabled = true;
            chkUseProxy.Enabled = true;
            labelProxyConnection.Enabled = chkUseProxy.Checked;
            txtProxy.Enabled = chkUseProxy.Checked;
        }

        private void InitializeFolders()
        {
            CleanUpDirectory(DirectoryPath.ANKI_FLASHCARDS);
            CleanUpDirectory(DirectoryPath.OXFORD);
            CleanUpDirectory(DirectoryPath.SOHA);
            CleanUpDirectory(DirectoryPath.LACVIET);
            CleanUpDirectory(DirectoryPath.CAMBRIDGE);
            CleanUpDirectory(DirectoryPath.COLLINS);
            CleanUpDirectory(DirectoryPath.SOUND);
            CleanUpDirectory(DirectoryPath.IMAGE);

            #region Oxlayout folder
            string input = Properties.Resources._interface;
            File.WriteAllText(Path.Combine(DirectoryPath.OXFORD, FileName.INTERFACE_CSS), input);

            input = Properties.Resources.oxford;
            File.WriteAllText(Path.Combine(DirectoryPath.OXFORD, FileName.OXFORD_CSS), input);

            input = Properties.Resources.responsive;
            File.WriteAllText(Path.Combine(DirectoryPath.OXFORD, FileName.RESPONSIVE_CSS), input);

            Bitmap input2 = Properties.Resources.btn_wordlist;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.BTN_WORDLIST_PNG));

            input2 = Properties.Resources.usonly_audio;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.USONLY_AUDIO_PNG));

            input2 = Properties.Resources.enlarge_img;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ENLARGE_IMG_PNG));

            input2 = Properties.Resources.entry_arrow;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ENTRY_ARROW_PNG));

            input2 = Properties.Resources.entry_bullet;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ENTRY_BULLET_PNG));

            input2 = Properties.Resources.entry_sqbullet;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ENTRY_SQBULLET_PNG));

            input2 = Properties.Resources.go_to_top;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.GO_TO_TOP_PNG));

            input2 = Properties.Resources.icon_academic;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_ACADEMIC_PNG));

            input2 = Properties.Resources.icon_audio_bre;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_AUDIO_BRE_PNG));

            input2 = Properties.Resources.icon_audio_name;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_AUDIO_NAME_PNG));

            input2 = Properties.Resources.icon_ox3000;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_OX3000_PNG));

            input2 = Properties.Resources.icon_plus_minus;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_PLUS_MINUS_PNG));

            input2 = Properties.Resources.icon_plus_minus_grey;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_PLUS_MINUS_GREY_PNG));

            input2 = Properties.Resources.icon_plus_minus_orange;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_PLUS_MINUS_ORANGE_PNG));

            input2 = Properties.Resources.icon_select_arrow_circle_blue;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.ICON_SELECT_ARROW_CIRRLE_BLUE_PNG));

            input2 = Properties.Resources.login_bg;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.LOGIN_BG_PNG));

            input2 = Properties.Resources.pvarr;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.PVARR_PNG));

            input2 = Properties.Resources.pvarr_blue;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.PVARR_BLUE_PNG));

            input2 = Properties.Resources.search_mag;
            input2.Save(Path.Combine(DirectoryPath.OXFORD, FileName.SEARCH_MAG_PNG));
            #endregion

            #region Soha folder
            input = Properties.Resources.main_min;
            File.WriteAllText(Path.Combine(DirectoryPath.SOHA, FileName.MAIN_MIN_CSS), input);

            input2 = Properties.Resources.dot;
            input2.Save(Path.Combine(DirectoryPath.SOHA, FileName.DOT_JPG));

            input2 = Properties.Resources.minus_section;
            input2.Save(Path.Combine(DirectoryPath.SOHA, FileName.MINUS_SECTION_JPG));

            input2 = Properties.Resources.plus_section;
            input2.Save(Path.Combine(DirectoryPath.SOHA, FileName.PLUS_SECTION_JPG));

            input2 = Properties.Resources.hidden;
            input2.Save(Path.Combine(DirectoryPath.SOHA, FileName.HIDDEN_JPG));

            input2 = Properties.Resources.external;
            input2.Save(Path.Combine(DirectoryPath.SOHA, FileName.EXTERNAL_PNG));
            #endregion

            #region Collins folder
            input = Properties.Resources.collins_common;
            File.WriteAllText(Path.Combine(DirectoryPath.LACVIET, FileName.COLLINS_CSS), input);

            input2 = Properties.Resources.icons_right;
            input2.Save(Path.Combine(DirectoryPath.LACVIET, FileName.ICONS_RIGHT_PNG));
            #endregion

            #region LacViet folder
            input = Properties.Resources.home;
            File.WriteAllText(Path.Combine(DirectoryPath.COLLINS, FileName.HOME_CSS), input);

            input2 = Properties.Resources.Icon_6_7;
            input2.Save(Path.Combine(DirectoryPath.COLLINS, FileName.ICON_6_7_PNG));

            input2 = Properties.Resources.Icon_7_4;
            input2.Save(Path.Combine(DirectoryPath.COLLINS, FileName.ICON_7_4_PNG));
            #endregion

            #region Cambridge folder
            input = Properties.Resources.common;
            File.WriteAllText(Path.Combine(DirectoryPath.CAMBRIDGE, FileName.COMMON_CSS), input);

            input2 = Properties.Resources.star;
            input2.Save(Path.Combine(DirectoryPath.CAMBRIDGE, FileName.STAR_PNG));
            #endregion

            #region Anki images & Note Type
            input2 = Properties.Resources.anki;
            input2.Save(Path.Combine(DirectoryPath.IMAGE, FileName.ANKI_PNG));

            byte[] input3 = Properties.Resources._en_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.EN_SINGLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._en_multiformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.EN_MULTIPLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._fr_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.FR_SINGLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._fr_multiformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.FR_MULTIPLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._vn_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(DirectoryPath.ANKI_FLASHCARDS, FileName.VN_SINGLE_FORM_ABCDEFGHLONGLEE123), input3);
            #endregion
        }
    }
}
