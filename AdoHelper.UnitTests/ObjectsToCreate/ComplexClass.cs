using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdoHelper.UnitTests.ObjectsToCreate
{
    class ComplexClass
    {
        public string Name { get; set; }

        public ConsoleKey Key { get; private set; }

        public StringBuilder builder;

        public ComplexClass(string name, ConsoleKey key, StringBuilder builder)
        {
            Name = name;
            Key = key;
            this.builder = builder;
        }
    }
}
