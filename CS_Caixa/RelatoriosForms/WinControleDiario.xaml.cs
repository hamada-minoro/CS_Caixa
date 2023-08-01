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
using CS_Caixa.Models;
using CS_Caixa.Controls;

namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Interaction logic for WinControleDiario.xaml
    /// </summary>
    public partial class WinControleDiario : Window
    {

        ClassUsuario classUsuario = new ClassUsuario();

        List<Usuario> Usuarios = new List<Usuario>();

        Usuario usuarioLogado;

        string Titulo;
        public WinControleDiario(string Titulo, Usuario usuarioLogado)
        {
            this.Titulo = Titulo;
            this.usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Titulo;
            datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            datePickerDataFim.SelectedDate = DateTime.Now.Date;
            
            Usuarios = classUsuario.ListaUsuarios();

            cmbNomeEscrevente.ItemsSource = Usuarios;
            cmbNomeEscrevente.DisplayMemberPath = "NomeUsu";
            cmbNomeEscrevente.SelectedValuePath = "Id_Usuario";

            if (usuarioLogado.Master == false || usuarioLogado.Caixa == false)
            {
                cmbNomeEscrevente.SelectedValue = usuarioLogado.Id_Usuario;
                cmbNomeEscrevente.IsEnabled = false;
            }

        }

        private void datePickerDataInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (datePickerDataInicio.SelectedDate > DateTime.Now.Date)
            {
                datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            }

            datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;

            if (datePickerDataInicio.SelectedDate > datePickerDataFim.SelectedDate)
            {
                datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;
            }

        }

        private void datePickerDataFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate != null)
            {
                if (datePickerDataInicio.SelectedDate > datePickerDataFim.SelectedDate)
                {
                    datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

        }

        private void btnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate == null || datePickerDataFim.SelectedDate == null)
            {
                MessageBox.Show("Por favor informe a data inicial e final.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (Titulo == "Controle Diário Escritura")
            {
                FrmControleDiarioEscritura frmShow = new FrmControleDiarioEscritura(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value, (Usuario)cmbNomeEscrevente.SelectedItem);
                frmShow.ShowDialog();
                frmShow.Close();
            }

            if (Titulo == "Controle Diário Procuração")
            {
                FrmControleDiarioProcuracao frmShow = new FrmControleDiarioProcuracao(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value, (Usuario)cmbNomeEscrevente.SelectedItem);
                frmShow.ShowDialog();
                frmShow.Close();
            }
        }
    }
}
