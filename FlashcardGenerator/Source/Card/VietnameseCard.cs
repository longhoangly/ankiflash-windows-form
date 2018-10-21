using FlashcardGenerator.Source.DictionaryModel;
using FlashcardGenerator.Utility;
using System;
using System.Linq;

namespace FlashcardGenerator.Source.Card
{
    public class VietnameseCard : Card
    {
        public string Thumb { get; set; }

        private readonly LacVietDictionary LacVietDict = new LacVietDictionary();

        public override string GenerateFlashCard(string word, string proxy, string language)
        {
            Word = word;
            if (!LacVietDict.IsConnectionEstablished(word, proxy, language))
            {
                return GeneratingStatus.CONNECTION_FAILED;
            }
            else if (!LacVietDict.IsWordingCorrect())
            {
                return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
            }

            Meaning = LacVietDict.GetMeaning();
            CopyRight = string.Format(DictConst.COPYRIGHT, LacVietDict.GetDictionaryName());

            Phonetic = LacVietDict.GetPhonetic();
            Example = LacVietDict.GetExample();
            Pron = LacVietDict.GetPron("embed");
            Image = Thumb = LacVietDict.GetImage(string.Empty, string.Empty);
            Tag = LacVietDict.GetTag();

            return CardContent = Word + ConstVars.TAB + Phonetic + ConstVars.TAB + Example + ConstVars.TAB + 
                Pron + ConstVars.TAB + Thumb + ConstVars.TAB + Image + ConstVars.TAB + 
                Meaning + ConstVars.TAB + CopyRight + ConstVars.TAB + Tag + Environment.NewLine;
        }
    }
}
