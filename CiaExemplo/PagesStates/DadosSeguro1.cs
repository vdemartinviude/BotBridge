using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.PagesStates;

public class DadosSeguro1 : BaseState
{
    public DadosSeguro1(Robot robot, BaseOrcamento inputdata) : base("DadosSeguro1", robot, inputdata)
    {
    }

    public override void Execute()
    {
        #region SexoSegurado

        if (_orcamento.GetStringData("$.Segurado.SexoSegurado") == "Feminino")
        {
            var clicksexfem = new ClickRequest()
            {
                By = By.Id("SeguradoPessoafisicaCotacaoModelCodigoSexoFeminino"),
                Timeout = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(clicksexfem).Wait();
        }
        else
        {
            var clicksexmas = new ClickRequest()
            {
                By = By.Id("SeguradoPessoafisicaCotacaoModelCodigoSexoMasculino"),
                Timeout = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(clicksexmas).Wait();
        }

        #endregion SexoSegurado

        #region Vigencia

        var setiniciovigencia = new SetTextWithJs
        {
            By = By.Id("CotacaoModelDataInicioVigencia"),
            Text = _orcamento.GetStringData("$.Seguro.InicioVigencia")
        };
        _robot.Execute(setiniciovigencia).Wait();

        _robot.Execute(new SetTextWithJs()
        {
            By = By.Id("CotacaoModelDataFimVigencia"),
            Text = _orcamento.GetStringData("$.Seguro.FinalVigencia")
        }).Wait();

        #endregion Vigencia

        #region CepPernoite

        _robot.Execute(new SetTextWithKeyDown()
        {
            By = By.Id("CodigoCodigoEnderecamentoPostalPernoite"),
            Text = _orcamento.GetStringData("$.Local.CEPPernoite").Substring(0, 5),
            DelayAfter = TimeSpan.FromSeconds(3)
        }).Wait();

        if (_orcamento.GetStringData("$.Local.CEPPernoiteResidencia") == "False")
        {
            _robot.Execute(new ClickRequest()
            {
                By = By.Id("ItemAutoCotacaoModelIndicadorCepResidenciaCheck"),
                Timeout = TimeSpan.FromSeconds(2)
            }).Wait();
            _robot.Execute(new SetTextRequest()
            {
                By = By.Id("ItemAutoCotacaoModelCepResidencia"),
                Text = _orcamento.GetStringData("$.Local.CEPResidencia")
            }).Wait();
        }

        #endregion CepPernoite

        #region IsencaoFiscal

        if (_orcamento.GetStringData("$.Fiscal.IsencaoFiscal") == "True")
        {
            if (_orcamento.GetStringData("$.Fiscal.TipoIsencao").Contains("PESSOA COM"))
            {
                _robot.Execute(new SelectBy2Clicks()
                {
                    By1 = By.XPath("//div[@id='ItemAutoCotacaoModelAdaptacaoVeiculo_chosen']//input"),
                    By2 = By.XPath("//div[@id='ItemAutoCotacaoModelAdaptacaoVeiculo_chosen']//ul[@class='chosen-results']/li[text()='DEF. FÍSICO']"),
                    DelayBetweenClicks = TimeSpan.FromSeconds(1)
                }).Wait();
            }

            _robot.Execute(new ScrollToElementRequest()
            {
                By = By.Id("rdIsencaoFiscalSim")
            }).Wait();

            _robot.Execute(new ClickByJavascriptRequest()
            {
                By = By.Id("rdIsencaoFiscalSim"),

                DelayAfter = TimeSpan.FromSeconds(10)
            }).Wait();

            _robot.Execute(new SelectBy2Clicks()
            {
                By1 = By.XPath("//div[@id='ItemAutoCotacaoModelMotivoIsencaoFiscal_chosen']//b"),
                By2 = By.XPath($"//div[@id='ItemAutoCotacaoModelMotivoIsencaoFiscal_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Fiscal.TipoIsencao")}')]"),
                DelayBetweenClicks = TimeSpan.FromSeconds(2)
            }).Wait();
        }

        #endregion IsencaoFiscal

        #region Proprietario

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='PropriedadeVeiculoCotacaoModelTipoPropriedadeVeiculo_chosen']//b"),
            By2 = By.XPath($"//div[@id='PropriedadeVeiculoCotacaoModelTipoPropriedadeVeiculo_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Proprietario.VinculoSegurado")}')]"),
            DelayBetweenClicks = TimeSpan.FromSeconds(1)
        }).Wait();

        #endregion Proprietario

        #region PrincipalCondutor

        _robot.Execute(new ScrollToElementRequest()
        {
            By = By.XPath("//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//b")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            DelayBetweenClicks = TimeSpan.FromSeconds(2),
            By1 = By.XPath("//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//b"),
            By2 = By.XPath($"//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.PrincipalCondutor.VinculoSegurado")}')]")
        }).Wait();
        By bySexoCondutor;
        if (_orcamento.GetStringData("$.PrincipalCondutor.SexoCondutor") == "Masculino")
        {
            bySexoCondutor = By.Id("CondutorVeiculoCotacaoModelCodigoSexoMasculino");
        }
        else
        {
            bySexoCondutor = By.Id("CondutorVeiculoCotacaoModelCodigoSexoFeminino");
        }
        _robot.Execute(new ClickByJavascriptRequest()
        {
            By = bySexoCondutor,
        }).Wait();

        #endregion PrincipalCondutor

        #region AvaliacaoPerfil

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='PerfilItem143_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem143_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.PrincipalCondutor.EstadoCivilCondutor")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='PerfilItem147_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem147_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Perfil.VeiculoUsadoPrestacaoServicos")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='PerfilItem148_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem148_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Perfil.ResideIdadeEntre18e24")}')]")
        }).Wait();

        if (_orcamento.GetStringData("$.Perfil.ResideIdadeEntre18e24") == "Sim")
        {
            _robot.Execute(new SelectBy2Clicks()
            {
                By1 = By.XPath("//div[@id='PerfilItem149_chosen']//b"),
                By2 = By.XPath($"//div[@id='PerfilItem149_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Perfil.EstenderCoberturaIdadeEntre18e24")}')]")
            }).Wait();
        }
        if (_orcamento.GetStringData("$.Perfil.EstenderCoberturaIdadeEntre18e24").Contains("Sim"))
        {
            _robot.Execute(new SelectBy2Clicks()
            {
                By1 = By.XPath("//div[@id='PerfilItem150_chosen']//b"),
                By2 = By.XPath($"//div[@id='PerfilItem150_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Perfil.SexoPessoasIdadeEntre18e24")}')]")
            }).Wait();
        }

        #endregion AvaliacaoPerfil

        Console.WriteLine("Testes");
    }
}