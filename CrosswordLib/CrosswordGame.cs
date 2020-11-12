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
        public LetterFromWord[,] CrosswordMatrix;
        Random random=new Random(DateTime.Now.Millisecond);
        private LetterFromWord[,] PreparingMatrix;
        private int verticalSize;
        private int horizontalSize;
        public int mainWordHorizontalIndex;
        private int maxLength=0;

        public CrosswordGame(List<SimpleTerm> list)
        {
            List = new List<SimpleTerm>(list);
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
                horizontalSize = maxLength * 2 + 1;
                CrossWordTerms = new SimpleTerm[verticalSize+1];
                CrossWordTerms[0] = mainWord;
                mainWordHorizontalIndex = maxLength + 1;
                PreparingMatrix = new LetterFromWord[verticalSize,horizontalSize];
                for (int i = 0; i < verticalSize; i++)
                {
                    PreparingMatrix[i, mainWordHorizontalIndex] = new LetterFromWord(mainWord.Word[i],mainWord);
                }

                int count = 0;
                while(count<verticalSize)
                {   
                    int rand = GetRandom(verticalSize)+1;
                    if (CrossWordTerms[rand] == null)
                    {
                        if(mainWord.Word[rand-1]!=' ')
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
                foreach (var term in maxLengthWords)
                {
                    int length = term.Word.Length;
                    if (length > maxLength)
                    {
                        maxLength = length;
                    }
                }
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
                    PreparingMatrix[i - 1, k + mainWordHorizontalIndex - index] = new LetterFromWord(term.Word[k],term);
                }
            }
        }

        private void PerformMatrix()
        {
            for (int i = 0; i < verticalSize; i++)
            {
                for (int j = 0; j < horizontalSize; j++)
                {
                    if(PreparingMatrix[i,j]!=null)
                        PreparingMatrix[i, j].Letter = char.ToUpper(PreparingMatrix[i, j].Letter);
                }
                Console.WriteLine();
            }
            //удаляем пустые столбцы слева
            bool emptyColumn = true;
            int m = 0;//отрезаем всё до m
            while(emptyColumn)
            {
                for (int n = 0; n < verticalSize; n++)
                {
                    if ((PreparingMatrix[n,m] != null) &&(PreparingMatrix[n, m].Letter != '\0'))
                    {
                        emptyColumn = false;
                    }
                }
                m++;
            }
            //удаляем пустые столбцы справа
            emptyColumn = true;
            int l = horizontalSize-1;//отрезаем всё после l
            while (emptyColumn)
            {
                for (int k = 0; k < verticalSize; k++)
                {
                    if ((PreparingMatrix[k,l] != null) && (PreparingMatrix[k, l].Letter != '\0'))
                    {
                        emptyColumn = false;
                    }
                }
                l--;
            }

            mainWordHorizontalIndex = mainWordHorizontalIndex - m+1;
            CrosswordMatrix=new LetterFromWord[verticalSize,l-m+3];
            for (int i = m; i <= l+2; i++)
            {
                for (int j = 0; j < verticalSize; j++)
                {
                    CrosswordMatrix[j,i - m] = PreparingMatrix[j,i-1];
                }
            }
        }

        public void GetMatrixOnConsole()
        {
            for(int i=0;i<verticalSize;i++)
            {
                for (int j = 0; j < CrosswordMatrix.GetLength(1); j++)
                {
                    Console.Write(CrosswordMatrix[i,j]+" ");
                }
                Console.WriteLine();
            }
        }
    }

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
