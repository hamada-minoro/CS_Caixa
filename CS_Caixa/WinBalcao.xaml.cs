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
using CS_Caixa.Controls;
using CS_Caixa.Models;



namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinBalcao.xaml
    /// </summary>
    public partial class WinBalcao : Window
    {
        private ReciboBalcao _recibo;

        public WinBalcao()
        {
            InitializeComponent();
        }

        public WinBalcao(ReciboBalcao recibo)
        {
            _recibo = recibo;
            InitializeComponent();
        }

        public int idRecibo;
        string calcularAto = string.Empty;
        public Usuario usuarioLogado = new Usuario();
        public List<Ato> listaSelos = new List<Ato>();
        ClassAto classAto = new ClassAto();
        ValorPago valorPago = new ValorPago();
        ClassBalcao classBalcao = new ClassBalcao();
        ClassUsuario classUsuario = new ClassUsuario();
        public List<Usuario> listaUsuarios = new List<Usuario>();
        ClassMensalista classMensalista = new ClassMensalista();
        List<CadMensalista> listaMensalista = new List<CadMensalista>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            listaUsuarios = classUsuario.ListaUsuarios();

            cmbFuncionario.ItemsSource = listaUsuarios;
            cmbFuncionario.DisplayMemberPath = "NomeUsu";
            cmbFuncionario.SelectedValuePath = "Id_Usuario";

            cmbFuncionario.SelectedItem = listaUsuarios.Where(p => p.Id_Usuario == _recibo.IdUsuario).FirstOrDefault();

            listaMensalista = classMensalista.ListaMensalistas();

            cmbMensalista.ItemsSource = listaMensalista.Select(p => p.Nome);



            checkBoxPago.IsChecked = _recibo.Pago;
            
            cmbTipoCustas.Text = _recibo.TipoCustas;

            cmbTipoPagamento.Text = _recibo.TipoPagamento;

            cmbMensalista.Text = _recibo.Mensalista;

            txtRequisicao.Text = _recibo.NumeroRequisicao.ToString();

            txtDesconto.Text = string.Format("{0:n2}", _recibo.ValorDesconto);

            txtAdicionar.Text = string.Format("{0:n2}", _recibo.ValorAdicionar);


            CarregarSelos();
            CarregarDadosRecibo();
            CarregarValores();
        }

        private void CarregarSelos()
        {
            listaSelos = classBalcao.ListaSelosBalcaoRecibo(_recibo);
            dataGridSelosAdicionados.ItemsSource = listaSelos;

            CarregaQtdGrid();
        }

        private void CarregarDadosRecibo()
        {
            datePickerData.SelectedDate = _recibo.Data;
            checkBoxPago.IsChecked = _recibo.Pago;
            cmbFuncionario.SelectedItem = _recibo.Usuario;
            cmbTipoCustas.SelectedItem = _recibo.TipoCustas;
            cmbTipoPagamento.SelectedItem = _recibo.TipoPagamento;
            txtDesconto.Text = string.Format("{0:n2}", _recibo.ValorDesconto);
            txtAdicionar.Text = string.Format("{0:n2}", _recibo.ValorAdicionar);
            cmbMensalista.SelectedItem = _recibo.Mensalista;
            txtRequisicao.Text = _recibo.NumeroRequisicao.ToString();
            
        }

        private void CarregarValores()
        {
            valorPago = classAto.ObterValorPagoPorIdReciboBalcao(_recibo.IdReciboBalcao);

            txtValorPagoBoleto.Text = string.Format("{0:N2}", valorPago.Boleto);
            txtValorPagoCartaoCredito.Text = string.Format("{0:N2}", valorPago.CartaoCredito);
            txtValorPagoCheque.Text = string.Format("{0:N2}", valorPago.Cheque);
            txtValorPagoChequePre.Text = string.Format("{0:N2}", valorPago.ChequePre);
            txtValorPagoDeposito.Text = string.Format("{0:N2}", valorPago.Deposito);
            txtValorPagoDinheiro.Text = string.Format("{0:N2}", valorPago.Dinheiro);

            lblTotal.Content = string.Format("{0:N2}", valorPago.Total);
            lblTroco.Content = string.Format("{0:N2}", valorPago.Troco);
        }

        private void CarregaQtdGrid()
        {
            try
            {
                lblQtdSeloAut.Content = listaSelos.Where(p => p.TipoAto == "AUTENTICAÇÃO").Count();

                lblQtdCopia.Content = _recibo.QuantCopia;

                lblQtdSeloAbert.Content = listaSelos.Where(p => p.TipoAto == "ABERTURA DE FIRMAS").Count();

                lblQtdSeloRecAut.Content = listaSelos.Where(p => p.TipoAto == "REC AUTENTICIDADE" || p.TipoAto == "REC AUTENTICIDADE (DUT)").Count();

                lblQtdSeloRecSem.Content = listaSelos.Where(p => p.TipoAto == "REC SEMELHANÇA" || p.TipoAto == "SINAL PÚBLICO").Count();

                lblQtdSeloMaterializacao.Content = listaSelos.Where(p => p.TipoAto == "MATERIALIZAÇÃO").Count();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbTipoPagamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}