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
    /// Lógica interna para WinConfirmaChamadaSenha.xaml
    /// </summary>
    public partial class WinConfirmaChamadaSenha : Window
    {

        WinChamarSenhas _chamadaSenhas;
        WinChamadaSenhaRecepcao _chamadaSenhasRecepcao;
        WinChamadaSenhaNotas _chamadaSenhasNotas;
        string formChamado;

        public WinConfirmaChamadaSenha(WinChamarSenhas chamadaSenhas)
        {
            _chamadaSenhas = chamadaSenhas;
            formChamado = "WinChamarSenhas";
            InitializeComponent();
        }

        public WinConfirmaChamadaSenha(WinChamadaSenhaRecepcao chamadaSenhas)
        {
            _chamadaSenhasRecepcao = chamadaSenhas;
            formChamado = "WinChamadaSenhaRecepcao";
            InitializeComponent();
        }

        public WinConfirmaChamadaSenha(WinChamadaSenhaNotas chamadaSenhas)
        {
            _chamadaSenhasNotas = chamadaSenhas;
            formChamado = "WinChamadaSenhaNotas";
            InitializeComponent();
        }


        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CancelarSenha();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (formChamado == "WinChamarSenhas")
            {
                lblSenha.Content = _chamadaSenhas.atendimento.Senha;

                if (formChamado == "WinChamadaSenhaRecepcao")
                    btnIniciar.Content = "Finalizar Atendimento";
            }
            if (formChamado == "WinChamadaSenhaRecepcao")
            {
                lblSenha.Content = _chamadaSenhasRecepcao.atendimento.Senha;

                if (formChamado == "WinChamadaSenhaRecepcao")
                    btnIniciar.Content = "Finalizar Atendimento";
            }
            if (formChamado == "WinChamadaSenhaNotas")
            {
                lblSenha.Content = _chamadaSenhasNotas.atendimento.Senha;

                if (formChamado == "WinChamadaSenhaNotas")
                    btnIniciar.Content = "Finalizar Atendimento";
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                CancelarSenha();
        }

        private void CancelarSenha()
        {
            if (MessageBox.Show("Deseja realmente CANCELAR esse atendimento?", "Cancelamento de Atendimento", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (formChamado == "WinChamarSenhas")
                {
                    _chamadaSenhas.atendimento.HoraFinalizado = DateTime.Now.ToLongTimeString();
                    _chamadaSenhas.AtualizaStatus(_chamadaSenhas.atendimento, "CANCELADO");
                    _chamadaSenhas.acaoConfirmaSenha = "cancelado";
                    _chamadaSenhas.InicializaConexao(_chamadaSenhas.atendimento.Senha + " CANCELADA");

                }
                if (formChamado == "WinChamadaSenhaRecepcao")
                {
                    _chamadaSenhasRecepcao.atendimento.HoraFinalizado = DateTime.Now.ToLongTimeString();
                    _chamadaSenhasRecepcao.AtualizaStatus(_chamadaSenhasRecepcao.atendimento, "CANCELADO");
                    _chamadaSenhasRecepcao.acaoConfirmaSenha = "cancelado";
                    _chamadaSenhasRecepcao.InicializaConexao(_chamadaSenhasRecepcao.atendimento.Senha + " CANCELADA");

                }
                if (formChamado == "WinChamadaSenhaNotas")
                {
                    _chamadaSenhasNotas.atendimento.HoraFinalizado = DateTime.Now.ToLongTimeString();
                    _chamadaSenhasNotas.AtualizaStatus(_chamadaSenhasNotas.atendimento, "CANCELADO");
                    _chamadaSenhasNotas.acaoConfirmaSenha = "cancelado";
                    _chamadaSenhasNotas.InicializaConexao(_chamadaSenhasNotas.atendimento.Senha + " CANCELADA");

                }

                this.Close();
            }
        }

        private void btnChamar_Click(object sender, RoutedEventArgs e)
        {
            if (formChamado == "WinChamarSenhas")
            _chamadaSenhas.InicializaConexao(_chamadaSenhas.senhaAtual);
            if (formChamado == "WinChamadaSenhaRecepcao")
                _chamadaSenhasRecepcao.InicializaConexao(_chamadaSenhasRecepcao.senhaAtual);
            if (formChamado == "WinChamadaSenhaNotas")
                _chamadaSenhasNotas.InicializaConexao(_chamadaSenhasNotas.senhaAtual);
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
             if (formChamado == "WinChamarSenhas")
                 _chamadaSenhas.acaoConfirmaSenha = "iniciar";
             
            if (formChamado == "WinChamadaSenhaRecepcao")
             {
                 _chamadaSenhasRecepcao.atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                 _chamadaSenhasRecepcao.atendimento.HoraFinalizado = DateTime.Now.ToLongTimeString();

                 _chamadaSenhasRecepcao.AtualizaStatus(_chamadaSenhasRecepcao.atendimento, "FINALIZADO");
                 

             }
            if (formChamado == "WinChamadaSenhaNotas")
            {
                _chamadaSenhasNotas.acaoConfirmaSenha = "iniciar";

                _chamadaSenhasNotas.atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                _chamadaSenhasNotas.atendimento.HoraFinalizado = DateTime.Now.ToLongTimeString();

                _chamadaSenhasNotas.AtualizaStatus(_chamadaSenhasNotas.atendimento, "FINALIZADO");
            }
            this.Close();
        }

        private void lblTitulo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
