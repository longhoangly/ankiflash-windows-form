using System;
using System.IO;
using System.Text.RegularExpressions;
using CsQuery;
using FlashcardsGenerator.Source;
using FlashcardsGenerator.Source.Dictionaries;

namespace FlashcardsGenerator
{
    class FrenchFlashcards
    {
        private CQ CollinsDom = string.Empty;

        private CQ LacVietDom = string.Empty;

        private string Wrd = string.Empty;

        private string WordType = string.Empty;

        private string Phonetic = string.Empty;

        private string CollinsExample = string.Empty;

        private string Pron = string.Empty;

        private string CollinsMeaning = string.Empty;

        private string LacVietMeaning = string.Empty;

        private string Thumb = string.Empty;

        private string Img = string.Empty;

        private string AnkiCard = string.Empty;

        private string CopyRight = string.Empty;

        private char Tag;

        private Collins collins = new Collins();

        string colTitle = string.Empty;

        private LacViet lacViet = new LacViet();

        string lacTitle = string.Empty;

        public string GenerateFrenchFlashCards(string word, string proxy, string language)
        {
            if (language.Equals(Languages.FR2EN))
            {
                CollinsDom = collins.GetDom(word, proxy);
                if (CollinsDom == null)
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else
                {
                    colTitle = collins.GetTitle(CollinsDom);
                    if (colTitle.Contains(Dictionary.COLLINS_SPELLING_WRONG))
                    {
                        return GeneratingStatus.SPELLING_WRONG + word + Environment.NewLine;
                    }

                    Wrd = collins.GetWord(CollinsDom);
                    if (Wrd.Equals(string.Empty))
                    {
                        return GeneratingStatus.GENERATING_FAILED + word + Environment.NewLine;
                    }
                }

                CollinsMeaning = collins.GetMeaning(CollinsDom);
            }
            else
            {
                CollinsDom = collins.GetDom(word, proxy);
                LacVietDom = lacViet.GetDom(word, proxy, language);
                if (CollinsDom == null || LacVietDom == null)
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else
                {
                    colTitle = collins.GetTitle(CollinsDom);
                    lacTitle = lacViet.GetTitle(LacVietDom);
                    if (colTitle.Contains(Dictionary.COLLINS_SPELLING_WRONG) ||
                        lacTitle.Contains(Dictionary.LACVIET_SPELLING_WRONG))
                    {
                        return GeneratingStatus.SPELLING_WRONG + word + Environment.NewLine;
                    }

                    Wrd = collins.GetWord(CollinsDom);
                    string lacWord = lacViet.GetWord(LacVietDom);
                    if (Wrd.Equals(string.Empty) || lacWord.Equals(string.Empty))
                    {
                        return GeneratingStatus.GENERATING_FAILED + word + Environment.NewLine;
                    }
                }

                CollinsMeaning = collins.GetMeaning(CollinsDom);
                LacVietMeaning = lacViet.GetMeaning(LacVietDom, Wrd);
            }

            WordType = collins.GetWordType(CollinsDom);
            Phonetic = collins.GetPhonetic(CollinsDom);
            CollinsExample = collins.GetExamples(CollinsDom, Wrd);
            Pron = collins.GetPronunciation(CollinsDom, proxy, "a[class*='audio_play_button']");
            Thumb = collins.GetImages(CollinsDom, Wrd, proxy, string.Empty, string.Empty);
            Img = Thumb;
            Tag = Wrd[0];

            CopyRight = Dictionary.COPYRIGHT_FR2VI;
            if (language.Equals(Languages.FR2EN))
            {
                CopyRight = Dictionary.COPYRIGHT_FR;
            }
            else if (language.Equals(Languages.FR2VI))
            {
                CopyRight = Dictionary.COPYRIGHT_VI;
            }

            AnkiCard = Wrd + Constant.TAB + WordType + Constant.TAB + Phonetic + Constant.TAB + CollinsExample + Constant.TAB + Pron + Constant.TAB + Thumb + Constant.TAB + Img + Constant.TAB + Constant.MEANING + Constant.TAB + CopyRight + Constant.TAB + Tag + Environment.NewLine;
            if (language.Equals(Languages.FR2EN))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, CollinsMeaning);
            }
            else if (language.Equals(Languages.FR2VI))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, LacVietMeaning);
            }
            else if (language.Equals(Languages.FR2EN_VI))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, CollinsMeaning + Constant.TAB + LacVietMeaning);
            }
            else if (language.Equals(Languages.FR2VI_EN))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, LacVietMeaning + Constant.TAB + CollinsMeaning);
            }

            return AnkiCard;
        }
    }
}
