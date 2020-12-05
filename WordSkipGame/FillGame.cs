using System.Collections.Generic;
using TermLib;

namespace FillGameLib
{
    public class FillGame
    {
        public List<SimpleTerm> List;
        public int Lvl;
        public bool FixedLength;
        public bool TrainingMode;
        public int Count;

        public FillGame(List<SimpleTerm> list, int lvl,int count, bool fixLength, bool trainingMode)
        {
            List = list;
            Lvl = lvl;
            FixedLength = fixLength;
            TrainingMode = trainingMode;
            Count = count;
        }
    }
}
