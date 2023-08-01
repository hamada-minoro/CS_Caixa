using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for WinAguardeAtualizacaoCustas.xaml
    /// </summary>
    public partial class WinAguardeAtualizacaoCustas : Window
    {
        List<CustasNota> listaCustasNotas = new List<CustasNota>();
        List<CustasProtesto> listaCustasProtesto = new List<CustasProtesto>();
        List<CustasRgi> listaCustasRgi = new List<CustasRgi>();
        List<CustasDistribuicao> listaCustasDistribuicao = new List<CustasDistribuicao>();
        ClassTabelaCustas tabelaCustas = new ClassTabelaCustas();
        string acao = string.Empty;
        BackgroundWorker worker;
        

        int ano = 2021;

        int anoAnterior = 2020;


        // Atos Comuns
        decimal mutua = 0M;
        decimal acoterj = 0M;
        decimal porcentagemISS = 5.26M;

        decimal tab16_item1 = 0.95M;
        decimal tab16_item2 = 22.28M;
        decimal tab16_item3 = 0M;
        decimal tab16_item4 = 11.63M;
        decimal tab16_item5 = 13.48M;
        decimal tab16_item6 = 19.34M;


        // Distribuição
        decimal porPessoaValor = 1.04M;
        decimal valorFixo = 22.28M;



       

        // RGI
        decimal tab201_item1_sub1 = 146.78M;
        decimal tab201_item1_sub2 = 211.05M;
        decimal tab201_item1_sub3 = 348.73M;
        decimal tab201_item1_sub4 = 486.44M;
        decimal tab201_item1_sub5 = 596.57M;
        decimal tab201_item1_sub6 = 1057.39M;
        decimal tab201_item1_sub7 = 1248.32M;
        decimal tab201_item1_sub8 = 1688.89M;
        decimal tab201_item1_sub9 = 1817.44M;
        decimal tab201_itemNI = 162.81M;

        decimal tab202_item1_sub1 = 1368.82M;
        decimal tab202_item1_sub2 = 2195.80M;
        decimal tab202_item1_sub3 = 3054.98M;
        decimal tab202_item1_sub4 = 3484.56M;
        decimal tab202_itemNI = 162.81M;

        decimal tab203_item1_sub1 = 147.06M;
        decimal tab203_item1_sub2 = 185.33M;
        decimal tab203_item1_sub3 = 261.72M;
        decimal tab203_item1_sub4 = 300.10M;
        decimal tab203_item1_sub5 = 376.57M;
        decimal tab203_item1_sub6 = 459.15M;
        decimal tab203_item1_sub7 = 541.49M;
        decimal tab203_item1_sub8 = 589.23M;
        decimal tab203_itemNI = 81.39M;

        decimal tab204_item1 = 110.10M;
        decimal tab204_item2 = 367.12M;
        decimal tab204_item3 = 23.80M;
        decimal tab204_item4 = 34.24M;
        decimal tab204_item5_subA = 139.44M;
        decimal tab204_item5_subB = 20.12M;
        decimal tab204_item5_subC = 18.29M;
        decimal tab204_item6 = 82.54M;
        decimal tab204_item7_subA = 6.75M;
        decimal tab204_item7_subB = 1.40M;
        decimal tab204_item8_subA = 73.37M;
        decimal tab204_item8_subB = 34.24M;
        decimal tab204_item8_subC = 34.24M;
        decimal tab204_item8_subD = 34.24M;
        decimal tab204_item9_subA1 = 330.37M;
        decimal tab204_item9_subA2 = 34.24M;
        decimal tab204_item9_subA3 = 34.24M;
        decimal tab204_item9_subB = 110.10M;
        decimal tab204_item9_subC = 110.10M;
        decimal tab204_item10 = 34.24M;
        decimal tab204_item10_subA = 3.17M;
        decimal tab204_item10_subB = 19.19M;
        decimal tab204_item11 = 73.37M;

        decimal indisponibilidade = 24.01M;
        decimal prenotacao = 23.80M;
        decimal renovacaoDevolucao = 41.27M; // METADE DE tab204_item6




        // Protesto
        decimal tarifaBancaria = 29.90M;

        decimal tab24_item1_subA = 14.15M;
        decimal tab24_item1_subB = 28.48M;
        decimal tab24_item1_subC = 42.69M;
        decimal tab24_item1_subD = 57.03M;
        decimal tab24_item1_subE = 71.25M;
        decimal tab24_item1_subF = 85.46M;
        decimal tab24_item1_subG = 99.81M;
        decimal tab24_item1_subH = 114.03M;
        decimal tab24_item1_subI = 128.25M;
        decimal tab24_item1_subJ = 142.57M;
        decimal tab24_item1_subK = 171.14M;
        decimal tab24_item1_subL = 199.69M;
        decimal tab24_item1_subM = 228.13M;
        decimal tab24_item1_subN = 256.68M;
        decimal tab24_item1_subO = 285.22M;
        decimal tab24_item1_subP = 320.79M;
        decimal tab24_item1_subQ = 356.35M;
        decimal tab24_item1_subR = 391.90M;
        decimal tab24_item1_subS = 427.46M;
        decimal tab24_item1_subT = 463.04M;
        decimal tab24_item1_subU = 498.60M;
        decimal tab24_item1_subV = 534.15M;
        decimal tab24_item1_subW = 569.69M;
        decimal tab24_item1_subX = 605.25M;
        decimal tab24_item1_subY = 640.80M;
        decimal tab24_item1_subZ = 676.37M;

        decimal tab24_item2 = 53.16M;

        decimal tab24_item3_sub1 = 23.44M;
        decimal tab24_item3_sub2 = 12.77M;



        


        // Notas
        decimal apostilamento = 56.83M; // tab7_item2
        decimal autenticidadeDUT = 31.58M; // SOMAR (tab16_item5 + tab25_item10) => CALCULAR AS CUSTAS E PEGAR O TOTAL.


        decimal tab22_item1_sub1 = 211.05M; 
        decimal tab22_item1_sub2 = 348.73M;
        decimal tab22_item1_sub3 = 486.44M;
        decimal tab22_item1_sub4 = 596.57M;
        decimal tab22_item1_sub5 = 1057.39M;
        decimal tab22_item1_sub6 = 1248.32M;
        decimal tab22_item1_sub7 = 1688.89M;
        decimal tab22_item1_sub8 = 1812.22M;

        decimal tab22_item1_1 = 1473.72M;
        decimal tab22_item1_1_subA = 101.55M;

        decimal tab22_item1_2_subA = 111.95M;
        decimal tab22_item1_2_subB = 293.67M;

        decimal tab22_item1_3 = 111.95M;

        decimal tab22_item1_4 = 155.97M;
        decimal tab22_item1_4_subA = 18.29M;

        decimal tab22_item2_subA = 19.19M;
        decimal tab22_item2_subB = 275.30M;
        decimal tab22_item2_subC = 0M;
        decimal tab22_item2_subD = 56.83M;

        decimal tab22_item2_1 = 9.09M;

        decimal tab22_item3_subA = 6.24M;
        decimal tab22_item3_subB = 6.06M;
        decimal tab22_item3_subC = 13.56M;
        decimal tab22_item3_subD = 0M;

        decimal tab22_item4 = 6.25M;

        decimal tab22_item5_subIA = 301.01M;
        decimal tab22_item5_subIB = 440.56M;

        decimal tab22_item5_subII = 440.56M;

        decimal tab22_item5_subIIA = 146.78M;

        decimal tab22_item5_subIIB = 137.77M;

        decimal tab22_item6 = 183.52M;
        decimal tab22_item6_subA = 27.46M;

        decimal tab22_item7_sub1 = 211.05M;
        decimal tab22_item7_sub2 = 348.73M;
        decimal tab22_item7_sub3 = 486.44M;
        decimal tab22_item7_sub4 = 596.57M;
        decimal tab22_item7_sub5 = 1057.39M;
        decimal tab22_item7_sub6 = 1248.32M;
        decimal tab22_item7_sub7 = 1688.89M;
        decimal tab22_item7_sub8 = 1812.22M;

        decimal tab22_itemNI_sub8 = 0M;
        decimal tab22_itemNI_sub13 = 0M;
        decimal tab22_itemNI_sub20 = 162.81M;
        decimal tab22_itemNI_sub21 = 3.25M;

        decimal tab22_item16 = 12.54M; 

        decimal escrituraSemValorDeclarado = 111.95M;

        decimal copia = 0.42M;
        decimal rtd = 9.09M;

        public WinAguardeAtualizacaoCustas()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label2.Content = "Carregando Informações...";

            listaCustasNotas = tabelaCustas.ListarCustasNotas(ano);
            listaCustasProtesto = tabelaCustas.ListarCustasProtesto(ano);
            listaCustasRgi = tabelaCustas.ListarCustasRgi(ano);
            listaCustasDistribuicao = tabelaCustas.ListarCustasDistribuicao(ano);


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

            if (acao == "removendo notas")
            {
                progressBar1.Maximum = listaCustasNotas.Count;
                label2.Content = "Removendo Registros Existentes.";
            }
            if (acao == "removendo protesto")
            {
                progressBar1.Maximum = listaCustasProtesto.Count;
                label2.Content = "Removendo Registros Existentes.";
            }
            if (acao == "removendo rgi")
            {
                progressBar1.Maximum = listaCustasRgi.Count;
                label2.Content = "Removendo Registros Existentes.";
            }

            if (acao == "removendo distribuicao")
            {
                progressBar1.Maximum = listaCustasDistribuicao.Count;
                label2.Content = "Removendo Registros Existentes.";
            }



            if (acao == "atualizando notas")
            {
                progressBar1.Maximum = listaCustasNotas.Count;
                label2.Content = "Atualizando Custas Notas.";
            }
            if (acao == "atualizando protesto")
            {
                progressBar1.Maximum = listaCustasProtesto.Count;
                label2.Content = "Atualizando Custas Protesto.";
            }
            if (acao == "atualizando rgi")
            {
                progressBar1.Maximum = listaCustasRgi.Count;
                label2.Content = "Atualizando Custas RGI.";
            }
            if (acao == "atualizando distribuicao")
            {
                progressBar1.Maximum = listaCustasDistribuicao.Count;
                label2.Content = "Atualizando Custas Distribuição.";
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

                RemoverCustasExistentes();

                listaCustasNotas = tabelaCustas.ListarCustasNotas(anoAnterior);
                listaCustasProtesto = tabelaCustas.ListarCustasProtesto(anoAnterior);
                listaCustasRgi = tabelaCustas.ListarCustasRgi(anoAnterior);

                CustasNotas();
                CustasRgi();
                CustasProtesto();
                CustaDistribuicao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao tentar atualizar as custas. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RemoverCustasExistentes()
        {

            if (listaCustasNotas.Count > 0)
            {
                acao = "removendo notas";
                for (int i = 0; i < listaCustasNotas.Count; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(i);
                    tabelaCustas.ExcluirCustasNotas(listaCustasNotas[i]);
                }
            }

            if (listaCustasProtesto.Count > 0)
            {
                acao = "removendo protesto";
                for (int i = 0; i < listaCustasProtesto.Count; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(i);
                    tabelaCustas.ExcluirCustasProtesto(listaCustasProtesto[i]);
                }
            }

            if (listaCustasRgi.Count > 0)
            {
                acao = "removendo rgi";
                for (int i = 0; i < listaCustasRgi.Count; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(i);
                    tabelaCustas.ExcluirCustasRgi(listaCustasRgi[i]);
                }
            }

            if (listaCustasDistribuicao.Count > 0)
            {
                acao = "removendo distribuicao";
                for (int i = 0; i < listaCustasDistribuicao.Count; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(i);
                    tabelaCustas.ExcluirCustasDistribuicao(listaCustasDistribuicao[i]);
                }
            }
        }

        private void CustaDistribuicao()
        {
            acao = "atualizando distribuicao";

            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss;
            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Spmcmv_2 = "0,00";
            string Siss = "0,00";
            int index;

            decimal porPessoa = 0M;

            try
            {
                for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(i);

                    emol = valorFixo;
                    pmcmv_2 = (emol * 2) / 100;
                    iss = porPessoa * i;
                    iss = (iss * 2) / 100;
                    emol = valorFixo + (porPessoa * i);
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    Semol = Convert.ToString(emol);
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                    Siss = Convert.ToString(iss);
                    index = Sfetj_20.IndexOf(',');
                    Sfetj_20 = Sfetj_20.Substring(0, index + 3);

                    if (Sfundperj_5.Contains(","))
                    {
                        index = Sfundperj_5.IndexOf(',');
                        Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);
                    }

                    if (Sfunperj_5.Contains(","))
                    {
                        index = Sfunperj_5.IndexOf(',');
                        Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);
                    }


                    if (Sfunarpen_4.Contains(","))
                    {
                        index = Sfunarpen_4.IndexOf(',');
                        Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);
                    }

                    if (Spmcmv_2.Contains(","))
                    {
                        index = Spmcmv_2.IndexOf(',');
                        Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);
                    }

                    if (Siss.Contains(","))
                    {
                        index = Siss.IndexOf(',');
                        if (Siss.Length > 3)
                            Siss = Siss.Substring(0, index + 3);
                        else
                            Siss = string.Format("{0:n2}", Convert.ToDecimal(Siss));
                    }

                    var custaDist = new CustasDistribuicao();

                    custaDist.Emolumentos = emol;
                    custaDist.Fetj = Convert.ToDecimal(Sfetj_20);
                    custaDist.Fundperj = Convert.ToDecimal(Sfundperj_5);
                    custaDist.Funperj = Convert.ToDecimal(Sfunperj_5);
                    custaDist.Funarpen = Convert.ToDecimal(Sfunarpen_4);
                    custaDist.Pmcmv = Convert.ToDecimal(Spmcmv_2);
                    custaDist.Iss = Convert.ToDecimal(Siss);
                    custaDist.Quant_Exced = i;
                    custaDist.Total = custaDist.Emolumentos + custaDist.Fetj + custaDist.Fundperj + custaDist.Funperj + custaDist.Funarpen + custaDist.Pmcmv + custaDist.Iss;
                    custaDist.VrFixo = valorFixo;
                    custaDist.VrExced = porPessoa;
                    custaDist.Ano = ano;

                    tabelaCustas.SalvarCustasDistribuicao(custaDist);

                    porPessoa = porPessoaValor;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void CustasProtesto()
        {
            acao = "atualizando protesto";
            for (int i = 0; i < listaCustasProtesto.Count(); i++)
            {
                Thread.Sleep(1);
                worker.ReportProgress(i);


                var custaProtesto = new CustasProtesto();
                custaProtesto = listaCustasProtesto[i];
                custaProtesto.ANO = ano;



                if (custaProtesto.DESCR == "TARIFA BANCÁRIA")
                {
                    custaProtesto.VALOR = tarifaBancaria;
                }

                if (custaProtesto.DESCR == "MUTUA")
                {
                    custaProtesto.VALOR = mutua;
                }

                if (custaProtesto.DESCR == "ACOTERJ")
                {
                    custaProtesto.VALOR = acoterj;
                }

                if (custaProtesto.TAB == "16" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab16_item1;
                }

                if (custaProtesto.TAB == "16" && custaProtesto.ITEM == "2" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab16_item2;
                }

                if (custaProtesto.TAB == "16" && custaProtesto.ITEM == "3" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab16_item3;
                }

                if (custaProtesto.TAB == "16" && custaProtesto.ITEM == "4" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab16_item4;
                }

                if (custaProtesto.TAB == "16" && custaProtesto.ITEM == "5" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab16_item5;
                }

                if (custaProtesto.TAB == "16" && custaProtesto.ITEM == "6" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab16_item6;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "A")
                {
                    custaProtesto.VALOR = tab24_item1_subA;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "B")
                {
                    custaProtesto.VALOR = tab24_item1_subB;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "C")
                {
                    custaProtesto.VALOR = tab24_item1_subC;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "D")
                {
                    custaProtesto.VALOR = tab24_item1_subD;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "E")
                {
                    custaProtesto.VALOR = tab24_item1_subE;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "F")
                {
                    custaProtesto.VALOR = tab24_item1_subF;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "G")
                {
                    custaProtesto.VALOR = tab24_item1_subG;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "H")
                {
                    custaProtesto.VALOR = tab24_item1_subH;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "I")
                {
                    custaProtesto.VALOR = tab24_item1_subI;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "J")
                {
                    custaProtesto.VALOR = tab24_item1_subJ;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "K")
                {
                    custaProtesto.VALOR = tab24_item1_subK;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "L")
                {
                    custaProtesto.VALOR = tab24_item1_subL;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "M")
                {
                    custaProtesto.VALOR = tab24_item1_subM;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "N")
                {
                    custaProtesto.VALOR = tab24_item1_subN;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "O")
                {
                    custaProtesto.VALOR = tab24_item1_subO;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "P")
                {
                    custaProtesto.VALOR = tab24_item1_subP;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "Q")
                {
                    custaProtesto.VALOR = tab24_item1_subQ;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "R")
                {
                    custaProtesto.VALOR = tab24_item1_subR;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "S")
                {
                    custaProtesto.VALOR = tab24_item1_subS;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "T")
                {
                    custaProtesto.VALOR = tab24_item1_subT;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "U")
                {
                    custaProtesto.VALOR = tab24_item1_subU;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "V")
                {
                    custaProtesto.VALOR = tab24_item1_subV;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "W")
                {
                    custaProtesto.VALOR = tab24_item1_subW;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "X")
                {
                    custaProtesto.VALOR = tab24_item1_subX;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "Y")
                {
                    custaProtesto.VALOR = tab24_item1_subY;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "1" && custaProtesto.SUB == "Z")
                {
                    custaProtesto.VALOR = tab24_item1_subZ;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "2" && custaProtesto.SUB == "*")
                {
                    custaProtesto.VALOR = tab24_item2;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "3" && custaProtesto.SUB == "1")
                {
                    custaProtesto.VALOR = tab24_item3_sub1;
                }

                if (custaProtesto.TAB == "24" && custaProtesto.ITEM == "3" && custaProtesto.SUB == "2")
                {
                    custaProtesto.VALOR = tab24_item3_sub2;
                }

                if (custaProtesto.DESCR == "PORCENTAGEM ISS")
                {
                    custaProtesto.VALOR = porcentagemISS;
                }

                tabelaCustas.SalvarCustasProtesto(custaProtesto);
            }
        }

        private void CustasRgi()
        {
            acao = "atualizando rgi";
            for (int i = 0; i < listaCustasRgi.Count(); i++)
            {
                Thread.Sleep(1);
                worker.ReportProgress(i);

                var custaRgi = new CustasRgi();
                custaRgi = listaCustasRgi[i];
                custaRgi.ANO = ano;

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "NI" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab203_itemNI;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "1" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item1;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "2" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item2;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "3" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item3;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "4" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item4;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "5" && custaRgi.SUB == "A")
                {
                    custaRgi.VALOR = tab204_item5_subA;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "5" && custaRgi.SUB == "B")
                {
                    custaRgi.VALOR = tab204_item5_subB;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "5" && custaRgi.SUB == "C")
                {
                    custaRgi.VALOR = tab204_item5_subC;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "6" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item6;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "7" && custaRgi.SUB == "A")
                {
                    custaRgi.VALOR = tab204_item7_subA;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "7" && custaRgi.SUB == "B")
                {
                    custaRgi.VALOR = tab204_item7_subB;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "8" && custaRgi.SUB == "A")
                {
                    custaRgi.VALOR = tab204_item8_subA;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "8" && custaRgi.SUB == "B")
                {
                    custaRgi.VALOR = tab204_item8_subB;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "8" && custaRgi.SUB == "C")
                {
                    custaRgi.VALOR = tab204_item8_subC;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "8" && custaRgi.SUB == "D")
                {
                    custaRgi.VALOR = tab204_item8_subD;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "9" && custaRgi.SUB == "A1")
                {
                    custaRgi.VALOR = tab204_item9_subA1;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "9" && custaRgi.SUB == "A2")
                {
                    custaRgi.VALOR = tab204_item9_subA2;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "9" && custaRgi.SUB == "A3")
                {
                    custaRgi.VALOR = tab204_item9_subA3;
                }
                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "9" && custaRgi.SUB == "B")
                {
                    custaRgi.VALOR = tab204_item9_subB;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "9" && custaRgi.SUB == "C")
                {
                    custaRgi.VALOR = tab204_item9_subC;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "10" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item10;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "10" && custaRgi.SUB == "A")
                {
                    custaRgi.VALOR = tab204_item10_subA;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "10" && custaRgi.SUB == "B")
                {
                    custaRgi.VALOR = tab204_item10_subB;
                }

                if (custaRgi.TAB == "20.4" && custaRgi.ITEM == "11" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab204_item11;
                }

                if (custaRgi.TAB == "16" && custaRgi.ITEM == "1" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab16_item1;
                }

                if (custaRgi.TAB == "16" && custaRgi.ITEM == "2" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab16_item2;
                }

                if (custaRgi.TAB == "16" && custaRgi.ITEM == "3" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab16_item3;
                }

                if (custaRgi.TAB == "16" && custaRgi.ITEM == "4" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab16_item4;
                }

                if (custaRgi.TAB == "16" && custaRgi.ITEM == "5" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab16_item5;
                }

                if (custaRgi.TAB == "16" && custaRgi.ITEM == "6" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab16_item6;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "1")
                {
                    custaRgi.VALOR = tab201_item1_sub1;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "2")
                {
                    custaRgi.VALOR = tab201_item1_sub2;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "3")
                {
                    custaRgi.VALOR = tab201_item1_sub3;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "4")
                {
                    custaRgi.VALOR = tab201_item1_sub4;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "5")
                {
                    custaRgi.VALOR = tab201_item1_sub5;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "6")
                {
                    custaRgi.VALOR = tab201_item1_sub6;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "7")
                {
                    custaRgi.VALOR = tab201_item1_sub7;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "8")
                {
                    custaRgi.VALOR = tab201_item1_sub8;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "1" && custaRgi.SUB == "9")
                {
                    custaRgi.VALOR = tab201_item1_sub9;
                }

                if (custaRgi.TAB == "20.1" && custaRgi.ITEM == "NI" && custaRgi.SUB == "1")
                {
                    custaRgi.VALOR = tab201_itemNI;
                }

                if (custaRgi.TAB == "20.2" && custaRgi.ITEM == "1" && custaRgi.SUB == "1")
                {
                    custaRgi.VALOR = tab202_item1_sub1;
                }

                if (custaRgi.TAB == "20.2" && custaRgi.ITEM == "1" && custaRgi.SUB == "2")
                {
                    custaRgi.VALOR = tab202_item1_sub2;
                }

                if (custaRgi.TAB == "20.2" && custaRgi.ITEM == "1" && custaRgi.SUB == "3")
                {
                    custaRgi.VALOR = tab202_item1_sub3;
                }

                if (custaRgi.TAB == "20.2" && custaRgi.ITEM == "1" && custaRgi.SUB == "4")
                {
                    custaRgi.VALOR = tab202_item1_sub4;
                }

                if (custaRgi.TAB == "20.2" && custaRgi.ITEM == "NI" && custaRgi.SUB == "*")
                {
                    custaRgi.VALOR = tab202_itemNI;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "1")
                {
                    custaRgi.VALOR = tab203_item1_sub1;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "2")
                {
                    custaRgi.VALOR = tab203_item1_sub2;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "3")
                {
                    custaRgi.VALOR = tab203_item1_sub3;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "4")
                {
                    custaRgi.VALOR = tab203_item1_sub4;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "5")
                {
                    custaRgi.VALOR = tab203_item1_sub5;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "6")
                {
                    custaRgi.VALOR = tab203_item1_sub6;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "7")
                {
                    custaRgi.VALOR = tab203_item1_sub7;
                }

                if (custaRgi.TAB == "20.3" && custaRgi.ITEM == "1" && custaRgi.SUB == "8")
                {
                    custaRgi.VALOR = tab203_item1_sub8;
                }

                if (custaRgi.DESCR == "MUTUA")
                {
                    custaRgi.VALOR = mutua;
                }

                if (custaRgi.DESCR == "ACOTERJ")
                {
                    custaRgi.VALOR = acoterj;
                }

                if (custaRgi.DESCR == "INDISPONIBILIDADE")
                {
                    custaRgi.VALOR = indisponibilidade;
                }

                if (custaRgi.DESCR == "PORCENTAGEM ISS")
                {
                    custaRgi.VALOR = porcentagemISS;
                }

                if (custaRgi.DESCR == "PRENOTAÇÃO")
                {
                    custaRgi.VALOR = prenotacao;
                }

                if (custaRgi.DESCR == "RENOVAÇÃO/DEVOLUÇÃO")
                {
                    custaRgi.VALOR = renovacaoDevolucao;
                }

                tabelaCustas.SalvarCustasRgi(custaRgi);
            }

        }

        private void CustasNotas()
        {

            acao = "atualizando notas";

           

            for (int i = 0; i < listaCustasNotas.Count(); i++)
            {
                Thread.Sleep(1);
                worker.ReportProgress(i);


                var custaNotas = new CustasNota();


                custaNotas = listaCustasNotas[i];
                custaNotas.ANO = ano;

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "1")
                {
                    custaNotas.VALOR = tab22_item1_sub1;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "2")
                {
                    custaNotas.VALOR = tab22_item1_sub2;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "3")
                {
                    custaNotas.VALOR = tab22_item1_sub3;
                }            

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "4")
                {
                    custaNotas.VALOR = tab22_item1_sub4;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "5")
                {
                    custaNotas.VALOR = tab22_item1_sub5;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "6")
                {
                    custaNotas.VALOR = tab22_item1_sub6;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "7")
                {
                    custaNotas.VALOR = tab22_item1_sub7;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1" && custaNotas.SUB == "8")
                {
                    custaNotas.VALOR = tab22_item1_sub8;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.1" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item1_1;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.1" && custaNotas.SUB == "A")
                {
                    custaNotas.VALOR = tab22_item1_1_subA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.2" && custaNotas.SUB == "A")
                {
                    custaNotas.VALOR = tab22_item1_2_subA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.2" && custaNotas.SUB == "B")
                {
                    custaNotas.VALOR = tab22_item1_2_subB;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.3" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item1_3;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.4" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item1_4;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "1.4" && custaNotas.SUB == "A")
                {
                    custaNotas.VALOR = tab22_item1_4_subA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "2" && custaNotas.SUB == "A")
                {
                    custaNotas.VALOR = tab22_item2_subA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "2" && custaNotas.SUB == "B")
                {
                    custaNotas.VALOR = tab22_item2_subB;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "2" && custaNotas.SUB == "C")
                {
                    custaNotas.VALOR = tab22_item2_subC;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "2" && custaNotas.SUB == "D")
                {
                    custaNotas.VALOR = tab22_item2_subD;
                }

                if (custaNotas.DESCR == "APOSTILAMENTO DE HAIA")
                {
                    custaNotas.VALOR = apostilamento;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "2.1" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item2_1;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "3" && custaNotas.SUB == "A")
                {
                    custaNotas.VALOR = tab22_item3_subA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "3" && custaNotas.SUB == "B")
                {
                    custaNotas.VALOR = tab22_item3_subB;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "3" && custaNotas.SUB == "C")
                {
                    custaNotas.VALOR = tab22_item3_subC;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "3" && custaNotas.SUB == "D")
                {
                    custaNotas.VALOR = tab22_item3_subD;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "4" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item4;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "5" && custaNotas.SUB == "I-A")
                {
                    custaNotas.VALOR = tab22_item5_subIA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "5" && custaNotas.SUB == "I-B")
                {
                    custaNotas.VALOR = tab22_item5_subIB;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "5" && custaNotas.SUB == "II")
                {
                    custaNotas.VALOR = tab22_item5_subII;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "5" && custaNotas.SUB == "II-A")
                {
                    custaNotas.VALOR = tab22_item5_subIIA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "5" && custaNotas.SUB == "II-B")
                {
                    custaNotas.VALOR = tab22_item5_subIIB;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "6" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item6;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "6" && custaNotas.SUB == "A")
                {
                    custaNotas.VALOR = tab22_item6_subA;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "1")
                {
                    custaNotas.VALOR = tab22_item7_sub1;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "2")
                {
                    custaNotas.VALOR = tab22_item7_sub2;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "3")
                {
                    custaNotas.VALOR = tab22_item7_sub3;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "4")
                {
                    custaNotas.VALOR = tab22_item7_sub4;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "5")
                {
                    custaNotas.VALOR = tab22_item7_sub5;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "6")
                {
                    custaNotas.VALOR = tab22_item7_sub6;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "7")
                {
                    custaNotas.VALOR = tab22_item7_sub7;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "7" && custaNotas.SUB == "8")
                {
                    custaNotas.VALOR = tab22_item7_sub8;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "NI" && custaNotas.SUB == "8")
                {
                    custaNotas.VALOR = tab22_itemNI_sub8;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "NI" && custaNotas.SUB == "13")
                {
                    custaNotas.VALOR = tab22_itemNI_sub13;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "NI" && custaNotas.SUB == "20")
                {
                    custaNotas.VALOR = tab22_itemNI_sub20;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "NI" && custaNotas.SUB == "21")
                {
                    custaNotas.VALOR = tab22_itemNI_sub21;
                }
                
                if (custaNotas.TAB == "16" && custaNotas.ITEM == "1" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab16_item1;
                }

                if (custaNotas.TAB == "16" && custaNotas.ITEM == "2" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab16_item2;
                }

                if (custaNotas.TAB == "16" && custaNotas.ITEM == "3" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab16_item3;
                }

                if (custaNotas.TAB == "16" && custaNotas.ITEM == "4" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab16_item4;
                }

                if (custaNotas.TAB == "16" && custaNotas.ITEM == "5" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab16_item5;
                }

                if (custaNotas.TAB == "16" && custaNotas.ITEM == "6" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab16_item6;
                }

                

                if (custaNotas.DESCR == "MUTUA")
                {
                    custaNotas.VALOR = mutua;
                }

                if (custaNotas.DESCR == "ACOTERJ")
                {
                    custaNotas.VALOR = acoterj;
                }

                if (custaNotas.DESCR == "ESCRITURA SEM VALOR DECLARADO")
                {
                    custaNotas.VALOR = escrituraSemValorDeclarado;
                }

                if (custaNotas.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE (DUT)")
                {
                    custaNotas.VALOR = autenticidadeDUT;
                }

                if (custaNotas.DESCR == "INDISPONIBILIDADE")
                {
                    custaNotas.VALOR = indisponibilidade;
                }

                if (custaNotas.DESCR == "Cópia")
                {
                    custaNotas.VALOR = copia;
                }

                if (custaNotas.TAB == "22" && custaNotas.ITEM == "16" && custaNotas.SUB == "*")
                {
                    custaNotas.VALOR = tab22_item16;
                }

                if (custaNotas.DESCR == "PORCENTAGEM ISS")
                {
                    custaNotas.VALOR = porcentagemISS;
                }

                if (custaNotas.DESCR == "RTD")
                {
                    custaNotas.VALOR = rtd;
                }


                tabelaCustas.SalvarCustasNotas(custaNotas);

            }
        }

    }
}
