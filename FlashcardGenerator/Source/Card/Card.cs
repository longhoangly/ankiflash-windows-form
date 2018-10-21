using CsQuery;

namespace FlashcardGenerator.Source.Card
{
    public abstract class Card
    {
        public CQ Dom { get; set; }

        public string Word { get; set; }

        public string WordType { get; set; }

        public string Phonetic { get; set; }

        public string Example { get; set; }

        public string Pron { get; set; }

        public string Meaning { get; set; }

        public string Image { get; set; }

        public char Tag { get; set; }

        public string CopyRight { get; set; }

        public string CardContent { get; set; }

        public abstract string GenerateFlashCard(string word, string proxy, string language);
    }
}
