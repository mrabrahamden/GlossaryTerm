using System;
using System.Collections.Generic;
using System.Linq;
using TermLib;

namespace CrosswordLib
{
    public class CrosswordGame
    {
        public List<SimpleTerm> List;
        public SimpleTerm[] CrossWordTerms;
        private SimpleTerm _mainWord;
        public bool IsReady;
        public LetterFromWord[,] CrosswordMatrix;
        private Random random = new Random(DateTime.Now.Millisecond);
        private LetterFromWord[,] _preparingMatrix;
        private int _verticalSize;
        private int _horizontalSize;
        public int MainWordHorizontalIndex;
        private int _maxLength=0;
        private int _lvl;

        public CrosswordGame(List<SimpleTerm> list, int lvl)
        {
            List = new List<SimpleTerm>(list);
            IsReady = false;
            this._lvl = lvl;
            PrepareGame();
        }

        private void PrepareGame()
        {
            var list = new List<SimpleTerm>();
            ChooseRandomMainWord();
            if (_mainWord != null)
            {
                _verticalSize = _mainWord.Word.Length;
                _horizontalSize = _maxLength * 2 + 1;
                CrossWordTerms = new SimpleTerm[_verticalSize+1];
                CrossWordTerms[0] = _mainWord;
                MainWordHorizontalIndex = _maxLength + 1;
                _preparingMatrix = new LetterFromWord[_verticalSize,_horizontalSize];
                for (int i = 0; i < _verticalSize; i++)
                {
                    _preparingMatrix[i, MainWordHorizontalIndex] = new LetterFromWord(_mainWord.Word[i],_mainWord);
                }

                int count = 0;
                while(count<_verticalSize)
                {   
                    int rand = GetRandom(_verticalSize)+1;
                    if (CrossWordTerms[rand] == null)
                    {
                        if(_mainWord.Word[rand-1]!=' ')
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
            int min, max;
            min = (from t in List orderby t.Word.Length select t).ToList().First().Word.Length;
            max = (from t in List orderby t.Word.Length select t).ToList().Last().Word.Length;
            int dif = (max - min)/3;
            
            if (_lvl == 1)
            {
                max = min + dif;
            }
            else if (_lvl == 2)
            {
                min = min + dif;
                max = max - dif;
            }
            else
            {
                min = max - dif;
            }

            List<SimpleTerm> mainWordCandidates = (from t in List where (t.Word.Length >= min)&& (t.Word.Length <= max) orderby t.Word.Length select t).ToList();
            while (mainWordCandidates.Count == 0)
            {
                min--;
                max++;
                mainWordCandidates=(from t in List where (t.Word.Length >= min) && (t.Word.Length <= max) orderby t.Word.Length select t).ToList();
            }
            if (mainWordCandidates.Count > 0)
            {
                _maxLength = (from t in List orderby t.Word.Length select t).ToList().Last().Word.Length;
                _mainWord = mainWordCandidates[GetRandom(mainWordCandidates.Count)];
                List.Remove(_mainWord);
            }
        }

        private int GetRandom(int mod)
        {
            return random.Next() % mod;
        }

        private void TryAddWordToCrossword(int i)
        {
            SimpleTerm term;
            char letter = _mainWord.Word[i-1];
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
            return (from t in List where t.Word.Contains(char.ToLower(letter)) && t != _mainWord select t).ToList();
        }

        private void FillMatrixByWord(SimpleTerm term,int i)
        {
            List<int> indexesOfLetters=new List<int>();
            for (int ind = 0; ind < term.Word.Length; ind++)
            {
                if (char.ToLower(term.Word[ind]) == char.ToLower(_mainWord.Word[i - 1]))
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
                    _preparingMatrix[i - 1, k + MainWordHorizontalIndex - index] = new LetterFromWord(term.Word[k],term);
                }
            }
        }

        private void PerformMatrix()
        {
            for (int i = 0; i < _verticalSize; i++)
            {
                for (int j = 0; j < _horizontalSize; j++)
                {
                    if(_preparingMatrix[i,j]!=null)
                        _preparingMatrix[i, j].Letter = char.ToUpper(_preparingMatrix[i, j].Letter);
                }
                Console.WriteLine();
            }
            //удаляем пустые столбцы слева
            bool emptyColumn = true;
            int m = 0;//отрезаем всё до m
            while(emptyColumn)
            {
                for (int n = 0; n < _verticalSize; n++)
                {
                    if ((_preparingMatrix[n,m] != null) &&(_preparingMatrix[n, m].Letter != '\0'))
                    {
                        emptyColumn = false;
                    }
                }
                m++;
            }
            //удаляем пустые столбцы справа
            emptyColumn = true;
            int l = _horizontalSize-1;//отрезаем всё после l
            while (emptyColumn)
            {
                for (int k = 0; k < _verticalSize; k++)
                {
                    if ((_preparingMatrix[k,l] != null) && (_preparingMatrix[k, l].Letter != '\0'))
                    {
                        emptyColumn = false;
                    }
                }
                l--;
            }

            MainWordHorizontalIndex = MainWordHorizontalIndex - m+1;
            CrosswordMatrix=new LetterFromWord[_verticalSize,l-m+3];
            for (int i = m; i <= l+2; i++)
            {
                for (int j = 0; j < _verticalSize; j++)
                {
                    CrosswordMatrix[j,i - m] = _preparingMatrix[j,i-1];
                }
            }
        }

        public void GetMatrixOnConsole()
        {
            for(int i=0;i<_verticalSize;i++)
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
