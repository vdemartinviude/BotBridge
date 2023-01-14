using JsonDocumentsManager;
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

public class DadosSeguro2 : BaseState
{
    public DadosSeguro2(Robot robot, BaseOrcamento inputdata, ResultJsonDocument resultJson) : base("DadosSeguro2", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        double? value;
        value = _orcamento.GetDoubleData("$.Coberturas.FatorDeAjuste");
        value ??= 100;
        string text = String.Format("{0:#.00}", value).Replace(".", "").Replace(",", "");
        _robot.Execute(new SetTextWithKeyDownAndBackSpace()
        {
            By = By.Id("TipoCobertura_FatorAjuste_1"),
            Text = text,
            BackSpaceNumber = 7
        }).Wait();

        _robot.Execute(new SelectRangeByValue()
        {
            ByClick = By.XPath("//div[@id='TipoCobertura_Franquia_FranquiaCodigo_Select_1_chosen']//b"),
            BySelectValues = By.XPath("//div[@id='TipoCobertura_Franquia_FranquiaCodigo_Select_1_chosen']//li[contains(text(),'%')]"),
            LessThan = true,
            Value = _orcamento.GetDoubleData("$.Coberturas.Franquia") ?? 50
        }).Wait();

        _robot.Execute(new SelectRangeByValue()
        {
            ByClick = By.XPath("//div[@id='TipoCobertura_LMIRFCDM_1_chosen']//b"),
            BySelectValues = By.XPath("//div[@id='TipoCobertura_LMIRFCDM_1_chosen']//li[contains(text(),'R$')]"),
            GreaterThan = true,
            Value = _orcamento.GetDoubleData("$.Coberturas.DanosMateriais") ?? double.MaxValue
        }).Wait();

        _robot.Execute(new SelectRangeByValue()
        {
            ByClick = By.XPath("//div[@id='TipoCobertura_LMIRCFDC_1_chosen']//b"),
            BySelectValues = By.XPath("//div[@id='TipoCobertura_LMIRCFDC_1_chosen']//li[contains(text(),'R$')]"),
            GreaterThan = true,
            Value = _orcamento.GetDoubleData("$.Coberturas.DanosCorporais") ?? double.MaxValue
        }).Wait();

        _robot.Execute(new SelectRangeByValue()
        {
            ByClick = By.XPath("//div[@id='TipoCobertura_LMIDanosMorais_1_chosen']//b"),
            BySelectValues = By.XPath("//div[@id='TipoCobertura_LMIDanosMorais_1_chosen']//li[contains(text(),'R$')]"),
            GreaterThan = true,
            Value = _orcamento.GetDoubleData("$.Coberturas.DanosMorais") ?? double.MaxValue,
            DelayBetweenClicks = TimeSpan.FromSeconds(2)
        }).Wait();

        value = _orcamento.GetDoubleData("$.Coberturas.AcidentesPessoaisMorte");
        value ??= 100;
        text = String.Format("{0:#.00}", value).Replace(".", "").Replace(",", "");
        _robot.Execute(new SetTextWithKeyDownAndBackSpace
        {
            BackSpaceNumber = 5,
            Text = text,
            By = By.Id("TipoCobertura_LMIAPPMorte_1")
        }).Wait();

        value = _orcamento.GetDoubleData("$.Coberturas.AcidentesPessoaisInvalidez");
        value ??= 100;
        text = String.Format("{0:#.00}", value).Replace(".", "").Replace(",", "");
        _robot.Execute(new SetTextWithKeyDownAndBackSpace
        {
            BackSpaceNumber = 5,
            Text = text,
            By = By.Id("TipoCobertura_LMIAPPInvalidez_1")
        }).Wait();

        value = _orcamento.GetDoubleData("$.Coberturas.AcidentesPessoaisDespesasMedicas");
        value ??= 100;
        text = String.Format("{0:#.00}", value).Replace(".", "").Replace(",", "");
        _robot.Execute(new SetTextWithKeyDownAndBackSpace
        {
            BackSpaceNumber = 5,
            Text = text,
            By = By.Id("TipoCobertura_LMIAPPDespesaHospitalar_1")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks
        {
            By1 = By.XPath("//div[@id='PecasReposicao_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='PecasReposicao_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.Reparos.TipoPecaRepraro")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks
        {
            By1 = By.XPath("//div[@id='_TipoCobertura_Assistencia24h_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='_TipoCobertura_Assistencia24h_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.Reparos.Assistencia24h")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks
        {
            By1 = By.XPath("//div[@id='TipoCobertura_LocalReparoAssistenciaVidros_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_LocalReparoAssistenciaVidros_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.Reparos.LocalReparoVidros")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks
        {
            By1 = By.XPath("//div[@id='TipoCobertura_AssistenciaVidros_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_AssistenciaVidros_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.Reparos.CoberturaVidros")}')]"),
            DelayBetweenClicks = TimeSpan.FromSeconds(1)
        }).Wait();

        if (_orcamento.GetBoolData("$.Coberturas.Reparos.ProtecaoPequenosReparos") ?? true)
        {
            _robot.Execute(new ClickByJavascriptRequest
            {
                By = By.XPath("//input[@id='TipoCobertura_PequenosReparos_1' and @type='checkbox']"),
                DelayAfter = TimeSpan.FromMilliseconds(750)
            }).Wait();
        }

        if (_orcamento.GetBoolData("$.Coberturas.Reparos.ProtecaoRodaPneu") ?? true)
        {
            _robot.Execute(new ClickByJavascriptRequest
            {
                By = By.XPath("//input[@id='TipoCobertura_UnderCar_1' and @type='checkbox']"),
                DelayAfter = TimeSpan.FromMilliseconds(750)
            }).Wait();
        }

        if (_orcamento.GetBoolData("$.Coberturas.Reparos.DespesasExtras") ?? true)
        {
            _robot.Execute(new ClickByJavascriptRequest
            {
                By = By.XPath("//input[@id='TipoCobertura_DespesaExtra_1' and @type='checkbox']"),
                DelayAfter = TimeSpan.FromMilliseconds(750)
            }).Wait();
        }

        if (_orcamento.GetBoolData("$.Coberturas.Reparos.DespesasHigienizacao") ?? true)
        {
            _robot.Execute(new ClickByJavascriptRequest
            {
                By = By.XPath("//input[@id='TipoCobertura_DespesaHigienizacao_1' and @type='checkbox']"),
                DelayAfter = TimeSpan.FromMilliseconds(750)
            }).Wait();
        }

        if (_orcamento.GetBoolData("$.Coberturas.Reparos.ExtensaoPerimetro") ?? true)
        {
            _robot.Execute(new ClickByJavascriptRequest
            {
                By = By.XPath("//input[@id='TipoCobertura_ExpansaoPerimetroDemaisPaises_1' and @type='checkbox']"),
                DelayAfter = TimeSpan.FromMilliseconds(750)
            }).Wait();
        }

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='TipoCobertura_CarroReservaPadraoVeiculo_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_CarroReservaPadraoVeiculo_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.CarroReserva.Padrao")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='TipoCobertura_CarroReservaDescontoFranquiaPerdaParcial_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_CarroReservaDescontoFranquiaPerdaParcial_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.CarroReserva.DiasPerdaParcial")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='TipoCobertura_CarroReservaTodosEventos_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_CarroReservaTodosEventos_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.CarroReserva.DiasTodosEventos")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='TipoCobertura_CarroReservaIndenizacaoIntegral_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_CarroReservaIndenizacaoIntegral_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.CarroReserva.IndenizacaoIntegral")}')]")
        }).Wait();

        _robot.Execute(new SelectBy2Clicks()
        {
            By1 = By.XPath("//div[@id='TipoCobertura_CarroReservaTerceiros_1_chosen']//b"),
            By2 = By.XPath($"//div[@id='TipoCobertura_CarroReservaTerceiros_1_chosen']//li[contains(text(),'{_orcamento.GetStringData("$.Coberturas.CarroReserva.Terceiros")}')]")
        }).Wait();

        value = _orcamento.GetDoubleData("$.DadosCorretagem.Comissao");
        value ??= 100;
        text = String.Format("{0:#.00}", value).Replace(".", "").Replace(",", "");
        _robot.Execute(new SetTextWithKeyDownAndBackSpace
        {
            BackSpaceNumber = 5,
            Text = text,
            By = By.Id("PercentualComissaoDesejadoa")
        }).Wait();

        _robot.Execute(new ClickRequest()
        {
            By = By.Id("btnCalcular"),
            Timeout = TimeSpan.FromSeconds(1),
            DelayAfter = TimeSpan.FromSeconds(17)
        }).Wait();
    }
}