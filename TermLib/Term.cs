﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermLib
{
    public class Term
    {
        public string Name;
        public string Description;

        public Term(string name, string desc)
        {
            Name = name;
            Description = desc;
        }
    }
}
