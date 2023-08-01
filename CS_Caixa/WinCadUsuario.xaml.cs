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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinCadUsuario.xaml
    /// </summary>
    public partial class WinCadUsuario : Window
    {
        Usuario usuarioLogado = new Usuario();
        ClassUsuario classUsuario = new ClassUsuario();
        List<Usuario> listaUsuarios = new List<Usuario>();
        string acao;
        public WinCadUsuario(Usuario usuarioLogado)
        {
            this.usuarioLogado = usuarioLogado;
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listaUsuarios = classUsuario.ListaUsuarios();
            cmbLogin.ItemsSource = listaUsuarios.Select(p => p.NomeUsu);
            cmbLogin.SelectedItem = usuarioLogado.NomeUsu;
            PreencheCampos();
            groupBoxCadUsuario.IsEnabled = false;

            if (usuarioLogado.Adm == false)
            {
                cmbLogin.IsEnabled = false;
                btnAdicionar.IsEnabled = false;
                btnExcluir.IsEnabled = false;
            }
            else
            {
                cmbLogin.IsEnabled = true;
                btnAdicionar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }
        }

        private void PreencheCampos()
        {
            try
            {
                

                txtNome.Text = listaUsuarios[cmbLogin.SelectedIndex].NomeUsu;
                txtSenha.Password = listaUsuarios[cmbLogin.SelectedIndex].Senha;
                checkBoxUsuarioMaster.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Master;
                checkBoxCaixa.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Caixa;
                checkBoxAlterarAtos.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].AlterarAtos;
                checkBoxExcluirAtos.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].ExcluirAtos;
                checkBoxProtesto.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Protesto;
                checkBoxRgi.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Rgi;
                checkBoxNotas.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Notas;
                checkBoxBalcao.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Balcao;
                checkBoxImprimirMatricula.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].ImprimirMatricula;
                checkBoxAdm.IsChecked = listaUsuarios[cmbLogin.SelectedIndex].Adm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreencheCampos();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            acao = "adicionar";
            groupBoxCadUsuario.IsEnabled = true;
            txtNome.Text = "";
            txtSenha.Password = "";
            checkBoxUsuarioMaster.IsChecked = false;
            checkBoxCaixa.IsChecked = false;
            checkBoxAlterarAtos.IsChecked = false;
            checkBoxExcluirAtos.IsChecked = false;
            checkBoxProtesto.IsChecked = false;
            checkBoxRgi.IsChecked = false;
            checkBoxNotas.IsChecked = false;
            checkBoxBalcao.IsChecked = false;
            checkBoxImprimirMatricula.IsChecked = false;
            checkBoxAdm.IsChecked = false;
            txtNome.Focus();
            groupBoxUsuario.IsEnabled = false;
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            acao = "alterar";
            
            groupBoxCadUsuario.IsEnabled = true;
            txtSenha.Password = ClassCriptografia.Decrypt(txtSenha.Password);
            groupBoxUsuario.IsEnabled = false;
            if (usuarioLogado.Adm == false)
            {
                groupBoxPermissoes.IsEnabled = false;
            }
            else
            {
                groupBoxPermissoes.IsEnabled = true;
            }
            txtNome.Focus();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listaUsuarios.Count <= 1)
                {
                    MessageBox.Show("Não é permitido excluir todos os usuário. É necessário conter ao menos um usuário.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (MessageBox.Show("Deseja realmente excluir o usuario " + cmbLogin.Text + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bool ok = classUsuario.ExcluirUsuario(listaUsuarios[cmbLogin.SelectedIndex].Id_Usuario);

                    if (ok)
                    {
                        MessageBox.Show("Usuário Excluído com sucesso!", "Excluído", MessageBoxButton.OK, MessageBoxImage.Information);
                       
                        listaUsuarios.RemoveAt(cmbLogin.SelectedIndex);


                        listaUsuarios = listaUsuarios.OrderBy(p => p.NomeUsu).ToList();

                        cmbLogin.SelectedItem = listaUsuarios.Select(p => p.NomeUsu).FirstOrDefault();

                        cmbLogin.ItemsSource = listaUsuarios.Select(p => p.NomeUsu);
                    }


                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado, nâo foi possível excluir o registro. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdicionar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAdicionar.Content = "Adicionar";
        }

        private void btnAdicionar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAdicionar.Content = "";
        }

        private void btnAlterar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAlterar.Content = "";
        }

        private void btnAlterar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAlterar.Content = "Alterar";
        }

        private void btnExcluir_MouseEnter(object sender, MouseEventArgs e)
        {
            btnExcluir.Content = "Excluir";
        }

        private void btnExcluir_MouseLeave(object sender, MouseEventArgs e)
        {
            btnExcluir.Content = "";
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            groupBoxCadUsuario.IsEnabled = false;
            groupBoxUsuario.IsEnabled = true;
            PreencheCampos();
        }

        private void btnCancelar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCancelar.Content = "Cancelar";
        }

        private void btnCancelar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCancelar.Content = "";
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNome.Text == "" || txtSenha.Password == "")
            {
                MessageBox.Show("Informe o nome e a senha.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (checkBoxAlterarAtos.IsChecked == false && checkBoxBalcao.IsChecked == false && checkBoxCaixa.IsChecked == false && checkBoxExcluirAtos.IsChecked == false && checkBoxNotas.IsChecked == false && checkBoxProtesto.IsChecked == false && checkBoxRgi.IsChecked == false) 
            {
                MessageBox.Show("Marque pelo menos uma permissão.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            int index = cmbLogin.SelectedIndex;
            bool ok = false;
            if (acao == "alterar")
            {
                
                listaUsuarios[cmbLogin.SelectedIndex].NomeUsu = txtNome.Text.Trim();
                listaUsuarios[cmbLogin.SelectedIndex].Senha = ClassCriptografia.Encrypt(txtSenha.Password);
                listaUsuarios[cmbLogin.SelectedIndex].Master = checkBoxUsuarioMaster.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].Caixa = checkBoxCaixa.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].AlterarAtos = checkBoxAlterarAtos.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].ExcluirAtos = checkBoxExcluirAtos.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].Protesto = checkBoxProtesto.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].Rgi = checkBoxRgi.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].Notas = checkBoxNotas.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].Balcao = checkBoxBalcao.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].ImprimirMatricula = checkBoxImprimirMatricula.IsChecked;
                listaUsuarios[cmbLogin.SelectedIndex].Adm = checkBoxAdm.IsChecked;
                ok = classUsuario.SalvarUsuario((Usuario)listaUsuarios[cmbLogin.SelectedIndex], acao);

                if (usuarioLogado.Id_Usuario == listaUsuarios[cmbLogin.SelectedIndex].Id_Usuario)
                {
                    usuarioLogado.NomeUsu = txtNome.Text;
                    usuarioLogado.Senha = ClassCriptografia.Encrypt(txtSenha.Password);
                    usuarioLogado.Master = checkBoxUsuarioMaster.IsChecked;
                    usuarioLogado.Caixa = checkBoxCaixa.IsChecked;
                    usuarioLogado.AlterarAtos = checkBoxAlterarAtos.IsChecked;
                    usuarioLogado.ExcluirAtos = checkBoxExcluirAtos.IsChecked;
                    usuarioLogado.Protesto = checkBoxProtesto.IsChecked;
                    usuarioLogado.Rgi = checkBoxRgi.IsChecked;
                    usuarioLogado.Notas = checkBoxNotas.IsChecked;
                    usuarioLogado.Balcao = checkBoxBalcao.IsChecked;
                    usuarioLogado.ImprimirMatricula = checkBoxImprimirMatricula.IsChecked;
                    usuarioLogado.Adm = checkBoxAdm.IsChecked;
                }

                if (ok)
                {
                    MessageBox.Show("Usuário alterado com sucesso!", "Alterado", MessageBoxButton.OK, MessageBoxImage.Information);
                   
                    listaUsuarios = new List<Usuario>();

                    listaUsuarios = classUsuario.ListaUsuarios();


                    listaUsuarios = listaUsuarios.OrderBy(p => p.NomeUsu).ToList();

                    cmbLogin.SelectedItem = txtNome.Text;

                    cmbLogin.ItemsSource = listaUsuarios.Select(p => p.NomeUsu);
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro inesperado ao tentar alterar o registro, favor informar ao responsável pelo sistema.", "Erro", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            
            if (acao == "adicionar")
            {

                Usuario usuarioNovo = new Usuario();
                usuarioNovo.Id_Usuario = classUsuario.ProximoIdDisponivel();
                usuarioNovo.NomeUsu = txtNome.Text.Trim();
                usuarioNovo.Senha = ClassCriptografia.Encrypt(txtSenha.Password);
                usuarioNovo.Master = checkBoxUsuarioMaster.IsChecked;
                usuarioNovo.Caixa = checkBoxCaixa.IsChecked;
                usuarioNovo.AlterarAtos = checkBoxAlterarAtos.IsChecked;
                usuarioNovo.ExcluirAtos = checkBoxExcluirAtos.IsChecked;
                usuarioNovo.Protesto = checkBoxProtesto.IsChecked;
                usuarioNovo.Rgi = checkBoxRgi.IsChecked;
                usuarioNovo.Notas = checkBoxNotas.IsChecked;
                usuarioNovo.Balcao = checkBoxBalcao.IsChecked;
                usuarioNovo.ImprimirMatricula = checkBoxImprimirMatricula.IsChecked;
                usuarioNovo.Adm = checkBoxAdm.IsChecked;
                ok = classUsuario.SalvarUsuario(usuarioNovo, acao);

                if (ok)
                {
                    MessageBox.Show("Usuário Salvo com sucesso!", "Alterado", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    listaUsuarios.Add(usuarioNovo);

                    listaUsuarios = listaUsuarios.OrderBy(p => p.NomeUsu).ToList();

                    cmbLogin.ItemsSource = listaUsuarios.Select(p => p.NomeUsu);

                    cmbLogin.SelectedItem = usuarioNovo.NomeUsu;
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro inesperado ao tentar salvar o registro, favor informar ao responsável pelo sistema.", "Erro", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                
            }

            groupBoxCadUsuario.IsEnabled = false;
            groupBoxUsuario.IsEnabled = true;

            
            PreencheCampos();
        }

        private void btnSalvar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSalvar.Content = "Salvar";
        }

        private void btnSalvar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnSalvar.Content = "";
        }

        private void checkBoxUsuarioMaster_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxUsuarioMaster.IsChecked == true)
            {
                checkBoxAlterarAtos.IsChecked = true;
                checkBoxBalcao.IsChecked = true;
                checkBoxCaixa.IsChecked = true;
                checkBoxExcluirAtos.IsChecked = true;
                checkBoxNotas.IsChecked = true;
                checkBoxProtesto.IsChecked = true;
                checkBoxRgi.IsChecked = true;
                checkBoxImprimirMatricula.IsChecked = true;

                checkBoxAlterarAtos.IsEnabled = false;
                checkBoxBalcao.IsEnabled = false;
                checkBoxCaixa.IsEnabled = false;
                checkBoxExcluirAtos.IsEnabled = false;
                checkBoxNotas.IsEnabled = false;
                checkBoxProtesto.IsEnabled = false;
                checkBoxRgi.IsEnabled = false;
                checkBoxImprimirMatricula.IsEnabled = false;
            }
            else
            {
                checkBoxAlterarAtos.IsEnabled = true;
                checkBoxBalcao.IsEnabled = true;
                checkBoxCaixa.IsEnabled = true;
                checkBoxExcluirAtos.IsEnabled = true;
                checkBoxNotas.IsEnabled = true;
                checkBoxProtesto.IsEnabled = true;
                checkBoxRgi.IsEnabled = true;
                checkBoxImprimirMatricula.IsEnabled = true;
            }
        }
    }
}
