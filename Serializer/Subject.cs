using System.Collections.Generic;

namespace SubjectLib
{
    public class Subject
    {
        private int num;
        public string Name;
        private List<string> subjNames;
        public Subject(int num)
        {
            subjNames = new List<string>()
            {
                "Английский язык",
                "Биология",
                "География",
                "Изобразительное искусство",
                "Информатика",
                "История",
                "Математика",
                "Музыка",
                "МХК",
                "Обществознание",
                "Русский язык",
                "Технология",
                "Физика",
                "Французский язык",
                "Химия"
            };
            if (num >= 0 && num<subjNames.Count)
                this.num = num;
            else
                this.num = 0;
            Name = subjNames[num];
        }
    }
}
