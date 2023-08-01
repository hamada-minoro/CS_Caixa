using CS_Caixa.Agragador;
using CS_Caixa.Objetos_de_Valor;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Lógica interna para AguardeCarregandoGridQrCode.xaml
    /// </summary>
    public partial class AguardeCarregandoGridQrCode : Window
    {
        WinGerarQrCode _winGerarQrCode;

        BackgroundWorker worker;

        List<AtoPrincipalQrCode> atosQrCode = new List<AtoPrincipalQrCode>();

        List<AtoConjuntoQrCode> atosConjuntosQrCode = new List<AtoConjuntoQrCode>();

        List<NaturezaQrCode> naturezas = new List<NaturezaQrCode>();

        bool escritura, certidaoEscritura, procuracao, certidaoProcuracao, certidaoEletronica, testamento, certidaoTestamento;

        string dataInicio, dataFim, livro, NumAto;

        WinGerarQrCodeRgi _winGerarQrCodeRgi;

        public AguardeCarregandoGridQrCode(WinGerarQrCode winGerarQrCode)
        {
            _winGerarQrCode = winGerarQrCode;
            InitializeComponent();
        }


        public AguardeCarregandoGridQrCode(WinGerarQrCodeRgi winGerarQrCodeRgi)
        {
            _winGerarQrCodeRgi = winGerarQrCodeRgi;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_winGerarQrCode != null)
            {
                escritura = _winGerarQrCode.rbEscritura.IsChecked.Value;
                certidaoEscritura = _winGerarQrCode.rbCertidaoEscritura.IsChecked.Value;
                procuracao = _winGerarQrCode.rbProcuracao.IsChecked.Value;
                certidaoProcuracao = _winGerarQrCode.rbCertidaoProcuracao.IsChecked.Value;
                certidaoEletronica = _winGerarQrCode.rbCertidaoEletronica.IsChecked.Value;
                testamento = _winGerarQrCode.rbTestamento.IsChecked.Value;
                certidaoTestamento = _winGerarQrCode.rbCertidaoTestamento.IsChecked.Value;

                if (_winGerarQrCode.tipoConsulta == "livroAto")
                {
                    livro = _winGerarQrCode.txtLivro.Text;
                    NumAto = _winGerarQrCode.txtAto.Text;
                }
                else
                {
                    dataInicio = _winGerarQrCode.datePickerdataConsulta.SelectedDate.Value.ToShortDateString().Replace("/", ".");
                    dataFim = _winGerarQrCode.datePickerdataConsultaFim.SelectedDate.Value.ToShortDateString().Replace("/", ".");
                }
            }
            if (_winGerarQrCodeRgi != null)
            {
                dataInicio = _winGerarQrCodeRgi.datePickerdataConsulta.SelectedDate.Value.ToShortDateString().Replace("/", ".");
                dataFim = _winGerarQrCodeRgi.datePickerdataConsultaFim.SelectedDate.Value.ToShortDateString().Replace("/", ".");
            }

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_winGerarQrCode != null)
            {
                naturezas = ObterNaturezas();


                if (_winGerarQrCode.tipoConsulta == "livroAto")
                {
                    ConsultaLivroAto();
                }
                else
                {
                    ConsultaData();
                }
            }
            if (_winGerarQrCodeRgi != null)
            {
                ConsultaDataRgi();
            }


        }

        private List<NaturezaQrCode> ObterNaturezas()
        {

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
            {


                try
                {
                    string comando = "select CODIGO, DESCRICAO from NATUREZAS";

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;
                        conn.Open();

                        NaturezaQrCode naturaza;

                        FbDataReader dr;

                        dr = cmdTotal.ExecuteReader();

                        while (dr.Read())
                        {
                            naturaza = new NaturezaQrCode();

                            naturaza.NaturezaId = (int)dr["CODIGO"];
                            naturaza.Descricao = dr["DESCRICAO"].ToString();


                            naturezas.Add(naturaza);
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }





                return naturezas;
            }
        }


        private void ConsultaDataRgi()
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingRgi))
            {
                conn.Open();

                try
                {
                    string comando = "select * from ATO where (CODIGO_ADICIONAL = 5213 OR CODIGO_ADICIONAL = 5012) AND DATA_REGISTRO between '" + dataInicio + "' and '" + dataFim + "'";

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr;

                        dr = cmdTotal.ExecuteReader();

                        AtoPrincipalQrCode ato;

                        while (dr.Read())
                        {
                            ato = new AtoPrincipalQrCode();

                            ato.AtoId = (int)dr["ID_ATO"]; ;
                            ato.Data = (DateTime)dr["DATA_REGISTRO"];
                            ato.Aleatorio = dr["ALEATORIO"].ToString();

                            ato.Cerp = dr["ID_CERP"].ToString();

                            if (ato.Cerp != "")
                                ato.Natureza = "CERTIDÃO ELETRÔNICA - " + dr["DESC_NATUREZA"].ToString();
                            else
                                ato.Natureza = dr["DESC_NATUREZA"].ToString();

                            ato.Selo = dr["SELO"].ToString();
                            ato.Serventia = 1823;

                            if (dr["CODIGO_ADICIONAL"].ToString() == "5213")
                                ato.Tipo = "ICRI";
                            else
                                ato.Tipo = "ICG";

                            ato.Obs = dr["OBS"].ToString();
                            ato.Protocolo = dr["TALAO"].ToString();
                            ato.Livro = dr["LIVRO"].ToString();

                            ato.Matricula = dr["MATRICULA"].ToString();

                            if (dr["EMOL"] != null)
                            ato.Emol = Convert.ToDecimal(dr["EMOL"]);

                            if (dr["FETJ"] != null)
                            ato.Fetj = Convert.ToDecimal(dr["FETJ"]);

                            if (dr["FUND"] != null)
                            ato.Fund = Convert.ToDecimal(dr["FUND"]);

                            if (dr["FUNP"] != null)
                            ato.Funp = Convert.ToDecimal(dr["FUNP"]);

                            if (dr["FUNA"] != null)
                            ato.Funarpen = Convert.ToDecimal(dr["FUNA"]);

                            if (dr["PMCMV"] != null)
                            ato.Pmcmv = Convert.ToDecimal(dr["PMCMV"]);

                            if (dr["ISS"] != null)
                            ato.Iss = Convert.ToDecimal(dr["ISS"]);

                            if (dr["TOTAL"] != null)
                            ato.Total = Convert.ToDecimal(dr["TOTAL"]);

                            atosQrCode.Add(ato);
                        }
                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }





        private void ConsultaData()
        {
            if (escritura == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from ESCRITURAS where TP_ATO = 'RE' and DT_ATO_REG between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = Convert.ToInt32(dr["ID_ATO"]);
                                ato.Data = (DateTime)dr["DT_ATO_REG"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();

                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];

                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO_ESCR"].ToString();
                                ato.Natureza = ObterNatureza(dr["NATUREZA"].ToString());
                                ato.Selo = dr["SELO_ESCR"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NLE";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (certidaoEscritura == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from ESCRITURAS where TP_ATO = 'CE' and DT_ATO_REG between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO_REG"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();

                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO_ESCR"].ToString();
                                ato.Natureza = ObterNatureza(dr["NATUREZA"].ToString());
                                ato.Selo = dr["SELO_ESCR"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NCE";

                                atosQrCode.Add(ato);
                            }
                        }


                        string comando2 = "select * from NEGATIVAS where DATA between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal2 = new FbCommand(comando2, conn))
                        {
                            cmdTotal2.CommandType = CommandType.Text;

                            FbDataReader dr2;

                            dr2 = cmdTotal2.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr2.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr2["ID_NEGATIVA"];
                                ato.Data = (DateTime)dr2["DATA"];
                                ato.Aleatorio = dr2["ALEATORIO"].ToString();
                                ato.Natureza = "CERTIDÃO GENÉRICA";
                                ato.Selo = dr2["SELO"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NCG";

                                atosQrCode.Add(ato);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (procuracao == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from LINK_PROCURACAO where DT_LAVRATURA between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_LAVRATURA"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "PROCURAÇÃO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NLP";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (certidaoProcuracao == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from CERT_PROC where DATA between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            int ato;

                            AtoPrincipalQrCode Ato;

                            while (dr.Read())
                            {
                                ato = (int)dr["ID_REFERENCIA"];

                                Ato = new AtoPrincipalQrCode();

                                Ato = ObterProcuracaoPorIdAto(ato);



                                Ato.AtoId = (int)dr["ID_ATO"];
                                Ato.Data = (DateTime)dr["DATA"];
                                Ato.Aleatorio = dr["ALEATORIO"].ToString();
                                Ato.Natureza = "CERTIDÃO DE PROCURAÇÃO";
                                Ato.Selo = dr["SELO"].ToString();
                                Ato.Serventia = 1823;
                                Ato.Tipo = "NCP";

                                atosQrCode.Add(Ato);
                            }
                        }


                        string comando2 = "select * from NEGATIVAS where DATA between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal2 = new FbCommand(comando2, conn))
                        {
                            cmdTotal2.CommandType = CommandType.Text;

                            FbDataReader dr2;

                            dr2 = cmdTotal2.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr2.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr2["ID_NEGATIVA"];
                                ato.Data = (DateTime)dr2["DATA"];
                                ato.Aleatorio = dr2["ALEATORIO"].ToString();
                                ato.Natureza = "CERTIDÃO GENÉRICA";
                                ato.Selo = dr2["SELO"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NCG";

                                atosQrCode.Add(ato);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (certidaoEletronica == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from CERTIDAO where DT_CERTIDAO between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_CERTIDAO"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                
                                ato.Ato = Convert.ToInt32(dr["NUM_ATO"].ToString());
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "CERTIDÃO ELETRÔNICA DE " + dr["NATUREZA"].ToString();

                                if (ato.Natureza.Contains("PROCURAÇÃO"))
                                    ato.Tipo = "NCP";

                                if (ato.Natureza.Contains("ESCRITURA"))
                                    ato.Tipo = "NCE";

                                if (ato.Natureza.Contains("TESTAMENTO"))
                                    ato.Tipo = "NCTP";

                                ato.Cerp = dr["ID_CERP"].ToString();
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (testamento == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from TESTAMENTOS where TP_ATO = 'RT' and DT_ATO between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "TESTAMENTO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;

                                if (dr["TP_TEST"].ToString() == "P")
                                    ato.Tipo = "NRTP";
                                else
                                    ato.Tipo = "NRTC";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (certidaoTestamento == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from TESTAMENTOS where TP_ATO = 'CT' and DT_ATO between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "CERTIDÃO DE TESTAMENTO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;

                                if (dr["TP_TEST"].ToString() == "P")
                                    ato.Tipo = "NRTP";
                                else
                                    ato.Tipo = "NRTC";

                                atosQrCode.Add(ato);
                            }
                        }

                        string comando2 = "select * from NEGATIVAS where DATA between '" + dataInicio + "' and '" + dataFim + "'";

                        using (FbCommand cmdTotal2 = new FbCommand(comando2, conn))
                        {
                            cmdTotal2.CommandType = CommandType.Text;

                            FbDataReader dr2;

                            dr2 = cmdTotal2.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr2.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr2["ID_NEGATIVA"];
                                ato.Data = (DateTime)dr2["DATA"];
                                ato.Aleatorio = dr2["ALEATORIO"].ToString();
                                ato.Natureza = "CERTIDÃO GENÉRICA";
                                ato.Selo = dr2["SELO"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NCG";

                                atosQrCode.Add(ato);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }



        private void ConsultaLivroAto()
        {
            if (escritura == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from ESCRITURAS where TP_ATO = 'RE' and LIVRO_ESCR = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO_REG"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO_ESCR"].ToString();
                                ato.Natureza = ObterNatureza(dr["NATUREZA"].ToString());
                                ato.Selo = dr["SELO_ESCR"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NLE";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }



            }
            else if (certidaoEscritura == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from ESCRITURAS where TP_ATO = 'CE' and LIVRO_ESCR = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO_REG"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO_ESCR"].ToString();
                                ato.Natureza = ObterNatureza(dr["NATUREZA"].ToString());
                                ato.Selo = dr["SELO_ESCR"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NCE";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (procuracao == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from LINK_PROCURACAO where LIVRO = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_LAVRATURA"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "PROCURAÇÃO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NLE";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (certidaoProcuracao == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from LINK_PROCURACAO where LIVRO = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;


                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_LAVRATURA"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "PROCURAÇÃO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;
                                ato.Tipo = "NCP";

                                ObterCertidaoProcuracao(ato);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }


            }
            else if (certidaoEletronica == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {
                        string comando = "select * from CERTIDAO where LIVRO = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_CERTIDAO"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                ato.Ato = Convert.ToInt32(dr["NUM_ATO"].ToString());
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "CERTIDÃO ELETRÔNICA DE " + dr["NATUREZA"].ToString();
                                if (ato.Natureza.Contains("PROCURAÇÃO"))
                                    ato.Tipo = "NCP";

                                if (ato.Natureza.Contains("ESCRITURA"))
                                    ato.Tipo = "NCE";

                                if (ato.Natureza.Contains("TESTAMENTO"))
                                    ato.Tipo = "NCTP";


                                ato.Cerp = dr["ID_CERP"].ToString();
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (testamento == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from TESTAMENTOS where TP_ATO = 'RT' and LIVRO = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "TESTAMENTO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;

                                if (dr["TP_TEST"].ToString() == "P")
                                    ato.Tipo = "NRTP";
                                else
                                    ato.Tipo = "NRTC";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            else if (certidaoTestamento == true)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
                {
                    conn.Open();

                    try
                    {


                        string comando = "select * from TESTAMENTOS where TP_ATO = 'CT' and LIVRO = '" + livro + "' and NUM_ATO = " + NumAto;

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr;

                            dr = cmdTotal.ExecuteReader();

                            AtoPrincipalQrCode ato;

                            while (dr.Read())
                            {
                                ato = new AtoPrincipalQrCode();

                                ato.AtoId = (int)dr["ID_ATO"];
                                ato.Data = (DateTime)dr["DT_ATO"];
                                ato.Aleatorio = dr["ALEATORIO"].ToString();
                                if (dr["NUM_ATO"].ToString() != "")
                                ato.Ato = (int)dr["NUM_ATO"];
                                ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                                ato.FolhasFim = dr["FL_FIM"].ToString();
                                ato.FolhasInicio = dr["FL_INI"].ToString();
                                ato.Livro = dr["LIVRO"].ToString();
                                ato.Natureza = "CERTIDÃO DE TESTAMENTO";
                                ato.Selo = dr["SELO"].ToString();
                                ato.Serventia = 1823;

                                if (dr["TP_TEST"].ToString() == "P")
                                    ato.Tipo = "NRTP";
                                else
                                    ato.Tipo = "NRTC";

                                atosQrCode.Add(ato);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private AtoPrincipalQrCode ObterProcuracaoPorIdAto(int IdAto)
        {
            AtoPrincipalQrCode ato = new AtoPrincipalQrCode();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
            {
                conn.Open();


                string comando = "select * from LINK_PROCURACAO where ID_ATO = " + IdAto;

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();

                    while (dr.Read())
                    {
                        ato = new AtoPrincipalQrCode();

                        ato.AtoId = (int)dr["ID_ATO"];
                        ato.Data = (DateTime)dr["DT_LAVRATURA"];
                        ato.Aleatorio = dr["ALEATORIO"].ToString();
                        ato.Ato = (int)dr["NUM_ATO"];
                        ato.AtosConjuntos = ObterAtosConjuntos(ato.AtoId);
                        ato.FolhasFim = dr["FL_FIM"].ToString();
                        ato.FolhasInicio = dr["FL_INI"].ToString();
                        ato.Livro = dr["LIVRO"].ToString();
                        ato.Natureza = "PROCURAÇÃO";
                        ato.Selo = dr["SELO"].ToString();
                        ato.Serventia = 1823;
                        ato.Tipo = "NCP";

                    }

                }


            }

            return ato;
        }

        private void ObterCertidaoProcuracao(AtoPrincipalQrCode Ato)
        {
            AtoPrincipalQrCode ato;

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
            {
                conn.Open();


                string comando = "select * from CERT_PROC where ID_REFERENCIA = " + Ato.AtoId;

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();



                    while (dr.Read())
                    {
                        ato = new AtoPrincipalQrCode();

                        ato.AtoId = (int)dr["ID_ATO"];
                        ato.Data = (DateTime)dr["DATA"];
                        ato.Aleatorio = dr["ALEATORIO"].ToString();
                        ato.Ato = Ato.Ato;
                        ato.FolhasFim = Ato.FolhasFim;
                        ato.FolhasInicio = Ato.FolhasInicio;
                        ato.Livro = Ato.Livro;
                        ato.Natureza = "CERTIDÃO DE PROCURAÇÃO";
                        ato.Selo = dr["SELO"].ToString();
                        ato.Serventia = 1823;
                        ato.Tipo = "NCP";

                        atosQrCode.Add(ato);
                    }

                }


            }

        }

        private string ObterNatureza(string codigo)
        {
            return naturezas.Where(p => p.NaturezaId == Convert.ToInt32(codigo)).FirstOrDefault().Descricao;
        }

        private List<AtoConjuntoQrCode> ObterAtosConjuntos(int idAto)
        {

            List<AtoConjuntoQrCode> atosConjuntos = new List<AtoConjuntoQrCode>();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingNotas))
            {
                conn.Open();

                try
                {
                    string comando = "select * from CONJUNTOS where ID_ATO = " + idAto;

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr;

                        dr = cmdTotal.ExecuteReader();

                        AtoConjuntoQrCode ato;

                        while (dr.Read())
                        {
                            ato = new AtoConjuntoQrCode();
                            ato.AtoConjuntoId = (int)dr["ID_CONJ"];
                            ato.IdAto = (int)dr["ID_ATO"];
                            ato.Aleatorio = dr["ALEATORIO"].ToString();
                            ato.Selo = dr["SELO"].ToString();

                            atosConjuntos.Add(ato);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return atosConjuntos;
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (_winGerarQrCode != null)
                _winGerarQrCode.atoPrincipalQrCode = atosQrCode;

            if (_winGerarQrCodeRgi != null)
                _winGerarQrCodeRgi.atoPrincipalQrCode = atosQrCode;

            this.Close();
        }

       

    }
}
