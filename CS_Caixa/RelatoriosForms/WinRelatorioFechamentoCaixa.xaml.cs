using CS_Caixa.Controls;
using CS_Caixa.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Lógica interna para WinRelatorioFechamentoCaixa.xaml
    /// </summary>
    public partial class WinRelatorioFechamentoCaixa : Window
    {
        List<AtosValores> _atosValores;
        List<AtosValores> _atosPagamento;
        List<Ato> _atos;
        string _tipoRelatorio;
        DateTime _dataInicio, _dataFim;
        List<Adicionar_Caixa> _valoresAdicionados;
        List<Retirada_Caixa> _valoresRetirados;
        List<Enotariado> _eNotariado;
        List<RepasseCaixa> _repasseCaixa = new List<RepasseCaixa>();
        List<RepasseCaixa> _repasseCaixa2 = new List<RepasseCaixa>();
        bool _exibirRepasse;


        public WinRelatorioFechamentoCaixa(List<AtosValores> atosValores, List<Ato> atos, string tipoRelatorio, DateTime dataInicio, DateTime dataFim, List<Adicionar_Caixa> valoresAdicionados, List<Retirada_Caixa> valoresRetirados, List<Enotariado> eNotariado, bool exibirRepasse)
        {
            _atosValores = atosValores;
            _atosPagamento = atosValores.Where(p => p.TipoAto == "PAGAMENTO").ToList();
            _atos = atos;
            _tipoRelatorio = tipoRelatorio;
            _dataInicio = dataInicio;
            _dataFim = dataFim;
            _valoresAdicionados = valoresAdicionados;
            _valoresRetirados = valoresRetirados;
            _eNotariado = eNotariado;
            _exibirRepasse = exibirRepasse;
            InitializeComponent();
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            this.Title = "";

            ClassCustasNotas classCustasNotas = new ClassCustasNotas();
            decimal copia = classCustasNotas.ValorCopia(_dataInicio.Year);


            decimal TotalDinheiro, TotalDeposito, TotalMensalista, TotalCheque, TotalChequePre, TotalBoleto, TotalGeral, TotalTroco, TotalCartaoCredito = 0M;

            decimal totalAtos = 0M;
            decimal totalAdicionados = 0M;
            decimal totalRetirados = 0M;
            decimal totalBib = 0M;
            decimal totalDist = 0M;
            decimal enotariado = 0M;

            List<Adicionar_Caixa> adicionarDinheiro, adicionarDeposito, adicionarCheque, adicionarChequePre, adicionarBoleto, adicionarCartaoCredito;

            var totalRegistroDut = _atos.Where(p => p.TipoAto == "REGISTRO (DUT)").ToList();

            decimal VadicionarDinheiro, VadicionarDeposito, VadicionarCheque, VadicionarChequePre, VadicionarBoleto, VadicionarCartaoCredito;

            if (_atosValores != null)
                totalAtos = Convert.ToDecimal(_atosValores.Sum(p => p.Total) - Convert.ToDecimal(_atosValores.Sum(p => p.ValorTroco)));

            if (_valoresAdicionados != null)
                totalAdicionados = Convert.ToDecimal(_valoresAdicionados.Sum(p => p.Valor));

            if (_valoresRetirados != null)
                totalRetirados = Convert.ToDecimal(_valoresRetirados.Sum(p => p.Valor));

            if(_eNotariado != null)
                enotariado = Convert.ToDecimal(_eNotariado.Sum(p => p.Valor));

            totalBib = Convert.ToDecimal(_atosValores.Sum(p => p.Indisponibilidade));

            totalDist = Convert.ToDecimal(_atosValores.Sum(p => p.Distribuicao));

            TotalGeral = (totalAtos + totalAdicionados) - (totalRetirados + totalBib + totalDist + enotariado);

            if (totalRegistroDut != null)
                TotalGeral = TotalGeral - Convert.ToDecimal(totalRegistroDut.Sum(p => p.Total));
            


            adicionarDinheiro = _valoresAdicionados.Where(p => p.TpPagamento == "DINHEIRO").ToList();
            adicionarDeposito = _valoresAdicionados.Where(p => p.TpPagamento == "DEPÓSITO").ToList();
            adicionarCheque = _valoresAdicionados.Where(p => p.TpPagamento == "CHEQUE").ToList();
            adicionarChequePre = _valoresAdicionados.Where(p => p.TpPagamento == "PIX BRADESCO").ToList();
            adicionarBoleto = _valoresAdicionados.Where(p => p.TpPagamento == "PIX NUBANK").ToList();
            adicionarCartaoCredito = _valoresAdicionados.Where(p => p.TpPagamento == "CARTÃO CRÉDITO").ToList();

            if (adicionarDinheiro != null)
                VadicionarDinheiro = Convert.ToDecimal(adicionarDinheiro.Sum(p => p.Valor)) - totalRetirados;
            else
                VadicionarDinheiro = 0M;

            if (adicionarDeposito != null)
                VadicionarDeposito = Convert.ToDecimal(adicionarDeposito.Sum(p => p.Valor));
            else
                VadicionarDeposito = 0M;

            if (adicionarCheque != null)
                VadicionarCheque = Convert.ToDecimal(adicionarCheque.Sum(p => p.Valor));
            else
                VadicionarCheque = 0M;

            if (adicionarChequePre != null)
                VadicionarChequePre = Convert.ToDecimal(adicionarChequePre.Sum(p => p.Valor));
            else
                VadicionarChequePre = 0M;

            if (adicionarBoleto != null)
                VadicionarBoleto = Convert.ToDecimal(adicionarBoleto.Sum(p => p.Valor));
            else
                VadicionarBoleto = 0M;

            if (adicionarCartaoCredito != null)
                VadicionarCartaoCredito = Convert.ToDecimal(adicionarCartaoCredito.Sum(p => p.Valor));
            else
                VadicionarCartaoCredito = 0M;

            if (_atosValores != null)
            {
                TotalDinheiro = (Convert.ToDecimal(_atosValores.Sum(p => p.Dinheiro)) + VadicionarDinheiro) - Convert.ToDecimal(_atosValores.Sum(p => p.ValorTroco));
                TotalDeposito = Convert.ToDecimal(_atosValores.Sum(p => p.Deposito)) + VadicionarDeposito;
                TotalMensalista = Convert.ToDecimal(_atosValores.Sum(p => p.VrMensalista));
                TotalCheque = Convert.ToDecimal(_atosValores.Sum(p => p.Cheque)) + VadicionarCheque;
                TotalChequePre = Convert.ToDecimal(_atosValores.Sum(p => p.ChequePre)) + VadicionarChequePre;
                TotalBoleto = Convert.ToDecimal(_atosValores.Sum(p => p.Boleto)) + VadicionarBoleto;
                TotalCartaoCredito = Convert.ToDecimal(_atosValores.Sum(p => p.CartaoCredito)) + VadicionarCartaoCredito;
                TotalTroco = Convert.ToDecimal(_atosValores.Sum(p => p.ValorTroco));
            }
            else
            {
                TotalDinheiro = 0M;
                TotalDeposito = 0M;
                TotalMensalista = 0M;
                TotalCheque = 0M;
                TotalChequePre = 0M;
                TotalBoleto = 0M;
                TotalCartaoCredito = 0M;
                TotalTroco = 0M;
            }

            ClassRepasseCaixa repasse = new ClassRepasseCaixa();
            if (_exibirRepasse)
            {
                _repasseCaixa = repasse.ObterTodosPorPeriodo(_dataInicio, _dataFim);
                _repasseCaixa2 = repasse.ObterTodosPorPeriodoRestante(_dataInicio, _dataFim);
            }
            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            reportParameter.Add(new ReportParameter("Periodo", string.Format("Período: {0} até {1}", _dataInicio.ToShortDateString(), _dataFim.ToShortDateString())));


            var dataSource = new ReportDataSource("DataSetAto", _atos);
            var dataSource2 = new ReportDataSource("DataSetAtosValores", _atosValores);
            var dataSource3 = new ReportDataSource("DataSetValoresRetirados", _valoresRetirados);
            var dataSource4 = new ReportDataSource("DataSetValoresAdicionados", _valoresAdicionados);
            var dataSource5 = new ReportDataSource("DataSetRepasse", _repasseCaixa);
            var dataSource6 = new ReportDataSource("DataSetRepasse2", _repasseCaixa2);

            switch (_tipoRelatorio)
            {
                case "SimplificadoFinanceiro":
                    reportParameter.Add(new ReportParameter("QtdDut", string.Format("{0}", _atos.Where(p => p.TipoAto == "REC AUTENTICIDADE (DUT)").Count())));
                    reportParameter.Add(new ReportParameter("copia", string.Format("{0}", copia)));
                    reportParameter.Add(new ReportParameter("TotalDinheiro", string.Format("{0:n2}", TotalDinheiro)));
                    reportParameter.Add(new ReportParameter("TotalDeposito", string.Format("{0:n2}", TotalDeposito)));
                    reportParameter.Add(new ReportParameter("TotalMensalista", string.Format("{0:n2}", TotalMensalista)));
                    reportParameter.Add(new ReportParameter("TotalCheque", string.Format("{0:n2}", TotalCheque)));
                    reportParameter.Add(new ReportParameter("TotalChequePre", string.Format("{0:n2}", TotalChequePre)));
                    reportParameter.Add(new ReportParameter("TotalBoleto", string.Format("{0:n2}", TotalBoleto)));
                    reportParameter.Add(new ReportParameter("TotalCartaoCredito", string.Format("{0:n2}", TotalCartaoCredito)));
                    reportParameter.Add(new ReportParameter("TotalGeral", string.Format("{0:n2}", TotalGeral)));
                    reportParameter.Add(new ReportParameter("TotalTroco", string.Format("{0:n2}", TotalTroco)));
                    reportParameter.Add(new ReportParameter("ExibirRepasse", string.Format("{0:n2}", _exibirRepasse)));
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.DataSources.Add(dataSource3);
                    ReportViewer.LocalReport.DataSources.Add(dataSource4);
                    ReportViewer.LocalReport.DataSources.Add(dataSource5);
                    ReportViewer.LocalReport.DataSources.Add(dataSource6);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaSimplificadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "SimplificadoAtosPraticados":

                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaSimplificadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "DetalhadoFinanceiro":
                    reportParameter.Add(new ReportParameter("copia", string.Format("{0}", copia)));
                    reportParameter.Add(new ReportParameter("TotalDinheiro", string.Format("{0:n2}", TotalDinheiro)));
                    reportParameter.Add(new ReportParameter("TotalDeposito", string.Format("{0:n2}", TotalDeposito)));
                    reportParameter.Add(new ReportParameter("TotalMensalista", string.Format("{0:n2}", TotalMensalista)));
                    reportParameter.Add(new ReportParameter("TotalCheque", string.Format("{0:n2}", TotalCheque)));
                    reportParameter.Add(new ReportParameter("TotalChequePre", string.Format("{0:n2}", TotalChequePre)));
                    reportParameter.Add(new ReportParameter("TotalBoleto", string.Format("{0:n2}", TotalBoleto)));
                    reportParameter.Add(new ReportParameter("TotalCartaoCredito", string.Format("{0:n2}", TotalCartaoCredito)));
                    reportParameter.Add(new ReportParameter("TotalGeral", string.Format("{0:n2}", TotalGeral)));
                    reportParameter.Add(new ReportParameter("TotalTroco", string.Format("{0:n2}", TotalTroco)));
                    reportParameter.Add(new ReportParameter("ExibirRepasse", string.Format("{0:n2}", _exibirRepasse)));

                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.DataSources.Add(dataSource3);
                    ReportViewer.LocalReport.DataSources.Add(dataSource4);
                    ReportViewer.LocalReport.DataSources.Add(dataSource5);
                    ReportViewer.LocalReport.DataSources.Add(dataSource6);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaDetalhadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "DetalhadoAtosPraticados":

                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaDetalhadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "ProtestoDetalhadoFinanceiro":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaProtestoDetalhadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "ProtestoSimplificadoFinanceiro":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaProtestoSimplificadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "ProtestoSimplificadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaProtestoSimplificadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "ProtestoDetalhadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaProtestoDetalhadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "NotasDetalhadoFinanceiro":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaNotasDetalhadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "NotasSimplificadoFinanceiro":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaNotasSimplificadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "NotasSimplificadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaNotasSimplificadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "NotasDetalhadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaNotasDetalhadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "RgiDetalhadoFinanceiro":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaRgiDetalhadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "RgiSimplificadoFinanceiro":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaRgiSimplificadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "RgiSimplificadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaRgiSimplificadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "RgiDetalhadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaRgiDetalhadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "BalcãoDetalhadoFinanceiro":
                    reportParameter.Add(new ReportParameter("copia", string.Format("{0}", copia)));
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaBalcaoDetalhadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "BalcãoSimplificadoFinanceiro":
                    reportParameter.Add(new ReportParameter("copia", string.Format("{0}", copia)));
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaBalcaoSimplificadoFinanceiro.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "BalcãoDetalhadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaBalcaoDetalhadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                case "BalcãoSimplificadoAtosPraticados":
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.DataSources.Add(dataSource2);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixaBalcaoSimplificadoAtosPraticados.rdlc";
                    ReportViewer.LocalReport.SetParameters(reportParameter);
                    break;

                default:
                    break;
            }

            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            ReportViewer.RefreshReport();
        }
    }
}
