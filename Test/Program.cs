using System;
using CrosswordLib;
using GlossaryTermApp;
using MatchGameLib;
using TermLib;
using SerializerLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Serializer serializer = new Serializer(9, "Обществознание");
            //serializer.TermList.Add(new SimpleTerm("Общество", "это обособившаяся от природы, но тесно связанная с ней часть материального мира, которая состоит из индивидуумов, обладающих волей и сознанием, и включает в себя способы взаимодействия людей и формы их объединения."));
            //serializer.TermList.Add(new SimpleTerm("termName2", "termDesc2"));
            //serializer.TermList.Add(new SimpleTerm("termName3", "termDesc3"));
            //serializer.TermList.Add(new SimpleTerm("termName4", "termDesc4"));
            //serializer.TermList.Add(new SimpleTerm("termName5", "termDesc5"));
            //serializer.Serialize();
            //serializer.Deserialize();
            //foreach (var term in serializer.TermList)
            //{
            //    Console.WriteLine(term.ToString());
            //}

            CrosswordGame crosswordGame=new CrosswordGame(serializer.TermList);
            crosswordGame.GetMatrixOnConsole();

            Console.ReadKey();
        }
    }
}
