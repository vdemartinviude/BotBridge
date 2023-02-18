using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class DadosSeguro1 : BaseState
{
    public DadosSeguro1(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("DadosSeguro1", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await ProcessaSexoSegurado();

        await ProcessaVigencia();

        await ProcessaCepPernoite();

        await ProcessaIsencaoFiscal();

        await ProcessaProprietario();

        await ProcessaPrincipalCondutor();

        await ProcessaAvaliacaoPerfil();
    }

    private async Task ProcessaAvaliacaoPerfil()
    {
        await _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PerfilItem143_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem143_chosen']//li[contains(text(),'{_inputData.GetStringData("$.PrincipalCondutor.EstadoCivilCondutor")}')]")
        });

        await _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PerfilItem147_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem147_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.VeiculoUsadoPrestacaoServicos")}')]")
        });

        await _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PerfilItem148_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem148_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.ResideIdadeEntre18e24")}')]")
        });

        if (_inputData.GetStringData("$.Perfil.ResideIdadeEntre18e24") == "Sim")
        {
            await _robot.Execute(new SelectBy2ClicksRequest()
            {
                By1 = By.XPath("//div[@id='PerfilItem149_chosen']//b"),
                By2 = By.XPath($"//div[@id='PerfilItem149_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.EstenderCoberturaIdadeEntre18e24")}')]")
            });
        }
        if (_inputData.GetStringData("$.Perfil.EstenderCoberturaIdadeEntre18e24").Contains("Sim"))
        {
            await _robot.Execute(new SelectBy2ClicksRequest()
            {
                By1 = By.XPath("//div[@id='PerfilItem150_chosen']//b"),
                By2 = By.XPath($"//div[@id='PerfilItem150_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.SexoPessoasIdadeEntre18e24")}')]")
            });
        }
    }

    private async Task ProcessaPrincipalCondutor()
    {
        await _robot.Execute(new ScrollToElementRequest()
        {
            By = By.XPath("//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//b")
        });

        await _robot.Execute(new SelectBy2ClicksRequest()
        {
            DelayBetweenClicks = TimeSpan.FromSeconds(2),
            By1 = By.XPath("//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//b"),
            By2 = By.XPath($"//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//li[contains(text(),'{_inputData.GetStringData("$.PrincipalCondutor.VinculoSegurado")}')]")
        });
        By bySexoCondutor;
        if (_inputData.GetStringData("$.PrincipalCondutor.SexoCondutor") == "Masculino")
        {
            bySexoCondutor = By.Id("CondutorVeiculoCotacaoModelCodigoSexoMasculino");
        }
        else
        {
            bySexoCondutor = By.Id("CondutorVeiculoCotacaoModelCodigoSexoFeminino");
        }
        await _robot.Execute(new ClickByJavascriptRequest()
        {
            By = bySexoCondutor,
        });
    }

    private async Task ProcessaProprietario()
    {
        await _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PropriedadeVeiculoCotacaoModelTipoPropriedadeVeiculo_chosen']//b"),
            By2 = By.XPath($"//div[@id='PropriedadeVeiculoCotacaoModelTipoPropriedadeVeiculo_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Proprietario.VinculoSegurado")}')]"),
            DelayBetweenClicks = TimeSpan.FromSeconds(1)
        });
    }

    private async Task ProcessaIsencaoFiscal()
    {
        if (_inputData.GetStringData("$.Fiscal.IsencaoFiscal") == "True")
        {
            if (_inputData.GetStringData("$.Fiscal.TipoIsencao").Contains("PESSOA COM"))
            {
                await _robot.Execute(new SelectBy2ClicksRequest()
                {
                    By1 = By.XPath("//div[@id='ItemAutoCotacaoModelAdaptacaoVeiculo_chosen']//input"),
                    By2 = By.XPath("//div[@id='ItemAutoCotacaoModelAdaptacaoVeiculo_chosen']//ul[@class='chosen-results']/li[text()='DEF. FÍSICO']"),
                    DelayBetweenClicks = TimeSpan.FromSeconds(1)
                });
            }

            await _robot.Execute(new ScrollToElementRequest()
            {
                By = By.Id("rdIsencaoFiscalSim")
            });

            await _robot.Execute(new ClickByJavascriptRequest()
            {
                By = By.Id("rdIsencaoFiscalSim"),

                DelayAfter = TimeSpan.FromSeconds(10)
            });

            await _robot.Execute(new SelectBy2ClicksRequest()
            {
                By1 = By.XPath("//div[@id='ItemAutoCotacaoModelMotivoIsencaoFiscal_chosen']//b"),
                By2 = By.XPath($"//div[@id='ItemAutoCotacaoModelMotivoIsencaoFiscal_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Fiscal.TipoIsencao")}')]"),
                DelayBetweenClicks = TimeSpan.FromSeconds(2)
            });
        }
    }

    private async Task ProcessaCepPernoite()
    {
        await _robot.Execute(new SetTextWithKeyDownRequest()
        {
            By = By.Id("CodigoCodigoEnderecamentoPostalPernoite"),
            Text = _inputData.GetStringData("$.Local.CEPPernoite").Substring(0, 5),
            DelayAfter = TimeSpan.FromSeconds(3)
        });

        if (_inputData.GetStringData("$.Local.CEPPernoiteResidencia") == "False")
        {
            await _robot.Execute(new ClickRequest()
            {
                By = By.Id("ItemAutoCotacaoModelIndicadorCepResidenciaCheck"),
                Timeout = TimeSpan.FromSeconds(2)
            });
            await _robot.Execute(new SetTextRequest()
            {
                By = By.Id("ItemAutoCotacaoModelCepResidencia"),
                Text = _inputData.GetStringData("$.Local.CEPResidencia")
            });
        }
    }

    private async Task ProcessaVigencia()
    {
        await _robot.Execute(new SetTextWithJsRequest
        {
            By = By.Id("CotacaoModelDataInicioVigencia"),
            Text = _inputData.GetStringData("$.Seguro.InicioVigencia")
        });

        await _robot.Execute(new SetTextWithJsRequest()
        {
            By = By.Id("CotacaoModelDataFimVigencia"),
            Text = _inputData.GetStringData("$.Seguro.FinalVigencia")
        });
    }

    private async Task ProcessaSexoSegurado()
    {
        if (_inputData.GetStringData("$.Segurado.SexoSegurado") == "Feminino")
        {
            await _robot.Execute(new ClickRequest()
            {
                By = By.Id("SeguradoPessoafisicaCotacaoModelCodigoSexoFeminino"),
                Timeout = TimeSpan.FromSeconds(1)
            });
        }
        else
        {
            await _robot.Execute(new ClickRequest()
            {
                By = By.Id("SeguradoPessoafisicaCotacaoModelCodigoSexoMasculino"),
                Timeout = TimeSpan.FromSeconds(1)
            });
        }
    }
}