using Json.Path;
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

public class DadosPrincipais : BaseState
{
    public DadosPrincipais(Robot robot, BaseOrcamento inputdata) : base("DadosPrincipais", robot, inputdata)
    {
    }

    public override void Execute()
    {
        var pathRenovacao = JsonPath.Parse("$.DadosRenovacao.Renovacao");
        if (_orcamento.GetStringData(pathRenovacao) == "True")

        #region ComRenovação

        {
            var pathSeguradora = JsonPath.Parse("$.DadosRenovacao.DadosApoliceAnterior.Seguradora");
            var seguradora = _orcamento.GetStringData(pathSeguradora);
            if (String.IsNullOrEmpty(seguradora))
            {
                throw new EstimateParametersException("Seguradora anterior não informada na renovação");
            }

            var clickSim = new ClickRequest()
            {
                By = By.Id("rdRenovacaoSim"),
                Timeout = TimeSpan.FromSeconds(3),
                DelayAfter = TimeSpan.FromSeconds(5)
            };
            _robot.Execute(clickSim).Wait();

            var selectSeguradora = new SelectBy2Clicks
            {
                By1 = By.XPath("//span[contains(text(),'Selecione uma opção')]/../div/b"),
                By2 = By.XPath($"//li[contains(@class,'active-result') and contains(text(),'{seguradora}')]"),
                DelayBetweenClicks = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(selectSeguradora).Wait();

            var apoliceAnterior = _orcamento.GetStringData("$.DadosRenovacao.DadosApoliceAnterior.Apolice");
            if (!String.IsNullOrEmpty(apoliceAnterior))
            {
                var digitaApolice = new SetTextRequest()
                {
                    By = By.Id("PrincipalRenovacaoNumeroApoliceAnterior"),
                    Text = apoliceAnterior
                };
                _robot.Execute(digitaApolice).Wait();
            }

            var itemAnterior = _orcamento.GetStringData("$.DadosRenovacao.DadosApoliceAnterior.Item");
            if (!String.IsNullOrEmpty(itemAnterior))
            {
                var digitaItem = new SetTextRequest()
                {
                    By = By.Id("PrincipalRenovacaoNumeroItemApoliceAnterior"),
                    Text = itemAnterior
                };
                _robot.Execute(digitaItem).Wait();
            }
        }

        #endregion ComRenovação

        #region SemRenovação

        else
        {
            var clicknao = new ClickRequest()
            {
                By = By.Id("rdRenovacaoNao"),
                Timeout = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(clicknao).Wait();
        }

        #endregion SemRenovação

        #region Segurado

        if (_orcamento.GetStringData("$.Segurado.TipoPessoa") == "Jurídica")
        {
            #region PessoaJuridica

            var clickpessoajuridica = new ClickRequest()
            {
                By = By.Id("rdSeguradoJuridica"),
                Timeout = TimeSpan.FromSeconds(1),
                DelayAfter = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(clickpessoajuridica).Wait();
            var cnpj = _orcamento.GetStringData("$.Segurado.CNPJ");
            if (String.IsNullOrEmpty(cnpj))
            {
                throw new EstimateParametersException("CNPJ da pessoa jurídica não informado");
            }
            var typecnpj = new SetTextRequest()
            {
                By = By.Id("PrincipalSeguradoCotacaoModelCodigoCodigoPessoaFisicaCgc"),
                Text = cnpj
            };
            _robot.Execute(typecnpj).Wait();
            var cpfcondutor = _orcamento.GetStringData("$.Segurado.CPFPrincipalCondutor");
            if (String.IsNullOrEmpty(cpfcondutor))
            {
                throw new EstimateParametersException("CPF Principal condutor não informado");
            }
            var typecpfcondutor = new SetTextRequest()
            {
                By = By.Id("PrincipalCpfPrincipalCondutor"),
                Text = cpfcondutor
            };
            _robot.Execute(typecpfcondutor).Wait();

            #endregion PessoaJuridica
        }
        else
        {
            #region PessoaFisica

            var clickpessoafisica = new ClickRequest()
            {
                By = By.Id("rdSeguradoFisica"),
                Timeout = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(clickpessoafisica).Wait();

            var cpf = _orcamento.GetStringData("$.Segurado.CPF");
            var typecpf = new SetTextRequest()
            {
                By = By.Id("PrincipalSeguradoCotacaoModelCodigoCodigoPessoaFisicaCgc"),
                Text = cpf
            };
            _robot.Execute(typecpf).Wait();

            if (_orcamento.GetStringData("$.Segurado.SeguradoPrincipalCondutor") == "True")
            {
                var clickprincipalcondutor = new ClickRequest()
                {
                    By = By.Id("PrincipalCotacaoSeguradoPrincipal"),
                    Timeout = TimeSpan.FromSeconds(1)
                };
                _robot.Execute(clickprincipalcondutor).Wait();
            }
            else
            {
                var typecpfcondutor = new SetTextRequest()
                {
                    By = By.Id("PrincipalCpfPrincipalCondutor"),
                    Text = _orcamento.GetStringData("$.Segurado.CPFPrincipalCondutor")
                };
                _robot.Execute(typecpfcondutor).Wait();
            }

            #endregion PessoaFisica
        }

        #endregion Segurado

        #region PlacaChassi

        if (_orcamento.GetStringData("$.Veiculo.ZeroKmSemPlaca") == "True")
        {
            var clicksemplaca = new ClickRequest()
            {
                By = By.Id("PrincipalIndicadorVeiculo0KMSemPlaca"),
                Timeout = TimeSpan.FromSeconds(1)
            };
            _robot.Execute(clicksemplaca).Wait();
        }
        else
        {
            var typeplaca = new SetTextRequest()
            {
                By = By.Id("PrincipalItemAutoCotacaoModelLicencaCodigo"),
                Text = _orcamento.GetStringData("$.Veiculo.Placa")
            };
            _robot.Execute(typeplaca).Wait();
        }
        var chassi = _orcamento.GetStringData("$.Veiculo.Chassi");
        if (!string.IsNullOrEmpty(chassi))
        {
            var typechassi = new SetTextRequest()
            {
                By = By.Id("PrincipalItemAutoCotacaoModelNumeroSerie"),
                Text = chassi
            };
            _robot.Execute(typechassi).Wait();
        }

        #endregion PlacaChassi

        var clickprosseguir = new ClickRequest()
        {
            By = By.Id("btnCotar"),
            Timeout = TimeSpan.FromSeconds(1)
        };
        _robot.Execute(clickprosseguir).Wait();
    }
}