using System;
using TermLib;
using SerializerLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Serializer serializer=new Serializer(9,"Обществознание");
            serializer.TermList.Add(new SimpleTerm("termName1", "termDesgejhf uweuihf iuehf uierguheukf hkufg uehfku ahfi uh eufh ekuf ukhuhf ug u fheejufg eagf juuh kuhfiurhfu huefiuhfu khfu aherfu uhfu riuuiyf uayc1"));
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
