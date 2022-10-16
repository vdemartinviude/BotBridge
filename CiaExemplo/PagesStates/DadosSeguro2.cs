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
    public DadosSeguro2(Robot robot, BaseOrcamento inputdata) : base("DadosSeguro2", robot, inputdata)
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
    }
}