using FlashcardGenerator.Source;
using FlashcardGenerator.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlashcardGenerator
{
    public partial class MainForm : Form
    {
        private string CardContent { get; set; }

        private string Language { get; set; }

        private string Proxy { get; set; }

        private List<string> InputWords { get; set; }

        private List<string> CompletedWords { get; set; }

        private List<string> FailedWords { get; set; }

        private bool Canceled = false;

        private readonly UTF8Encoding utf8WithoutBom = new UTF8Encoding(false);

        private readonly CardGenerator cardGenerator = new CardGenerator();

        public MainForm()
        {
            InitializeComponent();
            PopulateResources();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < InputWords.Count; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    Canceled = true;
                    return;
                }

                CardContent = cardGenerator.GenerateFlashCard(InputWords[i].Trim(), Proxy, Language);
                if (CardContent.Contains(GeneratingStatus.CONNECTION_FAILED))
                {
                    MessageBox.Show(MsgBoxProps.CANNOT_CONNECT_TO_DICTIONARY, MsgBoxProps.FAILED);
                    return;
                }
                else if (CardContent.Contains(GeneratingStatus.WORD_NOT_FOUND))
                {
                    FailedWords.Add(CardContent.Replace(GeneratingStatus.WORD_NOT_FOUND, string.Empty).Trim());
                }
                else
                {
                    CompletedWords.Add(CardContent.Trim());
                }

                backgroundWorker.ReportProgress(i + 1);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            proBar.Value = e.ProgressPercentage;
            if (CardContent.Contains(GeneratingStatus.WORD_NOT_FOUND))
            {
                txtFailure.Text += CardContent.Replace(GeneratingStatus.WORD_NOT_FOUND, " ==> Word not found!");
            }
            else
            {
                txtOutput.Text += CardContent.Substring(0, 100) + Environment.NewLine;
            }

            txtOutputNumber.Text = txtOutput.Text.Split('\n').Count(w => !string.IsNullOrEmpty(w) && !w.Equals(ConstVars.CR)).ToString();
            txtFailureNumber.Text = txtFailure.Text.Split('\n').Count(w => !string.IsNullOrEmpty(w) && !w.Equals(ConstVars.CR)).ToString();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                WriteListStringToFile(CompletedWords, Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.ANKI_DECK));
                RestoreDefaultLayout();

                var enSingleForm = new List<string>() { CardOptions.EN2VI, CardOptions.EN2EN, CardOptions.EN2CH };
                var enMultiForm = new List<string>() { CardOptions.EN2EN_VI, CardOptions.EN2VI_EN };
                var frSingleForm = new List<string>() { CardOptions.FR2EN, CardOptions.FR2VI };
                var frMultiForm = new List<string>() { CardOptions.FR2VI_EN, CardOptions.FR2EN_VI };
                var formType = new List<string>();

                if (enSingleForm.Contains(cbxLanguages.Text))
                {
                    formType.Add("[en]singleform");
                }
                else if (enMultiForm.Contains(cbxLanguages.Text))
                {
                    formType.Add("[en]multiform");
                }
                else if (frSingleForm.Contains(cbxLanguages.Text))
                {
                    formType.Add("[fr]singleform");
                }
                else if (frMultiForm.Contains(cbxLanguages.Text))
                {
                    formType.Add("[fr]multiform");
                }
                else
                {
                    formType.Add("[vn]singleform");
                }

                WriteListStringToFile(formType, Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.LANGUAGE));
                if (!Canceled)
                {
                    MessageBox.Show(MsgBoxProps.COMPLETED, MsgBoxProps.INFO);
                }
            }
            else
            {
                MessageBox.Show(MsgBoxProps.ERROR_OCCUR + Environment.NewLine + e.Error.ToString(), MsgBoxProps.ERROR);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            txtInputPath.Text = string.Empty;
            txtInput.Text = string.Empty;

            openFileDialog.CheckFileExists = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = OpenDialog.TITLE;
            openFileDialog.InitialDirectory = OpenDialog.INITIAL_PATH;
            openFileDialog.DefaultExt = OpenDialog.DEFAULT_EXT;
            openFileDialog.Filter = OpenDialog.FILTER;
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
            btnRun.Text = AnkiBtn.RUNNING;

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
            proBar.Maximum = InputWords.Count;

            Language = cbxLanguages.Text;
            Proxy = chkUseProxy.Checked ? txtProxy.Text : string.Empty;
            CompletedWords = new List<string>();
            FailedWords = new List<string>();

            backgroundWorker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }

            RestoreDefaultLayout();
            MessageBox.Show(MsgBoxProps.CANCELED, MsgBoxProps.INFO);
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            InputWords = txtInput.Text.Split('\n').Where(w => !string.IsNullOrEmpty(w) && !w.Equals(ConstVars.CR)).ToList();
            txtNumInput.Text = InputWords.Count().ToString();

            if (InputWords.Any())
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

        private void WriteListStringToFile(List<string> stringList, string filePath)
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
                MessageBox.Show(MsgBoxProps.ERROR_OCCUR + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace, MsgBoxProps.ERROR);
            }
        }

        private void RestoreDefaultLayout()
        {
            btnRun.Enabled = true;
            btnRun.Text = AnkiBtn.RUN;
            btnCancel.Enabled = false;
            cbxLanguages.Enabled = true;
            chkUseProxy.Enabled = true;
            labelProxyConnection.Enabled = chkUseProxy.Checked;
            txtProxy.Enabled = chkUseProxy.Checked;
        }

        private void PopulateResources()
        {
            InitializeDirectory(AnkiDirs.ANKI_FLASHCARDS);
            InitializeDirectory(AnkiDirs.OXFORD);
            InitializeDirectory(AnkiDirs.SOHA);
            InitializeDirectory(AnkiDirs.LACVIET);
            InitializeDirectory(AnkiDirs.CAMBRIDGE);
            InitializeDirectory(AnkiDirs.COLLINS);
            InitializeDirectory(AnkiDirs.SOUND);
            InitializeDirectory(AnkiDirs.IMAGE);

            #region Oxlayout folder
            string input = Properties.Resources._interface;
            File.WriteAllText(Path.Combine(AnkiDirs.OXFORD, AnkiFile.INTERFACE_CSS), input);

            input = Properties.Resources.oxford;
            File.WriteAllText(Path.Combine(AnkiDirs.OXFORD, AnkiFile.OXFORD_CSS), input);

            input = Properties.Resources.responsive;
            File.WriteAllText(Path.Combine(AnkiDirs.OXFORD, AnkiFile.RESPONSIVE_CSS), input);

            Bitmap input2 = Properties.Resources.btn_wordlist;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.BTN_WORDLIST_PNG));

            input2 = Properties.Resources.usonly_audio;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.USONLY_AUDIO_PNG));

            input2 = Properties.Resources.enlarge_img;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ENLARGE_IMG_PNG));

            input2 = Properties.Resources.entry_arrow;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ENTRY_ARROW_PNG));

            input2 = Properties.Resources.entry_bullet;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ENTRY_BULLET_PNG));

            input2 = Properties.Resources.entry_sqbullet;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ENTRY_SQBULLET_PNG));

            input2 = Properties.Resources.go_to_top;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.GO_TO_TOP_PNG));

            input2 = Properties.Resources.icon_academic;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_ACADEMIC_PNG));

            input2 = Properties.Resources.icon_audio_bre;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_AUDIO_BRE_PNG));

            input2 = Properties.Resources.icon_audio_name;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_AUDIO_NAME_PNG));

            input2 = Properties.Resources.icon_ox3000;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_OX3000_PNG));

            input2 = Properties.Resources.icon_plus_minus;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_PLUS_MINUS_PNG));

            input2 = Properties.Resources.icon_plus_minus_grey;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_PLUS_MINUS_GREY_PNG));

            input2 = Properties.Resources.icon_plus_minus_orange;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_PLUS_MINUS_ORANGE_PNG));

            input2 = Properties.Resources.icon_select_arrow_circle_blue;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.ICON_SELECT_ARROW_CIRRLE_BLUE_PNG));

            input2 = Properties.Resources.login_bg;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.LOGIN_BG_PNG));

            input2 = Properties.Resources.pvarr;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.PVARR_PNG));

            input2 = Properties.Resources.pvarr_blue;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.PVARR_BLUE_PNG));

            input2 = Properties.Resources.search_mag;
            input2.Save(Path.Combine(AnkiDirs.OXFORD, AnkiFile.SEARCH_MAG_PNG));
            #endregion

            #region Soha folder
            input = Properties.Resources.main_min;
            File.WriteAllText(Path.Combine(AnkiDirs.SOHA, AnkiFile.MAIN_MIN_CSS), input);

            input2 = Properties.Resources.dot;
            input2.Save(Path.Combine(AnkiDirs.SOHA, AnkiFile.DOT_JPG));

            input2 = Properties.Resources.minus_section;
            input2.Save(Path.Combine(AnkiDirs.SOHA, AnkiFile.MINUS_SECTION_JPG));

            input2 = Properties.Resources.plus_section;
            input2.Save(Path.Combine(AnkiDirs.SOHA, AnkiFile.PLUS_SECTION_JPG));

            input2 = Properties.Resources.hidden;
            input2.Save(Path.Combine(AnkiDirs.SOHA, AnkiFile.HIDDEN_JPG));

            input2 = Properties.Resources.external;
            input2.Save(Path.Combine(AnkiDirs.SOHA, AnkiFile.EXTERNAL_PNG));
            #endregion

            #region Collins folder
            input = Properties.Resources.collins_common;
            File.WriteAllText(Path.Combine(AnkiDirs.LACVIET, AnkiFile.COLLINS_CSS), input);

            input2 = Properties.Resources.icons_right;
            input2.Save(Path.Combine(AnkiDirs.LACVIET, AnkiFile.ICONS_RIGHT_PNG));
            #endregion

            #region LacViet folder
            input = Properties.Resources.home;
            File.WriteAllText(Path.Combine(AnkiDirs.COLLINS, AnkiFile.HOME_CSS), input);

            input2 = Properties.Resources.Icon_6_7;
            input2.Save(Path.Combine(AnkiDirs.COLLINS, AnkiFile.ICON_6_7_PNG));

            input2 = Properties.Resources.Icon_7_4;
            input2.Save(Path.Combine(AnkiDirs.COLLINS, AnkiFile.ICON_7_4_PNG));
            #endregion

            #region Cambridge folder
            input = Properties.Resources.common;
            File.WriteAllText(Path.Combine(AnkiDirs.CAMBRIDGE, AnkiFile.COMMON_CSS), input);

            input2 = Properties.Resources.star;
            input2.Save(Path.Combine(AnkiDirs.CAMBRIDGE, AnkiFile.STAR_PNG));
            #endregion

            #region Anki images & Note Type
            input2 = Properties.Resources.anki;
            input2.Save(Path.Combine(AnkiDirs.IMAGE, AnkiFile.ANKI_PNG));

            byte[] input3 = Properties.Resources._en_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.EN_SINGLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._en_multiformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.EN_MULTIPLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._fr_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.FR_SINGLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._fr_multiformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.FR_MULTIPLE_FORM_ABCDEFGHLONGLEE123), input3);

            input3 = Properties.Resources._vn_singleformABCDEFGHLONGLEE123;
            File.WriteAllBytes(Path.Combine(AnkiDirs.ANKI_FLASHCARDS, AnkiFile.VN_SINGLE_FORM_ABCDEFGHLONGLEE123), input3);
            #endregion
        }

        private void InitializeDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    DeleteDirectory(path);
                }
                catch (IOException e)
                {
                    MessageBox.Show(MsgBoxProps.ERROR_OCCUR + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace, MsgBoxProps.ERROR);
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

        private void cbxLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
