using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermLib;
using SerializerLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Serializer serializer=new Serializer(9,"Обществознание");
            serializer.TermList.Add(new Term("termName1", "termDesc1"));
            serializer.TermList.Add(new Term("termName2", "termDesc2"));
            serializer.TermList.Add(new Term("termName3", "termDesc3"));
            serializer.TermList.Add(new Term("termName4", "termDesc4"));
            serializer.TermList.Add(new Term("termName5", "termDesc5"));
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
