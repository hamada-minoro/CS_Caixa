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
    /// Lógica interna para AguardeSalvarParametros.xaml
    /// </summary>
    public partial class AguardeSalvarParametros : Window
    {
        List<Cadastro_Painel> _listaCadastroPainel;
        List<Cadastro_Pc> _listaCadastroPc;
        List<Mensagem> _listaMensagem;
        List<Pc_Painel> _listaPcPainel;
        List<Usuario> _listaUsuario;
        Parametro _parametros;
        RepositorioSenha _repositorioSenha = new RepositorioSenha();
        List<Cadastro_Painel> _listaCadastroPainelRemover;
        List<Cadastro_Pc> _listaCadastroPcRemover;
        List<Mensagem> _listaMensagemRemover;
        List<Pc_Painel> _listaPcPainelRemover;
        List<Usuario> _listaUsuarioRemover;
        Parametro _parametrosRemover;

        string execucaoAtual = string.Empty;

        BackgroundWorker worker;

        private IPAddress enderecoIPAtualizar;
        private StreamWriter stwEnviadorAtualizar;
        private TcpClient tcpServidorAtualizar;

        private IPAddress enderecoIPAtualizarPaineis;
        private StreamWriter stwEnviadorAtualizarPaineis;
        private TcpClient tcpServidorAtualizarPaineis;

        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();
        RepositorioUsuario _AppServicoUsuario = new RepositorioUsuario();
        RepositorioPc_Painel _AppServicoPc_Painel = new RepositorioPc_Painel();
        RepositorioMensagem _AppServicoMensagem = new RepositorioMensagem();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioSenha _AppServicoSenha = new RepositorioSenha();

        public AguardeSalvarParametros(List<Cadastro_Painel> listaCadastroPainel, List<Cadastro_Pc> listaCadastroPc, List<Mensagem> listaMensagem, Parametro parametros, List<Pc_Painel> listaPcPainel, List<Usuario> listaUsuario)
        {
            _listaCadastroPainel = listaCadastroPainel;
            _listaCadastroPc = listaCadastroPc;
            _listaMensagem = listaMensagem;
            _parametros = parametros;
            _listaPcPainel = listaPcPainel;
            _listaUsuario = listaUsuario;
            InitializeComponent();
        }

        public void InicializaConexaoAtualizarEstacoes(Cadastro_Pc maquinaEstacao)
        {
            try
            {
                if (maquinaEstacao.Tipo_Entrada == 1)
                {
                    enderecoIPAtualizar = IPAddress.Parse(maquinaEstacao.Ip_Pc);

                    tcpServidorAtualizar = new TcpClient();
                    tcpServidorAtualizar.NoDelay = true;
                    tcpServidorAtualizar.Connect(enderecoIPAtualizar, maquinaEstacao.Porta_Pc);

                    if (tcpServidorAtualizar.Connected == true)
                    {
                        stwEnviadorAtualizar = new StreamWriter(tcpServidorAtualizar.GetStream());
                        stwEnviadorAtualizar.WriteLine("Parametros");
                        stwEnviadorAtualizar.Flush();

                        tcpServidorAtualizar.Close();
                    }
                }
            }
            catch (Exception) { }


        }


        public void InicializaConexaoAtualizarPaineis(Cadastro_Painel maquinaPainel)
        {
            try
            {
                enderecoIPAtualizarPaineis = IPAddress.Parse(maquinaPainel.Ip_Pc);
                
                tcpServidorAtualizarPaineis = new TcpClient();
                tcpServidorAtualizarPaineis.NoDelay = true;
                tcpServidorAtualizarPaineis.Connect(enderecoIPAtualizarPaineis, maquinaPainel.Porta_Pc);

                if (tcpServidorAtualizarPaineis.Connected == true)
                {
                    stwEnviadorAtualizarPaineis = new StreamWriter(tcpServidorAtualizarPaineis.GetStream());
                    stwEnviadorAtualizarPaineis.WriteLine("Parametros");
                    stwEnviadorAtualizarPaineis.Flush();

                    tcpServidorAtualizarPaineis.Close();

                }

            }
            catch (Exception) { }


        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                _listaCadastroPainelRemover = _AppServicoCadastro_Painel.ObterTodos().ToList();
                _listaCadastroPcRemover = _AppServicoCadastro_Pc.ObterTodos().ToList();
                _listaMensagemRemover = _AppServicoMensagem.ObterTodos().ToList();
                _listaPcPainelRemover = _AppServicoPc_Painel.ObterTodos().ToList();
                _listaUsuarioRemover = _AppServicoUsuario.ObterTodos().ToList();
                _parametrosRemover = _AppServicoParametros.ObterTodos().FirstOrDefault();


                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Ocorreu um erro ao tentar salvar as configurações. Favor imformar ao Suporte.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                RemoveSalvarCadastroPainel();
                RemoveSalvarCadastroPc();
                RemoveSalvarMensagem();
                RemoveSalvarParamentros();
                RemoveSalvarPcPainel();
                RemoveSalvarUsuario();

                Close();
            }


        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            execucaoAtual = "Salvando Candastro Painel";
            Thread.Sleep(1);
            worker.ReportProgress(0);
            SalvarCadastroPainel(_listaCadastroPainel);

            execucaoAtual = "Salvando Candastro Máquina";
            Thread.Sleep(1);
            worker.ReportProgress(0);
            SalvarCadastroPc(_listaCadastroPc);

            execucaoAtual = "Salvando Mensagens";
            Thread.Sleep(1);
            worker.ReportProgress(0);
            SalvarMensagem(_listaMensagem);

            execucaoAtual = "Salvando Parâmetros";
            Thread.Sleep(1);
            worker.ReportProgress(0);
            SalvarParamentros(_parametros);

            execucaoAtual = "Salvando Relações Painel/PC";
            Thread.Sleep(1);
            worker.ReportProgress(0);
            SalvarPcPainel(_listaPcPainel);

            execucaoAtual = "Salvando Usuários";
            Thread.Sleep(1);
            worker.ReportProgress(0);
            SalvarUsuario(_listaUsuario);

            if (_parametros.ModoRetiradaSenhaManual == true)
            {
                int numeroNovaSequencia = _repositorioSenha.ObterProximaSequenciaManual();

                if (numeroNovaSequencia > 1)
                {
                    if (_repositorioSenha.SaberSeGeraMaisUmaSequenciaManual() == 0)
                    {
                        for (int i = 1; i <= 999; i++)
                        {
                            execucaoAtual = "Salvando Senhas Preferenciais";
                            Thread.Sleep(1);
                            worker.ReportProgress(i);
                            SalvarSenhasModoManualPrioridade(i, numeroNovaSequencia);
                        }
                        for (int i = 1; i <= 999; i++)
                        {
                            execucaoAtual = "Salvando Senhas Gerais";
                            Thread.Sleep(1);
                            worker.ReportProgress(i);
                            SalvarSenhasModoManualNormal(i, numeroNovaSequencia);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= 999; i++)
                    {
                        execucaoAtual = "Salvando Senhas Preferenciais";
                        Thread.Sleep(1);
                        worker.ReportProgress(i);
                        SalvarSenhasModoManualPrioridade(i, numeroNovaSequencia);
                    }
                    for (int i = 1; i <= 999; i++)
                    {
                        execucaoAtual = "Salvando Senhas Gerais";
                        Thread.Sleep(1);
                        worker.ReportProgress(i);
                        SalvarSenhasModoManualNormal(i, numeroNovaSequencia);
                    }
                }
            }

            execucaoAtual = "Aguarde...";
            Thread.Sleep(1);
            worker.ReportProgress(0);


            foreach (var item in _listaCadastroPainel)
            {
                InicializaConexaoAtualizarPaineis(item);
            }

            foreach (var item in _listaCadastroPc)
            {
                if(item.Tipo_Entrada == 0)
                InicializaConexaoAtualizarEstacoes(item);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (execucaoAtual == "Salvando Senhas Preferenciais" || execucaoAtual == "Salvando Senhas Gerais")
            {
                progressBar1.Value = e.ProgressPercentage;
                label1.Content = string.Format(execucaoAtual + "... {0}/999", e.ProgressPercentage);
            }
            else
                label1.Content = string.Format(execucaoAtual);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();

        }

        private void SalvarSenhasModoManualPrioridade(int senha, int numSequecia)
        {
            try
            {
                string tipoSenha = _parametros.Letra_Botao_2;
                int senhatipo = 2;
                string status = "EM ESPERA";


                Senha NumeroSenha = new Senha();


                NumeroSenha.Numero_Senha = senha;
                NumeroSenha.Tipo = tipoSenha;
                NumeroSenha.SenhaTipo = senhatipo;
                NumeroSenha.Status = status;
                NumeroSenha.SetorId = 5;


                NumeroSenha.QtdCaracteres = _parametros.Qtd_Caracteres_Senha;

                NumeroSenha.NumeroSequencia = numSequecia;


                NumeroSenha.LetraSetor = "";
                NumeroSenha.Voz = _parametros.Voz_Botao_2;




                _AppServicoSenha.Adicionar(NumeroSenha);


            }
            catch (Exception)
            {

                MessageBox.Show("Ocorreu um erro ao tentar gerar as senhas.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SalvarSenhasModoManualNormal(int senha, int numSequecia)
        {
            try
            {
                string tipoSenha = _parametros.Letra_Botao_1;
                int senhatipo = 1;
                string status = "EM ESPERA";


                Senha NumeroSenha = new Senha();



                NumeroSenha.Numero_Senha = senha;
                NumeroSenha.Tipo = tipoSenha;
                NumeroSenha.SenhaTipo = senhatipo;
                NumeroSenha.Status = status;
                NumeroSenha.SetorId = 5;


                NumeroSenha.QtdCaracteres = _parametros.Qtd_Caracteres_Senha;

                NumeroSenha.NumeroSequencia = numSequecia;


                NumeroSenha.LetraSetor = "";
                NumeroSenha.Voz = _parametros.Voz_Botao_1;




                _AppServicoSenha.Adicionar(NumeroSenha);


            }
            catch (Exception)
            {

                MessageBox.Show("Ocorreu um erro ao tentar gerar as senhas.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalvarCadastroPainel(List<Cadastro_Painel> listaCadastroPainel)
        {
            Cadastro_Painel cadastroPainel;


            List<Cadastro_Painel> cadastroExistente;
            cadastroExistente = _AppServicoCadastro_Painel.ObterTodos().ToList();
            foreach (var item in cadastroExistente)
            {
                var excluir = _AppServicoCadastro_Painel.ObterPorId(item.Cadastro_Painel_Id);

                if (listaCadastroPainel.Where(p => p.Cadastro_Painel_Id == excluir.Cadastro_Painel_Id).FirstOrDefault() == null)
                    _AppServicoCadastro_Painel.Remove(excluir);
            }


            foreach (var item in listaCadastroPainel)
            {
                if (item.Cadastro_Painel_Id == 0)
                    cadastroPainel = new Cadastro_Painel();
                else
                    cadastroPainel = _AppServicoCadastro_Painel.ObterPorId(item.Cadastro_Painel_Id);

                cadastroPainel.Data_Cadastro = item.Data_Cadastro;
                cadastroPainel.Identificador_Pc = item.Identificador_Pc;
                cadastroPainel.Ip_Pc = item.Ip_Pc;
                cadastroPainel.Nome_Pc = item.Nome_Pc;
                cadastroPainel.Porta_Pc = item.Porta_Pc;

                if (item.Cadastro_Painel_Id == 0)
                    _AppServicoCadastro_Painel.Adicionar(cadastroPainel);
                else
                    _AppServicoCadastro_Painel.Update(cadastroPainel);


            }

        }

        private void SalvarCadastroPc(List<Cadastro_Pc> listaCadastroPc)
        {
            Cadastro_Pc cadastroPc;


            List<Cadastro_Pc> cadastroExistente;
            cadastroExistente = _AppServicoCadastro_Pc.ObterTodos().ToList();
            foreach (var item in cadastroExistente)
            {
                var excluir = _AppServicoCadastro_Pc.ObterPorId(item.Cadastro_Pc_Id);

                if (listaCadastroPc.Where(p => p.Cadastro_Pc_Id == excluir.Cadastro_Pc_Id).FirstOrDefault() == null)
                    _AppServicoCadastro_Pc.Remove(excluir);
            }



            foreach (var item in listaCadastroPc)
            {

                if (item.Cadastro_Pc_Id == 0)
                    cadastroPc = new Cadastro_Pc();
                else
                    cadastroPc = _AppServicoCadastro_Pc.ObterPorId(item.Cadastro_Pc_Id);

                cadastroPc.Caracter = item.Caracter;
                cadastroPc.Data_Cadastro = item.Data_Cadastro;
                cadastroPc.Identificador_Pc = item.Identificador_Pc;
                cadastroPc.Ip_Pc = item.Ip_Pc;
                cadastroPc.Nome_Pc = item.Nome_Pc;
                cadastroPc.Porta_Pc = item.Porta_Pc;
                cadastroPc.Tipo_Atendimento = item.Tipo_Atendimento;
                cadastroPc.Tipo_Entrada = item.Tipo_Entrada;
                cadastroPc.FalaOutros = item.FalaOutros;
                cadastroPc.SetorId = item.SetorId;
                cadastroPc.TipoChamadaSenha = item.TipoChamadaSenha;
                if (item.Cadastro_Pc_Id == 0)
                    _AppServicoCadastro_Pc.Adicionar(cadastroPc);
                else
                    _AppServicoCadastro_Pc.Update(cadastroPc);
            }

        }

        private void SalvarMensagem(List<Mensagem> listaMensagem)
        {
            Mensagem mensagem;

            List<Mensagem> cadastroExistente;
            cadastroExistente = _AppServicoMensagem.ObterTodos().ToList();
            foreach (var item in cadastroExistente)
            {
                var excluir = _AppServicoMensagem.ObterPorId(item.Mensagem_Id);

                if (listaMensagem.Where(p => p.Mensagem_Id == excluir.Mensagem_Id).FirstOrDefault() == null)
                    _AppServicoMensagem.Remove(excluir);
            }


            foreach (var item in listaMensagem)
            {
                if (item.Mensagem_Id == 0)
                    mensagem = new Mensagem();
                else
                    mensagem = _AppServicoMensagem.ObterPorId(item.Mensagem_Id);
                mensagem.Cor = item.Cor;
                mensagem.Pisca = item.Pisca;
                mensagem.Texto = item.Texto;

                if (item.Mensagem_Id == 0)
                    _AppServicoMensagem.Adicionar(mensagem);
                else
                    _AppServicoMensagem.Update(mensagem);
            }

        }

        private void SalvarParamentros(Parametro param)
        {
            Parametro parametros;

            if (param.Parametro_Id == 0)
                parametros = new Parametro();
            else
                parametros = _AppServicoParametros.ObterPorId(param.Parametro_Id);


            parametros.CodigoInstalacao = param.CodigoInstalacao;
            parametros.DataInstalacao = param.DataInstalacao;
            parametros.Bip_Aviso = param.Bip_Aviso;
            parametros.Falar_Senha = param.Falar_Senha;
            parametros.Voz_RetiradaSenha = param.Voz_RetiradaSenha;
            parametros.Habilitado_Botao_1 = param.Habilitado_Botao_1;
            parametros.Habilitado_Botao_2 = param.Habilitado_Botao_2;
            parametros.Habilitado_Botao_3 = param.Habilitado_Botao_3;
            parametros.Letra_Botao_1 = param.Letra_Botao_1;
            parametros.Letra_Botao_2 = param.Letra_Botao_2;
            parametros.Letra_Botao_3 = param.Letra_Botao_3;
            parametros.Mostrar_Hora = param.Mostrar_Hora;
            parametros.Nome_Botao_1 = param.Nome_Botao_1;
            parametros.Nome_Botao_2 = param.Nome_Botao_2;
            parametros.Nome_Botao_3 = param.Nome_Botao_3;
            parametros.Voz_Botao_1 = param.Voz_Botao_1;
            parametros.Voz_Botao_2 = param.Voz_Botao_2;
            parametros.Voz_Botao_3 = param.Voz_Botao_3;
            parametros.ZerarSenhaDiaSeguinte = param.ZerarSenhaDiaSeguinte;
            parametros.Voz_Padrao = param.Voz_Padrao;
            parametros.ModoRetiradaSenhaManual = param.ModoRetiradaSenhaManual;

            parametros.Habilitado_Setor_1 = param.Habilitado_Setor_1;
            parametros.Habilitado_Setor_2 = param.Habilitado_Setor_2;
            parametros.Habilitado_Setor_3 = param.Habilitado_Setor_3;
            parametros.Habilitado_Setor_4 = param.Habilitado_Setor_4;

            parametros.Letra_Setor_1 = param.Letra_Setor_1;
            parametros.Letra_Setor_2 = param.Letra_Setor_2;
            parametros.Letra_Setor_3 = param.Letra_Setor_3;
            parametros.Letra_Setor_4 = param.Letra_Setor_4;

            parametros.Nome_Setor_1 = param.Nome_Setor_1;
            parametros.Nome_Setor_2 = param.Nome_Setor_2;
            parametros.Nome_Setor_3 = param.Nome_Setor_3;
            parametros.Nome_Setor_4 = param.Nome_Setor_4;

            parametros.Voz_Setor_1 = param.Voz_Setor_1;
            parametros.Voz_Setor_2 = param.Voz_Setor_2;
            parametros.Voz_Setor_3 = param.Voz_Setor_3;
            parametros.Voz_Setor_4 = param.Voz_Setor_4;


            parametros.Nome_Empresa = param.Nome_Empresa;
            parametros.Passar_Mensagem = param.Passar_Mensagem;
            parametros.Qtd_Caracteres_Senha = param.Qtd_Caracteres_Senha;
            parametros.Saudacao = param.Saudacao;
            parametros.Tipo_Senha = param.Tipo_Senha;
            parametros.Utilizar_Aleatorio = param.Utilizar_Aleatorio;
            parametros.CadastroCliente = param.CadastroCliente;

            parametros.InicioFimExpediente = param.InicioFimExpediente;
            parametros.Domingo = param.Domingo;
            parametros.Segunda = param.Segunda;
            parametros.Terca = param.Terca;
            parametros.Quarta = param.Quarta;
            parametros.Quinta = param.Quinta;
            parametros.Sexta = param.Sexta;
            parametros.Sabado = param.Sabado;
            parametros.HoraInicioExpediente = param.HoraInicioExpediente;
            parametros.HoraFimExpediente = param.HoraFimExpediente;
            parametros.DesligarPainel = param.DesligarPainel;
            parametros.HoraDesligarPainel = param.HoraDesligarPainel;
            parametros.DesligarSenha = param.DesligarSenha;
            parametros.HoraDesligarSenha = param.HoraDesligarSenha;
            parametros.DesligarEstacao = param.DesligarEstacao;
            parametros.HoraDesligarEstacao = param.HoraDesligarEstacao;
            parametros.MensagemInicioExpediente = param.MensagemInicioExpediente;
            parametros.MensagemFimExpediente = param.MensagemFimExpediente;




            if (param.Parametro_Id == 0)
                _AppServicoParametros.Adicionar(parametros);
            else
                _AppServicoParametros.Update(parametros);
        }

        private void SalvarPcPainel(List<Pc_Painel> listaPcPainel)
        {
            Pc_Painel pcPainel;


            List<Pc_Painel> cadastroExistente;
            cadastroExistente = _AppServicoPc_Painel.ObterTodos().ToList();
            foreach (var item in cadastroExistente)
            {
                var excluir = _AppServicoPc_Painel.ObterPorId(item.Pc_Painel_Id);

                if (listaPcPainel.Where(p => p.Pc_Painel_Id == excluir.Pc_Painel_Id).FirstOrDefault() == null)
                    _AppServicoPc_Painel.Remove(excluir);
            }

            foreach (var item in listaPcPainel)
            {
                if (item.Pc_Painel_Id == 0)
                    pcPainel = new Pc_Painel();
                else
                    pcPainel = _AppServicoPc_Painel.ObterPorId(item.Pc_Painel_Id);


                pcPainel.Cadastro_Painel_Id = item.Cadastro_Painel_Id;
                pcPainel.Cadastro_Pc_Id = item.Cadastro_Pc_Id;

                if (item.Pc_Painel_Id == 0)
                    _AppServicoPc_Painel.Adicionar(pcPainel);
                else
                    _AppServicoPc_Painel.Update(pcPainel);
            }

        }

        private void SalvarUsuario(List<Usuario> listaUsuario)
        {
            Usuario usuario;

            List<Usuario> cadastroExistente;
            cadastroExistente = _AppServicoUsuario.ObterTodos().ToList();
            foreach (var item in cadastroExistente)
            {
                var excluir = _AppServicoUsuario.ObterPorId(item.Id_Usuario);

                if (listaUsuario.Where(p => p.Id_Usuario == excluir.Id_Usuario).FirstOrDefault() == null)
                    _AppServicoUsuario.Remove(excluir);
            }

            foreach (var item in listaUsuario)
            {
                if (item.Id_Usuario == 0)
                    usuario = new Usuario();
                else
                    usuario = _AppServicoUsuario.ObterPorId(item.Id_Usuario);

                usuario.Alterar_Status_Senha = item.Alterar_Status_Senha;
                usuario.Cadastrar_Painel = item.Cadastrar_Painel;
                usuario.Cadastrar_Pc = item.Cadastrar_Pc;
                usuario.Cadastrar_Usuario = item.Cadastrar_Usuario;
                usuario.Chamar_Senha_Cancelada = item.Chamar_Senha_Cancelada;
                usuario.Chamar_Senha_Fora_Sequencia = item.Chamar_Senha_Fora_Sequencia;
                usuario.Configurar_Botoes = item.Configurar_Botoes;
                usuario.Configurar_Mensagem = item.Configurar_Mensagem;
                usuario.Configurar_Senha = item.Configurar_Senha;
                usuario.Master = item.Master;
                usuario.NomeUsu = item.NomeUsu;
                usuario.Qualificacao = item.Qualificacao;
                usuario.Senha = item.Senha;

                if (item.Id_Usuario == 0)
                    _AppServicoUsuario.Adicionar(usuario);
                else
                    _AppServicoUsuario.Update(usuario);
            }

        }




        private void RemoveSalvarCadastroPainel()
        {
            foreach (var item in _listaCadastroPainel)
            {

                var remove = _AppServicoCadastro_Painel.ObterPorId(item.Cadastro_Painel_Id);

                if (remove != null)
                    _AppServicoCadastro_Painel.Remove(item);
            }

            SalvarCadastroPainel(_listaCadastroPainelRemover);

        }

        private void RemoveSalvarCadastroPc()
        {
            foreach (var item in _listaCadastroPc)
            {
                var remove = _AppServicoCadastro_Pc.ObterPorId(item.Cadastro_Pc_Id);

                if (remove != null)
                    _AppServicoCadastro_Pc.Remove(item);
            }

            SalvarCadastroPc(_listaCadastroPcRemover);
        }

        private void RemoveSalvarMensagem()
        {
            foreach (var item in _listaMensagem)
            {
                var remove = _AppServicoMensagem.ObterPorId(item.Mensagem_Id);

                if (remove != null)
                    _AppServicoMensagem.Remove(item);
            }

            SalvarMensagem(_listaMensagemRemover);

        }

        private void RemoveSalvarParamentros()
        {
            var remove = _AppServicoParametros.ObterPorId(_parametros.Parametro_Id);

            if (remove != null)
                _AppServicoParametros.Remove(_parametros);

            SalvarParamentros(_parametrosRemover);
        }

        private void RemoveSalvarPcPainel()
        {
            foreach (var item in _listaPcPainel)
            {
                var remove = _AppServicoPc_Painel.ObterPorId(item.Pc_Painel_Id);

                if (remove != null)
                    _AppServicoPc_Painel.Remove(item);
            }

            SalvarPcPainel(_listaPcPainelRemover);
        }

        private void RemoveSalvarUsuario()
        {
            foreach (var item in _listaUsuario)
            {
                var remove = _AppServicoUsuario.ObterPorId(item.Id_Usuario);

                if (remove != null)
                    _AppServicoUsuario.Remove(item);
            }

            SalvarUsuario(_listaUsuarioRemover);
        }
    }
}
