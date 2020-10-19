using System;


namespace TermLib
{
    [Serializable]
    public abstract class Term
    {
        public string Word;
    }
    [Serializable]
    public class SimpleTerm : Term
    {
        public string Description;

        public SimpleTerm(string word, string descr)
        {
            Word = word;
            Description = descr;
        }

        public override string ToString() 
        {
            if (Word.Length > 0)
            {
                Word = Word.Substring(0, 1).ToUpper() + Word.Substring(1, Word.Length - 1).ToLower();
            }
            return Word + " -- " + Description;
        }
    }
}