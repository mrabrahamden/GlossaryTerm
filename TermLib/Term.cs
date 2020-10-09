using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public string ToString()
        {
            return Word + " -- " + Description;
        }
    }
}