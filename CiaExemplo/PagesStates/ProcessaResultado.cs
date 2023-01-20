using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class ProcessaResultado : BaseState
{
    public ProcessaResultado(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("Processa Resultado", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        var PremioTotal = _robot.Execute(new GetElementRequest()
        {
            By = By.XPath("//table[contains(@class,'TablePremio_Franquia')]//tbody//tr[1]//label[@class='valor_premioTotalNovo']"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Result.WebElement;

        var ValorPremioTotal = Convert.ToDouble(Regex.Match(PremioTotal.Text, @"\d{1,3}(\.\d{3})*,\d{2}").Value, new CultureInfo("pt-BR"));
        _results.AddResultValue("Prêmios", "Prêmio Total", ValorPremioTotal);

        _robot.Execute(new ClickByJavascriptRequest()
        {
            By = By.Id("btnShowModalCoberturas"),
            DelayAfter = TimeSpan.FromSeconds(7)
        }).Wait();

        _robot.Execute(new ChangeFrameRequest
        {
            By = By.Id("ifrmCotacao"),
            Timeout = TimeSpan.FromSeconds(5)
        }).Wait();

        bool continua = false;
        do
        {
            var linhas = _robot.Execute(new GetElementsListRequest()
            {
                DelayBefore = TimeSpan.FromSeconds(5),
                By = By.XPath("//table[@id='DataTables_Table_0']//tbody//tr"),
            }).Result.WebElements;

            foreach (IWebElement element in linhas)
            {
                var descricao = element.FindElement(By.XPath("./td[1]")).Text;
                var valor = Convert.ToDouble(Regex.Match(element.FindElement(By.XPath("./td[2]")).Text, @"\d{1,3}(\.\d{3})*,\d{2}").Value, new CultureInfo("pt-BR"));
                _results.AddResultValue("Prêmios", descricao, valor);
            }
            var botaoNext = _robot.Execute(new GetElementRequest
            {
                By = By.Id("DataTables_Table_0_next"),
                Timeout = TimeSpan.FromSeconds(3)
            }).Result.WebElement;

            continua = !botaoNext.GetAttribute("class").Contains("disabled");
            if (continua)
            {
                botaoNext.Click();
            }
        } while (continua);
        _robot.Execute(new SwitchToDefaultFrameRequest()).Wait();

        _robot.Execute(new ClickRequest()
        {
            By = By.Id("btnConfirmar"),
            Timeout = TimeSpan.FromSeconds(3),
            DelayAfter = TimeSpan.FromSeconds(1)
        }).Wait();

        var Franquia = _robot.Execute(new GetElementRequest()
        {
            By = By.XPath("//table[contains(@class,'TablePremio_Franquia')]//tbody//tr[2]//label"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Result.WebElement;

        var ValorFranquia = Convert.ToDouble(Regex.Match(Franquia.Text, @"\d{1,3}(\.\d{3})*,\d{2}").Value, new CultureInfo("pt-BR"));
        _results.AddResultValue("Prêmios", "Franquia", ValorFranquia);

        var PremioLiquido = _robot.Execute(new GetElementRequest()
        {
            By = By.XPath("//td[@id='formaPgtoPremioLiquido']"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Result.WebElement;

        var valorPremioLiquido = Convert.ToDouble(Regex.Match(PremioLiquido.Text, @"\d{1,3}(\.\d{3})*,\d{2}").Value, new CultureInfo("pt-BR"));
        _results.AddResultValue("Prêmios", "Prêmio Líquido", valorPremioLiquido);

        var IOF = _robot.Execute(new GetElementRequest()
        {
            By = By.XPath("//td[@id='formaPgtoIof']"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Result.WebElement;
        var valorIOF = Convert.ToDouble(Regex.Match(IOF.Text, @"\d{1,3}(\.\d{3})*,\d{2}").Value, new CultureInfo("pt-BR"));

        _results.AddResultValue("Prêmios", "IOF", valorIOF);

        _robot.Execute(new ClickRequest()
        {
            By = By.Id("btnShowModalFormaPagamento"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Wait();

        string formadepagamento = "";
        string parcela = "";

        var pagamentos = _robot.Execute(new GetElementsListRequest()
        {
            By = By.XPath("//div[contains(@class,'titulo-forma-pagamento')]/div[(@class='row-parcela' or @class='label-titulo') and not (contains(@style,'display:none'))]"),
        }).Result.WebElements;

        foreach (IWebElement element in pagamentos)
        {
            if (element.GetAttribute("class") == "label-titulo")
            {
                formadepagamento = element.Text;
            }
            if (element.GetAttribute("class") == "row-parcela")
            {
                parcela = Regex.Match(element.Text, @"(.*?)R\$").Groups[1].Value;
                var valorparcela = Convert.ToDouble(Regex.Match(element.Text, @"\d{1,3}(\.\d{3})*,\d{2}").Value, new CultureInfo("pt-BR"));
                _results.AddResultValue("Forma de Pagamento", formadepagamento + " " + parcela, valorparcela);
            }
        }

        _robot.Execute(new ClickRequest()
        {
            By = By.Id("btn-close-formapagamento"),
            Timeout = TimeSpan.FromSeconds(5),
            DelayBefore = TimeSpan.FromSeconds(3)
        }).Wait();

        var alertas = _robot.Execute(new GetElementsListRequest
        {
            By = By.XPath("//div[@role='alert']")
        }).Result.WebElements;

        foreach (var alert in alertas)
        {
            _results.AddResultMessage("Mensagem [CIA]", alert.Text);
        }
    }
}