using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using TermLib;

namespace SerializerLib
{
    public class Serializer 
    {
        public List<SimpleTerm> TermList=new List<SimpleTerm>();
        public static int Class;
        public static string Subject;

        public string DefaultPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Teacherry\\";

        public string Path;
        public string FileName;
        BinaryFormatter formatter = new BinaryFormatter();

        public Serializer(int cl,string subj)
        {
            if(cl<=11&&cl>=1)
                Class = cl;
            Subject = subj;
            FileName = Class + ".dat";
            Path = DefaultPath + Subject + "\\";
            Deserialize();
        }
        public void Serialize()
        {
            CheckForPathExist();
            using (FileStream fs = new FileStream(Path+FileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, TermList);
                Console.WriteLine("Сериализован");
            }
        }
        public void Deserialize()
        {
            CheckForPathExist();
            try
            {
                if (File.Exists(Path + FileName))
                {
                    using (FileStream fs = new FileStream(Path + FileName, FileMode.OpenOrCreate))
                    {
                        TermList = (List<SimpleTerm>) formatter.Deserialize(fs);
                        Console.WriteLine("Десериализован");
                    }
                }
                else
                {
                    Serialize();
                    Deserialize();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка десериализации");
            }
        }
        private void CheckForPathExist()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }
    }
}
