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

        public FillGame(List<SimpleTerm> list, int lvl,int count, bool fixlength, bool trainingmode)
        {
            List = list;
            Lvl = lvl;
            FixedLength = fixlength;
            TrainingMode = trainingmode;
            Count = count;
        }
    }
}
