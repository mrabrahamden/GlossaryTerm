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

        public string ToString() //можно здесь override написать? (Аня Н.)
        {
            return Word + " -- " + Description;
        }
    }
}