using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.RelatoriosForms;
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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinReciboNotasControle.xaml
    /// </summary>
    public partial class WinReciboNotasControle : Window
    {

        List<ReciboNota> reciboNotas;

        ClassReciboNotas classReciboNotas = new ClassReciboNotas();

        ReciboNota reciboSelecionado;

        Ato atoSelecionado;

        Enotariado enotariadoAlterar;

        ClassEnotariado enot = new ClassEnotariado();

        ClassAto classAto = new ClassAto();

        Parte partes;

        Usuario _usuarioLogado;

        public WinReciboNotasControle(Usuario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerInicio.SelectedDate = DateTime.Now.Date.AddDays(-15);
            datePickerFim.SelectedDate = DateTime.Now.Date;

            ConsultarRecibosNotas();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void datePickerInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicio.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicio.SelectedDate = DateTime.Now.Date;
            }

            datePickerFim.SelectedDate = datePickerInicio.SelectedDate;

            if (datePickerInicio.SelectedDate > datePickerFim.SelectedDate)
            {
                datePickerFim.SelectedDate = datePickerInicio.SelectedDate;
            }
        }

        private void datePickerFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicio.SelectedDate != null)
            {
                if (datePickerInicio.SelectedDate > datePickerFim.SelectedDate)
                {
                    datePickerFim.SelectedDate = datePickerInicio.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ConsultarRecibosNotas();
        }

        private void ConsultarRecibosNotas()
        {
            if (datePickerInicio.SelectedDate != null && datePickerFim.SelectedDate != null)
            {
                reciboNotas = classReciboNotas.ObterTodosPorDataAsNoTracking(datePickerInicio.SelectedDate.Value, datePickerFim.SelectedDate.Value);
                listViewRecibo.ItemsSource = null;
                listViewRecibo.ItemsSource = reciboNotas;
                if (reciboNotas.Count > 0)
                {
                    listViewRecibo.SelectedIndex = 0;
                    btnImprimir.IsEnabled = true;
                    btnCancelarRecibo.IsEnabled = true;
                    btnLiberarRecibo.IsEnabled = true;
                }
                else
                {
                    listViewRecibo.SelectedIndex = -1;
                    btnImprimir.IsEnabled = false;
                    btnCancelarRecibo.IsEnabled = false;
                    btnLiberarRecibo.IsEnabled = false;
                }
            }
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



        private void listViewRecibo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (listViewRecibo.SelectedItem != null)
            {
                reciboSelecionado = (ReciboNota)listViewRecibo.SelectedItem;
                atoSelecionado = classReciboNotas.ObterAtoPorIdAtoAsNoTracking(reciboSelecionado.AtoId);
                int parteId = Convert.ToInt32(reciboSelecionado.ApresentanteId);
                partes = classReciboNotas.ObterPartesPorIdParte(parteId);
                if (atoSelecionado != null)
                {
                    CarregaCampos();
                }
            }
        }

        private void CarregaCampos()
        {

            txtNomeApresentante.Text = partes.Nome;
            rbCpfApresentante.IsChecked = (partes.Cpf.Length == 11 ? true : false);
            rbCnpjApresentante.IsChecked = (partes.Cpf.Length == 14 ? true : false);
            txtCpfApresentante.Text = (partes.Cpf.Length == 11 ? string.Format("{0}\n", FormatCPF(partes.Cpf)) : string.Format("{0}\n", FormatCNPJ(partes.Cpf)));
            txtEndereçoApresentante.Text = partes.Endereco;
            txtTelefone.Text = partes.Telefone;
            txtCelular.Text = partes.Celular;
            txtEmailApresentante.Text = partes.Email;



            datePickerDataAto.SelectedDate = atoSelecionado.DataAto;
            txtLivro.Text = atoSelecionado.Livro;
            txtFlsInicial.Text = string.Format("{0:000}", atoSelecionado.FolhaInical);
            txtFlsFinal.Text = string.Format("{0:000}", atoSelecionado.FolhaFinal);


            txtAto.Text = string.Format("{0:000}", atoSelecionado.NumeroAto);
            txtLetraSelo.Text = atoSelecionado.LetraSelo;
            txtNumeroSelo.Text = string.Format("{0:00000}", atoSelecionado.NumeroSelo);
            txtAleatorio.Text = atoSelecionado.Aleatorio;
            txtNome.Text = atoSelecionado.Escrevente;
            cmbTipoCustas.Text = atoSelecionado.TipoCobranca;
            if (cmbTipoCustas.SelectedIndex <= 1)
            {
                cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;
            }
            if (cmbTipoPagamento.Text == "MENSALISTA")
            {
                cmbMensalista.Text = atoSelecionado.Mensalista;
                txtRequisicao.Text = atoSelecionado.NumeroRequisicao.ToString();
            }

            if (atoSelecionado.ValorEscrevente.ToString() != "")
                txtVrEscrevente.Text = string.Format("{0:n2}", atoSelecionado.ValorEscrevente);
            if (atoSelecionado.ValorCorretor.ToString() != "")
                txtVrCorretor.Text = string.Format("{0:n2}", atoSelecionado.ValorCorretor);
            if (atoSelecionado.ValorDesconto.ToString() != "")
                txtDesconto.Text = string.Format("{0:n2}", atoSelecionado.ValorDesconto);
            if (atoSelecionado.ValorAdicionar.ToString() != "")
                txtAdicionar.Text = string.Format("{0:n2}", atoSelecionado.ValorAdicionar);


            txtBaseCalculo.Text = atoSelecionado.Recibo.ToString();



            enotariadoAlterar = enot.ObterPorIdAto(atoSelecionado.Id_Ato);

            if (enotariadoAlterar != null)
            {
                if (enotariadoAlterar.Valor > 0)
                {
                    ckboxEnotariado.IsChecked = true;
                    txtEnotariado.IsEnabled = true;
                    txtEnotariado.Text = string.Format("{0:n2}", enotariadoAlterar.Valor);
                }
                else
                {
                    ckboxEnotariado.IsChecked = false;
                    txtEnotariado.IsEnabled = false;

                }
            }
            else
            {
                txtEnotariado.Text = "0,00";
            }

            txtNatureza.Text = atoSelecionado.Natureza;

            txtEmol.Text = string.Format("{0:n2}", atoSelecionado.Emolumentos);
            txtFetj.Text = string.Format("{0:n2}", atoSelecionado.Fetj);
            txtFundperj.Text = string.Format("{0:n2}", atoSelecionado.Fundperj);
            txtFunperj.Text = string.Format("{0:n2}", atoSelecionado.Funperj);
            txtFunarpen.Text = string.Format("{0:n2}", atoSelecionado.Funarpen);
            txtPmcmv.Text = string.Format("{0:n2}", atoSelecionado.Pmcmv);
            txtIss.Text = string.Format("{0:n2}", atoSelecionado.Iss);
            txtMutua.Text = string.Format("{0:n2}", atoSelecionado.Mutua);
            txtAcoterj.Text = string.Format("{0:n2}", atoSelecionado.Acoterj);
            txtTotal.Text = string.Format("{0:n2}", atoSelecionado.Total);


            datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

            if (atoSelecionado.QuantIndisp.ToString() != "")
                txtQtdIndisp.Text = atoSelecionado.QuantIndisp.ToString();

            if (atoSelecionado.QuantDistrib.ToString() != "")
                txtQtdExced.Text = atoSelecionado.QuantDistrib.ToString();



            var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);



            checkBoxPago.IsChecked = atoSelecionado.Pago;

            txtQtdAtos.Text = string.Format("{0}", atoSelecionado.QtdAtos);

            if (atoSelecionado.TipoAto == "ESCRITURA")
                txtFaixa.Text = atoSelecionado.Faixa;
            else
                txtFaixa.Text = atoSelecionado.Natureza;


            if (valorPago != null)
            {
                txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);

                lblTotal1.Content = string.Format("{0:n2}", valorPago.Total);
            }

        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            var imprimir = new WinImprimirReciboNotas(atoSelecionado, reciboSelecionado.Status);
            imprimir.Owner = this;
            imprimir.ShowDialog();
        }

        private void btnLiberarRecibo_Click(object sender, RoutedEventArgs e)
        {
            if (_usuarioLogado.Master == true)
            {
                if (reciboSelecionado != null)
                    if (MessageBox.Show("Deseja liberar o recibo seleciondo?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        classReciboNotas.RetornaReciboLivre(reciboSelecionado);
                        ConsultarRecibosNotas();
                    }
            }
            else
                MessageBox.Show("Apenas usuários MASTER têm permissão para liberar recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void btnCancelarRecibo_Click(object sender, RoutedEventArgs e)
        {
             if (_usuarioLogado.Master == true)
            {
            if (reciboSelecionado != null)
                if (MessageBox.Show("Deseja cancelar o recibo seleciondo?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classReciboNotas.CancelarRecibo(reciboSelecionado);
                    ConsultarRecibosNotas();
                }
            }
             else
                 MessageBox.Show("Apenas usuários MASTER têm permissão para cancelar recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
