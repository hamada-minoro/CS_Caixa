using System;
using System.Collections.Generic;
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
using System.Data;
using CS_Caixa.Models;
using CS_Caixa.Controls;
using FirebirdSql.Data.FirebirdClient;
using System.ComponentModel;
using System.Threading;


namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAguardeProtesto.xaml
    /// </summary>
    public partial class WinAguardeProtesto : Window
    {
        Ato Ato = new Ato();

        WinPrincipal Principal;

        List<Ato> listaTodosAtosApontameno = new List<Ato>();

        List<Ato> listaTodosAtosRetirado = new List<Ato>();

        List<Ato> listaTodosAtosPagamento = new List<Ato>();

        //int ano;

        ClassPortador classPortador = new ClassPortador();
        Portador portador = new Portador();

        List<Portador> listaPortador = new List<Portador>();

        CustasProtesto custas = new CustasProtesto();

        ClassCustasProtesto CustasProtesto = new ClassCustasProtesto();
        List<CustasProtesto> listaCustas = new List<CustasProtesto>();

        List<Ato> _listaRetorno = new List<Ato>();

        string status = string.Empty;

        decimal porcentagemIss = 0;

        string protocolo = string.Empty;

        ClassAto classAto = new ClassAto();
        BackgroundWorker worker;

        public WinAguardeProtesto(List<Ato> listaRetorno, WinPrincipal Principal)
        {
            _listaRetorno = listaRetorno;
            this.Principal = Principal;

            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            progressBar1.Maximum = _listaRetorno.Count;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }




        public string Faixa(double Valor)
        {
            string letra = string.Empty;

            if (Valor <= 50.00)
            {
                letra = "A";
            }
            else if (Valor <= 100.00)
            {
                letra = "B";
            }
            else if (Valor <= 150.00)
            {
                letra = "C";
            }
            else if (Valor <= 200.00)
            {
                letra = "D";
            }
            else if (Valor <= 250.00)
            {
                letra = "E";
            }
            else if (Valor <= 300.00)
            {
                letra = "F";
            }
            else if (Valor <= 350.00)
            {
                letra = "G";
            }
            else if (Valor <= 400.00)
            {
                letra = "H";
            }
            else if (Valor <= 450.00)
            {
                letra = "I";
            }
            else if (Valor <= 500.00)
            {
                letra = "J";
            }
            else if (Valor <= 600.00)
            {
                letra = "K";
            }
            else if (Valor <= 700.00)
            {
                letra = "L";
            }
            else if (Valor <= 800.00)
            {
                letra = "M";
            }
            else if (Valor <= 900.00)
            {
                letra = "N";
            }
            else if (Valor <= 1000.00)
            {
                letra = "O";
            }
            else if (Valor <= 1500.00)
            {
                letra = "P";
            }
            else if (Valor <= 2000.00)
            {
                letra = "Q";
            }
            else if (Valor <= 2500.00)
            {
                letra = "R";
            }
            else if (Valor <= 3000.00)
            {
                letra = "S";
            }
            else if (Valor <= 3500.00)
            {
                letra = "T";
            }
            else if (Valor <= 4000.00)
            {
                letra = "U";
            }
            else if (Valor <= 4500.00)
            {
                letra = "V";
            }
            else if (Valor <= 5000.00)
            {
                letra = "W";
            }
            else if (Valor <= 7500.00)
            {
                letra = "X";
            }
            else if (Valor <= 10000.00)
            {
                letra = "Y";
            }
            else if (Valor > 10000.00)
            {
                letra = "Z";
            }

            return letra;
        }


        private void CalcularValores(string postecipado)
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal iss = 0;
            decimal pmcmv_2 = 0;

            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Siss = "0,00";
            string Spmcmv_2 = "0,00";
            int index;

            if (Principal.TipoAto == "APONTAMENTO")
            {
                try
                {



                    bool result = VerificarPagAntecipado(Ato.Portador);



                    if (Ato.TipoCobranca == "COM COBRANÇA")
                    {
                        if (postecipado == "N")
                        {
                            if ((Ato.Convenio == "N") || (Ato.Convenio == "S" && result == true))
                            {


                                emol = Convert.ToDecimal(Ato.Emolumentos);

                                fetj_20 = emol * 20 / 100;
                                fundperj_5 = emol * 5 / 100;
                                funperj_5 = emol * 5 / 100;
                                funarpen_4 = emol * 4 / 100;
                                iss = emol * porcentagemIss / 100;

                                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                                    pmcmv_2 = Convert.ToDecimal(Ato.Emolumentos * 2) / 100;
                                else
                                    pmcmv_2 = 0;


                                if ((Ato.Convenio == "N") || (Ato.Convenio == "S" && result == true))
                                {
                                    if (Ato.TipoCobranca == "COM COBRANÇA")
                                    {
                                        Semol = Convert.ToString(emol);
                                    }
                                    Sfetj_20 = Convert.ToString(fetj_20);
                                    Sfundperj_5 = Convert.ToString(fundperj_5);
                                    Sfunperj_5 = Convert.ToString(funperj_5);
                                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                                    Siss = Convert.ToString(iss);
                                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                                        Spmcmv_2 = Convert.ToString(pmcmv_2);
                                }
                            }
                        }
                        else
                        {
                            if (status == "RETIRADO")
                            {

                                emol = Convert.ToDecimal(Ato.Emolumentos);

                                fetj_20 = emol * 20 / 100;
                                fundperj_5 = emol * 5 / 100;
                                funperj_5 = emol * 5 / 100;
                                funarpen_4 = emol * 4 / 100;
                                iss = emol * porcentagemIss / 100;

                                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                                    pmcmv_2 = Convert.ToDecimal(Ato.Emolumentos * 2) / 100;
                                else
                                    pmcmv_2 = 0;


                                if ((Ato.Convenio == "N") || (Ato.Convenio == "S" && result == false))
                                {
                                    if (Ato.TipoCobranca == "COM COBRANÇA")
                                    {
                                        Semol = Convert.ToString(emol);
                                    }
                                    Sfetj_20 = Convert.ToString(fetj_20);
                                    Sfundperj_5 = Convert.ToString(fundperj_5);
                                    Sfunperj_5 = Convert.ToString(funperj_5);
                                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                                    Siss = Convert.ToString(iss);
                                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                                        Spmcmv_2 = Convert.ToString(pmcmv_2);
                                }

                            }
                            else
                            {
                                Ato.Emolumentos = 0;
                            }
                        }


                    }
                    else
                        Ato.Emolumentos = 0;

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

                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                    {
                        index = Spmcmv_2.IndexOf(',');
                        Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);
                    }



                    Ato.Fetj = Convert.ToDecimal(Sfetj_20);
                    Ato.Fundperj = Convert.ToDecimal(Sfundperj_5);
                    Ato.Funperj = Convert.ToDecimal(Sfunperj_5);
                    Ato.Funarpen = Convert.ToDecimal(Sfunarpen_4);
                    Ato.Iss = Convert.ToDecimal(Siss);

                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        Ato.Pmcmv = Convert.ToDecimal(Spmcmv_2);

                    Ato.Mutua = Convert.ToDecimal(string.Format("{0:n2}", Ato.Mutua));

                    Ato.Acoterj = Convert.ToDecimal(string.Format("{0:n2}", Ato.Acoterj));

                    if ((Ato.Convenio == "N") || (Ato.Convenio == "S" && result == false))
                        Ato.Total = Ato.Emolumentos + Ato.Fetj + Ato.Fundperj + Ato.Funperj + Ato.Funarpen + Ato.Pmcmv + Ato.Iss + Ato.Mutua + Ato.Acoterj;
                    else
                    {
                        Ato.Emolumentos = 0;
                        Ato.Total = 0;
                        Ato.Mutua = 0;
                        Ato.Acoterj = 0;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            if (Principal.TipoAto == "PAGAMENTO")
            {
                try
                {

                    emol = Convert.ToDecimal(Ato.Emolumentos);

                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    //iss = (100 - porcentagemIss) / 100;
                    //iss = emol / iss - emol;

                    iss = emol * porcentagemIss / 100;

                    pmcmv_2 = Convert.ToDecimal(Ato.Emolumentos * 2) / 100;



                    Semol = Convert.ToString(emol);

                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);



                    if (Semol.Length > 1)
                    {
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



                        Ato.Fetj = Convert.ToDecimal(Sfetj_20);
                        Ato.Fundperj = Convert.ToDecimal(Sfundperj_5);
                        Ato.Funperj = Convert.ToDecimal(Sfunperj_5);
                        Ato.Funarpen = Convert.ToDecimal(Sfunarpen_4);
                        Ato.Iss = Convert.ToDecimal(Siss);
                        Ato.Pmcmv = Convert.ToDecimal(Spmcmv_2);

                        Ato.Mutua = Convert.ToDecimal(string.Format("{0:n2}", Ato.Mutua));

                        Ato.Acoterj = Convert.ToDecimal(string.Format("{0:n2}", Ato.Acoterj));
                    }



                    Ato.Total = Ato.Emolumentos + Ato.Fetj + Ato.Fundperj + Ato.Funperj + Ato.Funarpen + Ato.Pmcmv + Ato.Iss + Ato.Mutua + Ato.Acoterj + Ato.ValorTitulo + Ato.Bancaria;

                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private void SalvarPortador()
        {
            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
            conTotal.Open();
            try
            {


                FbCommand cmdTotal = new FbCommand("Select * from PORTADORES where NOME = '" + Ato.Portador + "'", conTotal);
                cmdTotal.CommandType = CommandType.Text;

                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                DataTable dtTotal = new DataTable();
                dtTotal.Load(drTotal);



                if (dtTotal.Rows.Count > 0)
                {
                    portador.CODIGO = dtTotal.Rows[0]["CODIGO"].ToString();

                    portador.NOME = dtTotal.Rows[0]["NOME"].ToString();

                    portador.TIPO = dtTotal.Rows[0]["TIPO"].ToString();

                    portador.DOCUMENTO = dtTotal.Rows[0]["DOCUMENTO"].ToString();

                    portador.ENDERECO = dtTotal.Rows[0]["ENDERECO"].ToString();

                    portador.BANCO = dtTotal.Rows[0]["BANCO"].ToString();

                    portador.CONVENIO = dtTotal.Rows[0]["CONVENIO"].ToString();

                    classPortador.SalvarPortador(portador);

                    listaPortador.Add(portador);
                }


            }
            catch (Exception)
            {

            }
            finally
            {
                conTotal.Close();
            }

        }

        private void SalvarItensCustas(int idAto)
        {
            ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();

            ItensCustasProtesto item = new ItensCustasProtesto();

            item.Id_Ato = idAto;

            item.Tabela = custas.TAB;

            item.Item = custas.ITEM;

            item.SubItem = custas.SUB;

            item.Quantidade = "1";

            item.Valor = custas.VALOR;

            item.Total = custas.VALOR;

            item.Descricao = custas.DESCR;

            classCustasProtesto.SalvarItensLista(item);



        }



        private bool VerificarPagAntecipado(string portador)
        {
            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
            conTotal.Open();
            try
            {


                FbCommand cmdTotal = new FbCommand("Select * from PORTADORES where NOME = '" + portador + "'", conTotal);
                cmdTotal.CommandType = CommandType.Text;

                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                DataTable dtTotal = new DataTable();
                dtTotal.Load(drTotal);



                if (dtTotal.Rows.Count > 0)
                {


                    if (dtTotal.Rows[0]["PAGAMENTO_ANTECIPADO"].ToString() == "S")
                    {
                        return true;
                    }


                }


                return false;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conTotal.Close();
            }
        }


        private DataTable ObterTituloNumeroProtocolo(int protocolo)
        {
            DataTable dtTotal = new DataTable();
            try
            {
                using (FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {
                    conTotal.Open();
                    using (FbCommand cmdTotal = new FbCommand("Select * from TITULOS where PROTOCOLO = " + protocolo, conTotal))
                    {
                        cmdTotal.CommandType = CommandType.Text;
                        FbDataReader drTotal;
                        drTotal = cmdTotal.ExecuteReader();
                        dtTotal.Load(drTotal);
                    }
                }
                return dtTotal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }


        void ObterListaDeAtosApontamento()
        {

            string postecipado = "N";




            for (int i = 0; i < _listaRetorno.Count; i++)
            {


                DataTable dtTotal = ObterTituloNumeroProtocolo(Convert.ToInt32(_listaRetorno[i].Protocolo));


                status = "APONTAMENTO";

                protocolo = dtTotal.Rows[0]["PROTOCOLO"].ToString();


                Thread.Sleep(1);
                worker.ReportProgress(i + 1);


                DateTime DataProtocolo = Convert.ToDateTime(dtTotal.Rows[0]["DT_PROTOCOLO"]);
                listaCustas = new List<CustasProtesto>();
                listaCustas = CustasProtesto.ListaCustas().Where(p => p.ANO == DataProtocolo.Date.Year).ToList();
                porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS").Select(p => p.VALOR).FirstOrDefault());


                Ato = new Ato();
                Ato.DataPagamento = Convert.ToDateTime(dtTotal.Rows[0]["DT_PROTOCOLO"]);
                Ato.DataAto = Convert.ToDateTime(dtTotal.Rows[0]["DT_PROTOCOLO"]);

                Ato.TipoAto = "APONTAMENTO";


                if (_listaRetorno[i].Checked == true)
                {
                    Ato.TipoPagamento = "CARTÃO CRÉDITO";
                }
                else
                {
                    Ato.TipoPagamento = "DEPÓSITO";
                    
                }

                Ato.Pago = true;

                Ato.IdUsuario = Principal.usuarioLogado.Id_Usuario;

                Ato.Usuario = Principal.usuarioLogado.NomeUsu;

                Ato.Atribuicao = "PROTESTO";

                Ato.Portador = dtTotal.Rows[0]["APRESENTANTE"].ToString();



                listaPortador = classPortador.VerificaExiste(Ato.Portador);


                if (listaPortador.Count == 0)
                {
                    SalvarPortador();
                }

                Ato.DescricaoAto = "I";

                if (dtTotal.Rows[0]["SALDO_TITULO"].ToString() != "")
                    Ato.ValorTitulo = Convert.ToDecimal(dtTotal.Rows[0]["SALDO_TITULO"]);

                Ato.Faixa = Faixa(Convert.ToDouble(Ato.ValorTitulo));

                if (dtTotal.Rows[0]["PROTOCOLO"].ToString() != "")
                    Ato.Protocolo = Convert.ToInt32(dtTotal.Rows[0]["PROTOCOLO"]);

                Ato.ValorAdicionar = 0;

                Ato.ValorDesconto = 0;

                Ato.Escrevente = Principal.usuarioLogado.NomeUsu;

                Ato.Convenio = dtTotal.Rows[0]["CONVENIO"].ToString();

                if (dtTotal.Rows[0]["COBRANCA"].ToString() == "CC")
                    Ato.TipoCobranca = "COM COBRANÇA";
                else
                    Ato.TipoCobranca = "SEM COBRANÇA";

                custas = listaCustas.Where(p => p.SUB == Ato.Faixa).FirstOrDefault();

                Ato.Natureza = "APONTAMENTO" + " - " + custas.DESCR;

                if (Ato.TipoCobranca == "COM COBRANÇA" || Ato.Convenio == "S")
                    Ato.Emolumentos = custas.VALOR;
                else
                    Ato.Emolumentos = 0;

                if (dtTotal.Rows[0]["POSTECIPADO"].ToString() == "P")
                    postecipado = "S";
                else
                    postecipado = "N";

                if (dtTotal.Rows[0]["MUTUA"].ToString() != "")
                    Ato.Mutua = Convert.ToDecimal(dtTotal.Rows[0]["MUTUA"]);

                if (dtTotal.Rows[0]["ACOTERJ"].ToString() != "")
                    Ato.Acoterj = Convert.ToDecimal(dtTotal.Rows[0]["ACOTERJ"]);

                if (dtTotal.Rows[0]["TARIFA_BANCARIA"].ToString() != "")
                    Ato.Bancaria = Convert.ToDecimal(dtTotal.Rows[0]["TARIFA_BANCARIA"]);

                CalcularValores(postecipado);



                

                List<Ato> RemoveAto = new List<Ato>();

                RemoveAto = classAto.ListarAtoProtocolo(Convert.ToInt32(Ato.Protocolo), Principal.TipoAto);

                if (RemoveAto.Count > 0)
                {
                    for (int r = 0; r < RemoveAto.Count; r++)
                    {
                        if (RemoveAto[r].TipoAto == Ato.TipoAto)
                            classAto.ExcluirAto(RemoveAto[r].Id_Ato, "PROTESTO");
                    }
                }

                int id = classAto.SalvarAto(Ato, "novo");

                SalvarItensCustas(id);

                SalvarPago(id,_listaRetorno[i].Checked);

                listaTodosAtosApontameno.Add(Ato);
            }
        }

        private void SalvarPago(int id, bool? cartao)
        {
            var pago = new ValorPago();

            pago.IdAto = id;
            pago.Data = Ato.DataPagamento;
            pago.Dinheiro = 0M;
            pago.Mensalista = 0M;
            pago.Cheque = 0M;
            pago.ChequePre = 0M;
            pago.Boleto = 0M;
            if (cartao == true)
            {
                pago.CartaoCredito = Ato.Total;
                pago.Deposito = 0M;
            }
            else
            {
                pago.Deposito = Ato.Total;
                pago.CartaoCredito = 0M;
            }


            classAto.SalvarValorPago(pago, "novo", "IdAto");

        }

        void ObterListaDeAtosRetirada()
        {
            string postecipado = "N";
            status = "RETIRADO";

            for (int i = 0; i < _listaRetorno.Count; i++)
            {

                DataTable dtTotal = ObterTituloNumeroProtocolo(Convert.ToInt32(_listaRetorno[i].Protocolo));


                protocolo = dtTotal.Rows[0]["PROTOCOLO"].ToString();


                Thread.Sleep(1);
                worker.ReportProgress(i + 1);

                if (dtTotal.Rows[0]["STATUS"].ToString() == "RETIRADO")
                {

                    DateTime DataProtocolo = Convert.ToDateTime(dtTotal.Rows[0]["DT_RETIRADO"]);
                    listaCustas = new List<CustasProtesto>();
                    listaCustas = CustasProtesto.ListaCustas().Where(p => p.ANO == DataProtocolo.Date.Year).ToList();
                    porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS").Select(p => p.VALOR).FirstOrDefault());


                    Ato = new Ato();
                    Ato.DataPagamento = Convert.ToDateTime(dtTotal.Rows[0]["DT_RETIRADO"]);
                    Ato.DataAto = Convert.ToDateTime(dtTotal.Rows[0]["DT_RETIRADO"]);

                    Ato.TipoAto = "APONTAMENTO" + " RETIRADO";
                    
                    if (_listaRetorno[i].Checked == true)
                    {
                        Ato.TipoPagamento = "CARTÃO CRÉDITO";
                    }
                    else
                    {
                        Ato.TipoPagamento = "DEPÓSITO";
                    }

                    Ato.Pago = true;

                    Ato.IdUsuario = Principal.usuarioLogado.Id_Usuario;

                    Ato.Usuario = Principal.usuarioLogado.NomeUsu;

                    Ato.Atribuicao = "PROTESTO";

                    Ato.Portador = dtTotal.Rows[0]["APRESENTANTE"].ToString();



                    listaPortador = classPortador.VerificaExiste(Ato.Portador);


                    if (listaPortador.Count == 0)
                    {
                        SalvarPortador();
                    }

                    Ato.DescricaoAto = "I";

                    if (dtTotal.Rows[0]["SALDO_TITULO"].ToString() != "")
                        Ato.ValorTitulo = Convert.ToDecimal(dtTotal.Rows[0]["SALDO_TITULO"]);

                    Ato.Faixa = Faixa(Convert.ToDouble(Ato.ValorTitulo));

                    if (dtTotal.Rows[0]["PROTOCOLO"].ToString() != "")
                        Ato.Protocolo = Convert.ToInt32(dtTotal.Rows[0]["PROTOCOLO"]);



                    Ato.Escrevente = Principal.usuarioLogado.NomeUsu;

                    Ato.Convenio = dtTotal.Rows[0]["CONVENIO"].ToString();

                    if (dtTotal.Rows[0]["COBRANCA"].ToString() == "CC")
                        Ato.TipoCobranca = "COM COBRANÇA";
                    else
                        Ato.TipoCobranca = "SEM COBRANÇA";

                    custas = listaCustas.Where(p => p.SUB == Ato.Faixa).FirstOrDefault();

                    Ato.Natureza = "RETIRADO" + " - " + custas.DESCR;

                    if (Ato.TipoCobranca == "COM COBRANÇA" || Ato.Convenio == "S")
                        Ato.Emolumentos = custas.VALOR;
                    else
                        Ato.Emolumentos = 0;

                    postecipado = "N";

                    if (dtTotal.Rows[0]["POSTECIPADO"].ToString() == "P")
                        postecipado = "S";


                    if (dtTotal.Rows[0]["MUTUA"].ToString() != "")
                        Ato.Mutua = Convert.ToDecimal(dtTotal.Rows[0]["MUTUA"]);

                    if (dtTotal.Rows[0]["ACOTERJ"].ToString() != "")
                        Ato.Acoterj = Convert.ToDecimal(dtTotal.Rows[0]["ACOTERJ"]);

                    if (dtTotal.Rows[0]["TARIFA_BANCARIA"].ToString() != "")
                        Ato.Bancaria = Convert.ToDecimal(dtTotal.Rows[0]["TARIFA_BANCARIA"]);

                    CalcularValores(postecipado);

                    ClassAto classAto = new ClassAto();

                    List<Ato> RemoveAto = new List<Ato>();

                    RemoveAto = classAto.ListarAtoProtocolo(Convert.ToInt32(Ato.Protocolo), Ato.TipoAto);

                    if (RemoveAto.Count > 0)
                    {
                        for (int r = 0; r < RemoveAto.Count; r++)
                        {
                            if (RemoveAto[r].TipoAto == Ato.TipoAto)
                                classAto.ExcluirAto(RemoveAto[r].Id_Ato, "PROTESTO");
                        }
                    }

                    int id = classAto.SalvarAto(Ato, "novo");

                    SalvarItensCustas(id);

                    SalvarPago(id, _listaRetorno[i].Checked);

                    listaTodosAtosRetirado.Add(Ato);
                }

            }
        }


        void ObterListaDeAtosPago()
        {
            string postecipado = "N";

            status = "PAGO";

            for (int i = 0; i < _listaRetorno.Count; i++)
            {

                DataTable dtTotal = ObterTituloNumeroProtocolo(Convert.ToInt32(_listaRetorno[i].Protocolo));

                protocolo = dtTotal.Rows[0]["PROTOCOLO"].ToString();



                Thread.Sleep(1);
                worker.ReportProgress(i + 1);

                if (dtTotal.Rows[0]["STATUS"].ToString() == "PAGO")
                {

                    DateTime DataProtocolo = Convert.ToDateTime(dtTotal.Rows[0]["DT_PAGAMENTO"]);
                    listaCustas = new List<CustasProtesto>();
                    listaCustas = CustasProtesto.ListaCustas().Where(p => p.ANO == DataProtocolo.Date.Year).ToList();
                    porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS").Select(p => p.VALOR).FirstOrDefault());

                    Ato.DataPagamento = Convert.ToDateTime(dtTotal.Rows[0]["DT_PAGAMENTO"]);
                    Ato.DataAto = Convert.ToDateTime(dtTotal.Rows[0]["DT_PAGAMENTO"]);

                    Ato.TipoAto = "PAGAMENTO";

                    if (_listaRetorno[i].Checked == true)
                    {
                        Ato.TipoPagamento = "CARTÃO CRÉDITO";
                        Ato.Pago = true;
                    }
                    else
                    {
                        Ato.TipoPagamento = "DEPÓSITO";
                        Ato.Pago = true;
                    }

                    Ato.IdUsuario = Principal.usuarioLogado.Id_Usuario;

                    Ato.Usuario = Principal.usuarioLogado.NomeUsu;

                    Ato.Atribuicao = "PROTESTO";

                    Ato.Portador = dtTotal.Rows[0]["APRESENTANTE"].ToString();



                    listaPortador = classPortador.VerificaExiste(Ato.Portador);


                    if (listaPortador.Count == 0)
                    {
                        SalvarPortador();
                    }



                    if (dtTotal.Rows[0]["SALDO_TITULO"].ToString() != "")
                        Ato.ValorTitulo = Convert.ToDecimal(dtTotal.Rows[0]["SALDO_TITULO"]);

                    Ato.Faixa = Faixa(Convert.ToDouble(Ato.ValorTitulo));

                    if (dtTotal.Rows[0]["PROTOCOLO"].ToString() != "")
                    {
                        Ato.Protocolo = Convert.ToInt32(dtTotal.Rows[0]["PROTOCOLO"]);
                        Ato.Recibo = Convert.ToInt32(dtTotal.Rows[0]["PROTOCOLO"]);
                    }

                    Ato.Escrevente = Principal.usuarioLogado.NomeUsu;

                    Ato.Convenio = dtTotal.Rows[0]["CONVENIO"].ToString();

                    if (dtTotal.Rows[0]["COBRANCA"].ToString() == "CC")
                        Ato.TipoCobranca = "COM COBRANÇA";
                    else
                        Ato.TipoCobranca = "SEM COBRANÇA";

                    custas = listaCustas.Where(p => p.SUB == Ato.Faixa).FirstOrDefault();


                    Ato.Natureza = "PAGAMENTO" + " - " + custas.DESCR;


                    if (Ato.TipoCobranca == "COM COBRANÇA" || Ato.Convenio == "S")
                        Ato.Emolumentos = custas.VALOR;
                    else
                        Ato.Emolumentos = 0;

                    postecipado = "N";

                    if (dtTotal.Rows[0]["POSTECIPADO"].ToString() == "P")
                        postecipado = "S";


                    if (dtTotal.Rows[0]["MUTUA"].ToString() != "")
                        Ato.Mutua = Convert.ToDecimal(dtTotal.Rows[0]["MUTUA"]);

                    if (dtTotal.Rows[0]["ACOTERJ"].ToString() != "")
                        Ato.Acoterj = Convert.ToDecimal(dtTotal.Rows[0]["ACOTERJ"]);


                    Ato.Bancaria = 0;

                    CalcularValores(postecipado);


                    if (postecipado == "N")
                    {
                        if (Ato.Convenio == "S")
                            Ato.Total = Ato.Total - Ato.ValorTitulo;
                        else
                            Ato.Total = Ato.Bancaria;
                    }
                    else
                    {
                        Ato.Total = Ato.Total - Ato.ValorTitulo;
                    }

                    ClassAto classAto = new ClassAto();

                    List<Ato> RemoveAto = new List<Ato>();

                    RemoveAto = classAto.ListarAtoProtocolo(Convert.ToInt32(Ato.Protocolo), Ato.TipoAto);

                    if (RemoveAto.Count > 0)
                    {
                        for (int r = 0; r < RemoveAto.Count; r++)
                        {
                            if (RemoveAto[r].TipoAto == Ato.TipoAto)
                                classAto.ExcluirAto(RemoveAto[r].Id_Ato, "PROTESTO");
                        }
                    }

                    int id = classAto.SalvarAto(Ato, "novo");

                    SalvarItensCustas(id);

                    SalvarPago(id, _listaRetorno[i].Checked);

                    listaTodosAtosPagamento.Add(Ato);

                    if (dtTotal.Rows[0]["TARIFA_BANCARIA"].ToString() != "")
                    {
                        Ato.Bancaria = Convert.ToDecimal(dtTotal.Rows[0]["TARIFA_BANCARIA"]);

                        ObterListaDeAtosEmissaoGuia(dtTotal.Rows[0], _listaRetorno[i].Checked);
                    }

                }

            }

        }

        void ObterListaDeAtosEmissaoGuia(DataRow linha, bool? cartao)
        {
            string postecipado = "N";

            protocolo = linha["PROTOCOLO"].ToString();

            if (linha["STATUS"].ToString() == "PAGO")
            {

                DateTime DataProtocolo = Convert.ToDateTime(linha["DT_PAGAMENTO"]);
                listaCustas = new List<CustasProtesto>();
                listaCustas = CustasProtesto.ListaCustas().Where(p => p.ANO == DataProtocolo.Date.Year).ToList();
                porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS").Select(p => p.VALOR).FirstOrDefault());

                Ato.DataPagamento = Convert.ToDateTime(linha["DT_PAGAMENTO"]);
                Ato.DataAto = Convert.ToDateTime(linha["DT_PAGAMENTO"]);

                Ato.TipoAto = "EMISSÃO DE GUIA";

                if (cartao == true)
                {
                    Ato.TipoPagamento = "CARTÃO CRÉDITO";
                    Ato.Pago = false;
                }
                else
                {
                    Ato.TipoPagamento = "DEPÓSITO";
                    Ato.Pago = true;
                }

                Ato.IdUsuario = Principal.usuarioLogado.Id_Usuario;

                Ato.Usuario = Principal.usuarioLogado.NomeUsu;

                Ato.Atribuicao = "PROTESTO";

                Ato.Portador = linha["APRESENTANTE"].ToString();



                listaPortador = classPortador.VerificaExiste(Ato.Portador);


                if (listaPortador.Count == 0)
                {
                    SalvarPortador();
                }



                if (linha["SALDO_TITULO"].ToString() != "")
                    Ato.ValorTitulo = Convert.ToDecimal(linha["SALDO_TITULO"]);

                Ato.Faixa = Faixa(Convert.ToDouble(Ato.ValorTitulo));

                if (linha["PROTOCOLO"].ToString() != "")
                {
                    Ato.Protocolo = Convert.ToInt32(linha["PROTOCOLO"]);
                    Ato.Recibo = Convert.ToInt32(linha["PROTOCOLO"]);
                }

                Ato.Escrevente = Principal.usuarioLogado.NomeUsu;

                Ato.Convenio = linha["CONVENIO"].ToString();

                if (linha["COBRANCA"].ToString() == "CC")
                    Ato.TipoCobranca = "COM COBRANÇA";
                else
                    Ato.TipoCobranca = "SEM COBRANÇA";

                custas = listaCustas.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES").FirstOrDefault();


                Ato.Natureza = custas.DESCR;


                if (Ato.TipoCobranca == "COM COBRANÇA" || Ato.Convenio == "S")
                    Ato.Emolumentos = custas.VALOR;
                else
                    Ato.Emolumentos = 0;

                postecipado = "N";

                if (linha["POSTECIPADO"].ToString() == "P")
                    postecipado = "S";


                if (linha["MUTUA"].ToString() != "")
                    Ato.Mutua = Convert.ToDecimal(linha["MUTUA"]);

                if (linha["ACOTERJ"].ToString() != "")
                    Ato.Acoterj = Convert.ToDecimal(linha["ACOTERJ"]);


                CalcularValores(postecipado);



                Ato.Total = Ato.Total - (Convert.ToDecimal(linha["TARIFA_BANCARIA"]) + Ato.ValorTitulo + Ato.Pmcmv);

                Ato.Bancaria = Convert.ToDecimal(linha["TARIFA_BANCARIA"]) - Ato.Total;

                Ato.Total = Ato.Total + Ato.Bancaria;

                Ato.Pmcmv = 0;

                ClassAto classAto = new ClassAto();

                List<Ato> RemoveAto = new List<Ato>();

                RemoveAto = classAto.ListarAtoProtocolo(Convert.ToInt32(Ato.Protocolo), Ato.TipoAto);

                if (RemoveAto.Count > 0)
                {
                    for (int r = 0; r < RemoveAto.Count; r++)
                    {
                        if (RemoveAto[r].TipoAto == Ato.TipoAto)
                            classAto.ExcluirAto(RemoveAto[r].Id_Ato, "PROTESTO");
                    }
                }

                int id = classAto.SalvarAto(Ato, "novo");

                SalvarItensCustas(id);

                SalvarPago(id, cartao);

                listaTodosAtosPagamento.Add(Ato);

            }



        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                if (Principal.TipoAto == "APONTAMENTO")
                {
                    ObterListaDeAtosApontamento();

                    ObterListaDeAtosRetirada();

                }

                if (Principal.TipoAto == "PAGAMENTO")
                {
                    ObterListaDeAtosPago();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (status == "APONTAMENTO")
            {
                progressBar1.Maximum = _listaRetorno.Count;

                label1.Content = status;

                label2.Content = "Verificando Protocolo: " + protocolo;

                progressBar1.Value = e.ProgressPercentage;
            }

            if (status == "RETIRADO")
            {
                progressBar1.Maximum = _listaRetorno.Count;

                label1.Content = status;

                label2.Content = "Verificando Protocolo: " + protocolo;

                progressBar1.Value = e.ProgressPercentage;
            }

            if (status == "PAGO")
            {
                progressBar1.Maximum = _listaRetorno.Count;

                label1.Content = status;

                label2.Content = "Verificando Protocolo: " + protocolo;

                progressBar1.Value = e.ProgressPercentage;
            }


            progressBar1.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            this.Close();

        }
    }

}
