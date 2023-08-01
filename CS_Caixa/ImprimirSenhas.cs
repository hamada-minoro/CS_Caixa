using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace CS_Caixa
{
    public partial class ImprimirSenhas : Form
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

        RetiradaSenha _retiradaSenha;

        public ImprimirSenhas(string chamarMetodo, RetiradaSenha retiradaSenha, int setor)
        {
            _chamarMetodo = chamarMetodo;
            _retiradaSenha = retiradaSenha;
            _setor = setor;
            InitializeComponent();
        }

       
        
        private void ImprimirSenhas_Load(object sender, EventArgs e)
        {
            try
            {
                this.Activate();

                _retiradaSenha.maquinasEstacao = _AppServicoCadastro_Pc.ObterTodos().Where(p => p.Tipo_Entrada == 1).ToList();

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Ocorreu um erro ao tentar obter as configurações. Favor imformar ao Suporte.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void SenhaPrioridade()
        {


            int IretornoConf = _AppServicoMP2032.ConfiguraModeloImpressora();

            if (IretornoConf != 1)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                return;
            }

            int IretornoPorta = _AppServicoMP2032.IniciaPorta("USB");

            if (IretornoPorta != 1)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                return;
            }


            int IRetornoStatus = _AppServicoMP2032.Le_Status();

            _AppServicoMP2032.FechaPorta();

            if (IRetornoStatus == 5)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ImpressoraPoucoPapel(_retiradaSenha.parametros.Voz_Botao_1);
            }

            if (IRetornoStatus != 24 && IRetornoStatus != 5)
            {

                if (IRetornoStatus == 32)
                {
                    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                        ImpressoraSemPapel(_retiradaSenha.parametros.Voz_Botao_1);
                    int Iretorno = _AppServicoMP2032.FechaPorta();
                }
                if (IRetornoStatus == 0)
                {
                    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                    int Iretorno = _AppServicoMP2032.FechaPorta();
                }
            }
            else
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

                    Senha NumeroSenha = new Senha();

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

                    int IretornoFormata;

                    string comandoPularLinha = "\r\n";



                    IretornoFormata = _AppServicoMP2032.FormataTX(_retiradaSenha.parametros.Nome_Empresa + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);


                    string imprimirSenha = "";

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                        imprimirSenha = string.Format("       SENHA: {0} {1:000}      ", tipoSenha, senha);

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                        imprimirSenha = string.Format("       SENHA: {0} {1:0000}     ", tipoSenha, senha);

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                        imprimirSenha = string.Format("       SENHA: {0} {1:00000}    ", tipoSenha, senha);

                    IretornoFormata = _AppServicoMP2032.FormataTX(imprimirSenha + comandoPularLinha, 3, 0, 1, 1, 1);

                    IretornoFormata = _AppServicoMP2032.FormataTX("       " + DateTime.Now.ToString() + comandoPularLinha + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    int IretornoGuilhot = _AppServicoMP2032.AcionaGuilhotina(1);

                    if (IretornoGuilhot != 1)
                    {
                        System.Windows.MessageBox.Show("Erro na Guilhotina.");
                        this.Close();
                    }

                    int Iretorno = _AppServicoMP2032.FechaPorta();

                    if (Iretorno != 1)
                    {
                        System.Windows.MessageBox.Show("Erro ao fechar a porta.");
                        this.Close();
                    }

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
                            System.Windows.MessageBox.Show("Não foi possível imprimir a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception) { }

                }
            }

        }

        private void SenhaNormal()
        {


            int IretornoConf = _AppServicoMP2032.ConfiguraModeloImpressora();

            if (IretornoConf != 1)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                return;
            }

            int IretornoPorta = _AppServicoMP2032.IniciaPorta("USB");

            if (IretornoPorta != 1)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                return;
            }


            int IRetornoStatus = _AppServicoMP2032.Le_Status();

            _AppServicoMP2032.FechaPorta();

            if (IRetornoStatus == 5)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ImpressoraPoucoPapel(_retiradaSenha.parametros.Voz_Botao_1);
            }

            if (IRetornoStatus != 24 && IRetornoStatus != 5)
            {

                if (IRetornoStatus == 32)
                {
                    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                        ImpressoraSemPapel(_retiradaSenha.parametros.Voz_Botao_1);
                    int Iretorno = _AppServicoMP2032.FechaPorta();
                }
                if (IRetornoStatus == 0)
                {
                    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                    int Iretorno = _AppServicoMP2032.FechaPorta();
                }
            }
            else
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


                    int IretornoFormata;

                    string comandoPularLinha = "\r\n";

                    IretornoFormata = _AppServicoMP2032.FormataTX(_retiradaSenha.parametros.Nome_Empresa + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    string imprimirSenha = "";

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                        imprimirSenha = string.Format("       SENHA: {0} {1:000}      ", tipoSenha, senha);

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                        imprimirSenha = string.Format("       SENHA: {0} {1:0000}     ", tipoSenha, senha);

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                        imprimirSenha = string.Format("       SENHA: {0} {1:00000}    ", tipoSenha, senha);

                    IretornoFormata = _AppServicoMP2032.FormataTX(imprimirSenha + comandoPularLinha, 3, 0, 1, 1, 1);

                    IretornoFormata = _AppServicoMP2032.FormataTX("       " + DateTime.Now.ToString() + comandoPularLinha + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    int IretornoGuilhot = _AppServicoMP2032.AcionaGuilhotina(1);

                    if (IretornoGuilhot != 1)
                    {
                        System.Windows.MessageBox.Show("Erro na Guilhotina.");
                        this.Close();
                    }

                    int Iretorno = _AppServicoMP2032.FechaPorta();

                    if (Iretorno != 1)
                    {
                        System.Windows.MessageBox.Show("Erro ao fechar a porta.");
                        this.Close();
                    }

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
                            System.Windows.MessageBox.Show("Não foi possível imprimir a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception) { }
                }
            }

        }

        private void SenhaPrioridade80()
        {


            int IretornoConf = _AppServicoMP2032.ConfiguraModeloImpressora();

            if (IretornoConf != 1)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                return;
            }

            int IretornoPorta = _AppServicoMP2032.IniciaPorta("USB");

            if (IretornoPorta != 1)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                return;
            }


            int IRetornoStatus = _AppServicoMP2032.Le_Status();

            _AppServicoMP2032.FechaPorta();

            if (IRetornoStatus == 5)
            {
                if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                    ImpressoraPoucoPapel(_retiradaSenha.parametros.Voz_Botao_1);
            }

            if (IRetornoStatus != 24 && IRetornoStatus != 5)
            {

                if (IRetornoStatus == 32)
                {
                    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                        ImpressoraSemPapel(_retiradaSenha.parametros.Voz_Botao_1);
                    int Iretorno = _AppServicoMP2032.FechaPorta();
                }
                if (IRetornoStatus == 0)
                {
                    if (_retiradaSenha.parametros.Voz_RetiradaSenha == true)
                        ErroNaImpressora(_retiradaSenha.parametros.Voz_Botao_1);
                    int Iretorno = _AppServicoMP2032.FechaPorta();
                }
            }
            else
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

                    Senha NumeroSenha = new Senha();

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


                    int IretornoFormata;

                    string comandoPularLinha = "\r\n";

                    IretornoFormata = _AppServicoMP2032.FormataTX(_retiradaSenha.parametros.Nome_Empresa + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    string imprimirSenha = "";

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 0)
                        imprimirSenha = string.Format("       SENHA: {0} {1:000}      ", tipoSenha, senha);

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 1)
                        imprimirSenha = string.Format("       SENHA: {0} {1:0000}     ", tipoSenha, senha);

                    if (_retiradaSenha.parametros.Qtd_Caracteres_Senha == 2)
                        imprimirSenha = string.Format("       SENHA: {0} {1:00000}    ", tipoSenha, senha);

                    IretornoFormata = _AppServicoMP2032.FormataTX(imprimirSenha + comandoPularLinha, 3, 0, 1, 1, 1);

                    IretornoFormata = _AppServicoMP2032.FormataTX("       " + DateTime.Now.ToString() + comandoPularLinha + comandoPularLinha + comandoPularLinha, 1, 0, 0, 0, 0);

                    int IretornoGuilhot = _AppServicoMP2032.AcionaGuilhotina(1);

                    if (IretornoGuilhot != 1)
                    {
                        System.Windows.MessageBox.Show("Erro na Guilhotina.");
                        this.Close();
                    }

                    int Iretorno = _AppServicoMP2032.FechaPorta();

                    if (Iretorno != 1)
                    {
                        System.Windows.MessageBox.Show("Erro ao fechar a porta.");
                        this.Close();
                    }

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
                            System.Windows.MessageBox.Show("Não foi possível imprimir a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception) { }

                }
            }

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

    }
}