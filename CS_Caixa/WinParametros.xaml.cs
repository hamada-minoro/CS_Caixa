using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Objetos_de_Valor;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinParametros.xaml
    /// </summary>
    public partial class WinParametros : Window
    {

        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();
        RepositorioUsuario _AppServicoUsuario = new RepositorioUsuario();
        RepositorioPc_Painel _AppServicoPc_Painel = new RepositorioPc_Painel();
        RepositorioMensagem _AppServicoMensagem = new RepositorioMensagem();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioControle_Uso _AppServicoControle_Uso = new RepositorioControle_Uso();

        List<Cadastro_Painel> listaCadastroPainel = new List<Cadastro_Painel>();
        List<Cadastro_Pc> listaCadastro_Pc = new List<Cadastro_Pc>();
        List<Pc_Painel> listaPc_Painel = new List<Pc_Painel>();
        List<Mensagem> listaMensagens = new List<Mensagem>();
        Parametro parametros = new Parametro();
        List<VoiceInfo> listaVozes = new List<VoiceInfo>();

        string statusPainel = string.Empty;
        string statusMaquina = string.Empty;
        List<SetorAtendimento> listaSetores = new List<SetorAtendimento>();
        bool carregarComboSetor = false;
        List<Controle_Uso> controles;
        Controle_Uso controle;

        private Usuario _usuario;
        string acao = "pronto";
        List<Usuario> usuarios;
        string verificarSeAlterou;
        Apresentacao _inicio;
        string _destino;

        bool primeiroAcesso = false;

        string novoCodigoIntalacao;

        bool comboTipoSenha = false;

        WinLogin _login;

        public WinParametros(Usuario usuario, Apresentacao inicio, string destino, WinLogin login)
        {
            _usuario = usuario;
            _inicio = inicio;
            _destino = destino;
            _login = login;
            InitializeComponent();
        }

        public WinParametros(Usuario usuario, Apresentacao inicio)
        {
            _usuario = usuario;
            _inicio = inicio;
            InitializeComponent();
        }

        public WinParametros(Apresentacao inicio)
        {
            _inicio = inicio;
            _usuario = new Usuario()
            {
                Alterar_Status_Senha = false,
                Cadastrar_Painel = true,
                Cadastrar_Pc = true,
                Cadastrar_Usuario = true,
                Chamar_Senha_Cancelada = false,
                Chamar_Senha_Fora_Sequencia = false,
                Configurar_Botoes = false,
                Configurar_Mensagem = false,
                Configurar_Senha = false,
                Master = false,
                NomeUsu = "Primeiro Acesso",
                Qualificacao = "",
                Id_Usuario = -1
            };
            primeiroAcesso = true;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();

            if (parametros != null)
                if (parametros.CodigoInstalacao == "")
                {
                    string codigo = ClassGerarAleatorio.NumerosLetrasAleatorias(7);

                    novoCodigoIntalacao = ClassCriptografia.Encrypt(codigo);

                    txtCodigoInstalacao.Text = string.Format("Código de Instalação: {0}", ClassCriptografia.Decrypt(novoCodigoIntalacao));

                    parametros.CodigoInstalacao = novoCodigoIntalacao;
                }
                else
                    novoCodigoIntalacao = parametros.CodigoInstalacao;


            controles = new List<Controle_Uso>();
            controles = _AppServicoControle_Uso.ObterTodos().ToList();

            controle = controles.Where(p => p.AtivacaoUso == ClassCriptografia.Encrypt("V")).FirstOrDefault();



            CarregandoForm();
            carregarComboSetor = true;
            CarregarComboSetor();
            VerificarHabilitadosSetor();
            VerificarHabilitadosBotoes();

            comboTipoSenha = true;

            if (cmbTipoSenha.SelectedIndex < 1)
                cmbQtdCaracter.IsEnabled = false;


            if (controle == null)
            {
                DesbilitarTabItems("Controle de Versão");
                tabItemControle.IsSelected = true;
                txtLicenca.IsEnabled = true;
            }

            if (parametros != null)
                if (parametros.CodigoInstalacao == "")
                {
                    DesbilitarTabItems("Controle de Versão");
                    tabItemControle.IsSelected = true;
                    txtLicenca.IsEnabled = true;

                    foreach (var item in controles)
                    {
                        Controle_Uso cont = _AppServicoControle_Uso.ObterPorId(item.ControleId);


                        _AppServicoControle_Uso.Remove(cont);


                    }
                }

            if (parametros == null)
                InicioParametros();
        }

        private void InicioParametros()
        {
            ckbModoRetiradaManual.IsChecked = false;

            ckbHabilitarBotao1.IsChecked = true;
            cmbLetraBotao1.Text = "G";
            cmbVozTipo1.SelectedItem = listaVozes.Where(p => p.Name.Contains("Vitória")).FirstOrDefault();

            ckbHabilitarBotao2.IsChecked = true;
            cmbLetraBotao2.Text = "P";
            cmbVozTipo2.SelectedItem = listaVozes.Where(p => p.Name.Contains("Vitória")).FirstOrDefault();

            ckbHabilitarBotao3.IsChecked = true;
            cmbLetraBotao3.Text = "E";
            cmbVozTipo3.SelectedItem = listaVozes.Where(p => p.Name.Contains("Vitória")).FirstOrDefault();

            if (usuarios.Count == 0)
            {
                tabItemCadastrarUsuario.IsSelected = true;
                AdicionarUsuario();
                txtNomeUsuario.Focus();
            }
        }

        private void CarregandoForm()
        {
            listaCadastroPainel = _AppServicoCadastro_Painel.ObterTodos().ToList();
            dataGridCadastroPainel.ItemsSource = listaCadastroPainel;


            string path2 = @"\\SERVIDOR\CS_Sistemas\CS_Caixa\Beeps";
            DirectoryInfo Dir2 = new DirectoryInfo(path2);
            FileInfo[] Files2 = Dir2.GetFiles();

            List<string> beeps = new List<string>();

            foreach (FileInfo File2 in Files2)
            {
                beeps.Add(File2.Name);
            }

            cmbBeeps.ItemsSource = beeps;


            string path = @"\\SERVIDOR\CS_Sistemas\CS_Caixa\Resources";
            DirectoryInfo Dir = new DirectoryInfo(path);
            FileInfo[] Files = Dir.GetFiles();

            foreach (FileInfo File in Files)
            {
                cmbBeeps.SelectedItem = File.Name;
            }


            if (_usuario != null)
            {
                if (_usuario.Id_Usuario == -1)
                {
                    cmbTipoSenha.SelectedIndex = 0;
                    cmbQtdCaracter.SelectedIndex = 0;
                    cmbLetraBotao1.Text = "C";
                    cmbLetraBotao2.Text = "P";
                    cmbLetraBotao3.Text = "E";
                }

                if (_usuario.Configurar_Botoes == true || _usuario.Id_Usuario == -1)
                    btnConfigurarBotoes.IsEnabled = true;
                else
                    btnConfigurarBotoes.IsEnabled = false;

                if (_usuario.Configurar_Senha == true || _usuario.Id_Usuario == -1)
                    ConfiguracaoSenha.IsEnabled = true;
                else
                    ConfiguracaoSenha.IsEnabled = false;
            }
            else
            {
                gridBotoes.IsEnabled = false;
                ConfiguracaoSenha.IsEnabled = false;
            }

            if (dataGridCadastroPainel.Items.Count > 0)
                dataGridCadastroPainel.SelectedIndex = 0;

            listaCadastro_Pc = _AppServicoCadastro_Pc.ObterTodos().ToList();
            dataGridCadastroMaquina.ItemsSource = listaCadastro_Pc;
            if (listaCadastro_Pc.Count > 0)
                dataGridCadastroMaquina.SelectedIndex = 0;


            cmbMaquinaEstacao.ItemsSource = listaCadastro_Pc.Where(p => p.Tipo_Entrada == 1);
            cmbMaquinaEstacao.DisplayMemberPath = "Nome_Pc";

            cmbMaquinaPainel.ItemsSource = listaCadastroPainel;
            cmbMaquinaPainel.DisplayMemberPath = "Nome_Pc";


            usuarios = _AppServicoUsuario.ObterTodos().OrderBy(p => p.NomeUsu).ToList();



            if (usuarios.Count > 0)
            {
                cmbUsuario.ItemsSource = usuarios;
                cmbUsuario.DisplayMemberPath = "NomeUsu";
                if (_usuario != null)
                    cmbUsuario.SelectedItem = usuarios.Where(p => p.Id_Usuario == _usuario.Id_Usuario).FirstOrDefault();

            }
            btnSalvarUsuario.IsEnabled = false;
            groupUsuarioSenha.IsEnabled = false;
            groupPermissoes.IsEnabled = false;

            listaPc_Painel = _AppServicoPc_Painel.ObterTodos().ToList();

            listaMensagens = _AppServicoMensagem.ObterTodos().ToList();

            dataGridCadastroMensagem.ItemsSource = listaMensagens;

            listaVozes = ClassFalarTexto.CarregaComboVozes();

            cmbVozTipo1.ItemsSource = listaVozes;
            cmbVozTipo1.DisplayMemberPath = "Name";
            cmbVozTipo2.ItemsSource = listaVozes;
            cmbVozTipo2.DisplayMemberPath = "Name";
            cmbVozTipo3.ItemsSource = listaVozes;
            cmbVozTipo3.DisplayMemberPath = "Name";

            cmbVozSetor1.ItemsSource = listaVozes;
            cmbVozSetor1.DisplayMemberPath = "Name";
            cmbVozSetor2.ItemsSource = listaVozes;
            cmbVozSetor2.DisplayMemberPath = "Name";
            cmbVozSetor3.ItemsSource = listaVozes;
            cmbVozSetor3.DisplayMemberPath = "Name";
            cmbVozSetor4.ItemsSource = listaVozes;
            cmbVozSetor4.DisplayMemberPath = "Name";

            InicioCadastroPainel();
            InicioCadastroMaquina();
            InicioCadastroUsuario();
            CarregarParametros();
            IniciaMensageiro();


            if (_usuario != null)
            {
                lblUsuario.Content = string.Format("Usuário: {0}", _usuario.NomeUsu);

                if (_usuario.Master != true && _usuario.NomeUsu != "Administrador" && _usuario.Cadastrar_Usuario != true && _usuario.Id_Usuario != -1)
                {
                    btnExcluir.IsEnabled = false;
                    btnAdicionar.IsEnabled = false;
                    cmbUsuario.IsEnabled = false;
                }

                if (_usuario.Master != true && _usuario.NomeUsu != "Administrador" && _usuario.Cadastrar_Painel != true && _usuario.Id_Usuario != -1)
                {
                    btnAdicionarPainel.IsEnabled = false;
                    btnRemoverPainel.IsEnabled = false;
                }

                if (_usuario.Master != true && _usuario.NomeUsu != "Administrador" && _usuario.Cadastrar_Pc != true && _usuario.Id_Usuario != -1)
                {
                    btnAdicionarMaquina.IsEnabled = false;
                    btnRemoverMaquina.IsEnabled = false;
                }

                if (_usuario.Master != true && _usuario.NomeUsu != "Administrador" && _usuario.Configurar_Botoes != true && _usuario.Id_Usuario != -1)
                {
                    gridBotoes.IsEnabled = false;
                }

                if (_usuario.Master != true && _usuario.NomeUsu != "Administrador" && _usuario.Configurar_Senha != true && _usuario.Id_Usuario != -1)
                {
                    gridConfiguracoesSenha.IsEnabled = false;
                }

                if (_usuario.Master != true && _usuario.NomeUsu != "Administrador" && _usuario.Configurar_Mensagem != true && _usuario.Id_Usuario != -1)
                {
                    btnAdicionarMensagem.IsEnabled = false;
                    btnRemoverMensagem.IsEnabled = false;
                }

            }
        }

        private void DesbilitarTabItems(string tabItem)
        {

            tabItemCadastrarPainel.IsEnabled = false;
            tabItemCadastrarMaquina.IsEnabled = false;
            tabItemCadastrarUsuario.IsEnabled = false;
            tabItemRelacaoPainel.IsEnabled = false;
            tabItemRetiradaSenhas.IsEnabled = false;
            tabItemMensageiro.IsEnabled = false;
            tabItemConfiguracoesSistema.IsEnabled = false;


            tabItemControle.IsEnabled = false;

            btnSalvarTudo.IsEnabled = false;
            btnOk.IsEnabled = false;

            switch (tabItem)
            {
                case "Cadastrar Painel":
                    tabItemCadastrarPainel.IsEnabled = true;
                    break;
                case "Cadastrar Máquinas":
                    tabItemCadastrarMaquina.IsEnabled = true;
                    break;
                case "Cadastrar Usuários":
                    tabItemCadastrarUsuario.IsEnabled = true;
                    break;
                case "Relação Pc/Painel":
                    tabItemRelacaoPainel.IsEnabled = true;
                    break;
                case "Retirada de Senhas":
                    tabItemRetiradaSenhas.IsEnabled = true;
                    break;
                case "Mensageiro":
                    tabItemMensageiro.IsEnabled = true;
                    break;
                case "Configuraçoes do Sistema":
                    tabItemConfiguracoesSistema.IsEnabled = true;
                    break;
                case "Controle de Versão":
                    if (parametros != null)
                        tabItemControle.IsEnabled = true;
                    break;
                default:
                    tabItemCadastrarPainel.IsEnabled = true;
                    tabItemCadastrarMaquina.IsEnabled = true;
                    tabItemCadastrarUsuario.IsEnabled = true;
                    tabItemRelacaoPainel.IsEnabled = true;
                    tabItemRetiradaSenhas.IsEnabled = true;
                    tabItemMensageiro.IsEnabled = true;
                    tabItemConfiguracoesSistema.IsEnabled = true;
                    if (parametros != null)
                        tabItemControle.IsEnabled = true;
                    btnSalvarTudo.IsEnabled = true;
                    btnOk.IsEnabled = true;
                    break;
            }
        }

        private void CarregarProximaLicenca()
        {
            if (controle != null)
            {
                Controle_Uso proximoControle = controles.Where(p => p.ControleId > controle.ControleId).FirstOrDefault();

                if (proximoControle != null)
                {
                    txtLicenca.IsEnabled = false;
                    gridProximaLicenca.Visibility = Visibility.Visible;
                    lblCodigoProximaLicenca.Content = string.Format("Próxima Licença: {0}", proximoControle.CodigoAtivacao);
                    lblVersaoProximaLicenca.Content = string.Format("Versão: {0}", ClassCriptografia.Decrypt(proximoControle.Versao));
                    lblDataAtivacaoProximaLicenca.Content = string.Format("Data Ativação: {0}", ClassCriptografia.Decrypt(proximoControle.DataAtivacao));
                    lblDataInicioProximaLicenca.Content = string.Format("Data Inicial: {0}", ClassCriptografia.Decrypt(proximoControle.DataValidadeInicio));
                    lblDataFimProximaLicenca.Content = string.Format("Data Final: {0}", ClassCriptografia.Decrypt(proximoControle.DataValidadeFim));
                }
                else
                {
                    if (parametros.CodigoInstalacao.Trim() != "")
                        txtLicenca.IsEnabled = true;
                    gridProximaLicenca.Visibility = Visibility.Hidden;
                }
            }
        }


        private void CarregarParametros()
        {
            if (parametros != null)
            {
                if (parametros.CodigoInstalacao.Trim() == "")
                    txtLicenca.IsEnabled = false;

                if (parametros.CodigoInstalacao != "")
                    txtCodigoInstalacao.Text = string.Format("Código de Instalação: {0}", ClassCriptografia.Decrypt(parametros.CodigoInstalacao));

                if (controle != null)
                {
                    txtVersao.Text = string.Format("{0}", ClassCriptografia.Decrypt(controle.Versao));
                    txtCodigoAtivacao.Text = string.Format("{0}", controle.CodigoAtivacao);
                    if (controle.DataAtivacao != null)
                        txtDataAtivacao.Text = string.Format("{0}", ClassCriptografia.Decrypt(controle.DataAtivacao));

                    txtDataInicio.Text = string.Format("{0}", ClassCriptografia.Decrypt(controle.DataValidadeInicio));
                    txtDataFim.Text = string.Format("{0}", ClassCriptografia.Decrypt(controle.DataValidadeFim));
                    CarregarProximaLicenca();
                }
                else
                    gridProximaLicenca.Visibility = Visibility.Hidden;

                ckbHabilitarBotao1.IsChecked = false;
                ckbHabilitarBotao2.IsChecked = false;
                ckbHabilitarBotao3.IsChecked = false;
                ckbHabilitarSetor1.IsChecked = false;
                ckbHabilitaSetor2.IsChecked = false;
                ckbHabilitarSetor3.IsChecked = false;
                ckbHabilitarSetor4.IsChecked = false;
                ckbZerarSenhaDiaSeguinte.IsChecked = false;

                if (parametros.ModoRetiradaSenhaManual == true)
                    ckbModoRetiradaManual.IsChecked = true;
                else
                    ckbModoRetiradaManual.IsChecked = false;

                if (parametros.Voz_Padrao == true)
                    ckbVozPadrao.IsChecked = true;
                else
                    ckbVozPadrao.IsChecked = false;

                if (parametros.Bip_Aviso == true)
                    ckbBip_Aviso.IsChecked = true;
                else
                    ckbBip_Aviso.IsChecked = false;

                if (parametros.Falar_Senha == true)
                    ckbFalar_Senha.IsChecked = true;
                else
                    ckbFalar_Senha.IsChecked = false;

                if (parametros.Voz_RetiradaSenha == true)
                    ckbFalarRetirada.IsChecked = true;
                else
                    ckbFalarRetirada.IsChecked = false;

                if (parametros.Utilizar_Aleatorio == true)
                    ckbUtilizarAleatorio.IsChecked = true;
                else
                    ckbUtilizarAleatorio.IsChecked = false;

                if (parametros.Habilitado_Botao_1 == true)
                    ckbHabilitarBotao1.IsChecked = true;

                if (parametros.Habilitado_Botao_2 == true)
                    ckbHabilitarBotao2.IsChecked = true;

                if (parametros.Habilitado_Botao_3 == true)
                    ckbHabilitarBotao3.IsChecked = true;

                if (parametros.Habilitado_Setor_1 == true)
                    ckbHabilitarSetor1.IsChecked = true;

                if (parametros.Habilitado_Setor_2 == true)
                    ckbHabilitaSetor2.IsChecked = true;

                if (parametros.Habilitado_Setor_3 == true)
                    ckbHabilitarSetor3.IsChecked = true;

                if (parametros.Habilitado_Setor_4 == true)
                    ckbHabilitarSetor4.IsChecked = true;

                if (parametros.ZerarSenhaDiaSeguinte == true)
                    ckbZerarSenhaDiaSeguinte.IsChecked = true;



                cmbLetraBotao1.Text = parametros.Letra_Botao_1;
                cmbLetraBotao2.Text = parametros.Letra_Botao_2;
                cmbLetraBotao3.Text = parametros.Letra_Botao_3;


                cmbLetraSetor1.Text = parametros.Letra_Setor_1;
                cmbLetraSetor2.Text = parametros.Letra_Setor_2;
                cmbLetraSetor3.Text = parametros.Letra_Setor_3;
                cmbLetraSetor4.Text = parametros.Letra_Setor_4;

                if (parametros.Mostrar_Hora == true)
                    ckbMostrar_Hora.IsChecked = true;
                else
                    ckbMostrar_Hora.IsChecked = false;

                txtNomeBotao1.Text = parametros.Nome_Botao_1;

                txtNomeBotao2.Text = parametros.Nome_Botao_2;

                txtNomeBotao3.Text = parametros.Nome_Botao_3;


                txtNomeSetor1.Text = parametros.Nome_Setor_1;
                txtNomeSetor2.Text = parametros.Nome_Setor_2;
                txtNomeSetor3.Text = parametros.Nome_Setor_3;
                txtNomeSetor4.Text = parametros.Nome_Setor_4;


                cmbVozTipo1.Text = parametros.Voz_Botao_1;

                cmbVozTipo2.Text = parametros.Voz_Botao_2;

                cmbVozTipo3.Text = parametros.Voz_Botao_3;


                cmbVozSetor1.Text = parametros.Voz_Setor_1;
                cmbVozSetor2.Text = parametros.Voz_Setor_2;
                cmbVozSetor3.Text = parametros.Voz_Setor_3;
                cmbVozSetor4.Text = parametros.Voz_Setor_4;


                txtNomeEmpresa.Text = parametros.Nome_Empresa;

                if (parametros.Passar_Mensagem == true)
                    ckbPassar_Mensagem.IsChecked = true;
                else
                    ckbPassar_Mensagem.IsChecked = false;

                if (parametros.CadastroCliente == true)
                    ckbCadastroCliente.IsChecked = true;
                else
                    ckbCadastroCliente.IsChecked = false;


                if (parametros.Qtd_Caracteres_Senha > -1)
                {
                    cmbQtdCaracter.SelectedIndex = parametros.Qtd_Caracteres_Senha;
                }


                txtSaudacao.Text = parametros.Saudacao;

                if (parametros.Tipo_Senha > -1)
                {
                    cmbTipoSenha.SelectedIndex = parametros.Tipo_Senha;
                }

                if (ckbModoRetiradaManual.IsChecked == true)
                {
                    cmbTipoSenha.SelectedIndex = 0;
                    cmbTipoSenha.IsEnabled = false;
                }


                ckbInicioFimExpediente.IsChecked = parametros.InicioFimExpediente;

                ckbDomingo.IsChecked = parametros.Domingo;
                ckbSegunda.IsChecked = parametros.Segunda;
                ckbTerca.IsChecked = parametros.Terca;
                ckbQuarta.IsChecked = parametros.Quarta;
                ckbQuinta.IsChecked = parametros.Quinta;
                ckbSexta.IsChecked = parametros.Sexta;
                ckbSabado.IsChecked = parametros.Sabado;

                if (parametros.HoraInicioExpediente != null)
                {
                    txtHoraInicioExpediente.Text = parametros.HoraInicioExpediente.Substring(0, 2);
                    txtMinutoInicioExpediente.Text = parametros.HoraInicioExpediente.Substring(3, 2);
                    txtSegundoInicioExpediente.Text = parametros.HoraInicioExpediente.Substring(6, 2);
                }

                if (parametros.HoraFimExpediente != null)
                {
                    txtHoraFimExpediente.Text = parametros.HoraFimExpediente.Substring(0, 2);
                    txtMinutoFimExpediente.Text = parametros.HoraFimExpediente.Substring(3, 2);
                    txtSegundoFimExpediente.Text = parametros.HoraFimExpediente.Substring(6, 2);
                }


                ckbDesligarPainel.IsChecked = parametros.DesligarPainel;
                if (parametros.HoraDesligarPainel != null)
                {
                    txtHoraDesligarPainel.Text = parametros.HoraDesligarPainel.Substring(0, 2);
                    txtMinutoDesligarPainel.Text = parametros.HoraDesligarPainel.Substring(3, 2);
                    txtSegundoDesligarPainel.Text = parametros.HoraDesligarPainel.Substring(6, 2);
                }


                ckbDesligarSenha.IsChecked = parametros.DesligarSenha;
                if (parametros.HoraDesligarSenha != null)
                {
                    txtHoraDesligarSenha.Text = parametros.HoraDesligarSenha.Substring(0, 2);
                    txtMinutoDesligarSenha.Text = parametros.HoraDesligarSenha.Substring(3, 2);
                    txtSegundoDesligarSenha.Text = parametros.HoraDesligarSenha.Substring(6, 2);
                }

                ckbDesligarEstacao.IsChecked = parametros.DesligarEstacao;
                if (parametros.HoraDesligarEstacao != null)
                {
                    txtHoraDesligarEstacao.Text = parametros.HoraDesligarEstacao.Substring(0, 2);
                    txtMinutoDesligarEstacao.Text = parametros.HoraDesligarEstacao.Substring(3, 2);
                    txtSegundoDesligarEstacao.Text = parametros.HoraDesligarEstacao.Substring(6, 2);
                }

                txtMensagemInicioExpediente.Text = parametros.MensagemInicioExpediente;
                txtMensagemFimExpediente.Text = parametros.MensagemFimExpediente;
            }
            else
                tabItemControle.IsEnabled = false;
        }

        private void btnSalvarTudo_Click(object sender, RoutedEventArgs e)
        {
            SalvarTudo();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SalvarTudo();
            Application.Current.Shutdown();
            Environment.Exit(Environment.ExitCode);
            
        }

        private void SalvarTudo()
        {
            if (usuarios.Count <= 0)
            {
                MessageBox.Show("É necessário cadastrar um Usuário.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (cmbQtdCaracter.SelectedIndex == -1)
            {
                MessageBox.Show("É necessário informar a quantidade de caracteres para a senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (cmbTipoSenha.SelectedIndex == -1)
            {
                MessageBox.Show("É necessário informar o tipo da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();

            if (parametros == null)
            {
                parametros = new Parametro();
                parametros.DataInstalacao = DateTime.Now;

                string codigo = ClassGerarAleatorio.NumerosLetrasAleatorias(7);

                parametros.CodigoInstalacao = ClassCriptografia.Encrypt(codigo);


            }



            if (ckbBip_Aviso.IsChecked == true)
                parametros.Bip_Aviso = true;
            else
                parametros.Bip_Aviso = false;

            if (ckbFalar_Senha.IsChecked == true)
                parametros.Falar_Senha = true;
            else
                parametros.Falar_Senha = false;

            if (ckbFalarRetirada.IsChecked == true)
                parametros.Voz_RetiradaSenha = true;
            else
                parametros.Voz_RetiradaSenha = false;


            if (ckbHabilitarBotao1.IsChecked == true)
                parametros.Habilitado_Botao_1 = true;
            else
                parametros.Habilitado_Botao_1 = false;

            if (ckbHabilitarBotao2.IsChecked == true)
                parametros.Habilitado_Botao_2 = true;
            else
                parametros.Habilitado_Botao_2 = false;

            if (ckbHabilitarBotao3.IsChecked == true)
                parametros.Habilitado_Botao_3 = true;
            else
                parametros.Habilitado_Botao_3 = false;


            if (ckbHabilitarSetor1.IsChecked == true)
                parametros.Habilitado_Setor_1 = true;
            else
                parametros.Habilitado_Setor_1 = false;

            if (ckbHabilitaSetor2.IsChecked == true)
                parametros.Habilitado_Setor_2 = true;
            else
                parametros.Habilitado_Setor_2 = false;

            if (ckbHabilitarSetor3.IsChecked == true)
                parametros.Habilitado_Setor_3 = true;
            else
                parametros.Habilitado_Setor_3 = false;

            if (ckbHabilitarSetor4.IsChecked == true)
                parametros.Habilitado_Setor_4 = true;
            else
                parametros.Habilitado_Setor_4 = false;

            if (ckbZerarSenhaDiaSeguinte.IsChecked == true)
                parametros.ZerarSenhaDiaSeguinte = true;
            else
                parametros.ZerarSenhaDiaSeguinte = false;


            if (ckbModoRetiradaManual.IsChecked == true)
                parametros.ModoRetiradaSenhaManual = true;
            else
                parametros.ModoRetiradaSenhaManual = false;

            parametros.Letra_Botao_1 = cmbLetraBotao1.Text;

            parametros.Letra_Botao_2 = cmbLetraBotao2.Text;

            parametros.Letra_Botao_3 = cmbLetraBotao3.Text;

            parametros.Letra_Setor_1 = cmbLetraSetor1.Text;

            parametros.Letra_Setor_2 = cmbLetraSetor2.Text;

            parametros.Letra_Setor_3 = cmbLetraSetor3.Text;

            parametros.Letra_Setor_4 = cmbLetraSetor4.Text;

            if (ckbMostrar_Hora.IsChecked == true)
                parametros.Mostrar_Hora = true;
            else
                parametros.Mostrar_Hora = false;

            if (ckbVozPadrao.IsChecked == true)
                parametros.Voz_Padrao = true;
            else
                parametros.Voz_Padrao = false;


            if (ckbModoRetiradaManual.IsChecked == true)
                parametros.ModoRetiradaSenhaManual = true;
            else
                parametros.ModoRetiradaSenhaManual = false;


            parametros.Nome_Botao_1 = txtNomeBotao1.Text;

            parametros.Nome_Botao_2 = txtNomeBotao2.Text;

            parametros.Nome_Botao_3 = txtNomeBotao3.Text;

            parametros.Nome_Setor_1 = txtNomeSetor1.Text;
            parametros.Nome_Setor_2 = txtNomeSetor2.Text;
            parametros.Nome_Setor_3 = txtNomeSetor3.Text;
            parametros.Nome_Setor_4 = txtNomeSetor4.Text;


            parametros.Voz_Botao_1 = cmbVozTipo1.Text;

            parametros.Voz_Botao_2 = cmbVozTipo2.Text;

            parametros.Voz_Botao_3 = cmbVozTipo3.Text;

            parametros.Voz_Setor_1 = cmbVozSetor1.Text;
            parametros.Voz_Setor_2 = cmbVozSetor2.Text;
            parametros.Voz_Setor_3 = cmbVozSetor3.Text;
            parametros.Voz_Setor_4 = cmbVozSetor4.Text;

            parametros.Nome_Empresa = txtNomeEmpresa.Text;

            if (ckbPassar_Mensagem.IsChecked == true)
                parametros.Passar_Mensagem = true;
            else
                parametros.Passar_Mensagem = false;



            if (ckbUtilizarAleatorio.IsChecked == true)
                parametros.Utilizar_Aleatorio = true;
            else
                parametros.Utilizar_Aleatorio = false;

            parametros.Saudacao = txtSaudacao.Text;

            if (cmbTipoSenha.SelectedIndex > -1)
            {
                parametros.Tipo_Senha = cmbTipoSenha.SelectedIndex;
            }

            if (cmbQtdCaracter.SelectedIndex > -1)
            {
                parametros.Qtd_Caracteres_Senha = cmbQtdCaracter.SelectedIndex;
            }

            if (ckbCadastroCliente.IsChecked == true)
                parametros.CadastroCliente = true;
            else
                parametros.CadastroCliente = false;


            parametros.InicioFimExpediente = ckbInicioFimExpediente.IsChecked.Value;
            parametros.Domingo = ckbDomingo.IsChecked.Value;
            parametros.Segunda = ckbSegunda.IsChecked.Value;
            parametros.Terca = ckbTerca.IsChecked.Value;
            parametros.Quarta = ckbQuarta.IsChecked.Value;
            parametros.Quinta = ckbQuinta.IsChecked.Value;
            parametros.Sexta = ckbSexta.IsChecked.Value;
            parametros.Sabado = ckbSabado.IsChecked.Value;
            parametros.HoraInicioExpediente = string.Format("{0}:{1}:{2}", txtHoraInicioExpediente.Text, txtMinutoInicioExpediente.Text, txtSegundoInicioExpediente.Text);
            parametros.HoraFimExpediente = string.Format("{0}:{1}:{2}", txtHoraFimExpediente.Text, txtMinutoFimExpediente.Text, txtSegundoFimExpediente.Text);
            parametros.DesligarPainel = ckbDesligarPainel.IsChecked.Value;
            parametros.HoraDesligarPainel = string.Format("{0}:{1}:{2}", txtHoraDesligarPainel.Text, txtMinutoDesligarPainel.Text, txtSegundoDesligarPainel.Text);
            parametros.DesligarSenha = ckbDesligarSenha.IsChecked.Value;
            parametros.HoraDesligarSenha = string.Format("{0}:{1}:{2}", txtHoraDesligarSenha.Text, txtMinutoDesligarSenha.Text, txtSegundoDesligarSenha.Text);
            parametros.DesligarEstacao = ckbDesligarEstacao.IsChecked.Value;
            parametros.HoraDesligarEstacao = string.Format("{0}:{1}:{2}", txtHoraDesligarEstacao.Text, txtMinutoDesligarEstacao.Text, txtSegundoDesligarEstacao.Text);
            parametros.MensagemInicioExpediente = txtMensagemInicioExpediente.Text;
            parametros.MensagemFimExpediente = txtMensagemFimExpediente.Text;

            foreach (var item in listaCadastro_Pc)
            {
                dataGridCadastroMaquina.SelectedItem = item;

                if (cmbSetor.Text == "")
                    item.SetorId = -1;
            }

            string curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            FileInfo arquivoVerificar = new FileInfo(curDir + @"\SysConf.xml");


            if (primeiroAcesso == true && !arquivoVerificar.Exists && controles.Count == 0 && parametros.Parametro_Id == 0)
            {
                CriarXML();

                int qdt = 6;

                string mesInicio = string.Format("{0:00}", DateTime.Now.Month);

                string ano = DateTime.Now.Year.ToString().Substring(2, 2);

                string dia = string.Format("{0:00}", DateTime.Now.Day.ToString());

                DateTime dataInicio = Convert.ToDateTime(DataPorMes(dia, mesInicio, ano));

                DateTime dataFim = dataInicio.AddMonths(qdt);


                _AppServicoControle_Uso.SalvarControle("S", "V", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, "Demo");


            }

            if (parametros.CodigoInstalacao != "")
            {
                AguardeSalvarParametros salvar = new AguardeSalvarParametros(listaCadastroPainel, listaCadastro_Pc, listaMensagens, parametros, listaPc_Painel, usuarios);
                salvar.Owner = this;
                salvar.ShowDialog();
            }
            else
            {
                MessageBox.Show("Não foi possível verificar o código de instalação. O sistema será encerrado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(Environment.ExitCode);
        }

        private void lblTitulo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        private void DigitarNumeros_Pontos(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25 || key == 144 || key == 148);
        }

        private void DigitarNumeros(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void DigitarLetras(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
        }


        private void DatagridContemItemsParaAtivarOuDestivarBotaoRemover(DataGrid dataGrid, Button botao)
        {
            if (dataGrid.Items.Count > 0)
                botao.IsEnabled = true;
            else
                botao.IsEnabled = false;
        }

        private void PassarDeUmCoponenteParaOutro(object sender, KeyEventArgs e)
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









        // Inicio da aba Cadastro de Painel de Chamadas

        private void InicioCadastroPainel()
        {
            gridCadastroPainel.IsEnabled = false;
            btnSalvarPainel.IsEnabled = false;
            btnRemoverPainel.IsEnabled = false;
            dataGridCadastroPainel.IsEnabled = true;
            btnAdicionarPainel.IsEnabled = true;

            DatagridContemItemsParaAtivarOuDestivarBotaoRemover(dataGridCadastroPainel, btnRemoverPainel);

        }
        private void AbrirCamposParaAdicionarNovoPainel()
        {
            txtIdentificadorPainel.Text = "";
            txtIpPainel.Text = "";
            txtNomePainel.Text = "";
            txtPortaPainel.Text = "";
            gridCadastroPainel.IsEnabled = true;
            btnSalvarPainel.IsEnabled = true;
            dataGridCadastroPainel.IsEnabled = false;
            btnRemoverPainel.IsEnabled = false;
            btnAdicionarPainel.IsEnabled = false;
        }




        private void txtPortaPainel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void btnAdicionarPainel_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Cadastrar_Painel == true)
            {
                statusPainel = "novo";
                AbrirCamposParaAdicionarNovoPainel();
                ObterDadosPainel();
                DesbilitarTabItems(tabItemCadastrarPainel.Header.ToString());
            }
            else
                MessageBox.Show("Usuário logado não tem permissão para adicionar novo painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void btnRemoverPainel_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Excluir Painel de Chamadas?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            if (dataGridCadastroPainel.SelectedItem != null)
            {
                var removerPainel = (Cadastro_Painel)dataGridCadastroPainel.SelectedItem;
                listaCadastroPainel.Remove(removerPainel);
                dataGridCadastroPainel.Items.Refresh();
                var itensExcluir = listaPc_Painel.Where(p => p.Cadastro_Painel_Id == removerPainel.Identificador_Pc).ToList();
                foreach (var item in itensExcluir)
                {

                    if (item.Cadastro_Painel_Id == item.Cadastro_Painel_Id)
                        listaPc_Painel.Remove(item);

                }
                cmbMaquinaPainel.Items.Refresh();
                CarregarGridPc_Painel();
            }
        }

        private void btnCancelarPainel_Click(object sender, RoutedEventArgs e)
        {
            InicioCadastroPainel();
            DesbilitarTabItems("LiberarSalvarTudo");
            SelecionaPainel();
        }

        private void ObterDadosPainel()
        {
            try
            {
                txtIpPainel.Text = _AppServicoCadastro_Pc.ObterIpMaquina();
                txtNomePainel.Text = _AppServicoCadastro_Pc.ObterNomeMaquina();
                txtPortaPainel.Text = "2502";
                txtIdentificadorPainel.Text = _AppServicoCadastro_Pc.ObterIdentificadorPc();
            }
            catch (Exception)
            {
                txtIdentificadorPainel.Text = txtNomePainel.Text;
            }

        }


        private void btnSalvarPainel_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Cadastrar_Painel == true)
            {

                if (txtIdentificadorPainel.Text == "" || txtIpPainel.Text == "" || txtNomePainel.Text == "" || txtPortaPainel.Text == "")
                {
                    MessageBox.Show("É necessário preencher todos os parâmetros", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }

                Cadastro_Painel cadastroPainel = new Cadastro_Painel();

                DesbilitarTabItems("LiberarSalvarTudo");

                if (statusPainel == "novo")
                {

                    cadastroPainel.Data_Cadastro = DateTime.Now.Date;

                    cadastroPainel.Identificador_Pc = txtIdentificadorPainel.Text;

                    cadastroPainel.Ip_Pc = txtIpPainel.Text;

                    cadastroPainel.Nome_Pc = txtNomePainel.Text;

                    cadastroPainel.Porta_Pc = Convert.ToInt16(txtPortaPainel.Text);

                    var existe = listaCadastroPainel.Where(p => p.Identificador_Pc == cadastroPainel.Identificador_Pc).FirstOrDefault();
                    if (existe != null)
                    {
                        MessageBox.Show("Painel de Chamadas já cadastrado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        InicioCadastroPainel();
                        DesbilitarTabItems("LiberarSalvarTudo");
                        SelecionaPainel();
                        return;
                    }


                    var existePc = listaCadastro_Pc.Where(p => p.Identificador_Pc == cadastroPainel.Identificador_Pc).FirstOrDefault();

                    if (existePc != null)
                    {
                        MessageBox.Show("Painel de Chamadas já cadastrado como Máquina Estação.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        InicioCadastroPainel();
                        DesbilitarTabItems("LiberarSalvarTudo");
                        SelecionaPainel();
                        return;
                    }

                    listaCadastroPainel.Add(cadastroPainel);

                    dataGridCadastroPainel.ItemsSource = null;

                    dataGridCadastroPainel.ItemsSource = listaCadastroPainel;
                    dataGridCadastroPainel.Items.Refresh();
                    cmbMaquinaPainel.Items.Refresh();

                    dataGridCadastroPainel.SelectedItem = cadastroPainel;
                    dataGridCadastroPainel.ScrollIntoView(cadastroPainel);
                }
                else
                {
                    var alterar = (Cadastro_Painel)dataGridCadastroPainel.SelectedItem;

                    alterar.Identificador_Pc = txtIdentificadorPainel.Text;

                    alterar.Ip_Pc = txtIpPainel.Text;

                    alterar.Nome_Pc = txtNomePainel.Text;

                    alterar.Porta_Pc = Convert.ToInt16(txtPortaPainel.Text);

                    dataGridCadastroPainel.ItemsSource = null;

                    dataGridCadastroPainel.ItemsSource = listaCadastroPainel;
                    dataGridCadastroPainel.Items.Refresh();

                    cmbMaquinaPainel.Items.Refresh();

                    dataGridCadastroPainel.SelectedItem = alterar;
                    dataGridCadastroPainel.ScrollIntoView(alterar);
                }

                InicioCadastroPainel();


            }
            else
                MessageBox.Show("Usuário logado não tem permissão para adicionar novo painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }


        private void txtIpPainel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros_Pontos(sender, e);
        }

        private void dataGridCadastroPainel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            SelecionaPainel();
        }


        private void SelecionaPainel()
        {
            if (dataGridCadastroPainel.SelectedItem != null)
            {
                var painelSelecionado = (Cadastro_Painel)dataGridCadastroPainel.SelectedItem;

                txtIdentificadorPainel.Text = painelSelecionado.Identificador_Pc;
                txtIpPainel.Text = painelSelecionado.Ip_Pc;
                txtNomePainel.Text = painelSelecionado.Nome_Pc;
                txtPortaPainel.Text = painelSelecionado.Porta_Pc.ToString();
            }
            else
            {
                txtIdentificadorPainel.Text = "";
                txtIpPainel.Text = "";
                txtNomePainel.Text = "";
                txtPortaPainel.Text = "";
            }

            if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Painel == true)
                if (dataGridCadastroPainel.SelectedIndex > -1)
                    btnRemoverPainel.IsEnabled = true;
                else
                    btnRemoverPainel.IsEnabled = false;
        }

        private void dataGridCadastroPainel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Painel == true)
            {
                DesbilitarTabItems(tabItemCadastrarPainel.Header.ToString());

                gridCadastroPainel.IsEnabled = true;
                btnSalvarPainel.IsEnabled = true;
                dataGridCadastroPainel.IsEnabled = false;
                btnRemoverPainel.IsEnabled = false;
                btnAdicionarPainel.IsEnabled = false;

                statusPainel = "alterar";
            }
        }



        // Fim da aba Cadastro de Painel de Chamadas






        // Inicio da aba Cadastro de Máquina Estação
        private void dataGridCadastroMaquina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelecionarMaquina();
        }

        private void SelecionarMaquina()
        {
            if (dataGridCadastroMaquina.SelectedItem != null)
            {
                var maquinaSelecionada = (Cadastro_Pc)dataGridCadastroMaquina.SelectedItem;
                cmbTipoEntradaMaquina.SelectedIndex = maquinaSelecionada.Tipo_Entrada;
                txtIdentificadorMaquina.Text = maquinaSelecionada.Identificador_Pc;
                txtIpMaquina.Text = maquinaSelecionada.Ip_Pc;
                txtNomeMaquina.Text = maquinaSelecionada.Nome_Pc;
                txtPortaMaquina.Text = maquinaSelecionada.Porta_Pc.ToString();
                txtTipoAtendimento.Text = maquinaSelecionada.Tipo_Atendimento;
                txtFalar.Text = maquinaSelecionada.FalaOutros;
                txtCaracter.Text = maquinaSelecionada.Caracter;
                cmbSetor.SelectedIndex = maquinaSelecionada.SetorId;
                cmbTipoChamadaSenha.SelectedIndex = maquinaSelecionada.TipoChamadaSenha;
            }
            else
            {
                txtIdentificadorMaquina.Text = "";
                txtIpMaquina.Text = "";
                txtNomeMaquina.Text = "";
                txtPortaMaquina.Text = "";
                txtTipoAtendimento.Text = "";
                txtCaracter.Text = "";
                cmbTipoEntradaMaquina.SelectedIndex = -1;
                txtFalar.Text = "";
                cmbSetor.SelectedIndex = -1;
                cmbTipoChamadaSenha.SelectedIndex = -1;
            }

            if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Pc == true)
                if (dataGridCadastroMaquina.SelectedIndex > -1)
                    btnRemoverMaquina.IsEnabled = true;
                else
                    btnRemoverMaquina.IsEnabled = false;
        }

        private void dataGridCadastroMaquina_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Pc == true)
            {
                DesbilitarTabItems(tabItemCadastrarMaquina.Header.ToString());

                gridCadastroMaquina.IsEnabled = true;
                btnSalvarMaquina.IsEnabled = true;
                dataGridCadastroMaquina.IsEnabled = false;
                btnRemoverMaquina.IsEnabled = false;
                btnAdicionarMaquina.IsEnabled = false;

                statusMaquina = "alterar";
            }
        }

        private void txtIpMaquina_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros_Pontos(sender, e);
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void txtPortaMaquina_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
            DigitarNumeros(sender, e);
        }



        private void cmbTipoEntradaMaquina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoEntradaMaquina.SelectedIndex <= 0)
            {
                txtCaracter.Text = "";
                txtCaracter.Visibility = Visibility.Hidden;
                lblTipo.Visibility = Visibility.Hidden;
                lblCaracter.Visibility = Visibility.Hidden;
                txtTipoAtendimento.Text = "";
                txtTipoAtendimento.Visibility = Visibility.Hidden;
                txtFalar.Visibility = Visibility.Hidden;
                lblFalar.Visibility = Visibility.Hidden;
                cmbSetor.Visibility = Visibility.Hidden;
                lblSetor.Visibility = Visibility.Hidden;
                cmbTipoChamadaSenha.Visibility = Visibility.Hidden;
                lblTipoChamada.Visibility = Visibility.Hidden;

            }
            else
            {
                txtCaracter.Text = "";
                txtTipoAtendimento.Visibility = Visibility.Visible;
                txtTipoAtendimento.Text = "";
                lblTipo.Visibility = Visibility.Visible;
                txtTipoAtendimento.MaxLength = 50;
                txtFalar.Visibility = Visibility.Visible;
                lblFalar.Visibility = Visibility.Visible;
                txtFalar.Text = "";
                txtCaracter.Visibility = Visibility.Visible;
                lblCaracter.Visibility = Visibility.Visible;
                cmbSetor.Visibility = Visibility.Visible;
                cmbSetor.SelectedIndex = -1;
                lblSetor.Visibility = Visibility.Visible;
                cmbTipoChamadaSenha.Visibility = Visibility.Visible;
                lblTipoChamada.Visibility = Visibility.Visible;
            }
        }

        private void txtTipoAtendimento_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void btnSalvarMaquina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbTipoEntradaMaquina.SelectedIndex == 0)
                {

                    if (txtIdentificadorMaquina.Text == "" || txtIpMaquina.Text == "" || txtNomeMaquina.Text == "" || txtPortaMaquina.Text == "" || cmbTipoEntradaMaquina.SelectedIndex < 0)
                    {
                        MessageBox.Show("É necessário preencher IP da Máquina e Porta de Comunicação", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        return;
                    }
                }
                else
                {

                    if (cmbTipoChamadaSenha.SelectedIndex == -1 || txtTipoAtendimento.Text == "" || txtCaracter.Text == "")
                    {
                        MessageBox.Show("É necessário preencher os campos Tipo de Chamada, Tipo e Caracter.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        return;
                    }



                }



                Cadastro_Pc cadastroMaquina = new Cadastro_Pc();

                DesbilitarTabItems("LiberarSalvarTudo");

                if (statusMaquina == "novo")
                {

                    cadastroMaquina.Data_Cadastro = DateTime.Now.Date;

                    cadastroMaquina.Identificador_Pc = txtIdentificadorMaquina.Text;

                    cadastroMaquina.Ip_Pc = txtIpMaquina.Text;

                    cadastroMaquina.Nome_Pc = txtNomeMaquina.Text;

                    cadastroMaquina.Porta_Pc = Convert.ToInt16(txtPortaMaquina.Text);

                    cadastroMaquina.Tipo_Entrada = cmbTipoEntradaMaquina.SelectedIndex;

                    cadastroMaquina.TipoChamadaSenha = cmbTipoChamadaSenha.SelectedIndex;

                    cadastroMaquina.Tipo_Atendimento = txtTipoAtendimento.Text;
                    cadastroMaquina.Caracter = txtCaracter.Text;

                    cadastroMaquina.FalaOutros = txtFalar.Text;

                    if (cmbSetor.Text != "")
                        cadastroMaquina.SetorId = cmbSetor.SelectedIndex;
                    else
                        cadastroMaquina.SetorId = -1;

                    var existe = listaCadastro_Pc.Where(p => p.Identificador_Pc == cadastroMaquina.Identificador_Pc).FirstOrDefault();
                    if (existe != null)
                    {
                        MessageBox.Show("Máquina já cadastrada.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        InicioCadastroMaquina();
                        DesbilitarTabItems("LiberarSalvarTudo");
                        SelecionarMaquina();
                        return;
                    }

                    var existePainel = listaCadastroPainel.Where(p => p.Identificador_Pc == cadastroMaquina.Identificador_Pc).FirstOrDefault();
                    if (existePainel != null)
                    {
                        MessageBox.Show("Máquina já cadastrada como Painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        InicioCadastroMaquina();
                        DesbilitarTabItems("LiberarSalvarTudo");
                        SelecionarMaquina();
                        return;
                    }


                    listaCadastro_Pc.Add(cadastroMaquina);

                    dataGridCadastroMaquina.ItemsSource = null;

                    dataGridCadastroMaquina.ItemsSource = listaCadastro_Pc;
                    dataGridCadastroMaquina.Items.Refresh();
                    //cmbMaquinaEstacao.Items.Refresh();
                    cmbMaquinaEstacao.ItemsSource = listaCadastro_Pc.Where(p => p.Tipo_Entrada == 1);
                    cmbMaquinaEstacao.DisplayMemberPath = "Nome_Pc";

                    dataGridCadastroMaquina.SelectedItem = cadastroMaquina;
                    dataGridCadastroMaquina.ScrollIntoView(cadastroMaquina);
                }
                else
                {
                    var alterar = (Cadastro_Pc)dataGridCadastroMaquina.SelectedItem;

                    alterar.Identificador_Pc = txtIdentificadorMaquina.Text;

                    alterar.Ip_Pc = txtIpMaquina.Text;

                    alterar.Nome_Pc = txtNomeMaquina.Text;

                    alterar.Porta_Pc = Convert.ToInt16(txtPortaMaquina.Text);

                    alterar.Tipo_Entrada = cmbTipoEntradaMaquina.SelectedIndex;

                    alterar.Tipo_Atendimento = txtTipoAtendimento.Text;
                    alterar.Caracter = txtCaracter.Text;

                    alterar.FalaOutros = txtFalar.Text;

                    if (cmbSetor.Text != "")
                        alterar.SetorId = cmbSetor.SelectedIndex;
                    else
                        alterar.SetorId = -1;

                    alterar.TipoChamadaSenha = cmbTipoChamadaSenha.SelectedIndex;

                    dataGridCadastroMaquina.ItemsSource = null;

                    dataGridCadastroMaquina.ItemsSource = listaCadastro_Pc;
                    dataGridCadastroMaquina.Items.Refresh();
                    //cmbMaquinaEstacao.Items.Refresh();
                    cmbMaquinaEstacao.ItemsSource = listaCadastro_Pc.Where(p => p.Tipo_Entrada == 1);
                    cmbMaquinaEstacao.DisplayMemberPath = "Nome_Pc";

                    dataGridCadastroMaquina.SelectedItem = alterar;
                    dataGridCadastroMaquina.ScrollIntoView(alterar);
                }

                InicioCadastroMaquina();

            }
            catch (FormatException)
            {
                MessageBox.Show("Número da porta não está em um formato correto.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btnAdicionarMaquina_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Cadastrar_Pc == true)
            {
                statusMaquina = "novo";
                AbrirCamposParaAdicionarNovoMaquina();
                ObterDadosMaquina();
                DesbilitarTabItems(tabItemCadastrarMaquina.Header.ToString());
            }
            else
                MessageBox.Show("Usuário logado não tem permissão para adicionar nova máquina estação.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void btnRemoverMaquina_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Excluir a Máquina do cadastro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            if (dataGridCadastroMaquina.SelectedItem != null)
            {
                var removerMaquina = (Cadastro_Pc)dataGridCadastroMaquina.SelectedItem;
                listaCadastro_Pc.Remove(removerMaquina);

                dataGridCadastroMaquina.Items.Refresh();
            }
        }

        private void txtIdentificadorMaquina_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

        }

        private void cmbTipoEntradaMaquina_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void txtNomeMaquina_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }



        private void txtCaracter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }


        private void InicioCadastroMaquina()
        {
            gridCadastroMaquina.IsEnabled = false;
            btnSalvarMaquina.IsEnabled = false;
            btnRemoverMaquina.IsEnabled = false;

            dataGridCadastroMaquina.IsEnabled = true;
            btnAdicionarMaquina.IsEnabled = true;

            DatagridContemItemsParaAtivarOuDestivarBotaoRemover(dataGridCadastroMaquina, btnRemoverMaquina);




        }

        private void AbrirCamposParaAdicionarNovoMaquina()
        {
            txtIdentificadorMaquina.Text = "";
            txtIpMaquina.Text = "";
            txtNomeMaquina.Text = "";
            txtPortaMaquina.Text = "";
            cmbTipoEntradaMaquina.SelectedIndex = -1;
            txtCaracter.Text = "";
            txtTipoAtendimento.Text = "";
            gridCadastroMaquina.IsEnabled = true;
            btnSalvarMaquina.IsEnabled = true;
            dataGridCadastroMaquina.IsEnabled = false;
            btnRemoverMaquina.IsEnabled = false;
            btnAdicionarMaquina.IsEnabled = false;
        }

        private void ObterDadosMaquina()
        {
            try
            {

                txtNomeMaquina.Text = _AppServicoCadastro_Pc.ObterNomeMaquina();
                txtIpMaquina.Text = _AppServicoCadastro_Pc.ObterIpMaquina();
                txtPortaMaquina.Text = "2505";
                txtIdentificadorMaquina.Text = _AppServicoCadastro_Pc.ObterIdentificadorPc();
            }
            catch (Exception)
            {
                txtIdentificadorMaquina.Text = txtNomeMaquina.Text;
            }

        }


        private void gridInternoMaquina_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                InicioCadastroMaquina();
        }

        private void btnCancelarMaquina_Click(object sender, RoutedEventArgs e)
        {
            InicioCadastroMaquina();
            DesbilitarTabItems("LiberarSalvarTudo");
            SelecionarMaquina();
        }

        // Fim da aba Cadastro de Máquina Estação




        // Inicio da aba Cadastro de Usuários

        private void InicioCadastroUsuario()
        {

            FecharCamposUsuario();

            if (usuarios.Count > 0)
                CarregarCamposUsuario();
        }

        private void cmbUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarregarCamposUsuario();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            AdicionarUsuario();
        }


        private void AdicionarUsuario()
        {
            if (_usuario.Cadastrar_Usuario == true)
            {
                AbrirCamposUsuario();
                txtNomeUsuario.Text = "";
                passSenha.Password = "";
                txtQualificacao.Text = "";
                acao = "adicionar";
                DesbilitarTabItems(tabItemCadastrarUsuario.Header.ToString());

                if (usuarios.Count > 0)
                {
                    ckbMaster.IsChecked = false;
                    ckbAlterar_Status_Senha.IsChecked = false;
                    ckbCadastrar_Painel.IsChecked = false;
                    ckbCadastrar_Pc.IsChecked = false;
                    ckbCadastrar_Usuario.IsChecked = false;
                    ckbChamar_Senha_Cancelada.IsChecked = false;
                    ckbChamar_Senha_Fora_Sequencia.IsChecked = false;
                    ckbConfigurar_Botoes.IsChecked = false;
                    ckbConfigurar_Mensagem.IsChecked = false;
                    ckbConfigurar_Senha.IsChecked = false;
                    ckbMaster.IsEnabled = true;
                }
                else
                {
                    ckbMaster.IsChecked = true;
                    ckbMaster.IsEnabled = false;
                }
            }
            else
                MessageBox.Show("Usuário logado não tem permissão para adicionar novo usuário.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void AbrirCamposUsuario()
        {
            groupUsuarioSenha.IsEnabled = true;

            if (_usuario != null)
            {
                if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Usuario == true)
                {
                    groupPermissoes.IsEnabled = true;
                }
                else
                {
                    groupPermissoes.IsEnabled = false;
                }


            }
            else
            {
                ckbMaster.IsChecked = true;

            }



            btnSalvarUsuario.IsEnabled = true;
            btnCancelarUsuario.IsEnabled = true;
            txtNomeUsuario.Focus();
            txtNomeUsuario.SelectAll();
            cmbUsuario.IsEnabled = false;
            toolBarPanel.IsEnabled = false;

        }


        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            AbrirCamposUsuario();
            DesbilitarTabItems(tabItemCadastrarUsuario.Header.ToString());
            passSenha.Password = _AppServicoUsuario.DecriptogravarSenha(passSenha.Password);
            acao = "alterar";
            verificarSeAlterou = string.Format("{0}{1}", txtNomeUsuario.Text, passSenha.Password);
        }

        private void FecharCamposUsuario()
        {

            toolBarPanel.IsEnabled = true;

            if (_usuario != null)
            {
                if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Usuario == true)
                {
                    groupUsuarioSenha.IsEnabled = false;
                    groupPermissoes.IsEnabled = false;
                    btnSalvarUsuario.IsEnabled = false;
                    btnCancelarUsuario.IsEnabled = false;
                    cmbUsuario.IsEnabled = true;
                }
                else
                {
                    btnAdicionar.IsEnabled = false;
                    btnExcluir.IsEnabled = false;
                    btnAlterar.IsEnabled = false;
                    btnSalvarUsuario.IsEnabled = false;
                    btnCancelarUsuario.IsEnabled = false;
                    groupUsuarioSenha.IsEnabled = false;
                    groupPermissoes.IsEnabled = false;
                }
            }
            else
            {
                btnExcluir.IsEnabled = false;
                btnAlterar.IsEnabled = false;
                btnSalvarUsuario.IsEnabled = false;
                btnCancelarUsuario.IsEnabled = false;
                groupUsuarioSenha.IsEnabled = false;
                groupPermissoes.IsEnabled = false;
            }

        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuarioExcluir = (Usuario)cmbUsuario.SelectedItem;


            if (usuarioExcluir != null)
            {
                if (usuarioExcluir.Id_Usuario == _usuario.Id_Usuario)
                {
                    MessageBox.Show("Não é possível excluir o usuário logado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                string usuarioAnterior = usuarioExcluir.NomeUsu;
                if (MessageBox.Show("Deseja realmente excluir o Usuário " + usuarioAnterior + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {

                        usuarios.Remove(usuarioExcluir);

                        cmbUsuario.Items.Refresh();

                        cmbUsuario.SelectedIndex = -1;

                        txtNomeUsuario.Text = "";
                        passSenha.Password = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro inesperado, " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void txtNomeUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void passSenha_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void txtQualificacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void btnCancelarUsuario_Click(object sender, RoutedEventArgs e)
        {
            DesbilitarTabItems("LiberarSalvarTudo");
            FecharCamposUsuario();
            CarregarCamposUsuario();
        }

        private void btnSalvarUsuario_Click(object sender, RoutedEventArgs e)
        {
            Usuario salvarUsuario = new Usuario();


            if (txtNomeUsuario.Text == "")
            {
                MessageBox.Show("Preencha o campo Nome.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtNomeUsuario.Focus();
                return;
            }



            if (passSenha.Password == "")
            {
                MessageBox.Show("Preencha o campo Senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                passSenha.Focus();
                return;
            }


            if (txtQualificacao.Text == "")
            {
                MessageBox.Show("Preencha o campo Qualificação.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtQualificacao.Focus();
                return;
            }



            try
            {
                DesbilitarTabItems("LiberarSalvarTudo");

                if (acao == "alterar")
                {
                    salvarUsuario = (Usuario)cmbUsuario.SelectedItem;
                }
                else
                {
                    salvarUsuario = new Usuario();
                }


                salvarUsuario.NomeUsu = txtNomeUsuario.Text.Trim();
                salvarUsuario.Senha = _AppServicoUsuario.CriptogravarSenha(passSenha.Password);
                salvarUsuario.Qualificacao = txtQualificacao.Text;

                if (ckbMaster.IsChecked == true)
                    salvarUsuario.Master = true;
                else
                    salvarUsuario.Master = false;

                if (ckbAlterar_Status_Senha.IsChecked == true)
                    salvarUsuario.Alterar_Status_Senha = true;
                else
                    salvarUsuario.Alterar_Status_Senha = false;

                if (ckbCadastrar_Painel.IsChecked == true)
                    salvarUsuario.Cadastrar_Painel = true;
                else
                    salvarUsuario.Cadastrar_Painel = false;

                if (ckbCadastrar_Pc.IsChecked == true)
                    salvarUsuario.Cadastrar_Pc = true;
                else
                    salvarUsuario.Cadastrar_Pc = false;

                if (ckbCadastrar_Usuario.IsChecked == true)
                    salvarUsuario.Cadastrar_Usuario = true;
                else
                    salvarUsuario.Cadastrar_Usuario = false;

                if (ckbChamar_Senha_Cancelada.IsChecked == true)
                    salvarUsuario.Chamar_Senha_Cancelada = true;
                else
                    salvarUsuario.Chamar_Senha_Cancelada = false;

                if (ckbChamar_Senha_Fora_Sequencia.IsChecked == true)
                    salvarUsuario.Chamar_Senha_Fora_Sequencia = true;
                else
                    salvarUsuario.Chamar_Senha_Fora_Sequencia = false;

                if (ckbConfigurar_Botoes.IsChecked == true)
                    salvarUsuario.Configurar_Botoes = true;
                else
                    salvarUsuario.Configurar_Botoes = false;

                if (ckbConfigurar_Mensagem.IsChecked == true)
                    salvarUsuario.Configurar_Mensagem = true;
                else
                    salvarUsuario.Configurar_Mensagem = false;

                if (ckbConfigurar_Senha.IsChecked == true)
                    salvarUsuario.Configurar_Senha = true;
                else
                    salvarUsuario.Configurar_Senha = false;

                if (acao == "alterar")
                {
                    var alterar = usuarios.Where(p => p.Id_Usuario == salvarUsuario.Id_Usuario).FirstOrDefault();

                    cmbUsuario.Items.Refresh();

                    cmbUsuario.SelectedIndex = -1;

                    cmbUsuario.Text = salvarUsuario.NomeUsu;
                    txtNomeUsuario.Text = salvarUsuario.NomeUsu;
                    passSenha.Password = salvarUsuario.Senha;
                }
                else
                {
                    usuarios.Add(salvarUsuario);

                    if (_usuario.Id_Usuario == -1)
                    {
                        _usuario = salvarUsuario;

                    }

                    cmbUsuario.ItemsSource = usuarios;
                    cmbUsuario.DisplayMemberPath = "Nome_Usuario";
                    cmbUsuario.Items.Refresh();
                    cmbUsuario.Text = salvarUsuario.NomeUsu;
                }

                MessageBox.Show("Usuário cadastrado com sucesso!", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado, " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            FecharCamposUsuario();
            acao = "pronto";
        }


        private void ckbMaster_Unchecked(object sender, RoutedEventArgs e)
        {
            ckbMaster.IsChecked = false;
            ckbAlterar_Status_Senha.IsChecked = false;
            ckbCadastrar_Painel.IsChecked = false;
            ckbCadastrar_Pc.IsChecked = false;
            ckbCadastrar_Usuario.IsChecked = false;
            ckbChamar_Senha_Cancelada.IsChecked = false;
            ckbChamar_Senha_Fora_Sequencia.IsChecked = false;
            ckbConfigurar_Botoes.IsChecked = false;
            ckbConfigurar_Mensagem.IsChecked = false;
            ckbConfigurar_Senha.IsChecked = false;

            ckbAlterar_Status_Senha.IsEnabled = true;
            ckbCadastrar_Painel.IsEnabled = true;
            ckbCadastrar_Pc.IsEnabled = true;
            ckbCadastrar_Usuario.IsEnabled = true;
            ckbChamar_Senha_Cancelada.IsEnabled = true;
            ckbChamar_Senha_Fora_Sequencia.IsEnabled = true;
            ckbConfigurar_Botoes.IsEnabled = true;
            ckbConfigurar_Mensagem.IsEnabled = true;
            ckbConfigurar_Senha.IsEnabled = true;
        }

        private void ckbMaster_Checked(object sender, RoutedEventArgs e)
        {
            ckbMaster.IsChecked = true;
            ckbAlterar_Status_Senha.IsChecked = true;
            ckbCadastrar_Painel.IsChecked = true;
            ckbCadastrar_Pc.IsChecked = true;
            ckbCadastrar_Usuario.IsChecked = true;
            ckbChamar_Senha_Cancelada.IsChecked = true;
            ckbChamar_Senha_Fora_Sequencia.IsChecked = true;
            ckbConfigurar_Botoes.IsChecked = true;
            ckbConfigurar_Mensagem.IsChecked = true;
            ckbConfigurar_Senha.IsChecked = true;

            ckbAlterar_Status_Senha.IsEnabled = false;
            ckbCadastrar_Painel.IsEnabled = false;
            ckbCadastrar_Pc.IsEnabled = false;
            ckbCadastrar_Usuario.IsEnabled = false;
            ckbChamar_Senha_Cancelada.IsEnabled = false;
            ckbChamar_Senha_Fora_Sequencia.IsEnabled = false;
            ckbConfigurar_Botoes.IsEnabled = false;
            ckbConfigurar_Mensagem.IsEnabled = false;
            ckbConfigurar_Senha.IsEnabled = false;
        }

        private void CarregarCamposUsuario()
        {
            try
            {
                if (cmbUsuario.SelectedIndex > -1)
                {
                    Usuario usu = (Usuario)cmbUsuario.SelectedItem;
                    txtNomeUsuario.Text = usu.NomeUsu;
                    passSenha.Password = usu.Senha;
                    txtQualificacao.Text = usu.Qualificacao;

                    ckbMaster.IsChecked = usu.Master;
                    ckbAlterar_Status_Senha.IsChecked = usu.Alterar_Status_Senha;
                    ckbCadastrar_Painel.IsChecked = usu.Cadastrar_Painel;
                    ckbCadastrar_Pc.IsChecked = usu.Cadastrar_Pc;
                    ckbCadastrar_Usuario.IsChecked = usu.Cadastrar_Usuario;
                    ckbChamar_Senha_Cancelada.IsChecked = usu.Chamar_Senha_Cancelada;
                    ckbChamar_Senha_Fora_Sequencia.IsChecked = usu.Chamar_Senha_Fora_Sequencia;
                    ckbConfigurar_Botoes.IsChecked = usu.Configurar_Botoes;
                    ckbConfigurar_Mensagem.IsChecked = usu.Configurar_Mensagem;
                    ckbConfigurar_Senha.IsChecked = usu.Configurar_Senha;

                    if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Cadastrar_Usuario == true)
                    {
                        btnExcluir.IsEnabled = true;
                        btnAlterar.IsEnabled = true;
                    }
                }
                else
                {
                    btnExcluir.IsEnabled = false;
                    btnAlterar.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Fim da aba Cadastro de Usuários





        // Inicio da aba Relação PC/Painel
        private void btnAdicionarRelacao_Click(object sender, RoutedEventArgs e)
        {


            if (cmbMaquinaEstacao.SelectedItem == null || cmbMaquinaPainel.SelectedItem == null)
            {
                MessageBox.Show("É necessário informar Máquina Estação e Máquina Painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }



            var estacao = (Cadastro_Pc)cmbMaquinaEstacao.SelectedItem;
            var painel = (Cadastro_Painel)cmbMaquinaPainel.SelectedItem;

            Pc_Painel adicionarRelacao = new Pc_Painel();
            adicionarRelacao.Cadastro_Pc_Id = estacao.Identificador_Pc;
            adicionarRelacao.Cadastro_Painel_Id = painel.Identificador_Pc;

            var contem = listaPc_Painel.Where(p => p.Cadastro_Painel_Id == adicionarRelacao.Cadastro_Painel_Id && p.Cadastro_Pc_Id == adicionarRelacao.Cadastro_Pc_Id).FirstOrDefault();

            if (contem != null)
            {
                CarregarGridPc_Painel();
                return;

            }

            listaPc_Painel.Add(adicionarRelacao);

            CarregarGridPc_Painel();
        }

        private void dataGridRelacaoPcPainel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridRelacaoPcPainel.SelectedItem != null)
            {
                btnRemoverRelacao.IsEnabled = true;
            }
            else
            {
                btnRemoverRelacao.IsEnabled = false;
            }
        }

        private void btnRemoverRelacao_Click(object sender, RoutedEventArgs e)
        {
            var painel = (Cadastro_Painel)cmbMaquinaPainel.SelectedItem;

            var maquina = (Cadastro_Pc)dataGridRelacaoPcPainel.SelectedItem;

            var remover = listaPc_Painel.Where(p => p.Cadastro_Painel_Id == painel.Identificador_Pc && p.Cadastro_Pc_Id == maquina.Identificador_Pc).FirstOrDefault();

            listaPc_Painel.Remove(remover);

            CarregarGridPc_Painel();

        }

        private void cmbMaquinaEstacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMaquinaEstacao.SelectedItem != null)
            {
                var itemSelecionado = (Cadastro_Pc)cmbMaquinaEstacao.SelectedItem;

                if (dataGridRelacaoPcPainel.Items.Contains(itemSelecionado))
                {
                    dataGridRelacaoPcPainel.Items.Refresh();
                    dataGridRelacaoPcPainel.SelectedItem = itemSelecionado;
                    dataGridRelacaoPcPainel.ScrollIntoView(itemSelecionado);
                }
                else
                    dataGridRelacaoPcPainel.SelectedIndex = -1;
            }
        }
        private void cmbMaquinaPainel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarregarGridPc_Painel();
        }

        private void CarregarGridPc_Painel()
        {

            var listaPainel = new List<Cadastro_Pc>();

            if (cmbMaquinaPainel.SelectedItem != null)
            {
                var itemSelecionado = (Cadastro_Painel)cmbMaquinaPainel.SelectedItem;

                var listaItens = listaPc_Painel.Where(p => p.Cadastro_Painel_Id == itemSelecionado.Identificador_Pc).ToList();

                foreach (var item in listaItens)
                {
                    foreach (var itemPc in listaCadastro_Pc)
                    {
                        if (item.Cadastro_Pc_Id == itemPc.Identificador_Pc)
                            listaPainel.Add(itemPc);
                    }
                }

                dataGridRelacaoPcPainel.ItemsSource = listaPainel;
                dataGridRelacaoPcPainel.Items.Refresh();

                if (cmbMaquinaEstacao.SelectedItem != null)
                {
                    var maquinaSelecionada = (Cadastro_Pc)cmbMaquinaEstacao.SelectedItem;

                    dataGridRelacaoPcPainel.Items.Refresh();
                    dataGridRelacaoPcPainel.SelectedItem = maquinaSelecionada;
                    dataGridRelacaoPcPainel.ScrollIntoView(maquinaSelecionada);
                }

            }
            else
            {
                dataGridRelacaoPcPainel.ItemsSource = listaPainel;
                dataGridRelacaoPcPainel.Items.Refresh();
                cmbMaquinaEstacao.SelectedIndex = -1;

            }

        }


        // Fim da aba Relação PC/Painel






        // Inicio da aba Parametros
        private void txtNomeBotao1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtNomeEmpresa_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSaudacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitarBotao1_Checked(object sender, RoutedEventArgs e)
        {
            //if (this.IsInitialized)
            //{
            //    txtNomeBotao1.IsEnabled = true;
            //    cmbLetraBotao1.IsEnabled = true;
            //    cmbVozTipo1.IsEnabled = true;
            //    VerificarHabilitadosBotoes();
            //}

            VerificarCheckBoxHabilitarBotoes();
        }

        private void ckbHabilitarBotao1_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (this.IsInitialized)
            //{
            //    txtNomeBotao1.IsEnabled = false;
            //    cmbLetraBotao1.IsEnabled = false;
            //    cmbVozTipo1.IsEnabled = false;
            //    VerificarHabilitadosBotoes();
            //}
            VerificarCheckBoxHabilitarBotoes();
        }

        private void txtLetraBotao1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitarBotao2_Checked(object sender, RoutedEventArgs e)
        {
            //if (this.IsInitialized)
            //{
            //    txtNomeBotao2.IsEnabled = true;
            //    cmbLetraBotao2.IsEnabled = true;
            //    cmbVozTipo2.IsEnabled = true;
            //    VerificarHabilitadosBotoes();
            //}
            VerificarCheckBoxHabilitarBotoes();
        }

        private void ckbHabilitarBotao2_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (this.IsInitialized)
            //{
            //    txtNomeBotao2.IsEnabled = false;
            //    cmbLetraBotao2.IsEnabled = false;
            //    cmbVozTipo2.IsEnabled = false;
            //    VerificarHabilitadosBotoes();
            //}
            VerificarCheckBoxHabilitarBotoes();
        }


        private void txtNomeBotao2_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtLetraBotao2_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitarBotao3_Checked(object sender, RoutedEventArgs e)
        {
            //if (this.IsInitialized)
            //{
            //    txtNomeBotao3.IsEnabled = true;
            //    cmbLetraBotao3.IsEnabled = true;
            //    cmbVozTipo3.IsEnabled = true;
            //    VerificarHabilitadosBotoes();
            //}
            VerificarCheckBoxHabilitarBotoes();
        }

        private void ckbHabilitarBotao3_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (this.IsInitialized)
            //{
            //    txtNomeBotao3.IsEnabled = false;
            //    cmbLetraBotao3.IsEnabled = false;
            //    cmbVozTipo3.IsEnabled = false;
            //    VerificarHabilitadosBotoes();
            //}
            VerificarCheckBoxHabilitarBotoes();
        }

        private void txtLetraBotao3_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtNomeBotao3_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void VerificarCheckBoxHabilitarBotoes()
        {
            if (this.IsInitialized)
            {

                if (ckbHabilitarBotao1.IsChecked == true)
                {
                    txtNomeBotao1.IsEnabled = true;
                    cmbLetraBotao1.IsEnabled = true;
                    cmbVozTipo1.IsEnabled = true;
                }
                else
                {
                    txtNomeBotao1.IsEnabled = false;
                    cmbLetraBotao1.IsEnabled = false;
                    cmbVozTipo1.IsEnabled = false;
                }

                if (ckbHabilitarBotao2.IsChecked == true)
                {
                    txtNomeBotao2.IsEnabled = true;
                    cmbLetraBotao2.IsEnabled = true;
                    cmbVozTipo2.IsEnabled = true;
                }
                else
                {
                    txtNomeBotao2.IsEnabled = false;
                    cmbLetraBotao2.IsEnabled = false;
                    cmbVozTipo2.IsEnabled = false;
                }

                if (ckbHabilitarBotao3.IsChecked == true)
                {
                    txtNomeBotao3.IsEnabled = true;
                    cmbLetraBotao3.IsEnabled = true;
                    cmbVozTipo3.IsEnabled = true;
                }
                else
                {
                    txtNomeBotao3.IsEnabled = false;
                    cmbLetraBotao3.IsEnabled = false;
                    cmbVozTipo3.IsEnabled = false;
                }

                VerificarHabilitadosBotoes();
            }
        }




        // Fim da aba Parametros



        // Inicio da aba Mensagem

        private void IniciaMensageiro()
        {
            txtTextoMensagem.IsEnabled = false;
            ckbPiscaMensagem.IsEnabled = false;
            cmbCorMensagem.IsEnabled = false;
            btnSalvarMensagem.IsEnabled = false;

            if (dataGridCadastroMensagem.Items.Count > 0)
                dataGridCadastroMensagem.SelectedIndex = 0;
        }

        private void txtTextoMensagem_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnAdicionarMensagem_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Configurar_Mensagem == true)
            {
                statusMensagem = "novo";
                DesbilitarTabItems(tabItemMensageiro.Header.ToString());
                btnAdicionarMensagem.IsEnabled = false;
                btnSalvarMensagem.IsEnabled = true;
                btnRemoverMensagem.IsEnabled = false;

                txtTextoMensagem.IsEnabled = true;
                ckbPiscaMensagem.IsEnabled = true;
                cmbCorMensagem.IsEnabled = true;


                dataGridCadastroMensagem.IsEnabled = false;

                txtTextoMensagem.Text = "";
                cmbCorMensagem.SelectedIndex = -1;
                ckbPiscaMensagem.IsChecked = false;

            }
            else
                MessageBox.Show("Usuário logado não tem permissão para adicionar nova mensagem.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void btnRemoverMensagem_Click(object sender, RoutedEventArgs e)
        {
            var mensagemRemover = (Mensagem)dataGridCadastroMensagem.SelectedItem;

            listaMensagens.Remove(mensagemRemover);

            dataGridCadastroMensagem.Items.Refresh();

            if (dataGridCadastroMensagem.Items.Count > 0)
                dataGridCadastroMensagem.SelectedIndex = 0;
        }


        private void dataGridCadastroMensagem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCadastroMensagem.SelectedItem != null)
            {
                btnRemoverMensagem.IsEnabled = true;


                var mensagemSelecionada = (Mensagem)dataGridCadastroMensagem.SelectedItem;

                txtTextoMensagem.Text = mensagemSelecionada.Texto;
                if (mensagemSelecionada.Pisca == true)
                    ckbPiscaMensagem.IsChecked = true;
                else
                    ckbPiscaMensagem.IsChecked = false;

                cmbCorMensagem.SelectedIndex = mensagemSelecionada.Cor;

            }
            else
            {
                btnRemoverMensagem.IsEnabled = false;
            }
        }

        private void btnSalvarMensagem_Click(object sender, RoutedEventArgs e)
        {


            if (txtTextoMensagem.Text == "" || cmbCorMensagem.SelectedIndex == -1)
            {
                MessageBox.Show("É necessário informar o Texto e Cor.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            DesbilitarTabItems("LiberarSalvarTudo");

            btnAdicionarMensagem.IsEnabled = true;
            btnSalvarMensagem.IsEnabled = false;
            btnRemoverMensagem.IsEnabled = true;

            txtTextoMensagem.IsEnabled = false;
            ckbPiscaMensagem.IsEnabled = false;
            cmbCorMensagem.IsEnabled = false;

            dataGridCadastroMensagem.IsEnabled = true;

            Mensagem mensagem;

            if (statusMensagem == "novo")
                mensagem = new Mensagem();
            else
                mensagem = listaMensagens[dataGridCadastroMensagem.SelectedIndex];

            mensagem.Texto = txtTextoMensagem.Text;

            if (ckbPiscaMensagem.IsChecked == true)
                mensagem.Pisca = true;
            else
                mensagem.Pisca = false;

            mensagem.Cor = cmbCorMensagem.SelectedIndex;

            if (statusMensagem == "novo")
                listaMensagens.Add(mensagem);

            if (statusMensagem == "alterando")
            {
                listaMensagens[dataGridCadastroMensagem.SelectedIndex] = mensagem;
            }

            dataGridCadastroMensagem.Items.Refresh();
            dataGridCadastroMensagem.SelectedItem = mensagem;
            dataGridCadastroMensagem.ScrollIntoView(mensagem);
        }

        string statusMensagem = "pronto";

        private void dataGridCadastroMensagem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_usuario.Master == true || _usuario.NomeUsu == "Administrador" || _usuario.Configurar_Mensagem == true)
            {
                DesbilitarTabItems(tabItemMensageiro.Header.ToString());
                statusMensagem = "alterando";

                btnAdicionarMensagem.IsEnabled = false;
                btnSalvarMensagem.IsEnabled = true;
                btnRemoverMensagem.IsEnabled = false;

                txtTextoMensagem.IsEnabled = true;
                ckbPiscaMensagem.IsEnabled = true;
                cmbCorMensagem.IsEnabled = true;


                dataGridCadastroMensagem.IsEnabled = false;
            }
        }


        private void gridMensageiro_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape)
            {
                statusMensagem = "pronto";


                btnAdicionarMensagem.IsEnabled = true;
                btnSalvarMensagem.IsEnabled = false;
                btnRemoverMensagem.IsEnabled = true;

                txtTextoMensagem.IsEnabled = false;
                ckbPiscaMensagem.IsEnabled = false;
                cmbCorMensagem.IsEnabled = false;

                dataGridCadastroMensagem.IsEnabled = true;

                DesbilitarTabItems("LiberarSalvarTudo");

                if (dataGridCadastroMensagem.SelectedItem != null)
                {
                    btnRemoverMensagem.IsEnabled = true;


                    var mensagemSelecionada = (Mensagem)dataGridCadastroMensagem.SelectedItem;

                    txtTextoMensagem.Text = mensagemSelecionada.Texto;
                    if (mensagemSelecionada.Pisca == true)
                        ckbPiscaMensagem.IsChecked = true;
                    else
                        ckbPiscaMensagem.IsChecked = false;

                    cmbCorMensagem.SelectedIndex = mensagemSelecionada.Cor;

                }
                else
                {
                    btnRemoverMensagem.IsEnabled = false;
                }
            }
        }


        private void CarregarComboSetor()
        {

            listaSetores = new List<SetorAtendimento>();
            SetorAtendimento setor;

            if (ckbHabilitarSetor1.IsChecked == true)
            {
                setor = new SetorAtendimento();
                setor.SetorAtendimentoId = 0;
                setor.NomeSetor = txtNomeSetor1.Text;
                setor.LetraSetor = cmbLetraSetor1.Text;
                setor.VozSetor = cmbVozSetor1.Text;
                listaSetores.Add(setor);
            }


            if (ckbHabilitaSetor2.IsChecked == true)
            {
                setor = new SetorAtendimento();
                setor.SetorAtendimentoId = 1;
                setor.NomeSetor = txtNomeSetor2.Text;
                setor.LetraSetor = cmbLetraSetor2.Text;
                setor.VozSetor = cmbVozSetor2.Text;
                listaSetores.Add(setor);
            }

            if (ckbHabilitarSetor3.IsChecked == true)
            {
                setor = new SetorAtendimento();
                setor.SetorAtendimentoId = 2;
                setor.NomeSetor = txtNomeSetor3.Text;
                setor.LetraSetor = cmbLetraSetor3.Text;
                setor.VozSetor = cmbVozSetor3.Text;
                listaSetores.Add(setor);
            }

            if (ckbHabilitarSetor4.IsChecked == true)
            {
                setor = new SetorAtendimento();
                setor.SetorAtendimentoId = 3;
                setor.NomeSetor = txtNomeSetor4.Text;
                setor.LetraSetor = cmbLetraSetor4.Text;
                setor.VozSetor = cmbVozSetor4.Text;
                listaSetores.Add(setor);
            }


            cmbSetor.ItemsSource = listaSetores;
            cmbSetor.DisplayMemberPath = "NomeSetor";
            cmbSetor.SelectedValue = "SetorAtendimentoId";
        }


        private void VerificarFilhosHabilitarBotoes()
        {
            if (ckbHabilitarBotao1.IsEnabled == false && ckbHabilitarBotao1.IsChecked == true)
            {
                txtNomeBotao1.IsEnabled = false;
                cmbLetraBotao1.IsEnabled = false;
                cmbVozTipo1.IsEnabled = false;
            }
            if (ckbHabilitarBotao1.IsEnabled == true && ckbHabilitarBotao1.IsChecked == true)
            {
                txtNomeBotao1.IsEnabled = true;
                cmbLetraBotao1.IsEnabled = true;
                cmbVozTipo1.IsEnabled = true;
            }

            if (ckbHabilitarBotao2.IsEnabled == false && ckbHabilitarBotao2.IsChecked == true)
            {
                txtNomeBotao2.IsEnabled = false;
                cmbLetraBotao2.IsEnabled = false;
                cmbVozTipo2.IsEnabled = false;
            }
            if (ckbHabilitarBotao2.IsEnabled == true && ckbHabilitarBotao2.IsChecked == true)
            {
                txtNomeBotao2.IsEnabled = true;
                cmbLetraBotao2.IsEnabled = true;
                cmbVozTipo2.IsEnabled = true;
            }



            if (ckbHabilitarSetor1.IsEnabled == false && ckbHabilitarSetor1.IsChecked == true)
            {
                txtNomeSetor1.IsEnabled = false;
                cmbLetraSetor1.IsEnabled = false;
                cmbVozSetor1.IsEnabled = false;
            }
            if (ckbHabilitarSetor1.IsEnabled == true && ckbHabilitarSetor1.IsChecked == true)
            {
                txtNomeSetor1.IsEnabled = true;
                cmbLetraSetor1.IsEnabled = true;
                cmbVozSetor1.IsEnabled = true;
            }


            if (ckbHabilitaSetor2.IsEnabled == false && ckbHabilitaSetor2.IsChecked == true)
            {
                txtNomeSetor2.IsEnabled = false;
                cmbLetraSetor2.IsEnabled = false;
                cmbVozSetor2.IsEnabled = false;
            }
            if (ckbHabilitaSetor2.IsEnabled == true && ckbHabilitaSetor2.IsChecked == true)
            {
                txtNomeSetor2.IsEnabled = true;
                cmbLetraSetor2.IsEnabled = true;
                cmbVozSetor2.IsEnabled = true;
            }

            if (ckbHabilitarSetor3.IsEnabled == false && ckbHabilitarSetor3.IsChecked == true)
            {
                txtNomeSetor3.IsEnabled = false;
                cmbLetraSetor3.IsEnabled = false;
                cmbVozSetor3.IsEnabled = false;
            }
            if (ckbHabilitarSetor3.IsEnabled == true && ckbHabilitarSetor3.IsChecked == true)
            {
                txtNomeSetor3.IsEnabled = true;
                cmbLetraSetor3.IsEnabled = true;
                cmbVozSetor3.IsEnabled = true;
            }

        }


        private void ckbHabilitarSetor1_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor1.IsEnabled = true;
                cmbLetraSetor1.IsEnabled = true;
                cmbVozSetor1.IsEnabled = true;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void ckbHabilitarSetor1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor1.IsEnabled = false;
                cmbLetraSetor1.IsEnabled = false;
                cmbVozSetor1.IsEnabled = false;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void txtNomeSetor1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtLetraSetor1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitaSetor2_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor2.IsEnabled = true;
                cmbLetraSetor2.IsEnabled = true;
                cmbVozSetor2.IsEnabled = true;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void ckbHabilitaSetor2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor2.IsEnabled = false;
                cmbLetraSetor2.IsEnabled = false;
                cmbVozSetor2.IsEnabled = false;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void txtNomeSetor2_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitarSetor3_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor3.IsEnabled = true;
                cmbLetraSetor3.IsEnabled = true;
                cmbVozSetor3.IsEnabled = true;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void txtLetraSetor2_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtNomeSetor3_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtLetraSetor3_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitarSetor3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor3.IsEnabled = false;
                cmbLetraSetor3.IsEnabled = false;
                cmbVozSetor3.IsEnabled = false;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void txtNomeSetor4_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtLetraSetor4_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbHabilitarSetor4_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                txtNomeSetor4.IsEnabled = true;
                cmbLetraSetor4.IsEnabled = true;
                cmbVozSetor4.IsEnabled = true;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void ckbHabilitarSetor4_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {

                txtNomeSetor4.IsEnabled = false;
                cmbLetraSetor4.IsEnabled = false;
                cmbVozSetor4.IsEnabled = false;
                if (carregarComboSetor == true)
                {
                    CarregarComboSetor();
                    VerificarHabilitadosSetor();
                }
            }
        }

        private void VerificarHabilitadosBotoes()
        {
            if (ckbHabilitarBotao1.IsChecked == false)
            {
                ckbHabilitarBotao1.IsEnabled = true;
                ckbHabilitarBotao2.IsEnabled = false;
                ckbHabilitarBotao3.IsEnabled = false;
            }
            else
            {
                ckbHabilitarBotao1.IsEnabled = true;
                ckbHabilitarBotao2.IsEnabled = true;
                ckbHabilitarBotao3.IsEnabled = false;


                if (ckbHabilitarBotao2.IsChecked == false)
                {
                    ckbHabilitarBotao1.IsEnabled = true;
                    ckbHabilitarBotao2.IsEnabled = true;
                    ckbHabilitarBotao3.IsEnabled = false;
                }
                else
                {
                    ckbHabilitarBotao1.IsEnabled = false;
                    ckbHabilitarBotao2.IsEnabled = true;
                    ckbHabilitarBotao3.IsEnabled = true;

                    if (ckbHabilitarBotao3.IsChecked == false)
                    {
                        ckbHabilitarBotao1.IsEnabled = false;
                        ckbHabilitarBotao2.IsEnabled = true;
                        ckbHabilitarBotao3.IsEnabled = true;
                    }
                    else
                    {
                        ckbHabilitarBotao1.IsEnabled = false;
                        ckbHabilitarBotao2.IsEnabled = false;
                        ckbHabilitarBotao3.IsEnabled = true;
                    }
                }
            }

        }


        private void VerificarHabilitadosSetor()
        {
            if (ckbHabilitarSetor1.IsChecked == false)
            {
                ckbHabilitarSetor1.IsEnabled = true;
                ckbHabilitaSetor2.IsEnabled = false;
                ckbHabilitarSetor3.IsEnabled = false;
                ckbHabilitarSetor4.IsEnabled = false;
            }
            else
            {
                ckbHabilitarSetor1.IsEnabled = true;
                ckbHabilitaSetor2.IsEnabled = true;
                ckbHabilitarSetor3.IsEnabled = false;
                ckbHabilitarSetor4.IsEnabled = false;

                if (ckbHabilitaSetor2.IsChecked == false)
                {
                    ckbHabilitarSetor1.IsEnabled = true;
                    ckbHabilitaSetor2.IsEnabled = true;
                    ckbHabilitarSetor3.IsEnabled = false;
                    ckbHabilitarSetor4.IsEnabled = false;
                }
                else
                {
                    ckbHabilitarSetor1.IsEnabled = false;
                    ckbHabilitaSetor2.IsEnabled = true;
                    ckbHabilitarSetor3.IsEnabled = true;
                    ckbHabilitarSetor4.IsEnabled = false;

                    if (ckbHabilitarSetor3.IsChecked == false)
                    {
                        ckbHabilitarSetor1.IsEnabled = false;
                        ckbHabilitaSetor2.IsEnabled = true;
                        ckbHabilitarSetor3.IsEnabled = true;
                        ckbHabilitarSetor4.IsEnabled = false;
                    }
                    else
                    {
                        ckbHabilitarSetor1.IsEnabled = false;
                        ckbHabilitaSetor2.IsEnabled = false;
                        ckbHabilitarSetor3.IsEnabled = true;
                        ckbHabilitarSetor4.IsEnabled = true;

                        if (ckbHabilitarSetor4.IsChecked == false)
                        {
                            ckbHabilitarSetor1.IsEnabled = false;
                            ckbHabilitaSetor2.IsEnabled = false;
                            ckbHabilitarSetor3.IsEnabled = true;
                            ckbHabilitarSetor4.IsEnabled = true;
                        }
                        else
                        {
                            ckbHabilitarSetor1.IsEnabled = false;
                            ckbHabilitaSetor2.IsEnabled = false;
                            ckbHabilitarSetor3.IsEnabled = false;
                            ckbHabilitarSetor4.IsEnabled = true;
                        }
                    }
                }
            }

            SelecionarMaquina();


        }

        private void txtNomeSetor1_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbLetraSetor1_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbVozSetor1_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void txtNomeSetor2_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbLetraSetor2_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbVozSetor2_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void txtNomeSetor3_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbLetraSetor3_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbVozSetor3_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void txtNomeSetor4_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbLetraSetor4_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbVozSetor4_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregarComboSetor();
        }

        private void cmbTipoSenha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboTipoSenha == true)
                if (cmbTipoSenha.SelectedIndex < 0)
                {
                    cmbQtdCaracter.SelectedIndex = -1;
                    cmbQtdCaracter.IsEnabled = false;
                }
                else
                {
                    if (cmbTipoSenha.SelectedIndex == 0)
                    {
                        cmbQtdCaracter.SelectedIndex = 0;
                        cmbQtdCaracter.IsEnabled = false;
                    }
                    else
                    {
                        cmbQtdCaracter.SelectedIndex = 0;
                        cmbQtdCaracter.IsEnabled = true;
                    }
                }

        }

        private void btnSalvarBotoes_Click(object sender, RoutedEventArgs e)
        {

            if (ckbHabilitarBotao1.IsChecked == true)
            {
                if (txtNomeBotao1.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Botao Geral.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (cmbLetraBotao1.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Botao Preferencial.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (cmbVozTipo1.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Botao Maior de 80 Anos.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (ckbHabilitarBotao2.IsChecked == true)
            {
                if (txtNomeBotao2.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Botao 2.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbLetraBotao2.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Botao 2.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbVozTipo2.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Botao 2.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (ckbHabilitarBotao3.IsChecked == true)
            {
                if (txtNomeBotao3.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Botao 3.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbLetraBotao3.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Botao 3.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbVozTipo3.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Botao 3.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }


            if (ckbHabilitarSetor1.IsChecked == true)
            {
                if (txtNomeSetor1.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Setor 1.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbLetraSetor1.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Setor 1.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (cmbVozSetor1.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Setor 1.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (ckbHabilitaSetor2.IsChecked == true)
            {
                if (txtNomeSetor2.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Setor 2.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbLetraSetor2.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Setor 2.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbVozSetor2.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Setor 2.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (ckbHabilitarSetor3.IsChecked == true)
            {
                if (txtNomeSetor3.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Setor 3.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbLetraSetor3.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Setor 3.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbVozSetor3.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Setor 3.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (ckbHabilitarSetor4.IsChecked == true)
            {
                if (txtNomeSetor4.Text == "")
                {
                    MessageBox.Show("Informe o Nome do Setor 4.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbLetraSetor4.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Letra do Setor 4.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (cmbVozSetor4.SelectedIndex == -1)
                {
                    MessageBox.Show("Informe a Voz do Setor 4.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }


            gridBotoes.IsEnabled = false;
            gridBotoes1.IsEnabled = false;
            ckbModoRetiradaManual.IsEnabled = false;
            ckbVozPadrao.IsEnabled = false;
            btnConfigurarBotoes.IsEnabled = true;
            btnSalvarBotoes.IsEnabled = false;
            DesbilitarTabItems("LiberarSalvarTudo");
        }

        private void btnConfigurarBotoes_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Configurar_Botoes == true)
            {
                ckbModoRetiradaManual.IsEnabled = true;
                ckbVozPadrao.IsEnabled = true;
                if (ckbModoRetiradaManual.IsChecked.Value == true)
                {
                    gridBotoes.IsEnabled = false;
                    gridBotoes1.IsEnabled = false;
                }
                else
                {
                    gridBotoes.IsEnabled = true;
                    gridBotoes1.IsEnabled = true;
                }

                btnConfigurarBotoes.IsEnabled = false;
                btnSalvarBotoes.IsEnabled = true;
                DesbilitarTabItems(tabItemRetiradaSenhas.Header.ToString());
                VerificarFilhosHabilitarBotoes();
                VerificarCheckBoxHabilitarBotoes();
            }
            else
                MessageBox.Show("Usuário logado não tem permissão para configurar Botões de Retirada de Senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void ckbHabilitarBotao1_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }

        private void ckbHabilitarBotao2_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }

        private void ckbHabilitarBotao3_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }

        private void ckbHabilitarSetor1_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }


        private void ckbHabilitaSetor2_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }


        private void ckbHabilitarSetor3_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }

        private void ckbHabilitarSetor4_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VerificarFilhosHabilitarBotoes();
        }


        private void ckbMensagemExpediente_Checked(object sender, RoutedEventArgs e)
        {
            gridExpediente.IsEnabled = true;
        }

        private void ckbMensagemExpediente_Unchecked(object sender, RoutedEventArgs e)
        {
            gridExpediente.IsEnabled = false;
        }

        private void txtHoraInicioExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMinutoInicioExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtSegundoInicioExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMensagemInicioExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtHoraFimExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMinutoFimExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMensagemFimExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSegundoFimExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtHoraDesligarPainel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMinutoDesligarPainel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtSegundoDesligarPainel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtHoraDesligarSenha_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMinutoDesligarSenha_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtSegundoDesligarSenha_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtHoraDesligarEstacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtMinutoDesligarEstacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtSegundoDesligarEstacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumeros(sender, e);
        }

        private void txtHoraInicioExpediente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraInicioExpediente.Text == "")
                txtHoraInicioExpediente.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtHoraInicioExpediente.Text);

                if (valor > 23)
                    valor = 23;

                txtHoraInicioExpediente.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtHoraInicioExpediente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraInicioExpediente.Text == "00")
                txtHoraInicioExpediente.Text = "";

            txtHoraInicioExpediente.SelectAll();
        }

        private void txtMinutoInicioExpediente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoInicioExpediente.Text == "")
                txtMinutoInicioExpediente.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtMinutoInicioExpediente.Text);

                if (valor > 59)
                    valor = 29;

                txtMinutoInicioExpediente.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtMinutoInicioExpediente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoInicioExpediente.Text == "00")
                txtMinutoInicioExpediente.Text = "";

            txtMinutoInicioExpediente.SelectAll();
        }

        private void txtSegundoInicioExpediente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoInicioExpediente.Text == "")
                txtSegundoInicioExpediente.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtSegundoInicioExpediente.Text);

                if (valor > 59)
                    valor = 59;

                txtSegundoInicioExpediente.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtSegundoInicioExpediente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoInicioExpediente.Text == "00")
                txtSegundoInicioExpediente.Text = "";

            txtSegundoInicioExpediente.SelectAll();
        }

        private void txtHoraFimExpediente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraFimExpediente.Text == "")
                txtHoraFimExpediente.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtHoraFimExpediente.Text);

                if (valor > 23)
                    valor = 23;

                txtHoraFimExpediente.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtHoraFimExpediente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraFimExpediente.Text == "00")
                txtHoraFimExpediente.Text = "";

            txtHoraFimExpediente.SelectAll();
        }

        private void txtMinutoFimExpediente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoFimExpediente.Text == "")
                txtMinutoFimExpediente.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtMinutoFimExpediente.Text);

                if (valor > 59)
                    valor = 59;

                txtMinutoFimExpediente.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtMinutoFimExpediente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoFimExpediente.Text == "00")
                txtMinutoFimExpediente.Text = "";

            txtMinutoFimExpediente.SelectAll();
        }

        private void txtSegundoFimExpediente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoFimExpediente.Text == "")
                txtSegundoFimExpediente.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtSegundoFimExpediente.Text);

                if (valor > 59)
                    valor = 59;

                txtSegundoFimExpediente.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtSegundoFimExpediente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoFimExpediente.Text == "00")
                txtSegundoFimExpediente.Text = "";

            txtSegundoFimExpediente.SelectAll();
        }

        private void txtHoraDesligarPainel_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtHoraDesligarPainel.Text == "")
                txtHoraDesligarPainel.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtHoraDesligarPainel.Text);

                if (valor > 23)
                    valor = 23;

                txtHoraDesligarPainel.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtHoraDesligarPainel_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraDesligarPainel.Text == "00")
                txtHoraDesligarPainel.Text = "";

            txtHoraDesligarPainel.SelectAll();
        }

        private void txtMinutoDesligarPainel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoDesligarPainel.Text == "")
                txtMinutoDesligarPainel.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtMinutoDesligarPainel.Text);

                if (valor > 59)
                    valor = 59;

                txtMinutoDesligarPainel.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtMinutoDesligarPainel_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoDesligarPainel.Text == "00")
                txtMinutoDesligarPainel.Text = "";

            txtMinutoDesligarPainel.SelectAll();
        }

        private void txtSegundoDesligarPainel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoDesligarPainel.Text == "")
                txtSegundoDesligarPainel.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtSegundoDesligarPainel.Text);

                if (valor > 59)
                    valor = 59;

                txtSegundoDesligarPainel.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtSegundoDesligarPainel_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoDesligarPainel.Text == "00")
                txtSegundoDesligarPainel.Text = "";

            txtSegundoDesligarPainel.SelectAll();
        }

        private void txtHoraDesligarSenha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraDesligarSenha.Text == "")
                txtHoraDesligarSenha.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtHoraDesligarSenha.Text);

                if (valor > 23)
                    valor = 23;

                txtHoraDesligarSenha.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtHoraDesligarSenha_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraDesligarSenha.Text == "00")
                txtHoraDesligarSenha.Text = "";

            txtHoraDesligarSenha.SelectAll();
        }

        private void txtMinutoDesligarSenha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoDesligarSenha.Text == "")
                txtMinutoDesligarSenha.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtMinutoDesligarSenha.Text);

                if (valor > 59)
                    valor = 59;

                txtMinutoDesligarSenha.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtMinutoDesligarSenha_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoDesligarSenha.Text == "00")
                txtMinutoDesligarSenha.Text = "";

            txtMinutoDesligarSenha.SelectAll();
        }

        private void txtSegundoDesligarSenha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoDesligarSenha.Text == "")
                txtSegundoDesligarSenha.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtSegundoDesligarSenha.Text);

                if (valor > 59)
                    valor = 59;

                txtSegundoDesligarSenha.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtSegundoDesligarSenha_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoDesligarSenha.Text == "00")
                txtSegundoDesligarSenha.Text = "";

            txtSegundoDesligarSenha.SelectAll();
        }

        private void txtHoraDesligarEstacao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraDesligarEstacao.Text == "")
                txtHoraDesligarEstacao.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtHoraDesligarEstacao.Text);

                if (valor > 23)
                    valor = 23;

                txtHoraDesligarEstacao.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtHoraDesligarEstacao_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtHoraDesligarEstacao.Text == "00")
                txtHoraDesligarEstacao.Text = "";

            txtHoraDesligarEstacao.SelectAll();
        }

        private void txtMinutoDesligarEstacao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoDesligarEstacao.Text == "")
                txtMinutoDesligarEstacao.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtMinutoDesligarEstacao.Text);

                if (valor > 59)
                    valor = 59;

                txtMinutoDesligarEstacao.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtMinutoDesligarEstacao_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinutoDesligarEstacao.Text == "00")
                txtMinutoDesligarEstacao.Text = "";

            txtMinutoDesligarEstacao.SelectAll();
        }

        private void txtSegundoDesligarEstacao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoDesligarEstacao.Text == "")
                txtSegundoDesligarEstacao.Text = "00";
            else
            {
                var valor = Convert.ToInt16(txtSegundoDesligarEstacao.Text);

                if (valor > 59)
                    valor = 59;

                txtSegundoDesligarEstacao.Text = string.Format("{0:00}", valor);
            }
        }

        private void txtSegundoDesligarEstacao_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSegundoDesligarEstacao.Text == "00")
                txtSegundoDesligarEstacao.Text = "";

            txtSegundoDesligarEstacao.SelectAll();
        }

        private void ckbDesligarPainel_Checked(object sender, RoutedEventArgs e)
        {
            gridDesligarPainel.IsEnabled = true;
        }

        private void ckbDesligarPainel_Unchecked(object sender, RoutedEventArgs e)
        {
            gridDesligarPainel.IsEnabled = false;
        }

        private void ckDesligarSenha_Checked(object sender, RoutedEventArgs e)
        {
            gridDesligarSenha.IsEnabled = true;
        }

        private void ckDesligarSenha_Unchecked(object sender, RoutedEventArgs e)
        {
            gridDesligarSenha.IsEnabled = false;
        }

        private void ckbDesligarEstacao_Checked(object sender, RoutedEventArgs e)
        {
            gridDesligarEstacao.IsEnabled = true;
        }

        private void ckbDesligarEstacao_Unchecked(object sender, RoutedEventArgs e)
        {
            gridDesligarEstacao.IsEnabled = false;
        }

        private void cmbTipoChamadaSenha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbTipoChamadaSenha_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbModoRetiradaManual_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbModoRetiradaManual.Focus())
            {
                gridBotoes.IsEnabled = false;
                gridBotoes1.IsEnabled = false;
                cmbTipoSenha.SelectedIndex = 0;
                cmbTipoSenha.IsEnabled = false;
                ckbZerarSenhaDiaSeguinte.IsChecked = false;
                ckbZerarSenhaDiaSeguinte.IsEnabled = false;
                ckbUtilizarAleatorio.IsChecked = false;
                ckbUtilizarAleatorio.IsEnabled = false;
                cmbTipoSenha.SelectedIndex = 0;
                cmbTipoSenha.IsEnabled = false;

                VerificarFilhosHabilitarBotoes();
                VerificarCheckBoxHabilitarBotoes();
            }
        }

        private void ckbModoRetiradaManual_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ckbModoRetiradaManual.Focus())
            {
                gridBotoes.IsEnabled = true;
                gridBotoes1.IsEnabled = true;
                cmbTipoSenha.IsEnabled = true;
                ckbZerarSenhaDiaSeguinte.IsEnabled = true;
                ckbUtilizarAleatorio.IsEnabled = true;
                cmbTipoSenha.IsEnabled = true;
                VerificarFilhosHabilitarBotoes();
                VerificarCheckBoxHabilitarBotoes();
            }
        }


        private void txtLicenca_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLicenca.Text.Length == 24)
            {
                ValidarLicenca();
            }
        }

        private void ValidarLicenca()
        {
            try
            {
                string licenca = ClassCriptografia.Decrypt(txtLicenca.Text);

                string codigoInstalacao = licenca.Substring(0, 7);

                string versao = licenca.Substring(7, licenca.Length - 7);

                string versaoOriginal = string.Format("{0}.{1}.{2}.{3}.{4}", versao.Substring(0, 1), versao.Substring(1, 1), versao.Substring(2, 2), versao.Substring(4, 2), versao.Substring(6, 2));

                if (controles.Where(p => p.CodigoAtivacao == txtLicenca.Text).FirstOrDefault() != null)
                {
                    MessageBox.Show("Licença inserida já foi utilizada.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtLicenca.Text = "";
                }
                else
                {
                    if (codigoInstalacao == ClassCriptografia.Decrypt(parametros.CodigoInstalacao) || codigoInstalacao == ClassCriptografia.Decrypt(novoCodigoIntalacao))
                    {

                        if (versao.Substring(0, 1) == "3" || versao.Substring(0, 1) == "2" || versao.Substring(0, 1) == "1")
                        {

                            if (versao.Substring(0, 1) == "3")
                            {

                                int qdt = Convert.ToInt16(versao.Substring(1, 1));

                                string mesInicio = versao.Substring(4, 2);

                                string ano = versao.Substring(2, 2);

                                string dia = versao.Substring(6, 2);

                                DateTime dataInicio = Convert.ToDateTime(DataPorMes(dia, mesInicio, ano));

                                DateTime dataFim = dataInicio.AddMonths(qdt);


                                if (controle != null)
                                {
                                    DateTime dataFimAtual = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataValidadeFim));

                                    if (dataInicio <= dataFimAtual)
                                    {
                                        MessageBox.Show("Data Início " + dataInicio.ToShortDateString() + " deve ser maior que a Data Fim " + dataFimAtual.ToShortDateString() + " da licença atual. Entre em contato com a CS Systems.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                        txtLicenca.Text = "";
                                        return;
                                    }
                                }


                                if (controle != null)
                                    _AppServicoControle_Uso.SalvarControle("N", "F", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, versaoOriginal);
                                else
                                    _AppServicoControle_Uso.SalvarControle("N", "V", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, versaoOriginal);

                                CriarXML();

                                controles = new List<Controle_Uso>();
                                controles = _AppServicoControle_Uso.ObterTodos().ToList();

                                controle = controles.Where(p => p.AtivacaoUso == ClassCriptografia.Encrypt("V")).FirstOrDefault();


                                AguardeSalvarParametros salvar = new AguardeSalvarParametros(listaCadastroPainel, listaCadastro_Pc, listaMensagens, parametros, listaPc_Painel, usuarios);
                                salvar.Owner = this;
                                salvar.ShowDialog();



                                this.Close();
                                _inicio = new Apresentacao();
                                _inicio.ShowDialog();

                            }

                            if (versao.Substring(0, 1) == "2")
                            {
                                int qdt = Convert.ToInt16(versao.Substring(1, 1));

                                string mesInicio = versao.Substring(4, 2);

                                string ano = versao.Substring(2, 2);

                                string dia = versao.Substring(6, 2);

                                DateTime dataInicio = Convert.ToDateTime(DataPorMes(dia, mesInicio, ano));

                                DateTime dataFim = dataInicio.AddYears(qdt);


                                if (controle != null)
                                {
                                    DateTime dataFimAtual = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataValidadeFim));

                                    if (dataInicio <= dataFimAtual)
                                    {
                                        MessageBox.Show("Data Início " + dataInicio.ToShortDateString() + " deve ser maior que a Data Fim " + dataFimAtual.ToShortDateString() + " da licença atual. Entre em contato com a CS Systems.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                        txtLicenca.Text = "";
                                        return;
                                    }
                                }

                                if (controle != null)
                                    _AppServicoControle_Uso.SalvarControle("N", "F", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, versaoOriginal);
                                else
                                    _AppServicoControle_Uso.SalvarControle("N", "V", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, versaoOriginal);

                                CriarXML();

                                controles = new List<Controle_Uso>();
                                controles = _AppServicoControle_Uso.ObterTodos().ToList();

                                controle = controles.Where(p => p.AtivacaoUso == ClassCriptografia.Encrypt("V")).FirstOrDefault();


                                AguardeSalvarParametros salvar = new AguardeSalvarParametros(listaCadastroPainel, listaCadastro_Pc, listaMensagens, parametros, listaPc_Painel, usuarios);
                                salvar.Owner = this;
                                salvar.ShowDialog();



                                this.Close();
                                _inicio = new Apresentacao();
                                _inicio.ShowDialog();
                            }

                            if (versao.Substring(0, 1) == "1")
                            {
                                int qdt = Convert.ToInt16(versao.Substring(1, 1));

                                string mesInicio = versao.Substring(4, 2);

                                string ano = versao.Substring(2, 2);

                                string dia = versao.Substring(6, 2);

                                DateTime dataInicio = Convert.ToDateTime(DataPorMes(dia, mesInicio, ano));

                                DateTime dataFim = dataInicio.AddMonths(qdt);


                                if (controle != null)
                                {
                                    DateTime dataFimAtual = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataValidadeFim));

                                    if (dataInicio <= dataFimAtual)
                                    {
                                        MessageBox.Show("Data Início " + dataInicio.ToShortDateString() + " deve ser maior que a Data Fim " + dataFimAtual.ToShortDateString() + " da licença atual. Entre em contato com a CS Systems.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                        txtLicenca.Text = "";
                                        return;
                                    }
                                }

                                if (controle != null)
                                    _AppServicoControle_Uso.SalvarControle("N", "F", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, versaoOriginal);
                                else
                                    _AppServicoControle_Uso.SalvarControle("N", "V", dataInicio.ToShortDateString(), dataFim.ToShortDateString(), DateTime.Now.Date.ToShortDateString(), txtLicenca.Text, versaoOriginal);

                                CriarXML();


                                controles = new List<Controle_Uso>();
                                controles = _AppServicoControle_Uso.ObterTodos().ToList();

                                controle = controles.Where(p => p.AtivacaoUso == ClassCriptografia.Encrypt("V")).FirstOrDefault();


                                AguardeSalvarParametros salvar = new AguardeSalvarParametros(listaCadastroPainel, listaCadastro_Pc, listaMensagens, parametros, listaPc_Painel, usuarios);
                                salvar.Owner = this;
                                salvar.ShowDialog();



                                this.Close();
                                _inicio = new Apresentacao();
                                _inicio.ShowDialog();
                            }



                        }
                        else
                        {
                            MessageBox.Show("Licença inválida. Entre em contato com a CS Systems.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);

                            txtLicenca.Focus();

                            txtLicenca.SelectAll();
                        }



                    }
                    else
                    {
                        MessageBox.Show("Licença inválida. Entre em contato com a CS Systems.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);

                        txtLicenca.Focus();

                        txtLicenca.SelectAll();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Licença inválida. Entre em contato com a CS Systems.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

        }


        private void CriarXML()
        {
            try
            {

                string curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                XmlTextWriter writer = new XmlTextWriter(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\SysConf.xml", null);


                //inicia o documento xml
                writer.WriteStartDocument();
                //escreve o elmento raiz
                writer.WriteStartElement("Config");
                //Escreve os sub-elementos
                writer.WriteElementString("Parametro1", ClassCriptografia.Encrypt("parametro1"));
                writer.WriteElementString("Parametro2", ClassCriptografia.Encrypt("parametro2"));
                writer.WriteElementString("Parametro3", ClassCriptografia.Encrypt("parametro3"));
                writer.WriteElementString("Parametro4", ClassCriptografia.Encrypt("parametro4"));
                writer.WriteElementString("Parametro5", ClassCriptografia.Encrypt("parametro5"));
                writer.WriteElementString("Parametro6", parametros.CodigoInstalacao);
                // encerra o elemento raiz
                writer.WriteEndElement();
                //Escreve o XML para o arquivo e fecha o objeto escritor
                writer.Close();



            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível fazer algumas verificações, favor entrar em contato com o suporte.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

        }




        private string DataPorMes(string dia, string mes, string ano)
        {
            string data = string.Empty;

            switch (mes)
            {
                case "01":
                    data = dia + "/01/20" + ano;
                    break;

                case "02":
                    data = dia + "/02/20" + ano;
                    break;

                case "03":
                    data = dia + "/03/20" + ano;
                    break;

                case "04":
                    data = dia + "/04/20" + ano;
                    break;

                case "05":
                    data = dia + "/05/20" + ano;
                    break;

                case "06":
                    data = dia + "/06/20" + ano;
                    break;

                case "07":
                    data = dia + "/07/20" + ano;
                    break;

                case "08":
                    data = dia + "/08/20" + ano;
                    break;

                case "09":
                    data = dia + "/09/20" + ano;
                    break;

                case "10":
                    data = dia + "/10/20" + ano;
                    break;

                case "11":
                    data = dia + "/11/20" + ano;
                    break;

                case "12":
                    data = dia + "/12/20" + ano;
                    break;


                default:
                    break;
            }


            return data;
        }

        private void gridExpediente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void ckbCadastroCliente_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ckbCadastroCliente_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSetor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                cmbSetor.SelectedIndex = -1;
        }

        private void cmbBeeps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FileInfo arquivoSelecionado = new FileInfo(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\Beeps\" + cmbBeeps.SelectedItem);

                DirectoryInfo diretorio = new DirectoryInfo(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\Resources");

                if (diretorio.Exists)
                {
                    foreach (FileInfo item in diretorio.GetFiles())
                    {
                        item.Delete();
                    }
                }

                string destino = string.Format(@"{0}\{1}", diretorio, cmbBeeps.SelectedItem);

                arquivoSelecionado.CopyTo(destino, true);


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ckbBip_Aviso_Checked(object sender, RoutedEventArgs e)
        {
            cmbBeeps.IsEnabled = true;
            imgSom.IsEnabled = true;
            imgSom.Visibility = Visibility.Visible;
        }

        private void ckbBip_Aviso_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbBeeps.IsEnabled = false;
            imgSom.IsEnabled = false;
            imgSom.Visibility = Visibility.Hidden;
        }

        private void imgSom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string path = @"\\SERVIDOR\CS_Sistemas\CS_Caixa\Resources\" + cmbBeeps.SelectedItem;
                        
            using (SoundPlayer sound = new SoundPlayer(path))
            {
                sound.Play();
            }
        }

        private void ckbVozPadrao_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ckbVozPadrao_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ckbVozPadrao_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }


    }
}
