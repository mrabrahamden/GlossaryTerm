using CrosswordLib;
using SerializerLib;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Serializer serializer = new Serializer(9, "Обществознание");
            CrosswordGame crosswordGame;
            for (int i = 0; i < 10; i++)
            {
                crosswordGame = new CrosswordGame(serializer.TermList);
                crosswordGame.GetMatrixOnConsole();
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
