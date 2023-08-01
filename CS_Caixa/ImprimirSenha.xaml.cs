
using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
using System.Runtime.InteropServices;
using System.Drawing;
using Xceed.Words.NET;


namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para ImprimirSenha.xaml
    /// </summary>
    public partial class ImprimirSenha : Window
    {

        RepositorioMP2032 _AppServicoMP2032 = new RepositorioMP2032();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();

        RepositorioSenha _repositorioSenha = new RepositorioSenha();

        Senha NumeroSenha = new Senha();
        BackgroundWorker worker;
        string _chamarMetodo;
        int _setor;
        string caminho;
        RetiradaSenha _retiradaSenha;

        public ImprimirSenha(string chamarMetodo, RetiradaSenha retiradaSenha, int setor)
        {
            _chamarMetodo = chamarMetodo;
            _retiradaSenha = retiradaSenha;
            _setor = setor;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Activate();

                _retiradaSenha.maquinasEstacao = _AppServicoCadastro_Pc.ObterTodos().Where(p => p.Tipo_Entrada == 1).ToList();

                SelecionarImpressora();

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Ocorreu um erro ao tentar obter as configurações. Favor imformar ao Suporte.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }



        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (_chamarMetodo)
            {

                case "SenhaPrioridade80":
                    SenhaPrioridade80();
                    break;
                case "SenhaPrioridade":
                    SenhaPrioridade();
                    break;
                case "SenhaNormal":
                    SenhaNormal();
                    break;

                default:
                    break;
            }

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        TextRange range;
        void LoadRTFPackage(string _fileName)
        {

            Alterar();
            
            FileStream fStream;
            if (File.Exists(_fileName))
            {
                range = new TextRange(rtf.Document.ContentStart, rtf.Document.ContentEnd);

                using (fStream = new FileStream(_fileName, FileMode.OpenOrCreate))
                {
                    range.Load(fStream, DataFormats.Rtf);
                }                   
               
                fStream.Close();
            }

        }
        void SaveXamlPackage(string _fileName)
        {
            TextRange range;
            FileStream fStream;
            range = new TextRange(rtf.Document.ContentStart, rtf.Document.ContentEnd);
            fStream = new FileStream(_fileName, FileMode.Create);
            range.Save(fStream, DataFormats.Rtf);
            fStream.Close();
        }


        [DllImport("shell32.dll", EntryPoint = "ShellExecute")]
        public static extern int ShellExecuteA(int hwnd, string lpOperation,
              string lpFile, string lpParameters, string lpDirectory, int nShowCmd);


        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (NumeroSenha.Data.ToString() != "01/01/0001")
                {
                    caminho = @"\\Servidor\c\Cartorio\CS_Sistemas\CS_Caixa\Senha\";



                   
                    LoadRTFPackage(caminho);


                    ImprimirSenhaRetirada();
                    //CarregarDocumento();

                    //SaveRTFPackage(caminho + NumeroSenha.Senha_Id + ".rtf");

                    //ImprimirSenha.ShellExecuteA(0, "print", caminho + NumeroSenha.Senha_Id + ".rtf", null, null, 0);
                    
                    //File.Copy(caminho + NumeroSenha.Senha_Id + ".rtf", @"\\RETIRADASENHA\" + impressora);

                    //using (var printDocument = new System.Drawing.Printing.PrintDocument())
                    //{
                    //    //printDocument.PrintPage += printDocument_PrintPage;
                    //    printDocument.PrinterSettings.PrinterName = impressora;
                    //    printDocument.Print();
                    //}

                }
            }
            catch (Exception)
            {
                
                throw;
            }
           

            this.Close();
        }

        string impressora = string.Empty;
        List<string> impressoras = new List<string>();
        void SelecionarImpressora()
        {
            
            foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                impressoras.Add(printer.ToString());
            }

            impressora = impressoras.Where(p => p.Contains("pdf") || p.Contains("PDF")).FirstOrDefault();
        }

        void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var printDocument = sender as System.Drawing.Printing.PrintDocument;



            if (printDocument != null)
            {
                using (var font = new System.Drawing.Font("Consolas", 11, System.Drawing.FontStyle.Bold))
                using (var brush = new SolidBrush(System.Drawing.Color.Black))
                {
                    e.Graphics.DrawString(
                        range.Text,
                        font,
                        brush,
                        new RectangleF(0, 0, printDocument.DefaultPageSettings.PrintableArea.Width, printDocument.DefaultPageSettings.PrintableArea.Height));
                }
            }
        }

        private void ImprimirSenhaRetirada()
        {

            //FileInfo file = new FileInfo(caminho);

            //byte[] t = File.ReadAllBytes(caminho);
            //string filename = caminho;
            //File.WriteAllBytes(filename, t);

            ////Process process = new Process();
            //System.Diagnostics.Process objP = new System.Diagnostics.Process();
            //objP.StartInfo.FileName = filename;

            ////objP.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //Hide the window.
            //objP.StartInfo.Verb = "print";
            //objP.StartInfo.CreateNoWindow = true;
            //objP.Start();

            //objP.CloseMainWindow();
            //objP.Close();

            //FileInfo arquivoRemover = new FileInfo(filename);
            //if (arquivoRemover.Exists)
            //    arquivoRemover.Delete();
            
            
            
            //try
            //{



            //    Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            //    Microsoft.Office.Interop.Word.Document document = application.Documents.Open(caminho);
            //    document.Activate();
            //    document.PrintOut();
            //    document.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    FileInfo arquivoRemover = new FileInfo(caminho);
            //    if (arquivoRemover.Exists)
            //        arquivoRemover.Delete();

            //}



            try
            {
                //conteudoTxt = File.ReadAllText(caminho);
                               

                //txtSenha.Text = txtSenha.Text.Replace("#senha", String.Format("{0} {1:000}", NumeroSenha.Tipo, Convert.ToInt16(NumeroSenha.Numero_Senha)));
                //txtSenha.Text = txtSenha.Text.Replace("#data", NumeroSenha.Data.ToShortDateString() + " - " + NumeroSenha.Hora_Retirada);
                //impressaoTextBox.Text = conteudoTxt;



                using (var printDocument = new System.Drawing.Printing.PrintDocument())
                {
                    printDocument.PrintPage += printDocument_PrintPage;
                    printDocument.PrinterSettings.PrinterName = impressora;
                    printDocument.Print();
                }
            }
            catch (Exception)
            {

                throw;
            }
                

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }


        private void SenhaPrioridade()
        {


            //int IretornoConf = _AppServicoMP2032.ConfiguraModeloImpressora();

            //if (IretornoConf != 1)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //    return;
            //}

            //int IretornoPorta = _AppServicoMP2032.IniciaPorta("USB");

            //if (IretornoPorta != 1)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //    return;
            //}


            //int IRetornoStatus = _AppServicoMP2032.Le_Status();

            //_AppServicoMP2032.FechaPorta();

            //if (IRetornoStatus == 5)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ImpressoraPoucoPapel(_retiradaSenha.parametros.Voz_Botao_1);
            //}

            //if (IRetornoStatus != 24 && IRetornoStatus != 5)
            //{

            //    if (IRetornoStatus == 32)
            //    {
            //        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //            ImpressoraSemPapel(_retiradaSenha.parametros.Voz_Botao_1);
            //        int Iretorno = _AppServicoMP2032.FechaPorta();
            //    }
            //    if (IRetornoStatus == 0)
            //    {
            //        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //            ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //        int Iretorno = _AppServicoMP2032.FechaPorta();
            //    }
            //}
            //else
            //{

                try
                {
                    int senha = 0;
                    string tipoSenha = _retiradaSenha.parametros.Letra_Botao_2;
                    int senhatipo = 2;
                    string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                    string status = "EM ESPERA";

                    int numSequecia = 0;

                    if (_retiradaSenha.parametros.Tipo_Senha == 0)
                        senha = _repositorioSenha.OberProximaSenha(_retiradaSenha.parametros.ZerarSenhaDiaSeguinte, _setor, senhatipo, out numSequecia, _retiradaSenha.parametros.Qtd_Caracteres_Senha);
                    else
                    {
                        do
                        {
                            senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(_retiradaSenha.parametros.Qtd_Caracteres_Senha + 3));
                        } while (_retiradaSenha.senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);
                    }

                    NumeroSenha = new Senha();

                    NumeroSenha.Data = DateTime.Now.Date;

                    if (_retiradaSenha.parametros.Utilizar_Aleatorio == true)
                    {
                        do
                        {
                            NumeroSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(_retiradaSenha.parametros.Qtd_Caracteres_Senha + 3);
                        }
                        while (NumeroSenha.Aleatorio_Confirmacao == senha.ToString() || NumeroSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                    }

                    NumeroSenha.Hora_Retirada = HoraRetiradaSenha;
                    NumeroSenha.Numero_Senha = senha;
                    NumeroSenha.Tipo = tipoSenha;
                    NumeroSenha.SenhaTipo = senhatipo;
                    NumeroSenha.Status = status;
                    NumeroSenha.SetorId = _setor;


                    if (_retiradaSenha.parametros.ZerarSenhaDiaSeguinte == true)
                        NumeroSenha.ModoSequencial = false;
                    else
                        NumeroSenha.ModoSequencial = true;

                    NumeroSenha.QtdCaracteres = _retiradaSenha.parametros.Qtd_Caracteres_Senha;

                    NumeroSenha.NumeroSequencia = numSequecia;




                    switch (_setor)
                    {
                        case -1:

                            switch (NumeroSenha.SenhaTipo)
                            {
                                case 1:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_1;
                                    break;

                                case 2:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_2;
                                    break;

                                case 3:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_3;
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case 0:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_1;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_1;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_1;
                            break;

                        case 1:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_2;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_2;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_2;
                            break;

                        case 2:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_3;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_3;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_3;
                            break;

                        case 3:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_4;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_4;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_4;
                            break;

                        default:
                            break;
                    }

                    //int IretornoFormata;

                    //string comandoPularLinha = "\r\n";



                    //IretornoFormata = _AppServicoMP2032.FormataTX(_retiradaSenha.parametros.Nome_Empresa + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);


                    //string imprimirSenha = "";

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:000}      ", tipoSenha, senha);

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:0000}     ", tipoSenha, senha);

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:00000}    ", tipoSenha, senha);

                    //IretornoFormata = _AppServicoMP2032.FormataTX(imprimirSenha + comandoPularLinha, 3, 0, 1, 1, 1);

                    //IretornoFormata = _AppServicoMP2032.FormataTX("       " + DateTime.Now.ToString() + comandoPularLinha + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    //int IretornoGuilhot = _AppServicoMP2032.AcionaGuilhotina(1);

                    //if (IretornoGuilhot != 1)
                    //{
                    //    MessageBox.Show("Erro na Guilhotina.");
                    //    this.Close();
                    //}

                    //int Iretorno = _AppServicoMP2032.FechaPorta();

                    //if (Iretorno != 1)
                    //{
                    //    MessageBox.Show("Erro ao fechar a porta.");
                    //    this.Close();
                    //}

                    _AppServicoSenha.Adicionar(NumeroSenha);

                    _retiradaSenha.EnviarEstacoes();

                    try
                    {
                        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                            ClassFalarTexto.FalarTexto("Retire sua senha.", _retiradaSenha.listaVozes, _retiradaSenha.parametros.Voz_Botao_2);
                    }
                    catch (Exception) { }

                }
                catch (Exception)
                {
                    try
                    {
                        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                            ClassFalarTexto.FalarTexto("Não foi possível imprimir a senha.", _retiradaSenha.listaVozes, _retiradaSenha.parametros.Voz_Botao_2);
                        else
                            MessageBox.Show("Não foi possível imprimir a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception) { }

                }
            //}

        }

        private void SenhaNormal()
        {


            //int IretornoConf = _AppServicoMP2032.ConfiguraModeloImpressora();

            //if (IretornoConf != 1)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //    return;
            //}

            //int IretornoPorta = _AppServicoMP2032.IniciaPorta("USB");

            //if (IretornoPorta != 1)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //    return;
            //}


            //int IRetornoStatus = _AppServicoMP2032.Le_Status();

            //_AppServicoMP2032.FechaPorta();

            //if (IRetornoStatus == 5)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ImpressoraPoucoPapel(_retiradaSenha.parametros.Voz_Botao_1);
            //}

            //if (IRetornoStatus != 24 && IRetornoStatus != 5)
            //{

            //    if (IRetornoStatus == 32)
            //    {
            //        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //            ImpressoraSemPapel(_retiradaSenha.parametros.Voz_Botao_1);
            //        int Iretorno = _AppServicoMP2032.FechaPorta();
            //    }
            //    if (IRetornoStatus == 0)
            //    {
            //        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //            ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //        int Iretorno = _AppServicoMP2032.FechaPorta();
            //    }
            //}
            //else
            //{

                try
                {
                    int senha = 0;
                    string tipoSenha = _retiradaSenha.parametros.Letra_Botao_1;
                    int senhatipo = 1;
                    string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                    string status = "EM ESPERA";

                    int numSequecia = 0;

                    if (_retiradaSenha.parametros.Tipo_Senha == 0)
                        senha = _repositorioSenha.OberProximaSenha(_retiradaSenha.parametros.ZerarSenhaDiaSeguinte, _setor, senhatipo, out numSequecia, _retiradaSenha.parametros.Qtd_Caracteres_Senha);
                    else
                    {
                        do
                        {
                            senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(_retiradaSenha.parametros.Qtd_Caracteres_Senha + 3));
                        } while (_retiradaSenha.senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);

                    }

                    NumeroSenha = new Senha();

                    NumeroSenha.Data = DateTime.Now.Date;


                    if (_retiradaSenha.parametros.Utilizar_Aleatorio == true)
                    {
                        do
                        {
                            NumeroSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(_retiradaSenha.parametros.Qtd_Caracteres_Senha + 3);
                        }
                        while (NumeroSenha.Aleatorio_Confirmacao == senha.ToString() || NumeroSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                    }

                    NumeroSenha.Hora_Retirada = HoraRetiradaSenha;
                    NumeroSenha.Numero_Senha = senha;
                    NumeroSenha.Tipo = tipoSenha;
                    NumeroSenha.SenhaTipo = senhatipo;
                    NumeroSenha.Status = status;
                    NumeroSenha.SetorId = _setor;



                    if (_retiradaSenha.parametros.ZerarSenhaDiaSeguinte == true)
                        NumeroSenha.ModoSequencial = false;
                    else
                        NumeroSenha.ModoSequencial = true;

                    NumeroSenha.QtdCaracteres = _retiradaSenha.parametros.Qtd_Caracteres_Senha;

                    NumeroSenha.NumeroSequencia = numSequecia;



                    switch (_setor)
                    {
                        case -1:

                            switch (NumeroSenha.SenhaTipo)
                            {
                                case 1:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_1;
                                    break;

                                case 2:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_2;
                                    break;

                                case 3:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_3;
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case 0:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_1;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_1;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_1;
                            break;

                        case 1:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_2;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_2;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_2;
                            break;

                        case 2:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_3;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_3;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_3;
                            break;

                        case 3:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_4;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_4;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_4;
                            break;

                        default:
                            break;
                    }


                    //int IretornoFormata;

                    //string comandoPularLinha = "\r\n";

                    //IretornoFormata = _AppServicoMP2032.FormataTX(_retiradaSenha.parametros.Nome_Empresa + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    //string imprimirSenha = "";

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:000}      ", tipoSenha, senha);

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:0000}     ", tipoSenha, senha);

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:00000}    ", tipoSenha, senha);

                    //IretornoFormata = _AppServicoMP2032.FormataTX(imprimirSenha + comandoPularLinha, 3, 0, 1, 1, 1);

                    //IretornoFormata = _AppServicoMP2032.FormataTX("       " + DateTime.Now.ToString() + comandoPularLinha + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    //int IretornoGuilhot = _AppServicoMP2032.AcionaGuilhotina(1);

                    //if (IretornoGuilhot != 1)
                    //{
                    //    MessageBox.Show("Erro na Guilhotina.");
                    //    this.Close();
                    //}

                    //int Iretorno = _AppServicoMP2032.FechaPorta();

                    //if (Iretorno != 1)
                    //{
                    //    MessageBox.Show("Erro ao fechar a porta.");
                    //    this.Close();
                    //}

                    _AppServicoSenha.Adicionar(NumeroSenha);

                    _retiradaSenha.EnviarEstacoes();
                    try
                    {
                        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                            ClassFalarTexto.FalarTexto("Retire sua senha.", _retiradaSenha.listaVozes, _retiradaSenha.parametros.Voz_Botao_1);
                    }
                    catch (Exception) { }

                }
                catch (Exception)
                {

                    try
                    {
                        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                            ClassFalarTexto.FalarTexto("Não foi possível imprimir a senha.", _retiradaSenha.listaVozes, _retiradaSenha.parametros.Voz_Botao_1);
                        else
                            MessageBox.Show("Não foi possível imprimir a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception) { }
                }
            //}

        }

        private void SenhaPrioridade80()
        {


            //int IretornoConf = _AppServicoMP2032.ConfiguraModeloImpressora();

            //if (IretornoConf != 1)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //    return;
            //}

            //int IretornoPorta = _AppServicoMP2032.IniciaPorta("USB");

            //if (IretornoPorta != 1)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //    return;
            //}


            //int IRetornoStatus = _AppServicoMP2032.Le_Status();

            //_AppServicoMP2032.FechaPorta();

            //if (IRetornoStatus == 5)
            //{
            //    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //        ImpressoraPoucoPapel(_retiradaSenha.parametros.Voz_Botao_1);
            //}

            //if (IRetornoStatus != 24 && IRetornoStatus != 5)
            //{

            //    if (IRetornoStatus == 32)
            //    {
            //        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //            ImpressoraSemPapel(_retiradaSenha.parametros.Voz_Botao_1);
            //        int Iretorno = _AppServicoMP2032.FechaPorta();
            //    }
            //    if (IRetornoStatus == 0)
            //    {
            //        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
            //            ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
            //        int Iretorno = _AppServicoMP2032.FechaPorta();
            //    }
            //}
            //else
            //{

                try
                {
                    int senha = 0;
                    string tipoSenha = _retiradaSenha.parametros.Letra_Botao_3;
                    int senhatipo = 3;
                    string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                    string status = "EM ESPERA";
                    string aleatorioConfirmacao = ClassGerarAleatorio.NumerosAleatorias(2);

                    int numSequecia = 0;

                    if (_retiradaSenha.parametros.Tipo_Senha == 0)
                        senha = _repositorioSenha.OberProximaSenha(_retiradaSenha.parametros.ZerarSenhaDiaSeguinte, _setor, senhatipo, out numSequecia, _retiradaSenha.parametros.Qtd_Caracteres_Senha);
                    else
                    {
                        do
                        {
                            senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(_retiradaSenha.parametros.Qtd_Caracteres_Senha + 3));
                        } while (_retiradaSenha.senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);
                    }

                    NumeroSenha = new Senha();

                    NumeroSenha.Data = DateTime.Now.Date;

                    if (_retiradaSenha.parametros.Utilizar_Aleatorio == true)
                    {
                        do
                        {
                            NumeroSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(_retiradaSenha.parametros.Qtd_Caracteres_Senha + 3);
                        }
                        while (NumeroSenha.Aleatorio_Confirmacao == senha.ToString() || NumeroSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                    }


                    NumeroSenha.Hora_Retirada = HoraRetiradaSenha;
                    NumeroSenha.Numero_Senha = senha;
                    NumeroSenha.Tipo = tipoSenha;
                    NumeroSenha.SenhaTipo = senhatipo;
                    NumeroSenha.Status = status;
                    NumeroSenha.SetorId = _setor;


                    if (_retiradaSenha.parametros.ZerarSenhaDiaSeguinte == true)
                        NumeroSenha.ModoSequencial = false;
                    else
                        NumeroSenha.ModoSequencial = true;

                    NumeroSenha.QtdCaracteres = _retiradaSenha.parametros.Qtd_Caracteres_Senha;

                    NumeroSenha.NumeroSequencia = numSequecia;

                    switch (_setor)
                    {
                        case -1:

                            switch (NumeroSenha.SenhaTipo)
                            {
                                case 1:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_1;
                                    break;

                                case 2:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_2;
                                    break;

                                case 3:
                                    NumeroSenha.LetraSetor = "";
                                    NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Botao_3;
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case 0:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_1;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_1;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_1;
                            break;

                        case 1:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_2;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_2;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_2;
                            break;

                        case 2:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_3;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_3;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_3;
                            break;

                        case 3:
                            NumeroSenha.LetraSetor = _retiradaSenha.parametros.Letra_Setor_4;
                            NumeroSenha.NomeSetor = _retiradaSenha.parametros.Nome_Setor_4;
                            NumeroSenha.Voz = _retiradaSenha.parametros.Voz_Setor_4;
                            break;

                        default:
                            break;
                    }


                    //int IretornoFormata;

                    //string comandoPularLinha = "\r\n";

                    //IretornoFormata = _AppServicoMP2032.FormataTX(_retiradaSenha.parametros.Nome_Empresa + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    //string imprimirSenha = "";

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:000}      ", tipoSenha, senha);

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:0000}     ", tipoSenha, senha);

                    //if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                    //    imprimirSenha = string.Format("       SENHA: {0} {1:00000}    ", tipoSenha, senha);

                    //IretornoFormata = _AppServicoMP2032.FormataTX(imprimirSenha + comandoPularLinha, 3, 0, 1, 1, 1);

                    //IretornoFormata = _AppServicoMP2032.FormataTX("       " + DateTime.Now.ToString() + comandoPularLinha + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    //int IretornoGuilhot = _AppServicoMP2032.AcionaGuilhotina(1);

                    //if (IretornoGuilhot != 1)
                    //{
                    //    MessageBox.Show("Erro na Guilhotina.");
                    //    this.Close();
                    //}

                    //int Iretorno = _AppServicoMP2032.FechaPorta();

                    //if (Iretorno != 1)
                    //{
                    //    MessageBox.Show("Erro ao fechar a porta.");
                    //    this.Close();
                    //}

                    _AppServicoSenha.Adicionar(NumeroSenha);


                    _retiradaSenha.EnviarEstacoes();

                    try
                    {
                        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                            ClassFalarTexto.FalarTexto("Retire sua senha.", _retiradaSenha.listaVozes, _retiradaSenha.parametros.Voz_Botao_3);

                    }
                    catch (Exception) { }


                }
                catch (Exception)
                {
                    try
                    {
                        if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                            ClassFalarTexto.FalarTexto("Não foi possível imprimir a senha.", _retiradaSenha.listaVozes, _retiradaSenha.parametros.Voz_Botao_3);
                        else
                            MessageBox.Show("Não foi possível imprimir a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception) { }

                }
            //}

        }




        private void ImpressoraSemPapel(string voz)
        {

            try
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ClassFalarTexto.FalarTexto("IMPRESSORA SEM PAPEL, FAVOR TROCAR A BUBINA.", _retiradaSenha.listaVozes, voz);
            }
            catch (Exception) { }
        }

        private void ImpressoraPoucoPapel(string voz)
        {


            try
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ClassFalarTexto.FalarTexto("IMPRESSORA COM POUCO PAPEL.", _retiradaSenha.listaVozes, voz);
            }
            catch (Exception) { }
        }

        private void ErroNaImpressora(string voz)
        {
            try
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ClassFalarTexto.FalarTexto("OCORREU UM ERRO AO TENTAR IMPRIMIR A SENHA.", _retiradaSenha.listaVozes, voz);
            }
            catch (Exception) { }

        }



        private void Alterar()
        {

            try
            {
                string caminhoNovo = @"\\Servidor\c\Cartorio\CS_Sistemas\CS_Caixa\Senha\";
                //Verifico se o arquivo que desejo abrir existe e passo como parâmetro a variável respectiva

                if (File.Exists(caminhoNovo))
                {

                    //Instancio o FileStream passando como parâmetro a variável padrão, o FileMode que será

                    //o modo Open e o FileAccess, que será Read(somente leitura). Este método é diferente dos

                    //demais: primeiro irei abrir o arquivo, depois criar um FileStream temporário que irá

                    //armazenar os novos dados e depois criarei outro FileStream para fazer a junção dos dois

                    using (FileStream fs = new FileStream(caminhoNovo, FileMode.Open, FileAccess.Read))
                    {

                        //Aqui instancio o StreamReader passando como parâmetro o FileStream criado acima.

                        //Uso o StreamReader já que faço 1º a leitura do arquivo. Irei percorrer o arquivo e

                        //quando encontrar uma string qualquer farei a alteração por outra string qualquer

                        using (StreamReader sr = new StreamReader(fs))
                        {

                            //Crio o FileStream temporário onde irei gravar as informações

                            using (FileStream fsTmp = new FileStream(caminhoNovo + NumeroSenha.Senha_Id,

                                                       FileMode.Create, FileAccess.Write))
                            {

                                //Instancio o StreamWriter para escrever os dados no arquivo temporário,

                                //passando como parâmetro a variável fsTmp, referente ao FileStream criado

                                using (StreamWriter sw = new StreamWriter(fsTmp))
                                {

                                    //Crio uma variável e a atribuo como nula. Faço um while que percorrerá

                                    //meu arquivo original e enquanto ele estiver diferente de nulo...

                                    string strLinha = null;

                                    while ((strLinha = sr.ReadLine()) != null)
                                    {

                                        //faço um indexof para verificar se existe a palavra adicionado,

                                        //se ela existir, a substituo pela palavra alterado

                                        if (strLinha.IndexOf("#senha") > -1)
                                        {

                                            //uso o método Replace que espera o valor antigo e valor novo

                                            strLinha = strLinha.Replace("#senha", NumeroSenha.Tipo + " " + NumeroSenha.Numero_Senha);

                                        }

                                        //Chamo o método Write do StreamWriter passando o strLinha como parâmetro

                                        sw.Write(strLinha);

                                    }

                                }

                            }

                        }

                    }



                    //Ao final excluo o arquivo anterior e movo o temporário no lugar do original

                    //Dessa forma não perco os dados de modificação de meu arquivo

                    //File.Delete(strPathFile);



                    //No método Move passo o arquivo de origem, o temporário, e o de destino, o original

                    File.Move(caminho + ".tmp", caminhoNovo);



                  

                }

                else
                {

                    //Se não existir exibo a mensagem

                    MessageBox.Show("Arquivo não encontrado!");

                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }

    }
}
