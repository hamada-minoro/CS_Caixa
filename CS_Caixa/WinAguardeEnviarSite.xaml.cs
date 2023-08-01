using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Objetos_de_Valor;
using FirebirdSql.Data.FirebirdClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinAguardeEnviarSite.xaml
    /// </summary>
    public partial class WinAguardeEnviarSite : Window
    {
        BackgroundWorker worker;
        DateTime _data;
        string carregaAtual;
        int qtdFirmas = 0;
        int qtdRegistro = 0;
        int qtdProtesto = 0;
        int qtdUsuariosSite = 0;
        bool _somenteProtesto = false;
        string _documento = "todos";

        ClassEnviarEmail enviarEmail = new ClassEnviarEmail();

        List<IntimacaoProtesto> intimacoes = new List<IntimacaoProtesto>();

        public WinAguardeEnviarSite(string documento)
        {
            _documento = documento;
            InitializeComponent();
        }

        public WinAguardeEnviarSite(DateTime data)
        {
            _data = data;
            InitializeComponent();
        }

        public WinAguardeEnviarSite(DateTime data, bool somenteProtesto)
        {
            _data = data;
            _somenteProtesto = somenteProtesto;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Processo();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

            if (carregaAtual == "Enviando Firmas")
            {
                progressBar1.Maximum = qtdFirmas;
                label2.Content = string.Format("Enviando Firmas. {0} de {1}", +progressBar1.Value, qtdFirmas);
            }

            if (carregaAtual == "Enviando Registro")
            {
                progressBar1.Maximum = qtdRegistro;
                label2.Content = string.Format("Enviando Registro. {0} de {1}", +progressBar1.Value, qtdRegistro);
            }

            if (carregaAtual == "Enviando Protesto")
            {
                progressBar1.Maximum = qtdProtesto;
                label2.Content = string.Format("Enviando Protesto. {0} de {1}", +progressBar1.Value, qtdProtesto);
            }


            if (carregaAtual == "Sincronizando Usuários do Site")
            {
                progressBar1.Maximum = qtdUsuariosSite;
                label2.Content = string.Format("Sincronizando Usuários do Site. {0} de {1}", +progressBar1.Value, qtdUsuariosSite);
            }

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void Processo()
        {
            if (_somenteProtesto == false)
            {
                if (_documento == "todos")
                {
                    EnviarFirmas();
                    EnviarRegistro();
                    EnviarProtesto();
                    SincronizarSite();

                    ClassAtualizaSite atualiza = new ClassAtualizaSite();
                    var atu = atualiza.ObterAtualizaSite();

                    atu.DataAtualizacao = _data.ToShortDateString();
                    atu.HoraAtualizacao = DateTime.Now.ToLongTimeString();
                    atu.Status = "ATUALIZADO";
                    atu.PcAtualizacao = Environment.MachineName;

                    atualiza.SalvarAtualizar(atu);
                }
                else
                    SincronizarSite();
            }
            else
                EnviarProtesto();
        }


        private void EnviarFirmas()
        {

            try
            {
                DateTime dataInicio = _data.AddDays(-7);

                FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingBalcaoSite);
                conTotal.Open();

                string dataIni = dataInicio.ToShortDateString().Replace("/", ".");

                string dataFim = _data.ToShortDateString().Replace("/", ".");

                FbCommand cmdTotal = new FbCommand("SELECT nome, cpf FROM fichas WHERE data between '" + dataIni + "' AND '" + dataFim + "'", conTotal);



                cmdTotal.CommandType = CommandType.Text;

                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                DataTable dtTotal = new DataTable();
                dtTotal.Load(drTotal);

                qtdFirmas = dtTotal.Rows.Count;

                ConsultaFirma firmaEnviar = new ConsultaFirma();

                carregaAtual = "Enviando Firmas";


                for (int i = 0; i < dtTotal.Rows.Count; i++)
                {
                    Thread.Sleep(1);

                    firmaEnviar.Nome = dtTotal.Rows[i][0].ToString();
                    firmaEnviar.Cpf = dtTotal.Rows[i][1].ToString();
                    worker.ReportProgress(i + 1);

                    if (VerificarEnviarFirmas(firmaEnviar) == "")
                        ConectarEnviarFirmas(firmaEnviar);


                }


            }
            catch (Exception e) { throw e; }


        }


        private void ConectarEnviarFirmas(ConsultaFirma enviar)
        {
            try
            {
                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO ConsultaFirma(Nome, Cpf) VALUES ('" + enviar.Nome + "','" + enviar.Cpf + "')", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception)
            {

            }
        }

        private string VerificarEnviarFirmas(ConsultaFirma enviar)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT Cpf FROM ConsultaFirma WHERE Cpf = '" + enviar.Cpf + "'", con);

                cmd.CommandType = CommandType.Text;

                result = cmd.ExecuteScalar().ToString();

                con.Close();

                return result;

            }
            catch (Exception)
            {
                return string.Empty;
            }

        }





        private void EnviarRegistro()
        {

            try
            {
                DateTime dataInicio = _data.AddDays(-180);

                FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingRgi);
                conTotal.Open();

                string dataIni = dataInicio.ToShortDateString().Replace("/", ".");

                string dataFim = _data.ToShortDateString().Replace("/", ".");

                FbCommand cmdTotal = new FbCommand("select * from ato where protocolo > 0 and data_protocolo between '" + dataIni + "' AND '" + dataFim + "'", conTotal);

                cmdTotal.CommandType = CommandType.Text;

                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                DataTable dtTotal = new DataTable();
                dtTotal.Load(drTotal);

                qtdRegistro = dtTotal.Rows.Count;

                ConsultaRegistro registroEnviar = new ConsultaRegistro();

                carregaAtual = "Enviando Registro";


                for (int i = 0; i < dtTotal.Rows.Count; i++)
                {
                    Thread.Sleep(1);

                    registroEnviar.IdAto = dtTotal.Rows[i]["id_ato"].ToString();
                    registroEnviar.Protocolo = dtTotal.Rows[i]["protocolo"].ToString();
                    registroEnviar.DataRegistro = dtTotal.Rows[i]["data_registro"].ToString();
                    registroEnviar.DataEntrada = dtTotal.Rows[i]["data_protocolo"].ToString();
                    registroEnviar.Matricula = dtTotal.Rows[i]["matricula"].ToString();
                    registroEnviar.Natureza = dtTotal.Rows[i]["desc_natureza"].ToString();
                    registroEnviar.Exigencia = dtTotal.Rows[i]["obs_exi"].ToString();
                    registroEnviar.Situacao = dtTotal.Rows[i]["status"].ToString();


                    switch (registroEnviar.Situacao)
                    {
                        case "E":
                            registroEnviar.Situacao = "ENTRADA";
                            break;

                        case "B":
                            registroEnviar.Situacao = "BUSCAS";
                            break;

                        case "N":
                            registroEnviar.Situacao = "ANÁLISE";
                            break;

                        case "X":
                            registroEnviar.Situacao = "EXIGÊNCIAS";
                            break;

                        case "T":
                            registroEnviar.Situacao = "REENTRADA";
                            break;

                        case "C":
                            registroEnviar.Situacao = "CANCELADO";
                            break;

                        case "R":
                            registroEnviar.Situacao = "REGISTRANDO";
                            break;

                        case "D":
                            registroEnviar.Situacao = "CONCLUÍDO";
                            break;

                        case "A":
                            registroEnviar.Situacao = "ARQUIVADO";
                            break;

                        case "U":
                            registroEnviar.Situacao = "ENTREGUE";
                            break;

                        default:
                            registroEnviar.Situacao = "*";
                            break;

                    }



                    if (registroEnviar.DataEntrada.Length >= 10)
                        registroEnviar.DataEntrada = registroEnviar.DataEntrada.Substring(0, 10);

                    if (registroEnviar.DataRegistro.Length >= 10)
                        registroEnviar.DataRegistro = registroEnviar.DataRegistro.Substring(0, 10);


                    worker.ReportProgress(i + 1);

                    if (ConsultaEnviarRegistro(registroEnviar))
                        UpdateEnviarRegistro(registroEnviar);
                    else
                        ConectarEnviarRegistro(registroEnviar);
                }


            }
            catch (Exception) { }


        }

        private void ConectarEnviarRegistro(ConsultaRegistro enviar)
        {
            try
            {
                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO ConsultaRegistro(Protocolo, DataRegistro, DataEntrada, Matricula, Natureza, Exigencia, Situacao, IdAto) VALUES ('" + enviar.Protocolo + "','" + enviar.DataRegistro + "','" + enviar.DataEntrada + "','" + enviar.Matricula + "','" + enviar.Natureza + "','" + enviar.Exigencia + "','" + enviar.Situacao + "','" + enviar.IdAto + "'" + ")", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();


            }
            catch (Exception)
            {

            }
        }

        private void UpdateEnviarRegistro(ConsultaRegistro enviar)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                {

                    MySqlCommand cmd = new MySqlCommand("update ConsultaRegistro set Protocolo = @Protocolo, DataRegistro = @DataRegistro, DataEntrada = @DataEntrada, Matricula = @Matricula, Natureza = @Natureza, Exigencia =@Exigencia, Situacao = @Situacao WHERE IdAto = '" + enviar.IdAto + "'", con);

                    cmd.Parameters.Add(new MySqlParameter("@Protocolo", enviar.Protocolo));
                    cmd.Parameters.Add(new MySqlParameter("@DataRegistro", enviar.DataRegistro));
                    cmd.Parameters.Add(new MySqlParameter("@DataEntrada", enviar.DataEntrada));
                    cmd.Parameters.Add(new MySqlParameter("@Matricula", enviar.Matricula));
                    cmd.Parameters.Add(new MySqlParameter("@Natureza", enviar.Natureza));
                    cmd.Parameters.Add(new MySqlParameter("@Exigencia", enviar.Exigencia));
                    cmd.Parameters.Add(new MySqlParameter("@Situacao", enviar.Situacao));
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {
            }

        }


        private bool ConsultaEnviarRegistro(ConsultaRegistro enviar)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Select IdAto FROM ConsultaRegistro WHERE IdAto = '" + enviar.IdAto + "'", con);

                cmd.CommandType = CommandType.Text;

                result = cmd.ExecuteScalar().ToString();

                con.Close();

                if (result == enviar.IdAto)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

        }



        private void EnviarProtesto()
        {
            try
            {
                DateTime dataInicio = _data.AddDays(-7);

                FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);

                conTotal.Open();

                string dataIni = dataInicio.ToShortDateString().Replace("/", ".");

                string dataFim = _data.ToShortDateString().Replace("/", ".");

                FbCommand cmdTotal = new FbCommand("select * from movimento where data between '" + dataIni + "' AND '" + dataFim + "'", conTotal);
                //FbCommand cmdTotal = new FbCommand("select id_ato from titulos where STATUS = 'PROTESTO' and dt_registro between '" + dataIni + "' AND '" + dataFim + "'", conTotal);
                //FbCommand cmdTotal = new FbCommand("select * from titulos where status like '%INTIMADO%' or status = 'APONTADO' AND (protocolo > 110000)", conTotal);

                cmdTotal.CommandType = CommandType.Text;

                FbDataReader drTotal;


                drTotal = cmdTotal.ExecuteReader();


                DataTable dtTotal = new DataTable();
                dtTotal.Load(drTotal);

                qtdProtesto = dtTotal.Rows.Count;

                ConsultaProtesto protestoEnviar = new ConsultaProtesto();
                ConsultaApontado apontadoEnviar = new ConsultaApontado();


                carregaAtual = "Enviando Protesto";


                for (int i = 0; i < dtTotal.Rows.Count; i++)
                {

                    FbCommand cmd = new FbCommand("select * from titulos where id_ato = " + dtTotal.Rows[i]["id_ato"].ToString(), conTotal);
                    cmd.CommandType = CommandType.Text;

                    FbDataReader dr;
                    dr = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    Thread.Sleep(1);
                    worker.ReportProgress(i + 1);

                    if (dt.Rows.Count > 0)
                    {
                        protestoEnviar.IdAto = dt.Rows[0]["id_ato"].ToString();
                        protestoEnviar.Devedor = dt.Rows[0]["devedor"].ToString();
                        protestoEnviar.DocumentoDevedor = dt.Rows[0]["cpf_cnpj_devedor"].ToString();


                        if (dt.Rows[0]["dt_registro"].ToString() != null && dt.Rows[0]["dt_registro"].ToString() != "")
                            protestoEnviar.DataProtesto = Convert.ToDateTime(dt.Rows[0]["dt_registro"]);

                        DeletarEnviarProtesto(protestoEnviar);

                        if (dt.Rows[0]["status"].ToString() == "PROTESTADO")
                            ConectarEnviarProtesto(protestoEnviar);

                        apontadoEnviar.IdAto = dt.Rows[0]["id_ato"].ToString();
                        apontadoEnviar.Devedor = dt.Rows[0]["devedor"].ToString();
                        apontadoEnviar.DocumentoDevedor = dt.Rows[0]["cpf_cnpj_devedor"].ToString();
                        apontadoEnviar.Protocolo = dt.Rows[0]["protocolo"].ToString();

                        if (dt.Rows[0]["dt_protocolo"].ToString() != null && dt.Rows[0]["dt_protocolo"].ToString() != "")
                            apontadoEnviar.DataProtocolo = Convert.ToDateTime(dt.Rows[0]["dt_protocolo"]);

                        DeletarEnviarApontado(apontadoEnviar);
                        if (dt.Rows[0]["status"].ToString() == "APONTADO" || dt.Rows[0]["status"].ToString().Contains("INTIMADO"))
                            ConectarEnviarApontado(apontadoEnviar);


                    }
                }

                conTotal.Close();
                ObterTitulosCaducados();
            }
            catch (Exception e) { throw e; }
        }


        private void IntimacaoProtesto(int idAto)
        {

            ClassTabelaCustas custas = new ClassTabelaCustas();

            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
            conTotal.Open();
            FbCommand cmd = new FbCommand("select * from titulos where Id_Ato = " + idAto, conTotal);
            //FbCommand cmdTotal = new FbCommand("select * from titulos where status like '%INTIMADO%' or status = 'APONTADO' AND (protocolo > 110000)", conTotal);

            cmd.CommandType = CommandType.Text;
            FbDataReader dr;

            dr = cmd.ExecuteReader();

            IntimacaoProtesto intimacao = new IntimacaoProtesto();


            while (dr.Read())
            {
                //DeletarEnviarIntimacao(Convert.ToInt32(dr["id_ato"]));

                intimacao = new IntimacaoProtesto();
                intimacao.IdAto = Convert.ToInt32(dr["id_ato"]);

                intimacao.Atribuicao = "PROTESTO";
                intimacao.Documento = dr["CPF_CNPJ_DEVEDOR"].ToString();
                intimacao.DataAto = Convert.ToDateTime(dr["DT_PROTOCOLO"]);
                intimacao.Tipo = "INTIMAÇÃO";
                intimacao.Descricao = "INTIMAÇÃO DE PROTESTO";
                intimacao.Status = dr["STATUS"].ToString();
                intimacao.Selo = dr["CCT"].ToString();
                intimacao.TipoCobranca = dr["COBRANCA"].ToString();

                intimacao.DataEntrada = Convert.ToDateTime(dr["DT_ENTRADA"]);
                intimacao.Protocolo = Convert.ToInt32(dr["PROTOCOLO"]);
                intimacao.DataPrazo = Convert.ToDateTime(dr["DT_PRAZO"]);
                intimacao.Devedor = dr["DEVEDOR"].ToString();
                intimacao.CpfCnpjDevedor = dr["CPF_CNPJ_DEVEDOR"].ToString();

                FbCommand cmdDevedor = new FbCommand("select * from DEVEDORES where id_ato = " + intimacao.IdAto, conTotal);
                cmdDevedor.CommandType = CommandType.Text;

                FbDataReader drDevedor;
                drDevedor = cmdDevedor.ExecuteReader();

                DataTable dtDevedor = new DataTable();
                dtDevedor.Load(drDevedor);

                intimacao.Endereco = dtDevedor.Rows[0]["ENDERECO"].ToString();
                intimacao.Bairro = dtDevedor.Rows[0]["BAIRRO"].ToString();
                intimacao.Municipio = dtDevedor.Rows[0]["MUNICIPIO"].ToString();
                intimacao.UF = dtDevedor.Rows[0]["UF"].ToString();
                intimacao.CEP = dtDevedor.Rows[0]["CEP"].ToString();
                intimacao.TipoPessoa = dtDevedor.Rows[0]["TIPO"].ToString();

                FbCommand cmdTipoTitulo = new FbCommand("select * from TIPOS where CODIGO = " + dr["TIPO_TITULO"].ToString(), conTotal);
                cmdTipoTitulo.CommandType = CommandType.Text;

                FbDataReader drTipoTitulo;
                drTipoTitulo = cmdTipoTitulo.ExecuteReader();

                DataTable dtTipoTitulo = new DataTable();
                dtTipoTitulo.Load(drTipoTitulo);

                intimacao.TipoTitulo = dtTipoTitulo.Rows[0]["DESCRICAO"].ToString();
                intimacao.NumeroTitulo = dr["NUMERO_TITULO"].ToString();
                intimacao.Portador = dr["APRESENTANTE"].ToString();
                intimacao.Cedente = dr["CEDENTE"].ToString();
                intimacao.Sacador = dr["SACADOR"].ToString();
                intimacao.Praca = dr["PRACA_PROTESTO"].ToString();
                intimacao.FinsFalimentares = "NÃO";

                if (dr["FINS_FALIMENTARES"].ToString() == "S")
                    intimacao.FinsFalimentares = "SIM";

                intimacao.DataTitulo = Convert.ToDateTime(dr["DT_TITULO"]);
                intimacao.DataVencimento = Convert.ToDateTime(dr["DT_VENCIMENTO"]);
                intimacao.DataIntimacao = DateTime.Now.Date;


                intimacao.ValorTitulo = Convert.ToDecimal(dr["SALDO_TITULO"]);
                intimacao.Custas = Convert.ToDecimal(dr["TOTAL"]);
                intimacao.Distribuicao = Convert.ToDecimal(dr["DISTRIBUICAO"]);
                intimacao.ValorChequeAdm = Convert.ToDecimal(dr["SALDO_PROTESTO"]);

                var custasProtesto = custas.ListarCustasProtesto(intimacao.DataEntrada.Year);

                var tarifa = custasProtesto.Where(p => p.DESCR == "TARIFA BANCÁRIA").FirstOrDefault();

                intimacao.Tarifa = Convert.ToDecimal(tarifa.VALOR);

                intimacao.Total = intimacao.ValorChequeAdm + intimacao.Tarifa;

                intimacao.ValoBoleto = intimacao.Total;

                intimacao.Visualizado = false;

                intimacoes.Add(intimacao);


            }


        }

        private bool VerificarExistenciaIntimacao(int idAto)
        {
            bool result = false;

            MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
            con.Open();

            MySqlCommand cmd = new MySqlCommand("select * FROM IntimacaoProtesto WHERE IdAto = " + idAto, con);

            cmd.CommandType = CommandType.Text;

            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
                result = true;
            

            con.Close();

            return result;
           
        }

        private void EnviarIntimacaoProtesto()
        {
            try
            {

                foreach (var enviar in intimacoes)
                {

                    if (VerificarExistenciaIntimacao(enviar.IdAto) == false)
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("INSERT INTO IntimacaoProtesto (IdAto, Atribuicao, Documento, DataAto, Tipo, Descricao, Status, Selo, TipoCobranca, DataEntrada, Protocolo, DataPrazo, Devedor, CpfCnpjDevedor, Endereco, Bairro, Municipio, UF, CEP, TipoPessoa, TipoTitulo, NumeroTitulo, Portador, Cedente, Sacador, Praca, FinsFalimentares, DataTitulo, DataVencimento, ValorTitulo, Custas, Distribuicao, ValorChequeAdm, Tarifa, Total, DataIntimacao, ValoBoleto, Visualizado) VALUES (@IdAto, @Atribuicao, @Documento, @DataAto, @Tipo, @Descricao, @Status, @Selo, @TipoCobranca, @DataEntrada, @Protocolo, @DataPrazo, @Devedor, @CpfCnpjDevedor, @Endereco, @Bairro, @Municipio, @UF, @CEP, @TipoPessoa, @TipoTitulo, @NumeroTitulo, @Portador, @Cedente, @Sacador, @Praca, @FinsFalimentares, @DataTitulo, @DataVencimento, @ValorTitulo, @Custas, @Distribuicao, @ValorChequeAdm, @Tarifa, @Total, @DataIntimacao, @ValoBoleto, @Visualizado)", con);

                            cmd.Parameters.Add(new MySqlParameter("@IdAto", enviar.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", enviar.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", enviar.Documento));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", enviar.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", enviar.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", enviar.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Status", enviar.Status));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", enviar.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", enviar.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@DataEntrada", enviar.DataEntrada));
                            cmd.Parameters.Add(new MySqlParameter("@Protocolo", enviar.Protocolo));
                            cmd.Parameters.Add(new MySqlParameter("@DataPrazo", enviar.DataPrazo));
                            cmd.Parameters.Add(new MySqlParameter("@Devedor", enviar.Devedor));
                            cmd.Parameters.Add(new MySqlParameter("@CpfCnpjDevedor", enviar.CpfCnpjDevedor));
                            cmd.Parameters.Add(new MySqlParameter("@Endereco", enviar.Endereco));
                            cmd.Parameters.Add(new MySqlParameter("@Bairro", enviar.Bairro));
                            cmd.Parameters.Add(new MySqlParameter("@Municipio", enviar.Municipio));
                            cmd.Parameters.Add(new MySqlParameter("@UF", enviar.UF));
                            cmd.Parameters.Add(new MySqlParameter("@CEP", enviar.CEP));
                            cmd.Parameters.Add(new MySqlParameter("@TipoPessoa", enviar.TipoPessoa));
                            cmd.Parameters.Add(new MySqlParameter("@TipoTitulo", enviar.TipoTitulo));
                            cmd.Parameters.Add(new MySqlParameter("@NumeroTitulo", enviar.NumeroTitulo));
                            cmd.Parameters.Add(new MySqlParameter("@Portador", enviar.Portador));
                            cmd.Parameters.Add(new MySqlParameter("@Cedente", enviar.Cedente));
                            cmd.Parameters.Add(new MySqlParameter("@Sacador", enviar.Sacador));
                            cmd.Parameters.Add(new MySqlParameter("@Praca", enviar.Praca));
                            cmd.Parameters.Add(new MySqlParameter("@FinsFalimentares", enviar.FinsFalimentares));
                            cmd.Parameters.Add(new MySqlParameter("@DataTitulo", enviar.DataTitulo));
                            cmd.Parameters.Add(new MySqlParameter("@DataVencimento", enviar.DataVencimento));
                            cmd.Parameters.Add(new MySqlParameter("@ValorTitulo", enviar.ValorTitulo));
                            cmd.Parameters.Add(new MySqlParameter("@Custas", enviar.Custas));
                            cmd.Parameters.Add(new MySqlParameter("@Distribuicao", enviar.Distribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@ValorChequeAdm", enviar.ValorChequeAdm));
                            cmd.Parameters.Add(new MySqlParameter("@Tarifa", enviar.Tarifa));
                            cmd.Parameters.Add(new MySqlParameter("@Total", enviar.Total));
                            cmd.Parameters.Add(new MySqlParameter("@DataIntimacao", enviar.DataIntimacao));
                            cmd.Parameters.Add(new MySqlParameter("@ValoBoleto", enviar.ValoBoleto));
                            cmd.Parameters.Add(new MySqlParameter("@Visualizado", enviar.Visualizado));

                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DeletarEnviarIntimacao(int id)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM IntimacaoProtesto WHERE IdAto = '" + id + "'", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();


            }
            catch (Exception)
            {
            }

        }

        private bool VerificarExistenteApontamento(ConsultaApontado consultaApontado)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("select * FROM ConsultaApontado WHERE IdAto = '" + consultaApontado.IdAto + "'", con);

                cmd.CommandType = CommandType.Text;

                MySqlDataReader dr = cmd.ExecuteReader();


                con.Close();


                if (dr.HasRows)
                    return true;
                else
                    return false;



            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ConectarEnviarApontado(ConsultaApontado enviar)
        {
            try
            {
                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();


                string dataProtesto = string.Format("{0}-{1}-{2} 00:00:00", enviar.DataProtocolo.Year, enviar.DataProtocolo.Month, enviar.DataProtocolo.Day);

                MySqlCommand cmd = new MySqlCommand("INSERT INTO ConsultaApontado(IdAto, Devedor, DocumentoDevedor, DataProtocolo, Protocolo, SolicitarBoleto) VALUES ('" + enviar.IdAto + "','" + enviar.Devedor + "','" + enviar.DocumentoDevedor + "','" + dataProtesto + "'" + ",'" + enviar.Protocolo + "'" + ", 0)", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception)
            {

            }
        }

        private void DeletarEnviarApontado(ConsultaApontado enviar)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM ConsultaApontado WHERE IdAto = '" + enviar.IdAto + "'", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();


            }
            catch (Exception)
            {
            }

        }

        private bool VerificarExistenteProtesto(ConsultaProtesto consultaProtesto)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("select * FROM ConsultaProtesto WHERE IdAto = '" + consultaProtesto.IdAto + "'", con);

                cmd.CommandType = CommandType.Text;

                MySqlDataReader dr = cmd.ExecuteReader();


                con.Close();


                if (dr.HasRows)
                    return true;
                else
                    return false;



            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ConectarEnviarProtesto(ConsultaProtesto enviar)
        {
            try
            {
                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();


                string dataProtesto = string.Format("{0}-{1}-{2} 00:00:00", enviar.DataProtesto.Year, enviar.DataProtesto.Month, enviar.DataProtesto.Day);

                MySqlCommand cmd = new MySqlCommand("INSERT INTO ConsultaProtesto(IdAto, Devedor, DocumentoDevedor, DataProtesto) VALUES ('" + enviar.IdAto + "','" + enviar.Devedor + "','" + enviar.DocumentoDevedor + "','" + dataProtesto + "'" + ")", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception)
            {

            }
        }

        private void DeletarEnviarProtesto(ConsultaProtesto enviar)
        {
            try
            {

                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM ConsultaProtesto WHERE IdAto = '" + enviar.IdAto + "'", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();


            }
            catch (Exception)
            {
            }

        }


        public void ObterTitulosCaducados()
        {

            var data = string.Format("{0}-{1}-{2}", DateTime.Now.Year - 5, DateTime.Now.Month, DateTime.Now.Day);

            string result = string.Empty;

            MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
            con.Open();

            MySqlCommand cmd = new MySqlCommand("DELETE FROM ConsultaProtesto WHERE dataprotesto <= '" + data + " 00:00:00'", con);

            cmd.CommandType = CommandType.Text;

            cmd.ExecuteNonQuery();

            con.Close();
        }




        // --------------- Sincronizar Site ----------------------------------

        UsuariosSite UsuarioSite;
        AtosPraticadosSite atosPraticados;




        private void SincronizarSite()
        {

            List<UsuariosSite> usuariosSite;

            usuariosSite = VerificarUsuarios(_documento);

            qtdUsuariosSite = usuariosSite.Count;

            carregaAtual = "Sincronizando Usuários do Site";

            if (usuariosSite != null)
                AtualizarCadaUsuariosNoBdTotal(usuariosSite);
        }



        // ----------------- Principal ----------------------------------------
        private void AtualizarCadaUsuariosNoBdTotal(List<UsuariosSite> usuariosSite)
        {
            try
            {


                for (int i = 0; i < usuariosSite.Count; i++)
                {
                    UsuarioSite = new UsuariosSite();

                    atosPraticados = new AtosPraticadosSite();
                    atosPraticados.Firmas = new List<AtosFirmasSite>();
                    atosPraticados.AtosNotas = new List<AtosNotasSite>();
                    atosPraticados.AtosRgi = new List<AtosRgiSite>();
                    atosPraticados.AtosProtesto = new List<AtosProtestoSite>();
                    intimacoes = new List<IntimacaoProtesto>();

                    UsuarioSite = usuariosSite[i];

                    Thread.Sleep(1);
                    worker.ReportProgress(i + 1);

                    if (UsuarioSite.TipoPessoa == "Física")
                        VerificarFirmas();

                    VerificarNotas();
                    VerificarProtesto();
                    VerificarRgi();

                    AtualizarUsuarioVerificado();


                    EnviarEmails();

                    using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                    {
                        MySqlCommand cmd = new MySqlCommand("update AspNetUsers set EnviarEmail = true WHERE Documento = '" + UsuarioSite.Documento + "'", con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        //^^^^^^^^^^^^^^^^ Principal ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        private void EnviarEmails()
        {
            var emalis = enviarEmail.ObterEmailsEnviar(UsuarioSite.Documento);

            foreach (var item in emalis)
            {
                string html = enviarEmail.ObterArquivoHtml(UsuarioSite.Nome, item.Mensagem);

                bool enviado = enviarEmail.EnvioMensagem(UsuarioSite.Email, "contato@1oficioararuama.com.br", "Cartório do 1º Ofício de Araruama", html);

                if (enviado == true)
                    enviarEmail.AlterarEmailEnviado(item);
            }

        }

        private List<UsuariosSite> VerificarUsuarios(string documento)
        {

            List<UsuariosSite> usuarios = new List<UsuariosSite>();

            MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
            con.Open();

            MySqlCommand cmd;

            if (documento == "todos")
                cmd = new MySqlCommand("select * FROM AspNetUsers where ConfirmacaoPresencial = true and Ativo = true and ReceberNotificacao = true", con);
            else
                cmd = new MySqlCommand("select * FROM AspNetUsers where ConfirmacaoPresencial = true and Ativo = true and ReceberNotificacao = true and Documento = '" + documento + "'", con);

            cmd.CommandType = CommandType.Text;

            MySqlDataReader dr = cmd.ExecuteReader();


            UsuariosSite usuarioSite;

            while (dr.Read())
            {
                usuarioSite = new UsuariosSite();

                usuarioSite.Ativo = Convert.ToBoolean(dr["Ativo"]);

                usuarioSite.AtosFirmas = Convert.ToInt32(dr["AtosFirmas"]);

                usuarioSite.AtosNotas = Convert.ToInt32(dr["AtosNotas"]);

                usuarioSite.AtosProtesto = Convert.ToInt32(dr["AtosProtesto"]);

                usuarioSite.AtosRgi = Convert.ToInt32(dr["AtosRgi"]);

                usuarioSite.ConfirmacaoPresencial = Convert.ToBoolean(dr["ConfirmacaoPresencial"]);

                usuarioSite.DataCadastro = Convert.ToDateTime(dr["DataCadastro"]);

                usuarioSite.DataModificado = Convert.ToDateTime(dr["DataModificado"]);

                usuarioSite.DataUltimaAtualizacao = Convert.ToDateTime(dr["DataUltimaAtualizacao"]);

                usuarioSite.Documento = dr["Documento"].ToString();

                usuarioSite.IntimacaoProtesto = Convert.ToInt32(dr["IntimacaoProtesto"]);

                usuarioSite.Nome = dr["Nome"].ToString();

                usuarioSite.ReceberNotificacao = Convert.ToBoolean(dr["ReceberNotificacao"]);

                usuarioSite.TipoPessoa = dr["TipoPessoa"].ToString();

                usuarioSite.Email = dr["Email"].ToString();

                usuarioSite.EnviarEmail = Convert.ToBoolean(dr["EnviarEmail"]);

                usuarios.Add(usuarioSite);

            }

            con.Close();

            return usuarios;
        }


        //------------------- Atos Firmas --------------------------------------

        private void VerificarFirmas()
        {

            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingBalcaoSite);
            conTotal.Open();

            FbCommand cmdTotal = new FbCommand("SELECT ID FROM fichas WHERE CPF = '" + UsuarioSite.Documento + "'", conTotal);

            cmdTotal.CommandType = CommandType.Text;

            FbDataReader drFicha;
            drFicha = cmdTotal.ExecuteReader();

            List<int> idsFichas = new List<int>();

            while (drFicha.Read())
            {
                idsFichas.Add(Convert.ToInt32(drFicha["ID"]));
            }
            conTotal.Close();




            foreach (var item in idsFichas)
            {
                ObterAtosTabelaTrec(item);
                ObterAtosTabelaAtos(item);
            }

        }


        private void ObterAtosTabelaTrec(int idFicha)
        {
            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingBalcaoSite);
            conTotal.Open();

            FbCommand cmdTotal = new FbCommand("SELECT * FROM TREC WHERE LINK = 'S' AND ID_FICHA = " + idFicha, conTotal);

            cmdTotal.CommandType = CommandType.Text;

            FbDataReader drFicha;
            drFicha = cmdTotal.ExecuteReader();

            AtosFirmasSite atosTotal;

            while (drFicha.Read())
            {
                atosTotal = new AtosFirmasSite();

                atosTotal.Aleatorio = drFicha["ALEATORIO"].ToString();
                atosTotal.Atribuicao = "BALCÃO DE FIRMAS";
                atosTotal.DataAto = Convert.ToDateTime(drFicha["DATA"]);
                atosTotal.Descricao = drFicha["NOME"].ToString();
                atosTotal.IdAto = Convert.ToInt32(drFicha["ID_ATO"]);
                atosTotal.Selo = drFicha["SELO"].ToString();
                atosTotal.Total = Convert.ToDecimal("0");

                switch (drFicha["TIPOATO"].ToString())
                {
                    case "RFA":
                        atosTotal.Descricao = "ABERTURA DE FIRMA";
                        atosTotal.Tipo = "ABERTURA DE FIRMA";
                        break;
                    case "RFR":
                        if (drFicha["TIPO_REC"].ToString() == "A")
                            atosTotal.Descricao = "RECONHECIMENTO DE FIRMAS POR AUTENTICIDADE";
                        else
                            atosTotal.Descricao = "RECONHECIMENTO DE FIRMAS POR SEMELHANÇA";

                        atosTotal.Tipo = "RECONHECIMENTO";
                        break;
                    default:
                        atosTotal.Tipo = "RECONHECIMENTO";
                        atosTotal.Descricao = "RECONHECIMENTO DE FIRMAS";
                        break;
                }

                switch (drFicha["TIPO_COBRANCA"].ToString())
                {
                    case "CC":
                        atosTotal.TipoCobranca = "COM COBRANÇA";
                        break;

                    case "JG":
                        atosTotal.TipoCobranca = "JUSTIÇA GRATUITA";
                        break;

                    case "SC":
                        atosTotal.TipoCobranca = "SEM COBRANÇA";
                        break;

                    default:
                        atosTotal.TipoCobranca = "NÃO INFORMADO";
                        break;
                }

                atosPraticados.Firmas.Add(atosTotal);

            }

            conTotal.Close();
        }

        private void ObterAtosTabelaAtos(int idFicha)
        {
            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingBalcaoSite);
            conTotal.Open();

            FbCommand cmdTotal = new FbCommand("SELECT * FROM ATOS WHERE STATUS = 'XML' AND ID_FICHA = " + idFicha, conTotal);

            cmdTotal.CommandType = CommandType.Text;

            FbDataReader drFicha;
            drFicha = cmdTotal.ExecuteReader();

            AtosFirmasSite atosTotal;

            while (drFicha.Read())
            {
                atosTotal = new AtosFirmasSite();

                atosTotal.Aleatorio = drFicha["ALEATORIO"].ToString();
                atosTotal.Atribuicao = "BALCÃO DE FIRMAS";
                atosTotal.DataAto = Convert.ToDateTime(drFicha["DATA"]);
                atosTotal.Descricao = drFicha["DESCRICAO"].ToString();
                atosTotal.IdAto = Convert.ToInt32(drFicha["ID_ATO"]);
                atosTotal.IdServico = Convert.ToInt32(drFicha["ID_SERVICO"]);
                atosTotal.Selo = drFicha["SELO"].ToString();
                atosTotal.Recibo = Convert.ToInt32(drFicha["RECIBO"]);
                atosTotal.Total = Convert.ToDecimal(drFicha["TOTAL"]);

                switch (drFicha["TIPO"].ToString())
                {
                    case "ABR":
                        atosTotal.Descricao = "ABERTURA DE FIRMA";
                        atosTotal.Tipo = "ABERTURA DE FIRMA";
                        break;
                    case "RFA":
                        atosTotal.Descricao = "RECONHECIMENTO DE FIRMAS POR AUTENTICIDADE";
                        atosTotal.Tipo = "RECONHECIMENTO";
                        break;
                    case "RFR":
                        atosTotal.Descricao = "RECONHECIMENTO DE FIRMAS POR SEMELHANÇA";
                        atosTotal.Tipo = "RECONHECIMENTO";
                        break;
                    default:
                        atosTotal.Descricao = "RECONHECIMENTO DE FIRMAS";
                        atosTotal.Tipo = "RECONHECIMENTO";
                        break;
                }


                switch (drFicha["COBRANCA"].ToString())
                {
                    case "CC":
                        atosTotal.TipoCobranca = "COM COBRANÇA";
                        break;

                    case "JG":
                        atosTotal.TipoCobranca = "JUSTIÇA GRATUITA";
                        break;

                    case "SC":
                        atosTotal.TipoCobranca = "SEM COBRANÇA";
                        break;

                    default:
                        atosTotal.TipoCobranca = "NÃO INFORMADO";
                        break;
                }



                atosPraticados.Firmas.Add(atosTotal);

            }

            conTotal.Close();
        }

        //^^^^^^^^^^^^^^^^^^^^^^^ Atos Firmas ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


        private void EnvioEmail(int idato, string documento, string descricao, string tipo)
        {
            Email email = new Email();
            email.Data = DateTime.Now.Date;
            email.Documento = documento;
            email.Enviado = false;
            email.Indicador = string.Format("Id Ato: {0} - {1}", idato, tipo);

            if (tipo != "APONTADO" && tipo != "DEVEDOR")
                email.Mensagem = string.Format("Verificamos em nossos registros um ato realizado referente ao documento: {0} e ato: {1}. Acesse: <a href='https://1oficioararuama.com.br'>https://1oficioararuama.com.br</a> para saber mais.", documento, descricao);
            else
            {
                if (tipo == "APONTADO")
                    email.Mensagem = string.Format("Verificamos em nossos registros um título apontado para protesto referente ao documento: {0} e Título nº: {1}. Acesse: <a href='https://1oficioararuama.com.br'>https://1oficioararuama.com.br</a> para saber mais.", documento, descricao);
                else
                    email.Mensagem = string.Format("Verificamos em nossos registros um ato realizado referente ao documento: {0} e Título nº: {1}. Acesse: <a href='https://1oficioararuama.com.br'>https://1oficioararuama.com.br</a> para saber mais.", documento, descricao);
            }

            enviarEmail.SalvarEmail(email);
        }


        private void AtualizarUsuarioVerificado()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                {

                    MySqlCommand cmd = new MySqlCommand("update AspNetUsers set AtosFirmas = @AtosFirmas, AtosNotas = @AtosNotas, AtosProtesto = @AtosProtesto, IntimacaoProtesto = @IntimacaoProtesto WHERE Documento = '" + UsuarioSite.Documento + "'", con);

                    cmd.Parameters.Add(new MySqlParameter("@AtosFirmas", atosPraticados.Firmas.Count));
                    cmd.Parameters.Add(new MySqlParameter("@AtosNotas", atosPraticados.AtosNotas.Count));
                    cmd.Parameters.Add(new MySqlParameter("@AtosProtesto", atosPraticados.AtosProtesto.Count));
                    cmd.Parameters.Add(new MySqlParameter("@IntimacaoProtesto", intimacoes.Where(p => p.CpfCnpjDevedor == UsuarioSite.Documento).ToList().Count));
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bool existe;

                foreach (var item in atosPraticados.Firmas)
                {
                    existe = false;

                    using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                    {
                        MySqlCommand cmd = new MySqlCommand("select * from AtosFirmas WHERE idato = " + item.IdAto, con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        if (dr.HasRows == true)
                        {
                            existe = true;
                        }
                    }

                    if (existe == false)
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("INSERT INTO AtosFirmas(Aleatorio, Atribuicao, DataAto, Descricao, IdServico, Selo, Recibo, Tipo, TipoCobranca, Documento, IdAto, Total, TotalDtd) VALUES (@Aleatorio,  @Atribuicao, @DataAto,  @Descricao, @IdServico, @Selo, @Recibo, @Tipo, @TipoCobranca, @Documento, @IdAto, @Total, @TotalDtd)", con);

                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@IdServico", item.IdServico));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Recibo", item.Recibo));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", UsuarioSite.Documento));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@TotalDtd", Convert.ToDecimal(0)));

                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            if (UsuarioSite.EnviarEmail == true)
                                EnvioEmail(item.IdAto, UsuarioSite.Documento, item.Descricao, item.Tipo);
                        }

                    }


                }



                foreach (var item in atosPraticados.AtosNotas)
                {
                    existe = false;

                    using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                    {
                        MySqlCommand cmd = new MySqlCommand("select * from AtosNotas WHERE idato = " + item.IdAto, con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        if (dr.HasRows == true)
                        {
                            existe = true;
                        }
                    }

                    if (existe == false)
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("INSERT INTO AtosNotas(Aleatorio, Atribuicao, DataAto, Descricao, Selo, Tipo, TipoCobranca, Documento, IdAto, Total, Ato, FolhaFim, FolhaInicio, Livro, Qualificacao) VALUES (@Aleatorio, @Atribuicao, @DataAto,  @Descricao, @Selo, @Tipo, @TipoCobranca, @Documento, @IdAto, @Total, @Ato, @FolhaFim, @FolhaInicio, @Livro, @Qualificacao)", con);

                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", UsuarioSite.Documento));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@Ato", item.Ato));
                            cmd.Parameters.Add(new MySqlParameter("@FolhaFim", item.FolhaFim));
                            cmd.Parameters.Add(new MySqlParameter("@FolhaInicio", item.FolhaInicio));
                            cmd.Parameters.Add(new MySqlParameter("@Livro", item.Livro));
                            cmd.Parameters.Add(new MySqlParameter("@Qualificacao", item.Qualificacao));

                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();

                            if (UsuarioSite.EnviarEmail == true)
                                EnvioEmail(item.IdAto, UsuarioSite.Documento, item.Descricao, item.Tipo);
                        }

                    }
                    else
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("UPDATE AtosNotas SET Aleatorio = @Aleatorio,  Atribuicao = @Atribuicao, DataAto = @DataAto,  Descricao = @Descricao, Selo = @Selo, Tipo = @Tipo, TipoCobranca = @TipoCobranca, Documento = @Documento, IdAto = @IdAto, Total = @Total, Ato = @Ato, FolhaFim = @FolhaFim, FolhaInicio = @FolhaInicio, Livro = @Livro, Qualificacao = @Qualificacao WHERE IdAto = " + item.IdAto, con);

                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", UsuarioSite.Documento));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@Ato", item.Ato));
                            cmd.Parameters.Add(new MySqlParameter("@FolhaFim", item.FolhaFim));
                            cmd.Parameters.Add(new MySqlParameter("@FolhaInicio", item.FolhaInicio));
                            cmd.Parameters.Add(new MySqlParameter("@Livro", item.Livro));
                            cmd.Parameters.Add(new MySqlParameter("@Qualificacao", item.Qualificacao));

                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }


                }


                foreach (var item in atosPraticados.AtosProtesto)
                {
                    existe = false;

                    using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                    {
                        MySqlCommand cmd = new MySqlCommand("select * from AtosProtesto WHERE idato = " + item.IdAto, con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        if (dr.HasRows == true)
                        {
                            existe = true;
                        }
                    }

                    if (existe == false)
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("INSERT INTO AtosProtesto(Aleatorio, Atribuicao, DataAto, Descricao, Selo, Tipo, TipoCobranca, Documento, IdAto, Total, Protocolo, Status) VALUES (@Aleatorio, @Atribuicao, @DataAto, @Descricao, @Selo, @Tipo, @TipoCobranca, @Documento, @IdAto, @Total, @Protocolo, @Status)", con);

                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", UsuarioSite.Documento));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@Protocolo", item.Protocolo));
                            cmd.Parameters.Add(new MySqlParameter("@Status", item.Status));


                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();


                            if (UsuarioSite.EnviarEmail == true)
                            {
                                if (item.Status == "APONTADO")
                                    EnvioEmail(item.IdAto, UsuarioSite.Documento, item.Descricao, item.Status);
                                else
                                    EnvioEmail(item.IdAto, UsuarioSite.Documento, item.Descricao, item.Tipo);
                            }
                        }

                    }
                    else
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("UPDATE AtosProtesto SET Aleatorio = @Aleatorio,  Atribuicao = @Atribuicao, DataAto = @DataAto,  Descricao = @Descricao, Selo = @Selo, Tipo = @Tipo, TipoCobranca = @TipoCobranca, Documento = @Documento, IdAto = @IdAto, Total = @Total, Protocolo = @Protocolo, Status = @Status WHERE IdAto = " + item.IdAto, con);

                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", UsuarioSite.Documento));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@Protocolo", item.Protocolo));
                            cmd.Parameters.Add(new MySqlParameter("@Status", item.Status));

                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }


                }

                foreach (var item in atosPraticados.AtosRgi)
                {
                    existe = false;

                    using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                    {
                        MySqlCommand cmd = new MySqlCommand("select * from AtosRgi WHERE idato = " + item.IdAto, con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        if (dr.HasRows == true)
                        {
                            existe = true;
                        }
                    }

                    if (existe == false)
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("INSERT INTO AtosRgi(Qualificacao, Protocolo, DataProtocolo, DataRegistro, Recibo, Matricula, TipoLancamento, NúmeroLancamento, Distribuicao, Prenotacao, Buscas, IdAto, DataAto, Tipo, Atribuicao, Descricao, Status, Selo, Aleatorio, TipoCobranca, Total, Documento) VALUES (@Qualificacao, @Protocolo, @DataProtocolo, @DataRegistro, @Recibo, @Matricula, @TipoLancamento, @NúmeroLancamento, @Distribuicao, @Prenotacao, @Buscas, @IdAto, @DataAto, @Tipo, @Atribuicao, @Descricao, @Status, @Selo, @Aleatorio, @TipoCobranca, @Total, @Documento)", con);

                            cmd.Parameters.Add(new MySqlParameter("@Qualificacao", item.Qualificacao));
                            cmd.Parameters.Add(new MySqlParameter("@Protocolo", item.Protocolo));
                            cmd.Parameters.Add(new MySqlParameter("@DataProtocolo", item.DataProtocolo));
                            cmd.Parameters.Add(new MySqlParameter("@DataRegistro", item.DataRegistro));
                            cmd.Parameters.Add(new MySqlParameter("@Recibo", item.Recibo));
                            cmd.Parameters.Add(new MySqlParameter("@Matricula", item.Matricula));
                            cmd.Parameters.Add(new MySqlParameter("@TipoLancamento", item.TipoLancamento));
                            cmd.Parameters.Add(new MySqlParameter("@NúmeroLancamento", item.NúmeroLancamento));
                            cmd.Parameters.Add(new MySqlParameter("@Distribuicao", item.Distribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@Prenotacao", item.Prenotacao));
                            cmd.Parameters.Add(new MySqlParameter("@Buscas", item.Buscas));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Status", item.Status));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", item.Documento));
                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();

                            if (UsuarioSite.EnviarEmail == true)
                                EnvioEmail(item.IdAto, UsuarioSite.Documento, item.Descricao, item.Tipo);
                        }

                    }
                    else
                    {
                        using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
                        {

                            MySqlCommand cmd = new MySqlCommand("UPDATE AtosRgi SET Qualificacao = @Qualificacao, Protocolo = @Protocolo, DataProtocolo = @DataProtocolo, DataRegistro = @DataRegistro, Recibo = @Recibo, Matricula = @Matricula, TipoLancamento = @TipoLancamento, NúmeroLancamento = @NúmeroLancamento, Distribuicao = @Distribuicao, Prenotacao = @Prenotacao, Buscas = @Buscas, IdAto = @IdAto, DataAto = @DataAto, Tipo = @Tipo, Atribuicao = @Atribuicao, Descricao = @Descricao, Status = @Status, Selo = @Selo, Aleatorio = @Aleatorio, TipoCobranca = @TipoCobranca, Total = @Total, Documento = @Documento WHERE IdAto = " + item.IdAto, con);

                            cmd.Parameters.Add(new MySqlParameter("@Qualificacao", item.Qualificacao));
                            cmd.Parameters.Add(new MySqlParameter("@Protocolo", item.Protocolo));
                            cmd.Parameters.Add(new MySqlParameter("@DataProtocolo", item.DataProtocolo));
                            cmd.Parameters.Add(new MySqlParameter("@DataRegistro", item.DataRegistro));
                            cmd.Parameters.Add(new MySqlParameter("@Recibo", item.Recibo));
                            cmd.Parameters.Add(new MySqlParameter("@Matricula", item.Matricula));
                            cmd.Parameters.Add(new MySqlParameter("@TipoLancamento", item.TipoLancamento));
                            cmd.Parameters.Add(new MySqlParameter("@NúmeroLancamento", item.NúmeroLancamento));
                            cmd.Parameters.Add(new MySqlParameter("@Distribuicao", item.Distribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@Prenotacao", item.Prenotacao));
                            cmd.Parameters.Add(new MySqlParameter("@Buscas", item.Buscas));
                            cmd.Parameters.Add(new MySqlParameter("@IdAto", item.IdAto));
                            cmd.Parameters.Add(new MySqlParameter("@DataAto", item.DataAto));
                            cmd.Parameters.Add(new MySqlParameter("@Tipo", item.Tipo));
                            cmd.Parameters.Add(new MySqlParameter("@Atribuicao", item.Atribuicao));
                            cmd.Parameters.Add(new MySqlParameter("@Descricao", item.Descricao));
                            cmd.Parameters.Add(new MySqlParameter("@Status", item.Status));
                            cmd.Parameters.Add(new MySqlParameter("@Selo", item.Selo));
                            cmd.Parameters.Add(new MySqlParameter("@Aleatorio", item.Aleatorio));
                            cmd.Parameters.Add(new MySqlParameter("@TipoCobranca", item.TipoCobranca));
                            cmd.Parameters.Add(new MySqlParameter("@Total", item.Total));
                            cmd.Parameters.Add(new MySqlParameter("@Documento", item.Documento));
                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }


                }

                EnviarIntimacaoProtesto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }











        //------------------- Atos Notas --------------------------------------


        private void VerificarNotas()
        {


            FbConnection conPessoa = new FbConnection(Properties.Settings.Default.SettingCentral);
            conPessoa.Open();

            FbCommand cmdTotal = new FbCommand("select * from pessoas where cpf_cgc  = '" + UsuarioSite.Documento + "'", conPessoa);

            cmdTotal.CommandType = CommandType.Text;

            FbDataReader drPessoa;
            drPessoa = cmdTotal.ExecuteReader();


            List<AtosNotasSite> atosNotasTemp = new List<AtosNotasSite>();
            AtosNotasSite ato = new AtosNotasSite();

            while (drPessoa.Read())
            {
                ato = new AtosNotasSite();
                ato.Id = Convert.ToInt32(drPessoa["ID"]);
                ato.Documento = UsuarioSite.Documento;
                atosNotasTemp.Add(ato);
            }

            conPessoa.Close();


            foreach (var item in atosNotasTemp)
            {
                var listNome = ObterNome(item.Id);

                int idato = Convert.ToInt32(listNome[1]);

                if (idato > 0)
                {
                    item.Qualificacao = listNome[0];
                    item.IdAto = idato;
                }
            }



            foreach (var item in atosNotasTemp)
            {
                using (FbConnection con = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    FbCommand cmd = new FbCommand("select * from ESCRITURAS WHERE ID_ATO = " + item.IdAto, con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    FbDataReader drAtoEscritura;

                    drAtoEscritura = cmd.ExecuteReader();

                    while (drAtoEscritura.Read())
                    {
                        ato = new AtosNotasSite();
                        ato.Qualificacao = item.Qualificacao;
                        ato.IdAto = item.IdAto;
                        ato.Documento = item.Documento;
                        ato.Aleatorio = drAtoEscritura["ALEATORIO"].ToString();
                        ato.Ato = drAtoEscritura["NUM_ATO"].ToString();
                        ato.Atribuicao = "NOTAS";
                        ato.DataAto = Convert.ToDateTime(drAtoEscritura["DT_ATO_REG"]);
                        ato.Descricao = "ESCRITURA";
                        ato.FolhaFim = drAtoEscritura["FL_FIM"].ToString();
                        ato.FolhaInicio = drAtoEscritura["FL_INI"].ToString();
                        ato.Livro = drAtoEscritura["LIVRO_ESCR"].ToString();
                        ato.Selo = drAtoEscritura["SELO_ESCR"].ToString();
                        switch (drAtoEscritura["TP_COBRA"].ToString())
                        {
                            case "CC":
                                ato.TipoCobranca = "COM COBRANÇA";
                                break;

                            case "JG":
                                ato.TipoCobranca = "JUSTIÇA GRATUITA";
                                break;

                            case "SC":
                                ato.TipoCobranca = "SEM COBRANÇA";
                                break;

                            default:
                                ato.TipoCobranca = "NÃO INFORMADO";
                                break;
                        }
                        ato.Total = Convert.ToDecimal(drAtoEscritura["TOTAL"]);
                        ato.Tipo = "ESCRITURA";
                        atosPraticados.AtosNotas.Add(ato);
                    }
                }
            }

            foreach (var item in atosNotasTemp)
            {
                using (FbConnection con = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    FbCommand cmd = new FbCommand("select * from LINK_PROCURACAO WHERE ID_ATO = " + item.IdAto, con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    FbDataReader drAtoProcuracao;

                    drAtoProcuracao = cmd.ExecuteReader();



                    while (drAtoProcuracao.Read())
                    {
                        ato = new AtosNotasSite();
                        ato.Qualificacao = item.Qualificacao;
                        ato.IdAto = item.IdAto;
                        ato.Documento = item.Documento;
                        ato.Aleatorio = drAtoProcuracao["ALEATORIO"].ToString();
                        ato.Ato = drAtoProcuracao["NUM_ATO"].ToString();
                        ato.Atribuicao = "NOTAS";
                        ato.DataAto = Convert.ToDateTime(drAtoProcuracao["DT_LAVRATURA"]);
                        ato.Descricao = "PROCURAÇÃO";
                        ato.FolhaFim = drAtoProcuracao["FL_FIM"].ToString();
                        ato.FolhaInicio = drAtoProcuracao["FL_INI"].ToString();
                        ato.Livro = drAtoProcuracao["LIVRO"].ToString();
                        ato.Selo = drAtoProcuracao["SELO"].ToString();
                        switch (drAtoProcuracao["TP_COBRA"].ToString())
                        {
                            case "CC":
                                ato.TipoCobranca = "COM COBRANÇA";
                                break;

                            case "JG":
                                ato.TipoCobranca = "JUSTIÇA GRATUITA";
                                break;

                            case "SC":
                                ato.TipoCobranca = "SEM COBRANÇA";
                                break;

                            default:
                                ato.TipoCobranca = "NÃO INFORMADO";
                                break;
                        }
                        ato.Total = Convert.ToDecimal(drAtoProcuracao["TOTAL"]);
                        ato.Tipo = "PROCURAÇÃO";
                        atosPraticados.AtosNotas.Add(ato);
                    }
                }
            }

            using (FbConnection con = new FbConnection(Properties.Settings.Default.SettingNotas))
            {
                FbCommand cmd = new FbCommand("select * from TESTAMENTOS WHERE CPF = '" + UsuarioSite.Documento + "'", con);

                cmd.CommandType = CommandType.Text;
                con.Open();
                FbDataReader drAtoTestamento;

                drAtoTestamento = cmd.ExecuteReader();



                while (drAtoTestamento.Read())
                {
                    ato = new AtosNotasSite();
                    ato.Qualificacao = "OUTORGANTE";
                    ato.IdAto = Convert.ToInt32(drAtoTestamento["ID_ATO"]);
                    ato.Documento = UsuarioSite.Documento;
                    ato.Aleatorio = drAtoTestamento["ALEATORIO"].ToString();
                    ato.Ato = drAtoTestamento["NUM_ATO"].ToString();
                    ato.Atribuicao = "NOTAS";
                    ato.DataAto = Convert.ToDateTime(drAtoTestamento["DT_ATO"]);
                    ato.Descricao = "TESTAMENTO";
                    ato.FolhaFim = drAtoTestamento["FL_FIM"].ToString();
                    ato.FolhaInicio = drAtoTestamento["FL_INI"].ToString();
                    ato.Livro = drAtoTestamento["LIVRO"].ToString();
                    ato.Selo = drAtoTestamento["SELO"].ToString();
                    switch (drAtoTestamento["TP_COBRA"].ToString())
                    {
                        case "CC":
                            ato.TipoCobranca = "COM COBRANÇA";
                            break;

                        case "JG":
                            ato.TipoCobranca = "JUSTIÇA GRATUITA";
                            break;

                        case "SC":
                            ato.TipoCobranca = "SEM COBRANÇA";
                            break;

                        default:
                            ato.TipoCobranca = "NÃO INFORMADO";
                            break;
                    }
                    ato.Total = Convert.ToDecimal(drAtoTestamento["TOTAL"]);
                    ato.Tipo = "TESTAMENTO";
                    atosPraticados.AtosNotas.Add(ato);

                }
            }


        }

        private List<String> ObterNome(int idPessoa)
        {
            List<String> listRetorno = new List<String>();

            FbConnection conNome = new FbConnection(Properties.Settings.Default.SettingNotas);
            conNome.Open();

            FbCommand cmdNome = new FbCommand("select DESCRICAO, ID_ATO from nomes where id_pessoa  = " + idPessoa, conNome);

            cmdNome.CommandType = CommandType.Text;

            FbDataReader drNome;
            drNome = cmdNome.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(drNome);

            string descricao = string.Empty;
            string idAto = "0";

            if (dt.Rows.Count > 0)
            {
                descricao = dt.Rows[0]["DESCRICAO"].ToString();
                idAto = dt.Rows[0]["ID_ATO"].ToString();
            }
            listRetorno.Add(descricao);
            listRetorno.Add(idAto);

            conNome.Close();

            return listRetorno;
        }

        //^^^^^^^^^^^^^^^^^^^^^^^ Atos Notas ^^^^^^^^^^^^^^^^^^^^^^^^^^^^






        //------------------- Atos Protesto --------------------------------------

        private void VerificarProtesto()
        {
            FbConnection conTitulo = new FbConnection(Properties.Settings.Default.SettingProtesto);
            conTitulo.Open();

            FbCommand cmdTotal = new FbCommand("select * from titulos where cpf_cnpj_devedor  = '" + UsuarioSite.Documento + "'", conTitulo);

            cmdTotal.CommandType = CommandType.Text;

            FbDataReader drTitulo;
            drTitulo = cmdTotal.ExecuteReader();


            atosPraticados.AtosProtesto = new List<AtosProtestoSite>();
            AtosProtestoSite ato = new AtosProtestoSite();

            while (drTitulo.Read())
            {
                ato = new AtosProtestoSite();
                ato.IdAto = Convert.ToInt32(drTitulo["ID_ATO"]);
                ato.Documento = UsuarioSite.Documento;
                ato.Status = drTitulo["STATUS"].ToString();
                ato.Descricao = drTitulo["NUMERO_TITULO"].ToString();
                ato.Tipo = "DEVEDOR";
                ato.Atribuicao = "PROTESTO";
                ato.Protocolo = Convert.ToInt32(drTitulo["PROTOCOLO"]);

                switch (ato.Status)
                {
                    case "APONTADO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_ENTRADA"]);
                        ato.Selo = drTitulo["CCT"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "INTIMADO PESSOAL":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_ENTRADA"]);
                        ato.Selo = drTitulo["CCT"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "INTIMADO EDITAL":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_ENTRADA"]);
                        ato.Selo = drTitulo["CCT"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "PROTESTADO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_REGISTRO"]);
                        ato.Selo = drTitulo["SELO_REGISTRO"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_PROTESTO"].ToString();
                        break;

                    case "RETIRADO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_RETIRADO"]);
                        ato.Selo = drTitulo["SELO_PAGAMENTO"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "PAGO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_PAGAMENTO"]);
                        ato.Selo = drTitulo["SELO_PAGAMENTO"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "DEVOLVIDO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_DEVOLVIDO"]);
                        ato.Selo = drTitulo["CCT"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "CANCELADO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_PAGAMENTO"]);
                        ato.Selo = drTitulo["SELO_PAGAMENTO"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    case "SUSTADO":
                        ato.DataAto = Convert.ToDateTime(drTitulo["DT_SUSTADO"]);
                        ato.Selo = drTitulo["SELO_PAGAMENTO"].ToString();
                        ato.Aleatorio = drTitulo["ALEATORIO_SOLUCAO"].ToString();
                        break;

                    default:
                        break;
                }

                switch (drTitulo["COBRANCA"].ToString())
                {
                    case "CC":
                        ato.TipoCobranca = "COM COBRANÇA";
                        break;

                    case "JG":
                        ato.TipoCobranca = "JUSTIÇA GRATUITA";
                        break;

                    case "SC":
                        ato.TipoCobranca = "SEM COBRANÇA";
                        break;

                    case "FL":
                        ato.TipoCobranca = "FORÇA DA LEI";
                        break;

                    default:
                        ato.TipoCobranca = "NÃO INFORMADO";
                        break;
                }

                ato.Total = Convert.ToDecimal(drTitulo["SALDO_PROTESTO"]);

                atosPraticados.AtosProtesto.Add(ato);

                if (ato.Status == "APONTADO")
                {
                    IntimacaoProtesto(ato.IdAto);
                }
                else
                    DeletarEnviarIntimacao(ato.IdAto);
            }

            conTitulo.Close();
        }


        //^^^^^^^^^^^^^^^^^^^^^^^ Atos Protesto ^^^^^^^^^^^^^^^^^^^^^^^^^^^^






        //------------------- Atos RGI --------------------------------------


        private void VerificarRgi()
        {
            FbConnection conAto = new FbConnection(Properties.Settings.Default.SettingRgi);
            conAto.Open();

            FbCommand cmdAto = new FbCommand("select * from ato where doc_apresentante  = '" + UsuarioSite.Documento + "'", conAto);

            cmdAto.CommandType = CommandType.Text;

            FbDataReader drAto;
            drAto = cmdAto.ExecuteReader();


            atosPraticados.AtosRgi = new List<AtosRgiSite>();
            AtosRgiSite ato = new AtosRgiSite();

            while (drAto.Read())
            {
                ato = new AtosRgiSite();
                ato.Qualificacao = "APRESENTANTE";

                if (drAto["PROTOCOLO"].ToString() != "")
                    ato.Protocolo = Convert.ToInt32(drAto["PROTOCOLO"]);
                if (drAto["DATA_PROTOCOLO"].ToString() != "")
                    ato.DataProtocolo = Convert.ToDateTime(drAto["DATA_PROTOCOLO"]);
                if (drAto["DATA_REGISTRO"].ToString() != "")
                    ato.DataRegistro = Convert.ToDateTime(drAto["DATA_REGISTRO"]);
                if (drAto["TALAO"].ToString() != "")
                    ato.Recibo = Convert.ToInt32(drAto["TALAO"]);

                ato.Matricula = drAto["MATRICULA"].ToString();

                ato.TipoLancamento = drAto["TIPO_LANCAMENTO"].ToString();

                if (drAto["NUM_LANCAMENTO"].ToString() != "")
                    ato.NúmeroLancamento = Convert.ToInt32(drAto["NUM_LANCAMENTO"]);
                if (drAto["DISTRIBUICAO"].ToString() != "")
                    ato.Distribuicao = Convert.ToDecimal(drAto["DISTRIBUICAO"]);
                if (drAto["PRENOTACAO"].ToString() != "")
                    ato.Prenotacao = Convert.ToDecimal(drAto["PRENOTACAO"]);
                if (drAto["BUSCAS"].ToString() != "")
                    ato.Buscas = Convert.ToDecimal(drAto["BUSCAS"]);

                ato.IdAto = Convert.ToInt32(drAto["ID_ATO"]);

                ato.Documento = UsuarioSite.Documento;

                ato.Status = drAto["STATUS"].ToString();

                ato.Descricao = drAto["DESC_NATUREZA"].ToString();

                switch (ato.TipoLancamento)
                {
                    case "R":
                        ato.Tipo = "REGISTRO";
                        break;
                    case "AV":
                        ato.Tipo = "AVERBAÇÃO";
                        break;
                    default:
                        ato.Tipo = "RGI";
                        break;
                }


                ato.Atribuicao = "RGI";

                switch (ato.Status)
                {
                    case "E":
                        ato.Status = "ENTRADA";
                        break;

                    case "B":
                        ato.Status = "BUSCAS";
                        break;

                    case "N":
                        ato.Status = "ANÁLISE";
                        break;

                    case "X":
                        ato.Status = "EXIGÊNCIAS";
                        break;

                    case "T":
                        ato.Status = "REENTRADA";
                        break;

                    case "C":
                        ato.Status = "CANCELADO";
                        break;

                    case "R":
                        ato.Status = "REGISTRANDO";
                        break;

                    case "D":
                        ato.Status = "CONCLUÍDO";
                        break;

                    case "A":
                        ato.Status = "ARQUIVADO";
                        break;

                    case "U":
                        ato.Status = "ENTREGUE";
                        break;

                    default:
                        ato.Status = "*";
                        break;

                }

                ato.Selo = drAto["SELO"].ToString();
                ato.Aleatorio = drAto["ALEATORIO"].ToString();

                if (drAto["DATA_REGISTRO"].ToString() != "")
                    ato.DataAto = Convert.ToDateTime(drAto["DATA_REGISTRO"]);



                switch (drAto["TIPO_COBRANCA"].ToString())
                {
                    case "CC":
                        ato.TipoCobranca = "COM COBRANÇA";
                        break;

                    case "JG":
                        ato.TipoCobranca = "JUSTIÇA GRATUITA";
                        break;

                    case "SC":
                        ato.TipoCobranca = "SEM COBRANÇA";
                        break;

                    case "FL":
                        ato.TipoCobranca = "FORÇA DA LEI";
                        break;

                    default:
                        ato.TipoCobranca = "NÃO INFORMADO";
                        break;
                }

                ato.Total = Convert.ToDecimal(drAto["TOTAL"]);

                atosPraticados.AtosRgi.Add(ato);
            }

            conAto.Close();
        }


        //^^^^^^^^^^^^^^^^^^^^^^^ Atos RGI ^^^^^^^^^^^^^^^^^^^^^^^^^^^^




    }
}
