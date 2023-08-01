using CS_Caixa.Controls;
using CS_Caixa.Models;
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
    /// Lógica interna para AgendaEletronica.xaml
    /// </summary>
    public partial class AgendaEletronica : Window
    {
        Usuario _usuarioLogado;
        List<CS_Caixa.Models.AgendaEletronica> agendaDiaSelecionado;
        CS_Caixa.Models.AgendaEletronica horaSelecionada;
        ClassAgendaEletronica classAgendaEletronica = new ClassAgendaEletronica();
        string hora = "00:00";

        public AgendaEletronica(Usuario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDate = DateTime.Now.Date;
            CarregarListaDiaSelecionado();
        }

        private void CarregarListaDiaSelecionado()
        {
            if (calendar.IsInitialized)
            {
                agendaDiaSelecionado = classAgendaEletronica.AgendasPorData(calendar.SelectedDate.Value, _usuarioLogado.Id_Usuario);
                VerificarCorHoras();
                lblDataHoraSelecionada.Content = "SELECIONE A HORA";
                lblDataHoraSelecionada.Background = Brushes.PapayaWhip;
                gridSalvar.IsEnabled = false;
            }
        }


        private void VerificarCorHoras()
        {

            if (agendaDiaSelecionado.Where(p => p.Hora == btn9hs.Content.ToString()).Count() == 1)
                btn9hs.Background = Brushes.OrangeRed;
            else
                btn9hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn9_30hs.Content.ToString()).Count() == 1)
                btn9_30hs.Background = Brushes.OrangeRed;
            else
                btn9_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn10hs.Content.ToString()).Count() == 1)
                btn10hs.Background = Brushes.OrangeRed;
            else
                btn10hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn10_30hs.Content.ToString()).Count() == 1)
                btn10_30hs.Background = Brushes.OrangeRed;
            else
                btn10_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn11hs.Content.ToString()).Count() == 1)
                btn11hs.Background = Brushes.OrangeRed;
            else
                btn11hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn11_30hs.Content.ToString()).Count() == 1)
                btn11_30hs.Background = Brushes.OrangeRed;
            else
                btn11_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn12hs.Content.ToString()).Count() == 1)
                btn12hs.Background = Brushes.OrangeRed;
            else
                btn12hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn12_30hs.Content.ToString()).Count() == 1)
                btn12_30hs.Background = Brushes.OrangeRed;
            else
                btn12_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn13hs.Content.ToString()).Count() == 1)
                btn13hs.Background = Brushes.OrangeRed;
            else
                btn13hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn13_30hs.Content.ToString()).Count() == 1)
                btn13_30hs.Background = Brushes.OrangeRed;
            else
                btn13_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn14hs.Content.ToString()).Count() == 1)
                btn14hs.Background = Brushes.OrangeRed;
            else
                btn14hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn14_30hs.Content.ToString()).Count() == 1)
                btn14_30hs.Background = Brushes.OrangeRed;
            else
                btn14_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn15hs.Content.ToString()).Count() == 1)
                btn15hs.Background = Brushes.OrangeRed;
            else
                btn15hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn15_30hs.Content.ToString()).Count() == 1)
                btn15_30hs.Background = Brushes.OrangeRed;
            else
                btn15_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn16hs.Content.ToString()).Count() == 1)
                btn16hs.Background = Brushes.OrangeRed;
            else
                btn16hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn16_30hs.Content.ToString()).Count() == 1)
                btn16_30hs.Background = Brushes.OrangeRed;
            else
                btn16_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn17hs.Content.ToString()).Count() == 1)
                btn17hs.Background = Brushes.OrangeRed;
            else
                btn17hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn17_30hs.Content.ToString()).Count() == 1)
                btn17_30hs.Background = Brushes.OrangeRed;
            else
                btn17_30hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn18hs.Content.ToString()).Count() == 1)
                btn18hs.Background = Brushes.OrangeRed;
            else
                btn18hs.Background = Brushes.Gainsboro;

            if (agendaDiaSelecionado.Where(p => p.Hora == btn18_30hs.Content.ToString()).Count() == 1)
                btn18_30hs.Background = Brushes.OrangeRed;
            else
                btn18_30hs.Background = Brushes.Gainsboro;

        }


        private void CarregarDadosHora(string hora)
        {
            horaSelecionada = agendaDiaSelecionado.Where(p => p.Hora == hora).FirstOrDefault();
            if (horaSelecionada != null)
            {
                cmbTipoAto.Text = horaSelecionada.TipoAto;
                txtNomeCliente.Text = horaSelecionada.NomeCliente;
                txtObservacao.Text = horaSelecionada.Observacao;
                
            }
            else
            {
                cmbTipoAto.Text = "";
                txtNomeCliente.Text = "";
                txtObservacao.Text = "";
                lblDataHoraSelecionada.Content = "SELECIONE A HORA";
            }
            lblDataHoraSelecionada.Background = Brushes.DarkSeaGreen;
            gridSalvar.IsEnabled = true;
            cmbTipoAto.ItemsSource = classAgendaEletronica.AgendasTipoAtoPorIdUsuario(_usuarioLogado.Id_Usuario);
            txtNomeCliente.ItemsSource = classAgendaEletronica.AgendasNomeClientePorIdUsuario(_usuarioLogado.Id_Usuario);
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            horaSelecionada = null;
            cmbTipoAto.Text = "";
            txtNomeCliente.Text = "";
            txtObservacao.Text = "";
            CarregarListaDiaSelecionado();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTipoAto.Text.Trim() == "")
            {
                MessageBox.Show("Campo 'Tipo de Ato' é obrigatório o preenchimento.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbTipoAto.Focus();
                return;
            }

            if (txtNomeCliente.Text.Trim() == "")
            {
                MessageBox.Show("Campo 'Nome do Cliente' é obrigatório o preenchimento.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNomeCliente.Focus();
                return;
            }

            Models.AgendaEletronica agenda = new Models.AgendaEletronica();
            agenda.Data = calendar.SelectedDate.Value;
            agenda.Hora = hora;
            agenda.IdUsuario = _usuarioLogado.Id_Usuario;
            agenda.NomeCliente = txtNomeCliente.Text.Trim();
            agenda.Observacao = txtObservacao.Text.Trim();
            agenda.TipoAto = cmbTipoAto.Text.ToUpper();
            agenda.Usuario = _usuarioLogado.NomeUsu;

            if (horaSelecionada == null)
                classAgendaEletronica.SalvarAgenda(agenda, "novo");
            else
            {
                agenda.IdAgenda = horaSelecionada.IdAgenda;
                classAgendaEletronica.SalvarAgenda(agenda, "alterar");
            }

            MessageBox.Show("Agendamento salvo com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            horaSelecionada = null;
            cmbTipoAto.Text = "";
            txtNomeCliente.Text = "";
            txtObservacao.Text = "";
            CarregarListaDiaSelecionado();
        }

        private void btn9hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "9:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 9:00", calendar.SelectedDate.Value);
        }

        private void btn9_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "9:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 9:30", calendar.SelectedDate.Value);
        }

        private void btn10hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "10:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 10:00", calendar.SelectedDate.Value);
        }

        private void btn10_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "10:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 10:30", calendar.SelectedDate.Value);
        }

        private void btn11hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "11:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 11:00", calendar.SelectedDate.Value);
        }

        private void btn11_30hs_Click(object sender, RoutedEventArgs e)
        {
            CarregarDadosHora("11:30");
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 11:30", calendar.SelectedDate.Value);
        }

        private void btn12hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "12:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 12:00", calendar.SelectedDate.Value);
        }

        private void btn12_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "12:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 12:30", calendar.SelectedDate.Value);
        }

        private void btn13hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "13:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 13:00", calendar.SelectedDate.Value);
        }

        private void btn13_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "13:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 13:30", calendar.SelectedDate.Value);
        }

        private void btn14hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "14:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 14:00", calendar.SelectedDate.Value);
        }

        private void btn14_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "14:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 14:30", calendar.SelectedDate.Value);
        }

        private void btn15hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "15:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 15:00", calendar.SelectedDate.Value);
        }

        private void btn15_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "15:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 15:30", calendar.SelectedDate.Value);
        }

        private void btn16hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "16:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 16:00", calendar.SelectedDate.Value);
        }

        private void btn16_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "16:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 16:30", calendar.SelectedDate.Value);
        }

        private void btn17hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "17:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 17:00", calendar.SelectedDate.Value);
        }

        private void btn17_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "17:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 17:30", calendar.SelectedDate.Value);
        }

        private void btn18hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "18:00";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 18:00", calendar.SelectedDate.Value);
        }

        private void btn18_30hs_Click(object sender, RoutedEventArgs e)
        {
            hora = "18:30";
            CarregarDadosHora(hora);
            lblDataHoraSelecionada.Content = string.Format("{0:dd/MM/yyyy} - 18:30", calendar.SelectedDate.Value);
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            if (horaSelecionada != null)
            {
                if (MessageBox.Show("Deseja realmente limpar este agendamento?", "Limpar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classAgendaEletronica.ExcluirAgenda(horaSelecionada);
                   
                    horaSelecionada = null;
                    cmbTipoAto.Text = "";
                    txtNomeCliente.Text = "";
                    txtObservacao.Text = "";
                    CarregarListaDiaSelecionado();
                }
            }
        }

        private void cmbTipoAto_LostFocus(object sender, RoutedEventArgs e)
        {
            cmbTipoAto.Text = cmbTipoAto.Text.ToUpper();
        }

        private void cmbTipoAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtNomeCliente.Focus();
        }

        private void txtNomeCliente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtObservacao.Focus();
        }

        private void txtObservacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSalvar.Focus();
        }

        private void txtNomeCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNomeCliente.Text = txtNomeCliente.Text.ToUpper();
        }


    }
}
