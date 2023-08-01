using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica interna para ConfirmarChamadaSenha.xaml
    /// </summary>
    public partial class ConfirmarChamadaSenha : Window
    {
        public Senha _chamarSenha;
        List<Cadastro_Painel> _paineis = new List<Cadastro_Painel>();
        List<Cadastro_Pc> _estacoes = new List<Cadastro_Pc>();
        List<Pc_Painel> _relacoesPcPainal = new List<Pc_Painel>();
        Cadastro_Pc _meuPc = new Cadastro_Pc();
        Usuario _usuario;
        Chamada _chamada;
        BackgroundWorker workerChamarSenha;
        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        WinPrincipal _principal;
        WinLogin _login;

        List<string> errosDeChamadas = new List<string>();

        bool abortado = false;

        public ConfirmarChamadaSenha(Senha chamarSenha, List<Cadastro_Pc> estacoes
            , List<Cadastro_Painel> paineis, List<Pc_Painel> relacoesPcPainal, 
            Cadastro_Pc meuPc, Usuario usuario, Chamada chamada, WinPrincipal principal, WinLogin login)
        {
            _chamarSenha = chamarSenha;
            _estacoes = estacoes;
            _paineis = paineis;
            _relacoesPcPainal = relacoesPcPainal;
            _meuPc = meuPc;
            _usuario = usuario;
            _chamada = chamada;
            _principal = principal;
            _login = login;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            VerificaSenhaChamar(_chamarSenha);

            if (abortado == false)
            {
                if (_chamarSenha.Aleatorio_Confirmacao == null)
                {
                    txtAleatorio.Visibility = Visibility.Hidden;
                    lblCodigo.Visibility = Visibility.Hidden;
                }
                else
                    txtAleatorio.Focus();

                VerificaSenhaChamar(_chamarSenha);
                ChamarSenha();
            }
            else
                this.Close();
        }

        public void ChamarSenha()
        {
            lblTitulo.Content = "Aguarde...";
            btnChamar.IsEnabled = false;
            workerChamarSenha = new BackgroundWorker();
            workerChamarSenha.WorkerReportsProgress = true;
            workerChamarSenha.DoWork += workerChamarSenha_DoWork;
            workerChamarSenha.ProgressChanged += workerChamarSenha_ProgressChanged;
            workerChamarSenha.RunWorkerCompleted += workerChamarSenha_RunWorkerCompleted;
            workerChamarSenha.RunWorkerAsync();
        }



        void workerChamarSenha_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var item in _relacoesPcPainal)
            {
                var result = _chamada.InicializaConexaoChamarSenha(_chamarSenha, item);

                try
                {
                    Thread.Sleep(200);
                    workerChamarSenha.ReportProgress(1, result);
                }
                catch (Exception) { }


            }

            foreach (var item in _estacoes)
            {
                if(item.SetorId == _meuPc.SetorId)
                _chamada.InicializaConexaoAtualizarGridsEstacoes(item);
            }
        }



        void workerChamarSenha_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.UserState == "ok")
            {
                lblTitulo.Content = "SENHA CHAMADA COM SUCESSO!";
                lblTitulo.Foreground = Brushes.White;
                btnChamar.IsEnabled = true;
            }
            else
            {
                errosDeChamadas.Add("Erro ao tentar chamar a senha no Painel: " + e.UserState);
                lblTitulo.Content = "ERRO NO PAINEL: " + e.UserState;
                lblTitulo.Foreground = Brushes.Red;
                btnChamar.IsEnabled = true;
            }
        }

        void workerChamarSenha_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void VerificaSenhaChamar(Senha senhaAtualizar)
        {


            int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(5));

            for (int i = 0; i < tempo; i++)
            {

            }

            Senha senha = _AppServicoSenha.ObterPorId(senhaAtualizar.Senha_Id);

            if (senha.Identificador_Pc == senhaAtualizar.Identificador_Pc)
            {
                switch (_login.parametros.Qtd_Caracteres_Senha)
                {
                    case 0:
                        lblSenha.Content = string.Format("{0}{1}{2:000}", _chamarSenha.Tipo, _chamarSenha.LetraSetor, _chamarSenha.Numero_Senha);
                        break;
                    case 1:
                        lblSenha.Content = string.Format("{0}{1}{2:0000}", _chamarSenha.Tipo, _chamarSenha.LetraSetor, _chamarSenha.Numero_Senha);
                        break;
                    case 2:
                        lblSenha.Content = string.Format("{0}{1}{2:00000}", _chamarSenha.Tipo, _chamarSenha.LetraSetor, _chamarSenha.Numero_Senha);
                        break;

                    default:

                        break;
                }

            }
            else
            {
                abortado = true;
                this.Close();
                MessageBox.Show("Tentativa de chamada da mesma senha por usuários diferentes. Favor chame outra senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        public void AtualizarStatusSenha(Senha senha, string status)
        {
            senha.Status = status;
            senha.Caracter_Atendimento = _meuPc.Caracter;
            senha.Identificador_Pc = _meuPc.Identificador_Pc;
            senha.Usuario_Id = _usuario.Id_Usuario;
            senha.Nome_Usuario = _usuario.NomeUsu;

            switch (senha.Status)
            {
                case "CHAMADA":
                    senha.Hora_Chamada = DateTime.Now.ToLongTimeString();
                    senha.Sequencia_Chamada = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date).Max(p => p.Sequencia_Chamada) + 1;
                    senha.DescricaoLocalAtendimento = _meuPc.Tipo_Atendimento;
                    break;
                case "CANCELADA":
                    senha.Hora_Cancelado = DateTime.Now.ToLongTimeString();
                    break;
                case "FINALIZADA":
                    senha.Hora_Finalizado = DateTime.Now.ToLongTimeString();
                    break;
                default:
                    senha.Hora_Retirada = DateTime.Now.ToLongTimeString();
                    break;
            }

            _AppServicoSenha.Update(senha);
            _chamarSenha = senha;

        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }


        private void btnChamar_Click(object sender, RoutedEventArgs e)
        {
            AtualizarStatusSenha(_chamarSenha, "CHAMADA");
            lblTitulo.Content = "Senha Chamada";
            lblTitulo.Foreground = Brushes.White;
            ChamarSenha();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("A senha será cancelada. Deseja sair?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AtualizarStatusSenha(_chamarSenha, "CANCELADA");
                    ChamarSenha();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível concluir a operação. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    this.Close();
                }
            }
        }

        private void lblTitulo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            _chamada.acao = "finalizar";

            if (_chamarSenha.Aleatorio_Confirmacao == null)
            {


                if (_login.parametros.CadastroCliente == false)
                {
                    //if (_chamada.chamarSenha.NomeSetor.Contains("BALCÃO"))
                    //{
                        WinBalcaoNovo balcao = new WinBalcaoNovo(_chamada, _principal, this);
                        balcao.Owner = this;
                        balcao.ShowDialog();
                    //}
                        if (_chamada.acao == "finalizar")
                            AtualizarStatusSenha(_chamarSenha, "FINALIZADA");
                        else
                            AtualizarStatusSenha(_chamarSenha, "CANCELADA");
                        ChamarSenha();

                        this.Close();
                    
                }
                else
                {
                    //if (_chamada.chamarSenha.NomeSetor.Contains("BALCÃO"))
                    //{
                        WinBalcaoNovo balcao = new WinBalcaoNovo(_chamada, _principal, this);
                        balcao.Owner = this;
                        balcao.ShowDialog();
                    //}

                    if (_chamada.acao == "finalizar")
                        AtualizarStatusSenha(_chamarSenha, "FINALIZADA");
                    else
                        AtualizarStatusSenha(_chamarSenha, "CANCELADA");

                    ChamarSenha();

                    ClienteCadastro clienteCadastro = new ClienteCadastro(this);
                    clienteCadastro.Owner = this;
                    clienteCadastro.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                if (_login.parametros.CadastroCliente == false)
                {
                    if (_chamarSenha.Aleatorio_Confirmacao == txtAleatorio.Text)
                    {
                        //if (_chamada.chamarSenha.NomeSetor.Contains("BALCÃO"))
                        //{
                            WinBalcaoNovo balcao = new WinBalcaoNovo(_chamada, _principal, this);
                            balcao.Owner = this;
                            balcao.ShowDialog();
                        //}
                        if (_chamada.acao == "finalizar")
                            AtualizarStatusSenha(_chamarSenha, "FINALIZADA");
                        else
                            AtualizarStatusSenha(_chamarSenha, "CANCELADA");

                        ChamarSenha();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Número aleatório informado não confere.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtAleatorio.Text = "";
                        txtAleatorio.Focus();
                    }
                }
                else
                {
                    if (_chamarSenha.Aleatorio_Confirmacao == txtAleatorio.Text)
                    {
                        //if (_chamada.chamarSenha.NomeSetor.Contains("BALCÃO"))
                        //{
                            WinBalcaoNovo balcao = new WinBalcaoNovo(_chamada, _principal, this);
                            balcao.Owner = this;
                            balcao.ShowDialog();
                        //}

                        if (_chamada.acao == "finalizar")
                            AtualizarStatusSenha(_chamarSenha, "FINALIZADA");
                        else
                            AtualizarStatusSenha(_chamarSenha, "CANCELADA");

                        ChamarSenha();

                        ClienteCadastro clienteCadastro = new ClienteCadastro(this);
                        clienteCadastro.Owner = this;
                        clienteCadastro.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Número aleatório informado não confere.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtAleatorio.Text = "";
                        txtAleatorio.Focus();
                    }
                }
            }
        }

        private void txtAleatorio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnFinalizar_Click(sender, e);
            }

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }


    }
}
