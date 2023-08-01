using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirebirdSql.Data.FirebirdClient;
using CS_Caixa.Controls;
using CS_Caixa.Models;
using System.ComponentModel;
using System.Threading;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAguardeBalcao.xaml
    /// </summary>
    public partial class WinAguardeBalcao : Window
    {

        BackgroundWorker worker;

        WinBalcaoNovo Balcao;
        WinBalcao BalcaoVelho;
        bool erroAut = false;
        bool erroAbert = false;
        bool erroRecAut = false;
        bool erroRecAutDut = false;
        bool erroRecSem = false;
        bool erroMaterializacao = false;
        Ato atoCorrente;
        string carregaAtual;
        string letraSelo;
        int seloInicio;

        DateTime datePickerData;
        int qtdAut;
        int qtdAbert;
        int qtdRecSem;
        int qtdRecAut;
        int qtdMat;
        int qtdRecAutDut;
        string txtSeloInicialAut;
        string txtLetraSeloAbert;
        string txtLetraSeloAut;
        string data;
        string txtSeloInicialAbert;
        string txtLetraSeloRecAut;
        string txtSeloInicialRecAut;
        string txtSeloInicialRecSem;
        string txtLetraSeloRecSem;
        string txtSeloInicialMaterializacao;
        string txtLetraSeloMaterializacao;
        string txtLetraSeloRecAutDut;
        string txtSeloInicialRecAutDut;

        string cmbTipoPagamento;
        bool checkBoxPago;
        decimal txtAdicionar;
        decimal txtDesconto;
        int cmbTipoPagamentoIndice;
        int cmbMensalistaIndice;
        string cmbMensalista;
        string cmbFuncionario;
        string cmbTipoCustas;
        int cmbTipoCustasIndice;
        string txtRequisicao;
        List<Ato> listaSelos = new List<Ato>();
        Usuario usuario;
        string descricaoAto = string.Empty;
        string fichaAto = string.Empty;

        ClassBalcao classBalcao = new ClassBalcao();

        public WinAguardeBalcao(WinBalcaoNovo Balcao)
        {
            this.Balcao = Balcao;
            datePickerData = Balcao.datePickerData.SelectedDate.Value;
            qtdAut = Convert.ToInt16(Balcao.txtQtdAut.Text);
            qtdAbert = Convert.ToInt16(Balcao.txtQtdAbert.Text);
            qtdRecSem = Convert.ToInt16(Balcao.txtQtdRecSem.Text);
            qtdRecAut = Convert.ToInt16(Balcao.txtQtdRecAut.Text);
            qtdMat = Convert.ToInt16(Balcao.txtQtdMaterializacao.Text);
            qtdRecAutDut = Convert.ToInt16(Balcao.txtQtdRecAutDut.Text);



            txtSeloInicialAut = Balcao.txtSeloInicialAut.Text;
            txtLetraSeloAut = Balcao.txtLetraSeloAut.Text;
            txtLetraSeloAbert = Balcao.txtLetraSeloAbert.Text;
            txtSeloInicialAbert = Balcao.txtSeloInicialAbert.Text;
            txtLetraSeloRecAut = Balcao.txtLetraSeloRecAut.Text;
            txtSeloInicialRecAut = Balcao.txtSeloInicialRecAut.Text;
            txtSeloInicialRecSem = Balcao.txtSeloInicialRecSem.Text;
            txtLetraSeloRecSem = Balcao.txtLetraSeloRecSem.Text;
            txtSeloInicialMaterializacao = Balcao.txtSeloInicialMaterializacao.Text;
            txtLetraSeloMaterializacao = Balcao.txtLetraSeloMaterializacao.Text;
            txtLetraSeloRecAutDut = Balcao.txtLetraSeloRecAutDut.Text;
            txtSeloInicialRecAutDut = Balcao.txtSeloInicialRecAutDut.Text;



            cmbTipoPagamentoIndice = Balcao.cmbTipoPagamento.SelectedIndex;
            cmbTipoPagamento = Balcao.cmbTipoPagamento.Text;
            if (Balcao.checkBoxPago.IsChecked == true)
                checkBoxPago = true;
            else
                checkBoxPago = false;

            txtAdicionar = Convert.ToDecimal(Balcao.txtAdicionar.Text);
            txtDesconto = Convert.ToDecimal(Balcao.txtDesconto.Text);
            cmbMensalistaIndice = Balcao.cmbMensalista.SelectedIndex;
            cmbMensalista = Balcao.cmbMensalista.Text;
            cmbFuncionario = Balcao.cmbFuncionario.Text;

            usuario = (Usuario)Balcao.cmbFuncionario.SelectedItem;

            cmbTipoCustas = Balcao.cmbTipoCustas.Text;
            txtRequisicao = Balcao.txtRequisicao.Text;
            cmbTipoCustasIndice = Balcao.cmbTipoCustas.SelectedIndex;
            listaSelos = Balcao.listaSelos;
            InitializeComponent();
        }

        public WinAguardeBalcao(WinBalcao Balcao)
        {
            this.BalcaoVelho = Balcao;

            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            data = string.Format("{0:00}.{1:00}.{2}", datePickerData.Day, datePickerData.Month, datePickerData.Year);

            if (Balcao.automatico == true)
                progressBar1.Visibility = Visibility.Hidden;

            

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



        public void VerificarSelosNaoLancadosAutomaticos()
        {

           
            var classAto = new ClassAto();

            List<int> ListIdServico = new List<int>();

            int idServico = 0;

            int nSelo = 0;

            List<Ato> selosCaixa = classAto.ListarAtoDataAto(datePickerData, datePickerData).Where(p => p.Atribuicao == "BALCÃO").ToList();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {

                string data = datePickerData.ToShortDateString().Replace("/", ".");

                

                string comando = string.Format("select * from atos where data = '{0}' and logado = '{1}' and status = 'XML'", data, cmbFuncionario);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();

                    while (dr.Read())
                    {

                        string selo = dr["SELO"].ToString();

                        string letraSelo = selo.Substring(0, 4);

                        int numeroSelo = Convert.ToInt32(selo.Substring(4, 5));

                        nSelo = numeroSelo;

                        string descricaoAto = dr["DESCRICAO"].ToString();

                        string fichaAto = dr["FICHA"].ToString();

                        string atual = "";

                        

                        if (dr["TIPO"].ToString() == "RFA" && dr["RTD_TOTAL"].ToString() == "0")
                        {
                            atual = "rec autenticidade";
                        }
                        if (dr["TIPO"].ToString() == "RFS")
                        {
                            atual = "REC SEMELHANÇA";
                        }

                        if (dr["TIPO"].ToString() == "SIN")
                        {
                            atual = "SINAL PÚBLICO";
                        }

                        if (dr["TIPO"].ToString() == "ABR")
                        {
                            atual = "abertura";
                        }
                        if (dr["TIPO"].ToString() == "AUT")
                        {
                            atual = "autenticação";
                        }

                        if (dr["TIPO"].ToString() == "MAT")
                        {
                            atual = "materializacao";
                        }

                        if (dr["RTD_TOTAL"].ToString() != "0")
                        {
                            atual = "dut";
                        }

                        
                            if (selosCaixa.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == numeroSelo).Count() == 0 && listaSelos.Where(p => p.LetraSelo == selo.Substring(0, 4) && p.NumeroSelo == Convert.ToInt32(selo.Substring(4, 5))).Count() == 0)
                            {
                                idServico = Convert.ToInt32(dr["ID_SERVICO"]);
                                SalvaSelo(letraSelo, numeroSelo, atual, descricaoAto, fichaAto);
                            }

                            if (!ListIdServico.Contains(idServico) && idServico > 0)
                                ListIdServico.Add(idServico);
                        
                    }

                }

                

                classBalcao.SalvarUltimoSelo(nSelo);

                if (ListIdServico.Count > 0)
                qtdCopiasAutomatico(ListIdServico);
            }
        }

        private void qtdCopiasAutomatico(List<int> idServico)
        {

            for (int i = 0; i < idServico.Count(); i++)
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
                {

                    string data = datePickerData.ToShortDateString().Replace("/", ".");

                    string comando = string.Format("select QCOP from servicos where id_servico = {0}", idServico[i]);
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        Balcao.qtdCopias = Balcao.qtdCopias + Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }
                }
            }

            
        }


        private void SalvaSelo(string letraSelo, int numeroSelo, string atual, string descricaoAto, string fichaAto)
        {
            var atoCorrente = new Ato();

            decimal arquivamento = 0;

            decimal itemAbertura = 0;

            decimal valorDut = 0;


            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Siss = "0,00";
            string Spmcmv_2 = "0,00";
            int index;

            string SemolArquiv = "0,00";
            string Sfetj_20Arquiv = "0,00";
            string Sfundperj_5Arquiv = "0,00";
            string Sfunperj_5Arquiv = "0,00";
            string Sfunarpen_4Arquiv = "0,00";
            string SissArquiv = "0,00";



            try
            {
                // data do pagamento
                if (datePickerData != null)
                {
                    atoCorrente.DataPagamento = datePickerData;
                }


                // tipo de pagamento
                atoCorrente.TipoPagamento = cmbTipoPagamento;


                // data do ato
                if (datePickerData != null)
                {
                    atoCorrente.DataAto = datePickerData;
                }



                // pago
                if (checkBoxPago == true)
                    atoCorrente.Pago = true;
                else
                    atoCorrente.Pago = false;



                // IdUsuario
                atoCorrente.IdUsuario = usuario.Id_Usuario;



                // Usuario
                atoCorrente.Usuario = usuario.NomeUsu;


                // Atribuiçao
                atoCorrente.Atribuicao = "BALCÃO";


                // Letra Selo

                atoCorrente.LetraSelo = letraSelo;

                // Numero Selo

                atoCorrente.NumeroSelo = numeroSelo;

                // valor adicionar
                atoCorrente.ValorAdicionar = Convert.ToDecimal(txtAdicionar);


                // Valor Desconto
                atoCorrente.ValorDesconto = Convert.ToDecimal(txtDesconto);


                atoCorrente.DescricaoAto = descricaoAto;

                atoCorrente.FichaAto = fichaAto;


                // Mensalista
                if (cmbTipoPagamentoIndice == 2)
                {
                    if (cmbMensalistaIndice >= 0)
                    {
                        atoCorrente.Mensalista = cmbMensalista;
                    }

                }

                if (atual == "autenticação")
                {

                    // TipoAto
                    atoCorrente.TipoAto = "AUTENTICAÇÃO";

                    atoCorrente.Natureza = "AUTENTICAÇÃO";

                    //Emolumentos
                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "AUTENTICAÇÃO POR DOCUMENTO OU PÁGINA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;


                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;


                }


                if (atual == "abertura")
                {

                    // TipoAto
                    atoCorrente.TipoAto = "ABERTURA DE FIRMAS";



                    atoCorrente.Natureza = "ABERTURA DE FIRMAS";


                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "ABERTURA DE FIRMA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").Select(p => p.VALOR).FirstOrDefault());

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;




                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura + arquivamento;
                        fetj_20 = emol * 20 / 100;
                        fundperj_5 = emol * 5 / 100;
                        funperj_5 = emol * 5 / 100;
                        funarpen_4 = emol * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;



                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);


                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    emol = Convert.ToDecimal(Semol);
                    fetj_20 = Convert.ToDecimal(Sfetj_20);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;
                }

                if (atual == "rec autenticidade")
                {


                    // TipoAto
                    atoCorrente.TipoAto = "REC AUTENTICIDADE";



                    atoCorrente.Natureza = "REC AUTENTICIDADE";

                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;





                }
                if (atual == "dut")
                {
                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES").Select(p => p.VALOR).FirstOrDefault());

                    valorDut = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE (DUT)").Select(p => p.VALOR).FirstOrDefault());




                    // TipoAto
                    atoCorrente.TipoAto = "REC AUTENTICIDADE (DUT)";

                    atoCorrente.Natureza = "REC AUTENTICIDADE (DUT)";


                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;


                }

                if (atual == "REC SEMELHANÇA" || atual == "SINAL PÚBLICO")
                {

                    // TipoAto
                    atoCorrente.TipoAto = atual;

                    atoCorrente.Natureza = atual;

                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR SEMELHANÇA OU CHANCELA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;

                }

                if (atual == "materializacao")
                {

                    // TipoAto
                    atoCorrente.TipoAto = "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS";

                    atoCorrente.Natureza = "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS";

                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;
                }

                // Escrevente
                atoCorrente.Escrevente = cmbFuncionario;

                // TipoCobranca
                atoCorrente.TipoCobranca = cmbTipoCustas;


                //NumeroRequisicao
                if (cmbTipoPagamentoIndice == 2)
                {
                    if (txtRequisicao != "")
                    {
                        atoCorrente.NumeroRequisicao = Convert.ToInt32(txtRequisicao);
                    }
                }

                // Total
                atoCorrente.Total = atoCorrente.Emolumentos + atoCorrente.Fetj + atoCorrente.Fundperj + atoCorrente.Funperj + atoCorrente.Funarpen + atoCorrente.Iss + atoCorrente.Pmcmv + valorDut;

                listaSelos.Add(atoCorrente);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }







        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;


            if (carregaAtual == "autenticação")
            {
                progressBar1.Maximum = qtdAut;
                label2.Content = "Autenticação.";
            }

            if (carregaAtual == "abertura")
            {
                progressBar1.Maximum = qtdAbert;
                label2.Content = "Abertura de Firmas.";
            }

            if (carregaAtual == "rec autenticidade")
            {
                progressBar1.Maximum = qtdRecAut;
                label2.Content = "Reconhecimento por Autenticidade.";
            }

            if (carregaAtual == "rec semelhança")
            {
                progressBar1.Maximum = qtdRecSem;
                label2.Content = "Reconhecimento por Semelhança.";
            }

            if (carregaAtual == "materializacao")
            {
                progressBar1.Maximum = qtdMat;
                label2.Content = "Materialização de Documentos.";
            }

            if (carregaAtual == "dut")
            {
                progressBar1.Maximum = qtdMat;
                label2.Content = "Reconhecimento por Autenticidade (DUT).";
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void Processo()
        {
            try
            {
                if (Balcao.automatico == true)
                {
                    VerificarSelosNaoLancadosAutomaticos();
                }
                else
                {
                    if (qtdAut > 0)
                    {
                        if (txtSeloInicialAut.Length == 5)
                        {

                            letraSelo = txtLetraSeloAut;
                            seloInicio = Convert.ToInt32(txtSeloInicialAut);

                            carregaAtual = "autenticação";

                            for (int i = 0; i < qtdAut; i++)
                            {

                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                CarregaAto(letraSelo, seloInicio, carregaAtual);

                                seloInicio++;

                            }
                        }
                    }


                    if (qtdAbert > 0)
                    {
                        if (txtSeloInicialAbert.Length == 5)
                        {

                            letraSelo = txtLetraSeloAbert;
                            seloInicio = Convert.ToInt32(txtSeloInicialAbert);

                            carregaAtual = "abertura";

                            for (int i = 0; i < qtdAbert; i++)
                            {

                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                CarregaAto(letraSelo, seloInicio, carregaAtual);

                                seloInicio++;

                            }
                        }
                    }


                    if (qtdRecAut > 0)
                    {
                        if (txtSeloInicialRecAut.Length == 5)
                        {
                            letraSelo = txtLetraSeloRecAut;
                            seloInicio = Convert.ToInt32(txtSeloInicialRecAut);

                            carregaAtual = "rec autenticidade";

                            for (int i = 0; i < qtdRecAut; i++)
                            {

                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                CarregaAto(letraSelo, seloInicio, carregaAtual);

                                seloInicio++;

                            }
                        }
                    }


                    if (qtdRecAutDut > 0)
                    {
                        if (txtSeloInicialRecAutDut.Length == 5)
                        {
                            letraSelo = txtLetraSeloRecAutDut;
                            seloInicio = Convert.ToInt32(txtSeloInicialRecAutDut);

                            carregaAtual = "dut";

                            for (int i = 0; i < qtdRecAutDut; i++)
                            {

                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                CarregaAto(letraSelo, seloInicio, carregaAtual);

                                seloInicio++;

                            }
                        }
                    }


                    if (qtdRecSem > 0)
                    {
                        if (txtSeloInicialRecSem.Length == 5)
                        {

                            letraSelo = txtLetraSeloRecSem;
                            seloInicio = Convert.ToInt32(txtSeloInicialRecSem);

                            carregaAtual = "rec semelhança";

                            for (int i = 0; i < qtdRecSem; i++)
                            {

                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                CarregaAto(letraSelo, seloInicio, carregaAtual);

                                seloInicio++;

                            }
                        }
                    }


                    if (qtdMat > 0)
                    {
                        if (txtSeloInicialMaterializacao.Length == 5)
                        {

                            letraSelo = txtLetraSeloMaterializacao;
                            seloInicio = Convert.ToInt32(txtSeloInicialMaterializacao);

                            carregaAtual = "materializacao";

                            for (int i = 0; i < qtdMat; i++)
                            {

                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                CarregaAto(letraSelo, seloInicio, carregaAtual);

                                seloInicio++;

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void CarregaAto(string letraSelo, int seloInicio, string atual)
        {

            ClassBalcao classBalcao = new ClassBalcao();

            string seloCompleto = string.Format("{0}{1:00000}", letraSelo, seloInicio);


            if (atual == "autenticação")
            {
                List<string> tipoAto = VerificaSelo(seloCompleto);

                if (tipoAto != null)
                {
                    if (tipoAto[0] == "AUT")
                    {
                        List<Ato> seloUtilizado = classBalcao.VerificaSeloExistente(letraSelo, seloInicio);

                        List<Ato> seloNesseRecibo = listaSelos.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == seloInicio).ToList();



                        if (seloNesseRecibo.Count > 0)
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado neste Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroAut = true;
                        }

                        if (seloUtilizado.Count == 0 || seloUtilizado[0].ReciboBalcao == Balcao.reciboBalcao.NumeroRecibo)
                        {
                            if (erroAut == false)
                                SalvaSelo(letraSelo, seloInicio, atual);
                        }
                        else
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado no Recibo Nº " + seloUtilizado[0].Recibo + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroAut = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("O SELO " + seloCompleto + " NÃO É AUTENTICAÇÃO OU AINDA NÃO FOI UTILIZADO NA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        erroAut = true;
                    }
                }
                else
                {
                    MessageBox.Show("O SELO " + seloCompleto + " COM DATA DE " + data + " NÃO FOI ENCONTRADO NO BANCO DE DADOS DA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    erroAut = true;
                }

                erroAut = false;
            }




            if (atual == "abertura")
            {
                List<string> tipoAto = VerificaSelo(seloCompleto);
                if (tipoAto != null)
                {
                    if (tipoAto[0] == "RFA")
                    {
                        List<Ato> seloUtilizado = classBalcao.VerificaSeloExistente(letraSelo, seloInicio);

                        List<Ato> seloNesseRecibo = listaSelos.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == seloInicio).ToList();

                        if (seloNesseRecibo.Count > 0)
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado neste Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroAbert = true;
                        }

                        if (seloUtilizado.Count == 0 || seloUtilizado[0].ReciboBalcao == Balcao.reciboBalcao.NumeroRecibo)
                        {
                            if (erroAbert == false)
                                SalvaSelo(letraSelo, seloInicio, atual);
                        }
                        else
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado no Recibo Nº " + seloUtilizado[0].Recibo + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroAbert = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("O SELO " + seloCompleto + " NÃO É ABERTURA DE FIRMAS OU AINDA NÃO FOI UTILIZADO NA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        erroAbert = true;
                    }
                }
                else
                {
                    MessageBox.Show("O SELO " + seloCompleto + " COM DATA DE " + data + " NÃO FOI ENCONTRADO NO BANCO DE DADOS DA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    erroAbert = true;
                }

                erroAbert = false;
            }





            if (atual == "rec autenticidade")
            {
                List<string> tipoAto = VerificaSelo(seloCompleto);
                if (tipoAto != null)
                {
                    if (tipoAto[0] == "RFR" && tipoAto[1] == "A")
                    {
                        List<Ato> seloUtilizado = classBalcao.VerificaSeloExistente(letraSelo, seloInicio);

                        List<Ato> seloNesseRecibo = listaSelos.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == seloInicio).ToList();

                        if (seloNesseRecibo.Count > 0)
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado neste Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroRecAut = true;
                        }

                        if (seloUtilizado.Count == 0 || seloUtilizado[0].ReciboBalcao == Balcao.reciboBalcao.NumeroRecibo)
                        {
                            if (erroRecAut == false)
                                SalvaSelo(letraSelo, seloInicio, atual);
                        }
                        else
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado no Recibo Nº " + seloUtilizado[0].Recibo + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroRecAut = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("O SELO " + seloCompleto + " NÃO É RECONHECIMENTO POR AUTENTICIDADE OU AINDA NÃO FOI UTILIZADO NA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        erroRecAut = true;
                    }
                }
                else
                {
                    MessageBox.Show("O SELO " + seloCompleto + " COM DATA DE " + data + " NÃO FOI ENCONTRADO NO BANCO DE DADOS DA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    erroRecAut = true;
                }

                erroRecAut = false;
            }


            if (atual == "dut")
            {
                List<string> tipoAto = VerificaSelo(seloCompleto);
                if (tipoAto != null)
                {
                    if (tipoAto[0] == "RFR" && tipoAto[1] == "D")
                    {
                        List<Ato> seloUtilizado = classBalcao.VerificaSeloExistente(letraSelo, seloInicio);

                        List<Ato> seloNesseRecibo = listaSelos.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == seloInicio).ToList();

                        if (seloNesseRecibo.Count > 0)
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado neste Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroRecAutDut = true;
                        }

                        if (seloUtilizado.Count == 0 || seloUtilizado[0].ReciboBalcao == Balcao.reciboBalcao.NumeroRecibo)
                        {
                            if (erroRecAutDut == false)
                                SalvaSelo(letraSelo, seloInicio, atual);
                        }
                        else
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado no Recibo Nº " + seloUtilizado[0].Recibo + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroRecAutDut = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("O SELO " + seloCompleto + " NÃO É RECONHECIMENTO POR AUTENTICIDADE (DUT) OU AINDA NÃO FOI UTILIZADO NA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        erroRecAutDut = true;
                    }
                }
                else
                {
                    MessageBox.Show("O SELO " + seloCompleto + " COM DATA DE " + data + " NÃO FOI ENCONTRADO NO BANCO DE DADOS DA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    erroRecAutDut = true;
                }

                erroRecAutDut = false;
            }


            if (atual == "rec semelhança")
            {
                List<string> tipoAto = VerificaSelo(seloCompleto);

                if (tipoAto != null)
                {
                    if (((tipoAto[0] == "RFR") && (tipoAto[1] == "S")) || ((tipoAto[0] == "RFR") && (tipoAto[1] == "P")))
                    {
                        List<Ato> seloUtilizado = classBalcao.VerificaSeloExistente(letraSelo, seloInicio);

                        List<Ato> seloNesseRecibo = listaSelos.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == seloInicio).ToList();

                        if (seloNesseRecibo.Count > 0)
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado neste Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroRecSem = true;
                        }

                        if (seloUtilizado.Count == 0 || seloUtilizado[0].ReciboBalcao == Balcao.reciboBalcao.NumeroRecibo)
                        {
                            if (tipoAto[1] == "S")
                                atual = "REC SEMELHANÇA";
                            else
                                atual = "SINAL PÚBLICO";

                            if (erroRecSem == false)
                                SalvaSelo(letraSelo, seloInicio, atual);
                        }
                        else
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado no Recibo Nº " + seloUtilizado[0].Recibo + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroRecSem = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("O SELO " + seloCompleto + " NÃO É RECONHECIMENTO POR SEMELHANÇA OU AINDA NÃO FOI UTILIZADO NA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        erroRecSem = true;
                    }
                }
                else
                {
                    MessageBox.Show("O SELO " + seloCompleto + " COM DATA DE " + data + " NÃO FOI ENCONTRADO NO BANCO DE DADOS DA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    erroRecSem = true;
                }

                erroRecSem = false;
            }


            if (atual == "materializacao")
            {
                List<string> tipoAto = VerificaSelo(seloCompleto);
                if (tipoAto != null)
                {
                    if (((tipoAto[0] == "MAT")))
                    {
                        List<Ato> seloUtilizado = classBalcao.VerificaSeloExistente(letraSelo, seloInicio);

                        List<Ato> seloNesseRecibo = listaSelos.Where(p => p.LetraSelo == letraSelo && p.NumeroSelo == seloInicio).ToList();

                        if (seloNesseRecibo.Count > 0)
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado neste Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroMaterializacao = true;
                        }

                        if (seloUtilizado.Count == 0 || seloUtilizado[0].ReciboBalcao == Balcao.reciboBalcao.NumeroRecibo)
                        {
                            if (erroMaterializacao == false)
                                SalvaSelo(letraSelo, seloInicio, atual);
                        }
                        else
                        {
                            MessageBox.Show("Selo Nº " + string.Format("{0:00000}", seloCompleto) + ", já foi utilizado no Recibo Nº " + seloUtilizado[0].Recibo + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            erroMaterializacao = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("O SELO " + seloCompleto + " NÃO É MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS OU AINDA NÃO FOI UTILIZADO NA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        erroMaterializacao = true;
                    }
                }
                else
                {
                    MessageBox.Show("O SELO " + seloCompleto + " COM DATA DE " + data + " NÃO FOI ENCONTRADO NO BANCO DE DADOS DA TOTAL, FAVOR VERIFICAR!", "ATENÇÃO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    erroMaterializacao = true;
                }

                erroMaterializacao = false;
            }

        }


        private void AdicionarSelos()
        {
            try
            {
                if (qtdAut > 0)
                {
                    if (txtSeloInicialAut.Length == 9)
                    {

                        string letraSelo = txtSeloInicialAut.Substring(0, 4);
                        int seloInicio = Convert.ToInt32(txtSeloInicialAut.Substring(4, txtSeloInicialAut.Length - 4));

                        carregaAtual = "autenticação";

                        for (int i = 0; i < qtdAut; i++)
                        {

                            CarregaAto(letraSelo, seloInicio, carregaAtual);

                            seloInicio++;
                        }
                    }
                }


                if (qtdAbert > 0)
                {
                    if (txtSeloInicialAbert.Length == 9)
                    {

                        string letraSelo = txtSeloInicialAbert.Substring(0, 4);
                        int seloInicio = Convert.ToInt32(txtSeloInicialAbert.Substring(4, txtSeloInicialAbert.Length - 4));

                        carregaAtual = "abertura";

                        for (int i = 0; i < qtdAbert; i++)
                        {

                            CarregaAto(letraSelo, seloInicio, carregaAtual);

                            seloInicio++;
                        }
                    }
                }


                if (qtdRecAut > 0)
                {
                    if (txtSeloInicialRecAut.Length == 9)
                    {

                        string letraSelo = Balcao.txtSeloInicialRecAut.Text.Substring(0, 4);
                        int seloInicio = Convert.ToInt32(txtSeloInicialRecAut.Substring(4, txtSeloInicialRecAut.Length - 4));

                        carregaAtual = "rec autenticidade";

                        for (int i = 0; i < qtdRecAut; i++)
                        {

                            CarregaAto(letraSelo, seloInicio, carregaAtual);

                            seloInicio++;
                        }
                    }
                }


                if (qtdRecAutDut > 0)
                {
                    if (txtSeloInicialRecAutDut.Length == 9)
                    {

                        string letraSelo = Balcao.txtSeloInicialRecAutDut.Text.Substring(0, 4);
                        int seloInicio = Convert.ToInt32(txtSeloInicialRecAutDut.Substring(4, txtSeloInicialRecAutDut.Length - 4));

                        carregaAtual = "dut";

                        for (int i = 0; i < qtdRecAutDut; i++)
                        {

                            CarregaAto(letraSelo, seloInicio, carregaAtual);

                            seloInicio++;
                        }
                    }
                }


                if (qtdRecSem > 0)
                {
                    if (txtSeloInicialRecSem.Length == 9)
                    {

                        string letraSelo = txtSeloInicialRecSem.Substring(0, 4);
                        int seloInicio = Convert.ToInt32(txtSeloInicialRecSem.Substring(4, txtSeloInicialRecSem.Length - 4));

                        carregaAtual = "rec semelhança";

                        for (int i = 0; i < qtdRecSem; i++)
                        {

                            CarregaAto(letraSelo, seloInicio, carregaAtual);

                            seloInicio++;
                        }
                    }
                }

                if (qtdMat > 0)
                {
                    if (txtSeloInicialMaterializacao.Length == 9)
                    {

                        string letraSelo = txtSeloInicialMaterializacao.Substring(0, 4);
                        int seloInicio = Convert.ToInt32(txtSeloInicialMaterializacao.Substring(4, txtSeloInicialMaterializacao.Length - 4));

                        carregaAtual = "materializacao";

                        for (int i = 0; i < qtdMat; i++)
                        {

                            CarregaAto(letraSelo, seloInicio, carregaAtual);

                            seloInicio++;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }


        private List<string> VerificaSelo(string seloCompleto)
        {
            string SeloTotal = "";
            string TipoAtoTotal = "";
            string TipoRec = "";




            List<string> tipos = new List<string>();

            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingBalcaoSite);
            conTotal.Open();
            try
            {


                FbCommand cmdTotal = new FbCommand("Select * from atos where STATUS = 'XML' AND SELO = '" + seloCompleto + "' and DATA = '" + data + "'", conTotal);

                cmdTotal.CommandType = CommandType.Text;

                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                while (drTotal.Read())
                {
                    descricaoAto = drTotal["DESCRICAO"].ToString();

                    fichaAto = drTotal["FICHA"].ToString();



                    if (drTotal["TIPO"].ToString() == "RFA" && drTotal["RTD_TOTAL"].ToString() == "0")
                    {
                        TipoAtoTotal = "RFR";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "A";
                    }
                    if (drTotal["TIPO"].ToString() == "RFS")
                    {
                        TipoAtoTotal = "RFR";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "S";
                    }

                    if (drTotal["TIPO"].ToString() == "SIN")
                    {
                        TipoAtoTotal = "RFR";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "P";
                    }

                    if (drTotal["TIPO"].ToString() == "ABR")
                    {
                        TipoAtoTotal = "RFA";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "";
                    }
                    if (drTotal["TIPO"].ToString() == "AUT")
                    {
                        TipoAtoTotal = "AUT";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "";
                    }

                    if (drTotal["TIPO"].ToString() == "MAT")
                    {
                        TipoAtoTotal = "MAT";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "";
                    }

                    if (drTotal["RTD_TOTAL"].ToString() != "0")
                    {
                        TipoAtoTotal = "RFR";
                        SeloTotal = drTotal["SELO"].ToString();
                        TipoRec = "D";
                    }
                }
                tipos.Add(TipoAtoTotal);
                tipos.Add(TipoRec);

                return tipos;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conTotal.Close();
            }


        }

        private void SalvaSelo(string letraSelo, int numeroSelo, string atual)
        {
            atoCorrente = new Ato();

            decimal arquivamento = 0;

            decimal itemAbertura = 0;

            decimal valorDut = 0;


            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Siss = "0,00";
            string Spmcmv_2 = "0,00";
            int index;

            string SemolArquiv = "0,00";
            string Sfetj_20Arquiv = "0,00";
            string Sfundperj_5Arquiv = "0,00";
            string Sfunperj_5Arquiv = "0,00";
            string Sfunarpen_4Arquiv = "0,00";
            string SissArquiv = "0,00";



            try
            {
                // data do pagamento
                if (datePickerData.Date != null)
                {
                    atoCorrente.DataPagamento = datePickerData.Date;
                }


                // tipo de pagamento
                atoCorrente.TipoPagamento = cmbTipoPagamento;


                // data do ato
                if (datePickerData.Date != null)
                {
                    atoCorrente.DataAto = datePickerData.Date;
                }



                // pago
                if (checkBoxPago == true)
                    atoCorrente.Pago = true;
                else
                    atoCorrente.Pago = false;



                // IdUsuario
                atoCorrente.IdUsuario = Balcao.usuarioLogado.Id_Usuario;



                // Usuario
                atoCorrente.Usuario = Balcao.usuarioLogado.NomeUsu;


                // Atribuiçao
                atoCorrente.Atribuicao = "BALCÃO";


                // Letra Selo

                atoCorrente.LetraSelo = letraSelo;

                // Numero Selo

                atoCorrente.NumeroSelo = numeroSelo;

                // valor adicionar
                atoCorrente.ValorAdicionar = txtAdicionar;


                // Valor Desconto
                atoCorrente.ValorDesconto = txtDesconto;


                atoCorrente.DescricaoAto = descricaoAto;

                atoCorrente.FichaAto = fichaAto;


                // Mensalista
                if (cmbTipoPagamentoIndice == 2)
                {
                    if (cmbMensalistaIndice >= 0)
                    {
                        atoCorrente.Mensalista = cmbMensalista;
                    }

                }

                if (atual == "autenticação")
                {

                    // TipoAto
                    atoCorrente.TipoAto = "AUTENTICAÇÃO";

                    atoCorrente.Natureza = "AUTENTICAÇÃO";

                    //Emolumentos
                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "AUTENTICAÇÃO POR DOCUMENTO OU PÁGINA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;


                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;


                }


                if (atual == "abertura")
                {

                    // TipoAto
                    atoCorrente.TipoAto = "ABERTURA DE FIRMAS";



                    atoCorrente.Natureza = "ABERTURA DE FIRMAS";


                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "ABERTURA DE FIRMA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").Select(p => p.VALOR).FirstOrDefault());

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;




                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura + arquivamento;
                        fetj_20 = emol * 20 / 100;
                        fundperj_5 = emol * 5 / 100;
                        funperj_5 = emol * 5 / 100;
                        funarpen_4 = emol * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;



                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);


                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    emol = Convert.ToDecimal(Semol);
                    fetj_20 = Convert.ToDecimal(Sfetj_20);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;
                }

                if (atual == "rec autenticidade")
                {


                    // TipoAto
                    atoCorrente.TipoAto = "REC AUTENTICIDADE";



                    atoCorrente.Natureza = "REC AUTENTICIDADE";

                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;





                }
                if (atual == "dut")
                {
                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES").Select(p => p.VALOR).FirstOrDefault());

                    valorDut = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE (DUT)").Select(p => p.VALOR).FirstOrDefault());




                    // TipoAto
                    atoCorrente.TipoAto = "REC AUTENTICIDADE (DUT)";

                    atoCorrente.Natureza = "REC AUTENTICIDADE (DUT)";


                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;


                }
                if (atual == "REC SEMELHANÇA" || atual == "SINAL PÚBLICO")
                {

                    // TipoAto
                    atoCorrente.TipoAto = atual;

                    atoCorrente.Natureza = atual;

                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR SEMELHANÇA OU CHANCELA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;

                }

                if (atual == "materializacao")
                {

                    // TipoAto
                    atoCorrente.TipoAto = "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS";

                    atoCorrente.Natureza = "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS";

                    itemAbertura = Convert.ToDecimal(Balcao.custas.Where(p => p.DESCR == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = 0;

                    decimal emol = 0;
                    decimal fetj_20 = 0;
                    decimal fundperj_5 = 0;
                    decimal funperj_5 = 0;
                    decimal funarpen_4 = 0;
                    decimal pmcmv_2 = 0;
                    decimal iss = 0;

                    decimal arquivEmol = 0;
                    decimal arquiv20 = 0;
                    decimal arquiv5 = 0;
                    decimal arquiv4 = 0;
                    decimal arquivIss = 0;


                    if (cmbTipoCustasIndice <= 1)
                    {

                        emol = itemAbertura;
                        fetj_20 = itemAbertura * 20 / 100;
                        fundperj_5 = itemAbertura * 5 / 100;
                        funperj_5 = itemAbertura * 5 / 100;
                        funarpen_4 = itemAbertura * 4 / 100;
                        pmcmv_2 = itemAbertura * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * Balcao.porcentagemIss / 100;
                        arquivIss = arquivamento * Balcao.porcentagemIss / 100;

                        if (cmbTipoCustasIndice == 0)
                        {
                            Semol = Convert.ToString(emol);

                            if (arquivEmol > 0)
                                SemolArquiv = Convert.ToString(arquivEmol);

                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Siss = Convert.ToString(iss);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);

                        if (arquivEmol > 0)
                        {
                            Sfetj_20Arquiv = Convert.ToString(arquiv20);
                            Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                            Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                            SissArquiv = Convert.ToString(arquivIss);
                        }

                    }

                    if (cmbTipoCustasIndice > 1)
                    {

                        valorDut = 0;

                        emol = 0;
                        fetj_20 = 0;
                        fundperj_5 = 0;
                        funperj_5 = 0;
                        funarpen_4 = 0;
                        iss = 0;
                        pmcmv_2 = 0;

                        Semol = "0,00";
                        Sfetj_20 = "0,00";
                        Sfundperj_5 = "0,00";
                        Sfunperj_5 = "0,00";
                        Sfunarpen_4 = "0,00";
                        Siss = "0,00";
                        Spmcmv_2 = "0,00";


                        arquivEmol = 0;
                        arquiv20 = 0;
                        arquiv5 = 0;
                        arquiv4 = 0;

                        SemolArquiv = "0,00";
                        Sfetj_20Arquiv = "0,00";
                        Sfundperj_5Arquiv = "0,00";
                        Sfunperj_5Arquiv = "0,00";
                        Sfunarpen_4Arquiv = "0,00";
                        SissArquiv = "0,00";

                    }


                    index = Semol.IndexOf(',');
                    Semol = Semol.Substring(0, index + 3);

                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                    index = Sfundperj_5.IndexOf(',');
                    Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                    index = Sfunperj_5.IndexOf(',');
                    Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                    index = Sfunarpen_4.IndexOf(',');
                    Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);

                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                    //--------------


                    index = SemolArquiv.IndexOf(',');
                    SemolArquiv = SemolArquiv.Substring(0, index + 3);

                    index = Sfetj_20Arquiv.IndexOf(',');
                    Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                    index = Sfundperj_5Arquiv.IndexOf(',');
                    Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunperj_5Arquiv.IndexOf(',');
                    Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                    index = Sfunarpen_4Arquiv.IndexOf(',');
                    Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                    index = SissArquiv.IndexOf(',');
                    SissArquiv = SissArquiv.Substring(0, index + 3);

                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);


                    atoCorrente.Emolumentos = emol;


                    atoCorrente.Fetj = fetj_20;

                    //Fundperj
                    atoCorrente.Fundperj = fundperj_5;


                    //Funperj
                    atoCorrente.Funperj = funperj_5;


                    //Funarpen
                    atoCorrente.Funarpen = funarpen_4;

                    //iss
                    atoCorrente.Iss = iss;

                    // Pmcmv
                    atoCorrente.Pmcmv = pmcmv_2;
                }

                // Escrevente
                atoCorrente.Escrevente = cmbFuncionario;

                // TipoCobranca
                atoCorrente.TipoCobranca = cmbTipoCustas;


                //NumeroRequisicao
                if (cmbTipoPagamentoIndice == 2)
                {
                    if (txtRequisicao != "")
                    {
                        atoCorrente.NumeroRequisicao = Convert.ToInt32(txtRequisicao);
                    }
                }

                // Total
                atoCorrente.Total = atoCorrente.Emolumentos + atoCorrente.Fetj + atoCorrente.Fundperj + atoCorrente.Funperj + atoCorrente.Funarpen + atoCorrente.Iss + atoCorrente.Pmcmv + valorDut;

                Balcao.listaSelos.Add(atoCorrente);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }



}
