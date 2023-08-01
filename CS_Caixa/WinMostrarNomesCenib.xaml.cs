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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinMostrarNomesCenib.xaml
    /// </summary>
    public partial class WinMostrarNomesCenib : Window
    {
        List<IndiceRegistro> listaNome = new List<IndiceRegistro>();
        string nome;
        public WinMostrarNomesCenib(List<IndiceRegistro> listaNome, string nome)
        {
            this.listaNome = listaNome;
            this.nome = nome;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = listaNome.OrderBy(p => p.Nome);

            lblNome.Content = nome;
        }
    }
}
