using System;
using System.Windows.Forms;

namespace FlashcardsGeneratorApplication
{
    internal class FlashcardsGenerator
    {
        public const string enToEng = "[en] English";
        public const string enToViet = "[en] Vietnamese";
        public const string enToEngViet = "[en] English & Vietnamese";
        public const string enToVietEng = "[en] Vietnamese & English";
        public const string frToViet = "[fr] Vietnamese";
        public const string frToEng = "[fr] English";
        public const string frToEngViet = "[fr] English & Vietnamese";
        public const string frToVietEng = "[fr] Vietnamese & English";
        public const string vnToEng = "[vn] English";
        public const string vnToFren = "[vn] French";

        public const string SpelNotOk = "Spelling Failed";
        public const string GenOk = "OK";
        public const string ConNotOk = "Connection Failed";
        public const string GenNotOk = "Generating Failed";

        private EnglishFlashcards _englishFlashcards = new EnglishFlashcards();
        private FrenchFlashcards _frenchFlashcards = new FrenchFlashcards();
        private VietnameseFlashcards _vietnameseFlashcards = new VietnameseFlashcards();
        private string _ankiCard;

        public string GenerateFlashCards(string word, string proxyStr, string language)
        {
            Console.WriteLine("Generate Card for Word: " + word);

            if (language.Contains("[en]"))
                _ankiCard = _englishFlashcards.GenerateEnglishFlashCards(word, proxyStr, language);
            else if (language.Contains("[vn]"))
                _ankiCard = _vietnameseFlashcards.GenerateFrenchFlashCards(word, proxyStr, language);
            else if (language.Contains("[fr]"))
                _ankiCard = _frenchFlashcards.GenerateFrenchFlashCards(word, proxyStr, language);

            return _ankiCard;
        }

        public string MonitorTextCopied(KeyEventArgs e)
        {
            bool ctrlC = e.Modifiers == Keys.Control && e.KeyCode == Keys.C;
            bool ctrlIns = e.Modifiers == Keys.Control && e.KeyCode == Keys.Insert;

            if (ctrlC || ctrlIns)
            {
                Console.WriteLine(Clipboard.GetText(TextDataFormat.Text));
                return Clipboard.GetText(TextDataFormat.Text);
            }

            return null;
        }

    }
}
