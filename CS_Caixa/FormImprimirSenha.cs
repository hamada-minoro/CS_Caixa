using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace CS_Caixa
{
    public partial class FormImprimirSenha : Form
    {
        string imprimir = "nao";

        RepositorioMP2032 _AppServicoMP2032 = new RepositorioMP2032();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();

        RepositorioSenha _repositorioSenha = new RepositorioSenha();

        Senha NumeroSenha = new Senha();
        BackgroundWorker worker;
        string _chamarMetodo;
        int _setor;
        RetiradaSenha _retiradaSenha;

        List<string> impressoras = new List<string>();

        string impressora = (new PrinterSettings()).PrinterName;
        string caminhoSalvar = string.Empty;

        public FormImprimirSenha(string chamarMetodo, RetiradaSenha retiradaSenha, int setor)
        {
            _chamarMetodo = chamarMetodo;
            _retiradaSenha = retiradaSenha;
            _setor = setor;
            InitializeComponent();
        }


        public FormImprimirSenha(string chamarMetodo)
        {
            _chamarMetodo = chamarMetodo;
            InitializeComponent();
        }

        private void FormImprimirSenha_Load(object sender, EventArgs e)
        {
            try
            {
                this.Activate();

                if (_chamarMetodo != "Teste")
                {
                    _retiradaSenha.maquinasEstacao = _AppServicoCadastro_Pc.ObterTodos().Where(p => p.Tipo_Entrada == 1).ToList();
                    SelecionarImpressora();
                    richTextBox1.LoadFile(@"\\servidor\CS_Sistemas\CS_Caixa\Senha\Senha.rtf");
                }
                else
                    richTextBox1.LoadFile(@"\\servidor\CS_Sistemas\CS_Caixa\Senha Teste\Senha.rtf");
                

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception)
            {

                Close();
            }
        }


        void SelecionarImpressora()
        {

            foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                impressoras.Add(printer.ToString());
            }

            impressora = impressoras.Where(p => p.Contains("mp") || p.Contains("MP")).FirstOrDefault();
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


        private void ImprimirTeste()
        {
            try
            {
                string imprimirSenha = string.Format("{0} {1:00000}", "T", 0);

                
                System.Text.RegularExpressions.Regex regExp = new System.Text.RegularExpressions.Regex("#senha");

                foreach (System.Text.RegularExpressions.Match match in regExp.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(match.Index, match.Length);
                    richTextBox1.SelectedText = string.Format("{0}", imprimirSenha);

                }

                System.Text.RegularExpressions.Regex regExp2 = new System.Text.RegularExpressions.Regex("#data");

                foreach (System.Text.RegularExpressions.Match match2 in regExp2.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(match2.Index, match2.Length);
                    richTextBox1.SelectedText = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString();

                }


                System.Text.RegularExpressions.Regex regExp3 = new System.Text.RegularExpressions.Regex("#maquina");

                foreach (System.Text.RegularExpressions.Match match3 in regExp3.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(match3.Index, match3.Length);
                    richTextBox1.SelectedText = string.Format("{0}", Environment.MachineName);

                }


                caminhoSalvar = @"\\servidor\CS_Sistemas\CS_Caixa\Senha Teste\" + "UltimoTeste.rtf";

                richTextBox1.SaveFile(caminhoSalvar, RichTextBoxStreamType.RichText);



                using (Process objP = new Process())
                {
                    objP.StartInfo.FileName = caminhoSalvar;

                    objP.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    objP.StartInfo.Verb = "print";
                    objP.StartInfo.CreateNoWindow = true;

                    objP.Start();                    
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (_chamarMetodo != "Teste")
            {
                if (imprimir == "sim")
                    ImprimirSenha();
                else
                    _AppServicoSenha.Remove(NumeroSenha);
            }
            else
                ImprimirTeste();

            this.Close();
        }

        private void ImprimirSenha()
        {
            try
            {
                string imprimirSenha = "";

                if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                    imprimirSenha = string.Format("{0} {1:000}", NumeroSenha.Tipo, NumeroSenha.Numero_Senha);

                if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                    imprimirSenha = string.Format("{0} {1:0000}", NumeroSenha.Tipo, NumeroSenha.Numero_Senha);

                if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                    imprimirSenha = string.Format("{0} {1:00000}", NumeroSenha.Tipo, NumeroSenha.Numero_Senha);


                System.Text.RegularExpressions.Regex regExp = new System.Text.RegularExpressions.Regex("#senha");

                foreach (System.Text.RegularExpressions.Match match in regExp.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(match.Index, match.Length);
                    richTextBox1.SelectedText = string.Format("{0}", imprimirSenha);

                }

                System.Text.RegularExpressions.Regex regExp2 = new System.Text.RegularExpressions.Regex("#data");

                foreach (System.Text.RegularExpressions.Match match2 in regExp2.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(match2.Index, match2.Length);
                    richTextBox1.SelectedText = NumeroSenha.Data.ToShortDateString() + " - " + NumeroSenha.Hora_Retirada;

                }

                caminhoSalvar = @"\\servidor\CS_Sistemas\CS_Caixa\Senha\" + "UltimaSenha.rtf";

                richTextBox1.SaveFile(caminhoSalvar, RichTextBoxStreamType.RichText);



                Process objP = new Process();
                objP.StartInfo.FileName = caminhoSalvar;

                objP.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; 
                objP.StartInfo.Verb = "print";
                objP.StartInfo.CreateNoWindow = true;
               
                objP.Start();
                
                objP.CloseMainWindow();
                objP.Close();


            }
            catch (Exception)
            {

                throw;
            }

        }



        private void SenhaPrioridade()
        {


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



                _AppServicoSenha.Adicionar(NumeroSenha);
                imprimir = "sim";
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
                        imprimir = "nao";
                }
                catch (Exception) { }

            }


        }


       

        private void SenhaNormal()
        {
           
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


               

                _AppServicoSenha.Adicionar(NumeroSenha);
                imprimir = "sim";
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
                        imprimir = "nao";
                }
                catch (Exception) { }
            }
            
        }

        private void SenhaPrioridade80()
        {
        
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


              
                _AppServicoSenha.Adicionar(NumeroSenha);

                imprimir = "sim";
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
                        imprimir = "nao";
                }
                catch (Exception) { }

            }

        }
    }
}
