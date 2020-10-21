﻿using System;
using System.Collections.Generic;


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