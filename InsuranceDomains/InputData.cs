namespace InsuranceDomains;

public enum Modalidade
{
    NovoSeguro,
    Renovacao
}

public enum SegmentoCliente
{
    Tradicional,
    Varejo,
    Empresas,
    BradescoPrime,
    BradescoCorporate
}

public enum TipoSeguro
{
    Individual,
    BradescoSeguroAuto,
    BradescoContrato
}

public enum Profissao
{
    Estudante,
    DonaDeCasa,
    Aposentado,
    Dentista,
    Medico,
    Militar,
    Policial,
    Bombeiro,
    ProfessorEnsinoBasico,
    ProfessorEnsinoFundamental,
    ProfessorEnsinoMedio,
    ProfessorEnsinoSuperior,
    ServidorPublico,
    Engenheiro,
    Arquiteto,
    Empresario,
    Vendedor,
    RepresentanteComercial,
    NaoTrabalhaNemEstuda,
    ProfessorEducacaoFisica,
    OutrasAtividades
}

public enum Cobertura18a25
{
    SimApenasCondutoresMasculinos,
    SimApenasCondutoresFemininos,
    SimCondutoresAmbosSexos,
    Nao
}

public enum EstadoCivil
{
    Solteiro,
    Casado,
    UniaoEstavel,
    Outros
}

public enum TipoGaragem
{
    SimPortaoManual,
    SimPortaoAutomatico,
    SimPorteiro,
    SimEstacionamentoPrivado,
    SemGaragem
}
public enum Kilometragem
{
    Ate500kmMes,
    Entre500e1500kmMes,
    Mais1500kmMes
}
public enum Sexo
{
    Masculino,
    Feminino
}

public enum TipoPessoa
{
    Fisica,
    Juridica
}
public enum RamoAtividade
{
    Confeccao,
    Corretora,
    DistribuidoraBebidas,
    Engenharia,
    ImportacaoExportacao,
    PrestadorServicos,
    SegurancaVigilancia,
    Supermercado,
    Telecomunicacao,
    Transportadora,
    Alimenticio,
    AssistenciaMedica,
    AutoPecas,
    ComercioPlasticos,
    Construtora,
    EditoraGrafica,
    IndustriaMetalurgica,
    Laticinios,
    Outras
}

public class InputData
{
    public InfoSeguro InfoSeguro { get; set; }
    public Condutor Condutor { get; set; }
}

public class InfoSeguro
{
    public Modalidade Modalidade { get; set; }
    public SegmentoCliente SegmentoCliente { get; set; }
    public TipoSeguro TipoSeguro { get; set; }
}

public class Condutor
{
    public Profissao AtividadeCondutor { get; set; }
    public bool UsaVeiculoEstudo { get; set; }
    public bool UsaVeiculoTrabalho { get; set; }
    public string CEPEstudo { get; set; }
    public string CEPTrabalho { get; set; }
    public string CEPPernoite { get; set; }
    public TipoGaragem TipoGaragemEstudo { get; set; }
    public TipoGaragem TipoGaragemTrabalho { get; set; }
    public TipoGaragem Pernoite { get; set; }
    public Cobertura18a25 Cobertura18a25 { get; set; }
    public string CPFCondutor { get; set; }
    public string CPFProponente { get; set; }
    public DateOnly DataNascimentoCondutor { get; set; }
    public EstadoCivil EstadoCivilCondutor { get; set; }
    public Kilometragem Kilometragem { get; set; }
    public Sexo SexoCondutor { get; set; }
    public RamoAtividade RamoAtividadeCondutor { get; set; }
    public bool MaisDeUmVeiculo { get; set; }
}

public class QuestionarioAvaliacaoRisco
{
    public TipoPessoa TipoPessoaSegurado { get; set; }
    public Sexo SexoSegurado { get; set; }
    public DateOnly DataNascimentoSegurado { get; set; }
    public bool SeguradoPrincipalCondutor { get; set; }
    public string CPFSegurado { get; set; }
    public string CNPJSegurado { get; set; }

}