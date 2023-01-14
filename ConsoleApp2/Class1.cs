using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Class1 : ITeste
    {
        public uint priority
        { get { return 10; } }

        public void WriteNumber(uint number)
        {
            Console.WriteLine(number);
        }
    }
}