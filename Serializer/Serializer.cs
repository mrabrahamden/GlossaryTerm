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

        private Settings settings;
        public string DefaultPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Teacherry\\";

        public string Path;
        public string FileName;
        BinaryFormatter formatter = new BinaryFormatter();

        public Serializer(int cl,string subj)
        {
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
            var descr = GetTermDescriptionByString(wordAndDescrString);
            TermList.RemoveAll((simpleTerm => simpleTerm.Word==term && simpleTerm.Description==descr));
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
            return DeleteSimilarTerms(result);
        }
        public List<SimpleTerm> DeleteSimilarTerms(List<SimpleTerm> list)
        {
            var resultList = list.Distinct().ToList();
            return resultList;
        }
        public bool DeleteSimilarTerms()
        {
            var list = TermList.Distinct(new SimpleTermEqualityComparer()).ToList();
            if (TermList.Count == list.Count)
            {
                TermList = list;
                return false;
            }
            else
            {
                TermList = list;
                return true;
            }
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
            CheckForPathExist(Path);
            using (FileStream fs = new FileStream(Path+FileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, TermList);
                Console.WriteLine("Сериализован");
            }
        }
        public void Deserialize()
        {
            CheckForPathExist(Path);
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
        private void CheckForPathExist(string path)
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        private void GetSettings()
        {
            string fileName = "settings.dat";
            CheckForPathExist(DefaultPath);
            try
            {
                if (File.Exists(DefaultPath + fileName))
                {
                    using (FileStream fs = new FileStream(DefaultPath + fileName, FileMode.OpenOrCreate))
                    {
                        settings = (Settings)formatter.Deserialize(fs);
                        Console.WriteLine("Десериализован");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка десериализации");
            }
        }

        public void SaveSettings(Settings settings)
        {

        }
    }

    [Serializable]
    public class Settings
    {
        public string Subject;
        public int Class;

        public Settings(int cl, string sub)
        {
            Class = cl;
            Subject = sub;
        }
    }
}
