using FlashcardGenerator.Source.DictionaryModel;
using FlashcardGenerator.Utility;
using System;

namespace FlashcardGenerator.Source.Card
{
    public class FrenchCard : Card
    {
        public string Thumb { get; set; }

        private readonly CollinsDictionary CollinsDict = new CollinsDictionary();

        private readonly LacVietDictionary LacVietDict = new LacVietDictionary();

        public override string GenerateFlashCard(string word, string proxy, string language)
        {
            Word = word;
            if (language.Equals(CardOptions.FR2EN))
            {
                if (!CollinsDict.IsConnectionEstablished(word, proxy, language))
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else if (CollinsDict.IsWordingCorrect())
                {
                    return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
                }

                Meaning = CollinsDict.GetMeaning();
                CopyRight = string.Format(DictConst.COPYRIGHT, CollinsDict.GetDictionaryName());
            }
            else
            {
                if (!CollinsDict.IsConnectionEstablished(word, proxy, language) ||
                    !LacVietDict.IsConnectionEstablished(word, proxy, language))
                {
                    return GeneratingStatus.CONNECTION_FAILED;
                }
                else if (!CollinsDict.IsWordingCorrect() ||
                         !LacVietDict.IsWordingCorrect())
                {
                    return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
                }

                if (language.Equals(CardOptions.FR2VI))
                {
                    Meaning = LacVietDict.GetMeaning();
                }
                else if (language.Equals(CardOptions.FR2EN_VI))
                {
                    Meaning = CollinsDict.GetMeaning() + ConstVars.TAB + LacVietDict.GetMeaning();
                }
                else if (language.Equals(CardOptions.FR2VI_EN))
                {
                    Meaning = LacVietDict.GetMeaning() + ConstVars.TAB + CollinsDict.GetMeaning();
                }

                CopyRight = string.Format(DictConst.COPYRIGHT, string.Join(", and", CollinsDict.GetDictionaryName(), LacVietDict.GetDictionaryName()));
            }

            WordType = CollinsDict.GetWordType();
            Phonetic = CollinsDict.GetPhonetic();
            Example = CollinsDict.GetExample();
            Pron = CollinsDict.GetPron("a[class*='audio_play_button']");
            Image = Thumb = CollinsDict.GetImage(string.Empty, string.Empty);
            Tag = CollinsDict.GetTag();

            return CardContent = Word + ConstVars.TAB + WordType + ConstVars.TAB + Phonetic + ConstVars.TAB + Example + ConstVars.TAB +
                    Pron + ConstVars.TAB + Thumb + ConstVars.TAB + Image + ConstVars.TAB +
                    Meaning + ConstVars.TAB + CopyRight + ConstVars.TAB + Tag + Environment.NewLine;
        }
    }
}
