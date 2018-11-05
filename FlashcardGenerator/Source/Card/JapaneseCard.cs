using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardGenerator.Source.DictionaryModel;
using FlashcardGenerator.Utility;

namespace FlashcardGenerator.Source.Card
{
    public class JapaneseCard : Card
    {
        public string Thumb { get; set; }

        private readonly JishoDictionary JishoDict = new JishoDictionary();

        public override string GenerateFlashCard(string word, string proxy, string language)
        {
            Word = word;
            if (!JishoDict.IsConnectionEstablished(word, proxy, language))
            {
                return GeneratingStatus.CONNECTION_FAILED;
            }
            else if (!JishoDict.IsWordingCorrect())
            {
                return $"{Word}{GeneratingStatus.WORD_NOT_FOUND}{Environment.NewLine}";
            }

            Meaning = JishoDict.GetMeaning();
            CopyRight = string.Format(DictConst.COPYRIGHT, JishoDict.GetDictionaryName());

            Phonetic = JishoDict.GetPhonetic();
            Example = JishoDict.GetExample();
            Pron = JishoDict.GetPron("embed");
            Image = Thumb = JishoDict.GetImage(string.Empty, string.Empty);
            Tag = JishoDict.GetTag();

            return CardContent = Word + ConstVars.TAB + Phonetic + ConstVars.TAB + Example + ConstVars.TAB +
                Pron + ConstVars.TAB + Thumb + ConstVars.TAB + Image + ConstVars.TAB +
                Meaning + ConstVars.TAB + CopyRight + ConstVars.TAB + Tag + Environment.NewLine;
        }
    }
}
