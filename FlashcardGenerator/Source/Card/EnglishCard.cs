using FlashcardGenerator.Source.DictionaryModel;
using FlashcardGenerator.Utility;
using System;

namespace FlashcardGenerator.Source.Card
{
    public class EnglishCard : Card
    {
        public string ProUk { get; set; }

        public string Thumb { get; set; }

        private readonly OxfordDictionary OxfordDict = new OxfordDictionary();

        private readonly CambridgeDictionary CambridgeDict = new CambridgeDictionary();

        private readonly LacVietDictionary LacVietDict = new LacVietDictionary();

        public override string GenerateFlashCard(string word, string proxy, string language)
        {
            Word = word;
            if (language.Equals(CardOptions.EN2EN))
            {
                if (!OxfordDict.IsConnectionEstablished(word, proxy, language))
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else if (!OxfordDict.IsWordingCorrect())
                {
                    return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
                }

                Meaning = OxfordDict.GetMeaning();
                CopyRight = string.Format(DictConst.COPYRIGHT, OxfordDict.GetDictionaryName());
            }
            else if (language.Equals(CardOptions.EN2CH))
            {
                if (!OxfordDict.IsConnectionEstablished(word, proxy, language) ||
                    !CambridgeDict.IsConnectionEstablished(word, proxy, language))
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else if (!OxfordDict.IsWordingCorrect() ||
                         !CambridgeDict.IsWordingCorrect())
                {
                    return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
                }

                Meaning = CambridgeDict.GetMeaning();
                CopyRight = string.Format(DictConst.COPYRIGHT, CambridgeDict.GetDictionaryName());
            }
            else
            {
                if (!OxfordDict.IsConnectionEstablished(word, proxy, language) ||
                    !LacVietDict.IsConnectionEstablished(word, proxy, language))
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else if (!OxfordDict.IsWordingCorrect() ||
                         !LacVietDict.IsWordingCorrect())
                {
                    return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
                }

                if (language.Equals(CardOptions.EN2VI))
                {
                    Meaning = LacVietDict.GetMeaning();
                }
                else if (language.Equals(CardOptions.EN2EN_VI))
                {
                    Meaning = OxfordDict.GetMeaning() + ConstVars.TAB + LacVietDict.GetMeaning();
                }
                else if (language.Equals(CardOptions.EN2VI_EN))
                {
                    Meaning = LacVietDict.GetMeaning() + ConstVars.TAB + OxfordDict.GetMeaning();
                }

                CopyRight = string.Format(DictConst.COPYRIGHT, string.Join(", and", OxfordDict.GetDictionaryName(), LacVietDict.GetDictionaryName()));
            }

            WordType = OxfordDict.GetWordType();
            Phonetic = OxfordDict.GetPhonetic();
            Example = OxfordDict.GetExample();
            Pron = OxfordDict.GetPron("div.pron-us");
            Image = OxfordDict.GetImage("a[class=topic]", "href");
            Tag = OxfordDict.GetTag();

            ProUk = OxfordDict.GetPron("div.pron-uk");
            Thumb = OxfordDict.GetImage("img.thumb", "src");

            return CardContent = Word + ConstVars.TAB + WordType + ConstVars.TAB + Phonetic + ConstVars.TAB + Example + ConstVars.TAB +
                    ProUk + ConstVars.TAB + Pron + ConstVars.TAB + Thumb + ConstVars.TAB + Image + ConstVars.TAB +
                    Meaning + ConstVars.TAB + CopyRight + ConstVars.TAB + Tag + Environment.NewLine;
        }
    }
}
