using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using TermLib;

namespace SerializerLib
{
    public class Serializer 
    {
        public List<Term> TermList=new List<Term>();
        public static int Class;
        public static string Subject;
        public string Path = @"%appdata%\Roaming\Teacherry"+Subject;
        public string FileName = Class.ToString()+".dat";
        BinaryFormatter formatter = new BinaryFormatter();

        public Serializer(int cl,string subj)
        {
            Class = cl;
            Subject = subj;
        }
        public void Serialize()
        {
            CheckForPathExist();
            using (FileStream fs = new FileStream(Path+FileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, TermList);
                Console.WriteLine("Объект сериализован");
            }
        }
        public void Deserialize()
        {
            CheckForPathExist();
            using (FileStream fs = new FileStream(Path+FileName, FileMode.OpenOrCreate))
            {
                TermList = (List<Term>)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован");
            }
        }
        private void CheckForPathExist()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }
    }
}
