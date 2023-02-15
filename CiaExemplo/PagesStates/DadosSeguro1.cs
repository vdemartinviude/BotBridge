﻿using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class DadosSeguro1 : BaseState
{
    public DadosSeguro1(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("DadosSeguro1", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        #region SexoSegurado

        if (_inputData.GetStringData("$.Segurado.SexoSegurado") == "Feminino")
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

        var setiniciovigencia = new SetTextWithJsRequest
        {
            By = By.Id("CotacaoModelDataInicioVigencia"),
            Text = _inputData.GetStringData("$.Seguro.InicioVigencia")
        };
        _robot.Execute(setiniciovigencia).Wait();

        _robot.Execute(new SetTextWithJsRequest()
        {
            By = By.Id("CotacaoModelDataFimVigencia"),
            Text = _inputData.GetStringData("$.Seguro.FinalVigencia")
        }).Wait();

        #endregion Vigencia

        #region CepPernoite

        _robot.Execute(new SetTextWithKeyDownRequest()
        {
            By = By.Id("CodigoCodigoEnderecamentoPostalPernoite"),
            Text = _inputData.GetStringData("$.Local.CEPPernoite").Substring(0, 5),
            DelayAfter = TimeSpan.FromSeconds(3)
        }).Wait();

        if (_inputData.GetStringData("$.Local.CEPPernoiteResidencia") == "False")
        {
            _robot.Execute(new ClickRequest()
            {
                By = By.Id("ItemAutoCotacaoModelIndicadorCepResidenciaCheck"),
                Timeout = TimeSpan.FromSeconds(2)
            }).Wait();
            _robot.Execute(new SetTextRequest()
            {
                By = By.Id("ItemAutoCotacaoModelCepResidencia"),
                Text = _inputData.GetStringData("$.Local.CEPResidencia")
            }).Wait();
        }

        #endregion CepPernoite

        #region IsencaoFiscal

        if (_inputData.GetStringData("$.Fiscal.IsencaoFiscal") == "True")
        {
            if (_inputData.GetStringData("$.Fiscal.TipoIsencao").Contains("PESSOA COM"))
            {
                _robot.Execute(new SelectBy2ClicksRequest()
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

            _robot.Execute(new SelectBy2ClicksRequest()
            {
                By1 = By.XPath("//div[@id='ItemAutoCotacaoModelMotivoIsencaoFiscal_chosen']//b"),
                By2 = By.XPath($"//div[@id='ItemAutoCotacaoModelMotivoIsencaoFiscal_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Fiscal.TipoIsencao")}')]"),
                DelayBetweenClicks = TimeSpan.FromSeconds(2)
            }).Wait();
        }

        #endregion IsencaoFiscal

        #region Proprietario

        _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PropriedadeVeiculoCotacaoModelTipoPropriedadeVeiculo_chosen']//b"),
            By2 = By.XPath($"//div[@id='PropriedadeVeiculoCotacaoModelTipoPropriedadeVeiculo_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Proprietario.VinculoSegurado")}')]"),
            DelayBetweenClicks = TimeSpan.FromSeconds(1)
        }).Wait();

        #endregion Proprietario

        #region PrincipalCondutor

        _robot.Execute(new ScrollToElementRequest()
        {
            By = By.XPath("//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//b")
        }).Wait();

        _robot.Execute(new SelectBy2ClicksRequest()
        {
            DelayBetweenClicks = TimeSpan.FromSeconds(2),
            By1 = By.XPath("//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//b"),
            By2 = By.XPath($"//div[@id='CondutorVeiculoCotacaoModelTipoCondutorDependente_chosen']//li[contains(text(),'{_inputData.GetStringData("$.PrincipalCondutor.VinculoSegurado")}')]")
        }).Wait();
        By bySexoCondutor;
        if (_inputData.GetStringData("$.PrincipalCondutor.SexoCondutor") == "Masculino")
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

        _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PerfilItem143_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem143_chosen']//li[contains(text(),'{_inputData.GetStringData("$.PrincipalCondutor.EstadoCivilCondutor")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PerfilItem147_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem147_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.VeiculoUsadoPrestacaoServicos")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2ClicksRequest()
        {
            By1 = By.XPath("//div[@id='PerfilItem148_chosen']//b"),
            By2 = By.XPath($"//div[@id='PerfilItem148_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.ResideIdadeEntre18e24")}')]")
        }).Wait();

        if (_inputData.GetStringData("$.Perfil.ResideIdadeEntre18e24") == "Sim")
        {
            _robot.Execute(new SelectBy2ClicksRequest()
            {
                By1 = By.XPath("//div[@id='PerfilItem149_chosen']//b"),
                By2 = By.XPath($"//div[@id='PerfilItem149_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.EstenderCoberturaIdadeEntre18e24")}')]")
            }).Wait();
        }
        if (_inputData.GetStringData("$.Perfil.EstenderCoberturaIdadeEntre18e24").Contains("Sim"))
        {
            _robot.Execute(new SelectBy2ClicksRequest()
            {
                By1 = By.XPath("//div[@id='PerfilItem150_chosen']//b"),
                By2 = By.XPath($"//div[@id='PerfilItem150_chosen']//li[contains(text(),'{_inputData.GetStringData("$.Perfil.SexoPessoasIdadeEntre18e24")}')]")
            }).Wait();
        }

        #endregion AvaliacaoPerfil

        Console.WriteLine("Testes");
    }
}