using System;
using TermLib;
using SerializerLib;
using SubjectLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var subj=new Subject(10);
            Serializer serializer=new Serializer(9,subj.Name);
            serializer.TermList.Add(new SimpleTerm("termName1", "termDesc1"));
            serializer.TermList.Add(new SimpleTerm("termName2", "termDesc2"));
            serializer.TermList.Add(new SimpleTerm("termName3", "termDesc3"));
            serializer.TermList.Add(new SimpleTerm("termName4", "termDesc4"));
            serializer.TermList.Add(new SimpleTerm("termName5", "termDesc5"));
            serializer.Serialize();
            serializer.Deserialize();
            foreach (var term in serializer.TermList)
            {
                Console.WriteLine(term.ToString());
            }

            Console.ReadKey();
        }
    }
}
