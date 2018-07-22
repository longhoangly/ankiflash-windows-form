using System;
using FlashcardsGenerator.Source;

namespace FlashcardsGenerator
{
    public class FlashcardsGenerator
    {
        private EnglishFlashcards englishFlashcards = new EnglishFlashcards();

        private FrenchFlashcards frenchFlashcards = new FrenchFlashcards();

        private VietnameseFlashcards vietnameseFlashcards = new VietnameseFlashcards();

        public string GenerateFlashCards(string word, string proxyStr, string language)
        {
            Console.WriteLine(Constant.GENERATE_CARD_FOR_THE_WORD + word);
            string ankiCard = string.Empty;

            if (language.StartsWith(Languages.EN_SRC))
            {
                ankiCard = englishFlashcards.GenerateEnglishFlashCards(word, proxyStr, language);
            }
            else if (language.StartsWith(Languages.VN_SRC))
            {
                ankiCard = vietnameseFlashcards.GeneratevietnameseFlashCards(word, proxyStr, language);
            }
            else if (language.StartsWith(Languages.FR_SRC))
            {
                ankiCard = frenchFlashcards.GenerateFrenchFlashCards(word, proxyStr, language);
            }

            return ankiCard;
        }
    }
}
