using System;
using System.IO;
using System.Text.RegularExpressions;
using CsQuery;
using FlashcardsGenerator.Source;
using FlashcardsGenerator.Source.Dictionaries;

namespace FlashcardsGenerator
{
    class VietnameseFlashcards
    {
        private CQ LacVietDom { get; set; }

        private string Wrd { get; set; }

        private string Phonetic { get; set; }

        private string LacVietExample { get; set; }

        private string Pron { get; set; }

        private string LacVietMeaning { get; set; }

        private string Thumb { get; set; }

        private string Img { get; set; }

        private string AnkiCard { get; set; }

        private string CopyRight { get; set; }

        private char Tag;

        private LacViet lacViet = new LacViet();

        private string lacResult = string.Empty;

        public string GeneratevietnameseFlashCards(string word, string proxy, string language)
        {
            LacVietDom = lacViet.GetDom(word, proxy, language);
            if (LacVietDom == null)
            {
                return GeneratingStatus.CONNECTION_FAILED;
            }
            else
            {
                lacResult = lacViet.GetDomResult(LacVietDom);
                if (lacResult.Contains(Dictionary.LACVIET_SPELLING_WRONG))
                {
                    return GeneratingStatus.SPELLING_WRONG + word + Environment.NewLine;
                }

                Wrd = lacViet.GetWord(LacVietDom);
                if (Wrd.Equals(string.Empty))
                {
                    return GeneratingStatus.GENERATING_FAILED + word + Environment.NewLine;
                }
            }

            LacVietMeaning = lacViet.GetMeaning(LacVietDom, Wrd);
            Phonetic = lacViet.GetPhonetic(LacVietDom);
            LacVietExample = lacViet.GetExamples(LacVietDom, Wrd);
            Pron = lacViet.GetPronunciation(LacVietDom, proxy, "embed");
            Thumb = lacViet.GetImages(LacVietDom, Wrd, proxy, string.Empty, string.Empty);
            Img = Thumb;
            Tag = Wrd[0];

            CopyRight = Dictionary.COPYRIGHT_VI;
            AnkiCard = Wrd + Constant.TAB + Phonetic + Constant.TAB + LacVietExample + Constant.TAB + Pron + Constant.TAB + Thumb + Constant.TAB + Img + Constant.TAB + LacVietMeaning + Constant.TAB + CopyRight + Constant.TAB + Tag + Environment.NewLine;

            return AnkiCard;
        }
    }
}
