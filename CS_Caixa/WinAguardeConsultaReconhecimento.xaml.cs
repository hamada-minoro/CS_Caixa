using CS_Caixa.Objetos_de_Valor;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Lógica interna para WinAguardeConsultaReconhecimento.xaml
    /// </summary>
    public partial class WinAguardeConsultaReconhecimento : Window
    {

        BackgroundWorker worker;
        WinConsultaReconhecimento _winConsulta;
        DateTime dataInicio;
        DateTime dataFim;
        List<int> idsFichas = new List<int>();
        string _nomeCpf;
        public WinAguardeConsultaReconhecimento(WinConsultaReconhecimento winConsulta, string nomeCpf)
        {

            _winConsulta = winConsulta;
            _nomeCpf = nomeCpf;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            lblContagem.Content = "Processando Dados Solicitados";
            dataInicio = _winConsulta.datePickerdataConsulta.SelectedDate.Value;
            dataFim = _winConsulta.datePickerdataConsultaFim.SelectedDate.Value;
           
            _winConsulta.qtdAbertura.Content = string.Format("Abertura de Firmas: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "ABR" && p.STATUS == "XML").Count());
            _winConsulta.qtdAutenticacao.Content = string.Format("Autenticação: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "AUT" && p.STATUS == "XML").Count());
            _winConsulta.qtdAutenticidade.Content = string.Format("Reconhecimento por Autenticidade: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "RFA" && p.STATUS == "XML").Count());
            _winConsulta.qtdSemelhanca.Content = string.Format("Reconhecimento por Semelhança: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "RFS" && p.STATUS == "XML").Count());
            _winConsulta.qtdMaterializacao.Content = string.Format("Materialização de Documentos: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "MAT" && p.STATUS == "XML").Count());
            _winConsulta.qtdTotalAtos.Content = string.Format("Total de Atos: {0}", _winConsulta.atosFirmas.Where(p => p.STATUS == "XML").Count());
            _winConsulta.qtdSinal.Content = string.Format("Sinal Público: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "SIN" && p.STATUS == "XML").Count());

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

            
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            _winConsulta.qtdAbertura.Content = string.Format("Abertura de Firmas: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "ABR" && p.STATUS == "XML").Count());
            _winConsulta.qtdAutenticacao.Content = string.Format("Autenticação: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "AUT" && p.STATUS == "XML").Count());
            _winConsulta.qtdAutenticidade.Content = string.Format("Reconhecimento por Autenticidade: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "RFA" && p.STATUS == "XML").Count());
            _winConsulta.qtdSemelhanca.Content = string.Format("Reconhecimento por Semelhança: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "RFS" && p.STATUS == "XML").Count());           
            _winConsulta.qtdMaterializacao.Content = string.Format("Materialização de Documentos: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "MAT" && p.STATUS == "XML").Count());
            _winConsulta.qtdTotalAtos.Content = string.Format("Total de Atos: {0}", _winConsulta.atosFirmas.Where(p => p.STATUS == "XML").Count());
            _winConsulta.qtdSinal.Content = string.Format("Sinal Público: {0}", _winConsulta.atosFirmas.Where(p => p.TIPO == "SIN" && p.STATUS == "XML").Count());
            this.Close();

        }

        private void Processo()
        {
            try
            {

                if (_winConsulta.tipoConsulta == "Data")
                    ConsultaPorPeriodoTabelaNova();
                else
                {
                    ConsultaPorNomeCpf();

                    if (idsFichas.Count > 0)
                        ConsultaPorIdFicha();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }



        private void ConsultaPorPeriodoTabelaNova()
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {
                AtosFirmas atosFirmas;
                try
                {

                    string dataIni = string.Format("{0:0000}.{1:00}.{2:00}", dataInicio.Year, dataInicio.Month, dataInicio.Day);

                    string dataFinal = string.Format("{0:0000}.{1:00}.{2:00}", dataFim.Year, dataFim.Month, dataFim.Day);

                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from atos where Data between '{0}' and '{1}' ", dataIni, dataFinal);

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr = cmdTotal.ExecuteReader();
                        while (dr.Read())
                        {

                            atosFirmas = new AtosFirmas();

                            atosFirmas.ALEATORIO = dr["ALEATORIO"].ToString();
                            
                            if (dr["ANO"].ToString() != "")
                            atosFirmas.ANO = Convert.ToInt32(dr["ANO"]);

                            atosFirmas.COBRANCA = dr["COBRANCA"].ToString();

                            if (dr["ID_ATO"].ToString() != "")
                            atosFirmas.ID_ATO = Convert.ToInt32(dr["ID_ATO"]);

                            if (dr["CODIGO"].ToString() != "")
                            atosFirmas.CODIGO = Convert.ToInt32(dr["CODIGO"]);

                            atosFirmas.DATA = dr["DATA"].ToString();

                            atosFirmas.DESCRICAO = dr["DESCRICAO"].ToString();

                            if (dr["EMOLUMENTOS"].ToString() != "")
                            atosFirmas.EMOLUMENTOS = Convert.ToDecimal(dr["EMOLUMENTOS"]);

                            if (dr["FETJ"].ToString() != "")
                            atosFirmas.FETJ = Convert.ToDecimal(dr["FETJ"]);

                            if (dr["FICHA"].ToString() != "")
                            atosFirmas.FICHA = Convert.ToInt32(dr["FICHA"]);

                            if (dr["FUNARPEN"].ToString() != "")
                            atosFirmas.FUNARPEN = Convert.ToDecimal(dr["FUNARPEN"]);

                            if (dr["FUNDPERJ"].ToString() != "")
                            atosFirmas.FUNDPERJ = Convert.ToDecimal(dr["FUNDPERJ"]);

                            if (dr["FUNPERJ"].ToString() != "")
                            atosFirmas.FUNPERJ = Convert.ToDecimal(dr["FUNPERJ"]);

                            atosFirmas.HORA = dr["HORA"].ToString();

                            if (dr["ID_FICHA"].ToString() != "")
                            atosFirmas.ID_FICHA = Convert.ToInt32(dr["ID_FICHA"]);

                            if (dr["ISS"].ToString() != "")
                            atosFirmas.ISS = Convert.ToDecimal(dr["ISS"]);

                            atosFirmas.LIVRO = dr["LIVRO"].ToString();

                            atosFirmas.LOGADO = dr["LOGADO"].ToString();

                            if (dr["PMCMV"].ToString() != "")
                            atosFirmas.PMCMV = Convert.ToDecimal(dr["PMCMV"]);

                            if (dr["RECIBO"].ToString() != "")
                                atosFirmas.RECIBO = Convert.ToInt32(dr["RECIBO"]);


                            atosFirmas.SELO = dr["SELO"].ToString();

                            atosFirmas.STATUS = dr["STATUS"].ToString();

                            atosFirmas.TERMO = dr["TERMO"].ToString();

                            atosFirmas.TIPO = dr["TIPO"].ToString();

                            if (dr["TOTAL"].ToString() != "")
                            atosFirmas.TOTAL = Convert.ToDecimal(dr["TOTAL"]);

                            if (dr["TIPO_DOCUMENTO"].ToString() != "")
                                atosFirmas.TIPO_DOCUMENTO = Convert.ToInt32(dr["TIPO_DOCUMENTO"]);

                            _winConsulta.atosFirmas.Add(atosFirmas);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }




        private void ConsultaPorIdFicha()
        {

            foreach (var item in idsFichas)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
                {
                    AtosFirmas atosFirmas;
                    try
                    {                        
                        string comando = string.Empty;
                        conn.Open();

                        comando = string.Format("select * from atos where ID_FICHA = '{0}'", item);

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr = cmdTotal.ExecuteReader();
                            while (dr.Read())
                            {

                                atosFirmas = new AtosFirmas();

                                atosFirmas.ALEATORIO = dr["ALEATORIO"].ToString();

                                if (dr["ANO"].ToString() != "")
                                    atosFirmas.ANO = Convert.ToInt32(dr["ANO"]);

                                atosFirmas.COBRANCA = dr["COBRANCA"].ToString();

                                if (dr["ID_ATO"].ToString() != "")
                                    atosFirmas.ID_ATO = Convert.ToInt32(dr["ID_ATO"]);

                                if (dr["CODIGO"].ToString() != "")
                                    atosFirmas.CODIGO = Convert.ToInt32(dr["CODIGO"]);

                                atosFirmas.DATA = dr["DATA"].ToString();

                                atosFirmas.DESCRICAO = dr["DESCRICAO"].ToString();

                                if (dr["EMOLUMENTOS"].ToString() != "")
                                    atosFirmas.EMOLUMENTOS = Convert.ToDecimal(dr["EMOLUMENTOS"]);

                                if (dr["FETJ"].ToString() != "")
                                    atosFirmas.FETJ = Convert.ToDecimal(dr["FETJ"]);

                                if (dr["FICHA"].ToString() != "")
                                    atosFirmas.FICHA = Convert.ToInt32(dr["FICHA"]);

                                if (dr["FUNARPEN"].ToString() != "")
                                    atosFirmas.FUNARPEN = Convert.ToDecimal(dr["FUNARPEN"]);

                                if (dr["FUNDPERJ"].ToString() != "")
                                    atosFirmas.FUNDPERJ = Convert.ToDecimal(dr["FUNDPERJ"]);

                                if (dr["FUNPERJ"].ToString() != "")
                                    atosFirmas.FUNPERJ = Convert.ToDecimal(dr["FUNPERJ"]);

                                atosFirmas.HORA = dr["HORA"].ToString();

                                if (dr["ID_FICHA"].ToString() != "")
                                    atosFirmas.ID_FICHA = Convert.ToInt32(dr["ID_FICHA"]);

                                if (dr["ISS"].ToString() != "")
                                    atosFirmas.ISS = Convert.ToDecimal(dr["ISS"]);

                                atosFirmas.LIVRO = dr["LIVRO"].ToString();

                                atosFirmas.LOGADO = dr["LOGADO"].ToString();

                                if (dr["PMCMV"].ToString() != "")
                                    atosFirmas.PMCMV = Convert.ToDecimal(dr["PMCMV"]);

                                if (dr["RECIBO"].ToString() != "")
                                    atosFirmas.RECIBO = Convert.ToInt32(dr["RECIBO"]);


                                atosFirmas.SELO = dr["SELO"].ToString();

                                atosFirmas.STATUS = dr["STATUS"].ToString();

                                atosFirmas.TERMO = dr["TERMO"].ToString();

                                atosFirmas.TIPO = dr["TIPO"].ToString();

                                if (dr["TOTAL"].ToString() != "")
                                    atosFirmas.TOTAL = Convert.ToDecimal(dr["TOTAL"]);

                                if (dr["TIPO_DOCUMENTO"].ToString() != "")
                                    atosFirmas.TIPO_DOCUMENTO = Convert.ToInt32(dr["TIPO_DOCUMENTO"]);

                                _winConsulta.atosFirmas.Add(atosFirmas);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }




            foreach (var item in idsFichas)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
                {
                    AtosFirmas atosFirmas;
                    try
                    {
                        string comando = string.Empty;
                        conn.Open();

                        comando = string.Format("select * from TREC where ID_FICHA = '{0}'", item);

                        using (FbCommand cmdTotal = new FbCommand(comando, conn))
                        {
                            cmdTotal.CommandType = CommandType.Text;

                            FbDataReader dr = cmdTotal.ExecuteReader();
                            while (dr.Read())
                            {

                                atosFirmas = new AtosFirmas();

                                atosFirmas.ALEATORIO = dr["ALEATORIO"].ToString();

                                
                                atosFirmas.COBRANCA = dr["TIPO_COBRANCA"].ToString();

                                if (dr["ID_ATO"].ToString() != "")
                                    atosFirmas.ID_ATO = Convert.ToInt32(dr["ID_ATO"]);


                                atosFirmas.DATA = dr["DATA"].ToString();

                                atosFirmas.DESCRICAO = dr["NOME"].ToString();                                                                

                                
                                atosFirmas.LOGADO = dr["LOGADO"].ToString();
                               
                                atosFirmas.SELO = dr["SELO"].ToString();
                                
                                atosFirmas.TERMO = dr["TERMO"].ToString();


                                if (dr["TIPOATO"].ToString() == "RFA")
                                    atosFirmas.TIPO = "ABR";

                                if (dr["TIPOATO"].ToString() == "AUT")
                                    atosFirmas.TIPO = "AUT";

                                if (dr["TIPOATO"].ToString() == "RFR")
                                {
                                    if (dr["TIPO_REC"].ToString() == "P")
                                        atosFirmas.TIPO = "SIN";

                                    if (dr["TIPO_REC"].ToString() == "S")
                                        atosFirmas.TIPO = "RFS";

                                    if (dr["TIPO_REC"].ToString() == "A")
                                        atosFirmas.TIPO = "RFA";

                                }

                                atosFirmas.STATUS = "XML";

                                _winConsulta.atosFirmas.Add(atosFirmas);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }



        private void ConsultaPorNomeCpf()
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {
                
                try
                { 
                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from fichas where nome = '{0}' or cpf = '{1}'", _nomeCpf, _nomeCpf);

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr = cmdTotal.ExecuteReader();
                        while (dr.Read())
                        {
                            idsFichas.Add(Convert.ToInt32(dr["ID"]));
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}