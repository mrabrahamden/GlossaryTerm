using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace SerializerLib
{
    public class SimpleTermEqualityComparer : IEqualityComparer<SimpleTerm>
    {
        public bool Equals(SimpleTerm t1, SimpleTerm t2)
        {
            if (t2 == null && t1 == null)
                return true;
            else if (t1 == null || t2 == null)
                return false;
            else if (t1.Word == t2.Word && t1.Description == t2.Description)
                return true;
            else return false;
        }

        public int GetHashCode(SimpleTerm obj)
        {
            int hCode = obj.ToString().GetHashCode();
            return hCode.GetHashCode();
        }
    }
    [Serializable]
    public abstract class Term
    {
        public string Word;
    }
    [Serializable]
    public class SimpleTerm : Term
    {
        public string Description;
        public bool ReadyForFillGame;
        public List<DescriptionWord> DescriptionWordsAndSplittersList;

        public SimpleTerm(string word, string descr)
        {
            var indexLastSymbol = word.Length - 1;
            indexLastSymbol = FindIndexLastSymbol(word, indexLastSymbol);
            if (indexLastSymbol > 0)
            {
                Word = word.Substring(0, indexLastSymbol + 1);
            }

            else Word = word;
            indexLastSymbol = descr.Length - 1;
            bool hasDot = indexLastSymbol > 0 && descr[indexLastSymbol] == '.';

            if (hasDot)
            {
                Description = descr.Substring(0, indexLastSymbol);
            }
            else
            {
                if (descr.Length > 0)
                {
                    indexLastSymbol = FindIndexLastSymbol(descr, indexLastSymbol);
                    Description = descr.Substring(0, indexLastSymbol + 1);
                }
                else Description = descr;
            }
            ReadyForFillGame = false;
            DescriptionWordsAndSplittersList = new List<DescriptionWord>();
            FillingListsForFillGame();
        }

        private static int FindIndexLastSymbol(string word, int indexLastSymbol)
        {
            for (int i = word.Length - 1; i >= 0; i--)
            {
                if (char.IsWhiteSpace(word[i]) || char.IsPunctuation(word[i]))
                {
                    indexLastSymbol--;
                }

                if (char.IsLetter(word[i])) break;
            }

            return indexLastSymbol;
        }

        public override string ToString()
        {
            if (Word.Length > 0)
            {
                Word = Word.Substring(0, 1).ToUpper() + Word.Substring(1, Word.Length - 1).ToLower();
            }
            return Word + " ⸺ " + Description;
        }

        public void FillingListsForFillGame()
        {
            DescriptionWordsAndSplittersList.Clear();
            Regex regexForWordAndSplit = new Regex(@"(\w)+((\W)+)?");
            Regex regexForWord = new Regex(@"(\w)+");
            Regex regexForSplit = new Regex(@"(\W)+");
            var matches = regexForWordAndSplit.Matches(Description);
            foreach (var wordAndSplitter in matches)
            {
                var stringWord = wordAndSplitter.ToString();
                var word = regexForWord.Match(stringWord).ToString();
                var splitter = regexForSplit.Match(stringWord).ToString();
                DescriptionWordsAndSplittersList.Add(new DescriptionWord(word, false, false));
                DescriptionWordsAndSplittersList.Add(new DescriptionWord(splitter, false, true));
            }
        }
    }
    [Serializable]
    public class DescriptionWord
    {
        public string Word;
        public bool IsKeyWord;
        public bool IsSplitter;
        public DescriptionWord(string word, bool isKeyword, bool isSplitter)
        {
            Word = word;
            IsKeyWord = isKeyword;
            IsSplitter = isSplitter;
            if (IsSplitter)
            {
                IsKeyWord = false;
            }
        }
    }
}