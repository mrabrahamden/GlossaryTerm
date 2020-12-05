using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SerializerLib
{
    public class Serializer
    {
        public List<SimpleTerm> TermList = new List<SimpleTerm>();
        public string DefaultPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Teacherry\\";
        public string Path;
        public string FileName;
        private string settingsFileName = "settings.dat";
        internal BinaryFormatter Formatter = new BinaryFormatter();
        public Settings Settings = new Settings(0, null);

        public Serializer()
        {
            GetSettings();
            UpdateFileNameAndPath();
            Deserialize();
        }
        public Serializer(int cl, string subj)
        {
            Settings.Class = cl;
            Settings.Subject = subj;
            UpdateFileNameAndPath();
            Deserialize();
        }
        private void UpdateFileNameAndPath()
        {
            FileName = Settings.Class + ".dat";
            Path = DefaultPath + Settings.Subject + "\\";
        }
        public void DeleteTermByString(string wordAndDescrString)
        {
            Regex regex = new Regex(@"((\w)+\s?)+");
            var term = regex.Match(wordAndDescrString).ToString();
            term = term.Substring(0, term.Length - 1);
            var descr = GetTermDescriptionByString(wordAndDescrString);
            TermList.RemoveAll((simpleTerm => simpleTerm.Word == term && simpleTerm.Description == descr));
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
            Regex regex = new Regex(@" ⸺ .+");
            MatchCollection matches = regex.Matches(wordAndDescrString);
            var description = regex.Match(wordAndDescrString).ToString();
            description = description.Substring(3, description.Length - 3);
            return description;
        }
        public SimpleTerm GetTermByString(string wordAndDescr)
        {
            var result = TermList.Find(term =>
                 term.Word == GetTermNameByString(wordAndDescr) &&
                 term.Description == GetTermDescriptionByString(wordAndDescr));
            return result;
        }
        public List<SimpleTerm> LookForAWord(string word)
        {
            var result = new List<SimpleTerm>();
            result = TermList.FindAll(term => term.Word.ToLower().Contains(word.ToLower()));
            result.AddRange(TermList.FindAll(term => term.Description.ToLower().Contains(word.ToLower())));
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
            var sortedList = (from term in TermList
                              orderby term.Word
                              select term).ToList();
            TermList = sortedList;
        }
        public void Serialize()
        {
            UpdateFileNameAndPath();
            CheckForPathExist(Path);
            using (FileStream fs = new FileStream(Path + FileName, FileMode.OpenOrCreate))
            {
                Formatter.Serialize(fs, TermList);
                Console.WriteLine("Сериализован");
            }
        }
        public void Deserialize()
        {
            UpdateFileNameAndPath();
            CheckForPathExist(Path);
            try
            {
                if (File.Exists(Path + FileName))
                {
                    if (TermList.Count == 0)
                    {
                        using (FileStream fs = new FileStream(Path + FileName, FileMode.OpenOrCreate))
                        {
                            TermList = (List<SimpleTerm>)Formatter.Deserialize(fs);
                            Console.WriteLine("Десериализован");
                        }
                    }
                }
                else
                {
                    if (Settings.Class > 0)
                        Serialize();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка десериализации");
            }
        }
        internal void CheckForPathExist(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public bool GetSettings()
        {
            CheckForPathExist(DefaultPath);
            try
            {
                if (File.Exists(DefaultPath + settingsFileName))
                {
                    using (FileStream fs = new FileStream(DefaultPath + settingsFileName, FileMode.OpenOrCreate))
                    {
                        Settings = (Settings)Formatter.Deserialize(fs);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveSettings()
        {
            CheckForPathExist(DefaultPath);
            try
            {
                using (FileStream fs = new FileStream(DefaultPath + settingsFileName, FileMode.OpenOrCreate))
                {
                    Formatter.Serialize(fs, Settings);
                    return true;
                }

            }
            catch (Exception)
            {
                try
                {
                    File.Delete(DefaultPath + settingsFileName);
                }
                catch (Exception)
                {
                    //не удалось удалить файл настроек
                }
                return false;
            }
        }
    }
}
