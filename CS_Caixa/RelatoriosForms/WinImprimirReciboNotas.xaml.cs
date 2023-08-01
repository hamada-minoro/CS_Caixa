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
    /// Interaction logic for WinImprimirReciboNotas.xaml
    /// </summary>
    public partial class WinImprimirReciboNotas : Window
    {
        Ato _ato;
        List<ItensCustasNota> _itensCustasNotas;
        ClassAto classAto = new ClassAto();
        ReciboNota _recibo;
        Parte _parte;
        string cancelado = "nao";

        ClassReciboNotas classReciboNotas = new ClassReciboNotas();
        public WinImprimirReciboNotas(Ato ato, string status)
        {
            _itensCustasNotas = classAto.CarregaItensCustasNotas(ato.Id_Ato);
            _ato = ato;

            if (status == "UTILIZADO")
                _recibo = classReciboNotas.ObterReciboNotasPorIdAto(ato.Id_Ato);

            if (status == "CANCELADO")
            {
                _recibo = classReciboNotas.ObterReciboNotasCanceladoPorIdAto(ato.Id_Ato);
                cancelado = "sim";
            }


            _parte = classReciboNotas.ObterPartesPorIdParte((int)_recibo.ApresentanteId);
            InitializeComponent();
        }

        public string FormatCPF(string sender)
        {
            string response = sender.Trim();
            if (response.Length == 11)
            {
                response = response.Insert(9, "-");
                response = response.Insert(6, ".");
                response = response.Insert(3, ".");
            }
            return response;
        }

        public string FormatCNPJ(string sender)
        {
            string response = sender.Trim();
            if (response.Length == 14)
            {
                response = response.Insert(12, "-");
                response = response.Insert(8, "/");
                response = response.Insert(5, ".");
                response = response.Insert(2, ".");
            }
            return response;
        }

        public string FormatTelefone(string sender)
        {
            string response = sender.Trim();
            if (response.Length == 10)
            {

                response = response.Insert(6, "-");
                response = response.Insert(2, ")");
                response = response.Insert(0, "(");
            }
            return response;
        }


        public string FormatCelular(string sender)
        {
            string response = sender.Trim();
            if (response.Length == 11)
            {

                response = response.Insert(7, "-");
                response = response.Insert(2, ")");
                response = response.Insert(0, "(");
            }
            return response;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            List<ReportParameter> reportParameter = new List<ReportParameter>();


            string itensCustas = string.Empty;

            string servico = string.Format("Data: {0:dd/MM/yyyy}       Prazo de entrega: {1:dd/MM/yyyy}\n", _recibo.Data.Value.Date, _recibo.DataEntrega.Value.Date);
            servico += string.Format("Natureza: {0}\n", _ato.Natureza);
            servico += string.Format("Outorgado: {0}\n", _parte.Outorgado);
            servico += (_parte.CpfOutorgado.Length == 11 ? string.Format("CPF: {0}\n", FormatCPF(_parte.CpfOutorgado)) : string.Format("CNPJ: {0}\n", FormatCNPJ(_parte.CpfOutorgado)));
            servico += string.Format("Tipo de Cobrança: {0}", _ato.TipoCobranca);



            string apresentante = string.Empty;
            apresentante += string.Format("Nome: {0}\n", _parte.Nome);
            apresentante += string.Format("End.: {0}\n", _parte.Endereco);
            apresentante += string.Format("E-mail: {0}\n", _parte.Email);
            apresentante += (_parte.Cpf.Length == 11 ? string.Format("CPF: {0}\n", FormatCPF(_parte.Cpf)) : string.Format("CNPJ: {0}\n", FormatCNPJ(_parte.Cpf)));
            apresentante += string.Format("Telefone: {0}", FormatTelefone(_parte.Telefone));
            apresentante += string.Format("                           Celular: {0}", FormatCelular(_parte.Celular));
            var custas = _ato.Emolumentos + _ato.Fetj + _ato.Fundperj + _ato.Funperj + _ato.Funarpen + _ato.Pmcmv + _ato.Iss + _ato.Mutua + _ato.Acoterj + _ato.ValorAdicionar - _ato.ValorDesconto;

            foreach (var item in _itensCustasNotas)
            {
                itensCustas += string.Format("({0}x){1} {2}.{3}.{4} R${5:N2}; ", item.Quantidade, item.Descricao, item.Tabela, item.Item, item.SubItem, item.Total);
            }


            reportParameter.Add(new ReportParameter("Recibo", string.Format("RECIBO Nº: {0}/{1}", _recibo.Recibo, _recibo.Data.Value.Year)));
            reportParameter.Add(new ReportParameter("Atribuicao", _recibo.Atribuicao));
            reportParameter.Add(new ReportParameter("Datas", string.Format("Data: {0:dd/MM/yyyy}    Prazo de entrega: {1:dd/MM/yyyy}", _recibo.Data.Value.Date, _recibo.DataEntrega.Value.Date)));
            reportParameter.Add(new ReportParameter("Natureza", string.Format("Natureza: {0}", _ato.Natureza)));
            reportParameter.Add(new ReportParameter("Outorgado", string.Format("Outorgado: {0}", _parte.Outorgado)));
            reportParameter.Add(new ReportParameter("CpfOutorgado", string.Format("CPF: {0}", FormatCPF(_parte.CpfOutorgado))));
            reportParameter.Add(new ReportParameter("TipoCobranca", string.Format("Tipo de Cobrança: {0}", _ato.TipoCobranca)));
            reportParameter.Add(new ReportParameter("NomeApresentante", string.Format("Nome: {0}", _parte.Nome)));
            reportParameter.Add(new ReportParameter("EnderecoApresentante", string.Format("Endereço: {0}", _parte.Endereco)));
            reportParameter.Add(new ReportParameter("Email", string.Format("E-mail: {0}", _parte.Email)));
            reportParameter.Add(new ReportParameter("CpfApresentante", string.Format("CPF: {0}", _parte.Cpf)));
            reportParameter.Add(new ReportParameter("Telefone", string.Format("Telefone: {0}", FormatTelefone(_parte.Telefone))));
            reportParameter.Add(new ReportParameter("Celular", string.Format("Celular: {0}", FormatCelular(_parte.Celular))));
            reportParameter.Add(new ReportParameter("Ato", string.Format("{0} x {1}", _itensCustasNotas.FirstOrDefault().Quantidade, _itensCustasNotas.FirstOrDefault().Descricao)));
            reportParameter.Add(new ReportParameter("FormaPagamento", string.Format("Forma de Pagamento: {0}", _ato.TipoPagamento)));
            reportParameter.Add(new ReportParameter("NumeroAto", string.Format("{0}", _ato.NumeroAto)));
            reportParameter.Add(new ReportParameter("Livro", string.Format("{0}", _ato.Livro)));
            reportParameter.Add(new ReportParameter("Folhas", string.Format("{0}-{1}", _ato.FolhaInical, _ato.FolhaFinal)));
            reportParameter.Add(new ReportParameter("ItensCustas", string.Format("{0}", itensCustas)));
            reportParameter.Add(new ReportParameter("Emol", string.Format("Emol.: {0:N2}", _ato.Emolumentos)));
            reportParameter.Add(new ReportParameter("FETJ", string.Format("FETJ: {0:N2}", _ato.Fetj)));
            reportParameter.Add(new ReportParameter("FUND", string.Format("FUND: {0:N2}", _ato.Fundperj)));
            reportParameter.Add(new ReportParameter("FUNP", string.Format("FUNP: {0:N2}", _ato.Funperj)));
            reportParameter.Add(new ReportParameter("FUNA", string.Format("FUNA: {0:N2}", _ato.Funarpen)));
            reportParameter.Add(new ReportParameter("PMCMV", string.Format("PMCMV: {0:N2}", _ato.Pmcmv)));
            reportParameter.Add(new ReportParameter("ISS", string.Format("ISS: {0:N2}", _ato.Iss)));
            reportParameter.Add(new ReportParameter("MutAcoterj", string.Format("Mút/Aco: {0:N2}", _ato.Mutua + _ato.Acoterj)));
            reportParameter.Add(new ReportParameter("Distribuicao", string.Format("{0:N2}", _ato.Distribuicao)));
            reportParameter.Add(new ReportParameter("Custas", string.Format("{0:N2}", custas)));
            reportParameter.Add(new ReportParameter("Indisponibilidade", string.Format("{0:N2}", _ato.Indisponibilidade)));
            reportParameter.Add(new ReportParameter("Total", string.Format("{0:N2}", _ato.Distribuicao + _ato.Indisponibilidade + custas)));
            reportParameter.Add(new ReportParameter("Pago", string.Format("{0:N2}", _ato.Distribuicao + _ato.Indisponibilidade + custas)));
            reportParameter.Add(new ReportParameter("Outros", string.Format("{0:N2}", _ato.ValorAdicionar)));
            reportParameter.Add(new ReportParameter("Escrevente", string.Format("{0:N2}", _ato.Escrevente)));
            reportParameter.Add(new ReportParameter("Cancelado", cancelado));

            reportParameter.Add(new ReportParameter("Servico", string.Format("{0}", servico)));
            reportParameter.Add(new ReportParameter("Apresentante", string.Format("{0}", apresentante)));

            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                        
            reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepReciboNotasImprimir.rdlc";

            reportViewer.LocalReport.SetParameters(reportParameter);
            reportViewer.RefreshReport();
        }
    }
}
