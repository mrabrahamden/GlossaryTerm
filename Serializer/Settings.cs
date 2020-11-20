using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializerLib
{
    [Serializable]
    public class Settings
    {
        public string Subject;
        public int Class;
        public List<string> ListOfSubjects=new List<string>();
        public Settings(int cl, string sub)
        {
            Class = cl;
            Subject = sub;
        }
    }
}
