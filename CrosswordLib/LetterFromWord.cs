using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerializerLib;

namespace GameLib
{
    public class LetterFromWord
    {
        public char Letter;
        public SimpleTerm Term;

        public LetterFromWord(char letter, SimpleTerm term)
        {
            Letter = letter;
            Term = term;
        }
    }
}
