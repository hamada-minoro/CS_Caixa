using CS_Caixa.Controls;
using CS_Caixa.Objetos_de_Valor;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica interna para WinCadastroSite.xaml
    /// </summary>
    public partial class WinCadastroSite : Window
    {
        public WinCadastroSite()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.IsEnabled = true;
            groupBox1.IsEnabled = false;
            groupBoxConsulta.IsEnabled = true;
            btnSalvar.IsEnabled = false;

            ConsultaCadastroNaoConfirmado("");
        }


        private void ConsultaCadastroNaoConfirmado(string email)
        {
            List<UsuariosSite> usuarios = new List<UsuariosSite>();
            MySqlCommand cmd;

            MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
            con.Open();


            if (email != "")
                cmd = new MySqlCommand("select * FROM AspNetUsers where ConfirmacaoPresencial = false and Ativo = true and Email = '" + email + "'", con);
            else
                cmd = new MySqlCommand("select * FROM AspNetUsers where ConfirmacaoPresencial = false and Ativo = true", con);

            cmd.CommandType = CommandType.Text;

            MySqlDataReader dr = cmd.ExecuteReader();


            UsuariosSite usuarioSite;

            while (dr.Read())
            {
                usuarioSite = new UsuariosSite();

                usuarioSite.Id = dr["Id"].ToString();

                usuarioSite.DataCadastro = Convert.ToDateTime(dr["DataCadastro"]);

                usuarioSite.Email = dr["Email"].ToString();

                usuarios.Add(usuarioSite);

            }

            con.Close();


            dataGrid1.ItemsSource = usuarios;
            dataGrid1.Items.Refresh();


        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex > -1)
            {
                dataGrid1.IsEnabled = false;
                groupBox1.IsEnabled = true;
                groupBoxConsulta.IsEnabled = false;
                btnSalvar.IsEnabled = true;
            }
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid1.SelectedItem != null)
                {

                    if (MessageBox.Show("Deseja realmente excluir este usuário?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return;
                    }

                    UsuariosSite usuario = (UsuariosSite)dataGrid1.SelectedItem;

                    using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                    {

                        MySqlCommand cmd = new MySqlCommand("update AspNetUsers set Ativo = false WHERE id = '" + usuario.Id + "'", con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    ConsultaCadastroNaoConfirmado("");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void groupBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ConsultaCadastroNaoConfirmado(txtConsulta.Text);
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (ValidaCpfCnpj.ValidaCPF(txtDocumento.Text) == false && ValidaCpfCnpj.ValidaCNPJ(txtDocumento.Text) == false)
                {
                    MessageBox.Show("Documento digitado não é válido.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                if (txtNome.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Informe o Nome.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                UsuariosSite usuario = (UsuariosSite)dataGrid1.SelectedItem;

                 using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                {

                    MySqlCommand cmd = new MySqlCommand("update AspNetUsers set Nome = @Nome, Documento = @Documento, ReceberNotificacao = @ReceberNotificacao, TipoPessoa = @TipoPessoa, ConfirmacaoPresencial = true WHERE id = '" + usuario.Id + "'", con);

                    cmd.Parameters.Add(new MySqlParameter("@Nome", txtNome.Text));
                    cmd.Parameters.Add(new MySqlParameter("@Documento", txtDocumento.Text));
                    cmd.Parameters.Add(new MySqlParameter("@ReceberNotificacao", ckbReceberNotificacao.IsChecked));
                    cmd.Parameters.Add(new MySqlParameter("@TipoPessoa", cmbTipo.Text));
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            WinAguardeEnviarSite enviar = new WinAguardeEnviarSite(txtDocumento.Text);
            enviar.Owner = this;
            enviar.ShowDialog();

            txtDocumento.Text = "";
            cmbTipo.SelectedIndex = -1;
            ckbReceberNotificacao.IsChecked = false;
            txtNome.Text = "";

            ConsultaCadastroNaoConfirmado("");

            dataGrid1.IsEnabled = true;
            groupBox1.IsEnabled = false;
            groupBoxConsulta.IsEnabled = true;
            btnSalvar.IsEnabled = false;
            txtDocumento.Background = Brushes.White;
        }

        private void txtDocumento_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtDocumento_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDocumento.Text.Length == 11 || txtDocumento.Text.Length == 14)
            {
                if (txtDocumento.Text.Length == 11)
                {
                    if (ValidaCpfCnpj.ValidaCPF(txtDocumento.Text) == true)
                    {
                        txtDocumento.Background = Brushes.White;
                        cmbTipo.SelectedIndex = 0;
                    }
                    else
                    {
                        txtDocumento.Background = Brushes.OrangeRed;                       
                    }
                }


                if (txtDocumento.Text.Length == 14)
                {
                    if (ValidaCpfCnpj.ValidaCNPJ(txtDocumento.Text) == true)
                    {
                        txtDocumento.Background = Brushes.White;
                        cmbTipo.SelectedIndex = 1;
                    }
                    else
                    {
                        txtDocumento.Background = Brushes.OrangeRed;
                     
                    }
                }                
            }
            else
            {
                txtDocumento.Background = Brushes.OrangeRed;
                
            }
            
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (btnSalvar.IsEnabled == true)
                {
                    txtDocumento.Text = "";
                    cmbTipo.SelectedIndex = -1;
                    ckbReceberNotificacao.IsChecked = false;
                    txtNome.Text = "";

                    ConsultaCadastroNaoConfirmado("");

                    dataGrid1.IsEnabled = true;
                    groupBox1.IsEnabled = false;
                    groupBoxConsulta.IsEnabled = true;
                    btnSalvar.IsEnabled = false;
                    txtDocumento.Background = Brushes.White;
                }
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.Items.Count > 0)
                menu.Visibility = Visibility.Visible;
            else
                menu.Visibility = Visibility.Hidden;
        }
    }
}
