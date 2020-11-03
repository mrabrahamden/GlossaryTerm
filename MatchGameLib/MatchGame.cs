using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermLib;

namespace MatchGameLib
{
    public class MatchGame
    {
        public List<SimpleTerm> TermList;
        public int NumOfTerms;
        public bool TrainingMode;

        public MatchGame(List<SimpleTerm> list, int numOfTerms, bool trainingMode)
        {
            TermList = new List<SimpleTerm>(list);
            NumOfTerms = numOfTerms;
            TrainingMode = trainingMode;
            GetRandomListOfTerms();
        }

        private void GetRandomListOfTerms()
        {
            List<SimpleTerm> resultList=new List<SimpleTerm>();
            Random random = new Random((int) DateTime.Now.Millisecond);
            while (TermList.Count>NumOfTerms)
            {
                int numNext= random.Next() % TermList.Count;
                TermList.RemoveAt(numNext);
            }
        }

    }
}
