using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace TermLib
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
        public bool[] KeyWordBools;
        public bool ReadyForFillGame;
        public List<string> DescriptionWordsList;
        public List<string> DescriptionWordsAndSplittersList;

        public SimpleTerm(string word, string descr)
        {
            Word = word;
            Description = descr;
            ReadyForFillGame = false;
            DescriptionWordsAndSplittersList=new List<string>();
            DescriptionWordsList = FillingDescriptionWordsList();
            KeyWordBools=new bool[DescriptionWordsList.Count];
        }

        public override string ToString() 
        {
            if (Word.Length > 0)
            {
                Word = Word.Substring(0, 1).ToUpper() + Word.Substring(1, Word.Length - 1).ToLower();
            }
            return Word + " -- " + Description;
        }

        private List<string> FillingDescriptionWordsList()
        {
            Regex regexForWordAndSplit=new Regex(@"(\w)+(\W)+");
            Regex regexForWord=new Regex(@"(\w)+");
            Regex regexForSplit=new Regex(@"(\W)+");
            var matches = regexForWordAndSplit.Matches(Description);
            var result = new List<string>();
            foreach (var wordAndSplitter in matches)
            {
                var stringword = wordAndSplitter.ToString();
                var word = regexForWord.Match(stringword).ToString();
                var splitter = regexForSplit.Match(stringword).ToString();
                DescriptionWordsList.Add(word);
                result.Add(word);
                result.Add(splitter);
            }
            return result;
        }

    }
}