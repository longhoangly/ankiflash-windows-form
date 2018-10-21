using FlashcardGenerator.Source.Card;
using FlashcardGenerator.Utility;
using System;

namespace FlashcardGenerator.Source
{
    public class CardGenerator
    {
        private readonly EnglishCard EnglishCard = new EnglishCard();

        private readonly FrenchCard FrenchCard = new FrenchCard();

        private readonly VietnameseCard VietnameseCard = new VietnameseCard();

        private readonly JapaneseCard JapaneseCard = new JapaneseCard();

        public string GenerateFlashCard(string word, string proxyStr, string language)
        {
            Console.WriteLine($"Generate card for the word ====>>>> {word}");
            string cardContent = string.Empty;

            if (language.StartsWith(CardOptions.EN_SRC))
            {
                cardContent = EnglishCard.GenerateFlashCard(word, proxyStr, language);
            }
            else if (language.StartsWith(CardOptions.VN_SRC))
            {
                cardContent = FrenchCard.GenerateFlashCard(word, proxyStr, language);
            }
            else if (language.StartsWith(CardOptions.FR_SRC))
            {
                cardContent = VietnameseCard.GenerateFlashCard(word, proxyStr, language);
            }
            else if (language.StartsWith(CardOptions.JP_SRC))
            {
                cardContent = JapaneseCard.GenerateFlashCard(word, proxyStr, language);
            }

            return cardContent;
        }
    }
}
