using CsQuery;

namespace FlashcardGenerator.Source.DictionaryModel
{
    public abstract class Dictionary
    {
        public CQ Dom { get; set; }

        protected string Word { get; set; }

        protected string Proxy { get; set; }

        protected string Language { get; set; }

        public abstract bool IsConnectionEstablished(string word, string proxy, string language);

        public abstract bool IsWordingCorrect();

        public abstract string GetWordType();

        public abstract string GetExample();

        public abstract string GetPhonetic();

        public abstract string GetImage(string selector, string attr);

        public abstract string GetPron(string selector);

        public abstract string GetMeaning();

        public char GetTag()
        {
            return Word[0];
        }

        public abstract string GetDictionaryName();
    }
}
