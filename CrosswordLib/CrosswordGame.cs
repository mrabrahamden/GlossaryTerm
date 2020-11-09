using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TermLib;

namespace CrosswordLib
{
    public class CrosswordGame
    {
        public List<SimpleTerm> List;
        public SimpleTerm[] CrossWordTerms;
        private SimpleTerm mainWord;
        public bool IsReady;
        public char[,] CrosswordMatrix;
        Random random=new Random(DateTime.Now.Millisecond);
        private char[,] PreparingMatrix;
        private int verticalSize;
        private int horizontalSize;
        private int halfHorizontalSize;

        public CrosswordGame(List<SimpleTerm> list)
        {
            List = list;
            IsReady = false;
            PrepareGame();
        }
        private void PrepareGame()
        {
            var list = new List<SimpleTerm>();
            ChooseRandomMainWord();
            if (mainWord != null)
            {
                verticalSize = mainWord.Word.Length;
                horizontalSize = verticalSize * 2 + 1;
                CrossWordTerms = new SimpleTerm[verticalSize+1];
                CrossWordTerms[0] = mainWord;
                halfHorizontalSize = verticalSize + 1;
                PreparingMatrix = new char[verticalSize,(verticalSize*2)+1];
                for (int i = 0; i < verticalSize; i++)
                {
                    PreparingMatrix[i, halfHorizontalSize] = mainWord.Word[i];
                }

                int count = 0;
                for (int i = 0; i < verticalSize; i++)
                {
                    int rand = GetRandom(verticalSize)+1;
                    if (CrossWordTerms[rand] == null)
                    {
                        TryAddWordToCrossword(rand);
                        count++;
                    }
                } 
                PerformMatrix();
                IsReady = true;
            }
        }

        private void ChooseRandomMainWord()
        {
            List<SimpleTerm> maxLengthWords=new List<SimpleTerm>();
            maxLengthWords = (from t in List where (t.Word.Length > 6) orderby t.Word.Length select t).ToList();
            if (maxLengthWords.Count > 0)
            {
                mainWord = maxLengthWords[GetRandom(maxLengthWords.Count)];
                List.Remove(mainWord);
            }
        }

        private int GetRandom(int mod)
        {
            return random.Next() % mod;
        }

        private void TryAddWordToCrossword(int i)
        {
            SimpleTerm term;
            char letter = mainWord.Word[i-1];
            var listWords = GetWordsByLetter(letter);
            if (listWords.Count > 0)
            {
                term = listWords[GetRandom(listWords.Count)];
                CrossWordTerms[i] = term;
                FillMatrixByWord(term,i);
                List.Remove(term);
            }
        }

        private List<SimpleTerm> GetWordsByLetter(char letter)
        {
            return (from t in List where t.Word.Contains(char.ToLower(letter)) && t != mainWord select t).ToList();
        }

        private void FillMatrixByWord(SimpleTerm term,int i)
        {
            List<int> indexesOfLetters=new List<int>();
            for (int ind = 0; ind < term.Word.Length; ind++)
            {
                if (char.ToLower(term.Word[ind]) == char.ToLower(mainWord.Word[i - 1]))
                {
                    indexesOfLetters.Add(ind);
                }
            }

            int index=-1;
            if (indexesOfLetters.Count > 0)
            {
                index = indexesOfLetters[GetRandom(indexesOfLetters.Count)];
            }

            if (index >= 0)
            {
                int length = term.Word.Length;
                for (int k = 0; k < length; k++)
                {
                    PreparingMatrix[i - 1, k + halfHorizontalSize - index] = term.Word[k];
                }
            }
        }
        private void PerformMatrix()
        {

        }

        public void GetMatrixOnConsole()
        {
            for(int i=0;i<verticalSize;i++)
            {
                for (int j = 0; j < horizontalSize; j++)
                {
                    Console.Write(PreparingMatrix[i,j]+" ");
                }
                Console.WriteLine();
            }
        }
    }
}
