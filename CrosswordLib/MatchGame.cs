using System;
using System.Collections.Generic;
using SerializerLib;

namespace GameLib
{
    public class MatchGame
    {
        public List<SimpleTerm> TermList;
        public int NumOfTerms;
        public bool TrainingMode;
        public bool IsReady;

        public MatchGame(List<SimpleTerm> list, int numOfTerms, bool trainingMode)
        {
            TermList = new List<SimpleTerm>(list);
            NumOfTerms = numOfTerms;
            TrainingMode = trainingMode;
            GetRandomListOfTerms();
            IsReady = false;
        }

        private void GetRandomListOfTerms()
        {
            List<SimpleTerm> resultList = new List<SimpleTerm>();
            Random random = new Random((int)DateTime.Now.Millisecond);
            while (TermList.Count > NumOfTerms)
            {
                int numNext = random.Next() % TermList.Count;
                TermList.RemoveAt(numNext);
            }
        }
    }
}
