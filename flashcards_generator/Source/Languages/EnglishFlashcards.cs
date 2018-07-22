using System;
using CsQuery;
using FlashcardsGenerator.Source;
using FlashcardsGenerator.Source.Dictionaries;

namespace FlashcardsGenerator
{
    public class EnglishFlashcards
    {
        private CQ OxfDom { get; set; }

        private CQ CamDom { get; set; }

        private CQ LacVietDom { get; set; }

        private string Wrd { get; set; }

        private string WordType { get; set; }

        private string Phonetic { get; set; }

        private string Example { get; set; }

        private string ProUk { get; set; }

        private string ProUs { get; set; }

        private string OxfMeaning { get; set; }

        private string LacVietMeaning { get; set; }

        private string CamMeaning { get; set; }

        private string Thumb { get; set; }

        private string Img { get; set; }

        private string AnkiCard { get; set; }

        private string CopyRight { get; set; }

        private char Tag { get; set; }

        private Oxford oxford = new Oxford();

        private string oxfTitle = string.Empty;

        private Cambridge cambridge = new Cambridge();

        private string camTitle = string.Empty;

        private LacViet lacViet = new LacViet();

        private string lacResult = string.Empty;

        public string GenerateEnglishFlashCards(string word, string proxy, string language)
        {
            if (language.Equals(Languages.EN2EN))
            {
                OxfDom = oxford.GetDom(word, proxy);
                if (OxfDom == null)
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else
                {
                    oxfTitle = oxford.GetTitle(OxfDom);
                    if (oxfTitle.Contains(Dictionary.OXFORD_SPELLING_WRONG_1) ||
                        oxfTitle.Contains(Dictionary.OXFORD_SPELLING_WRONG_2))
                    {
                        return GeneratingStatus.SPELLING_WRONG + word + Environment.NewLine;
                    }

                    Wrd = oxford.GetWord(OxfDom);
                    if (Wrd.Equals(string.Empty))
                    {
                        return GeneratingStatus.GENERATING_FAILED + word + Environment.NewLine;
                    }
                }

                OxfMeaning = oxford.GetMeaning(OxfDom);
            }
            else if (language.Equals(Languages.EN2CH))
            {
                OxfDom = oxford.GetDom(word, proxy);
                CamDom = cambridge.GetDom(word, proxy);
                if (OxfDom == null || CamDom == null)
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else
                {
                    oxfTitle = oxford.GetTitle(OxfDom);
                    camTitle = cambridge.GetTitle(CamDom);
                    if (oxfTitle.Contains(Dictionary.OXFORD_SPELLING_WRONG_1) ||
                        oxfTitle.Contains(Dictionary.OXFORD_SPELLING_WRONG_2) ||
                        camTitle.Contains(Dictionary.CAMBRIDGE_SPELLING_WRONG))
                    {
                        return GeneratingStatus.SPELLING_WRONG + word + Environment.NewLine;
                    }

                    Wrd = oxford.GetWord(OxfDom);
                    string camWord = cambridge.GetWord(CamDom);
                    if (Wrd.Equals(string.Empty) || camWord.Equals(string.Empty))
                    {
                        return GeneratingStatus.GENERATING_FAILED + word + Environment.NewLine;
                    }
                }

                // OxfMeaning = oxford.GetMeaning(OxfDom);
                CamMeaning = cambridge.GetMeaning(CamDom);
            }
            else
            {
                OxfDom = oxford.GetDom(word, proxy);
                LacVietDom = lacViet.GetDom(word, proxy, language);
                if (OxfDom == null || LacVietDom == null)
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else
                {
                    oxfTitle = oxford.GetTitle(OxfDom);
                    lacResult = lacViet.GetDomResult(LacVietDom);
                    if (oxfTitle.Contains(Dictionary.OXFORD_SPELLING_WRONG_1) ||
                        oxfTitle.Contains(Dictionary.OXFORD_SPELLING_WRONG_2) ||
                        lacResult.Contains(Dictionary.LACVIET_SPELLING_WRONG))
                    {
                        return GeneratingStatus.SPELLING_WRONG + word + Environment.NewLine;
                    }

                    Wrd = oxford.GetWord(OxfDom);
                    if (Wrd.Equals(string.Empty))
                    {
                        return GeneratingStatus.GENERATING_FAILED + word + Environment.NewLine;
                    }
                }

                OxfMeaning = oxford.GetMeaning(OxfDom);
                LacVietMeaning = lacViet.GetMeaning(LacVietDom, Wrd);
            }

            WordType = oxford.GetWordType(OxfDom);
            Phonetic = oxford.GetPhonetic(OxfDom);
            Example = oxford.GetExamples(OxfDom, Wrd);
            ProUk = oxford.GetPronunciation(OxfDom, proxy, "div.pron-uk");
            ProUs = oxford.GetPronunciation(OxfDom, proxy, "div.pron-us");
            Thumb = oxford.GetImages(OxfDom, Wrd, proxy, "img.thumb", "src");
            Img = oxford.GetImages(OxfDom, Wrd, proxy, "a[class=topic]", "href");
            Tag = Wrd[0];

            CopyRight = Dictionary.COPYRIGHT_EN2VI;
            if (language.Equals(Languages.EN2EN))
            {
                CopyRight = Dictionary.COPYRIGHT_EN2EN;
            }
            else if (language.Equals(Languages.EN2CH))
            {
                CopyRight = Dictionary.COPYRIGHT_EN2CH;
            }

            AnkiCard = Wrd + Constant.TAB + WordType + Constant.TAB + Phonetic + Constant.TAB + Example + Constant.TAB + ProUk + Constant.TAB + ProUs + Constant.TAB + Thumb + Constant.TAB + Img + Constant.TAB + Constant.MEANING + Constant.TAB + CopyRight + Constant.TAB + Tag + Environment.NewLine;
            if (language.Equals(Languages.EN2EN))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, OxfMeaning);
            }
            else if (language.Equals(Languages.EN2VI))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, LacVietMeaning);
            }
            else if (language.Equals(Languages.EN2CH))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, CamMeaning);
            }
            else if (language.Equals(Languages.EN2EN_VI))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, OxfMeaning + Constant.TAB + LacVietMeaning);
            }
            else if (language.Equals(Languages.EN2VI_EN))
            {
                AnkiCard = AnkiCard.Replace(Constant.MEANING, LacVietMeaning + Constant.TAB + OxfMeaning);
            }

            return AnkiCard;
        }
    }
}
