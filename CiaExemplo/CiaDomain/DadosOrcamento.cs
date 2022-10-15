using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiaExemplo.CiaDomain;

public class DadosOrcamento
{
    public DadosLogin DadosLogin { get; set; }
}

public class DadosLogin
{
    public string CpfLogin { get; set; }
    public string Password { get; set; }
}