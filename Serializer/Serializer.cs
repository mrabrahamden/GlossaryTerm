using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
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

        public void DeleteTermByString(string wordAndDescrString)
        {
            Regex regex=new Regex(@"((\w)+\s?)+");
            var term =regex.Match(wordAndDescrString).ToString();
            term = term.Substring(0, term.Length - 1);
            TermList.RemoveAll((simpleTerm => simpleTerm.Word==term));
        }

        public string GetTermNameByString(string wordAndDescrString)
        {
            Regex regex = new Regex(@"((\w)+\s?)+");
            var term = regex.Match(wordAndDescrString).ToString();
            term = term.Substring(0, term.Length - 1);
            return term;
        }

        public string GetTermDescriptionByString(string wordAndDescrString)
        {
            Regex regex = new Regex(@" -- .+");
            MatchCollection matches = regex.Matches(wordAndDescrString);
            var description = regex.Match(wordAndDescrString).ToString();
            description = description.Substring(4, description.Length - 4);
            return description;
        }

        public SimpleTerm GetTermByString(string wordAndDescr)
        {
            var result= TermList.Find(term =>
                term.Word == GetTermNameByString(wordAndDescr) &&
                term.Description == GetTermDescriptionByString(wordAndDescr));
            return result;
        }

        public List<SimpleTerm> LookForAWord(string word)
        {
            var result = new List<SimpleTerm>();
            result = TermList.FindAll(term => term.Word.ToLower().Contains(word.ToLower()));
            result.AddRange(TermList.FindAll(term=> term.Description.ToLower().Contains(word.ToLower())));
            return result;
        }
        public void SortList()
        {
            var sortedList =(from term in TermList
                orderby term.Word
                select term).ToList();
            TermList = sortedList;
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
                    if (TermList.Count == 0)
                    {
                        using (FileStream fs = new FileStream(Path + FileName, FileMode.OpenOrCreate))
                        {
                            TermList = (List<SimpleTerm>) formatter.Deserialize(fs);
                            Console.WriteLine("Десериализован");
                        }
                    }
                }
                else
                {
                    Serialize();
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
