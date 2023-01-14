using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;

public interface ITeste
{
    public uint priority { get; }

    public void WriteNumber(uint number);
}