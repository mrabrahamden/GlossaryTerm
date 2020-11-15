using System;
using System.Collections.Generic;
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
        public bool ReadyForFillGame;
        public List<DescriptionWord> DescriptionWordsAndSplittersList;

        public SimpleTerm(string word, string descr)
        {
            Word = word;
            Description = descr;
            ReadyForFillGame = false;
            DescriptionWordsAndSplittersList=new List<DescriptionWord>();
            FillingListsForFillGame();
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
            Regex regexForWordAndSplit=new Regex(@"(\w)+((\W)+)?");
            Regex regexForWord=new Regex(@"(\w)+");
            Regex regexForSplit=new Regex(@"(\W)+");
            var matches = regexForWordAndSplit.Matches(Description);
            foreach (var wordAndSplitter in matches)
            {
                var stringword = wordAndSplitter.ToString();
                var word = regexForWord.Match(stringword).ToString();
                var splitter = regexForSplit.Match(stringword).ToString();
                DescriptionWordsAndSplittersList.Add(new DescriptionWord(word,false,false));
                DescriptionWordsAndSplittersList.Add(new DescriptionWord(splitter,false,true));
            }
        }
    }
    [Serializable]
    public class DescriptionWord
    {
        //private List<string> DefNotKeyWords = new List<string>()
        //    {"это", "так", "которое", "которая", "который", "которые"};
        public string Word;
        public bool IsKeyWord;
        public bool IsSplitter;
        public DescriptionWord(string word, bool iskeyword,bool issplitter)
        {
            Word = word;
            IsKeyWord = iskeyword;
           // if (DefNotKeyWords.Contains(Word))
          //  {

          //  }
            IsSplitter = issplitter;
            if (IsSplitter)
            {
                IsKeyWord = false;
            }
        }
    }
}