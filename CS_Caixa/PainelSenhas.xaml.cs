using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para PainelSenhas.xaml
    /// </summary>
    public partial class PainelSenhas : Window
    {

        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioPc_Painel _AppServicoPc_Painel = new RepositorioPc_Painel();
        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();
        RepositorioMensagem _IAppServicoMensagem = new RepositorioMensagem();

        List<Cadastro_Painel> paineis = new List<Cadastro_Painel>();
        ClassServidor mainServidor;
        public Parametro parametros = new Parametro();
        private delegate void AtualizaStatusCallback(string strMensagem);
        string _idMaquina;
        List<Pc_Painel> relacoesPcPainal = new List<Pc_Painel>();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer dispatcherTimer2 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer3 = new DispatcherTimer();
        Cadastro_Painel meuPc = new Cadastro_Painel();
        List<Senha> senhas = new List<Senha>();
        List<Senha> senhasArmazenadas = new List<Senha>();
        List<Senha> senhasCanceladas = new List<Senha>();
        string formatoSenha = "{0}{1:00000}";
        List<VoiceInfo> listaVozes = new List<VoiceInfo>();
        bool chamarSenha = true;
        bool passarSenha = true;
        Senha senha = new Senha();
        Senha senhaVerificarCancelada = new Senha();
        List<Mensagem> mensagens = new List<Mensagem>();
        Mensagem mensagem = new Mensagem();

        string mensagemAtual;
        Senha cancelada;
        BackgroundWorker worker;

        BackgroundWorker worker2;

        BackgroundWorker worker3;

        BackgroundWorker worker4;

        Senha senhaNova;

        Senha senhaRemoreCancelada;

        Senha senhaArmazenar;

        bool atualizarParametros = false;

        string expediente = string.Empty;

        public PainelSenhas(string idMaquina)
        {
            _idMaquina = idMaquina;
            InitializeComponent();
        }

         private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                this.Cursor = Cursors.None;

                CarregamentoInicial();

                AtualizarSenhas();

                // Analisa o endereço IP do servidor informado no textbox
                IPAddress enderecoIP = IPAddress.Parse(meuPc.Ip_Pc);

                // Cria uma nova instância do objeto ChatServidor
                mainServidor = new ClassServidor(enderecoIP, meuPc.Porta_Pc);

                // Vincula o tratamento de evento StatusChanged a mainServer_StatusChanged
                ClassServidor.StatusChanged += new StatusChangedEventHandler(mainServidor_StatusChanged);

                // Inicia o atendimento das conexões
                mainServidor.IniciaAtendimento();


                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();


                dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);
                dispatcherTimer2.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer2.Start();


                dispatcherTimer3.Tick += new EventHandler(dispatcherTimer3_Tick);
                dispatcherTimer3.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer3.Start();


                AtualizarSenhas();
            }
            catch (Exception)
            {

            }
        }

        public void mainServidor_StatusChanged(object sender, ClassStatusChangedEventArgs e)
        {


            if (e.EventMessage == "Parametros")
            {
                atualizarParametros = true;
            }
            else
            {
                senhaNova = _AppServicoSenha.ObterPorId(Convert.ToInt32(e.EventMessage));

                senhaRemoreCancelada = senhasCanceladas.Where(p => p.Senha_Id == senhaNova.Senha_Id).FirstOrDefault();

                if (senhaRemoreCancelada != null)
                    senhasCanceladas.Remove(senhaRemoreCancelada);

                if (senhaNova.Status == "CHAMADA")
                    senhas.Add(senhaNova);

                if (senhaNova.Status == "CANCELADA")
                {
                    senhasCanceladas.Add(senhaNova);

                    //foreach (var item in senhas)
                    //{
                    //    if (item.Senha_Id == senhaNova.Senha_Id)
                    //        senhas.Remove(item);
                    //}

                }
            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (parametros.Mostrar_Hora == true)
                lblhora.Content = DateTime.Now.ToLongTimeString();
            else
                lblhora.Content = "";


            if (parametros.InicioFimExpediente == true)
            {
                if (parametros.HoraInicioExpediente == DateTime.Now.ToLongTimeString())
                {
                    if (parametros.Falar_Senha == true)
                    {
                        expediente = "inicio";

                        worker4 = new BackgroundWorker();
                        worker4.WorkerReportsProgress = true;
                        worker4.DoWork += worker4_DoWork;
                        worker4.ProgressChanged += worker4_ProgressChanged;
                        worker4.RunWorkerCompleted += worker4_RunWorkerCompleted;
                        worker4.RunWorkerAsync();
                    }
                    
                }

                if (parametros.HoraFimExpediente == DateTime.Now.ToLongTimeString())
                {
                    if (parametros.Falar_Senha == true)
                    {
                        expediente = "fim";

                        worker4 = new BackgroundWorker();
                        worker4.WorkerReportsProgress = true;
                        worker4.DoWork += worker4_DoWork;
                        worker4.ProgressChanged += worker4_ProgressChanged;
                        worker4.RunWorkerCompleted += worker4_RunWorkerCompleted;
                        worker4.RunWorkerAsync();
                    }
                    
                }

                if (parametros.DesligarPainel == true)
                {
                    if (parametros.HoraDesligarPainel == DateTime.Now.ToLongTimeString())
                    {
                        Process.Start("Shutdown", "-s -f -t 00");
                    }
                }
                
            }
            AtualizarSenhas();
        }

        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            if (senhas.Count() > 0)
            {
                if (chamarSenha == true)
                {
                    chamarSenha = false;

                    senha = senhas.OrderBy(P => P.Sequencia_Chamada).FirstOrDefault();

                    senhaVerificarCancelada = senhasCanceladas.Where(p => p.Senha_Id == senha.Senha_Id).FirstOrDefault();
                    if (senhaVerificarCancelada == null)
                    {                       

                        ChamarSenha(senha);
                    }
                    else
                    {

                        senhaArmazenar = senhasArmazenadas.Where(p => p.Senha_Id == senha.Senha_Id).FirstOrDefault();

                        if (senhaArmazenar != null)
                        {
                            senhasArmazenadas.Remove(senhaArmazenar);
                        }

                        senhasArmazenadas.Add(senha);

                        AtualizarSenhas();

                        chamarSenha = true;
                    }

                    senhas.Remove(senha);
                }
            }

            if (atualizarParametros == true)
            {
                atualizarParametros = false;
                CarregarAtualizacoes();
                AtualizarSenhas();
            }
        }

        private void dispatcherTimer3_Tick(object sender, EventArgs e)
        {
            if (parametros.Passar_Mensagem == true)
            {
                if (passarSenha == true)
                {
                    passarSenha = false;
                    try
                    {
                        worker3 = new BackgroundWorker();
                        worker3.WorkerReportsProgress = true;
                        worker3.DoWork += worker3_DoWork;
                        worker3.ProgressChanged += worker3_ProgressChanged;
                        worker3.RunWorkerCompleted += worker3_RunWorkerCompleted;
                        worker3.RunWorkerAsync();

                    }
                    catch (Exception)
                    {
                        passarSenha = true;
                    }
                }
            }
        }


        private void ChamarSenha(Senha senha)
        {
            try
            {
                senhaArmazenar = senhasArmazenadas.Where(p => p.Senha_Id == senha.Senha_Id).FirstOrDefault();

                if (senhaArmazenar != null)
                {
                    senhasArmazenadas.Remove(senhaArmazenar);
                }

                senhasArmazenadas.Add(senha);

                AtualizarSenhas();
                if (parametros.Bip_Aviso == true)
                {
                    string path = @"\\SERVIDOR\CS_Sistemas\CS_Caixa\Resources";

                    DirectoryInfo Dir = new DirectoryInfo(path);
                    FileInfo[] Files = Dir.GetFiles();
                    foreach (FileInfo File in Files)
                    {
                        path = File.FullName;
                    }


                    using (SoundPlayer sound = new SoundPlayer(path))
                    {
                        sound.Play();
                    }

                }

                ChangeLabelColor();
            }
            catch (Exception) { }
        }

        private void FalarSenha()
        {
            try
            {
                worker2 = new BackgroundWorker();
                worker2.WorkerReportsProgress = true;
                worker2.DoWork += worker2_DoWork;
                worker2.ProgressChanged += worker2_ProgressChanged;
                worker2.RunWorkerCompleted += worker2_RunWorkerCompleted;
                worker2.RunWorkerAsync();

            }
            catch (Exception)
            {
            }

        }

        private void ChangeLabelColor()
        {
            try
            {

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();

            }
            catch (Exception)
            {
            }
        }


        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempo = 100;


            if (senha.SenhaTipo == 1)
                tempo = 80;

            if (senha.SenhaTipo == 2)
                tempo = 90;

            for (int i = 0; i <= tempo; i++)
            {
                Thread.Sleep(200);
                worker.ReportProgress(i);
            }
        }


        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


            if (e.ProgressPercentage == 17)
            {
                if (parametros.Falar_Senha == true)
                {
                    FalarSenha();
                }
            }


            if (lblSenhaChamada.Foreground == Brushes.Red)
            {
                lblSenhaChamada.Foreground = Brushes.White;
                lblCaracterChamada.Foreground = Brushes.White;
                lblPreferencial.Foreground = Brushes.White;
                lblEspecial.Foreground = Brushes.White;
                lblNomeSetor.Foreground = Brushes.White;
            }
            else
            {
                lblSenhaChamada.Foreground = Brushes.Red;
                lblCaracterChamada.Foreground = Brushes.Red;
                lblPreferencial.Foreground = Brushes.Red;
                lblEspecial.Foreground = Brushes.Red;
                lblNomeSetor.Foreground = Brushes.Red;
            }



        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblSenhaChamada.Foreground = Brushes.White;
            lblCaracterChamada.Foreground = Brushes.White;
            lblPreferencial.Foreground = Brushes.White;
            lblEspecial.Foreground = Brushes.White;
            lblNomeSetor.Foreground = Brushes.White;
            chamarSenha = true;
        }



        private void worker2_DoWork(object sender, DoWorkEventArgs e)
        {



            string voz = senha.Voz;


            ClassFalarTexto.FalarTexto("SENHA.", listaVozes, voz);

           

            if (senha.SetorId == -1)
            {
                ClassFalarTexto.FalarTexto(senha.Tipo, listaVozes, voz);

                var novaSenha = string.Format(formatoSenha, senha.Tipo, senha.Numero_Senha);

                var qtd = novaSenha.Length - 1;


                switch (qtd)
                {

                    case 3:
                        var falarSenha = string.Format("{0}... {1}... {2}...", novaSenha.Substring(1, 1), novaSenha.Substring(2, 1), novaSenha.Substring(3, 1));
                        ClassFalarTexto.FalarTexto(falarSenha, listaVozes, voz);
                        break;

                    case 4:
                        var falarSenha1 = string.Format("{0}... {1}... {2}...{3}...", novaSenha.Substring(1, 1), novaSenha.Substring(2, 1), novaSenha.Substring(3, 1), novaSenha.Substring(4, 1));
                        ClassFalarTexto.FalarTexto(falarSenha1, listaVozes, voz);
                        break;

                    case 5:
                        var falarSenha2 = string.Format("{0}... {1}... {2}...{3}...{4}...", novaSenha.Substring(1, 1), novaSenha.Substring(2, 1), novaSenha.Substring(3, 1), novaSenha.Substring(4, 1), novaSenha.Substring(5, 1));
                        ClassFalarTexto.FalarTexto(falarSenha2, listaVozes, voz);
                        break;
                    default:
                         var falarSenha3 = string.Format("{0}... {1}... {2}...", novaSenha.Substring(1, 1), novaSenha.Substring(2, 1), novaSenha.Substring(3, 1));
                        ClassFalarTexto.FalarTexto(falarSenha3, listaVozes, voz);
                        break;
                }
            }
            else
            {

                ClassFalarTexto.FalarTexto(string.Format("{0}... {1}...", senha.Tipo, senha.LetraSetor), listaVozes, voz);

                var novaSenha = string.Format(formatoSenha, senha.Tipo + senha.LetraSetor, senha.Numero_Senha);

                var qtd = novaSenha.Length - 2;


                switch (qtd)
                {

                    case 3:
                        var falarSenha = string.Format("{0}... {1}... {2}...", novaSenha.Substring(2, 1), novaSenha.Substring(3, 1), novaSenha.Substring(4, 1));
                        ClassFalarTexto.FalarTexto(falarSenha, listaVozes, voz);
                        break;

                    case 4:
                        var falarSenha1 = string.Format("{0}... {1}... {2}...{3}...", novaSenha.Substring(2, 1), novaSenha.Substring(3, 1), novaSenha.Substring(4, 1), novaSenha.Substring(5, 1));
                        ClassFalarTexto.FalarTexto(falarSenha1, listaVozes, voz);
                        break;

                    case 5:
                        var falarSenha2 = string.Format("{0}... {1}... {2}...{3}...{4}...", novaSenha.Substring(2, 1), novaSenha.Substring(3, 1), novaSenha.Substring(4, 1), novaSenha.Substring(5, 1), novaSenha.Substring(6, 1));
                        ClassFalarTexto.FalarTexto(falarSenha2, listaVozes, voz);
                        break;
                    default:
                        var falarSenha3 = string.Format("{0}... {1}... {2}...", novaSenha.Substring(1, 1), novaSenha.Substring(2, 1), novaSenha.Substring(3, 1));
                        ClassFalarTexto.FalarTexto(falarSenha3, listaVozes, voz);
                        break;
                }
            }



            if (senha.SetorId > -1)
            {
                if (senha.FalaOutros.ToString() != "")
                    ClassFalarTexto.FalarTexto(string.Format("{0}...", senha.FalaOutros), listaVozes, voz);
                else
                    ClassFalarTexto.FalarTexto(string.Format("{0} - {1}. {2}.", senha.DescricaoLocalAtendimento, senha.Caracter_Atendimento, senha.NomeSetor), listaVozes, voz);
            }
            else
            {
                if (senha.FalaOutros.ToString() != "")
                    ClassFalarTexto.FalarTexto(string.Format("{0}...", senha.FalaOutros), listaVozes, voz);
                else
                    ClassFalarTexto.FalarTexto(string.Format("{0} - {1}...", senha.DescricaoLocalAtendimento, senha.Caracter_Atendimento), listaVozes, voz);
            }


          
            if (senha.SenhaTipo == 2)
                ClassFalarTexto.FalarTexto("SENHA PREFERENCIAL.", listaVozes, voz);

            if (senha.SenhaTipo == 3)
                ClassFalarTexto.FalarTexto("SENHA PREFERENCIAL ESPECIAL.", listaVozes, voz);

            


        }

        private void worker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


        }

        private void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void worker4_DoWork(object sender, DoWorkEventArgs e)
        {
            if(expediente == "inicio")
            ClassFalarTexto.FalarTexto(parametros.MensagemInicioExpediente, listaVozes, parametros.Voz_Botao_1);
            else
            ClassFalarTexto.FalarTexto(parametros.MensagemFimExpediente, listaVozes, parametros.Voz_Botao_1);
        }

        private void worker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


        }

        private void worker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }


        private void worker3_DoWork(object sender, DoWorkEventArgs e)
        {
            mensagem = SelecionarMensagem();

            if (mensagem != null)
            {
                mensagemAtual = "                                                                                                                               " + mensagem.Texto + "                                 ";

                int length = mensagemAtual.Length;


                for (int i = 0; i < length; i++)
                {
                    Thread.Sleep(200);
                    worker3.ReportProgress(i);
                }

            }

        }

        private void worker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (mensagem.Cor == 0)
                txtMensagem.Foreground = Brushes.Red;
            if (mensagem.Cor == 1)
                txtMensagem.Foreground = Brushes.Yellow;
            if (mensagem.Cor == 2)
                txtMensagem.Foreground = Brushes.Blue;
            if (mensagem.Cor == 3)
                txtMensagem.Foreground = Brushes.White;

            if (mensagem.Pisca == true)
            {
                if (txtMensagem.Text == "")
                {
                    string c = mensagemAtual.Substring(0, 2);
                    mensagemAtual = mensagemAtual.Remove(0, 2) + c;
                    txtMensagem.Text = mensagemAtual;
                }
                else
                {
                    txtMensagem.Text = "";
                }
            }
            else
            {
                string c = mensagemAtual.Substring(0, 1);
                mensagemAtual = mensagemAtual.Remove(0, 1) + c;
                txtMensagem.Text = mensagemAtual;
            }
        }

        private void worker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            passarSenha = true;
        }


        private Mensagem SelecionarMensagem()
        {
            if (mensagens.Count == 0)
                mensagens = _IAppServicoMensagem.ObterTodos().ToList();

            mensagem = mensagens.FirstOrDefault();

            mensagens.Remove(mensagem);

            return mensagem;
        }

        
        private void AtualizarSenhas()
        {
            try
            {

                senhasArmazenadas = senhasArmazenadas.OrderByDescending(p => p.Sequencia_Chamada).ToList();

                for (int i = 0; i <= 6; i++)
                {
                    cancelada = senhasCanceladas.Where(p => p.Senha_Id == senhasArmazenadas[i].Senha_Id).FirstOrDefault();


                    if (i == 0)
                    {
                        if (senha.Senha_Id == 0)
                        {
                            lblSenhaChamada.Content = string.Format(formatoSenha, senhasArmazenadas[i].Tipo + senhasArmazenadas[i].LetraSetor, senhasArmazenadas[i].Numero_Senha);
                            lblNomeLocal.Content = senhasArmazenadas[i].DescricaoLocalAtendimento;
                            lblCaracterChamada.Content = senhasArmazenadas[i].Caracter_Atendimento;
                            lblNomeSetor.Content = senhasArmazenadas[i].NomeSetor;

                            switch (senhasArmazenadas[i].SenhaTipo)
                            {
                                case 1:
                                    lblPreferencial.Content = "";
                                    lblEspecial.Content = "";
                                    break;
                                case 2:
                                    lblPreferencial.Content = "PREFERENCIAL";
                                    lblEspecial.Content = "";
                                    break;
                                case 3:
                                    lblPreferencial.Content = "PREFERENCIAL";
                                    lblEspecial.Content = "ESPECIAL";
                                    break;

                                default:

                                    break;
                            }
                        }
                        else
                        {
                            lblSenhaChamada.Content = string.Format(formatoSenha, senha.Tipo + senha.LetraSetor, senha.Numero_Senha);
                            lblNomeLocal.Content = senha.DescricaoLocalAtendimento;
                            lblCaracterChamada.Content = senha.Caracter_Atendimento;
                            lblNomeSetor.Content = senhasArmazenadas[i].NomeSetor;

                            switch (senha.SenhaTipo)
                            {
                                case 1:
                                    lblPreferencial.Content = "";
                                    lblEspecial.Content = "";
                                    break;
                                case 2:
                                    lblPreferencial.Content = "PREFERENCIAL";
                                    lblEspecial.Content = "";
                                    break;
                                case 3:
                                    lblPreferencial.Content = "PREFERENCIAL";
                                    lblEspecial.Content = "ESPECIAL";
                                    break;

                                default:

                                    break;
                            }
                        }




                    }
                    else if (i == 1)
                    {

                        lblHistoricoSenha1.Content = string.Format(formatoSenha, senhasArmazenadas[i].Tipo + senhasArmazenadas[i].LetraSetor, senhasArmazenadas[i].Numero_Senha);
                        lblHistoricoLocal1.Content = string.Format("{0} {1}", senhasArmazenadas[i].DescricaoLocalAtendimento, senhasArmazenadas[i].Caracter_Atendimento);
                        lblHistoricoHora1.Content = senhasArmazenadas[i].Hora_Chamada;
                        

                        if (cancelada != null)
                        {
                            lblHistoricoSenha1.Foreground = Brushes.Red;
                            lblHistoricoLocal1.Foreground = Brushes.Red;
                            lblHistoricoHora1.Foreground = Brushes.Red;
                        }
                        else
                        {
                            lblHistoricoSenha1.Foreground = Brushes.White;
                            lblHistoricoLocal1.Foreground = Brushes.White;
                            lblHistoricoHora1.Foreground = Brushes.White;
                        }

                    }
                    else if (i == 2)
                    {

                        lblHistoricoSenha2.Content = string.Format(formatoSenha, senhasArmazenadas[i].Tipo + senhasArmazenadas[i].LetraSetor, senhasArmazenadas[i].Numero_Senha);
                        lblHistoricoLocal2.Content = string.Format("{0} {1}", senhasArmazenadas[i].DescricaoLocalAtendimento, senhasArmazenadas[i].Caracter_Atendimento);
                        lblHistoricoHora2.Content = senhasArmazenadas[i].Hora_Chamada;


                        if (cancelada != null)
                        {
                            lblHistoricoSenha2.Foreground = Brushes.Red;
                            lblHistoricoLocal2.Foreground = Brushes.Red;
                            lblHistoricoHora2.Foreground = Brushes.Red;
                        }
                        else
                        {
                            lblHistoricoSenha2.Foreground = Brushes.White;
                            lblHistoricoLocal2.Foreground = Brushes.White;
                            lblHistoricoHora2.Foreground = Brushes.White;
                        }
                    }
                    else if (i == 3)
                    {

                        lblHistoricoSenha3.Content = string.Format(formatoSenha, senhasArmazenadas[i].Tipo + senhasArmazenadas[i].LetraSetor, senhasArmazenadas[i].Numero_Senha);
                        lblHistoricoLocal3.Content = string.Format("{0} {1}", senhasArmazenadas[i].DescricaoLocalAtendimento, senhasArmazenadas[i].Caracter_Atendimento);
                        lblHistoricoHora3.Content = senhasArmazenadas[i].Hora_Chamada;

                        if (cancelada != null)
                        {
                            lblHistoricoSenha3.Foreground = Brushes.Red;
                            lblHistoricoLocal3.Foreground = Brushes.Red;
                            lblHistoricoHora3.Foreground = Brushes.Red;
                        }
                        else
                        {
                            lblHistoricoSenha3.Foreground = Brushes.White;
                            lblHistoricoLocal3.Foreground = Brushes.White;
                            lblHistoricoHora3.Foreground = Brushes.White;
                        }
                    }
                    else if (i == 4)
                    {

                        lblHistoricoSenha4.Content = string.Format(formatoSenha, senhasArmazenadas[i].Tipo + senhasArmazenadas[i].LetraSetor, senhasArmazenadas[i].Numero_Senha);
                        lblHistoricoLocal4.Content = string.Format("{0} {1}", senhasArmazenadas[i].DescricaoLocalAtendimento, senhasArmazenadas[i].Caracter_Atendimento);
                        lblHistoricoHora4.Content = senhasArmazenadas[i].Hora_Chamada;

                        if (cancelada != null)
                        {
                            lblHistoricoSenha4.Foreground = Brushes.Red;
                            lblHistoricoLocal4.Foreground = Brushes.Red;
                            lblHistoricoHora4.Foreground = Brushes.Red;
                        }
                        else
                        {
                            lblHistoricoSenha4.Foreground = Brushes.White;
                            lblHistoricoLocal4.Foreground = Brushes.White;
                            lblHistoricoHora4.Foreground = Brushes.White;
                        }
                    }
                    else if (i == 5)
                    {

                        lblHistoricoSenha5.Content = string.Format(formatoSenha, senhasArmazenadas[i].Tipo + senhasArmazenadas[i].LetraSetor, senhasArmazenadas[i].Numero_Senha);
                        lblHistoricoLocal5.Content = string.Format("{0} {1}", senhasArmazenadas[i].DescricaoLocalAtendimento, senhasArmazenadas[i].Caracter_Atendimento);
                        lblHistoricoHora5.Content = senhasArmazenadas[i].Hora_Chamada;

                        if (cancelada != null)
                        {
                            lblHistoricoSenha5.Foreground = Brushes.Red;
                            lblHistoricoLocal5.Foreground = Brushes.Red;
                            lblHistoricoHora5.Foreground = Brushes.Red;
                        }
                        else
                        {
                            lblHistoricoSenha5.Foreground = Brushes.White;
                            lblHistoricoLocal5.Foreground = Brushes.White;
                            lblHistoricoHora5.Foreground = Brushes.White;
                        }
                    }
                }

            }
            catch (Exception) { }
        }


        public void CarregamentoInicial()
        {

            lblSenhaChamada.Content = "";
            lblNomeLocal.Content = "";
            lblCaracterChamada.Content = "";
            lblPreferencial.Content = "";
            lblEspecial.Content = "";
            lblHistoricoHora1.Content = "";
            lblHistoricoHora2.Content = "";
            lblHistoricoHora3.Content = "";
            lblHistoricoHora4.Content = "";
            lblHistoricoHora5.Content = "";
            lblHistoricoLocal1.Content = "";
            lblHistoricoLocal2.Content = "";
            lblHistoricoLocal3.Content = "";
            lblHistoricoLocal4.Content = "";
            lblHistoricoLocal5.Content = "";
            lblHistoricoSenha1.Content = "";
            lblHistoricoSenha2.Content = "";
            lblHistoricoSenha3.Content = "";
            lblHistoricoSenha4.Content = "";
            lblHistoricoSenha5.Content = "";
            lblhora.Content = "";


            parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();

            lblSaudacao.Content = parametros.Saudacao;

            relacoesPcPainal = _AppServicoPc_Painel.ObterTodos().ToList();
            paineis = _AppServicoCadastro_Painel.ObterTodos().ToList();
            meuPc = paineis.Where(p => p.Identificador_Pc == _idMaquina).FirstOrDefault();
            lblIp.Content = string.Format("Nome: {0} - IP: {1}", meuPc.Nome_Pc, meuPc.Ip_Pc);
            senhasArmazenadas = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date).Where(p => p.Hora_Chamada != null).ToList();
            senhasCanceladas = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date).Where(p => p.Status == "CANCELADA").ToList();
            listaVozes = ClassFalarTexto.CarregaComboVozes();

            mensagens = _IAppServicoMensagem.ObterTodos().ToList();

            switch (parametros.Qtd_Caracteres_Senha)
            {
                case 0:
                    formatoSenha = "{0}{1:000}";
                    break;

                case 1:
                    formatoSenha = "{0}{1:0000}";
                    break;

                case 2:
                    formatoSenha = "{0}{1:00000}";
                    break;
                default:
                    break;
            }

        }

        private void CarregarAtualizacoes()
        {
            parametros = new Parametro();
            parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();

            lblSaudacao.Content = parametros.Saudacao;

            relacoesPcPainal = _AppServicoPc_Painel.ObterTodos().ToList();
            paineis = _AppServicoCadastro_Painel.ObterTodos().ToList();
            meuPc = paineis.Where(p => p.Identificador_Pc == _idMaquina).FirstOrDefault();
            lblIp.Content = string.Format("Nome: {0} - IP: {1}", meuPc.Nome_Pc, meuPc.Ip_Pc);
            senhasArmazenadas = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date).Where(p => p.Hora_Chamada != null).ToList();
            senhasCanceladas = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date).Where(p => p.Status == "CANCELADA").ToList();
            listaVozes = ClassFalarTexto.CarregaComboVozes();

            mensagens = _IAppServicoMensagem.ObterTodos().ToList();

            switch (parametros.Qtd_Caracteres_Senha)
            {
                case 0:
                    formatoSenha = "{0}{1:000}";
                    break;

                case 1:
                    formatoSenha = "{0}{1:0000}";
                    break;

                case 2:
                    formatoSenha = "{0}{1:00000}";
                    break;
                default:
                    break;
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(Environment.ExitCode);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }



    }
}

