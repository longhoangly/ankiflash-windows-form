using System;

namespace FlashcardsGeneratorApplication
{
    internal class FlashcardsGenerator
    {
        public const string EN2EN = "[EN] English";
        public const string EN2VI = "[EN] Vietnamese";
        public const string EN2CH = "[EN] Chinese";
        public const string EN2EN_VI = "[EN] English & Vietnamese";
        public const string EN2VI_EN = "[EN] Vietnamese & English";
        public const string FR2VI = "[FR] Vietnamese";
        public const string FR2EN = "[FR] English";
        public const string FR2EN_VI = "[FR] English & Vietnamese";
        public const string FR2VI_EN = "[FR] Vietnamese & English";
        public const string VN2EN = "[VN] English";
        public const string VN2FR = "[VN] French";

        public const string CONNECTION_FAILED = "Connection Failed"; 
        public const string SPELLING_FAILED = "Spelling Failed";
        public const string GENERATING_SUCCESS = "Generating Succeed";
        public const string GENERATING_FAILED = "Generating Failed";

        private EnglishFlashcards _englishFlashcards = new EnglishFlashcards();
        private FrenchFlashcards _frenchFlashcards = new FrenchFlashcards();
        private VietnameseFlashcards _vietnameseFlashcards = new VietnameseFlashcards();
        private string _ankiCard;

        public string GenerateFlashCards(string word, string proxyStr, string language)
        {
            Console.WriteLine("Generate Card For The Word: " + word);

            if (language.Contains("[EN]"))
                _ankiCard = _englishFlashcards.GenerateEnglishFlashCards(word, proxyStr, language);
            else if (language.Contains("[VN]"))
                _ankiCard = _vietnameseFlashcards.GeneratevietnameseFlashCards(word, proxyStr, language);
            else if (language.Contains("[FR]"))
                _ankiCard = _frenchFlashcards.GenerateFrenchFlashCards(word, proxyStr, language);

            return _ankiCard;
        }
    }
}
