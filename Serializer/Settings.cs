using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializerLib
{
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
