using Json.Path;
using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class DadosPrincipais : BaseState
{
    public DadosPrincipais(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("DadosPrincipais", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        var pathRenovacao = JsonPath.Parse("$.DadosRenovacao.Renovacao");
        if (_inputData.GetStringData(pathRenovacao) == "True")
        {
            await ProcessaComRenovacao();
        }
        else
        {
            await ProcessaSemRenovacao();
        }

        if (_inputData.GetStringData("$.Segurado.TipoPessoa") == "Jurídica")
        {
            await ProcessaPessoaJuridica();
        }
        else
        {
            await ProcessaPessoaFisica();
        }

        await ProcessaPlacaChassi();

        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("btnCotar"),
            Timeout = TimeSpan.FromSeconds(1)
        });
    }

    private async Task ProcessaComRenovacao()
    {
        var pathSeguradora = JsonPath.Parse("$.DadosRenovacao.DadosApoliceAnterior.Seguradora");
        var seguradora = _inputData.GetStringData(pathSeguradora);
        if (String.IsNullOrEmpty(seguradora))
        {
            throw new EstimateParametersException("Seguradora anterior não informada na renovação");
        }

        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("rdRenovacaoSim"),
            Timeout = TimeSpan.FromSeconds(3),
            DelayAfter = TimeSpan.FromSeconds(5)
        });

        await _robot.Execute(new SelectBy2ClicksRequest
        {
            By1 = By.XPath("//span[contains(text(),'Selecione uma opção')]/../div/b"),
            By2 = By.XPath($"//li[contains(@class,'active-result') and contains(text(),'{seguradora}')]"),
            DelayBetweenClicks = TimeSpan.FromSeconds(1)
        });

        var apoliceAnterior = _inputData.GetStringData("$.DadosRenovacao.DadosApoliceAnterior.Apolice");
        if (!String.IsNullOrEmpty(apoliceAnterior))
        {
            await _robot.Execute(new SetTextRequest()
            {
                By = By.Id("PrincipalRenovacaoNumeroApoliceAnterior"),
                Text = apoliceAnterior
            });
        }

        var itemAnterior = _inputData.GetStringData("$.DadosRenovacao.DadosApoliceAnterior.Item");
        if (!String.IsNullOrEmpty(itemAnterior))
        {
            await _robot.Execute(new SetTextRequest()
            {
                By = By.Id("PrincipalRenovacaoNumeroItemApoliceAnterior"),
                Text = itemAnterior
            });
        }
    }

    private async Task ProcessaPessoaFisica()
    {
        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("rdSeguradoFisica"),
            Timeout = TimeSpan.FromSeconds(1)
        });

        var cpf = _inputData.GetStringData("$.Segurado.CPF");
        await _robot.Execute(new SetTextRequest()
        {
            By = By.Id("PrincipalSeguradoCotacaoModelCodigoCodigoPessoaFisicaCgc"),
            Text = cpf
        });

        if (_inputData.GetStringData("$.Segurado.SeguradoPrincipalCondutor") == "True")
        {
            await _robot.Execute(new ClickRequest()
            {
                By = By.Id("PrincipalCotacaoSeguradoPrincipal"),
                Timeout = TimeSpan.FromSeconds(1)
            });
        }
        else
        {
            await _robot.Execute(new SetTextRequest()
            {
                By = By.Id("PrincipalCpfPrincipalCondutor"),
                Text = _inputData.GetStringData("$.Segurado.CPFPrincipalCondutor")
            });
        }
    }

    private async Task ProcessaPessoaJuridica()
    {
        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("rdSeguradoJuridica"),
            Timeout = TimeSpan.FromSeconds(1),
            DelayAfter = TimeSpan.FromSeconds(1)
        });
        var cnpj = _inputData.GetStringData("$.Segurado.CNPJ");
        if (String.IsNullOrEmpty(cnpj))
        {
            throw new EstimateParametersException("CNPJ da pessoa jurídica não informado");
        }
        await _robot.Execute(new SetTextRequest()
        {
            By = By.Id("PrincipalSeguradoCotacaoModelCodigoCodigoPessoaFisicaCgc"),
            Text = cnpj
        });
        var cpfcondutor = _inputData.GetStringData("$.Segurado.CPFPrincipalCondutor");
        if (String.IsNullOrEmpty(cpfcondutor))
        {
            throw new EstimateParametersException("CPF Principal condutor não informado");
        }
        await _robot.Execute(new SetTextRequest()
        {
            By = By.Id("PrincipalCpfPrincipalCondutor"),
            Text = cpfcondutor
        });
    }

    private async Task ProcessaPlacaChassi()
    {
        if (_inputData.GetStringData("$.Veiculo.ZeroKmSemPlaca") == "True")
        {
            await _robot.Execute(new ClickRequest()
            {
                By = By.Id("PrincipalIndicadorVeiculo0KMSemPlaca"),
                Timeout = TimeSpan.FromSeconds(1)
            });
        }
        else
        {
            await _robot.Execute(new SetTextRequest()
            {
                By = By.Id("PrincipalItemAutoCotacaoModelLicencaCodigo"),
                Text = _inputData.GetStringData("$.Veiculo.Placa")
            });
        }
        var chassi = _inputData.GetStringData("$.Veiculo.Chassi");
        if (!string.IsNullOrEmpty(chassi))
        {
            await _robot.Execute(new SetTextRequest()
            {
                By = By.Id("PrincipalItemAutoCotacaoModelNumeroSerie"),
                Text = chassi
            });
        }
    }

    private async Task ProcessaSemRenovacao()
    {
        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("rdRenovacaoNao"),
            Timeout = TimeSpan.FromSeconds(1)
        });
    }
}